namespace weitongManager
{
    partial class FrmHisStorage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_hisStorage = new System.Windows.Forms.DataGridView();
            this.HiStorageCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HiStorageDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HiStorageSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HiStoragePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HiStorageUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HiStorageAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HiStorageDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_hisStorage)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_hisStorage
            // 
            this.dgv_hisStorage.AllowUserToAddRows = false;
            this.dgv_hisStorage.AllowUserToDeleteRows = false;
            this.dgv_hisStorage.AllowUserToResizeColumns = false;
            this.dgv_hisStorage.AllowUserToResizeRows = false;
            this.dgv_hisStorage.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgv_hisStorage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_hisStorage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HiStorageCode,
            this.HiStorageDescription,
            this.HiStorageSupplier,
            this.HiStoragePrice,
            this.HiStorageUnits,
            this.HiStorageAmount,
            this.HiStorageDate});
            this.dgv_hisStorage.Location = new System.Drawing.Point(3, 3);
            this.dgv_hisStorage.MultiSelect = false;
            this.dgv_hisStorage.Name = "dgv_hisStorage";
            this.dgv_hisStorage.ReadOnly = true;
            this.dgv_hisStorage.RowHeadersVisible = false;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgv_hisStorage.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_hisStorage.RowTemplate.Height = 23;
            this.dgv_hisStorage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_hisStorage.Size = new System.Drawing.Size(819, 258);
            this.dgv_hisStorage.TabIndex = 6;
            // 
            // HiStorageCode
            // 
            this.HiStorageCode.HeaderText = "编号";
            this.HiStorageCode.Name = "HiStorageCode";
            this.HiStorageCode.ReadOnly = true;
            // 
            // HiStorageDescription
            // 
            this.HiStorageDescription.HeaderText = "描述";
            this.HiStorageDescription.Name = "HiStorageDescription";
            this.HiStorageDescription.ReadOnly = true;
            this.HiStorageDescription.Width = 200;
            // 
            // HiStorageSupplier
            // 
            this.HiStorageSupplier.HeaderText = "供应商";
            this.HiStorageSupplier.Name = "HiStorageSupplier";
            this.HiStorageSupplier.ReadOnly = true;
            // 
            // HiStoragePrice
            // 
            this.HiStoragePrice.HeaderText = "进价";
            this.HiStoragePrice.Name = "HiStoragePrice";
            this.HiStoragePrice.ReadOnly = true;
            // 
            // HiStorageUnits
            // 
            this.HiStorageUnits.HeaderText = "数量";
            this.HiStorageUnits.Name = "HiStorageUnits";
            this.HiStorageUnits.ReadOnly = true;
            // 
            // HiStorageAmount
            // 
            this.HiStorageAmount.HeaderText = "金额";
            this.HiStorageAmount.Name = "HiStorageAmount";
            this.HiStorageAmount.ReadOnly = true;
            // 
            // HiStorageDate
            // 
            this.HiStorageDate.HeaderText = "日期";
            this.HiStorageDate.Name = "HiStorageDate";
            this.HiStorageDate.ReadOnly = true;
            // 
            // FrmHisStorage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 273);
            this.Controls.Add(this.dgv_hisStorage);
            this.MaximizeBox = false;
            this.Name = "FrmHisStorage";
            this.Text = "入库记录";
            this.Load += new System.EventHandler(this.FrmHisStorage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_hisStorage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_hisStorage;
        private System.Windows.Forms.DataGridViewTextBoxColumn HiStorageCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn HiStorageDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn HiStorageSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn HiStoragePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn HiStorageUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn HiStorageAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn HiStorageDate;
    }
}