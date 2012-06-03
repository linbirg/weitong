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
    partial class FrmOrderPreview : Form
    {

        private Order m_cart_order = null;
        private BindingList<OrderRowData> m_currentOrderDetailList = null;

        public Order Order
        {
            get { return m_cart_order; }
            set { m_cart_order = value; }
        }

        public FrmOrderPreview()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 显示当前订单。
        /// </summary>
        private void showCurrentOrder()
        {
            if (Order == null) return;
            Customer orderCust = Customer.findByID(Order.CustomerID);
            showOrderCustomer(orderCust);


            generateCurrentOrderDetails();
            lbl_total.Text = Order.Amount.ToString();
            //else
            //{
            //    m_salesMgr.generateCurrentOrderDetails();
            //    lbl_currentOrderAmount.Text = m_salesMgr.CurrentOrder.Amount.ToString();
            //    lbl_currentOrderFavorablePrice.Text = "0";
            //    lbl_curOrderTotal.Text = m_salesMgr.CurrentOrder.Amount.ToString();
            //    tBox_actualMoney.Text = m_salesMgr.CurrentOrder.Amount.ToString();
            //    tBox_change.Text = "0";

            //    enableCurrentOrderBtnByState(m_salesMgr.CurrentOrder.State);
            //}

            dgv_currentOrder.Refresh();
        }

        private void showOrderCustomer(Customer aCustomer)
        {
            if (aCustomer == null) return;
            lbl_customerName.Text = aCustomer.Name;
            MemberLevel customerLevel = MemberLevel.findByLevel(aCustomer.MemberLevel);
            if (customerLevel != null)
            {
                lbl_membLevel.Text = customerLevel.ToString();
            }
            else
            {
                WARNING("未找到客户对应的级别！");
                lbl_membLevel.Text = "普通会员";
            }

            lbl_customerJob.Text = aCustomer.Job;
            lbl_customerBirthday.Text = aCustomer.Birthday.ToShortDateString();
            lbl_custAddress.Text = aCustomer.Address;
            lbl_custEmail.Text = aCustomer.Email;
            lbl_custPhone.Text = aCustomer.PhoneNumber;
            lbl_custRegisterDay.Text = aCustomer.RegisterDate.ToShortDateString();

        }

        private void WARNING(string msg)
        {
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private DialogResult SelectionDlgShow(string msg)
        {
            return MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void FrmOrderPreview_Load(object sender, EventArgs e)
        {
            try
            {
                CenterToScreen();
                bindOrderDetails();
                showCurrentOrder();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void generateCurrentOrderDetails()
        {
            if (Order == null) return;
            List<OrderDetail> details = Order.getDetails();
            if (details != null)
            {
                if (m_currentOrderDetailList == null)
                {
                    m_currentOrderDetailList = new BindingList<OrderRowData>();
                }
                m_currentOrderDetailList.Clear();

                foreach (OrderDetail detail in details)
                {
                    OrderRowData data = new OrderRowData();
                    data.Code = detail.Code;
                    weitongDataSet1.storageRow aWineStrge = Storage.findByCode(data.Code);
                    data.Description = aWineStrge.description;
                    data.KnockDownPrice = detail.KnockDownPrice;
                    data.Units = detail.Units;
                    data.FavorablePrice = aWineStrge.retailprice - detail.KnockDownPrice;
                    m_currentOrderDetailList.Add(data);
                }
            }
            else
            {
                if (m_currentOrderDetailList != null) m_currentOrderDetailList.Clear();
            }
        }

        private void bindOrderDetails()
        {
            this.dgv_currentOrder.AutoGenerateColumns = false;
            if (m_currentOrderDetailList == null)
            {
                m_currentOrderDetailList = new BindingList<OrderRowData>();
            }
            else
            {
                m_currentOrderDetailList.Clear();
            }
            dgv_currentOrder.DataSource = m_currentOrderDetailList;
            // bind column
            this.dgv_currentOrder.Columns["currentOrderCode"].DataPropertyName = "code";
            this.dgv_currentOrder.Columns["currentOrderDescription"].DataPropertyName = "description";
            this.dgv_currentOrder.Columns["currentOrderKnockDownPrice"].DataPropertyName = "KnockDownPrice";
            this.dgv_currentOrder.Columns["currentOrderFavorablePrice"].DataPropertyName = "FavorablePrice";
            this.dgv_currentOrder.Columns["currentOrderUnits"].DataPropertyName = "Units";
            this.dgv_currentOrder.Columns["currentOrderAmount"].DataPropertyName = "Amount";
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                Order.save();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }
    }
}
