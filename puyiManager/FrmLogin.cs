using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace weitongManager
{
    partial class FrmLogin : Form
    {
        private User m_user = null;

        public FrmLogin()
        {
            InitializeComponent();
        }

        public User getLoginUser()
        {
            return m_user;
        }


        private void WARNING(string msg)
        {
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                doAuthenticate();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void clear_error_msg()
        {
            lbl_error.Text = "";
        }

        private void tbox_userName_Enter(object sender, EventArgs e)
        {
            try
            {
                clear_error_msg();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_passwd_Enter(object sender, EventArgs e)
        {
            try
            {
                clear_error_msg();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tbox_userName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r') tBox_passwd.Focus();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_passwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    doAuthenticate();
                }
                else if (e.KeyChar == Convert.ToChar(27))
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void doAuthenticate()
        {
            if (User.authenticate(tbox_userName.Text, tBox_passwd.Text))
            {
                //FrmMain main = new FrmMain();
                m_user = User.find_by_name(tbox_userName.Text);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                lbl_error.Text = "*错误的用户名/密码";
            }
        }

        private void tBox_passwd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                clear_error_msg();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

    }
}
