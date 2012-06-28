using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    class Customer
    {
        // =================属性==============================
        public int ID
        {
            get { return m_id; }
            //set { m_id = value; }
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

        //
        // ======================== 公有方法 ==============================
        //
        public void save()
        {
            if (m_id < 0)
            {
                this.m_id = insertCustomerAndGetID(this);
            }
            else
            {
                updateCustomer(this);
            }
        }

        //
        //================================= 静态公有方法 ==============================
        //

        public static Customer NewCustomer()
        {
            Customer aCustomer = new Customer();
            aCustomer.m_id = -1;
            return aCustomer;
        }
        public static Customer findByID(int id)
        {
            Customer aCustomer = null;

            string queryStr = @"SELECT   customers.id, customers.name, customers.phonenumber, customers.registedate, customers.sex, customers.job, 
                customers.birthday, customers.address, customers.email, 
                member.memberid, member.memlevel, member.registerdate as memberdate, member.discount
                FROM customers INNER JOIN
                     member ON customers.id = member.customerid
                WHERE customers.id=@id";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.CommandText = queryStr;
            queryCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            queryCmd.Connection = ConnSingleton.Connection;

            try
            {
                queryCmd.Connection.Open();
                MySqlDataReader reader = queryCmd.ExecuteReader();
                if (reader.Read())
                {
                    aCustomer = getCustomerFromReader(reader);
                }
            }
            finally
            {
                queryCmd.Connection.Close();
            }

            return aCustomer;
        }

        // 根据客户名称查找客户信息(暂时不考虑多个客户同名的问题)。
        public static Customer findByName(string name)
        {
            Customer aCustomer = null;
            if (name == null || name == "") return aCustomer;

            string queryStr = @"SELECT   customers.id, customers.name, customers.phonenumber, customers.registedate, customers.sex, customers.job, 
                customers.birthday, customers.address, customers.email, 
                member.memberid, member.memlevel, member.registerdate as memberdate, member.discount
                FROM customers LEFT OUTER JOIN
                     member ON customers.id = member.customerid
                WHERE customers.name like @name";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.CommandText = queryStr;
            queryCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            queryCmd.Connection = ConnSingleton.Connection;//this.m_storageAdapter.Connection;

            try
            {
                queryCmd.Connection.Open();
                MySqlDataReader reader = queryCmd.ExecuteReader();
                if (reader.Read())
                {
                    aCustomer = getCustomerFromReader(reader);
                }

            }
            finally
            {
                queryCmd.Connection.Close();
            }

            return aCustomer;
        }

        public static Customer findByPhNumber(string phoneNumber)
        {
            Customer aCustomer = null;
            if (phoneNumber == null || phoneNumber == "") return aCustomer;

            string queryStr = @"SELECT   customers.id, customers.name, customers.phonenumber, customers.registedate, customers.sex, customers.job, 
                customers.birthday, customers.address, customers.email, 
                member.memberid, member.memlevel, member.registerdate as memberdate, member.discount
                FROM customers LEFT OUTER JOIN
                     member ON customers.id = member.customerid
                WHERE customers.phonenumber = @phonenumber";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.CommandText = queryStr;
            queryCmd.Parameters.Add("@phonenumber", MySqlDbType.VarChar).Value = phoneNumber;
            queryCmd.Connection = ConnSingleton.Connection;//this.m_storageAdapter.Connection;

            try
            {
                queryCmd.Connection.Open();
                MySqlDataReader reader = queryCmd.ExecuteReader();
                if (reader.Read())
                {
                    aCustomer = getCustomerFromReader(reader);
                }

            }
            finally
            {
                queryCmd.Connection.Close();
            }

            return aCustomer;
        }

        /// <summary>
        /// 加载所有的客户信息到列表中
        /// </summary>
        /// <returns></returns>
        public static List<Customer> load()
        {
            List<Customer> customers = null;
            

            string queryStr = @"SELECT   customers.id, customers.name, customers.phonenumber, customers.registedate, customers.sex, customers.job, 
                customers.birthday, customers.address, customers.email, 
                member.memberid, member.memlevel, member.registerdate as memberdate, member.discount
                FROM customers LEFT OUTER JOIN
                     member ON customers.id = member.customerid";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.CommandText = queryStr;
            
            queryCmd.Connection = ConnSingleton.Connection;

            try
            {
                queryCmd.Connection.Open();
                MySqlDataReader reader = queryCmd.ExecuteReader();
                while(reader.Read())
                {
                    if(customers == null) customers = new List<Customer>();
                    Customer aCustomer = getCustomerFromReader(reader);
                    if (aCustomer != null) customers.Add(aCustomer);
                }

            }
            finally
            {
                queryCmd.Connection.Close();
            }

            return customers;
        }

        // ========================= 私有方法=============================
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



        // ======================= 静态私有方法==================================
        // customer
        private static int insertCustomerAndGetID(Customer aCustomer)
        {
            int id = -1;

            string insertStr = @"INSERT INTO customers(name,phonenumber,registedate,sex,job,birthday,address,email) 
                                VALUES(@name,@phonenumber,@registedate,@sex,@job,@birthday,@address,@email)";
            MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = ConnSingleton.Connection;
            insertCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = aCustomer.Name;
            insertCmd.Parameters.Add("@phonenumber", MySqlDbType.VarChar).Value = aCustomer.PhoneNumber;
            insertCmd.Parameters.Add("@registedate", MySqlDbType.DateTime).Value = aCustomer.RegisterDate;
            insertCmd.Parameters.Add("@sex", MySqlDbType.Int32).Value = aCustomer.Sex;
            insertCmd.Parameters.Add("@job", MySqlDbType.VarChar).Value = aCustomer.Job;
            insertCmd.Parameters.Add("@birthday", MySqlDbType.Date).Value = aCustomer.Birthday;
            insertCmd.Parameters.Add("@address", MySqlDbType.VarChar).Value = aCustomer.Address;
            insertCmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = aCustomer.Email;

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
                    id = reader.GetInt32(0);
                }
            }
            finally
            {
                insertCmd.Connection.Close();
            }

            return id;
        }


        private static void updateCustomer(Customer aCustomer)
        {
            if (aCustomer == null) return;
            string updateStr = @"UPDATE customers SET name=@name,phonenumber=@phonenumber,registedate=@registedate,
                                                sex=@sex,job=@job,birthday=@birthday,address=@address,email=@email
                                 WHERE id=@id";
            MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;
            updateCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = aCustomer.Name;
            updateCmd.Parameters.Add("@phonenumber", MySqlDbType.VarChar).Value = aCustomer.PhoneNumber;
            updateCmd.Parameters.Add("@registedate", MySqlDbType.DateTime).Value = aCustomer.RegisterDate;
            updateCmd.Parameters.Add("@sex", MySqlDbType.Int32).Value = aCustomer.Sex;
            updateCmd.Parameters.Add("@job", MySqlDbType.VarChar).Value = aCustomer.Job;
            updateCmd.Parameters.Add("@birthday", MySqlDbType.Date).Value = aCustomer.Birthday;
            updateCmd.Parameters.Add("@address", MySqlDbType.VarChar).Value = aCustomer.Address;
            updateCmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = aCustomer.Email;

            updateCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = aCustomer.ID;

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

        

        private static Customer getCustomerFromReader(MySqlDataReader reader)
        {
            Customer aCustomer = null;
            if (reader == null || reader.IsClosed || !reader.HasRows) return null;

            aCustomer = new Customer();

            aCustomer.m_id = reader.GetInt32("id");
            aCustomer.Name = reader.GetString("name");
            aCustomer.PhoneNumber = reader.GetString("phonenumber");
            aCustomer.Email = reader.GetString("email");
            aCustomer.Address = reader.GetString("address");
            aCustomer.Birthday = reader.GetDateTime("birthday");
            aCustomer.Job = reader.GetString("job");
            aCustomer.RegisterDate = reader.GetDateTime("registedate");
            aCustomer.Sex = reader.GetInt32("sex");

            if (!reader.IsDBNull(9))
            {
                aCustomer.MemberID = reader.GetString("memberid");
            }
            if (!reader.IsDBNull(10))
            {
                aCustomer.MemberLevel = reader.GetInt32("memlevel");
            }
            else
            {
                aCustomer.MemberLevel = 1;
            }
            if (!reader.IsDBNull(11))
            {
                aCustomer.MemberDate = reader.GetDateTime("memberdate");
            }
            

            return aCustomer;
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
