using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    class Supplier
    {
        private int m_id = -1;
        private string m_name;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        public int ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public void save()
        {
            if (m_id < 0)
            {
                this.m_id = insertSupplier(this);
            }
            else
            {
                updateSupplier(this);
            }
        }


        // =================静态公有方法=====================
        public static Supplier findByName(string name)
        {
            Supplier splr = null;
            string qryStr = @"SELECT id, name FROM supplier WHERE name=@name";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.CommandText = qryStr;
            qryCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if (reader.Read())
                {
                    splr = new Supplier();
                    splr.ID = reader.GetInt32("id");
                    splr.Name = reader.GetString("name");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return splr;
        }

        public static Supplier findByID(int id)
        {
            Supplier splr = null;
            string qryStr = @"SELECT id, name FROM supplier WHERE id=@id";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.CommandText = qryStr;
            qryCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if (reader.Read())
                {
                    splr = new Supplier();
                    splr.ID = reader.GetInt32("id");
                    splr.Name = reader.GetString("name");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return splr;
        }

        public static Supplier NewSupplier()
        {
            Supplier spler = new Supplier();
            spler.ID = -1;
            spler.Name = "linbirg";
            return spler;
        }

        //========================私有静态方法=================================
        // 将supplier的插入到数据库，函数返回新插入的supplier的id。
        private static int insertSupplier(Supplier supplier)
        {
            int supplier_id = -1;
            if (supplier == null) return supplier_id;
            string insertStr = @"INSERT INTO supplier(name) VALUES(@name)";
            MySqlCommand insertCmd = new MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = ConnSingleton.Connection;
            insertCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = supplier.Name;
            
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
                    supplier_id = reader.GetInt32(0);
                }
            }
            finally
            {
                insertCmd.Connection.Close();
            }
            return supplier_id;
        }

        // 负的id不更新
        private static void updateSupplier(Supplier supplier)
        {
            if (supplier == null || supplier.m_id < 0) return;
            string updateStr = @"UPDATE supplier SET name=@name WHERE id=@supplier_id";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;
            updateCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = supplier.Name;
            
            updateCmd.Parameters.Add("@supplier_id", MySqlDbType.Int32).Value = supplier.m_id;

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
