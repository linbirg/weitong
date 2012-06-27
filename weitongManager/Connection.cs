using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
namespace weitongManager
{
    sealed class ConnSingleton
    {
        private ConnSingleton()
        { 

        }
        private static MySqlConnection m_conn = null;
        //public static readonly Connection Instance = new Connection();
        public static MySqlConnection Connection
        {
            get
            {
                //if (m_conn == null)
                //    m_conn = new MySqlConnection("server=localhost;User Id=weitong;password=weitong;database=weitong");
                //return m_conn;
                return new MySqlConnection("server=localhost;User Id=weitong;password=weitong;database=weitong");
            }
        }
    }
}
