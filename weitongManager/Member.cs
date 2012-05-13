using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
      
    class Member
    {
        private int m_id = -1;
        private int m_customer_id;
        private string m_memberid;               // memberid应该是4位的年份加上8位的会员编码。
        private int m_level;
        private DateTime m_reg_date;
        private int m_discount = 100;            // ？？还没确定这个属性要不要。如果要这个属性，
                                                 // 意味着会员的折扣可以不倚靠等级而单独定义，存在会员级别被跳过的风险。
        public int ID
        {
            get { return m_id; }
        }

        public int CustomerID
        {
            get { return m_customer_id; }
            set { m_customer_id = value; }
        }

        public string MemberID
        {
            get { return m_memberid; }
        }

        public int Level
        {
            get { return m_level; }
            set { m_level = value; }
        }

        public DateTime RegDATE
        {
            get { return m_reg_date; }
            set { m_reg_date = value; }
        }

        //public int Discount
        //{
        //    get { return m_discount; }
        //}

        

        public void save()
        {
            // mysql数据库中的id是从1开始计数的。
            if (m_id <= 0)
            {
                this.m_id = insertAndGetID(this);
            }
            else
            {
                update(this);
            }
        }

        // ==========================私有方法=====================
        /// <summary>
        /// 私有化默认构造函数，防止在外部被初始化。
        /// </summary>
        private Member()
        { }


        //============================公有静态方法===========================
        /// <summary>
        /// Member的工程方法，经过工厂出来的member其memberid已经分配完成。
        /// </summary>
        /// <returns>返回的Member其memberid满足特定规则</returns>
        public static Member NewMember()
        {
            Member memb = new Member();
            memb.m_id = -1;
            memb.m_discount = 100;
            memb.m_memberid = MaxMemberID.getNewMemberid();
            memb.m_reg_date = DateTime.Now;
            memb.Level = 1; // 默认1级。
            return memb;
        }

        public static Member findByCustomer(int customerID)
        {
            Member member = null;

            string qStr = @"SELECT id, memberid,customerid,memlevel,registerdate,discount
                            FROM member
                            WHERE customerid=@customer_id";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@customer_id", MySqlDbType.Int32).Value = customerID;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if (reader.Read())
                {
                    member = new Member();
                    member.m_id = reader.GetInt32("id");
                    member.m_memberid = reader.GetString("memberid");
                    member.m_customer_id = reader.GetInt32("customerid");
                    member.m_level = reader.GetInt32("memlevel");
                    member.m_reg_date = reader.GetDateTime("registerdate");
                    member.m_discount = reader.GetInt32("discount");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return member;
        }

        //=========================== 私有静态方法 =======================================

        /// <summary>
        /// 将Member的信息插入数据库，并返回其id.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private static int insertAndGetID(Member member)
        {
            int id = -1;
            if (member == null) return id;
            
            string insStr = @"INSERT INTO member(memberid,customerid,memlevel,registerdate,discount)
                              VALUE(@memberid,@customerid,@level,@regdate,@discount)";
            MySqlCommand insertCmd = new MySqlCommand();
            insertCmd.CommandText = insStr;
            insertCmd.Connection = ConnSingleton.Connection;

            insertCmd.Parameters.Add("@memberid", MySqlDbType.VarChar).Value = member.m_memberid;
            insertCmd.Parameters.Add("@customerid", MySqlDbType.UInt32).Value = member.m_customer_id;
            insertCmd.Parameters.Add("@level", MySqlDbType.UInt32).Value = member.m_level;
            insertCmd.Parameters.Add("@regdate", MySqlDbType.DateTime).Value = member.m_reg_date;
            insertCmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = member.m_discount;

            try
            {
                insertCmd.Connection.Open();
                insertCmd.ExecuteNonQuery();

                string queryIdStr = "SELECT LAST_INSERT_ID()";
                insertCmd.CommandText = queryIdStr;
                MySqlDataReader reader = insertCmd.ExecuteReader();
                
                if (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
            }
            finally
            {
                insertCmd.Connection.Close();
            }

            return id;
        }

        /// <summary>
        /// 将Member的信息更新到数据库，函数要求member必须是数据库中已经存在的。
        /// </summary>
        /// <param name="member"></param>
        private static void update(Member member)
        {
            
            if (member == null) return;
            // 如果member不在数据库中，则返回。
            if (member.m_id <= 0) return;

            string updateStr = @"UPDATE member 
                                 SET memberid=@memberid,customerid=@customerid,memlevel=@level,registerdate=@regdate,discount=@discount
                                 WHERE id=@id";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;

            updateCmd.Parameters.Add("@memberid", MySqlDbType.VarChar).Value = member.m_memberid;
            updateCmd.Parameters.Add("@customerid", MySqlDbType.UInt32).Value = member.m_customer_id;
            updateCmd.Parameters.Add("@level", MySqlDbType.UInt32).Value = member.m_level;
            updateCmd.Parameters.Add("@regdate", MySqlDbType.DateTime).Value = member.m_reg_date;
            updateCmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = member.m_discount;
            updateCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = member.m_id;

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


        private class MaxMemberID
        {
            //static string m_maxID = null;

            /// <summary>
            /// 获取新的会员编号。
            /// 会员编号由四位数的年份加上八位数的数字组成，新的会员编号在现有的编号上增加1.
            /// 如果是新的一年，则重新从1开始计数。
            /// </summary>
            /// <returns></returns>
            public static string getNewMemberid()
            {
                string m_maxID = queryMaxMemberID();

                if (m_maxID == null) m_maxID = DateTime.Now.Year.ToString() + "00000001";

                string idStr = m_maxID.Substring(4, 8);
                string yearStr = m_maxID.Substring(0, 4);

                int year = Int32.Parse(yearStr);
                if (year == DateTime.Now.Year)
                {
                    int id = Int32.Parse(idStr);
                    id += 1;
                    m_maxID = yearStr + id.ToString("D8");
                }
                else
                {
                    // 新的一年，则已新的年份为基础，重新冲1开始编码。
                    m_maxID = DateTime.Now.Year.ToString() + "00000001";
                }

                return m_maxID;
            }

            /// <summary>
            /// 从数据库中查询最大的memberid
            /// </summary>
            /// <returns></returns>
            private static string queryMaxMemberID()
            {
                string max_id = null;

                string qryStr = @"SELECT MAX(memberid) FROM member";

                MySqlCommand qryCmd = new MySqlCommand();
                qryCmd.CommandText = qryStr;
                qryCmd.Connection = ConnSingleton.Connection;

                try
                {
                    qryCmd.Connection.Open();
                    MySqlDataReader reader = qryCmd.ExecuteReader();
                    
                    // MAX函数可能返回NULL值
                    if (reader.Read() && !reader.IsDBNull(0))
                    {
                        max_id = reader.GetString(0);
                    }
                }
                finally
                {
                    qryCmd.Connection.Close();
                }


                return max_id;
            }
        }

    }
}
