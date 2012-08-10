using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace weitongManager
{
    partial class FrmCustomerList : Form
    {

        private BindingList<Customer> m_customers = null;
        private Customer m_selCustomer = null;

        private void binding()
        {
            this.dgv_customers.AutoGenerateColumns = false;
            dgv_customers.DataSource = m_customers;

            dgv_customers.Columns["customerNameColumn"].DataPropertyName = "Name";
            dgv_customers.Columns["customerPhoneColumn"].DataPropertyName = "PhoneNumber";
        }

        public FrmCustomerList()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void WARNING(string msg)
        {
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private DialogResult SelectionDlgShow(string msg)
        {
            return MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void FrmCustomerList_Load(object sender, EventArgs e)
        {
            try
            {
                loadCustomers();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void loadCustomers()
        {
            if (m_customers == null) m_customers = new BindingList<Customer>();
            m_customers.Clear();
            List<Customer> list = Customer.load();
            foreach (Customer aCust in list)
            {
                m_customers.Add(aCust);
            }

            binding();
        }

        private void dgv_customers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow curRow = dgv_customers.CurrentRow;
                Customer curCustomer = curRow.DataBoundItem as Customer;
                m_selCustomer = curCustomer;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        public Customer SelectedCustomer
        {
            get { return m_selCustomer; }
        }

        private void btn_findCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                //string phoneNumerPattern = @"^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$";
                string input = tBox_customerInfo.Text.Trim();
                if (input != "")
                {
                    Customer aCustomer = null;
                    if (util.isPhoneNumber(input))
                    {
                        aCustomer = Customer.findByPhNumber(input);
                    }
                    else
                    {
                        aCustomer = Customer.findByName(input);
                    }

                    if (aCustomer != null)
                    {
                        if (m_customers == null) m_customers = new BindingList<Customer>();
                        m_customers.Clear();
                        m_customers.Add(aCustomer);
                    }
                    else
                    {
                        WARNING("找不到客户信息！");
                    }
                }
                else
                {
                    loadCustomers();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }
    }
}
