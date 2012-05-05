using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    class Role
    {
        private int m_id = -1;
        private string m_name;
        private int m_discount;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public int Discount
        {
            get { return m_discount; }
            set { m_discount = value; }
        }

        public void save()
        {
            if (m_id < 0)
            {
                this.m_id = insertRole(this);
            }
            else
            {
                updateRole(this);
            }
        }

        // ====================公开静态方法======================
        public static Role NewRole()
        {
            Role rl = new Role();
            rl.m_id = -1;
            rl.m_discount = 100;
            rl.m_name = "customer";
            return rl;
        }

        public static List<Role> loadData()
        {
            List<Role> list = null;
            string qryStr = @"SELECT id,name,discount FROM roles";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                while (reader.Read())
                {
                    if (list == null) list = new List<Role>();
                    Role aRow = new Role();
                    
                    aRow.Discount = reader.GetInt32("discount");
                    aRow.Name = reader.GetString("name");
                    aRow.m_id = reader.GetInt32("id");
                    list.Add(aRow);
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }
            return list;
        }

        public static Role findByID(int id)
        {
            Role aRole = null;
            string qryStr = @"SELECT id,name,discount FROM roles WHERE id=@id";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if(reader.Read())
                {
                    aRole = NewRole();
                    aRole.Discount = reader.GetInt32("discount");
                    aRole.Name = reader.GetString("name");
                    aRole.m_id = reader.GetInt32("id");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }
            return aRole;
        }


        // ====================私有静态方法======================
        // 将role的插入到数据库，函数返回新插入的role的id。
        private static int insertRole(Role role)
        {
            int role_id = -1;
            if (role == null) return role_id;
            string insertStr = @"INSERT INTO roles(name,discount) VALUES(@name,@discount)";
            MySqlCommand insertCmd = new MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = ConnSingleton.Connection;
            insertCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = role.Name;
            insertCmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = role.Discount;

            try
            {
                insertCmd.Connection.Open();
                insertCmd.ExecuteNonQuery();
                string queryIdStr = "SELECT LAST_INSERT_ID()";
                insertCmd.CommandText = queryIdStr;
                MySqlDataReader reader = insertCmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows && !reader.IsDBNull(0))
                {
                    role_id = reader.GetInt32(0);
                }
            }
            finally
            {
                insertCmd.Connection.Close();
            }
            return role_id;
        }

        // 负的id不更新
        private static void updateRole(Role role)
        {
            if (role == null || role.m_id < 0) return;
            string updateStr = @"UPDATE roles SET name=@name, discount=@discount WHERE id=@role_id";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;
            updateCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = role.Name;
            updateCmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = role.Discount;
            updateCmd.Parameters.Add("@role_id", MySqlDbType.Int32).Value = role.m_id;

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
    }
}
