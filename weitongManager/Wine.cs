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
    }
}
