using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
//    CREATE TABLE wines(
//    id INT UNIQUE NOT NULL AUTO_INCREMENT,
//    code CHAR(100) UNIQUE NOT NULL,
//    chateau TEXT,
//    country TEXT,
//    appellation TEXT,
//    quality TEXT,
//    vintage YEAR,
//    description TEXT,
//    bottle TEXT,
//    score TEXT,
//    INDEX (code),
//    PRIMARY KEY(id)
//)TYPE=INNODB;
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
                    aWine = new Wine();
                    aWine.m_id = reader.GetInt32("id");
                    aWine.Code = reader.GetString("code");
                    aWine.Chateau = reader.GetString("chateau");
                    aWine.Country = reader.GetString("country");
                    aWine.Appellation = reader.GetString("appellation");
                    aWine.Quality = reader.GetString("quality");
                    aWine.Vintage = reader.GetInt32("vintage");
                    aWine.Description = reader.GetString("description");
                    aWine.Bottle = reader.GetString("bottle");
                    aWine.Score = reader.GetString("score");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return aWine;
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
    }
}
