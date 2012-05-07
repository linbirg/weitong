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
        private string m_name;
        private string m_hashedpassword;
        private string m_salt;
        private string m_email;
        private DateTime m_regDate;
        private int m_roleid;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
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

        public bool isAdministrator()
        {
            Role role = Role.findByID(this.m_roleid);
            return role.Name == "administrator";
        }

        //====================================公有静态方法=============================
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
            string qryStr = @"SELECT user_name,passwd,salt,email,reg_date,role_id
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
                    aUser.Name = reader.GetString(0);
                    aUser.m_hashedpassword = reader.GetString(1);
                    aUser.m_salt = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    aUser.Email = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    if (!reader.IsDBNull(4)) aUser.m_regDate = reader.GetDateTime(4);
                    aUser.m_roleid = reader.GetInt32(5);
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return aUser;
        }


        //====================================私有静态方法=============================
        

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
    }
}
