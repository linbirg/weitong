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
        private int m_minconsuption;

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

        public int MinConsuption
        {
            get { return m_minconsuption; }
            set { m_minconsuption = value; }
        }

        public void update2DB()
        {
            updateMemberLevel2DB(this);
        }

        public void deleteFromDB()
        {
            deleteMemberLevelFromDB(this);
        }

        public override string ToString()
        {
            return this.Name;
        }

        #region public static method
        
        /// <summary>
        /// 从数据库中加载所有的级别信息。
        /// </summary>
        /// <returns></returns>
        public static List<MemberLevel> loadData()
        {
            List<MemberLevel> list = null;
            string qryStr = @"SELECT memlevel,discount,levelname,minconsuption FROM memberlevel";
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
                    aRow.MinConsuption = reader.GetInt32("minconsuption");

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
            lvl.MinConsuption = 99999999;
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
            insertMemberLevel(level.Level, level.Name, level.Discount,level.MinConsuption);
        }

        /// <summary>
        /// 通过级别查找。
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static MemberLevel findByLevel(int lv)
        {
            MemberLevel level = null;
            string qryStr = @"SELECT memlevel,discount,levelname,minconsuption FROM memberlevel WHERE memlevel=@level";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@level", MySqlDbType.Int32).Value = lv;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if(reader.Read())
                {
                    level = new MemberLevel();
                    level.Level = reader.GetInt32("memlevel");
                    level.Discount = reader.GetInt32("discount");
                    level.Name = reader.GetString("levelname");
                    level.MinConsuption = reader.GetInt32("minconsuption");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }
            return level;
        }

        /// <summary>
        /// 根据消费的金额查找级别,返回最接近该金额的级别
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static MemberLevel findByAmount(int amount)
        {
            List<MemberLevel> allLvs = MemberLevel.loadData();
            MemberLevel matchLevel = null;
            foreach (MemberLevel aLv in allLvs)
            {
                if (aLv.MinConsuption <= amount)
                {
                    if (matchLevel == null)
                    {
                        matchLevel = aLv;
                    }
                    else
                    {
                        if (aLv.MinConsuption > matchLevel.MinConsuption)
                        {
                            matchLevel = aLv;
                        }
                    }
                }
            }
            return matchLevel; 
        }


        #endregion

        // ==========================私有静态方法===============================

        #region private static method
        // 插入数据库
        private static void insertMemberLevel(int level, string name, int discount, int minConsuption)
        {
            string insertStr = @"INSERT INTO memberlevel(memlevel,levelname,discount,minconsuption) 
                                 VALUES(@level,@name,@discount,@minconsuption)";
            MySqlCommand insertCmd = new MySqlCommand();
            insertCmd.CommandText = insertStr;
            insertCmd.Connection = ConnSingleton.Connection;

            insertCmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = discount;
            insertCmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            insertCmd.Parameters.Add("@level", MySqlDbType.Int32).Value = level;
            insertCmd.Parameters.Add("@minconsuption", MySqlDbType.Int32).Value = minConsuption;

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
            string updateStr = @"UPDATE memberlevel SET discount=@discount, levelname=@name, minconsuption=@minconsuption 
                                 WHERE memlevel=@level";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;

            updateCmd.Parameters.Add("@discount",MySqlDbType.Int32).Value = memLevInfo.Discount;
            updateCmd.Parameters.Add("@name",MySqlDbType.VarChar).Value = memLevInfo.Name;
            updateCmd.Parameters.Add("@level", MySqlDbType.Int32).Value = memLevInfo.Level;
            updateCmd.Parameters.Add("@minconsuption", MySqlDbType.Int32).Value = memLevInfo.MinConsuption;

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

        #endregion

    }
}
