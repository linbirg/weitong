namespace weitongManager
{
    partial class FrmCustomerList
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
            this.btn_findCustomer = new System.Windows.Forms.Button();
            this.tBox_customerInfo = new System.Windows.Forms.TextBox();
            this.dgv_customers = new System.Windows.Forms.DataGridView();
            this.customerNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerPhoneColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_customers)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_findCustomer);
            this.groupBox1.Controls.Add(this.tBox_customerInfo);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 53);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btn_findCustomer
            // 
            this.btn_findCustomer.Location = new System.Drawing.Point(213, 18);
            this.btn_findCustomer.Name = "btn_findCustomer";
            this.btn_findCustomer.Size = new System.Drawing.Size(75, 23);
            this.btn_findCustomer.TabIndex = 1;
            this.btn_findCustomer.Text = "查找";
            this.btn_findCustomer.UseVisualStyleBackColor = true;
            this.btn_findCustomer.Click += new System.EventHandler(this.btn_findCustomer_Click);
            // 
            // tBox_customerInfo
            // 
            this.tBox_customerInfo.Location = new System.Drawing.Point(11, 20);
            this.tBox_customerInfo.Name = "tBox_customerInfo";
            this.tBox_customerInfo.Size = new System.Drawing.Size(184, 21);
            this.tBox_customerInfo.TabIndex = 0;
            // 
            // dgv_customers
            // 
            this.dgv_customers.AllowUserToAddRows = false;
            this.dgv_customers.AllowUserToDeleteRows = false;
            this.dgv_customers.AllowUserToResizeColumns = false;
            this.dgv_customers.AllowUserToResizeRows = false;
            this.dgv_customers.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgv_customers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_customers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.customerNameColumn,
            this.customerPhoneColumn});
            this.dgv_customers.Location = new System.Drawing.Point(4, 63);
            this.dgv_customers.MultiSelect = false;
            this.dgv_customers.Name = "dgv_customers";
            this.dgv_customers.ReadOnly = true;
            this.dgv_customers.RowHeadersVisible = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgv_customers.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_customers.RowTemplate.Height = 23;
            this.dgv_customers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_customers.Size = new System.Drawing.Size(301, 323);
            this.dgv_customers.TabIndex = 1;
            this.dgv_customers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_customers_CellDoubleClick);
            // 
            // customerNameColumn
            // 
            this.customerNameColumn.HeaderText = "姓名";
            this.customerNameColumn.Name = "customerNameColumn";
            this.customerNameColumn.ReadOnly = true;
            this.customerNameColumn.Width = 130;
            // 
            // customerPhoneColumn
            // 
            this.customerPhoneColumn.HeaderText = "手机";
            this.customerPhoneColumn.Name = "customerPhoneColumn";
            this.customerPhoneColumn.ReadOnly = true;
            this.customerPhoneColumn.Width = 145;
            // 
            // FrmCustomerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 398);
            this.Controls.Add(this.dgv_customers);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmCustomerList";
            this.Text = "客户";
            this.Load += new System.EventHandler(this.FrmCustomerList_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_customers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_findCustomer;
        private System.Windows.Forms.TextBox tBox_customerInfo;
        private System.Windows.Forms.DataGridView dgv_customers;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerPhoneColumn;
    }
}