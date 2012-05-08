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
            get { return m_amount; }
            set { m_amount = value; }
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


        // 公用接口
        public void save()
        {
            if (ID == -1)
            {
                this.m_id = insertOrderAndGetID(CustomerID, EffectDate, (int)State, Amount);
            }
            else
            {
                updateOrder(m_id, CustomerID, EffectDate, State, Amount, Received);
            }
        }

        public void saveDetail2DB(string Code, int Units, decimal memberPrice)
        {
            insertOrderDetail(ID, Code, Units, memberPrice);
            necStorageUnits(Code, Units);
        }

        // 取消订单
        public void cancelOrder()
        {
            if (State == OrderState.COMPLETED || State == OrderState.CANCEL) return;
            cancelOrder(ID);
            State = OrderState.CANCEL;
        }

        // 
        public List<OrderDetail> getDetails()
        {
            return getOrderDetail(this.ID);
        }




        //
        // 
        //
        private bool isCompletedState()
        {
            return (m_id != -1 && State == OrderState.COMPLETED);
        }




        // ==================================== 公用静态方法区 ===================================

        public static Order findByID(int orderID)
        {
            Order newOrder = null;
            string qryStr = @"SELECT customerid,effectdate,orderstate,amount,received
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
                    newOrder.Amount = reader.GetDecimal("amount");
                    newOrder.CustomerID = reader.GetInt32("customerid");
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

        public static Order NewOrder()
        {
            Order anOrder = new Order();
            anOrder.m_id = -1;
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


        // ==================================== 私有静态方法区 ===================================

        //order 
        private static int insertOrderAndGetID(int customerID, DateTime effectDate, int orderState, decimal amount)
        {
            int orderID = -1;
            string insertStr = @"INSERT INTO orders(customerid,effectdate,orderstate,amount) 
                                        VALUES(@customerid,@effectdate,@orderstate,@amount)";
            MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = ConnSingleton.Connection;
            insertCmd.Parameters.Add("@customerid", MySqlDbType.Int32).Value = customerID;
            insertCmd.Parameters.Add("@effectdate", MySqlDbType.DateTime).Value = effectDate;
            insertCmd.Parameters.Add("@orderstate", MySqlDbType.Int32).Value = orderState;
            insertCmd.Parameters.Add("@amount", MySqlDbType.Decimal).Value = amount;

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
        private static void updateOrder(int orderID, int customerID, DateTime effectDate, OrderState state, decimal amount, decimal recved)
        {
            string updateStr = @"UPDATE orders SET customerid = @customerid, effectdate = @effectdate, 
                                                   orderstate = @orderstate, amount = @amount, received = @recved
                                 WHERE id=@id";
            MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;

            updateCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = orderID;
            updateCmd.Parameters.Add("@customerid", MySqlDbType.Int32).Value = customerID;
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

        private static void insertOrderDetail(int orderID, string code, int units, decimal memberPrice)
        {
            string insertStr = @"INSERT INTO order_wines(orderid,code,units,knockdownprice) 
                                VALUES(@orderid,@code,@units,@knockdownprice)";
            MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = ConnSingleton.Connection;

            MySqlParameter para_orderID = new MySqlParameter("@orderid", MySqlDbType.Int32);
            MySqlParameter para_code = new MySqlParameter("@code", MySqlDbType.VarChar);
            MySqlParameter para_units = new MySqlParameter("@units", MySqlDbType.Int32);
            MySqlParameter para_price = new MySqlParameter("@knockdownprice", MySqlDbType.Decimal);

            insertCmd.Parameters.Add(para_orderID);
            insertCmd.Parameters.Add(para_code);
            insertCmd.Parameters.Add(para_units);
            insertCmd.Parameters.Add(para_price);

            try
            {
                insertCmd.Connection.Open();
                
                para_orderID.Value = orderID;
                para_code.Value = code;
                para_units.Value = units;
                para_price.Value = memberPrice;
                insertCmd.ExecuteNonQuery();
            }
            finally
            {
                insertCmd.Connection.Close();
            }
        }


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
        
        // 取消订单。
        /* 
         * 函数需要做以下两件事情
         * 1. 找到订单的详细记录，增加相应的酒的库存。
         * 2. 修改订单的状态为取消（对应的状态是3）。已完成的订单无法撤销。
         * 
         */
        private static void cancelOrder(int orderID)
        {
            MySqlConnection conn = ConnSingleton.Connection;
            try
            {
                conn.Open();
                OrderState state = (OrderState)getOrderState(orderID);
                if (state != OrderState.FOR_PAY) return;
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
        private static void plusStorageUnits(string code, int plus = 1)
        {
            string updateStr = @"UPDATE storage SET units = units + @plus WHERE code = @code";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;
            updateCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;
            updateCmd.Parameters.Add("@plus", MySqlDbType.Int32).Value = plus;

            try
            {
                updateCmd.Connection.Open();
                updateCmd.ExecuteNonQuery();
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


        private int m_id = -1;
        private int m_customerID = -1;
        private DateTime m_effectDate;
        private OrderState m_orderState;
        private decimal m_amount;
        private decimal m_received;
    }

    enum OrderState
    {
        FOR_PAY = 1,
        COMPLETED,
        CANCEL
    }
}
