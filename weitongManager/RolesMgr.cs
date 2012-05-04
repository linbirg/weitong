using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace weitongManager
{
    class RolesMgr
    {
        private DataGridView m_dgv_roles = null;
        private BindingList<Role> m_roles_list = null;

        // ===============属性=================
        public DataGridView RolesGrid
        {
            get { return m_dgv_roles; }
            set
            {
                m_dgv_roles = value;
            }
        }


        public void init()
        {
            m_roles_list = new BindingList<Role>(Role.loadData());
            bindingData();
        }

        public void updateDB(Role role)
        {
            if (role == null) return;
            role.save();
        }

        private void bindingData()
        {
            if (m_dgv_roles == null) return;
            m_dgv_roles.AutoGenerateColumns = false;
            m_dgv_roles.DataSource = m_roles_list;
            m_dgv_roles.Columns["roleNameTextBoxColumn"].DataPropertyName = "Name";
            m_dgv_roles.Columns["roleDiscountTextBoxColumn"].DataPropertyName = "Discount";
            
        }
    }
}
