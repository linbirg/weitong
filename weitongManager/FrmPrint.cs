using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace weitongManager
{
    partial class FrmPrint : Form
    {
       
        //PrintDocument类是实现打印功能的核心，它封装了打印有关的属性、事件、和方法
        private PrintDocument m_printDocument = null;//new PrintDocument();

        private BindingList<CartDetailRowData> m_DetailList = null;
        private DataGridView m_CartDetaiGrid = null;
        private Customer m_customer = null;
        private Order m_order = null;
        private User m_user = null;

        private DateTime m_orderTime;
        private int m_order_total_units = 0;

        private int m_currentPageIndex = 0;
        private int m_maxRowPerPage = MAX_ROW_PER_PAGE;
        private int m_totalPageCount = 0;
        private readonly static int MAX_ROW_PER_PAGE = 8;

        private RectangleF m_printArea;

        public FrmPrint()
        {
            InitializeComponent();
            m_printDocument = new PrintDocument();
            //打印开始前
            m_printDocument.BeginPrint += new PrintEventHandler(printDocument_BeginPrint);
            //打印输出（过程）
            m_printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            //打印结束
            m_printDocument.EndPrint += new PrintEventHandler(printDocument_EndPrint);
            m_DetailList = new BindingList<CartDetailRowData>();
        }
        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                resetGlobalSettings();
                //printDocument.PrinterSettings可以获取或设置计算机默认打印相关属性或参数，如：printDocument.PrinterSettings.PrinterName获得默认打印机打印机名称
                //printDocument.DefaultPageSettings   //可以获取或设置打印页面参数信息、如是纸张大小，是否横向打印等

                //设置文档名
                m_printDocument.DocumentName = lbl_custNameContent.Text + " 订金单";//设置完后可在打印对话框及队列中显示（默认显示document）

                //设置纸张大小（可以不设置取，取默认设置）
                PaperSize ps = new PaperSize("Your Paper Name", 210, 297);
                ps.RawKind = 9; //如果是自定义纸张，就要大于118，（A4值为9，详细纸张类型与值的对照请看http://msdn.microsoft.com/zh-tw/library/system.drawing.printing.papersize.rawkind(v=vs.85).aspx）
                m_printDocument.DefaultPageSettings.PaperSize = ps;

                ////打印开始前
                //printDocument.BeginPrint += new PrintEventHandler(printDocument_BeginPrint);
                ////打印输出（过程）
                //printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
                ////打印结束
                //printDocument.EndPrint += new PrintEventHandler(printDocument_EndPrint);

                //跳出打印对话框，提供打印参数可视化设置，如选择哪个打印机打印此文档等
                PrintDialog pd = new PrintDialog();
                pd.Document = m_printDocument;
                if (DialogResult.OK == pd.ShowDialog()) //如果确认，将会覆盖所有的打印参数设置
                {
                    //页面设置对话框（可以不使用，其实PrintDialog对话框已提供页面设置）
                    PageSetupDialog psd = new PageSetupDialog();
                    psd.Document = m_printDocument;
                    if (DialogResult.OK == psd.ShowDialog())
                    {
                        ////打印预览
                        //PrintPreviewDialog ppd = new PrintPreviewDialog();
                        //ppd.Document = printDocument;
                        //if (DialogResult.OK == ppd.ShowDialog())
                        m_printDocument.Print();          //打印
                    }
                }
            }
            catch (Exception ex)
            {
                //打印异常信息
            }
        }

        private void btn_Preview_Click(object sender, EventArgs e)
        {
            try
            {
                resetGlobalSettings();
                //printDocument.PrinterSettings可以获取或设置计算机默认打印相关属性或参数，如：printDocument.PrinterSettings.PrinterName获得默认打印机打印机名称
                //printDocument.DefaultPageSettings   //可以获取或设置打印页面参数信息、如是纸张大小，是否横向打印等
                //Bitmap bmp = BarCode.BuildBarCode("20120004");
                //bmp.Save(@"D:\\tiaoxingma.bmp");
                //设置文档名
                m_printDocument.DocumentName = lbl_custNameContent.Text + " 订金单";//设置完后可在打印对话框及队列中显示（默认显示document）

                //设置纸张大小（可以不设置取，取默认设置）
                PaperSize ps = new PaperSize("Your Paper Name", 210, 297);
                ps.RawKind = 9; //如果是自定义纸张，就要大于118，（A4值为9，详细纸张类型与值的对照请看http://msdn.microsoft.com/zh-tw/library/system.drawing.printing.papersize.rawkind(v=vs.85).aspx）
                m_printDocument.DefaultPageSettings.PaperSize = ps;

                ////打印开始前
                //printDocument.BeginPrint += new PrintEventHandler(printDocument_BeginPrint);
                ////打印输出（过程）
                //printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
                ////打印结束
                //printDocument.EndPrint += new PrintEventHandler(printDocument_EndPrint);

                //跳出打印对话框，提供打印参数可视化设置，如选择哪个打印机打印此文档等
                //PrintDialog pd = new PrintDialog();
                //pd.Document = printDocument;
                //if (DialogResult.OK == pd.ShowDialog()) //如果确认，将会覆盖所有的打印参数设置
                //{
                PaperSize p = null;
                foreach (PaperSize ps2 in m_printDocument.PrinterSettings.PaperSizes)
                {
                    if (ps.PaperName.Equals("A4"))//这里设置纸张大小,但必须是定义好的  
                        p = ps2;
                }

                m_printDocument.DefaultPageSettings.PaperSize = p;

                //打印预览
                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ppd.Document = m_printDocument;
                Form f = (Form)ppd;
                f.WindowState = FormWindowState.Maximized;
                if (DialogResult.OK == ppd.ShowDialog())
                    m_printDocument.Print();          //打印

                //}
            }
            catch (Exception ex)
            {
                //打印异常信息
            }
        }

        void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            //也可以把一些打印的参数放在此处设置
            //m_totalPageCount = (m_DetailList.Count-1) / m_maxRowPerPage + 1;
        }

        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            RectangleF bestArea = GetBestPrintableArea(e);
            m_printArea = bestArea;
            float scaleX = bestArea.Width / (float)this.DisplayRectangle.Width;
            float scaleY = bestArea.Height / ((float)(this.DisplayRectangle.Height - lbl_Logo.Location.Y) * 2);
            //float scaleX = (e.PageBounds.Width - e.MarginBounds.Left*2) / (float)this.DisplayRectangle.Width;
            //int ht = (int)bestArea.Height;
            //MessageBox.Show(ht.ToString());
            // 页面对应到一半的纸张上面，只打印在上半部分。
            //float scaleY = (e.PageBounds.Height) / ((float)(this.DisplayRectangle.Height - lbl_Logo.Location.Y) * 2);
            //打印啥东东就在这写了
            Graphics g = e.Graphics;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; 


            // 上半页
            drawHalfPage(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);
            drowMiddleLine(bestArea, e);
            drawHalfPage(0, this.DisplayRectangle.Height - lbl_Logo.Location.Y * 2, scaleX, scaleY, e);
            if (hasNextPage()) printNextPage(e);
        }

        /// <summary>
        /// 获取打印机的最佳可打印区域
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private RectangleF GetBestPrintableArea(PrintPageEventArgs e)
        {
            RectangleF marginBounds = e.MarginBounds;
            RectangleF printableArea = e.PageSettings.PrintableArea;
            RectangleF pageBounds = e.PageBounds;

            if (e.PageSettings.Landscape)
                printableArea = new RectangleF(printableArea.Y, printableArea.X, printableArea.Height, printableArea.Width);

            RectangleF bestArea = RectangleF.FromLTRB(
                (float)Math.Max(marginBounds.Left, printableArea.Left),
                (float)Math.Max(marginBounds.Top, printableArea.Top),
                (float)Math.Min(marginBounds.Right, printableArea.Right),
                (float)Math.Min(marginBounds.Bottom, printableArea.Bottom)
            );

            float bestMarginX = (float)Math.Max(bestArea.Left, pageBounds.Right - bestArea.Right);
            float bestMarginY = (float)Math.Max(bestArea.Top, pageBounds.Bottom - bestArea.Bottom);

            bestArea = RectangleF.FromLTRB(
                bestMarginX,
                bestMarginY,
                pageBounds.Right - bestMarginX,
                pageBounds.Bottom - bestMarginY
            );

            return printableArea;
        }


        private void drawHalfPage(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            drowLogo(posX, posY, scaleX, scaleY, e);
            drowCode(posX, posY, scaleX, scaleY, e);
            drowTitle(posX, posY, scaleX, scaleY, e);
            showCurrentPage();
            drowOrderInfo(posX, posY, scaleX, scaleY, e);
            drawCustomerInfo(posX, posY, scaleX, scaleY, e);
            drawSignAndBeizhu(posX, posY, scaleX, scaleY, e);
            drawAnancement(posX, posY, scaleX, scaleY, e);

            if (m_currentPageIndex == m_totalPageCount - 1)
            {
                drawTotalCount(posX, posY, scaleX, scaleY, e);
                drawTotalAmount(posX, posY, scaleX, scaleY, e);
            }

            drawColumnHeader(posX, posY, scaleX, scaleY, e);
            // drawRows会改变当前页面的索引等一些全局的设置，因此必须在此页面最后打印。
            drawRows(posX, posY, scaleX, scaleY, e);
        }

        void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            //打印结束后相关操作
        }

        private void FrmPrint_Paint(object sender, PaintEventArgs e)
        {
            
        }

        //各种drow
        private void drowMiddleLine(RectangleF printableArea, PrintPageEventArgs e)
        {
            // 中线分页，将A4一分为二
            SolidBrush brush = new SolidBrush(Color.Gray);
            Graphics gh = e.Graphics;
            Pen aPen = new Pen(brush);
            aPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            int midlleY = (int)(printableArea.Y + printableArea.Height / 2);
            gh.DrawLine(aPen, 0, midlleY, e.PageBounds.Width, midlleY);
        }

        private void drowLogo(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            Graphics gh = e.Graphics;
            
            gh.DrawImage(lbl_Logo.Image,
                m_printArea.X + (posX + lbl_Logo.Location.X) * scaleX,
                m_printArea.Y + (posY + lbl_Logo.Location.Y) * scaleY,
                lbl_Logo.Width*scaleX ,
                lbl_Logo.Height*scaleY);
            drowLabel(posX, posY, scaleX,scaleY, lbl_logoAdress, e);
            //drowLabel(posX, posY, scaleX,scaleY, lbl_LogoPhone, e);
        }

        private void drowCode(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            Graphics gh = e.Graphics;
            //gh.DrawImage(lbl_code.Image, e.MarginBounds.Left + (posX + lbl_code.Location.X), 
            //             (posY + lbl_code.Location.Y), lbl_code.Width, lbl_code.Height);
            //gh.DrawImage(BarCode.BuildBarCode("20120002"),e.MarginBounds.Left + (posX + lbl_code.Location.X)*scaleX, 
            //             (posY + lbl_code.Location.Y)*scaleY, lbl_code.Width*scaleX, lbl_code.Height*scaleY);
            gh.DrawImage(lbl_code.Image, m_printArea.X + (posX + lbl_code.Location.X) * scaleX,
                         m_printArea.Y + (posY + lbl_code.Location.Y) * scaleY, lbl_code.Image.Width * scaleX, lbl_code.Image.Height * scaleY);

        }

        private void drowTitle(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            drowLabel(posX, posY, scaleX, scaleY, lbl_Title, e);
        }

        private void drowOrderInfo(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            drowLabel(posX, posY, scaleX, scaleY, lbl_orderDate, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_orderDateContent, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_orderTime, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_orderTimeContent, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_page, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_pageContent, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_orderID, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_orderIDContent, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_saler, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_salerContent, e);
        }

        private void drawCustomerInfo(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            drowLabel(posX, posY, scaleX, scaleY, lbl_custName, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_custNameContent, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_custNumber, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_custNumberContent, e);
            //drowLabel(posX, posY, scaleX, scaleY, lbl_custEffectDate, e);
            //drowLabel(posX, posY, scaleX, scaleY, lbl_custEffectDateContent, e);
        }

        private void drawSignAndBeizhu(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            drowLabel(posX, posY, scaleX, scaleY, lbl_beizhu, e);
            //drowLabel(posX, posY, scaleX, scaleY, lbl_signature, e);
            //drowLabel(posX, posY, scaleX, scaleY, lbl_signLine, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_salerSign, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_salerSignLine, e);
            //drowLabel(lbl_anuncment1, e);
        }

        private void drawAnancement(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            // 由于lbl_anuncment1字符过长需要特殊处理。
            Brush b = new SolidBrush(Color.Black);
            Pen aPen = new Pen(b);
            Font txtFont = lbl_anuncment1.Font;
            string text = lbl_anuncment1.Text;
            Graphics gh = e.Graphics;
            SizeF sz = gh.MeasureString(text, txtFont);
            
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            gh.DrawString(text, txtFont, b,
                new RectangleF(m_printArea.X + (posX + lbl_anuncment1.Location.X) * scaleX,
                    m_printArea.Y + (posY + lbl_anuncment1.Location.Y) * scaleY, 
                    lbl_anuncment1.Width * scaleX, 
                    lbl_anuncment1.Height * scaleY), 
                    format);
            //gh.DrawRectangle(aPen, new Rectangle(lbl_anuncment1.Location.X, lbl_anuncment1.Location.Y, lbl_anuncment1.Width, lbl_anuncment1.Height));


            drowLabel(posX, posY, scaleX, scaleY, lbl_anuncment2, e);
        }

        private void drawTotalAmount(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            //drowLabel(posX, posY, scaleX, scaleY, lbl_AmountSigleLine, e);
            drawLine(posX, posY, scaleX, scaleY, lbl_AmountSigleLine.Location.X, lbl_AmountSigleLine.Location.Y + lbl_AmountSigleLine.Height, lbl_AmountSigleLine.Width, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_Amount, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_AmountContent, e);
            drawLine(posX, posY, scaleX, scaleY, lbl_AmountSigleLine.Location.X, lbl_Amount.Location.Y + lbl_Amount.Height + 3, lbl_AmountSigleLine.Width, e);
        }

        private void drawTotalCount(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            //drowLabel(posX, posY, scaleX, scaleY, lbl_AmountSigleLine, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_count, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_CountContent, e);
        }

        private void drawColumnHeader(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            int orginX = (int)(m_printArea.X + (dgv_dingjindan.Location.X + posX) * scaleX);
            int orginY = (int)(m_printArea.Y + (dgv_dingjindan.Location.Y + posY) * scaleY);
            Point grid_origin = new Point(orginX, orginY);
            int iLeftMargin = 0;//(int)m_printArea.X;

            
            string text = this.dgv_dingjindan.Columns[0].HeaderText;
            
            int hdwidth = (int)(dgv_dingjindan.Columns[0].Width * scaleX);
            int hdheight = (int)(dgv_dingjindan.ColumnHeadersHeight*scaleY);
            DataGridViewCellStyle cellStyle = dgv_dingjindan.ColumnHeadersDefaultCellStyle;

            Brush b = new SolidBrush(Color.Black);
            Font headerFont = cellStyle.Font;
            Graphics gh = e.Graphics;

            
            //gh.DrawString(text, headerFont, b, new RectangleF(e.MarginBounds.Left + hd_origin.X, hd_origin.Y, hdwidth, hdheight));
            Pen aPen = new Pen(b);
            aPen.Width = 2;

            for (int i = 0; i < dgv_dingjindan.ColumnCount; i++)
            {
                text = dgv_dingjindan.Columns[i].HeaderText;
                hdwidth = (int)(dgv_dingjindan.Columns[i].Width*scaleX);
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                //format.LineAlignment = StringAlignment.Near;
                format.Alignment = StringAlignment.Near;
                if (i == 2 || i == 4 || i == 5 || i == 6)
                {
                    //format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Far;
                }
                gh.DrawString(text, headerFont, b, new RectangleF(iLeftMargin + grid_origin.X, grid_origin.Y, hdwidth, hdheight),format);
                gh.DrawLine(aPen, iLeftMargin + grid_origin.X, grid_origin.Y + hdheight, iLeftMargin +  grid_origin.X + hdwidth - 2, grid_origin.Y + hdheight);
                iLeftMargin += hdwidth;
            }
        }

        private void drawRows(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            int originX = (int)(m_printArea.X + (dgv_dingjindan.Location.X + posX) * scaleX);
            int originY = (int)(m_printArea.Y + (dgv_dingjindan.Location.Y + posY) * scaleY);
            Point grid_origin = new Point(originX, originY);
            int iLeftMargin = 0;//(int)m_printArea.X;
            int iTopMargin = (int)(dgv_dingjindan.ColumnHeadersHeight*scaleY);
            int cWidth = 0;
            int rHeight = 0;
            string text = "";

            DataGridViewCellStyle cellStyle = dgv_dingjindan.DefaultCellStyle;
            Font cellFont = cellStyle.Font;
            Brush b = new SolidBrush(Color.Black);

            Graphics gh = e.Graphics;
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Center;

            int row_index = m_currentPageIndex * m_maxRowPerPage;
            int count = 0;
            if (hasNextPage())
            {
                count = row_index + m_maxRowPerPage;
            }
            else
            {
                count = dgv_dingjindan.RowCount;
            }
            for (; row_index < count; row_index++)
            {
                DataGridViewRow row = dgv_dingjindan.Rows[row_index];
                rHeight = (int)(row.Height*scaleY);
                iLeftMargin = 0;

                for (int j = 0; j < dgv_dingjindan.ColumnCount; j++)
                {
                    cWidth = (int)(dgv_dingjindan.Columns[j].Width*scaleX);
                    text = row.Cells[j].Value.ToString();
                    if (j == 2 || j == 4 || j == 5 || j == 6)
                    {
                        format.Alignment = StringAlignment.Far;
                        if (j == 4 || j == 6)
                        {
                            decimal price = (decimal)row.Cells[j].Value;
                            int intv = decimal.ToInt32(price);
                            text = intv.ToString("f2");
                        }
                    }
                    else
                    {
                        format.Alignment = StringAlignment.Near;
                    }

                    gh.DrawString(text, cellFont, b, new RectangleF(iLeftMargin + grid_origin.X, iTopMargin + grid_origin.Y, cWidth, rHeight), format);
                    iLeftMargin += cWidth;
                }

                iTopMargin += rHeight;
            }
        }

        // 
        /// <summary>
        /// 该函数按照一定的比例，将控件的大小转换成纸张上的大小。
        /// </summary>
        /// <param name="posX">控件在窗体上的X方向的相对偏移量</param>
        /// <param name="posY">控件在窗体上的Y方向的相对偏移量</param>
        /// <param name="scaleX">控件转换为打印纸张时的X方向的缩放倍数</param>
        /// <param name="scaleY">控件转换为打印纸张时的Y方向的缩放倍数</param>
        /// <param name="lbl">需要打印的控件</param>
        /// <param name="e">打印机事件</param>
        private void drowLabel(int posX, int posY, float scaleX, float scaleY, Label lbl, PrintPageEventArgs e)
        {
            Brush b = new SolidBrush(Color.Black);
            Font txtFont = lbl.Font;
            string text = lbl.Text;
            Graphics gh = e.Graphics;
            SizeF sz = gh.MeasureString(text, txtFont);
            float width = (sz.Width > lbl.Width*scaleX) ? sz.Width : lbl.Width*scaleX;
            float height = (sz.Height > lbl.Height*scaleY) ? sz.Height : lbl.Height*scaleY;

            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            gh.DrawString(text, txtFont, b, new RectangleF(m_printArea.X + (posX + lbl.Location.X) * scaleX, m_printArea.Y + (posY + lbl.Location.Y) * scaleY, width, height), format);
        }

        /// <summary>
        /// 在指定的位置，按照一定的比例画一条水平直线
        /// </summary>
        /// <param name="posX">点X方向的相对偏移量</param>
        /// <param name="posY">点Y方向的相对偏移量</param>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <param name="length"></param>
        /// <param name="e"></param>
        /// <param name="x">点的X坐标</param>
        /// <param name="y">点的Y坐标</param>
        private void drawLine(int posX, int posY, float scaleX, float scaleY, int x,int y, int length, PrintPageEventArgs e)
        {
            float scale_length = length * scaleX;

            Brush brush = new SolidBrush(Color.Black);
            
            Graphics gh = e.Graphics;
            Pen aPen = new Pen(brush);
            //aPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            gh.DrawLine(aPen, m_printArea.X + (posX + x) * scaleX, m_printArea.Y + (posY + y) * scaleY, 
                m_printArea.X + (posX + x) * scaleX + scale_length, m_printArea.Y + (posY + y) * scaleY);
        }

        // 该函数按照控件的实际大小打印在纸张上
        private void drowLabel(int posX, int posY, Label lbl, PrintPageEventArgs e)
        {
            drowLabel(posX, posY, 1, 1, lbl, e);
        }

        private void FrmPrint_Load(object sender, EventArgs e)
        {
            Bitmap bmp = BarCode.BuildBarCode(lbl_orderIDContent.Text);
            lbl_code.Image = bmp;
            CenterToScreen();
        }

        private void binding()
        {
            this.dgv_dingjindan.AutoGenerateColumns = false;
            dgv_dingjindan.DataSource = m_DetailList;

            dgv_dingjindan.Columns["dingJinDanCode"].DataPropertyName = "Code";
            dgv_dingjindan.Columns["dingJinDanDescription"].DataPropertyName = "description";
            dgv_dingjindan.Columns["dingJinDanUnits"].DataPropertyName = "Units";
            dgv_dingjindan.Columns["dingJinDanBottle"].DataPropertyName = "Bottle";
            dgv_dingjindan.Columns["dingJinDanMemberprice"].DataPropertyName = "Price";//"memberprice";
            dgv_dingjindan.Columns["dingJinDanDiscount"].DataPropertyName = "Discount";
            dgv_dingjindan.Columns["dingJinDanAmount"].DataPropertyName = "Amount";
        }

        private void addDetail(CartDetailRowData data)
        {
            m_DetailList.Add(data);
        }

        

        public DataGridView CartDetaiGrid
        {
            get { return m_CartDetaiGrid; }
            set { 
                m_CartDetaiGrid = value;
                foreach (DataGridViewRow row in m_CartDetaiGrid.Rows)
                {
                    addDetail(row.DataBoundItem as CartDetailRowData);
                }
                binding();
            }
        }

        //public Customer CartCustomer
        //{
        //    //get { return m_customer; }
        //    set { m_customer = value; }
        //}

        public void SetCustomer(Customer aCustomer)
        {
            m_customer = aCustomer;
            showCustomer();
        }

        public DateTime OrderTime
        {
            get { return m_orderTime; }
            set 
            { 
                m_orderTime = value; 
                
            }
        }

        public Order Order
        {
            get { return m_order; }
            set 
            {
                m_order = value;
                
                showOrder();
                // Customer
                SetCustomer(Customer.findByID(Order.CustomerID));
                // User
                User aUser = User.find_by_id(Order.UserID);
                User = aUser;

                // order_detail
                // generateOrderDetail
                List<OrderDetail> details = m_order.getDetails();
                m_DetailList.Clear();
                int total_units = 0;
                foreach (OrderDetail detail in details)
                {
                    CartDetailRowData data = new CartDetailRowData();
                    
                    data.Code = detail.Code;
                    data.Discount = detail.Discount;
                    data.Price = detail.KnockDownPrice * 100 / detail.Discount;
                    data.Units = detail.Units;
                    total_units += detail.Units;
                    Wine aWine = Wine.findByCode(detail.Code);
                    if (aWine != null)
                    {
                        data.Bottle = aWine.Bottle;//"BT";//aWine.Bottle;
                        data.Description = aWine.Description;
                    }
                    addDetail(data);
                }
                m_order_total_units = total_units;
                binding();
                showOrderAmount();
                showOrderCount();
                resetGlobalSettings();
            }
        }

        public User User
        {
            get { return m_user; }
            set 
            { 
                m_user = value;
                showUser();
            }
        }

        private void showCustomer()
        {
            if (m_customer != null)
            {
                lbl_custNameContent.Text = m_customer.Name;
                lbl_custNumberContent.Text = m_customer.PhoneNumber;
                //lbl_custEffectDateContent.Text = this.m_order.EffectDate.ToShortDateString();
            }
        }

        private void showOrder()
        {
            if (m_order != null)
            {
                lbl_orderDateContent.Text = m_order.EffectDate.ToShortDateString();
                lbl_orderTimeContent.Text = m_order.EffectDate.ToShortTimeString();
                lbl_orderIDContent.Text = m_order.ID.ToString("D8");
            }
        }

        private void showUser()
        {
            if (m_user != null)
            {
                string user = "";
                if (m_user.Alias != null && m_user.Alias != "")
                {
                    user = m_user.Alias;
                }
                else
                {
                    user = m_user.Name;
                }
                lbl_salerContent.Text = user;
            }
        }

        private void showOrderAmount()
        {
            lbl_Amount.Visible = true;
            lbl_AmountContent.Visible = true;
            lbl_AmountContent.Text = "￥"+ Order.Amount.ToString();
        }

        private void showOrderCount()
        {
            lbl_CountContent.Text = m_order_total_units.ToString();
        }

        private void showCurrentPage()
        {
            // 
            string pageStr = (m_currentPageIndex+1).ToString() + "/" + m_totalPageCount.ToString() + "页";
            lbl_pageContent.Text = pageStr;
        }

        private void calcPages()
        {
            m_totalPageCount = (m_DetailList.Count - 1) / m_maxRowPerPage + 1;
        }

        private void resetGlobalSettings()
        {
            calcPages();
            m_currentPageIndex = 0;
            showCurrentPage();
        }

        private bool hasNextPage()
        {
            return (m_currentPageIndex + 1) * this.m_maxRowPerPage < dgv_dingjindan.RowCount;
        }

        private void printNextPage(PrintPageEventArgs e)
        {
            if (hasNextPage())
            {
                e.HasMorePages = true;
                m_currentPageIndex++;
            }
            else
            {
                e.HasMorePages = false;
                resetGlobalSettings();
            }
        }

        private void dgv_dingjindan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dgv_dingjindan.Columns[e.ColumnIndex].Name == "dingJinDanMemberprice" || dgv_dingjindan.Columns[e.ColumnIndex].Name == "dingJinDanAmount")
                {
                    decimal price = (decimal)e.Value;
                    int intv = decimal.ToInt32(price);
                    e.Value = intv.ToString("f2");
                }
                
            }
            catch (Exception ex)
            {
                
            }
        }
        
    }
}
