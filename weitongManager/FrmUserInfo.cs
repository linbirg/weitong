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
    partial class FrmNewUser : Form
    {
        private User m_curUser = null;
        private List<Role> m_rolesList = null;

        public FrmNewUser()
        {
            InitializeComponent();
            CenterToScreen();
        }


        public User User
        {
            get { return m_curUser; }
            set { m_curUser = value; }
        }

        private void FrmUserInfo_Load(object sender, EventArgs e)
        {
            try
            {
                m_rolesList = Role.loadData();
                if (m_rolesList != null)
                {
                    cmbBox_Role.Items.Clear();
                    foreach(Role role in m_rolesList)
                    {
                        cmbBox_Role.Items.Add(role);
                    }
                }

                showUserInfo(m_curUser);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void WARNING(string msg)
        {
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private DialogResult SelectionDlgShow(string msg)
        {
            return MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void showUserInfo(User aUser)
        {
            if (aUser != null)
            {
                lbl_regDate.Text = aUser.RegisterDate.ToShortDateString();
                if (aUser.ID >= 0)
                {
                    tBox_Aliase.Text = m_curUser.Alias;
                    tBox_Email.Text = m_curUser.Email;
                    tBox_userName.Text = m_curUser.Name;
                    // 对于已有用户，暂时设置其显示密码为“password”。
                    tBox_newPasswd.Text = "password";
                    tBox_confirmPasswd.Text = "password";
                    cmbBox_Role_setRole(Role.getNameByID(m_curUser.RoleID));
                }
                else
                {
                    cmbBox_Role_setDefaultRole();
                }
                
            }
            else
            {
                //showEmptyUser();
                cmbBox_Role_setDefaultRole();
                lbl_regDate.Text = DateTime.Now.ToShortDateString();
            }
        }

        private void cmbBox_Role_setDefaultRole()
        {
            cmbBox_Role_setRole("saler");
        }

        private void cmbBox_Role_setRole(string role_name)
        {
            if (m_rolesList == null) return;
            foreach (Role role in m_rolesList)
            {
                //默认设置为销售
                if (role.Name == role_name)
                {
                    cmbBox_Role.SelectedItem = role;
                    break;
                }
            }
        }

        /// <summary>
        /// 赋值给user.如果是修改用户，则会判断用户新密码是否和原密码一致，
        /// 用户可以选择不修改密码。
        /// </summary>
        /// <param name="aUser"></param>
        private void assignUser(User aUser)
        {
            if (aUser == null) return;
            aUser.Alias = tBox_Aliase.Text.Trim();
            aUser.Email = tBox_Email.Text.Trim();
            string passwd = tBox_newPasswd.Text.Trim();

            if (aUser.ID > 0) 
            {
                if (!User.authenticate(aUser.Name, passwd))
                {
                    if (DialogResult.Yes == SelectionDlgShow("新密码与原密码不一致，你确定要修改密码么？\r\t如果不修改请选择否"))
                    {
                        aUser.setPasswd(passwd);
                    }
                }
            }
            else
            {
                aUser.setPasswd(passwd);
            }
            aUser.Name = tBox_userName.Text.Trim();
            Role role = cmbBox_Role.SelectedItem as Role;
            aUser.RoleID = role.ID;
        }

        //private void btn_OK_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (checkInput())
        //        {
        //        //    assignUser(m_curUser);
        //        //    this.DialogResult = DialogResult.OK;
        //        }      
        //    }
        //    catch (Exception ex)
        //    {
        //        WARNING(ex.Message);
        //    }
        //}

        private void tBox_userName_Leave(object sender, EventArgs e)
        {
            try
            {
                string name = tBox_userName.Text.Trim();
                if (name == "")
                {
                    WARNING("用户名不能为空！");
                    tBox_userName.Focus();
                }
                User aUser = User.find_by_name(name);
                
                //用户已存在，并且不是当前用户，则用户名已经被占用。
                if ((aUser != null)&&(aUser.ID != m_curUser.ID))
                {
                    WARNING("用户名 " + name + " 已经存在！请重新输入！");
                    tBox_userName.Focus();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_newPasswd_Leave(object sender, EventArgs e)
        {
            try
            {
                string passwd = tBox_newPasswd.Text.Trim();
                if (passwd == "")
                {
                    WARNING("密码不能为空！");
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_confirmPasswd_Leave(object sender, EventArgs e)
        {
            try
            {
                string confirm = tBox_confirmPasswd.Text.Trim();
                string passwd = tBox_newPasswd.Text.Trim();
                if (confirm != passwd)
                {
                    WARNING("两次输入的密码不一致！");
                    //tBox_confirmPasswd.Focus();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private bool checkInput()
        {
            // 检查用户名
            string name = tBox_userName.Text.Trim();
            if (name == "")
            {
                WARNING("用户名不能为空！");
                
                return false;
            }

            User aUser = User.find_by_name(name);
            //用户已存在，用户名不可用。
            if ((aUser != null) && (aUser.ID != m_curUser.ID))
            {
                WARNING("用户名 " + name + " 已经存在！请重新输入！");
                tBox_userName.Focus();
                return false;
            }

            // 检查密码
            string passwd = tBox_newPasswd.Text.Trim();
            if (passwd == "")
            {
                WARNING("密码不能为空！");
                tBox_newPasswd.Focus();
                return false;
            }

            string confirm = tBox_confirmPasswd.Text.Trim();
            
            if (confirm != passwd)
            {
                WARNING("两次输入的密码不一致！");
                tBox_confirmPasswd.Focus();
                return false;
            }

            return true;
        }

        private void btn_userOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    assignUser(m_curUser);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }
    }
}
