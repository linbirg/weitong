namespace weitongManager
{
    partial class FrmGenerateCode
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
            this.lbl_Code = new System.Windows.Forms.Label();
            this.btn_Generate = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tBox_code1 = new System.Windows.Forms.TextBox();
            this.tBox_code2 = new System.Windows.Forms.TextBox();
            this.tBox_code3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "编码：";
            // 
            // lbl_Code
            // 
            this.lbl_Code.AutoSize = true;
            this.lbl_Code.Location = new System.Drawing.Point(102, 87);
            this.lbl_Code.Name = "lbl_Code";
            this.lbl_Code.Size = new System.Drawing.Size(77, 12);
            this.lbl_Code.TabIndex = 1;
            this.lbl_Code.Text = "12位随即编码";
            // 
            // btn_Generate
            // 
            this.btn_Generate.Location = new System.Drawing.Point(63, 111);
            this.btn_Generate.Name = "btn_Generate";
            this.btn_Generate.Size = new System.Drawing.Size(75, 23);
            this.btn_Generate.TabIndex = 2;
            this.btn_Generate.Text = "Generate";
            this.btn_Generate.UseVisualStyleBackColor = true;
            this.btn_Generate.Click += new System.EventHandler(this.btn_Generate_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(178, 111);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 3;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "新鲜出炉，欢迎使用！";
            // 
            // tBox_code1
            // 
            this.tBox_code1.Enabled = false;
            this.tBox_code1.Location = new System.Drawing.Point(100, 60);
            this.tBox_code1.Name = "tBox_code1";
            this.tBox_code1.Size = new System.Drawing.Size(57, 21);
            this.tBox_code1.TabIndex = 5;
            this.tBox_code1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tBox_code2
            // 
            this.tBox_code2.Enabled = false;
            this.tBox_code2.Location = new System.Drawing.Point(163, 60);
            this.tBox_code2.Name = "tBox_code2";
            this.tBox_code2.Size = new System.Drawing.Size(57, 21);
            this.tBox_code2.TabIndex = 6;
            this.tBox_code2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tBox_code3
            // 
            this.tBox_code3.Enabled = false;
            this.tBox_code3.Location = new System.Drawing.Point(226, 60);
            this.tBox_code3.Name = "tBox_code3";
            this.tBox_code3.Size = new System.Drawing.Size(57, 21);
            this.tBox_code3.TabIndex = 7;
            this.tBox_code3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrmGenerateCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 215);
            this.Controls.Add(this.tBox_code3);
            this.Controls.Add(this.tBox_code2);
            this.Controls.Add(this.tBox_code1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_Generate);
            this.Controls.Add(this.lbl_Code);
            this.Controls.Add(this.label1);
            this.Name = "FrmGenerateCode";
            this.Text = "生成编码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Code;
        private System.Windows.Forms.Button btn_Generate;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBox_code1;
        private System.Windows.Forms.TextBox tBox_code2;
        private System.Windows.Forms.TextBox tBox_code3;
    }
}