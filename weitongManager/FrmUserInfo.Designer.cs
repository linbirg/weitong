namespace weitongManager
{
    partial class FrmNewUser
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
            this.tBox_userName = new System.Windows.Forms.TextBox();
            this.tBox_newPasswd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tBox_confirmPasswd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tBox_Aliase = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tBox_Email = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbBox_Role = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_regDate = new System.Windows.Forms.Label();
            this.btn_userOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // tBox_userName
            // 
            this.tBox_userName.Location = new System.Drawing.Point(114, 58);
            this.tBox_userName.Name = "tBox_userName";
            this.tBox_userName.Size = new System.Drawing.Size(100, 21);
            this.tBox_userName.TabIndex = 1;
            this.tBox_userName.Leave += new System.EventHandler(this.tBox_userName_Leave);
            // 
            // tBox_newPasswd
            // 
            this.tBox_newPasswd.Location = new System.Drawing.Point(114, 99);
            this.tBox_newPasswd.Name = "tBox_newPasswd";
            this.tBox_newPasswd.PasswordChar = '*';
            this.tBox_newPasswd.Size = new System.Drawing.Size(100, 21);
            this.tBox_newPasswd.TabIndex = 3;
            this.tBox_newPasswd.Leave += new System.EventHandler(this.tBox_newPasswd_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "新密码：";
            // 
            // tBox_confirmPasswd
            // 
            this.tBox_confirmPasswd.Location = new System.Drawing.Point(114, 141);
            this.tBox_confirmPasswd.Name = "tBox_confirmPasswd";
            this.tBox_confirmPasswd.PasswordChar = '*';
            this.tBox_confirmPasswd.Size = new System.Drawing.Size(100, 21);
            this.tBox_confirmPasswd.TabIndex = 5;
            this.tBox_confirmPasswd.Leave += new System.EventHandler(this.tBox_confirmPasswd_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "确认密码：";
            // 
            // tBox_Aliase
            // 
            this.tBox_Aliase.Location = new System.Drawing.Point(114, 186);
            this.tBox_Aliase.Name = "tBox_Aliase";
            this.tBox_Aliase.Size = new System.Drawing.Size(100, 21);
            this.tBox_Aliase.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "姓名：";
            // 
            // tBox_Email
            // 
            this.tBox_Email.Location = new System.Drawing.Point(114, 230);
            this.tBox_Email.Name = "tBox_Email";
            this.tBox_Email.Size = new System.Drawing.Size(100, 21);
            this.tBox_Email.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(55, 233);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "Email：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(263, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "角色：";
            // 
            // cmbBox_Role
            // 
            this.cmbBox_Role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_Role.FormattingEnabled = true;
            this.cmbBox_Role.Location = new System.Drawing.Point(322, 141);
            this.cmbBox_Role.Name = "cmbBox_Role";
            this.cmbBox_Role.Size = new System.Drawing.Size(121, 20);
            this.cmbBox_Role.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(249, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "注册日期：";
            // 
            // lbl_regDate
            // 
            this.lbl_regDate.AutoSize = true;
            this.lbl_regDate.Location = new System.Drawing.Point(320, 195);
            this.lbl_regDate.Name = "lbl_regDate";
            this.lbl_regDate.Size = new System.Drawing.Size(65, 12);
            this.lbl_regDate.TabIndex = 15;
            this.lbl_regDate.Text = "2012-05-18";
            // 
            // btn_userOk
            // 
            this.btn_userOk.Location = new System.Drawing.Point(175, 287);
            this.btn_userOk.Name = "btn_userOk";
            this.btn_userOk.Size = new System.Drawing.Size(129, 27);
            this.btn_userOk.TabIndex = 16;
            this.btn_userOk.Text = "确定";
            this.btn_userOk.UseVisualStyleBackColor = true;
            this.btn_userOk.Click += new System.EventHandler(this.btn_userOk_Click);
            // 
            // FrmNewUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 371);
            this.Controls.Add(this.btn_userOk);
            this.Controls.Add(this.lbl_regDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbBox_Role);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tBox_Email);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tBox_Aliase);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tBox_confirmPasswd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tBox_newPasswd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tBox_userName);
            this.Controls.Add(this.label1);
            this.Name = "FrmNewUser";
            this.Text = "创建用户";
            this.Load += new System.EventHandler(this.FrmUserInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBox_userName;
        private System.Windows.Forms.TextBox tBox_newPasswd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBox_confirmPasswd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBox_Aliase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tBox_Email;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbBox_Role;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_regDate;
        private System.Windows.Forms.Button btn_userOk;
    }
}