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
