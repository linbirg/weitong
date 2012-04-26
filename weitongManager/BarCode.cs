using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
namespace weitongManager
{
    /// <summary>

    /// 生成条形码( 128条码,标准参考:GB/T 18347-2001 )

    /// BY JUNSON 20090508

    /// </summary>

    public class BarCode
    {

        /// <summary>

        /// 条形码生成函数

        /// </summary>

        /// <param name="text">条型码字串</param>

        /// <returns></returns>

        public static Bitmap BuildBarCode(string text)
        {

            //查检是否合条件TEXT

            bool ck = CheckErrerCode(text);

            if (!ck)

                throw new Exception("条形码字符不合要求，不能是汉字或全角字符");

            string barstring = BuildBarString(text);

            return KiCode128CWithText(barstring, 30, text);

        }

        /// <summary>

        /// 建立条码字符串

        /// </summary>

        /// <param name="tex">条码内容</param>

        /// <returns></returns>

        private static string BuildBarString(string tex)
        {

            string barstart = "bbsbssbssss";    //码头

            string barbody = "";                //码身

            string barcheck = "";               //码检

            string barend = "bbsssbbbsbsbb";    //码尾

            int checkNum = 104;

            //循环添加码身,计算码检

            for (int i = 1; i <= tex.Length; i++)
            {

                int index = (int)tex[i - 1] - 32;

                checkNum += (index * i);

                barbody += AddSimpleTag(index);//加入字符值的条码标记

            }

            //码检值计算

            barcheck = AddSimpleTag(int.Parse(Convert.ToDouble(checkNum % 103).ToString("0")));

            string barstring = barstart + barbody + barcheck + barend;

            return barstring;

        }

        //增加一个条码标记

        private static string AddSimpleTag(int CodeIndex)
        {

            string res = "";

            /// <summary>1-4的条的字符标识 </summary>

            string[] TagB = { "", "b", "bb", "bbb", "bbbb" };

            /// <summary>1-4的空的字符标识 </summary>

            string[] TagS = { "", "s", "ss", "sss", "ssss" };

            string[] Code128List = new string[] {
 
                "212222","222122","222221","121223","121322","131222","122213","122312","132212","221213",
 
                "221312","231212","112232","122132","122231","113222","123122","123221","223211","221132",
 
                "221231","213212","223112","312131","311222","321122","321221","312212","322112","322211",
 
                "212123","212321","232121","111323","131123","131321","112313","132113","132311","211313",
 
                "231113","231311","112133","112331","132131","113123","113321","133121","313121","211331",
 
                "231131","213113","213311","213131","311123","311321","331121","312113","312311","332111",
 
                "314111","221411","431111","111224","111422","121124","121421","141122","141221","112214",
 
                "112412","122114","122411","142112","142211","241211","221114","413111","241112","134111",
 
                "111242","121142","121241","114212","124112","124211","411212","421112","421211","212141",
 
                "214121","412121","111143","111341","131141","114113","114311","411113","411311","113141",
 
                "114131","311141","411131","211412","211214","211232" };

            string tag = Code128List[CodeIndex];

            for (int i = 0; i < tag.Length; i++)
            {

                string temp = "";

                int num = int.Parse(tag[i].ToString());

                if (i % 2 == 0)
                {

                    temp = TagB[num];

                }

                else
                {

                    temp = TagS[num];

                }

                res += temp;

            }

            return res;

        }

        /// <summary>

        /// 检查条形码文字是否合条件(不能是汉字或全角字符)

        /// </summary>

        /// <param name="cktext"></param>

        /// <returns></returns>

        private static bool CheckErrerCode(string cktext)
        {

            foreach (char c in cktext)
            {

                byte[] tmp = System.Text.UnicodeEncoding.Default.GetBytes(c.ToString());

                if (tmp.Length > 1)

                    return false;

            }

            return true;

        }

        /// <summary>生成条码 </summary>

        /// <param name="BarString">条码模式字符串</param> //Format32bppArgb

        /// <param name="Height">生成的条码高度</param>

        /// <returns>条码图形</returns>

        private static Bitmap KiCode128C(string BarString, int _Height, int Botton_Margin = 0)
        {

            Bitmap b = new Bitmap(BarString.Length, _Height + Botton_Margin, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //using (Graphics grp = Graphics.FromImage(b))

            //{

            try
            {

                char[] cs = BarString.ToCharArray();

                for (int i = 0; i < cs.Length; i++)
                {

                    for (int j = 0; j < _Height; j++)
                    {

                        if (cs[i] == 'b')
                        {

                            b.SetPixel(i, j, Color.Black);

                        }

                        else
                        {

                            b.SetPixel(i, j, Color.White);

                        }

                    }

                }

                //grp.DrawString(text, SystemFonts.CaptionFont, Brushes.Black, new PointF(0, _Height));

                return b;

            }

            catch
            {

                return null;

            }

            //}

        }

        /// <summary>生成条码下面的字符串 </summary>
        /// <param name="BarString">条码模式字符串</param> //Format32bppArgb
        /// <param name="Height">条码高度</param>

        /// <returns>包含字符的条码图形</returns>

        private static Bitmap KiCode128CWithText(string BarString, int _Height, string text)
        {
            try
            {
                Bitmap barMap = KiCode128C(BarString, _Height, 20);
                char[] textArray = text.ToArray<char>();
                int width = barMap.Width;
                int countChar = (textArray.Length > 1) ? (textArray.Length-1):1;
                Font txtFont = new Font("SimSun", 10); //SystemFonts.DefaultFont; //
                
                using (Graphics grp = Graphics.FromImage(barMap))
                {
                    SizeF size = grp.MeasureString(textArray[0].ToString(),txtFont);
                    int delta = (width - (int)size.Width) / countChar;
                    for (int i = 0; i < textArray.Length; i++)
                    {
                        string t = textArray[i].ToString();
                        grp.DrawString(t, txtFont, Brushes.Black, new PointF(i * delta, _Height));
                    }
                }
                return barMap;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
