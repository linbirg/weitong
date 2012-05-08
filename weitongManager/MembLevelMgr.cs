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

        // ===============属性=================
        public DataGridView MemberLevelGrid
        {
            get { return m_DGV_member_level; }
            set {
                m_DGV_member_level = value;
            }
        }

        public MemberLevel getMaxLevel()
        {
            return getMaxLevelFromList();
        }

        // 添加级别从大到小。
        public void addDefaultTopLevelInfo()
        {
            MemberLevel lvl = MemberLevel.NewMemberLevel();
            int topLv = MemberLevel.getTopLevel();
            if (topLv == -1)
            {
                lvl.Level = 1;
            }
            else
            {
                lvl.Level = topLv + 1;
            }
            MemberLevel.insertLevelInfo(lvl);
            m_level_list.Add(lvl);
        }

        //=========================私有方法===========================
        private MemberLevel getMaxLevelFromList()
        {
            MemberLevel max_level = null;
            if (m_level_list != null)
            {
                max_level = m_level_list[0];

                foreach (MemberLevel lv in m_level_list)
                {
                    if (lv.Level > max_level.Level)
                        max_level = lv;
                }
            }
            return max_level;
        }

        private void bindingData()
        {
            if (m_DGV_member_level == null) return;
            m_DGV_member_level.AutoGenerateColumns = false;
            m_DGV_member_level.DataSource = m_level_list;
            m_DGV_member_level.Columns["memlevelDataGridViewTextBoxColumn"].DataPropertyName = "Level";
            m_DGV_member_level.Columns["levelnameDataGridViewTextBoxColumn"].DataPropertyName = "Name";
            m_DGV_member_level.Columns["discountDataGridViewTextBoxColumn"].DataPropertyName = "Discount";
            m_DGV_member_level.Columns["minConsumptionTextBoxColumn"].DataPropertyName = "MinConsuption";
        }
    }
}
