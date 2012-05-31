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
        //public delegate void completeOrderHandler(object sender);

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
            bindOrderDetails();
            //bindCurrentOrder();
            bindOrderList();

            //m_currentCart = new Cart();
            // 添加事件
            this.StorageReload += new storageReloadEventHandler(OnStorageReload);
        }

        public void initCart()
        {
            if (m_cartDetailList == null) m_cartDetailList = new BindingList<CartDetailRowData>();
            m_cartDetailList.Clear();
            CurrentOrder = null;
        }

        /// <summary>
        /// 在内存中的库存信息重新从数据库加载到内存的时候，产生此事件。
        /// </summary>
        /// <param name="sender"></param>
        public delegate void storageReloadEventHandler(object sender);
        public event storageReloadEventHandler StorageReload;
        
        /// <summary>
        /// 更新内存中的库存信息。
        /// 函数产生更新库存的事件。购物车需要根据此事件更新购物车的状态。
        /// </summary>
        /// <param name="table"></param>
        public void reloadStorage(weitongDataSet1.storageDataTable table)
        {
            m_storageAdapter.Fill(table);
            bindStorageTable(table);
            StorageReload(this);
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
            addStorage2Cart(dataRow.code, 1);
            //if (dataRow.units < 1)
            //{
            //    throw new ZeroStorageException();
            //    // 库存为零则不能加入购物车。
            //    //return;
            //}
            //int discount = 100;
            //if (CartCustomer != null) discount = CartCustomer.Discount;
            //int units = 1;
            //CartDetailRowData item = new CartDetailRowData(dataRow.code, dataRow.description, dataRow.bottle, dataRow.retailprice, discount, units);
            //int i =0;
            //for(;i<m_cartDetailList.Count; i++) // CartDetailRowData data in m_cartDetailList)
            //{
            //    CartDetailRowData data = m_cartDetailList[i];
            //    if (data.Code == item.Code)
            //    {
            //        data.Units = data.Units + 1;
            //        break;
            //    }
            //}
            //if(i == m_cartDetailList.Count) m_cartDetailList.Add(item);
            //CartDetailView.Refresh();
            //// 此时并没有减少实际的库存量，库存量在生成订单的时候会修改。
            //dataRow.units = dataRow.units - 1;
            
        }

        /// <summary>
        /// 增加购物车中指定酒的数量，如果库存不足，则无法增加数量。
        /// 如果购物车中的数量加上增加的数量units(units可能为负，即减去购物车的内容，加入库存)，则无法增加数量。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="units">增加的数量，默认为1。</param>
        public void plusCartWineUnits(string code, int units = 1)
        {
            if (m_cartDetailList == null) return;


            // 减少内存中用于显示的库存信息，主要包括库存表和销售库存显示表。
            //DataTable bindedTable = m_storageInfoGrid.DataSource as DataTable;
            //DataRow[] dtRows = bindedTable.Select("code ='" + code + "'");
            weitongDataSet1.storageRow dataRow = findCodeInStorageInf(code);

            if (dataRow != null)
            {
                int i = 0;
                for (; i < m_cartDetailList.Count; i++)
                {
                    if (m_cartDetailList[i].Code == code) break;
                }

                if (i == m_cartDetailList.Count) return;       // 指定的酒不在购物车中。

                // 
                if ((dataRow.units - units >= 0))
                {
                    if ((m_cartDetailList[i].Units + units > 0))
                    {
                        dataRow.units -= units;
                        m_cartDetailList[i].Units += units;            // 增加购物车中指定酒的数量
                    }
                    else
                    {
                        // 购物车里数量不足
                        throw new ZeroCartException();
                    }
                }
                else
                {
                    // 库存不足
                    throw new ZeroStorageException("库存不足！");
                }
            }
            else
            {
                return;   // 如果指定code的酒不存在，直接退出。
            }
        }

        /// <summary>
        /// 删除购物车中指定行的数据。
        /// 函数首先修改对应的库存信息，然后再删除。
        /// </summary>
        /// <param name="rowIndex"></param>
        public void deleteCartDetailRow(int rowIndex)
        {
            DataGridViewRow row = m_cartDetailGrid.Rows[rowIndex];
            CartDetailRowData data = row.DataBoundItem as CartDetailRowData;
            // 增加内存中用于显示的库存信息，主要包括库存表和销售库存显示表。
            //DataRow[] dtRows = m_dataset.storage.Select("code ='" + data.Code + "'");
            weitongDataSet1.storageRow dataRow = findCodeInStorageInf(data.Code);
            if (dataRow != null)
            {
                dataRow.units += data.Units;
            }

            m_cartDetailList.Remove(data);
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

        
        
        /// <summary>
        /// 生成订单信息(订单和订单详情)，订单状态为未付款(当完成付款后，修改状态为完成)。
        /// 此时订单已经写入数据库，库存也已经冻结。冻结的库存可以在取消订单的时候解冻。
        /// </summary>
        public Order calcCart()
        {
            Order anOrder = null;
            //if (CurrentOrder != null)
            //{
            //    anOrder = CurrentOrder;
            //}
            //else
            //{
              anOrder = Order.NewOrder();
            //}

            anOrder.CustomerID = CartCustomer.ID;
            anOrder.UserID = m_currentUser.ID;
            anOrder.EffectDate = DateTime.Now;
            anOrder.State = OrderState.FOR_PAY;
        
            decimal amount = 0;
            anOrder.emptyDetails();
            foreach (CartDetailRowData item in m_cartDetailList)
            {
                amount += item.Amount;
                anOrder.addDetail(item.Code, item.Units, item.Memberprice, item.Discount);
            }
            
            //anOrder.save();
            
            return anOrder;
            
            //reloadStorageInfo();
            //CurrentOrderID = CurrentOrder.ID;
            //bindCurrentOrder();
        }

        /// <summary>
        /// 完成订单，将订单写入数据库。
        /// 函数修改订单的日期为当前日期，状态为完成，写入收到的数额，设置当前用户为订单用户。
        /// </summary>
        /// <param name="recved"></param>
        public void completeCurrentOrder(decimal recved)
        {
            if (CurrentOrder == null) return;
            //CurrentOrder.CustomerID = CartCustomer.ID;
            CurrentOrder.EffectDate = DateTime.Now;
            CurrentOrder.State = OrderState.COMPLETED;
            
            CurrentOrder.Received = recved;
            CurrentOrder.UserID = m_currentUser.ID;
            CurrentOrder.save(); 
            // 完成订单之后，购物系统应该处于初始化状态，等待新的购物流程开始。
            m_cartDetailList.Clear();
            CurrentOrder = null;
        }

        // 更新除ID之外的所有列
        public void cancelOrder(Order anOrder)
        {
            anOrder.cancelOrder();
            //reloadStorageInfo();
            reloadStorage(DataSet.storage);
            initCart();
            m_orderAdapter.Fill(DataSet.order);
        }

        public void updateOrderList()
        {
            m_orderAdapter.Fill(m_dataset.order);
        }

        public void generateCurrentOrderDetails()
        {
            if (m_currentOrder == null) return;
            List<OrderDetail> details = m_currentOrder.getDetails();
            if (details != null)
            {
                if (m_currentOrderDetailList == null)
                {
                    m_currentOrderDetailList = new BindingList<OrderRowData>();
                }
                m_currentOrderDetailList.Clear();

                foreach (OrderDetail detail in details)
                {
                    OrderRowData data = new OrderRowData();
                    data.Code = detail.Code;
                    weitongDataSet1.storageRow aWineStrge = Storage.findByCode(data.Code);
                    data.Description = aWineStrge.description;
                    data.KnockDownPrice = detail.KnockDownPrice;
                    data.Units = detail.Units;
                    data.FavorablePrice = aWineStrge.retailprice - detail.KnockDownPrice;
                    m_currentOrderDetailList.Add(data);
                }
            }
            else
            {
                if(m_currentOrderDetailList != null) m_currentOrderDetailList.Clear();
            }
        }

        //public decimal getCurrentOrderAmount()
        //{
        //    decimal amount = 0;
        //    foreach (OrderRowData item in m_currentOrderDetailList)
        //    {
        //        amount += item.KnockDownPrice * item.Units;
        //        //favorableAmount += item.FavorablePrice * item.Units;
        //    }
        //    return amount;
        //}

        //public decimal getCurrentOrderFavorableAmount()
        //{
        //    decimal amount = 0;
        //    foreach (OrderRowData item in m_currentOrderDetailList)
        //    {
        //        //amount += item.KnockDownPrice * item.Units;
        //        amount += item.FavorablePrice * item.Units;
        //    }
        //    return amount;
        //}
        //// 设置指定的orderID为当前的订单(订单必须已经存在于数据库中)。
        //// 根据订单信息，分别设定当前的客户信息和订单详细信息。
        //// 返回值：-1表示未找到订单 -2表示未找到指定的客户信息 0表示正常完成。
        //public int setCurrentOrder(int orderID)
        //{
        //    Order curOrder = Order.findByID(orderID);
        //    if (curOrder == null) return -1;
        //    m_orderID = curOrder.ID;
        //    m_currentOrder = curOrder;

        //    Customer curCustomer = Customer.findByID(curOrder.CustomerID);
        //    if (curCustomer == null) return -2;
        //    m_currentCustomer = curCustomer;

        //    List<OrderRowData> detailList = getOrderDetailByOrderID(m_orderID);
        //    // 如果没有订单的详细信息，则正常返回
        //    if (detailList == null) return 0;
        //    if (this.m_currentOrderDetailList == null)
        //    {
        //        m_currentOrderDetailList = new BindingList<OrderRowData>(detailList);
        //    }
        //    else
        //    {
        //        m_currentOrderDetailList.Clear();
        //        foreach (OrderRowData item in detailList)
        //        {
        //            m_currentOrderDetailList.Add(item);
        //        }
        //    }

        //    //
        //    calcAmountAndFavorableAmountProperty();
        //    return 0;
        //}

        public void generateCartDetail()
        {
            if (m_cartDetailList == null) m_cartDetailList = new BindingList<CartDetailRowData>();
            m_cartDetailList.Clear();
            foreach (OrderRowData orderDetail in m_currentOrderDetailList)
            {
                //CartDetailRowData aRow = new CartDetailRowData();
                //aRow.Code = orderDetail.Code;
                //aRow.Description = orderDetail.Description;
                //aRow.Discount = (int)(100*orderDetail.KnockDownPrice / (orderDetail.KnockDownPrice + orderDetail.FavorablePrice)); //CartCustomer.Discount;//
                //aRow.Units = orderDetail.Units;
                //weitongDataSet1.storageRow storageRow = Storage.findByCode(aRow.Code);
                ////CartDetailRowData item = new CartDetailRowData(dataRow.code, dataRow.description, dataRow.bottle, dataRow.retailprice);
                //aRow.Bottle = storageRow.bottle;
                //aRow.Price = storageRow.retailprice;
                //m_cartDetailList.Add(aRow);
                addStorage2Cart(orderDetail.Code, orderDetail.Units);
                CartDetailRowData detail = findCodeInCartDetails(orderDetail.Code);
                detail.Discount = (int)(100 * orderDetail.KnockDownPrice / (orderDetail.KnockDownPrice + orderDetail.FavorablePrice));
            }
        }


        /// <summary>
        /// 更新orderlist列表
        /// </summary>
        public void reloadOrderList()
        {
            m_orderAdapter.Fill(DataSet.order);
        }


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

        /// <summary>
        /// 设置或者获取当前订单的值
        /// 设置订单时，如果值为空，则会清除当前订单的相关信息。
        /// 如果值不为空，则会生成订单的详细列表等操作。
        /// </summary>
        public Order CurrentOrder
        {
            get { return m_currentOrder; }
            set { 
                m_currentOrder = value;
                if (m_currentOrder != null)
                {
                    //generateCurrentOrderDetails();
                    // 设置
                    //CartCustomer = Customer.findByID(m_currentOrder.CustomerID);
                    //List<OrderDetail> details = m_currentOrder.getDetails();
                    //if (details != null)
                    //{
                    //    if (m_currentOrderDetailList == null)
                    //    {
                    //        m_currentOrderDetailList = new BindingList<OrderRowData>();
                    //    }
                    //    m_currentOrderDetailList.Clear();
                        
                    //    foreach (OrderDetail detail in details)
                    //    {
                    //        OrderRowData data = new OrderRowData();
                    //        data.Code = detail.Code;
                    //        weitongDataSet1.storageRow aWineStrge = Storage.findByCode(data.Code);
                    //        data.Description = aWineStrge.description;
                    //        data.KnockDownPrice = detail.KnockDownPrice;
                    //        data.Units = detail.Units;
                    //        data.FavorablePrice = aWineStrge.retailprice - detail.KnockDownPrice;
                    //        m_currentOrderDetailList.Add(data);
                    //    }
                    //    //calcAmountAndFavorableAmountProperty();
                    //}
                    //m_currentOrderDetailList
                    //bindCurrentOrder();
                }
                else
                {
                    // 清空订单的信息。
                    if(m_currentOrderDetailList != null)
                        m_currentOrderDetailList.Clear();
                }
            }
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
           
            if (m_currentOrderDetailList == null)
            {
                m_currentOrderDetailList = new BindingList<OrderRowData>();
            }
            else
            {
                m_currentOrderDetailList.Clear();
            }
           
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

            //calcAmountAndFavorableAmountProperty();
        }

        private void bindOrderDetails()
        {
            this.CurrentOrderView.AutoGenerateColumns = false;
            if (m_currentOrderDetailList == null)
            {
                m_currentOrderDetailList = new BindingList<OrderRowData>();
            }
            else
            {
                m_currentOrderDetailList.Clear();
            }
            m_currentOrderGrid.DataSource = m_currentOrderDetailList;
            // bind column
            this.m_currentOrderGrid.Columns["currentOrderCode"].DataPropertyName = "code";
            this.m_currentOrderGrid.Columns["currentOrderDescription"].DataPropertyName = "description";
            this.m_currentOrderGrid.Columns["currentOrderKnockDownPrice"].DataPropertyName = "KnockDownPrice";
            this.m_currentOrderGrid.Columns["currentOrderFavorablePrice"].DataPropertyName = "FavorablePrice";
            this.m_currentOrderGrid.Columns["currentOrderUnits"].DataPropertyName = "Units";
        }

        private void bindOrderList()
        {
            this.m_orderListGrid.AutoGenerateColumns = false;
            m_orderListGrid.DataSource = m_dataset.order;

            m_orderListGrid.Columns["orderListOrderID"].DataPropertyName = "id";
            m_orderListGrid.Columns["orderListCustomerName"].DataPropertyName = "name";
            m_orderListGrid.Columns["orderListOrderAmount"].DataPropertyName = "amount";
            m_orderListGrid.Columns["orderListOrderReceived"].DataPropertyName = "received";
            m_orderListGrid.Columns["orderListOrderDate"].DataPropertyName = "effectdate";
            m_orderListGrid.Columns["orderListOrderState"].DataPropertyName = "orderstate";
            m_orderListGrid.Columns["orderListOrderComment"].DataPropertyName = "comment";
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


        //private void reloadStorageInfo()
        //{
        //    m_storageAdapter.Fill(DataSet.storage);
        //}
        

        private void assignMember2Customer(Customer aCustomer)
        {
            if (aCustomer == null) return;
            Member member = Member.findByCustomer(aCustomer.ID);
            if(member == null)
                member = Member.NewMember();

            member.CustomerID = aCustomer.ID;
            member.Level = aCustomer.MemberLevel;
            member.save();
            //string insertStr = @"sp_insertmember";
            //MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            //insertCmd.CommandText = insertStr;
            //insertCmd.Connection = m_storageAdapter.Connection;
            //insertCmd.CommandType = CommandType.StoredProcedure;

            //insertCmd.Parameters.Add("?customerid", MySqlDbType.Int32).Value = aCustomer.ID;
            //insertCmd.Parameters.Add("?mem_id", MySqlDbType.VarChar).Direction = ParameterDirection.Output;

            //try
            //{
            //    insertCmd.Connection.Open();
            //    insertCmd.ExecuteNonQuery();
                
            //    string memberid = insertCmd.Parameters["?mem_id"].Value.ToString();
            //}
            //catch (Exception e)
            //{
            //    string str = e.Message;
            //}
            //finally
            //{
            //    insertCmd.Connection.Close();
            //}
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

        ///// <summary>
        ///// 减少内存中的酒的数量
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="units">减少的数量（units为负表示增加）</param>
        //private void necStorgeUnits(string code, int units = 1)
        //{
        //    DataRow[] dtRows = m_dataset.storage.Select("code ='" + code + "'");
        //    if (dtRows.Length > 0)
        //    {
        //        weitongDataSet1.storageRow dataRow = dtRows[0] as weitongDataSet1.storageRow;
        //        if (dataRow.units - units >= 0)
        //        {
        //            dataRow.units -= units;
        //        }
        //        else
        //        {
        //            throw new ZeroStorageException();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 增加购物车中酒的数量
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="units">待增加的酒的数量（如果为负值表示减少）</param>
        //private void plusCartUnits(string code, int units = 1)
        //{
        //    CartDetailRowData data = findCodeInCartDetails(code);
        //    if (data == null) return;
        //    if (data.Units + units > 0)
        //    {
        //        data.Units += units;
        //    }
        //    else
        //    {
        //        throw new ZeroCartException();
        //    }
        //}

        /// <summary>
        /// 在m_cartDetailList中查找code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private CartDetailRowData findCodeInCartDetails(string code)
        {
            if (code == null || code == "") return null;

            int i = 0;
            for (; i < m_cartDetailList.Count; i++)
            {
                if (m_cartDetailList[i].Code == code) break;
            }

            if (i == m_cartDetailList.Count) return null;       // 指定的酒不在购物车中。

            return m_cartDetailList[i];
        }

        private weitongDataSet1.storageRow findCodeInStorageInf(string code)
        {
            DataTable bindedTable = m_storageInfoGrid.DataSource as DataTable;
            DataRow[] dtRows = bindedTable.Select("code ='" + code + "'");
            if (dtRows.Length > 0)
            {
                weitongDataSet1.storageRow dataRow = dtRows[0] as weitongDataSet1.storageRow;
                return dataRow;
            }
            else return null;
        }

        /// <summary>
        /// 将酒添加到购物车，并减少内存中的相应库存。
        /// 加入购物车的酒其数据库中的库存此时并没有减少，只有在生成订单的时候库存才会被冻结（或者减少）。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="units"></param>
        private void addStorage2Cart(string code, int units)
        {
            weitongDataSet1.storageRow dataRow = findCodeInStorageInf(code);
            if (dataRow.units - units < 0)
            {
                throw new ZeroStorageException("库存不足！");
                // 库存为零则不能加入购物车。
                //return;
            }
            int discount = 100;
            if (CartCustomer != null) discount = CartCustomer.Discount;
            //int units = 1;
            CartDetailRowData item = new CartDetailRowData(dataRow.code, dataRow.description, dataRow.bottle, dataRow.retailprice, discount, units);

            if (m_cartDetailList == null)            
                m_cartDetailList = new BindingList<CartDetailRowData>();
            
            int i = 0;
            for (; i < m_cartDetailList.Count; i++) // CartDetailRowData data in m_cartDetailList)
            {
                CartDetailRowData data = m_cartDetailList[i];
                if (data.Code == item.Code)
                {
                    data.Units = data.Units + units;
                    break;
                }
            }
            if (i == m_cartDetailList.Count) m_cartDetailList.Add(item);
            CartDetailView.Refresh();
            // 此时并没有减少实际的库存量，库存量在生成订单的时候会修改。
            dataRow.units = dataRow.units - units;
        }

        /// <summary>
        /// 内存中的库存信息重新加载的响应函数。该函数会清空购物车的内容。
        /// </summary>
        /// <param name="sender"></param>
        private void OnStorageReload(object sender)
        {
            if (m_cartDetailList != null) m_cartDetailList.Clear();
        }

        /// <summary>
        /// 完成订单后的响应函数。该函数重新初始化购物系统（购物车和订单等的状态）
        /// </summary>
        /// <param name="sender"></param>
        private void OnCompleteOrder(object sender)
        {
            initCart();
            m_orderAdapter.Fill(DataSet.order);
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
