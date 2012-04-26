using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    class Customer
    {
        public int ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string PhoneNumber
        {
            get { return m_phonenumber; }
            set { m_phonenumber = value; }
        }

        public DateTime RegisterDate
        {
            get { return m_registerdate; }
            set { m_registerdate = value; }
        }

        public int Sex
        {
            get { return m_sex; }
            set { m_sex = value; }
        }

        public string Job
        {
            get { return m_job; }
            set { m_job = value; }
        }

        public DateTime Birthday
        {
            get { return m_birthday; }
            set { m_birthday = value; }
        }

        public string Address
        {
            get { return m_address; }
            set { m_address = value; }
        }

        public string Email
        {
            get { return m_email; }
            set { m_email = value; }
        }

        // memberinfo
        public string MemberID
        {
            get { return m_memberID; }
            set { m_memberID = value; }
        }

        public int MemberLevel
        {
            get { return m_memlevel; }
            set { m_memlevel = value; }
        }

        public DateTime MemberDate
        {
            get { return m_memberDate; }
            set { m_memberDate = value; }
        }

        public int Discount
        {
            get
            {
                if (m_discount < 0) m_discount = getLevelDiscount(m_memlevel);
                return m_discount;
            }
        }

        //public MySqlConnection Connection
        //{
        //    get { return m_conn; }
        //    set { m_conn = value; }
        //}

        // private
        private int getLevelDiscount(int level)
        {
            int discount = 100;

            string qryStr = @"SELECT discount
                              FROM memberlevel
                              WHERE memlevel=@level";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.CommandText = qryStr;
            qryCmd.Parameters.Add("@level", MySqlDbType.Int32).Value = level;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if (reader.Read())
                {
                    discount = reader.GetInt32("discount");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return discount;
        }

        // 让m_discount的值无效。
        private void reSetDiscount()
        {
            m_discount = -1;
        }

        private int m_id = -1;
        private string m_name;
        private string m_phonenumber;
        private DateTime m_registerdate;
        private int m_sex;
        private string m_job;
        private DateTime m_birthday;
        private string m_address;
        private string m_email;

        // memberinfo
        //member.memberid, member.memlevel, member.registerdate, member.discount
        private string m_memberID = null;    // 类似“20120001”这样的字符串。
        private int m_memlevel = 1;          // 默认会员等级为一级（会员等级从低到高一次为1,2,3级）
        private DateTime m_memberDate;       // 成为会员的日期，客户默认都为会员，所以应该与客户注册信息一致。
        private int m_discount = -1;         // 应该以memlevel对应的折扣一致。
    }
}
