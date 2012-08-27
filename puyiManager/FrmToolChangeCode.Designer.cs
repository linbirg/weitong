namespace weitongManager
{
    partial class FrmToolChangeCode
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckb_order_wines = new System.Windows.Forms.CheckBox();
            this.ckb_his_storage = new System.Windows.Forms.CheckBox();
            this.ckb_storage = new System.Windows.Forms.CheckBox();
            this.ckb_wines = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tBox_OldCode = new System.Windows.Forms.TextBox();
            this.tBox_newCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_ChangeCode = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(455, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "编码是酒的唯一性标识，是酒最重要的属性；改变编码可能带来数据丢失、数据不匹配等各种严重的问题；请您确认知道您正在做的事情的意义。";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckb_order_wines);
            this.groupBox1.Controls.Add(this.ckb_his_storage);
            this.groupBox1.Controls.Add(this.ckb_storage);
            this.groupBox1.Controls.Add(this.ckb_wines);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(15, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(453, 121);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "需要修改的表";
            // 
            // ckb_order_wines
            // 
            this.ckb_order_wines.AutoSize = true;
            this.ckb_order_wines.Location = new System.Drawing.Point(84, 93);
            this.ckb_order_wines.Name = "ckb_order_wines";
            this.ckb_order_wines.Size = new System.Drawing.Size(15, 14);
            this.ckb_order_wines.TabIndex = 7;
            this.ckb_order_wines.UseVisualStyleBackColor = true;
            // 
            // ckb_his_storage
            // 
            this.ckb_his_storage.AutoSize = true;
            this.ckb_his_storage.Location = new System.Drawing.Point(84, 67);
            this.ckb_his_storage.Name = "ckb_his_storage";
            this.ckb_his_storage.Size = new System.Drawing.Size(15, 14);
            this.ckb_his_storage.TabIndex = 6;
            this.ckb_his_storage.UseVisualStyleBackColor = true;
            // 
            // ckb_storage
            // 
            this.ckb_storage.AutoSize = true;
            this.ckb_storage.Location = new System.Drawing.Point(84, 44);
            this.ckb_storage.Name = "ckb_storage";
            this.ckb_storage.Size = new System.Drawing.Size(15, 14);
            this.ckb_storage.TabIndex = 5;
            this.ckb_storage.UseVisualStyleBackColor = true;
            // 
            // ckb_wines
            // 
            this.ckb_wines.AutoSize = true;
            this.ckb_wines.Location = new System.Drawing.Point(84, 21);
            this.ckb_wines.Name = "ckb_wines";
            this.ckb_wines.Size = new System.Drawing.Size(15, 14);
            this.ckb_wines.TabIndex = 4;
            this.ckb_wines.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "order_wines";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "his_storage";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "storage";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "wines";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 204);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "原编码：";
            // 
            // tBox_OldCode
            // 
            this.tBox_OldCode.Location = new System.Drawing.Point(81, 201);
            this.tBox_OldCode.Name = "tBox_OldCode";
            this.tBox_OldCode.Size = new System.Drawing.Size(108, 21);
            this.tBox_OldCode.TabIndex = 9;
            // 
            // tBox_newCode
            // 
            this.tBox_newCode.Location = new System.Drawing.Point(267, 201);
            this.tBox_newCode.Name = "tBox_newCode";
            this.tBox_newCode.Size = new System.Drawing.Size(108, 21);
            this.tBox_newCode.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(208, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "新编码：";
            // 
            // btn_ChangeCode
            // 
            this.btn_ChangeCode.Location = new System.Drawing.Point(163, 241);
            this.btn_ChangeCode.Name = "btn_ChangeCode";
            this.btn_ChangeCode.Size = new System.Drawing.Size(75, 23);
            this.btn_ChangeCode.TabIndex = 12;
            this.btn_ChangeCode.Text = "修改";
            this.btn_ChangeCode.UseVisualStyleBackColor = true;
            this.btn_ChangeCode.Click += new System.EventHandler(this.btn_ChangeCode_Click);
            // 
            // FrmToolChangeCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 304);
            this.Controls.Add(this.btn_ChangeCode);
            this.Controls.Add(this.tBox_newCode);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tBox_OldCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "FrmToolChangeCode";
            this.Text = "编码修改";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ckb_his_storage;
        private System.Windows.Forms.CheckBox ckb_storage;
        private System.Windows.Forms.CheckBox ckb_wines;
        private System.Windows.Forms.CheckBox ckb_order_wines;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tBox_OldCode;
        private System.Windows.Forms.TextBox tBox_newCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_ChangeCode;
    }
}