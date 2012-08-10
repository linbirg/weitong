using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace weitongManager
{
    class util
    {
        #region random string
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        ///<summary>
        ///生成字符串源
        ///</summary>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>包含指定字符类型的字符串源</returns>
        static string GetSourceString(string custom="", bool useSpe = false,bool useNum = true, bool useLow = true, bool useUpp = true)
        {
            string sourceStr = custom;
            if (useNum == true) { sourceStr += "0123456789"; }
            if (useLow == true) { sourceStr += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { sourceStr += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { sourceStr += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

            return sourceStr;
        }

        /// <summary>
        /// 生成指定长度的随即字符串。字符串可能包含特殊字符，数字，大小写等。
        /// </summary>
        /// <param name="length"></param>
        /// <returns>包含指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useSpe = false, bool useNum = true, bool useLow = true, bool useUpp = true)
        {
            StringBuilder sbPwd = new StringBuilder();
            Random random = new Random(GetRandomSeed());
            string strSource = GetSourceString("", useSpe, useNum, useLow, useUpp);
            for (int i = 0; i < length; i++)
            {
                sbPwd.Append(strSource.Substring(random.Next(0, strSource.Length -1), 1));
                //sbPwd.Append( sourceArray[random.Next( 0 , 25 )] );
            }
            return sbPwd.ToString();
        }

        #endregion

        #region order2table

        public static weitongDataSet1.orderDataTable Order2Table(List<Order> orders)
        {
            weitongDataSet1.orderDataTable table = new weitongDataSet1.orderDataTable();
            if (orders != null)
            {
                foreach (Order anOrder in orders)
                {
                    weitongDataSet1.orderRow row = table.NeworderRow();
                    row.amount = anOrder.Amount;
                    row.comment = anOrder.Comment;
                    row.effectdate = anOrder.EffectDate;
                    row.id = anOrder.ID;
                    row.name = Customer.findByID(anOrder.CustomerID).Name;
                    row.orderstate = (int)anOrder.State;
                    row.received = anOrder.Received;
                    table.AddorderRow(row);
                }
            }
            return table;
        }

        /// <summary>
        /// 交互两个变量的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="my"></param>
        /// <param name="other"></param>
        public static void Swap<T>(ref T my, ref T other)
        {
            T temp = my;
            my = other;
            other = temp;
        }


        /// <summary>  
        /// 取得某月的第一天  
        /// </summary>  
        /// <param name="datetime">要取得月份第一天的时间</param>  
        /// <returns></returns>  
        public static DateTime FirstDayOfMonth(DateTime datetime)  
        {
            return datetime.AddDays(1 - datetime.Day);
        }  
  
        /**//// <summary>  
        /// 取得某月的最后一天  
        /// </summary>  
        /// <param name="datetime">要取得月份最后一天的时间</param>  
        /// <returns></returns>  
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }  
  
        /**//// <summary>  
        /// 取得上个月第一天  
        /// </summary>  
        /// <param name="datetime">要取得上个月第一天的当前时间</param>  
        /// <returns></returns>  
        public static DateTime FirstDayOfPreviousMonth(DateTime datetime)  
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1); 
        }  
  
        /**//// <summary>  
        /// 取得上个月的最后一天  
        /// </summary>  
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>  
        /// <returns></returns>  
        public static DateTime LastDayOfPrdviousMonth(DateTime datetime)  
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1); 
        }  

        #endregion


        public static bool isPhoneNumber(string input)
        {
            string phoneNumerPattern = @"^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$";
            return Regex.IsMatch(input, phoneNumerPattern);
        }

        public static bool isMailAdress(string input)
        {
            string regexEmail = "//w{1,}@//w{1,}//.//w{1,}";
            return Regex.IsMatch(input, regexEmail);
        }


    }
}
