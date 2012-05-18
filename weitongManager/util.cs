using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weitongManager
{
    class util
    {
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
        public static string GetRandomString(int length)
        {
            StringBuilder sbPwd = new StringBuilder();
            Random random = new Random(GetRandomSeed());
            string strSource = GetSourceString("", true);
            for (int i = 0; i < length; i++)
            {
                sbPwd.Append(strSource.Substring(random.Next(0, strSource.Length -1), 1));
                //sbPwd.Append( sourceArray[random.Next( 0 , 25 )] );
            }
            return sbPwd.ToString();
        }

        
    }
}
