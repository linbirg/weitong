using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace weitongManager
{
    class User
    {
        private int m_id;
        private string m_name;
        private string m_hashedpassword;
        private string m_salt;
        private string m_alias = null;
        private string m_email;
        private DateTime m_regDate;
        private int m_roleid;

        public int ID
        {
            get { return m_id; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Alias
        {
            get { return m_alias; }
            set { m_alias = value; }
        }

        public string Email
        {
            get { return m_email; }
            set { m_email = value; }
        }

        public DateTime RegisterDate
        {
            get { return m_regDate; }
            set { m_regDate = value; }
        }

        //public string Salt
        //{
        //    get { return m_salt; }
        //}

        public int RoleID
        {
            get { return m_roleid; }
            set { m_roleid = value; }
        }

        public int MinDiscount
        {
            get 
            {
                Role rl = Role.findByID(m_roleid);
                if (rl != null) return rl.Discount;
                return 100;
            }
        }

        

        //======================公有方法==========================
        public bool isAdministrator()
        {
            Role role = Role.findByID(this.m_roleid);
            return role.Name == "administrator";
        }

        /// <summary>
        /// 设置用户的密码属性(密码属性为只写)。
        /// </summary>
        public void setPasswd(string passwd)
        {
            m_hashedpassword = encrypt_passwd(passwd, m_salt);
        }

        public void delete()
        {
            deleteUser(this.m_id);
        }

        /// <summary>
        /// 保存User信息到数据库。如果用户信息是全新的，则插入数据库；如果用户信息已经存入数据库，则更新相应信息。
        /// </summary>
        public void save()
        {
            if (m_id < 0)
            {
                this.m_id = insertAndGetID(this);
            }
            else
            {
                updateUser(this);
            }
        }


        //=====================私有方法==========================
        /// <summary>
        /// 声明私有的构造函数
        /// </summary>
        private User() { }

        //====================================公有静态方法=============================

        /// <summary>
        /// 新建用户。新建用户id为负值,注册日期为当前值，salt为随即10为字符串。
        /// </summary>
        /// <returns></returns>
        public static User NewUser()
        {
            User aUser = new User();
            aUser.m_id = -1;
            aUser.m_regDate = DateTime.Now;
            aUser.m_salt = util.GetRandomString(10);
            return aUser;
        }
        
        /// <summary>
        /// 从数据库中加载所有的用户信息
        /// </summary>
        /// <returns></returns>
        public static List<User> loadData()
        {
            List<User> list = null;
            string qryStr = @"SELECT id, user_name,passwd,salt,alias_name,email,reg_date,role_id
                               FROM users";

            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                while (reader.Read())
                {
                    if (list == null) list = new List<User>();
                    User aUser = new User();
                    aUser.m_id = reader.GetInt32("id");
                    aUser.Name = reader.GetString("user_name");
                    aUser.m_hashedpassword = reader.GetString("passwd");
                    aUser.m_salt = reader.IsDBNull(3) ? "" : reader.GetString("salt");
                    aUser.m_alias = reader.IsDBNull(4) ? "" : reader.GetString("alias_name");
                    aUser.Email = reader.IsDBNull(5) ? "" : reader.GetString("email");
                    if (!reader.IsDBNull(6)) aUser.m_regDate = reader.GetDateTime("reg_date");
                    aUser.m_roleid = reader.GetInt32("role_id");

                    list.Add(aUser);
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }
            return list;
        }

        /// <summary>
        /// 验证用户名密码是否正确
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public static bool authenticate(string name,string passwd)
        {
            User aUser = find_by_name(name);
            if (aUser == null) return false;
            string hashed_passwd = encrypt_passwd(passwd, aUser.m_salt);
            if (hashed_passwd == aUser.m_hashedpassword) return true;
            return false;
        }

        public static User find_by_name(string name)
        {
            User aUser = null;
            string qryStr = @"SELECT id, user_name,passwd,salt,alias_name,email,reg_date,role_id
                               FROM users
                               WHERE user_name=@name";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    aUser = new User();
                    aUser.m_id = reader.GetInt32("id");
                    aUser.Name = reader.GetString("user_name");
                    aUser.m_hashedpassword = reader.GetString("passwd");
                    aUser.m_salt = reader.IsDBNull(3) ? "" : reader.GetString("salt");
                    aUser.m_alias = reader.IsDBNull(4) ? "" : reader.GetString("alias_name");
                    aUser.Email = reader.IsDBNull(5) ? "" : reader.GetString("email");
                    if (!reader.IsDBNull(6)) aUser.m_regDate = reader.GetDateTime("reg_date");
                    aUser.m_roleid = reader.GetInt32("role_id");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return aUser;
        }

        public static User find_by_id(int id)
        {
            User aUser = null;
            string qryStr = @"SELECT id, user_name,passwd,salt,alias_name,email,reg_date,role_id
                               FROM users
                               WHERE id=@id";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    aUser = new User();
                    aUser.m_id = reader.GetInt32("id");
                    aUser.Name = reader.GetString("user_name");
                    aUser.m_hashedpassword = reader.GetString("passwd");
                    aUser.m_salt = reader.IsDBNull(3) ? "" : reader.GetString("salt");
                    aUser.m_alias = reader.IsDBNull(4) ? "" : reader.GetString("alias_name");
                    aUser.Email = reader.IsDBNull(5) ? "" : reader.GetString("email");
                    if (!reader.IsDBNull(6)) aUser.m_regDate = reader.GetDateTime("reg_date");
                    aUser.m_roleid = reader.GetInt32("role_id");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return aUser;
        }


        //====================================私有静态方法=============================
        
        /// <summary>
        /// 根据密码和随机字串生成加密的密码
        /// </summary>
        /// <param name="passwd">密码</param>
        /// <param name="salt">随机字符串</param>
        /// <returns>已经加密的密码</returns>
        private static string encrypt_passwd(string passwd, string salt)
        {
            string str_sha_in = passwd + salt;
            byte[] bytes_sha_in = UTF8Encoding.Default.GetBytes(str_sha_in);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] bytes_sha_out = sha.ComputeHash(bytes_sha_in);
            //string str_sha_out = UTF8Encoding.Default.GetString(bytes_sha_out); //BitConverter.ToString(bytes_sha_out);
            
            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in bytes_sha_out)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }
            return EnText.ToString();
        }

        private static int insertAndGetID(User aNewUser)
        {
            int id = -1;
            if (aNewUser == null||aNewUser.m_id >= 0) return id;
            string insStr = @"INSERT INTO users(user_name,passwd,salt,alias_name,email,reg_date,role_id)
                              VALUES(@user_name,@hashed_passwd,@salt,@alias,@email,@reg_date,@role_id)";

            MySqlCommand insrtCmd = new MySqlCommand();
            insrtCmd.CommandText = insStr;
            insrtCmd.Connection = ConnSingleton.Connection;

            insrtCmd.Parameters.Add("@user_name", MySqlDbType.VarChar).Value = aNewUser.Name;
            insrtCmd.Parameters.Add("@hashed_passwd", MySqlDbType.VarChar).Value = aNewUser.m_hashedpassword;
            insrtCmd.Parameters.Add("@salt", MySqlDbType.VarChar).Value = aNewUser.m_salt;
            insrtCmd.Parameters.Add("@alias", MySqlDbType.VarChar).Value = aNewUser.m_alias;
            insrtCmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = aNewUser.m_email;
            insrtCmd.Parameters.Add("@reg_date", MySqlDbType.DateTime).Value = aNewUser.m_regDate;
            insrtCmd.Parameters.Add("@role_id", MySqlDbType.Int32).Value = aNewUser.m_roleid;

            try
            {
                insrtCmd.Connection.Open();
                insrtCmd.ExecuteNonQuery();

                string queryIdStr = "SELECT LAST_INSERT_ID()";
                insrtCmd.CommandText = queryIdStr;
                MySqlDataReader reader = insrtCmd.ExecuteReader();

                if (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
            }
            finally
            {
                insrtCmd.Connection.Close();
            }
            
            return id;
        }

        private static void updateUser(User aUser)
        {
            if (aUser == null || aUser.m_id < 0) return;

            string insStr = @"UPDATE users SET user_name=@user_name,passwd=@hashed_passwd,salt=@salt,alias_name=@alias,
                                                email=@email,reg_date=@reg_date,role_id=@role_id
                              WHERE id=@id";

            MySqlCommand insrtCmd = new MySqlCommand();
            insrtCmd.CommandText = insStr;
            insrtCmd.Connection = ConnSingleton.Connection;

            insrtCmd.Parameters.Add("@user_name", MySqlDbType.VarChar).Value = aUser.Name;
            insrtCmd.Parameters.Add("@hashed_passwd", MySqlDbType.VarChar).Value = aUser.m_hashedpassword;
            insrtCmd.Parameters.Add("@salt", MySqlDbType.VarChar).Value = aUser.m_salt;
            insrtCmd.Parameters.Add("@alias", MySqlDbType.VarChar).Value = aUser.m_alias;
            insrtCmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = aUser.m_email;
            insrtCmd.Parameters.Add("@reg_date", MySqlDbType.DateTime).Value = aUser.m_regDate;
            insrtCmd.Parameters.Add("@role_id", MySqlDbType.Int32).Value = aUser.m_roleid;
            insrtCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = aUser.m_id;

            try
            {
                insrtCmd.Connection.Open();
                insrtCmd.ExecuteNonQuery();
            }
            finally
            {
                insrtCmd.Connection.Close();
            }
        }

        private void deleteUser(int id)
        {
            if (id < 0) return;
            string delStr = @"DELETE FROM users WHERE id=@id";

            MySqlCommand delCmd = new MySqlCommand();
            delCmd.CommandText = delStr;
            delCmd.Connection = ConnSingleton.Connection;
            delCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

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
    }
}
