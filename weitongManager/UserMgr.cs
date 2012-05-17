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
            bindingGrid();
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
    }
}
