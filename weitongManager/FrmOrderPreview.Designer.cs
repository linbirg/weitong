namespace weitongManager
{
    partial class FrmOrderPreview
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_total = new System.Windows.Forms.Label();
            this.lbl_custAddress = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_custEmail = new System.Windows.Forms.Label();
            this.lbl_custRegisterDay = new System.Windows.Forms.Label();
            this.lbl_custPhone = new System.Windows.Forms.Label();
            this.lbl_customerBirthday = new System.Windows.Forms.Label();
            this.lbl_customerJob = new System.Windows.Forms.Label();
            this.lbl_membLevel = new System.Windows.Forms.Label();
            this.lbl_customerName = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.dgv_currentOrder = new System.Windows.Forms.DataGridView();
            this.currentOrderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentOrderDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentOrderKnockDownPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentOrderFavorablePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentOrderUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentOrderAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currentOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_total);
            this.groupBox1.Controls.Add(this.lbl_custAddress);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbl_custEmail);
            this.groupBox1.Controls.Add(this.lbl_custRegisterDay);
            this.groupBox1.Controls.Add(this.lbl_custPhone);
            this.groupBox1.Controls.Add(this.lbl_customerBirthday);
            this.groupBox1.Controls.Add(this.lbl_customerJob);
            this.groupBox1.Controls.Add(this.lbl_membLevel);
            this.groupBox1.Controls.Add(this.lbl_customerName);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.label31);
            this.groupBox1.Controls.Add(this.label33);
            this.groupBox1.Controls.Add(this.label34);
            this.groupBox1.Controls.Add(this.label35);
            this.groupBox1.Controls.Add(this.label36);
            this.groupBox1.Controls.Add(this.label37);
            this.groupBox1.Controls.Add(this.label38);
            this.groupBox1.Location = new System.Drawing.Point(12, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 120);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "客户信息";
            // 
            // lbl_total
            // 
            this.lbl_total.AutoSize = true;
            this.lbl_total.Location = new System.Drawing.Point(659, 95);
            this.lbl_total.Name = "lbl_total";
            this.lbl_total.Size = new System.Drawing.Size(11, 12);
            this.lbl_total.TabIndex = 25;
            this.lbl_total.Text = "0";
            // 
            // lbl_custAddress
            // 
            this.lbl_custAddress.AutoSize = true;
            this.lbl_custAddress.Location = new System.Drawing.Point(77, 95);
            this.lbl_custAddress.Name = "lbl_custAddress";
            this.lbl_custAddress.Size = new System.Drawing.Size(0, 12);
            this.lbl_custAddress.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(612, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "总金额：";
            // 
            // lbl_custEmail
            // 
            this.lbl_custEmail.AutoSize = true;
            this.lbl_custEmail.Location = new System.Drawing.Point(274, 61);
            this.lbl_custEmail.Name = "lbl_custEmail";
            this.lbl_custEmail.Size = new System.Drawing.Size(0, 12);
            this.lbl_custEmail.TabIndex = 22;
            // 
            // lbl_custRegisterDay
            // 
            this.lbl_custRegisterDay.AutoSize = true;
            this.lbl_custRegisterDay.Location = new System.Drawing.Point(612, 63);
            this.lbl_custRegisterDay.Name = "lbl_custRegisterDay";
            this.lbl_custRegisterDay.Size = new System.Drawing.Size(0, 12);
            this.lbl_custRegisterDay.TabIndex = 21;
            // 
            // lbl_custPhone
            // 
            this.lbl_custPhone.AutoSize = true;
            this.lbl_custPhone.Location = new System.Drawing.Point(274, 33);
            this.lbl_custPhone.Name = "lbl_custPhone";
            this.lbl_custPhone.Size = new System.Drawing.Size(0, 12);
            this.lbl_custPhone.TabIndex = 20;
            // 
            // lbl_customerBirthday
            // 
            this.lbl_customerBirthday.AutoSize = true;
            this.lbl_customerBirthday.Location = new System.Drawing.Point(466, 63);
            this.lbl_customerBirthday.Name = "lbl_customerBirthday";
            this.lbl_customerBirthday.Size = new System.Drawing.Size(0, 12);
            this.lbl_customerBirthday.TabIndex = 19;
            // 
            // lbl_customerJob
            // 
            this.lbl_customerJob.AutoSize = true;
            this.lbl_customerJob.Location = new System.Drawing.Point(466, 33);
            this.lbl_customerJob.Name = "lbl_customerJob";
            this.lbl_customerJob.Size = new System.Drawing.Size(0, 12);
            this.lbl_customerJob.TabIndex = 18;
            // 
            // lbl_membLevel
            // 
            this.lbl_membLevel.AutoSize = true;
            this.lbl_membLevel.Location = new System.Drawing.Point(77, 61);
            this.lbl_membLevel.Name = "lbl_membLevel";
            this.lbl_membLevel.Size = new System.Drawing.Size(0, 12);
            this.lbl_membLevel.TabIndex = 17;
            // 
            // lbl_customerName
            // 
            this.lbl_customerName.AutoSize = true;
            this.lbl_customerName.Location = new System.Drawing.Point(77, 33);
            this.lbl_customerName.Name = "lbl_customerName";
            this.lbl_customerName.Size = new System.Drawing.Size(0, 12);
            this.lbl_customerName.TabIndex = 16;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(6, 95);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(41, 12);
            this.label30.TabIndex = 7;
            this.label30.Text = "地址：";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(203, 33);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(41, 12);
            this.label31.TabIndex = 6;
            this.label31.Text = "手机：";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(541, 63);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(65, 12);
            this.label33.TabIndex = 5;
            this.label33.Text = "注册日期：";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(395, 63);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(41, 12);
            this.label34.TabIndex = 4;
            this.label34.Text = "生日：";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(203, 61);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(47, 12);
            this.label35.TabIndex = 3;
            this.label35.Text = "email：";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(395, 33);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(41, 12);
            this.label36.TabIndex = 2;
            this.label36.Text = "职业：";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 61);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(65, 12);
            this.label37.TabIndex = 1;
            this.label37.Text = "会员级别：";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(6, 33);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(41, 12);
            this.label38.TabIndex = 0;
            this.label38.Text = "姓名：";
            // 
            // dgv_currentOrder
            // 
            this.dgv_currentOrder.AllowUserToAddRows = false;
            this.dgv_currentOrder.AllowUserToDeleteRows = false;
            this.dgv_currentOrder.AllowUserToResizeColumns = false;
            this.dgv_currentOrder.AllowUserToResizeRows = false;
            this.dgv_currentOrder.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgv_currentOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_currentOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.currentOrderCode,
            this.currentOrderDescription,
            this.currentOrderKnockDownPrice,
            this.currentOrderFavorablePrice,
            this.currentOrderUnits,
            this.currentOrderAmount});
            this.dgv_currentOrder.Location = new System.Drawing.Point(12, 127);
            this.dgv_currentOrder.MultiSelect = false;
            this.dgv_currentOrder.Name = "dgv_currentOrder";
            this.dgv_currentOrder.ReadOnly = true;
            this.dgv_currentOrder.RowHeadersVisible = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgv_currentOrder.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_currentOrder.RowTemplate.Height = 23;
            this.dgv_currentOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_currentOrder.Size = new System.Drawing.Size(759, 211);
            this.dgv_currentOrder.TabIndex = 5;
            // 
            // currentOrderCode
            // 
            this.currentOrderCode.HeaderText = "编号";
            this.currentOrderCode.Name = "currentOrderCode";
            this.currentOrderCode.ReadOnly = true;
            // 
            // currentOrderDescription
            // 
            this.currentOrderDescription.HeaderText = "描述";
            this.currentOrderDescription.Name = "currentOrderDescription";
            this.currentOrderDescription.ReadOnly = true;
            this.currentOrderDescription.Width = 250;
            // 
            // currentOrderKnockDownPrice
            // 
            this.currentOrderKnockDownPrice.HeaderText = "会员价";
            this.currentOrderKnockDownPrice.Name = "currentOrderKnockDownPrice";
            this.currentOrderKnockDownPrice.ReadOnly = true;
            // 
            // currentOrderFavorablePrice
            // 
            this.currentOrderFavorablePrice.HeaderText = "已优惠";
            this.currentOrderFavorablePrice.Name = "currentOrderFavorablePrice";
            this.currentOrderFavorablePrice.ReadOnly = true;
            // 
            // currentOrderUnits
            // 
            this.currentOrderUnits.HeaderText = "数量";
            this.currentOrderUnits.Name = "currentOrderUnits";
            this.currentOrderUnits.ReadOnly = true;
            // 
            // currentOrderAmount
            // 
            this.currentOrderAmount.HeaderText = "金额";
            this.currentOrderAmount.Name = "currentOrderAmount";
            this.currentOrderAmount.ReadOnly = true;
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(251, 366);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 7;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(409, 366);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 8;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // FrmOrderPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 403);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgv_currentOrder);
            this.Name = "FrmOrderPreview";
            this.Text = "FrmOrderPreview";
            this.Load += new System.EventHandler(this.FrmOrderPreview_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currentOrder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_custAddress;
        private System.Windows.Forms.Label lbl_custEmail;
        private System.Windows.Forms.Label lbl_custRegisterDay;
        private System.Windows.Forms.Label lbl_custPhone;
        private System.Windows.Forms.Label lbl_customerBirthday;
        private System.Windows.Forms.Label lbl_customerJob;
        private System.Windows.Forms.Label lbl_membLevel;
        private System.Windows.Forms.Label lbl_customerName;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.DataGridView dgv_currentOrder;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentOrderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentOrderDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentOrderKnockDownPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentOrderFavorablePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentOrderUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentOrderAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_total;
    }
}