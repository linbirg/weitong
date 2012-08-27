using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace weitongManager
{
    public partial class FrmHisStorage : Form
    {
        private string m_code = null;
        private BindingList<HisStorage> m_history = null;

        public FrmHisStorage()
        {
            InitializeComponent();
        }

        public String Wine
        {
            get { return m_code; }
            set { m_code = value; }
        }

        private void FrmHisStorage_Load(object sender, EventArgs e)
        {
            try
            {
                CenterToScreen();
                List<HisStorage> list = Storage.findHistoryByCode(Wine);
                if (list != null)
                {
                    m_history = new BindingList<HisStorage>(list);
                    bindingHistory();
                }
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

        private void bindingHistory()
        {
            dgv_hisStorage.AutoGenerateColumns = false;
            dgv_hisStorage.DataSource = m_history;

            dgv_hisStorage.Columns["HiStorageCode"].DataPropertyName = "Code";
            dgv_hisStorage.Columns["HiStorageDescription"].DataPropertyName = "Description";
            dgv_hisStorage.Columns["HiStorageSupplier"].DataPropertyName = "Supplier";
            dgv_hisStorage.Columns["HiStoragePrice"].DataPropertyName = "Price";
            dgv_hisStorage.Columns["HiStorageUnits"].DataPropertyName = "Units";
            dgv_hisStorage.Columns["HiStorageAmount"].DataPropertyName = "Amount";
            dgv_hisStorage.Columns["HiStorageDate"].DataPropertyName = "EffectDate";
        }
    }
}
