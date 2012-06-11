namespace weitongManager
{
    partial class FrmPrint
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrint));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbl_Logo = new System.Windows.Forms.Label();
            this.lbl_orderDate = new System.Windows.Forms.Label();
            this.lbl_orderTime = new System.Windows.Forms.Label();
            this.lbl_page = new System.Windows.Forms.Label();
            this.lbl_orderID = new System.Windows.Forms.Label();
            this.lbl_saler = new System.Windows.Forms.Label();
            this.lbl_orderDateContent = new System.Windows.Forms.Label();
            this.lbl_orderTimeContent = new System.Windows.Forms.Label();
            this.lbl_pageContent = new System.Windows.Forms.Label();
            this.lbl_orderIDContent = new System.Windows.Forms.Label();
            this.lbl_salerContent = new System.Windows.Forms.Label();
            this.lbl_Title = new System.Windows.Forms.Label();
            this.lbl_code = new System.Windows.Forms.Label();
            this.lbl_custName = new System.Windows.Forms.Label();
            this.lbl_custNumber = new System.Windows.Forms.Label();
            this.dgv_dingjindan = new System.Windows.Forms.DataGridView();
            this.dingJinDanCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dingJinDanDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dingJinDanUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dingJinDanBottle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dingJinDanMemberprice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dingJinDanDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dingJinDanAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbl_custNameContent = new System.Windows.Forms.Label();
            this.lbl_custNumberContent = new System.Windows.Forms.Label();
            this.lbl_beizhu = new System.Windows.Forms.Label();
            this.lbl_salerSign = new System.Windows.Forms.Label();
            this.lbl_salerSignLine = new System.Windows.Forms.Label();
            this.lbl_anuncment1 = new System.Windows.Forms.Label();
            this.lbl_anuncment2 = new System.Windows.Forms.Label();
            this.lbl_logoAdress = new System.Windows.Forms.Label();
            this.btn_Print = new System.Windows.Forms.Button();
            this.btn_Preview = new System.Windows.Forms.Button();
            this.lbl_Amount = new System.Windows.Forms.Label();
            this.lbl_AmountContent = new System.Windows.Forms.Label();
            this.lbl_AmountSigleLine = new System.Windows.Forms.Label();
            this.lbl_CountContent = new System.Windows.Forms.Label();
            this.lbl_count = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_dingjindan)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Logo
            // 
            this.lbl_Logo.Image = ((System.Drawing.Image)(resources.GetObject("lbl_Logo.Image")));
            this.lbl_Logo.Location = new System.Drawing.Point(265, 47);
            this.lbl_Logo.Name = "lbl_Logo";
            this.lbl_Logo.Size = new System.Drawing.Size(211, 47);
            this.lbl_Logo.TabIndex = 2;
            // 
            // lbl_orderDate
            // 
            this.lbl_orderDate.AutoSize = true;
            this.lbl_orderDate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_orderDate.Location = new System.Drawing.Point(635, 138);
            this.lbl_orderDate.Name = "lbl_orderDate";
            this.lbl_orderDate.Size = new System.Drawing.Size(44, 12);
            this.lbl_orderDate.TabIndex = 4;
            this.lbl_orderDate.Text = "日期：";
            this.lbl_orderDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_orderTime
            // 
            this.lbl_orderTime.AutoSize = true;
            this.lbl_orderTime.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_orderTime.Location = new System.Drawing.Point(635, 158);
            this.lbl_orderTime.Name = "lbl_orderTime";
            this.lbl_orderTime.Size = new System.Drawing.Size(44, 12);
            this.lbl_orderTime.TabIndex = 5;
            this.lbl_orderTime.Text = "时间：";
            this.lbl_orderTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_page
            // 
            this.lbl_page.AutoSize = true;
            this.lbl_page.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_page.Location = new System.Drawing.Point(635, 178);
            this.lbl_page.Name = "lbl_page";
            this.lbl_page.Size = new System.Drawing.Size(44, 12);
            this.lbl_page.TabIndex = 6;
            this.lbl_page.Text = "页次：";
            // 
            // lbl_orderID
            // 
            this.lbl_orderID.AutoSize = true;
            this.lbl_orderID.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_orderID.Location = new System.Drawing.Point(635, 198);
            this.lbl_orderID.Name = "lbl_orderID";
            this.lbl_orderID.Size = new System.Drawing.Size(44, 12);
            this.lbl_orderID.TabIndex = 7;
            this.lbl_orderID.Text = "编号：";
            // 
            // lbl_saler
            // 
            this.lbl_saler.AutoSize = true;
            this.lbl_saler.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_saler.Location = new System.Drawing.Point(610, 218);
            this.lbl_saler.Name = "lbl_saler";
            this.lbl_saler.Size = new System.Drawing.Size(70, 12);
            this.lbl_saler.TabIndex = 8;
            this.lbl_saler.Text = "销售人员：";
            // 
            // lbl_orderDateContent
            // 
            this.lbl_orderDateContent.AutoSize = true;
            this.lbl_orderDateContent.Location = new System.Drawing.Point(686, 138);
            this.lbl_orderDateContent.Name = "lbl_orderDateContent";
            this.lbl_orderDateContent.Size = new System.Drawing.Size(65, 12);
            this.lbl_orderDateContent.TabIndex = 9;
            this.lbl_orderDateContent.Text = "2012.04.24";
            // 
            // lbl_orderTimeContent
            // 
            this.lbl_orderTimeContent.AutoSize = true;
            this.lbl_orderTimeContent.Location = new System.Drawing.Point(686, 158);
            this.lbl_orderTimeContent.Name = "lbl_orderTimeContent";
            this.lbl_orderTimeContent.Size = new System.Drawing.Size(35, 12);
            this.lbl_orderTimeContent.TabIndex = 10;
            this.lbl_orderTimeContent.Text = "23:00";
            // 
            // lbl_pageContent
            // 
            this.lbl_pageContent.AutoSize = true;
            this.lbl_pageContent.Location = new System.Drawing.Point(686, 178);
            this.lbl_pageContent.Name = "lbl_pageContent";
            this.lbl_pageContent.Size = new System.Drawing.Size(11, 12);
            this.lbl_pageContent.TabIndex = 11;
            this.lbl_pageContent.Text = "1";
            // 
            // lbl_orderIDContent
            // 
            this.lbl_orderIDContent.AutoSize = true;
            this.lbl_orderIDContent.Location = new System.Drawing.Point(686, 198);
            this.lbl_orderIDContent.Name = "lbl_orderIDContent";
            this.lbl_orderIDContent.Size = new System.Drawing.Size(53, 12);
            this.lbl_orderIDContent.TabIndex = 12;
            this.lbl_orderIDContent.Text = "20120002";
            // 
            // lbl_salerContent
            // 
            this.lbl_salerContent.AutoSize = true;
            this.lbl_salerContent.Location = new System.Drawing.Point(686, 218);
            this.lbl_salerContent.Name = "lbl_salerContent";
            this.lbl_salerContent.Size = new System.Drawing.Size(29, 12);
            this.lbl_salerContent.TabIndex = 13;
            this.lbl_salerContent.Text = "文通";
            // 
            // lbl_Title
            // 
            this.lbl_Title.AutoSize = true;
            this.lbl_Title.Font = new System.Drawing.Font("SimSun", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Title.Location = new System.Drawing.Point(314, 123);
            this.lbl_Title.Name = "lbl_Title";
            this.lbl_Title.Size = new System.Drawing.Size(98, 21);
            this.lbl_Title.TabIndex = 14;
            this.lbl_Title.Text = "销售清单";
            this.lbl_Title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_code
            // 
            this.lbl_code.Location = new System.Drawing.Point(12, 112);
            this.lbl_code.Name = "lbl_code";
            this.lbl_code.Size = new System.Drawing.Size(166, 58);
            this.lbl_code.TabIndex = 15;
            // 
            // lbl_custName
            // 
            this.lbl_custName.AutoSize = true;
            this.lbl_custName.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_custName.Location = new System.Drawing.Point(10, 178);
            this.lbl_custName.Name = "lbl_custName";
            this.lbl_custName.Size = new System.Drawing.Size(66, 12);
            this.lbl_custName.TabIndex = 16;
            this.lbl_custName.Text = "致     ：";
            this.lbl_custName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_custNumber
            // 
            this.lbl_custNumber.AutoSize = true;
            this.lbl_custNumber.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_custNumber.Location = new System.Drawing.Point(10, 198);
            this.lbl_custNumber.Name = "lbl_custNumber";
            this.lbl_custNumber.Size = new System.Drawing.Size(70, 12);
            this.lbl_custNumber.TabIndex = 17;
            this.lbl_custNumber.Text = "联系电话：";
            this.lbl_custNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgv_dingjindan
            // 
            this.dgv_dingjindan.AllowUserToAddRows = false;
            this.dgv_dingjindan.AllowUserToDeleteRows = false;
            this.dgv_dingjindan.AllowUserToResizeColumns = false;
            this.dgv_dingjindan.AllowUserToResizeRows = false;
            this.dgv_dingjindan.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgv_dingjindan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_dingjindan.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_dingjindan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_dingjindan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dingJinDanCode,
            this.dingJinDanDescription,
            this.dingJinDanUnits,
            this.dingJinDanBottle,
            this.dingJinDanMemberprice,
            this.dingJinDanDiscount,
            this.dingJinDanAmount});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_dingjindan.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_dingjindan.GridColor = System.Drawing.SystemColors.Control;
            this.dgv_dingjindan.Location = new System.Drawing.Point(12, 243);
            this.dgv_dingjindan.MultiSelect = false;
            this.dgv_dingjindan.Name = "dgv_dingjindan";
            this.dgv_dingjindan.ReadOnly = true;
            this.dgv_dingjindan.RowHeadersVisible = false;
            this.dgv_dingjindan.RowTemplate.Height = 20;
            this.dgv_dingjindan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_dingjindan.Size = new System.Drawing.Size(763, 186);
            this.dgv_dingjindan.TabIndex = 19;
            // 
            // dingJinDanCode
            // 
            this.dingJinDanCode.HeaderText = "编号";
            this.dingJinDanCode.Name = "dingJinDanCode";
            this.dingJinDanCode.ReadOnly = true;
            // 
            // dingJinDanDescription
            // 
            this.dingJinDanDescription.HeaderText = "名称";
            this.dingJinDanDescription.Name = "dingJinDanDescription";
            this.dingJinDanDescription.ReadOnly = true;
            this.dingJinDanDescription.Width = 280;
            // 
            // dingJinDanUnits
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dingJinDanUnits.DefaultCellStyle = dataGridViewCellStyle1;
            this.dingJinDanUnits.HeaderText = "数量";
            this.dingJinDanUnits.Name = "dingJinDanUnits";
            this.dingJinDanUnits.ReadOnly = true;
            this.dingJinDanUnits.Width = 55;
            // 
            // dingJinDanBottle
            // 
            this.dingJinDanBottle.HeaderText = "单位";
            this.dingJinDanBottle.Name = "dingJinDanBottle";
            this.dingJinDanBottle.ReadOnly = true;
            this.dingJinDanBottle.Width = 55;
            // 
            // dingJinDanMemberprice
            // 
            this.dingJinDanMemberprice.HeaderText = "单价(￥)";
            this.dingJinDanMemberprice.Name = "dingJinDanMemberprice";
            this.dingJinDanMemberprice.ReadOnly = true;
            // 
            // dingJinDanDiscount
            // 
            this.dingJinDanDiscount.HeaderText = "折扣(%)";
            this.dingJinDanDiscount.Name = "dingJinDanDiscount";
            this.dingJinDanDiscount.ReadOnly = true;
            this.dingJinDanDiscount.Width = 70;
            // 
            // dingJinDanAmount
            // 
            this.dingJinDanAmount.HeaderText = "金额(￥)";
            this.dingJinDanAmount.Name = "dingJinDanAmount";
            this.dingJinDanAmount.ReadOnly = true;
            // 
            // lbl_custNameContent
            // 
            this.lbl_custNameContent.AutoSize = true;
            this.lbl_custNameContent.Location = new System.Drawing.Point(93, 178);
            this.lbl_custNameContent.Name = "lbl_custNameContent";
            this.lbl_custNameContent.Size = new System.Drawing.Size(41, 12);
            this.lbl_custNameContent.TabIndex = 20;
            this.lbl_custNameContent.Text = "王嘉梁";
            // 
            // lbl_custNumberContent
            // 
            this.lbl_custNumberContent.AutoSize = true;
            this.lbl_custNumberContent.Location = new System.Drawing.Point(93, 198);
            this.lbl_custNumberContent.Name = "lbl_custNumberContent";
            this.lbl_custNumberContent.Size = new System.Drawing.Size(71, 12);
            this.lbl_custNumberContent.TabIndex = 21;
            this.lbl_custNumberContent.Text = "15999092384";
            // 
            // lbl_beizhu
            // 
            this.lbl_beizhu.AutoSize = true;
            this.lbl_beizhu.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_beizhu.Location = new System.Drawing.Point(10, 462);
            this.lbl_beizhu.Name = "lbl_beizhu";
            this.lbl_beizhu.Size = new System.Drawing.Size(44, 12);
            this.lbl_beizhu.TabIndex = 23;
            this.lbl_beizhu.Text = "备注：";
            this.lbl_beizhu.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_salerSign
            // 
            this.lbl_salerSign.AutoSize = true;
            this.lbl_salerSign.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_salerSign.Location = new System.Drawing.Point(496, 547);
            this.lbl_salerSign.Name = "lbl_salerSign";
            this.lbl_salerSign.Size = new System.Drawing.Size(70, 12);
            this.lbl_salerSign.TabIndex = 25;
            this.lbl_salerSign.Text = "贵方签署：";
            this.lbl_salerSign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_salerSignLine
            // 
            this.lbl_salerSignLine.AutoSize = true;
            this.lbl_salerSignLine.Location = new System.Drawing.Point(572, 547);
            this.lbl_salerSignLine.Name = "lbl_salerSignLine";
            this.lbl_salerSignLine.Size = new System.Drawing.Size(167, 12);
            this.lbl_salerSignLine.TabIndex = 27;
            this.lbl_salerSignLine.Text = "___________________________";
            // 
            // lbl_anuncment1
            // 
            this.lbl_anuncment1.Location = new System.Drawing.Point(10, 577);
            this.lbl_anuncment1.Name = "lbl_anuncment1";
            this.lbl_anuncment1.Size = new System.Drawing.Size(757, 28);
            this.lbl_anuncment1.TabIndex = 28;
            this.lbl_anuncment1.Text = "*By signing this list, customer acknowledges receipt of the above product item(s)" +
    " in good(or acceptable) condition, and agrees that no refund is allowed.";
            this.lbl_anuncment1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_anuncment2
            // 
            this.lbl_anuncment2.AutoSize = true;
            this.lbl_anuncment2.Location = new System.Drawing.Point(106, 605);
            this.lbl_anuncment2.Name = "lbl_anuncment2";
            this.lbl_anuncment2.Size = new System.Drawing.Size(461, 12);
            this.lbl_anuncment2.TabIndex = 29;
            this.lbl_anuncment2.Text = "客户签收即表示确认收妥此单所列货品且货品均处于良好（或可接受）状态及不能退款";
            // 
            // lbl_logoAdress
            // 
            this.lbl_logoAdress.AutoSize = true;
            this.lbl_logoAdress.Location = new System.Drawing.Point(184, 97);
            this.lbl_logoAdress.Name = "lbl_logoAdress";
            this.lbl_logoAdress.Size = new System.Drawing.Size(317, 12);
            this.lbl_logoAdress.TabIndex = 30;
            this.lbl_logoAdress.Text = "上海市哈尔滨路160号1913老洋行A102  Tel：021-63561196";
            // 
            // btn_Print
            // 
            this.btn_Print.Location = new System.Drawing.Point(14, 12);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Size = new System.Drawing.Size(75, 27);
            this.btn_Print.TabIndex = 32;
            this.btn_Print.Text = "打印";
            this.btn_Print.UseVisualStyleBackColor = true;
            this.btn_Print.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btn_Preview
            // 
            this.btn_Preview.Location = new System.Drawing.Point(116, 12);
            this.btn_Preview.Name = "btn_Preview";
            this.btn_Preview.Size = new System.Drawing.Size(75, 27);
            this.btn_Preview.TabIndex = 33;
            this.btn_Preview.Text = "预览";
            this.btn_Preview.UseVisualStyleBackColor = true;
            this.btn_Preview.Click += new System.EventHandler(this.btn_Preview_Click);
            // 
            // lbl_Amount
            // 
            this.lbl_Amount.AutoSize = true;
            this.lbl_Amount.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Amount.Location = new System.Drawing.Point(678, 445);
            this.lbl_Amount.Name = "lbl_Amount";
            this.lbl_Amount.Size = new System.Drawing.Size(44, 12);
            this.lbl_Amount.TabIndex = 34;
            this.lbl_Amount.Text = "合计：";
            // 
            // lbl_AmountContent
            // 
            this.lbl_AmountContent.Location = new System.Drawing.Point(720, 445);
            this.lbl_AmountContent.Name = "lbl_AmountContent";
            this.lbl_AmountContent.Size = new System.Drawing.Size(55, 12);
            this.lbl_AmountContent.TabIndex = 35;
            this.lbl_AmountContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_AmountSigleLine
            // 
            this.lbl_AmountSigleLine.AutoSize = true;
            this.lbl_AmountSigleLine.BackColor = System.Drawing.Color.Transparent;
            this.lbl_AmountSigleLine.Location = new System.Drawing.Point(569, 430);
            this.lbl_AmountSigleLine.Name = "lbl_AmountSigleLine";
            this.lbl_AmountSigleLine.Size = new System.Drawing.Size(203, 12);
            this.lbl_AmountSigleLine.TabIndex = 36;
            this.lbl_AmountSigleLine.Text = "_________________________________";
            // 
            // lbl_CountContent
            // 
            this.lbl_CountContent.Location = new System.Drawing.Point(620, 445);
            this.lbl_CountContent.Name = "lbl_CountContent";
            this.lbl_CountContent.Size = new System.Drawing.Size(55, 12);
            this.lbl_CountContent.TabIndex = 38;
            this.lbl_CountContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_count
            // 
            this.lbl_count.AutoSize = true;
            this.lbl_count.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_count.Location = new System.Drawing.Point(569, 445);
            this.lbl_count.Name = "lbl_count";
            this.lbl_count.Size = new System.Drawing.Size(57, 12);
            this.lbl_count.TabIndex = 37;
            this.lbl_count.Text = "总数量：";
            // 
            // FrmPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 629);
            this.Controls.Add(this.lbl_CountContent);
            this.Controls.Add(this.lbl_count);
            this.Controls.Add(this.lbl_AmountSigleLine);
            this.Controls.Add(this.lbl_AmountContent);
            this.Controls.Add(this.lbl_Amount);
            this.Controls.Add(this.btn_Preview);
            this.Controls.Add(this.btn_Print);
            this.Controls.Add(this.lbl_logoAdress);
            this.Controls.Add(this.lbl_anuncment2);
            this.Controls.Add(this.lbl_anuncment1);
            this.Controls.Add(this.lbl_salerSignLine);
            this.Controls.Add(this.lbl_salerSign);
            this.Controls.Add(this.lbl_beizhu);
            this.Controls.Add(this.lbl_custNumberContent);
            this.Controls.Add(this.lbl_custNameContent);
            this.Controls.Add(this.dgv_dingjindan);
            this.Controls.Add(this.lbl_custNumber);
            this.Controls.Add(this.lbl_custName);
            this.Controls.Add(this.lbl_code);
            this.Controls.Add(this.lbl_Title);
            this.Controls.Add(this.lbl_salerContent);
            this.Controls.Add(this.lbl_orderIDContent);
            this.Controls.Add(this.lbl_pageContent);
            this.Controls.Add(this.lbl_orderTimeContent);
            this.Controls.Add(this.lbl_orderDateContent);
            this.Controls.Add(this.lbl_saler);
            this.Controls.Add(this.lbl_orderID);
            this.Controls.Add(this.lbl_page);
            this.Controls.Add(this.lbl_orderTime);
            this.Controls.Add(this.lbl_orderDate);
            this.Controls.Add(this.lbl_Logo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPrint";
            this.Text = "打印预览";
            this.Load += new System.EventHandler(this.FrmPrint_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmPrint_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_dingjindan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Logo;
        private System.Windows.Forms.Label lbl_orderDate;
        private System.Windows.Forms.Label lbl_orderTime;
        private System.Windows.Forms.Label lbl_page;
        private System.Windows.Forms.Label lbl_orderID;
        private System.Windows.Forms.Label lbl_saler;
        private System.Windows.Forms.Label lbl_orderDateContent;
        private System.Windows.Forms.Label lbl_orderTimeContent;
        private System.Windows.Forms.Label lbl_pageContent;
        private System.Windows.Forms.Label lbl_orderIDContent;
        private System.Windows.Forms.Label lbl_salerContent;
        private System.Windows.Forms.Label lbl_Title;
        private System.Windows.Forms.Label lbl_code;
        private System.Windows.Forms.Label lbl_custName;
        private System.Windows.Forms.Label lbl_custNumber;
        private System.Windows.Forms.DataGridView dgv_dingjindan;
        private System.Windows.Forms.Label lbl_custNameContent;
        private System.Windows.Forms.Label lbl_custNumberContent;
        private System.Windows.Forms.Label lbl_beizhu;
        private System.Windows.Forms.Label lbl_salerSign;
        private System.Windows.Forms.Label lbl_salerSignLine;
        private System.Windows.Forms.Label lbl_anuncment1;
        private System.Windows.Forms.Label lbl_anuncment2;
        private System.Windows.Forms.Label lbl_logoAdress;
        private System.Windows.Forms.Button btn_Print;
        private System.Windows.Forms.Button btn_Preview;
        private System.Windows.Forms.Label lbl_Amount;
        private System.Windows.Forms.Label lbl_AmountContent;
        private System.Windows.Forms.Label lbl_AmountSigleLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn dingJinDanCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dingJinDanDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn dingJinDanUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn dingJinDanBottle;
        private System.Windows.Forms.DataGridViewTextBoxColumn dingJinDanMemberprice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dingJinDanDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn dingJinDanAmount;
        private System.Windows.Forms.Label lbl_CountContent;
        private System.Windows.Forms.Label lbl_count;
    }
}