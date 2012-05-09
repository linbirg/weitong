using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using System.ComponentModel;

namespace weitongManager
{
    class SalesMgr
    {
        public void init()
        {
            // 初始化，完成绑定和填充数据等准备工作。
            try
            {
                this.m_storageAdapter.Fill(m_dataset.storage);
                this.m_orderAdapter.Fill(m_dataset.order);
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            bindStorageInfoData();
            setOffColumn();

            m_cartDetailList = new BindingList<CartDetailRowData>();
            bindCartDetailData();
            bindCurrentOrder();
            bindOrderList();

            //m_currentCart = new Cart();
        }

        public void updateTable(weitongDataSet1.storageDataTable table)
        {
            m_storageAdapter.Fill(table);
            bindStorageTable(table);
        }

        public void bindStorageTable(weitongDataSet1.storageDataTable table)
        {
            m_storageInfoGrid.DataSource = table;
            setOffColumn();
        }

        public void listStorageInfo()
        {
            m_storageInfoGrid.DataSource = m_dataset.storage;
            setOffColumn();
        }

        public void addCurrentStorage2Cart()
        {
            DataRowView dataRowView = this.m_storageInfoGrid.CurrentRow.DataBoundItem as DataRowView;
            weitongDataSet1.storageRow dataRow = dataRowView.Row as weitongDataSet1.storageRow;
            if (dataRow.units < 1)
            {
                //throw ShortStorageException
                // 库存为0则不能加入购物车。
                return;
            }
            int discount = 100;
            if (CartCustomer != null) discount = CartCustomer.Discount;
            int units = 1;
            CartDetailRowData item = new CartDetailRowData(dataRow.code, dataRow.description, dataRow.bottle, dataRow.retailprice, discount, units);
            int i =0;
            for(;i<m_cartDetailList.Count; i++) // CartDetailRowData data in m_cartDetailList)
            {
                CartDetailRowData data = m_cartDetailList[i];
                if (data.Code == item.Code)
                {
                    data.Units = data.Units + 1;
                    break;
                }
            }
            if(i == m_cartDetailList.Count) m_cartDetailList.Add(item);
            CartDetailView.Refresh();
            // 此时并没有减少实际的库存量，库存量在生成订单的时候会修改。
            dataRow.units = dataRow.units - 1;
            
        }

        // 增加购物车中指定酒的数量
        public void plusCartWineUnits(string code, int units = 1)
        {
            if (m_cartDetailList == null) return;
            int i = 0;
            for (; i < m_cartDetailList.Count; i++)
            {
                if (m_cartDetailList[i].Code == code) break;
            }

            if (i == m_cartDetailList.Count) return;       // 指定的酒不在购物车中。
            m_cartDetailList[i].Units += units;            // 增加购物车中指定酒的数量

            // 减少内存中用于显示的库存信息，主要包括库存表和销售库存显示表。
            DataRow[] dtRows = m_dataset.storage.Select("code ='"+ code + "'");
            if (dtRows.Length > 0)
            {
                weitongDataSet1.storageRow dataRow = dtRows[0] as weitongDataSet1.storageRow;
                dataRow.units -= units;
            }

            // 更新显示界面
            m_cartDetailGrid.Refresh();
        }

        // 将客户信息存入数据库
        public void addCustomer2DB(Customer customer)
        {
            //customer.ID = insertCustomerAndGetID(customer);
            if (customer == null) return;
            customer.save();
            assignMember2Customer(customer);
        }

        
        // 生成订单信息(订单和订单详情)，订单状态为未付款(当完成付款后，修改状态为完成)。
        // 再更新相关的信息。
        public void calcCart()
        {
            Order anOrder = Order.NewOrder();

            anOrder.CustomerID = CartCustomer.ID;
            anOrder.UserID = m_currentUser.ID;
            anOrder.EffectDate = DateTime.Now;
            anOrder.State = OrderState.FOR_PAY;
        
            decimal amount = 0;

            foreach (CartDetailRowData item in m_cartDetailList)
            {
                amount += item.Amount;
            }
            anOrder.Amount = amount;
            anOrder.save();

            if (anOrder.ID == -1) return;
            
            CurrentOrder = anOrder;
            foreach (CartDetailRowData item in m_cartDetailList)
            {
                CurrentOrder.saveDetail2DB(item.Code, item.Units, item.Memberprice,item.Discount);
            }

            
            reloadStorageInfo();
            CurrentOrderID = CurrentOrder.ID;
            bindCurrentOrder();
        }

        public void completeCurrentOrder(decimal recved)
        {
            CurrentOrder.CustomerID = CartCustomer.ID;
            CurrentOrder.EffectDate = DateTime.Now;
            CurrentOrder.State = OrderState.COMPLETED;
            CurrentOrder.Amount = CurrentOrderAmount;
            CurrentOrder.Received = recved;
            CurrentOrder.save();
            
        }

        // 更新除ID之外的所有列
        public void cancelOrder(Order anOrder)
        {
            anOrder.cancelOrder();
            reloadStorageInfo();
            
        }

        public void updateOrderList()
        {
            m_orderAdapter.Fill(m_dataset.order);
        }

        // 设置指定的orderID为当前的订单(订单必须已经存在于数据库中)。
        // 根据订单信息，分别设定当前的客户信息和订单详细信息。
        // 返回值：-1表示未找到订单 -2表示未找到指定的客户信息 0表示正常完成。
        public int setCurrentOrder(int orderID)
        {
            Order curOrder = Order.findByID(orderID);
            if (curOrder == null) return -1;
            m_orderID = curOrder.ID;
            m_currentOrder = curOrder;

            Customer curCustomer = Customer.findByID(curOrder.CustomerID);
            if (curCustomer == null) return -2;
            m_currentCustomer = curCustomer;

            List<OrderRowData> detailList = getOrderDetailByOrderID(m_orderID);
            // 如果没有订单的详细信息，则正常返回
            if (detailList == null) return 0;
            if (this.m_currentOrderDetailList == null)
            {
                m_currentOrderDetailList = new BindingList<OrderRowData>(detailList);
            }
            else
            {
                m_currentOrderDetailList.Clear();
                foreach (OrderRowData item in detailList)
                {
                    m_currentOrderDetailList.Add(item);
                }
            }

            //
            calcAmountAndFavorableAmountProperty();
            return 0;
        }

        public void generateCartDetail()
        {
            if (m_cartDetailList == null) m_cartDetailList = new BindingList<CartDetailRowData>();
            m_cartDetailList.Clear();
            foreach (OrderRowData order in m_currentOrderDetailList)
            {
                CartDetailRowData aRow = new CartDetailRowData();
                aRow.Code = order.Code;
                aRow.Description = order.Description;
                aRow.Discount = CartCustomer.Discount;//
                aRow.Units = order.Units;
                weitongDataSet1.storageRow storageRow = findStorageByCode(aRow.Code);
                //CartDetailRowData item = new CartDetailRowData(dataRow.code, dataRow.description, dataRow.bottle, dataRow.retailprice);
                aRow.Bottle = storageRow.bottle;
                aRow.Price = storageRow.retailprice;
                m_cartDetailList.Add(aRow);
            }
        }


        // utities
        // 根据编码查找库存信息
        // 查找完整的库存信息
        public weitongDataSet1.storageRow findStorageByCode(string code)
        {
            string qStr = @"SELECT storage.id, storage.code, storage.price, storage.retailprice, storage.units, wines.chateau, 
                            wines.country, wines.appellation, wines.quality, wines.vintage, wines.description, wines.bottle, wines.score, 
                            supplier.name
                            FROM storage INNER JOIN
                            wines ON storage.code = wines.code LEFT OUTER JOIN
                            supplier ON storage.supplierid = supplier.id
                            WHERE storage.code = @code";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = this.m_storageAdapter.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;

            // 先保存原查询命令，再执行完本查询后，立即回复原有的查询命令
            MySqlCommand temp = m_storageAdapter.Adapter.SelectCommand;
            m_storageAdapter.Adapter.SelectCommand = queryCmd;

            weitongDataSet1.storageDataTable table = new weitongDataSet1.storageDataTable();
            m_storageAdapter.Adapter.Fill(table);
            m_storageAdapter.Adapter.SelectCommand = temp;
            
            if (table != null && table.Rows.Count != 0)
            {
                // 如果查询结果不为空，应该只有一条记录
                //DataTable table2 = table.Clone();
                return table[0]; //as weitongDataSet1.storageRow;
            }
            else
            {
                return null;
            }
        }

//        // 根据客户名称查找客户信息(暂时不考虑多个客户同名的问题)。
//        public Customer findCustomerByName(string name)
//        {
//            Customer aCustomer = null;

//            string queryStr = @"SELECT   customers.id, customers.name, customers.phonenumber, customers.registedate, customers.sex, customers.job, 
//                customers.birthday, customers.address, customers.email, 
//                member.memberid, member.memlevel, member.registerdate as memberdate, member.discount
//                FROM customers INNER JOIN
//                     member ON customers.id = member.customerid
//                WHERE customers.name like @name";
//            MySqlCommand queryCmd = new MySql.Data.MySqlClient.MySqlCommand();
//            queryCmd.CommandText = queryStr;
//            queryCmd.Parameters.Add("@name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = name;
//            queryCmd.Connection = ConnSingleton.Connection;//this.m_storageAdapter.Connection;

//            try
//            {
//                queryCmd.Connection.Open();
//                MySqlDataReader reader = queryCmd.ExecuteReader();
//                reader.Read();
//                aCustomer = getCustomerFromReader(reader); 
                
//            }
//            finally
//            {
//                queryCmd.Connection.Close();
//            }

//            return aCustomer;
//        }



        //================================= 属性 =================================
        public DataGridView StorgeInfoView
        {
            get
            {
                return m_storageInfoGrid;
            }
            set
            {
                m_storageInfoGrid = value;
            }
        }

        public weitongDataSet1 DataSet
        {
            get { return m_dataset; }
            set { m_dataset = value; }
        }

        public weitongDataSet1TableAdapters.storageTableAdapter StorageAdapter
        {
            get { return m_storageAdapter; }
            set { m_storageAdapter = value; }
        }

        public weitongDataSet1TableAdapters.orderTableAdapter OrderAdapter
        {
            get { return m_orderAdapter; }
            set { m_orderAdapter = value; }
        }

        public DataGridView CartDetailView
        {
            get
            {
                return m_cartDetailGrid;
            }
            set
            {
                m_cartDetailGrid = value;
            }
        }

        public DataGridView CurrentOrderView
        {
            get
            {
                return m_currentOrderGrid;
            }
            set
            {
                m_currentOrderGrid = value;
            }
        }

        public Customer CartCustomer
        {
            get { return m_currentCustomer; }
            set 
            { 
                m_currentCustomer = value;
                updateCartMemberDiscout(m_currentCustomer);
            }
        }

        public User CurrentUser
        {
            get { return m_currentUser; }
            set { m_currentUser = value; }
        }

        public int CurrentOrderID
        {
            get { return m_orderID; }
            set { m_orderID = value; }
        }

        public Order CurrentOrder
        {
            get { return m_currentOrder; }
            set { m_currentOrder = value; }
        }

        public decimal CurrentOrderAmount
        {
            get { return m_currentOrderAmount; }
        }

        public decimal CurrentOrderFavorableAmount
        {
            get { return m_currentOrderFavorableAmount; }
        }

        public DataGridView OrderListView
        {
            get { return m_orderListGrid; }
            set { m_orderListGrid = value; }
        }



        //================================= 私有方法 =================================
       
        // binding
        private void bindStorageInfoData()
        {
            this.m_storageInfoGrid.AutoGenerateColumns = false;
            m_storageInfoGrid.DataSource = m_dataset.storage;

            // bind column
            m_storageInfoGrid.Columns["codeDGVStorageInfoTextBoxColumn"].DataPropertyName = "code";
            m_storageInfoGrid.Columns["chateauDGVStorageInfoTextBoxColumn"].DataPropertyName = "chateau";
            m_storageInfoGrid.Columns["vintageDGVStorageInfoTextBoxColumn"].DataPropertyName = "vintage";
            m_storageInfoGrid.Columns["appelltionDGVStorageInfoTextBoxColumn"].DataPropertyName = "appellation";
            m_storageInfoGrid.Columns["qualityDGVStorageInfoTextBoxColumn"].DataPropertyName = "quality";
            m_storageInfoGrid.Columns["bottleDGVStorageInfoTextBoxColumn"].DataPropertyName = "bottle";
            m_storageInfoGrid.Columns["scoreDGVStorageInfoTextBoxColumn"].DataPropertyName = "score";
           
            m_storageInfoGrid.Columns["unitsDGVStorageInfoTextBoxColumn"].DataPropertyName = "units";
            m_storageInfoGrid.Columns["descriptionDGVStorageInfoTextBoxColumn"].DataPropertyName = "description";
            m_storageInfoGrid.Columns["retailpriceDGVStorageInfoTextBoxColumn"].DataPropertyName = "retailprice";
            m_storageInfoGrid.Columns["countryDGVStorageInfoTextBoxColumn"].DataPropertyName = "country";
            //m_storageInfoGrid.Columns["offDGVStorageInfoTextBoxColumn"].DataPropertyName = "country";
        }

        // 绑定订单详细信息到列表
        private void bindCurrentOrder()
        {
            this.CurrentOrderView.AutoGenerateColumns = false;
            m_currentOrderDetailList = new BindingList<OrderRowData>();
            m_currentOrderGrid.DataSource = m_currentOrderDetailList;
            foreach (CartDetailRowData item in m_cartDetailList)
            {
                OrderRowData aRow = new OrderRowData();
                aRow.Code = item.Code;
                aRow.Description = item.Description;
                aRow.FavorablePrice = item.Price - item.Memberprice;
                aRow.KnockDownPrice = item.Memberprice;
                aRow.Units = item.Units;
                m_currentOrderDetailList.Add(aRow);
            }

            // bind column
            this.m_currentOrderGrid.Columns["currentOrderCode"].DataPropertyName = "code";
            this.m_currentOrderGrid.Columns["currentOrderDescription"].DataPropertyName = "description";
            this.m_currentOrderGrid.Columns["currentOrderKnockDownPrice"].DataPropertyName = "KnockDownPrice";
            this.m_currentOrderGrid.Columns["currentOrderFavorablePrice"].DataPropertyName = "FavorablePrice";
            this.m_currentOrderGrid.Columns["currentOrderUnits"].DataPropertyName = "Units";

            calcAmountAndFavorableAmountProperty();
        }

        private void bindOrderList()
        {
            this.m_orderListGrid.AutoGenerateColumns = false;
            m_orderListGrid.DataSource = m_dataset.order;

            m_orderListGrid.Columns["orderListOrderID"].DataPropertyName = "id";
            m_orderListGrid.Columns["orderListCustomerName"].DataPropertyName = "name";
            m_orderListGrid.Columns["orderListOrderAmount"].DataPropertyName = "received";
            m_orderListGrid.Columns["orderListOrderDate"].DataPropertyName = "effectdate";
            m_orderListGrid.Columns["orderListOrderState"].DataPropertyName = "orderstate";
        }

        private void bindCartDetailData()
        {
            this.CartDetailView.AutoGenerateColumns = false;
            CartDetailView.DataSource = m_cartDetailList;

            CartDetailView.Columns["codeDGVOrderDetailTextBoxColumn"].DataPropertyName = "Code";
            CartDetailView.Columns["descriptionDGVOrderDetailTextBoxColumn"].DataPropertyName = "description";
            CartDetailView.Columns["bottleDGVOrderDetailTextBoxColumn"].DataPropertyName = "Bottle";
            CartDetailView.Columns["priceDGVOrderDetailTextBoxColumn"].DataPropertyName = "Price";
            CartDetailView.Columns["memberpriceDGVOrderDetailTextBoxColumn"].DataPropertyName = "memberprice";
            CartDetailView.Columns["discountDGVOrderDetailTextBoxColumn"].DataPropertyName = "Discount";
            CartDetailView.Columns["unitsDGVOrderDetailTextBoxColumn"].DataPropertyName = "Units";
            CartDetailView.Columns["amountGridViewTextBoxColumn"].DataPropertyName = "Amount";
        }

        // utilty
        // offDGVStorageInfoTextBoxColumn列用于显示打折促销信息。
        // 可以通过设置最低等级的会员（比普通会员还低一级）的折扣来实行折扣促销，这种促销是全品促销。
        private void setOffColumn()
        {
            foreach(DataGridViewRow aRow in m_storageInfoGrid.Rows)
            {
                aRow.Cells["offDGVStorageInfoTextBoxColumn"].Value = "100";//Customer.GetCommestOff();
            }
        }

        private void calcAmountAndFavorableAmountProperty()
        {
            decimal amount = 0;
            decimal favorableAmount = 0;
            if (m_currentOrderDetailList == null) return;
            foreach (OrderRowData item in m_currentOrderDetailList)
            {
                amount += item.KnockDownPrice * item.Units;
                favorableAmount += item.FavorablePrice * item.Units;
            }

            m_currentOrderAmount = amount;
            m_currentOrderFavorableAmount = favorableAmount;
        }


        // storage


        // 将库存中相应酒的库存减少（如果为负数则为增加）。
        private void necStorageUnits(string code, int nec = 1)
        {
            string updateStr = @"UPDATE storage SET units = units - @nec WHERE code = @code";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;
            updateCmd.Parameters.Add("@nec", MySqlDbType.Int32).Value = nec;

            updateCmd.Connection = ConnSingleton.Connection;

            try
            {
                updateCmd.Connection.Open();
                updateCmd.ExecuteNonQuery();
            }
            finally
            {
                updateCmd.Connection.Close();
            }
        }

        private void reloadStorageInfo()
        {
            try
            {
                m_storageAdapter.Fill(DataSet.storage);
            }
            catch (Exception e)
            {
                string str = e.Message;
            }
        }
        

        private void assignMember2Customer(Customer aCustomer)
        {
            string insertStr = @"sp_insertmember";
            MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = m_storageAdapter.Connection;
            insertCmd.CommandType = CommandType.StoredProcedure;

            insertCmd.Parameters.Add("?customerid", MySqlDbType.Int32).Value = aCustomer.ID;
            insertCmd.Parameters.Add("?mem_id", MySqlDbType.VarChar).Direction = ParameterDirection.Output;

            try
            {
                insertCmd.Connection.Open();
                insertCmd.ExecuteNonQuery();
                
                string memberid = insertCmd.Parameters["?mem_id"].Value.ToString();
            }
            catch (Exception e)
            {
                string str = e.Message;
            }
            finally
            {
                insertCmd.Connection.Close();
            }
        }

        // 根据客户的折扣信息更新购物车中的折扣信息。
        private void updateCartMemberDiscout(Customer aCustomer)
        {
            if (m_cartDetailList != null)
            {
                foreach(CartDetailRowData item in m_cartDetailList)
                {
                    item.Discount = aCustomer.Discount;
                }
            }
            m_cartDetailGrid.Refresh();
        }

        private List<OrderRowData> getOrderDetailByOrderID(int orderID)
        {
            List<OrderRowData> detailList = null;
            string qryStr = @"SELECT order_wines.id, order_wines.orderid, order_wines.code, order_wines.units, order_wines.knockdownprice, 
                                    wines.description, storage.retailprice
                              FROM order_wines INNER JOIN
                                   wines ON order_wines.code = wines.code INNER JOIN
                                   storage ON wines.code = storage.code
                              WHERE order_wines.orderid=@orderid";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.Connection = m_storageAdapter.Connection;
            qryCmd.CommandText = qryStr;
            qryCmd.Parameters.Add("@orderid", MySqlDbType.Int32).Value = orderID;
            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                while (reader.Read())
                {
                    if (detailList == null) detailList = new List<OrderRowData>();
                    OrderRowData aRow = new OrderRowData();
                    
                    aRow.Code = reader.GetString("code");
                    aRow.Description = reader.GetString("description");
                    decimal retailPrice = reader.GetDecimal("retailprice");
                    
                    aRow.KnockDownPrice = reader.GetDecimal("knockdownprice");
                    aRow.FavorablePrice = retailPrice - aRow.KnockDownPrice;
                    aRow.Units = reader.GetInt32("units");

                    detailList.Add(aRow);
                }
            }
            catch (Exception e)
            {
                string str = e.Message;
            }
            finally
            {
                qryCmd.Connection.Close();
            }
            return detailList;
        }



        private DataGridView m_storageInfoGrid = null;
        private weitongDataSet1 m_dataset = null;
        private weitongDataSet1TableAdapters.storageTableAdapter m_storageAdapter = null;

        private DataGridView m_cartDetailGrid = null;
        private BindingList<CartDetailRowData> m_cartDetailList = null;

        private DataGridView m_currentOrderGrid = null;
        private BindingList<OrderRowData> m_currentOrderDetailList = null;

        private Customer m_currentCustomer = null;
        private int m_orderID = -1;
        private Order m_currentOrder = null;

        private decimal m_currentOrderAmount = 0;
        private decimal m_currentOrderFavorableAmount = 0;

        private DataGridView m_orderListGrid = null;
        private weitongDataSet1TableAdapters.orderTableAdapter m_orderAdapter = null;

        // 当前购物车
        private Cart m_currentCart = null;
        private User m_currentUser = null;
    }
}
