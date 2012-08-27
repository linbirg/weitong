using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    class StatisticsMgr
    {
        /// <summary>
        /// 获取用户某日完成的订单总数
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="day">指定日期</param>
        /// <returns>当日用户完成的订单总数</returns>
        public static int countUserDayOrders(int userID, DateTime day)
        {
            return countUserOrders(userID, day, day.AddDays(1));
        }

        public static int sumUserDayOrders(int userID, DateTime day)
        {
            return sumUserOrders(userID, day, day.AddDays(1));
        }

        public static int countUserMonthOrders(int userID, int month)
        {
            int year = DateTime.Now.Year;
            DateTime startDayOfMonth = new DateTime(DateTime.Now.Year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            DateTime startDayOfNextMonth = startDayOfMonth.AddDays(days);
            return countUserOrders(userID, startDayOfMonth, startDayOfNextMonth);
        }

        public static int sumUserMonthOrders(int userID, int month)
        {
            int year = DateTime.Now.Year;
            DateTime startDayOfMonth = new DateTime(DateTime.Now.Year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            DateTime startDayOfNextMonth = startDayOfMonth.AddDays(days);
            return sumUserOrders(userID, startDayOfMonth, startDayOfNextMonth); 
        }

        // 从数据库中查找指定日期间的用户的订单总数。
        // beginDate和endDate是DateTime类型，endDate必须要大于等于beginDate，否则查不到数据。
        public static int countUserOrders(int userID, DateTime beginDate, DateTime endDate)
        {
            int total = 0;
            string qryStr = @"SELECT COUNT(1) as total FROM orders 
                              WHERE userid=@userid
                              AND CAST(effectdate as DATETIME) BETWEEN CAST(@begindate as DATETIME) AND CAST(@enddate as DATETIME)
                              AND orderstate=2";   //orderstate=2,指定是完成付款的订单";
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@userid", MySqlDbType.Int32).Value = userID;
            qryCmd.Parameters.Add("@begindate", MySqlDbType.DateTime).Value = beginDate;
            qryCmd.Parameters.Add("@enddate", MySqlDbType.DateTime).Value = endDate;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if (reader.Read())
                {
                    total = reader.GetInt32("total");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }
            return total; 
        }

        // 从数据库中查找指定日期间的用户的订单总金额。
        // beginDate和endDate是DateTime类型，endDate必须要大于等于beginDate，否则查不到数据。
        public static int sumUserOrders(int userID, DateTime beginDate, DateTime endDate)
        {
            int total = 0;
            string qryStr = @"SELECT SUM(received) as total FROM orders 
                              WHERE userid=@userid
                              AND CAST(effectdate as DATETIME) BETWEEN CAST(@begindate as DATETIME) AND CAST(@enddate as DATETIME)
                              AND orderstate=2";   //orderstate=2,指定是完成付款的订单
            MySqlCommand qryCmd = new MySqlCommand();
            qryCmd.CommandText = qryStr;
            qryCmd.Connection = ConnSingleton.Connection;
            qryCmd.Parameters.Add("@userid", MySqlDbType.Int32).Value = userID;
            qryCmd.Parameters.Add("@begindate", MySqlDbType.DateTime).Value = beginDate;
            qryCmd.Parameters.Add("@enddate", MySqlDbType.DateTime).Value = endDate;

            try
            {
                qryCmd.Connection.Open();
                MySqlDataReader reader = qryCmd.ExecuteReader();
                if (reader.Read()&&!reader.IsDBNull(0))
                {
                    total = reader.GetInt32("total");
                }
            }
            finally
            {
                qryCmd.Connection.Close();
            }
            return total;
        }
    }
}
