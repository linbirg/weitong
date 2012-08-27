using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{


    class Wine
    {
        private int m_id;
        private string m_code;
        private string m_chateau;
        private string m_country;
        private string m_appellation;
        private string m_quality;
        private int m_vintage;
        private string m_description;
        private string m_bottle;
        private string m_score;

        #region public property
        public string Code
        {
            get { return m_code; }
            set { m_code = value; }
        }

        public string Chateau
        {
            get { return m_chateau; }
            set { m_chateau = value; }
        }

        public string Country
        {
            get { return m_country; }
            set { m_country = value; }
        }

        public string Appellation
        {
            get { return m_appellation; }
            set { m_appellation = value; }
        }

        public string Quality
        {
            get { return m_quality; }
            set { m_quality = value; }
        }

        public int Vintage
        {
            get { return m_vintage; }
            set { m_vintage = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public string Bottle
        {
            get { return m_bottle; }
            set { m_bottle = value; }
        }

        public string Score
        {
            get { return m_score; }
            set { m_score = value; }
        }
        #endregion

        #region public static method
        /// <summary>
        /// 根据编码查找酒
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Wine findByCode(string code)
        {
            Wine aWine = null;
            string qStr = @"SELECT id, code, chateau, country, appellation, quality, vintage, description, bottle, score
                            FROM wines
                            WHERE code = @code";

            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if (reader.Read())
                {
                    aWine = getWineFromReader(reader);
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return aWine;
        }

        /// <summary>
        /// 根据描述查找酒
        /// </summary>
        /// <param name="description"></param>
        /// <returns>包含酒的列表，如果没找到则列表元素数量为零</returns>
        public static List<Wine> findByDescription(string description)
        {
            List<Wine> list = new List<Wine>();
            string qStr = @"SELECT id, code, chateau, country, appellation, quality, vintage, description, bottle, score
                            FROM wines
                            WHERE description like @description";

            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@description", MySqlDbType.VarChar).Value = "%" + description + "%";

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                while (reader.Read())
                {
                    Wine aWine = getWineFromReader(reader);
                    list.Add(aWine);
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return list;
        }

        // 
        /// <summary>
        /// 测试指定的酒是否已经在wines表中
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool existsWine(string code)
        {
            bool result = false;
            string qStr = @"SELECT EXISTS(
                                SELECT * FROM wines WHERE code = @code)";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = ConnSingleton.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;
            queryCmd.Connection.Open();
            MySqlDataReader reader = queryCmd.ExecuteReader();
            reader.Read();

            if (reader.HasRows)
            {
                result = reader.GetBoolean(0);
            }
            queryCmd.Connection.Close();

            return result;
        }

        // 
        /// <summary>
        /// 添加酒的信息，code不能为空
        /// </summary>
        /// <param name="code"></param>
        /// <param name="chateau"></param>
        /// <param name="country"></param>
        /// <param name="appellation"></param>
        /// <param name="quality"></param>
        /// <param name="vintage"></param>
        /// <param name="description"></param>
        /// <param name="bottle"></param>
        /// <param name="score"></param>
        public static void insert(string code, string chateau = null, string country = null,
            string appellation = null, string quality = null, string vintage = null,
            string description = null, string bottle = null, string score = null)
        {
            string insStr = @"INSERT INTO wines(code,chateau,country,appellation,quality,vintage,description,bottle,score) 
                                VALUES(@code,@chateau,@country,@appellation,@quality,@vintage,@description,@bottle,@score)";
            MySql.Data.MySqlClient.MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.Connection = ConnSingleton.Connection;
            insertCmd.CommandText = insStr;
            insertCmd.Parameters.Add("@code", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = code;
            insertCmd.Parameters.Add("@chateau", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = chateau;
            insertCmd.Parameters.Add("@country", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = country;
            insertCmd.Parameters.Add("@appellation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = appellation;
            insertCmd.Parameters.Add("@quality", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = quality;
            insertCmd.Parameters.Add("@vintage", MySql.Data.MySqlClient.MySqlDbType.Year).Value = vintage;
            insertCmd.Parameters.Add("@description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = description;
            insertCmd.Parameters.Add("@bottle", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = bottle;
            insertCmd.Parameters.Add("@score", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = score;
            //m_dataAdapter.Adapter.InsertCommand = insertCmd;
            try
            {
                insertCmd.Connection.Open();
                insertCmd.ExecuteNonQuery();
            }
            finally
            {
                insertCmd.Connection.Close();
            }
            
        }

        // 
        /// <summary>
        /// 更新酒的信息，code不能为空
        /// </summary>
        /// <param name="code"></param>
        /// <param name="chateau"></param>
        /// <param name="country"></param>
        /// <param name="appellation"></param>
        /// <param name="quality"></param>
        /// <param name="vintage"></param>
        /// <param name="description"></param>
        /// <param name="bottle"></param>
        /// <param name="score"></param>
        public static void update(string code, string chateau = null, string country = null, string appellation = null, string quality = null, string vintage = null, string description = null, string bottle = null, string score = null)
        {
            string updateStr = @"UPDATE wines SET chateau = @chateau ,country = @country, appellation = @appellation, 
                                quality = @quality, vintage = @vintage, description = @description, bottle = @bottle, score = @score
                                WHERE code = @code";
            MySql.Data.MySqlClient.MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
            updateCmd.Connection = ConnSingleton.Connection;
            updateCmd.CommandText = updateStr;
            updateCmd.Parameters.Add("@code", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = code;
            updateCmd.Parameters.Add("@chateau", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = chateau;
            updateCmd.Parameters.Add("@country", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = country;
            updateCmd.Parameters.Add("@appellation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = appellation;
            updateCmd.Parameters.Add("@quality", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = quality;
            updateCmd.Parameters.Add("@vintage", MySql.Data.MySqlClient.MySqlDbType.Year).Value = vintage;
            updateCmd.Parameters.Add("@description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = description;
            updateCmd.Parameters.Add("@bottle", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = bottle;
            updateCmd.Parameters.Add("@score", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = score;

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

#endregion

        #region private static method

        /// <summary>
        /// 从reader中读取酒的信息。
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static Wine getWineFromReader(MySqlDataReader reader)
        {
            Wine aWine = new Wine();
            aWine.m_id = reader.GetInt32("id");
            aWine.Code = reader.GetString("code");
            aWine.Chateau = reader.IsDBNull(2) ? null : reader.GetString("chateau");
            aWine.Country = reader.IsDBNull(3) ? null : reader.GetString("country");
            aWine.Appellation = reader.IsDBNull(4) ? null : reader.GetString("appellation");
            aWine.Quality = reader.IsDBNull(5) ? null : reader.GetString("quality");
            aWine.Vintage = reader.IsDBNull(6) ? default(int) : reader.GetInt32("vintage");
            aWine.Description = reader.IsDBNull(7) ? null : reader.GetString("description");
            aWine.Bottle = reader.IsDBNull(8) ? null : reader.GetString("bottle");
            aWine.Score = reader.IsDBNull(9) ? null : reader.GetString("score");
            return aWine;
        }

        #endregion
    }
}
