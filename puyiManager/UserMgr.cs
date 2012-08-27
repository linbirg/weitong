using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Windows.Forms;

namespace weitongManager
{
    class UserMgr
    {
        private BindingList<User> m_users = null;
        private DataGridView m_userGridView = null;

        public void init()
        {
            m_users = new BindingList<User>(User.loadData());
            //loadUsers();
            bindingGrid();
        }

        /// <summary>
        /// 重新加载数据并更新显示
        /// </summary>
        public void reLoadUsers()
        {
            m_users = new BindingList<User>(User.loadData());
            bindingGrid();
        }

        public void deleteUser(int user_id)
        {
            User user = findInUsersByID(user_id);
            if (user == null) return;
            m_users.Remove(user);
            user.delete();
            //m_userGridView.Refresh();
        }

        public DataGridView UserGridView
        {
            get { return m_userGridView; }
            set { m_userGridView = value; }
        }


        /// <summary>
        /// 设置grid列的数据属性，将其与User属性对应。
        /// </summary>
        private void bindingGrid()
        { 
            if (m_userGridView == null) return;
            m_userGridView.AutoGenerateColumns = false;
            m_userGridView.DataSource = m_users;
            m_userGridView.Columns["userGridNameColumn"].DataPropertyName = "Name";
            m_userGridView.Columns["userGridAliasNameColumn"].DataPropertyName = "Alias";
            m_userGridView.Columns["userGridRoleColumn"].DataPropertyName = "RoleID";
            m_userGridView.Columns["userGridEmailColumn"].DataPropertyName = "Email";
            m_userGridView.Columns["userGridRegDateColumn"].DataPropertyName = "RegisterDate";
        }

        /// <summary>
        /// 从用户列表中查找指定id的用户
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>null或者指定id的用户</returns>
        private User findInUsersByID(int user_id)
        {
            User aUser = null;
            if (m_users == null) return aUser;
            for (int i = 0; i < m_users.Count; i++)
            {
                if (m_users[i].ID == user_id)
                {
                    aUser = m_users[i];
                    break;
                }
            }

            return aUser;
        }
    }
}
