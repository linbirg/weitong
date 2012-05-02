using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    class MemberLevel
    {
        private int m_level;
        private int m_discount;
        private string m_name;

        public int Level
        {
            get { return m_level; }
            set { m_level = value; }
        }

        public int Discount
        {
            get { return m_discount; }
            set { m_discount = value; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }


        public void update2DB()
        {
            updateMemberLevel2DB(this);
        }

        public void deleteFromDB()
        {
            deleteMemberLevelFromDB(this);
        }


        // 从数据库中加载级别信息。
        public static List<MemberLevel> loadData()
        {
            List<MemberLevel> list = null;
            string qryStr = @"SELECT memlevel,discount,levelname FROM memberlevel";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                while (reader.Read())
                {
                    if (list == null) list = new List<MemberLevel>();
                    MemberLevel aRow = new MemberLevel();
                    aRow.Level = reader.GetInt32("memlevel");
                    aRow.Discount = reader.GetInt32("discount");
                    aRow.Name = reader.GetString("levelname");

                    list.Add(aRow);
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }
            return list;
        }

        public static MemberLevel NewMemberLevel()
        {
            MemberLevel lvl = new MemberLevel();
            lvl.Level = 1;
            lvl.Discount = 100;
            lvl.Name = "普通会员";
            return lvl;
        }

        public static int getTopLevel()
        {
            int level = -1;

            string qryStr = @"SELECT MAX(memlevel) FROM memberlevel";

            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows && !reader.IsDBNull(0))
                {
                    level = reader.GetInt32(0);
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }

            return level;
        }

        // 
        public static void insertLevelInfo(MemberLevel level)
        {
            insertMemberLevel(level.Level, level.Name, level.Discount);
        }
        

        // ==========================私有===============================
        // 插入数据库
        private static void insertMemberLevel(int level, string name, int discount)
        {
            string insertStr = @"INSERT INTO memberlevel(memlevel,levelname,discount) 
                                 VALUES(@level,@name,@discount)";
            MySqlCommand insertCmd = new MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = ConnSingleton.Connection;

            insertCmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = discount;
            insertCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            insertCmd.Parameters.Add("@level", MySqlDbType.Int32).Value = level;

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


        private static void updateMemberLevel2DB(MemberLevel memLevInfo)
        {
            string updateStr = @"UPDATE memberlevel SET discount=@discount, levelname=@name 
                                 WHERE memlevel=@level";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;

            updateCmd.Parameters.Add("@discount",MySqlDbType.Int32).Value = memLevInfo.Discount;
            updateCmd.Parameters.Add("@name",MySqlDbType.VarChar).Value = memLevInfo.Name;
            updateCmd.Parameters.Add("@level", MySqlDbType.Int32).Value = memLevInfo.Level;

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

        private static void deleteMemberLevelFromDB(MemberLevel memLevInfo)
        {
            string deleteStr = @"DELETE FROM memberlevel 
                                 WHERE memlevel=@level";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = deleteStr;
            updateCmd.Connection = ConnSingleton.Connection;

            
            updateCmd.Parameters.Add("@level", MySqlDbType.Int32).Value = memLevInfo.Level;

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
