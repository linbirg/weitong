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
        public FrmPrint()
        {
            InitializeComponent();
            m_DetailList = new BindingList<CartDetailRowData>();
        }

        //PrintDocument类是实现打印功能的核心，它封装了打印有关的属性、事件、和方法
        PrintDocument printDocument = new PrintDocument();

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //printDocument.PrinterSettings可以获取或设置计算机默认打印相关属性或参数，如：printDocument.PrinterSettings.PrinterName获得默认打印机打印机名称
            //printDocument.DefaultPageSettings   //可以获取或设置打印页面参数信息、如是纸张大小，是否横向打印等
            
            //设置文档名
            printDocument.DocumentName = lbl_custNameContent.Text + " 订金单";//设置完后可在打印对话框及队列中显示（默认显示document）

            //设置纸张大小（可以不设置取，取默认设置）
            PaperSize ps = new PaperSize("Your Paper Name", 210, 297);
            ps.RawKind = 9; //如果是自定义纸张，就要大于118，（A4值为9，详细纸张类型与值的对照请看http://msdn.microsoft.com/zh-tw/library/system.drawing.printing.papersize.rawkind(v=vs.85).aspx）
            printDocument.DefaultPageSettings.PaperSize = ps;

            //打印开始前
            printDocument.BeginPrint += new PrintEventHandler(printDocument_BeginPrint);
            //打印输出（过程）
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            //打印结束
            printDocument.EndPrint += new PrintEventHandler(printDocument_EndPrint);

            //跳出打印对话框，提供打印参数可视化设置，如选择哪个打印机打印此文档等
            PrintDialog pd = new PrintDialog();
            pd.Document = printDocument;
            if (DialogResult.OK == pd.ShowDialog()) //如果确认，将会覆盖所有的打印参数设置
            {
                //页面设置对话框（可以不使用，其实PrintDialog对话框已提供页面设置）
                PageSetupDialog psd = new PageSetupDialog();
                psd.Document = printDocument;
                if (DialogResult.OK == psd.ShowDialog())
                {
                    ////打印预览
                    //PrintPreviewDialog ppd = new PrintPreviewDialog();
                    //ppd.Document = printDocument;
                    //if (DialogResult.OK == ppd.ShowDialog())
                        printDocument.Print();          //打印
                }
            }
        }

        private void btn_Preview_Click(object sender, EventArgs e)
        {
            //printDocument.PrinterSettings可以获取或设置计算机默认打印相关属性或参数，如：printDocument.PrinterSettings.PrinterName获得默认打印机打印机名称
            //printDocument.DefaultPageSettings   //可以获取或设置打印页面参数信息、如是纸张大小，是否横向打印等
            //Bitmap bmp = BarCode.BuildBarCode("20120004");
            //bmp.Save(@"D:\\tiaoxingma.bmp");
            //设置文档名
            printDocument.DocumentName = lbl_custNameContent.Text + " 订金单";//设置完后可在打印对话框及队列中显示（默认显示document）

            //设置纸张大小（可以不设置取，取默认设置）
            PaperSize ps = new PaperSize("Your Paper Name", 210, 297);
            ps.RawKind = 9; //如果是自定义纸张，就要大于118，（A4值为9，详细纸张类型与值的对照请看http://msdn.microsoft.com/zh-tw/library/system.drawing.printing.papersize.rawkind(v=vs.85).aspx）
            printDocument.DefaultPageSettings.PaperSize = ps;

            //打印开始前
            printDocument.BeginPrint += new PrintEventHandler(printDocument_BeginPrint);
            //打印输出（过程）
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            //打印结束
            printDocument.EndPrint += new PrintEventHandler(printDocument_EndPrint);

            //跳出打印对话框，提供打印参数可视化设置，如选择哪个打印机打印此文档等
            //PrintDialog pd = new PrintDialog();
            //pd.Document = printDocument;
            //if (DialogResult.OK == pd.ShowDialog()) //如果确认，将会覆盖所有的打印参数设置
            //{
                PaperSize p = null;
                foreach (PaperSize ps2 in printDocument.PrinterSettings.PaperSizes)
                {
                    if (ps.PaperName.Equals("A4"))//这里设置纸张大小,但必须是定义好的  
                        p = ps2;
                }

                printDocument.DefaultPageSettings.PaperSize = p;
                
                //打印预览
                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ppd.Document = printDocument;
                Form f = (Form)ppd;
                f.WindowState = FormWindowState.Maximized;
                if (DialogResult.OK == ppd.ShowDialog())
                    printDocument.Print();          //打印
                
            //}
        }

        void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            //也可以把一些打印的参数放在此处设置
        }

        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = lbl_Logo.Location.X;
            int y = this.Height - this.Bottom;
            int width = lbl_Logo.Width;
            int height = lbl_anuncment2.Bottom + 20;
            float scaleX = (e.PageBounds.Width - e.MarginBounds.Left * 2) / (float)this.DisplayRectangle.Width;
            
            // 页面对应到一半的纸张上面，只打印在上半部分。
            float scaleY = e.PageBounds.Height / ((float)(this.DisplayRectangle.Height - lbl_Logo.Location.Y) * 2);
            //打印啥东东就在这写了
            Graphics g = e.Graphics;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; 

            Bitmap   formBitmap   =   new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(formBitmap, new Rectangle(0, 0, this.Width, this.Height));
            //g.DrawImage(formBitmap, 0, 0);
            
            
            
            Brush b = new SolidBrush(Color.Black);
            Font titleFont = new Font("宋体", 16);
            string title = "五凤街道梅峰社区卫生服务站 处方笺";
            g.DrawString(title, titleFont, b, new PointF((e.PageBounds.Width - g.MeasureString(title, titleFont).Width) / 2, this.Height/2 + 300));
            
            //g.DrawImage(lbl_Logo.Image,e.MarginBounds.Left,0,e.PageBounds.Width-e.MarginBounds.Left*2,100);

            drowMiddleLine(e);
            drowLogo(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);
            drowCode(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);
            drowTitle(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);
            drowOrderInfo(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);
            drawCustomerInfo(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);
            drawSignAndBeizhu(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);
            drawAnancement(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);

            drawColumnHeader(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);
            drawRows(0, -lbl_Logo.Location.Y, scaleX, scaleY, e);

            //e.Cancel//获取或设置是否取消打印
            //e.HasMorePages    //为true时，该函数执行完毕后还会重新执行一遍（可用于动态分页）
        }

        void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            //打印结束后相关操作
        }

        private void FrmPrint_Paint(object sender, PaintEventArgs e)
        {
            
        }

        //各种drow
        private void drowMiddleLine(PrintPageEventArgs e)
        {
            // 中线分页，将A4一分为二
            SolidBrush brush = new SolidBrush(Color.Gray);
            Graphics gh = e.Graphics;
            Pen aPen = new Pen(brush);
            aPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            gh.DrawLine(aPen, 0, e.PageBounds.Height / 2, e.PageBounds.Width, e.PageBounds.Height / 2);
        }

        private void drowLogo(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            Graphics gh = e.Graphics;
            
            gh.DrawImage(lbl_Logo.Image, 
                e.MarginBounds.Left + (posX + lbl_Logo.Location.X) * scaleX, 
                (posY + lbl_Logo.Location.Y)*scaleY,
                lbl_Logo.Width*scaleX ,
                lbl_Logo.Height*scaleY);
            drowLabel(posX, posY, scaleX,scaleY, lbl_logoAdress, e);
            drowLabel(posX, posY, scaleX,scaleY, lbl_LogoPhone, e);
        }

        private void drowCode(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            Graphics gh = e.Graphics;
            //gh.DrawImage(lbl_code.Image, e.MarginBounds.Left + (posX + lbl_code.Location.X), 
            //             (posY + lbl_code.Location.Y), lbl_code.Width, lbl_code.Height);
            //gh.DrawImage(BarCode.BuildBarCode("20120002"),e.MarginBounds.Left + (posX + lbl_code.Location.X)*scaleX, 
            //             (posY + lbl_code.Location.Y)*scaleY, lbl_code.Width*scaleX, lbl_code.Height*scaleY);
            gh.DrawImage(lbl_code.Image, e.MarginBounds.Left + (posX + lbl_code.Location.X) * scaleX,
                         (posY + lbl_code.Location.Y) * scaleY, lbl_code.Image.Width * scaleX, lbl_code.Image.Height * scaleY);

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
            drowLabel(posX, posY, scaleX, scaleY, lbl_custEffectDate, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_custEffectDateContent, e);
        }

        private void drawSignAndBeizhu(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            drowLabel(posX, posY, scaleX, scaleY, lbl_beizhu, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_signature, e);
            drowLabel(posX, posY, scaleX, scaleY, lbl_signLine, e);
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
                new RectangleF(e.MarginBounds.Left + (posX + lbl_anuncment1.Location.X) * scaleX, 
                    (posY + lbl_anuncment1.Location.Y) * scaleY, 
                    lbl_anuncment1.Width * scaleX, 
                    lbl_anuncment1.Height * scaleY), 
                    format);
            //gh.DrawRectangle(aPen, new Rectangle(lbl_anuncment1.Location.X, lbl_anuncment1.Location.Y, lbl_anuncment1.Width, lbl_anuncment1.Height));


            drowLabel(posX, posY, scaleX, scaleY, lbl_anuncment2, e);
        }

        private void drawColumnHeader(int posX, int posY, float scaleX, float scaleY, PrintPageEventArgs e)
        {
            int orginX = (int)((dgv_dingjindan.Location.X + posX)*scaleX);
            int orginY = (int)((dgv_dingjindan.Location.Y + posY)*scaleY);
            Point grid_origin = new Point(orginX, orginY);
            int iLeftMargin = e.MarginBounds.Left;

            //Point hd_origin = dgv_dingjindan.Location;
            string text = this.dgv_dingjindan.Columns[0].HeaderText;
            //Rectangle rec = dgv_dingjindan.Columns[0].HeaderCell.ContentBounds;
            int hdwidth = (int)(dgv_dingjindan.Columns[0].Width * scaleX);
            int hdheight = (int)(dgv_dingjindan.ColumnHeadersHeight*scaleY);
            DataGridViewCellStyle cellStyle = dgv_dingjindan.ColumnHeadersDefaultCellStyle;

            Brush b = new SolidBrush(Color.Black);
            Font headerFont = cellStyle.Font;
            Graphics gh = e.Graphics;

            //SizeF sz = gh.MeasureString(text, headerFont);
            //float width = (sz.Width > hdwidth) ? sz.Width : hdwidth;
            //float height = (sz.Height > hdheight) ? sz.Height : hdheight;

            //gh.DrawString(text, headerFont, b, new RectangleF(e.MarginBounds.Left + hd_origin.X, hd_origin.Y, hdwidth, hdheight));
            Pen aPen = new Pen(b);
            aPen.Width = 2;
            //Point p1 = new Point(e.MarginBounds.Left + hd_origin.X, hd_origin.Y + hdheight);
            //Point p2 = new Point(e.MarginBounds.Left + hd_origin.X + hdwidth, hd_origin.Y + hdheight);
            //gh.DrawLine(aPen,p1,p2);

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
            int originX = (int)((dgv_dingjindan.Location.X + posX)*scaleX);
            int originY = (int)((dgv_dingjindan.Location.Y + posY)*scaleY);
            Point grid_origin = new Point(originX, originY);
            int iLeftMargin = e.MarginBounds.Left;
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
           

            for (int i = 0; i < dgv_dingjindan.RowCount; i++)
            {
                DataGridViewRow row = dgv_dingjindan.Rows[i];
                rHeight = (int)(row.Height*scaleY);
                iLeftMargin = e.MarginBounds.Left;

                for (int j = 0; j < dgv_dingjindan.ColumnCount; j++)
                {
                    cWidth = (int)(dgv_dingjindan.Columns[j].Width*scaleX);
                    text = row.Cells[j].Value.ToString();
                    if (j == 2 || j == 4 || j == 5 || j == 6)
                    {
                        format.Alignment = StringAlignment.Far;
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

        // 该函数按照一定的比例，将控件的大小转换成纸张上的大小。
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


            gh.DrawString(text, txtFont, b, new RectangleF(e.MarginBounds.Left + ( posX+ lbl.Location.X)*scaleX, (posY + lbl.Location.Y)*scaleY, width, height), format);

            //Pen aPen = new Pen(b);
            //Point p10 = lbl.Location;
            //Point p11 = new Point(p10.X+ lbl.Width,p10.Y);
            //Point p20 = new Point(p10.X, p10.Y + lbl.Height);
            //Point p21 = new Point(p20.X + lbl.Width, p20.Y);
            //gh.DrawLine(aPen, p10, p11);
            //gh.DrawLine(aPen, p10, p20);
            //gh.DrawLine(aPen, p11, p21);
            //gh.DrawLine(aPen, p20, p21);
        }

        // 该函数按照控件的实际大小打印在纸张上
        private void drowLabel(int posX, int posY, Label lbl, PrintPageEventArgs e)
        {
            drowLabel(posX, posY, 1, 1, lbl, e);
        }

        private void FrmPrint_Load(object sender, EventArgs e)
        {
            Bitmap bmp = BarCode.BuildBarCode("2012000004");
            lbl_code.Image = bmp;
        }

        private void binding()
        {
            this.dgv_dingjindan.AutoGenerateColumns = false;
            dgv_dingjindan.DataSource = m_DetailList;

            dgv_dingjindan.Columns["dingJinDanCode"].DataPropertyName = "Code";
            dgv_dingjindan.Columns["dingJinDanDescription"].DataPropertyName = "description";
            dgv_dingjindan.Columns["dingJinDanUnits"].DataPropertyName = "Units";
            dgv_dingjindan.Columns["dingJinDanBottle"].DataPropertyName = "Bottle";
            dgv_dingjindan.Columns["dingJinDanMemberprice"].DataPropertyName = "memberprice";
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
                SetCustomer(Customer.findByID(Order.CustomerID));
                // generateOrderDetail
                List<OrderDetail> details = m_order.getDetails();
                m_DetailList.Clear();
                foreach (OrderDetail detail in details)
                {
                    CartDetailRowData data = new CartDetailRowData();
                    
                    data.Code = detail.Code;
                    data.Discount = detail.Discount;
                    data.Price = detail.KnockDownPrice * 100 / detail.Discount;
                    data.Units = detail.Units;
                    Wine aWine = Wine.findByCode(detail.Code);
                    if (aWine != null)
                    {
                        data.Bottle = aWine.Bottle;
                        data.Description = aWine.Description;
                    }
                    addDetail(data);
                }
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
                lbl_custEffectDateContent.Text = this.m_order.EffectDate.ToShortDateString();
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
                lbl_salerContent.Text = m_user.Name;
        }


        private BindingList<CartDetailRowData> m_DetailList = null;
        private DataGridView m_CartDetaiGrid = null;
        private Customer m_customer = null;
        private Order m_order = null;
        private User m_user = null;

        private DateTime m_orderTime;

        
    }
}
