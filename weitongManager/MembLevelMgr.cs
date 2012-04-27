using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace weitongManager
{
    class MembLevelMgr
    {
        private DataGridView m_DGV_member_level = null;
        private BindingList<MemberLevel> m_level_list = null;

        public void init()
        {
            m_level_list = new BindingList<MemberLevel>(MemberLevel.loadData());
            bindingData();
        }

        public void update2DB(MemberLevel levelInfo)
        {
            levelInfo.update2DB();
        }

        // 从数据库中删除，并从列表中移除。
        public void remove(MemberLevel levelInfo)
        {
            levelInfo.deleteFromDB();
            m_level_list.Remove(levelInfo);
        }

        public DataGridView MemberLevelGrid
        {
            get { return m_DGV_member_level; }
            set {
                m_DGV_member_level = value;
            }
        }

        private void bindingData()
        {
            if (m_DGV_member_level == null) return;
            m_DGV_member_level.AutoGenerateColumns = false;
            m_DGV_member_level.DataSource = m_level_list;
            m_DGV_member_level.Columns["memlevelDataGridViewTextBoxColumn"].DataPropertyName = "Level";
            m_DGV_member_level.Columns["levelnameDataGridViewTextBoxColumn"].DataPropertyName = "Name";
            m_DGV_member_level.Columns["discountDataGridViewTextBoxColumn"].DataPropertyName = "Discount";
        }
    }
}
