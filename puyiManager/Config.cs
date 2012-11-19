using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weitongManager
{
    class Config
    {
        public static string ConnectionString
        {
            get {
                return isUnderControl ? weitongConStr : puyiConStr;
            }
        }
        public static string Version = "1.0.6";
        public static string Name = "葡驿红酒销售管理系统";
        public static string suffix = "内控";
        public static bool isUnderControl = false;

        private static string puyiConStr = "server=localhost;User Id=puyi;password=puyi;database=puyi";
        private static string weitongConStr = "server=localhost;User Id=weitong;password=weitong;database=weitong";
    }
}
