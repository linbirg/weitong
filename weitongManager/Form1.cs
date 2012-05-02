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
    partial class FrmMain : Form
    {
        private SupplierMgr m_supplierMgr = null;
        private WineStorageMgr m_wineStorageMgr = null;
        private SalesMgr m_salesMgr = null;
        private MembLevelMgr m_membLevelMgr = null;


        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'weitongDataSet1.memberlevel' table. You can move, or remove it, as needed.
            this.memberlevelTableAdapter.Fill(this.weitongDataSet1.memberlevel);
            m_supplierMgr = new SupplierMgr();
            m_supplierMgr.DataSet = weitongDataSet1;
            m_supplierMgr.SupplierTableAdapter = supplierTableAdapter;
            m_supplierMgr.GridViewControl = dgv_supplier;
            m_supplierMgr.init();

            m_wineStorageMgr = new WineStorageMgr();
            m_wineStorageMgr.WineStorageGridView = dgv_storage;
            m_wineStorageMgr.DataSet = weitongDataSet1;
            m_wineStorageMgr.Adapter = new weitongDataSet1TableAdapters.storageTableAdapter();
            m_wineStorageMgr.init();

            // 
            m_salesMgr = new SalesMgr();
            m_salesMgr.StorgeInfoView = dgv_storageInfo;
            m_salesMgr.DataSet = new weitongDataSet1();
            m_salesMgr.StorageAdapter = new weitongDataSet1TableAdapters.storageTableAdapter();

            m_salesMgr.CartDetailView = dgv_cartDetail;
            m_salesMgr.CurrentOrderView = dgv_currentOrder;
            m_salesMgr.OrderListView = dgv_orderList;
            m_salesMgr.OrderAdapter = new weitongDataSet1TableAdapters.orderTableAdapter();

            m_salesMgr.init();

            // memberlevel
            m_membLevelMgr = new MembLevelMgr();
            m_membLevelMgr.MemberLevelGrid = dgv_memberLevel;

            m_membLevelMgr.init();
            
        }

        private void btn_addSupplier_Click(object sender, EventArgs e)
        {
            if (tbox_supplierName.Text.Length > 0)
            {
                m_supplierMgr.insertSupplier(tbox_supplierName.Text);
            }
            else
            {
                MessageBox.Show("请输入合法的供应商信息！");
            }
            //dgv_supplier.Refresh();
        }

        private void tsmi_deleteSupplier_Click(object sender, EventArgs e)
        {
            m_supplierMgr.deleteCurrentSupplier();
        }

        private void btn_addStorage_Click(object sender, EventArgs e)
        {
            string code = tBox_code.Text;
            string chateau = tBox_chateau.Text;
            string country = tBox_country.Text; 
            string appellation = tBox_appellation.Text; 
            string quality = tBox_quality.Text;
            string vintage = tBox_Vintage.Text;
            if (vintage == "") vintage = null;
            string description = tBox_description.Text;
            string bottle = tBox_bottle.Text;
            string score = tBox_score.Text;
            string supplier = tBox_supplier.Text;
            decimal price = decimal.Parse(tBox_price.Text);
            decimal caseprice = decimal.Parse(tBox_price.Text);
            decimal retailprice = decimal.Parse(tBox_retailprice.Text);
            int units = Int32.Parse(tBox_units.Text);
            m_wineStorageMgr.addStorage(code, chateau, country, appellation, quality, vintage, description, bottle, score, 
                supplier, price, caseprice, retailprice, units);
        }

        private void tsmi_deleteStorage_Click(object sender, EventArgs e)
        {
            m_wineStorageMgr.deleteCurrentStorage();
        }

        private void dgv_storage_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_storage.CurrentRow == null) return;
            DataRowView dataRowView = dgv_storage.CurrentRow.DataBoundItem as DataRowView;
            weitongDataSet1.storageRow dataRow = dataRowView.Row as weitongDataSet1.storageRow;
            showStorageInfo(dataRow);
        }

        private void showStorageInfo(weitongDataSet1.storageRow row)
        {
            if (row == null) return;
            
            tBox_units.Text = row.units.ToString();
            tBox_bottle.Text = row.bottle;
            tBox_supplier.Text = row.name;
            tBox_price.Text = row.price.ToString();
            tBox_retailprice.Text = row.retailprice.ToString();
            showWineInfo(row.code, row.chateau, row.vintage, row.appellation, 
                row.quality, row.score, row.description, row.country);
        }

        private void showWineInfo(string code, string chateau, int vintage, string appellation, 
            string quality, string score, string description, string country)
        {
            tBox_code.Text = code;
            tBox_chateau.Text = chateau;
            tBox_country.Text = country;
            tBox_score.Text = score;
            tBox_Vintage.Text = vintage.ToString();
            tBox_appellation.Text = appellation;
            tBox_quality.Text = quality;
            tBox_description.Text = description;
        }

        private void btn_seachStorage_Click(object sender, EventArgs e)
        {
            weitongDataSet1.storageRow aStorageRow = m_wineStorageMgr.findStorageByCode(tBox_code.Text);
            // 如果没有对应酒的库存，则在wines里面查找酒的信息。
            if (aStorageRow == null)
            {
                weitongDataSet1.winesRow aWineRow = m_wineStorageMgr.findWineByCode(tBox_code.Text);
                if (aWineRow == null)
                {
                    MessageBox.Show("未找到指定酒的相关信息，请确定编码是否正确！");
                    return;
                }
                               
                showWineInfo(aWineRow.code, 
                    aWineRow.IschateauNull() ? null : aWineRow.chateau, 
                    aWineRow.IsvintageNull() ? 0 : aWineRow.vintage, 
                    aWineRow.IsappellationNull() ? null : aWineRow.appellation,
                    aWineRow.IsqualityNull() ? null : aWineRow.quality, 
                    aWineRow.IsscoreNull() ? null: aWineRow.score, 
                    aWineRow.IsdescriptionNull() ? null : aWineRow.description, 
                    aWineRow.IscountryNull() ? null : aWineRow.country);
            }
            else
            {
                showStorageInfo(aStorageRow);
            }
        }

        private void btn_SearchStorageInfo_Click(object sender, EventArgs e)
        {
            weitongDataSet1.storageRow aStorageInfoRow = this.m_salesMgr.findStorageByCode(tBox_salesWineCode.Text);
            if (aStorageInfoRow != null)
            {
                weitongDataSet1.storageDataTable table = new weitongDataSet1.storageDataTable();
                table.ImportRow(aStorageInfoRow);
                m_salesMgr.updateTable(table);
            }
            else
            {
                MessageBox.Show("未找到指定酒的相关信息，请确定编码是否正确！");
            }
        }

        private void btn_ListStorageInfo_Click(object sender, EventArgs e)
        {
            m_salesMgr.updateTable(this.m_salesMgr.DataSet.storage);
        }

        private void tsmi_addToCart_Click(object sender, EventArgs e)
        {
            m_salesMgr.addCurrentStorage2Cart();
        }

        private void assignCustomerInfo(Customer customer)
        {
            if (customer == null) return;
            customer.Name = tBox_custermorName.Text;
            customer.Address = tBox_customerAddress.Text;
            customer.Birthday = DateTime.Now.Date;
            customer.Email = tBox_customerEmail.Text;
            customer.Job = tBox_customerJob.Text;
            customer.PhoneNumber = tBox_customerPhNumber.Text;
            customer.RegisterDate = DateTime.Now;
            customer.Sex = 1;
        }

        private void jump2CurrentOrder()
        {
            tabCtrl_Order.SelectTab("tabPg_CurrentOrder");
        }


        // 结算购物车
        // 函数首先确定联系人信息，如果是新的联系人信息，则添加到客户表中(默认为最低等级会员)。
        // 再生成订单信息，订单状态为未付款(当完成付款后，修改状态为完成)。
        private void btn_CalcOrder_Click(object sender, EventArgs e)
        {
            if (checkCustomerValidtion())
            {
                Customer aCustomer = m_salesMgr.findCustomerByName(tBox_custermorName.Text);
                if (aCustomer == null)
                {
                    aCustomer = new Customer();
                    assignCustomerInfo(aCustomer);
                    m_salesMgr.addCustomer2DB(aCustomer);
                }
                m_salesMgr.CartCustomer = aCustomer;
                m_salesMgr.calcCart();
                jump2CurrentOrder();
                showCurrentOrder();
            }
        }

        // 验证输入的客户信息是否正确
        private bool checkCustomerValidtion()
        {
            bool ret = false;
            if (tBox_custermorName.Text == "")
            {
                MessageBox.Show("请输入正确的客户名称！","提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        private void btn_SearchCustomer_Click(object sender, EventArgs e)
        {
            if (checkCustomerValidtion())
            {
                Customer aCustomer = m_salesMgr.findCustomerByName(tBox_custermorName.Text);
                if (aCustomer != null)
                {
                    showCustomerInfo(aCustomer);
                    m_salesMgr.CartCustomer = aCustomer;
                }
                else
                {
                    MessageBox.Show("未找到您要的客户信息！你确定输入信息是否正确。");
                }
            }
        }

        private void showCustomerInfo(Customer aCustomer)
        {
            if (aCustomer == null) return;
            tBox_custermorName.Text = aCustomer.Name;
            tBox_customerAddress.Text = aCustomer.Address;
            tBox_customerBirthday.Text = aCustomer.Birthday.ToString();
            tBox_customerEmail.Text = aCustomer.Email;
            tBox_customerJob.Text = aCustomer.Job;
            tBox_customerPhNumber.Text = aCustomer.PhoneNumber;
            tBox_customerRegisterDay.Text = aCustomer.RegisterDate.ToString();
            tBox_cartMemLevel.Text = aCustomer.MemberLevel.ToString();
        }

        private void showCurrentOrder()
        {
            showOrderCustomer(m_salesMgr.CartCustomer);
            //m_salesMgr.bindCurrentOrder();
            lbl_currentOrderAmount.Text = m_salesMgr.CurrentOrderAmount.ToString();
            lbl_currentOrderFavorablePrice.Text = "0";
            lbl_curOrderTotal.Text = m_salesMgr.CurrentOrderAmount.ToString();
        }

        private void showOrderCustomer(Customer aCustomer)
        {
            if (aCustomer == null) return;
            lbl_customerName.Text = aCustomer.Name;
            lbl_membLevel.Text = aCustomer.MemberLevel.ToString();
            lbl_customerJob.Text = aCustomer.Job;
            lbl_customerBirthday.Text = aCustomer.Birthday.ToShortDateString();
            lbl_custAddress.Text = aCustomer.Address;
            lbl_custEmail.Text = aCustomer.Email;
            lbl_custPhone.Text = aCustomer.PhoneNumber;
            lbl_custRegisterDay.Text = aCustomer.RegisterDate.ToShortDateString();
            
        }

        private void btn_CompleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                decimal money = Decimal.Parse(tBox_actualMoney.Text);
                decimal change = Decimal.Parse(tBox_change.Text);
                decimal recved = money - change;
                if (recved == 0)
                {                 
                    DialogResult rst = MessageBox.Show("你确定订单免费么？","警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (rst == DialogResult.Cancel) return;
                }
                m_salesMgr.completeCurrentOrder(recved);
                m_salesMgr.updateOrderList();
                jump2OrderList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void jump2OrderList()
        {
            tabCtrl_Order.SelectTab("tabPg_OrderList");
        }

        private void tsmi_viewOrderDetail_Click(object sender, EventArgs e)
        {
            if (dgv_orderList.CurrentRow == null) return;
            DataRowView curRow = dgv_orderList.CurrentRow.DataBoundItem as DataRowView;
            weitongDataSet1.orderRow data = curRow.Row as weitongDataSet1.orderRow;
            // 设置订单为当前的订单
            int ret = m_salesMgr.setCurrentOrder(data.id);
            if (ret == 0)
            {
                if ((OrderState)data.orderstate == OrderState.CANCEL)
                {
                    btn_CompleteOrder.Enabled = false;
                    btn_continueOrder.Enabled = false;
                    btn_cancelOrder.Enabled = false;
                }
                showCurrentOrder();
                jump2CurrentOrder();
            }
            else
            {
                MessageBox.Show("指定的订单不存在，请于管理员联系！");
            }
        }

        // 撤销订单，只有等待付款的订单可以撤销，已经完成付款的单子无法撤销。
        private void btn_cancelOrder_Click(object sender, EventArgs e)
        {
            if (m_salesMgr.CurrentOrder != null && m_salesMgr.CurrentOrder.isCompleted)
            {
                MessageBox.Show("订单已完成付款，无法撤销！");
            }
            else
            {
                // 修改库存，并更新订单状态为撤销
                m_salesMgr.cancelOrder(m_salesMgr.CurrentOrder);
                m_salesMgr.updateOrderList();
                jump2OrderList();
            }
        }

        private void tBox_actualMoney_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal money = Decimal.Parse(tBox_actualMoney.Text);
                tBox_change.Text = (money - m_salesMgr.CurrentOrderAmount).ToString();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 继续购物，根据现有的订单信息生成购物车。
        private void btn_curOrderContinue_Click(object sender, EventArgs e)
        {
            m_salesMgr.generateCartDetail();
            showCustomerInfo(m_salesMgr.CartCustomer);
            jump2CurrentCart();
        }

        private void jump2CurrentCart()
        {
            tabCtrl_Order.SelectTab("tabPg_Cart");
        }

        private void btn_PreViewCart_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_salesMgr.CartCustomer == null)
                {
                    WARNING("订单不能没有客户信息，请输入客户信息。");
                    return;
                }
                FrmPrint frmPrint = new FrmPrint();
                frmPrint.CartDetaiGrid = dgv_cartDetail;
                frmPrint.SetCustomer(m_salesMgr.CartCustomer);
                frmPrint.OrderTime = DateTime.Now;
                frmPrint.ShowDialog();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_cartAddUnits_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_cartDetail.CurrentRow == null) return;

                CartDetailRowData curRow = dgv_cartDetail.CurrentRow.DataBoundItem as CartDetailRowData;

                m_salesMgr.plusCartWineUnits(curRow.Code);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        

        private void dgv_memberLevel_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow curRow = dgv_memberLevel.Rows[e.RowIndex];
                MemberLevel data = curRow.DataBoundItem as MemberLevel;
                m_membLevelMgr.update2DB(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常：" + ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 添加的顺序是从低到高，与之对应的删除的顺序是从高到低。
        private void tsmi_AddLevel_Click(object sender, EventArgs e)
        {
            try
            {
                m_membLevelMgr.addDefaultTopLevelInfo();
                dgv_memberLevel.Refresh();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("出现异常：" + ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                WARNING(ex.Message);
            }
        }

        // 删除最高的级别，删除顺序从高到底，与之对应的，添加的顺序是从低到高。
        private void tsmi_DeleteLevel_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow curRow = dgv_memberLevel.CurrentRow;

                MemberLevel data = m_membLevelMgr.getMaxLevel(); //curRow.DataBoundItem as MemberLevel;
                if (data == null) return;
                if(DialogResult.OK == 
                    MessageBox.Show("您确定删除级别" + data.Level.ToString() + "的信息么？","警告",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2))
                {
                    m_membLevelMgr.remove(data);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("出现异常：" + ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                WARNING("出现异常："+ ex.Message);
            }
        }

        private void WARNING(string msg)
        {
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        
    }
}
