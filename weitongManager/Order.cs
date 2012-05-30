using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    
    class Order
    {
        /*
         *  Order对象直接涉及3个表的内容，分别是Order表，customer表和order_wines表
         *  分别存放订单的信息，订单的客户信息和订单的详细信息。
         */


        // 属性
        #region public property
        public int ID
        {
            get { return m_id; }
            //set { m_id = value; }
        }

        public int CustomerID
        {
            get { return m_customerID; }
            set { m_customerID = value; }
        }

        public int UserID
        {
            get { return m_userID; }
            set { m_userID = value; }
        }

        public DateTime EffectDate
        {
            get { return m_effectDate; }
            set { m_effectDate = value; }
        }

        public OrderState State
        {
            get { return m_orderState; }
            set { m_orderState = value; }
        }

        //state 1 等待付款，2 完成付款，3 取消订单
        public string StateName
        {
            get {
                return getStateName(m_orderState);
            }
        }

        public decimal Amount
        {
            get
            {
                //if (m_id > 0) return m_amount;
                return calcAmount(); 
            }
            //set { m_amount = value; }
        }

        public decimal Received
        {
            get { return m_received; }
            set { m_received = value; }
        }

        public bool isCompleted
        {
            get { return isCompletedState(); }
        }

        #endregion


        #region public method
        // 公用接口
        /// <summary>
        /// 保存订单信息。
        /// 如果订单是新订单（id为负值），则将订单信息和订单细项插入数据库。
        /// 如果数据库中已经有该订单的信息（id为正值），则更新订单信息。
        /// 对于更新，
        /// </summary>
        public void save()
        {
            if (ID <= 0)
            {
                //this.m_id = insertOrderAndGetID(CustomerID, UserID, EffectDate, (int)State, Amount,Received);
                //insertOrderDetails();
                insert();
            }
            else
            {
                //updateOrder(m_id, CustomerID, UserID, EffectDate, State, Amount, Received);
                //clearDetails(m_id);
                //insertOrderDetails();
                update();
            }
        }

        

        // 取消订单
        public void cancelOrder()
        {
            if (State == OrderState.COMPLETED || State == OrderState.CANCEL) return;
            cancelOrder(ID);
            State = OrderState.CANCEL;
        }

        /// <summary>
        /// 如果订单是新订单，返回内存中的细项。
        /// 如果订单已经存入数据库，返回数据库中的细项，并替换内存中的副本。
        /// </summary>
        /// <returns></returns>
        public List<OrderDetail> getDetails()
        {
            if (ID < 0) return m_details;
            m_details = getOrderDetail(this.ID);
            return m_details;
        }

        /// <summary>
        /// 清空内存中的订单列表。
        /// </summary>
        public void emptyDetails()
        {
            if (m_details == null) return;
            m_details.Clear();
        }

        /// <summary>
        /// 添加订单细项到列表中。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="units"></param>
        /// <param name="memberPrice"></param>
        /// <param name="discount"></param>
        public void addDetail(string code, int units, decimal memberPrice, int discount)
        {
            OrderDetail detail = new OrderDetail();
            detail.ID = -1;
            detail.OrderID = this.m_id;
            detail.Code = code;
            detail.Units = units;
            detail.Discount = discount;
            detail.KnockDownPrice = memberPrice;
            if (m_details == null) m_details = new List<OrderDetail>();
            m_details.Add(detail);
        }

        /// <summary>
        /// 将其他订单信息的内容copy到当前订单。copy不会改变当前订单的id,和状态信息（当前状态必须为FOR_PAY，否则会抛出异常）
        /// </summary>
        /// <param name="other"></param>
        public void copy(Order other)
        {
            if (other == null) return;

            copyOrder(this, other);
        }

        #endregion


        // ==================================== 公用静态方法区 ===================================
        #region public static method(Class method)

        /// <summary>
        /// 从order表中取出订单的信息。
        /// 此时取出的订单并不包含订单细项。
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static Order findByID(int orderID)
        {
            Order newOrder = null;
            string qryStr = @"SELECT customerid,userid,effectdate,orderstate,amount,received
                              FROM orders
                              WHERE id=@id";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.CommandText = qryStr;
            qryCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = orderID;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if (reader.Read())
                {
                    newOrder = new Order();
                    newOrder.m_id = orderID;
                    newOrder.m_amount = reader.GetDecimal("amount");
                    newOrder.CustomerID = reader.GetInt32("customerid");
                    newOrder.UserID = reader.GetInt32("userid");
                    newOrder.EffectDate = reader.GetDateTime("effectdate");
                    newOrder.Received = reader.GetDecimal("received");
                    newOrder.State = (OrderState)reader.GetInt32("orderstate");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return newOrder;
        }

        /// <summary>
        /// 新建订单,并设置订单id为-1.
        /// </summary>
        /// <returns></returns>
        public static Order NewOrder()
        {
            Order anOrder = new Order();
            anOrder.m_id = -1;
            return anOrder;
        }

        /// <summary>
        /// 根据现有的订单生产新的订单。
        /// 新的订单，其id为-1，且其状态为等待付款状态，日期为当前日期，
        /// 其余信息,包括userid和订单细项与现有订单一致
        /// </summary>
        /// <param name="other">现有订单,如果现有订单为null，则简单的新建订单</param>
        /// <returns>新的订单</returns>
        public static Order NewOrder(Order other)
        {
            if (other == null) return NewOrder();

            Order anOrder = new Order();
            anOrder.m_id = -1;
            anOrder.State = OrderState.FOR_PAY;

            copyOrder(anOrder, other);
            //anOrder.CustomerID = other.CustomerID;
            //anOrder.EffectDate = DateTime.Now;
            //anOrder.Received = 0;
            //anOrder.UserID = other.UserID;

            ////
            //anOrder.emptyDetails();
            //if (other.m_details != null)
            //{
            //    foreach (OrderDetail detail in other.m_details)
            //    {
            //        anOrder.addDetail(detail.Code, detail.Units, detail.KnockDownPrice, detail.Discount);
            //    }
            //}

            return anOrder;
        }

        public static string getStateName(OrderState state)
        {
            string stateStr = "未知状态";
            if (state == OrderState.FOR_PAY)
            {
                stateStr = "等待付款";
            }
            else if (state == OrderState.COMPLETED)
            {
                stateStr = "已完成";
            }
            else if (state == OrderState.CANCEL)
            {
                stateStr = "已取消";
            }
            return stateStr;
        }

        #endregion



        // ==========================私有方法=========================

        #region private method
        /// <summary>
        /// 声明私有的构造函数，防止在外部被构造。
        /// </summary>
        private Order()
        { }
        /// <summary>
        /// 将订单的所有细项插入数据库，并修改对应的库存信息。
        /// </summary>
        private void insertOrderDetails()
        {
            if (m_details != null)
            {
                foreach (OrderDetail detail in m_details)
                {
                    insertDetail(detail.Code, detail.Units, detail.KnockDownPrice, detail.Discount);
                }
            }
        }

        /// <summary>
        /// 判断订单是否为完成状态。
        /// 完成状态的订单，必须是数据库中存在的（id为正值）,且状态为完成状态。
        /// </summary>
        /// <returns></returns>
        private bool isCompletedState()
        {
            return (m_id > 0 && State == OrderState.COMPLETED);
        }

        /// <summary>
        /// 根据订单的细项计算订单的总金额。
        /// </summary>
        /// <returns>根据细项计算出的订单金额</returns>
        private decimal calcAmount()
        {
            decimal amount = 0;
            if (m_details == null) return 0;
            foreach (OrderDetail detail in m_details)
            {
                amount += detail.KnockDownPrice * detail.Units;
            }
            return amount;
        }

        /// <summary>
        /// 将订单插入数据库中。
        /// 订单必须是新订单(id小于0)。
        /// 函数保证订单要么完整的插入，要么完全不会插入。
        /// </summary>
        private void insert()
        {
            if (ID > 0) return;    //mysql中的id是正数。

            try
            {
                this.m_id = insertOrderAndGetID(CustomerID, UserID, EffectDate, (int)State, Amount, Received);
                insertOrderDetails();
            }
            catch (ZeroStorageException zsEx)
            {
                //log(zsEx.msg)
                // 如果出现插入细项时库存不足的异常，则撤销对库存的修改。
                clearDetails(this.m_id);
                Order.delete(m_id);
                m_id = -1;
                throw zsEx;
            }
            
        }

        /// <summary>
        /// 将订单更新到数据库。
        /// 订单必须是数据库已存在的，并且其状态只能为For_Pay状态（取消或者完成的订单不能再修改）。
        /// 函数保证更新要么完成，所有细项都被写入，要么完全不会被更新。
        /// 注意：无法通过设置状态为取消来达到撤销订单的目的。
        /// 函数会先回滚所有订单的细项，然后删除已有的细项，再重新插入新的订单的细项。
        /// </summary>
        private void update()
        {
            
            if (this.m_id <= 0) return;

            // temp用于备份数据库中当前订单的信息，如果再更新数据库的过程中出现异常，则恢复数据库的内容。
            Order temp = Order.findByID(this.m_id);
            // 如果数据库中的该订单不是待付款状态，是不允许再更新的。
            if (temp.State != OrderState.FOR_PAY) return;
            temp.m_details = getOrderDetail(this.m_id);

            try
            {
                updateOrder(m_id, CustomerID, UserID, EffectDate, State, Amount, Received);
                clearDetails(m_id);
                insertOrderDetails();
            }
            catch (ZeroStorageException zsEx)
            {
                // 清除已经插入的细项
                clearDetails(m_id);
                updateOrder(temp.m_id, temp.CustomerID, temp.UserID, temp.EffectDate, temp.State, temp.Amount, temp.Received);
                if (temp.m_details != null)
                {
                    foreach (OrderDetail detail in temp.m_details)
                    {
                        insertDetail(detail.Code, detail.Units, detail.KnockDownPrice, detail.Discount);
                    }
                }
                throw zsEx;
            }

        }

        /// <summary>
        /// 修改相应的库存信息，并将订单详细信息保存（插入）到数据库。
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Units"></param>
        /// <param name="memberPrice"></param>
        /// <param name="discount"></param>
        private void insertDetail(string Code, int Units, decimal memberPrice, int discount)
        {
            necStorageUnits(Code, Units);
            insertOrderDetail(ID, Code, Units, memberPrice, discount);
        }

        /// <summary>
        /// 还原数据库中相应的库存信息,并清除订单的所有细项。
        /// </summary>
        /// <param name="order_id">订单编号，必须是数据库中存在的（id为正值)</param>
        private void clearDetails(int order_id)
        {
            if (order_id < 0) return;
            //OrderState state = (OrderState)getOrderState(order_id);
            //if (state != OrderState.FOR_PAY) return;
            List<OrderDetail> details = Order.getOrderDetail(order_id);
            // 订单没有对应的细项，返回。
            if (details == null) return;
            // 还原对应的库存信息。
            foreach (OrderDetail detail in details)
            {
                plusStorageUnits(detail.Code, detail.Units);
            }

            Order.deleteOrderDetails(order_id);

        }

        #endregion


        // ==================================== 私有静态方法区 ===================================
        #region private static methord
        //order 
        private static int insertOrderAndGetID(int customerID, int userID, DateTime effectDate, int orderState, decimal amount, decimal received)
        {
            int orderID = -1;
            string insertStr = @"INSERT INTO orders(customerid,userid,effectdate,orderstate,amount,received) 
                                        VALUES(@customerid,@userid,@effectdate,@orderstate,@amount,@received)";
            MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = ConnSingleton.Connection;
            insertCmd.Parameters.Add("@userid", MySqlDbType.Int32).Value = userID;
            insertCmd.Parameters.Add("@customerid", MySqlDbType.Int32).Value = customerID;
            insertCmd.Parameters.Add("@effectdate", MySqlDbType.DateTime).Value = effectDate;
            insertCmd.Parameters.Add("@orderstate", MySqlDbType.Int32).Value = orderState;
            insertCmd.Parameters.Add("@amount", MySqlDbType.Decimal).Value = amount;
            insertCmd.Parameters.Add("@received", MySqlDbType.Decimal).Value = received;

            try
            {
                insertCmd.Connection.Open();
                insertCmd.ExecuteNonQuery();
                string queryIdStr = "SELECT LAST_INSERT_ID()";
                insertCmd.CommandText = queryIdStr;
                MySqlDataReader reader = insertCmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    orderID = reader.GetInt32(0);
                }
            }
            finally
            {
                insertCmd.Connection.Close();
            }

            return orderID;
        }

        // state 1 等待付款，2 完成付款，3 取消订单
        private static void updateOrder(int orderID, int customerID, int userID, DateTime effectDate, OrderState state, decimal amount, decimal recved)
        {
            string updateStr = @"UPDATE orders SET customerid = @customerid, userid=@userid, effectdate = @effectdate, 
                                                   orderstate = @orderstate, amount = @amount, received = @recved
                                 WHERE id=@id";
            MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;

            updateCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = orderID;
            updateCmd.Parameters.Add("@customerid", MySqlDbType.Int32).Value = customerID;
            updateCmd.Parameters.Add("@userid", MySqlDbType.Int32).Value = userID;
            updateCmd.Parameters.Add("@effectdate", MySqlDbType.DateTime).Value = effectDate;
            updateCmd.Parameters.Add("@orderstate", MySqlDbType.Int32).Value = (int)state;
            updateCmd.Parameters.Add("@amount", MySqlDbType.Decimal).Value = amount;
            updateCmd.Parameters.Add("@recved", MySqlDbType.Decimal).Value = recved;
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

        private static void insertOrderDetail(int orderID, string code, int units, decimal memberPrice,int discount)
        {
            string insertStr = @"INSERT INTO order_wines(orderid,code,units,knockdownprice,discount) 
                                VALUES(@orderid,@code,@units,@knockdownprice,@discount)";
            MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = ConnSingleton.Connection;

            MySqlParameter para_orderID = new MySqlParameter("@orderid", MySqlDbType.Int32);
            MySqlParameter para_code = new MySqlParameter("@code", MySqlDbType.VarChar);
            MySqlParameter para_units = new MySqlParameter("@units", MySqlDbType.Int32);
            MySqlParameter para_price = new MySqlParameter("@knockdownprice", MySqlDbType.Decimal);
            MySqlParameter para_discount = new MySqlParameter("@discount", MySqlDbType.Int32);

            insertCmd.Parameters.Add(para_orderID);
            insertCmd.Parameters.Add(para_code);
            insertCmd.Parameters.Add(para_units);
            insertCmd.Parameters.Add(para_price);
            insertCmd.Parameters.Add(para_discount);

            try
            {
                insertCmd.Connection.Open();
                
                para_orderID.Value = orderID;
                para_code.Value = code;
                para_units.Value = units;
                para_price.Value = memberPrice;
                para_discount.Value = discount;
                insertCmd.ExecuteNonQuery();
            }
            finally
            {
                insertCmd.Connection.Close();
            }
        }


        /// <summary>
        /// 从数据库中查找订单的细项
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>包含细项的列表或者null</returns>
        private static List<OrderDetail> getOrderDetail(int orderID)
        {
            List<OrderDetail> list = null;
            string qryStr = @"SELECT id, orderid, code, units,knockdownprice,discount 
                              FROM order_wines WHERE orderid=@orderid";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@orderid", MySqlDbType.Int32).Value = orderID;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                while (reader.Read())
                {
                    if (list == null) list = new List<OrderDetail>();
                    OrderDetail detail = new OrderDetail();
                    detail.ID = reader.GetInt32("id");
                    detail.OrderID = reader.GetInt32("orderid");
                    detail.Code = reader.GetString("code");
                    detail.KnockDownPrice = reader.GetDecimal("knockdownprice");
                    detail.Units = reader.GetInt32("units");
                    detail.Discount = reader.GetInt32("discount");
                    list.Add(detail);
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return list;
        }

        /// <summary>
        /// 删除数据库中订单的所有细项。
        /// </summary>
        /// <param name="order_id">订单编号，必须为正，即订单必须在数据库中存在。</param>
        private static void deleteOrderDetails(int order_id)
        {
            if (order_id < 0) return;
            string dStr = @"DELETE FROM order_wines WHERE orderid=@orderid";
            MySqlCommand delCmd = new MySqlCommand();
            delCmd.CommandText = dStr;
            delCmd.Connection = ConnSingleton.Connection;
            delCmd.Parameters.Add("@orderid", MySqlDbType.Int32).Value = order_id;

            try
            {
                delCmd.Connection.Open();
                delCmd.ExecuteNonQuery();
            }
            finally
            {
                delCmd.Connection.Close();
            }
        }
        
        
        /// <summary>
        /// 取消订单。
        /// 函数需要做以下两件事情
        /// * 1. 找到订单的详细记录，增加相应的酒的库存。
        /// * 2. 修改订单的状态为取消（对应的状态是3）。已完成的订单无法撤销。
        /// </summary>
        /// <param name="orderID"></param>
        private static void cancelOrder(int orderID)
        {
            MySqlConnection conn = ConnSingleton.Connection;
            OrderState state = (OrderState)getOrderState(orderID);
            if (state != OrderState.FOR_PAY) return;

            try
            {
                conn.Open();
                string qryStr = @"SELECT code, units FROM order_wines WHERE orderid=@orderid";
                MySqlCommand qryCmd = new MySqlCommand();
                qryCmd.Connection = conn;
                qryCmd.CommandText = qryStr;
                qryCmd.Parameters.Add("@orderid", MySqlDbType.Int32).Value = orderID;

                MySqlDataReader reader = qryCmd.ExecuteReader();
                Dictionary<string,int> dtailList = new Dictionary<string,int>();
                while (reader.Read())
                {
                    string code = reader.GetString("code");
                    int units = reader.GetInt32("units");
                    if (dtailList.ContainsKey(code))
                    {
                        int oldUnits = dtailList[code];
                        dtailList[code] = oldUnits + units;
                    }
                    else
                    {
                        dtailList.Add(code, units);
                    }
                    //plusStorageUnits(conn, code, units);
                }
                reader.Close();
                foreach(KeyValuePair<string,int> pair in dtailList )
                {
                    plusStorageUnits(pair.Key, pair.Value);
                }

                updateOrderState(orderID, OrderState.CANCEL);
            }
            //catch (Exception ex)
            //{
            //}
            finally
            {
                conn.Close();
            }
        }

        private static int getOrderState(int orderID)
        {
            int orderSate = -1;
            //if (conn == null || conn.State != System.Data.ConnectionState.Open)
            //    return -1;

            string qryStr = @"SELECT orderstate FROM orders WHERE id=@id";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.CommandText = qryStr;
            qryCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = orderID;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();

                if (reader.Read())
                {
                    orderSate = reader.GetInt32("orderstate");
                }
                else
                {
                    orderSate = -1;
                }
                reader.Close();
            }
            //catch (Exception ex)
            //{ }
            finally
            {
                qryCmd.Connection.Close();
            }
            
            return orderSate;
        }

        private static void updateOrderState(int orderid, OrderState state)
        {
            string updateStr = @"UPDATE orders SET orderstate = @state WHERE id = @orderid";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;
            updateCmd.Parameters.Add("@state", MySqlDbType.Int32).Value = (int)state;
            updateCmd.Parameters.Add("@orderid", MySqlDbType.Int32).Value = orderid;

            try
            {
                updateCmd.Connection.Open();
                updateCmd.ExecuteNonQuery();
            }
            //catch (Exception ex)
            //{ }
            finally
            {
                updateCmd.Connection.Close();
            }
        }

        // 增加酒的库存（如果为负数则为减少）。
        /// <summary>
        /// 修改指定酒的库存数量。如果修改后库存数量为负，则抛出ZeroStorageException异常。
        /// 如果指定的酒不存在，则抛出InvalidWineCodeException.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="plus"></param>
        private static void plusStorageUnits(string code, int plus = 1)
        {
            string updateStr = @"UPDATE storage SET units = units + @plus WHERE code = @code";
            string qUnitsStr = @"SELECT units FROM storage WHERE code = @code";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;
            updateCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;
            updateCmd.Parameters.Add("@plus", MySqlDbType.Int32).Value = plus;

            try
            {
                updateCmd.Connection.Open();
                updateCmd.CommandText = qUnitsStr;
                MySqlDataReader reader = updateCmd.ExecuteReader();
                if (reader.Read())
                {
                    int units = reader.GetInt32("units");
                    reader.Close();
                    if (units + plus >= 0)
                    {
                        updateCmd.CommandText = updateStr;
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new ZeroStorageException("库存不足");
                    }
                }
                else
                {
                    throw new InvalidWineCodeException();
                }
            }
            //catch (Exception ex)
            //{
            //}
            finally
            {
                updateCmd.Connection.Close();
            }
        }

        private static void necStorageUnits(string code, int nec = 1)
        {
            plusStorageUnits(code, -nec);
        }

        /// <summary>
        /// 将other订单的信息copy到one订单。拷贝函数不会改变订单的id,状态等信息。
        /// 拷贝的one订单的状态必须是FOR_PAY(完成和取消的订单意味着不可修改)。
        /// 
        /// </summary>
        /// <param name="one"></param>
        /// <param name="other"></param>
        private static void copyOrder(Order one, Order other)
        {
            if (one == null || other == null) return;
            if (one.State != OrderState.FOR_PAY) 
                throw new Exception("不能修改已完成或取消的订单！");

            one.CustomerID = other.CustomerID;
            one.EffectDate = DateTime.Now;
            one.Received = 0;
            one.UserID = other.UserID;

            //
            one.emptyDetails();
            List<OrderDetail> other_details = other.getDetails();
            if (other_details != null)
            {
                foreach (OrderDetail detail in other_details)
                {
                    one.addDetail(detail.Code, detail.Units, detail.KnockDownPrice, detail.Discount);
                }
            }
        }

        /// <summary>
        /// 从order表中删除指定id的order信息。
        /// order的id必须是已经存在数据库中的。
        /// 调用此函数之前请确保order的细项已经删除，所有可能依赖order的信息都已经不存在，否则可能会抛出异常。
        /// </summary>
        /// <param name="id"></param>
        private static void delete(int id)
        {
            if (id < 0) return;
            //@"UPDATE orders SET orderstate = @state WHERE id = @orderid";
            string delStr = @"DELETE FROM orders WHERE id=@order_id";
            MySqlCommand delCmd = new MySqlCommand();
            delCmd.CommandText = delStr;
            delCmd.Connection = ConnSingleton.Connection;
            delCmd.Parameters.Add("@order_id", MySqlDbType.Int32).Value = id;

            try
            {
                delCmd.Connection.Open();
                delCmd.ExecuteNonQuery();
            }
            finally
            {
                delCmd.Connection.Close();
            }
        }

        #endregion


        private int m_id = -1;
        private int m_customerID = -1;
        private int m_userID = -1;
        private DateTime m_effectDate;
        private OrderState m_orderState;
        private decimal m_amount;
        private decimal m_received;
        private List<OrderDetail> m_details = null;
    }


    class OrderDetail
    {
        private int m_id;
        private int m_order_id;
        private string m_code;
        private int m_units;
        private decimal m_knock_price;
        private int discount;

        public int ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public int OrderID
        {
            get { return m_order_id; }
            set { m_order_id = value; }
        }

        public string Code
        {
            get { return m_code; }
            set { m_code = value; }
        }

        public int Units
        {
            get { return m_units; }
            set { m_units = value; }
        }

        public decimal KnockDownPrice
        {
            get { return m_knock_price; }
            set { m_knock_price = value; }
        }

        public int Discount
        {
            get { return discount; }
            set { discount = value; }
        }
    }

    enum OrderState
    {
        FOR_PAY = 1,
        COMPLETED,
        CANCEL
    }
}
