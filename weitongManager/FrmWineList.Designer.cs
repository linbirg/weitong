namespace weitongManager
{
    partial class FrmWineList
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWineList));
            this.dgv_wineList = new System.Windows.Forms.DataGridView();
            this.codeDGVStorageInfoTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDGVStorageInfoTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chateauDGVStorageInfoTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vintageDGVStorageInfoTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appelltionDGVStorageInfoTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qualityDGVStorageInfoTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scoreDGVStorageInfoTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bottleDGVStorageInfoTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countryDGVStorageInfoTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.ctms_WineList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_rmove = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_wineList)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.ctms_WineList.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_wineList
            // 
            this.dgv_wineList.AllowUserToAddRows = false;
            this.dgv_wineList.AllowUserToDeleteRows = false;
            this.dgv_wineList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Honeydew;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgv_wineList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_wineList.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgv_wineList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_wineList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codeDGVStorageInfoTextBoxColumn,
            this.descriptionDGVStorageInfoTextBoxColumn,
            this.chateauDGVStorageInfoTextBoxColumn,
            this.vintageDGVStorageInfoTextBoxColumn,
            this.appelltionDGVStorageInfoTextBoxColumn,
            this.qualityDGVStorageInfoTextBoxColumn,
            this.scoreDGVStorageInfoTextBoxColumn,
            this.bottleDGVStorageInfoTextBoxColumn,
            this.countryDGVStorageInfoTextBoxColumn});
            this.dgv_wineList.ContextMenuStrip = this.ctms_WineList;
            this.dgv_wineList.Location = new System.Drawing.Point(0, 28);
            this.dgv_wineList.MultiSelect = false;
            this.dgv_wineList.Name = "dgv_wineList";
            this.dgv_wineList.ReadOnly = true;
            this.dgv_wineList.RowHeadersVisible = false;
            this.dgv_wineList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgv_wineList.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_wineList.RowTemplate.Height = 23;
            this.dgv_wineList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_wineList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_wineList.Size = new System.Drawing.Size(1205, 368);
            this.dgv_wineList.TabIndex = 3;
            // 
            // codeDGVStorageInfoTextBoxColumn
            // 
            this.codeDGVStorageInfoTextBoxColumn.HeaderText = "编号";
            this.codeDGVStorageInfoTextBoxColumn.Name = "codeDGVStorageInfoTextBoxColumn";
            this.codeDGVStorageInfoTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDGVStorageInfoTextBoxColumn
            // 
            this.descriptionDGVStorageInfoTextBoxColumn.HeaderText = "名称";
            this.descriptionDGVStorageInfoTextBoxColumn.Name = "descriptionDGVStorageInfoTextBoxColumn";
            this.descriptionDGVStorageInfoTextBoxColumn.ReadOnly = true;
            // 
            // chateauDGVStorageInfoTextBoxColumn
            // 
            this.chateauDGVStorageInfoTextBoxColumn.HeaderText = "酒庄";
            this.chateauDGVStorageInfoTextBoxColumn.Name = "chateauDGVStorageInfoTextBoxColumn";
            this.chateauDGVStorageInfoTextBoxColumn.ReadOnly = true;
            // 
            // vintageDGVStorageInfoTextBoxColumn
            // 
            this.vintageDGVStorageInfoTextBoxColumn.HeaderText = "年份";
            this.vintageDGVStorageInfoTextBoxColumn.Name = "vintageDGVStorageInfoTextBoxColumn";
            this.vintageDGVStorageInfoTextBoxColumn.ReadOnly = true;
            // 
            // appelltionDGVStorageInfoTextBoxColumn
            // 
            this.appelltionDGVStorageInfoTextBoxColumn.HeaderText = "产区";
            this.appelltionDGVStorageInfoTextBoxColumn.Name = "appelltionDGVStorageInfoTextBoxColumn";
            this.appelltionDGVStorageInfoTextBoxColumn.ReadOnly = true;
            // 
            // qualityDGVStorageInfoTextBoxColumn
            // 
            this.qualityDGVStorageInfoTextBoxColumn.HeaderText = "等级";
            this.qualityDGVStorageInfoTextBoxColumn.Name = "qualityDGVStorageInfoTextBoxColumn";
            this.qualityDGVStorageInfoTextBoxColumn.ReadOnly = true;
            // 
            // scoreDGVStorageInfoTextBoxColumn
            // 
            this.scoreDGVStorageInfoTextBoxColumn.HeaderText = "评分";
            this.scoreDGVStorageInfoTextBoxColumn.Name = "scoreDGVStorageInfoTextBoxColumn";
            this.scoreDGVStorageInfoTextBoxColumn.ReadOnly = true;
            // 
            // bottleDGVStorageInfoTextBoxColumn
            // 
            this.bottleDGVStorageInfoTextBoxColumn.HeaderText = "规格";
            this.bottleDGVStorageInfoTextBoxColumn.Name = "bottleDGVStorageInfoTextBoxColumn";
            this.bottleDGVStorageInfoTextBoxColumn.ReadOnly = true;
            // 
            // countryDGVStorageInfoTextBoxColumn
            // 
            this.countryDGVStorageInfoTextBoxColumn.HeaderText = "国家";
            this.countryDGVStorageInfoTextBoxColumn.Name = "countryDGVStorageInfoTextBoxColumn";
            this.countryDGVStorageInfoTextBoxColumn.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_save});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(929, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btn_save
            // 
            this.btn_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(36, 22);
            this.btn_save.Text = "保存";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // ctms_WineList
            // 
            this.ctms_WineList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_rmove});
            this.ctms_WineList.Name = "ctms_WineList";
            this.ctms_WineList.Size = new System.Drawing.Size(153, 48);
            // 
            // tsmi_rmove
            // 
            this.tsmi_rmove.Name = "tsmi_rmove";
            this.tsmi_rmove.Size = new System.Drawing.Size(152, 22);
            this.tsmi_rmove.Text = "&R移除";
            this.tsmi_rmove.Click += new System.EventHandler(this.tsmi_rmove_Click);
            // 
            // FrmWineList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 436);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dgv_wineList);
            this.Name = "FrmWineList";
            this.Text = "酒单";
            this.Load += new System.EventHandler(this.FrmWineList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_wineList)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ctms_WineList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_wineList;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDGVStorageInfoTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDGVStorageInfoTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chateauDGVStorageInfoTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vintageDGVStorageInfoTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn appelltionDGVStorageInfoTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qualityDGVStorageInfoTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scoreDGVStorageInfoTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bottleDGVStorageInfoTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countryDGVStorageInfoTextBoxColumn;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ContextMenuStrip ctms_WineList;
        private System.Windows.Forms.ToolStripMenuItem tsmi_rmove;
    }
}