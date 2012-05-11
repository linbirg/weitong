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
        private User m_currentUser = null;
        private RolesMgr m_rolesMgr = null;


        public FrmMain()
        {
            InitializeComponent();
            CenterToScreen();
            tBox_CellEditer.LostFocus += new EventHandler(tBox_CellEditer_LostFocus);
        }

        public User CurrentUser
        {
            get { return m_currentUser; }
            set { 
                m_currentUser = value;
                if (m_currentUser != null) showCurrentUser();
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
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
                m_salesMgr.CurrentUser = m_currentUser;

                m_salesMgr.init();

                // memberlevel
                m_membLevelMgr = new MembLevelMgr();
                m_membLevelMgr.MemberLevelGrid = dgv_memberLevel;

                m_membLevelMgr.init();

                // role
                m_rolesMgr = new RolesMgr();
                m_rolesMgr.RolesGrid = dgv_roles;
                m_rolesMgr.init();

                cbox_memblevel_load();

                // 加载统计信息
                showStatistics();
            }
            catch (Exception ex)
            {
                WARNING("启动出现异常：" + ex.Message);
            }
            
        }

        private void btn_addSupplier_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_deleteSupplier_Click(object sender, EventArgs e)
        {
            m_supplierMgr.deleteCurrentSupplier();
        }

        private void btn_addStorage_Click(object sender, EventArgs e)
        {
            try
            {
                string supplierName = tBox_supplier.Text;
                Supplier aSpler = Supplier.findByName(supplierName);
                // 如果供应商信息不存在，则提示是否添加供应商，如果客户选择不添加供应商，则供应商信息为空。
                if (aSpler == null)
                {
                    if (DialogResult.Yes == SelectionDlgShow("供应商" + supplierName + "信息不在库中，你要保存供应商信息么？"))
                    {
                        aSpler = Supplier.NewSupplier();
                        aSpler.Name = supplierName;
                        aSpler.save();
                    }
                }

                int supplier_id = aSpler == null ? -1 : aSpler.ID;

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
                
                decimal price = decimal.Parse(tBox_price.Text);
                decimal caseprice = decimal.Parse(tBox_price.Text);
                decimal retailprice = decimal.Parse(tBox_retailprice.Text);
                int units = Int32.Parse(tBox_units.Text);
                m_wineStorageMgr.addStorage(code, chateau, country, appellation, quality, vintage, description, bottle, score,
                    supplier_id, price, caseprice, retailprice, units);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_deleteStorage_Click(object sender, EventArgs e)
        {
            try
            {
                m_wineStorageMgr.deleteCurrentStorage();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void dgv_storage_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgv_storage.CurrentRow == null) return;
                DataRowView dataRowView = dgv_storage.CurrentRow.DataBoundItem as DataRowView;
                weitongDataSet1.storageRow dataRow = dataRowView.Row as weitongDataSet1.storageRow;
                showStorageInfo(dataRow);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
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
            try
            {
                // 模拟tbox_KeyPress事件,调用其相应函数
                tBox_code_KeyPress(sender,new KeyPressEventArgs('\r'));
                //weitongDataSet1.storageRow aStorageRow = Storage.findByCode(tBox_code.Text.Trim());
                //// 如果没有对应酒的库存，则在wines里面查找酒的信息。
                //if (aStorageRow == null)
                //{
                //    weitongDataSet1.winesRow aWineRow = m_wineStorageMgr.findWineByCode(tBox_code.Text);
                //    if (aWineRow == null)
                //    {
                //        WARNING("未找到指定酒的相关信息，请确定编码是否正确！");
                //        return;
                //    }

                //    showWineInfo(aWineRow.code,
                //        aWineRow.IschateauNull() ? null : aWineRow.chateau,
                //        aWineRow.IsvintageNull() ? 0 : aWineRow.vintage,
                //        aWineRow.IsappellationNull() ? null : aWineRow.appellation,
                //        aWineRow.IsqualityNull() ? null : aWineRow.quality,
                //        aWineRow.IsscoreNull() ? null : aWineRow.score,
                //        aWineRow.IsdescriptionNull() ? null : aWineRow.description,
                //        aWineRow.IscountryNull() ? null : aWineRow.country);
                //}
                //else
                //{
                //    showStorageInfo(aStorageRow);
                //}
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }


        /// <summary>
        /// 查找酒的库存信息。如果输入了code，则按照code查找；
        /// 如果输入了描述信息，则按照描述信息进行模糊查找。
        /// 否则提示输入的信息不完整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SearchStorageInfo_Click(object sender, EventArgs e)
        {
            try
            {
                tBox_salesWineCode_KeyPress(sender, new KeyPressEventArgs('\r'));
                //if (tBox_salesWineCode.Text.Trim() != "")
                //{
                //    doSearchStorgeByCode();
                //}
                //else if (tBox_salesWineDescription.Text.Trim() != "")
                //{
                //    doSearchStorgeByDescription();
                //}
                //else
                //{
                //    WARNING("请输入正确的酒信息以便于查找！");
                //}
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void btn_ListStorageInfo_Click(object sender, EventArgs e)
        {
            try
            {
                m_salesMgr.updateTable(this.m_salesMgr.DataSet.storage);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
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
            customer.Birthday = DateTime.Parse(tBox_customerBirthday.Text);
            customer.Email = tBox_customerEmail.Text;
            customer.Job = tBox_customerJob.Text;
            customer.PhoneNumber = tBox_customerPhNumber.Text;
            customer.RegisterDate = DateTime.Now;
            customer.Sex = 1;
            
            // memberinfo
            if(cbox_membLevel.SelectedItem!= null)
            {
                MemberLevel level = (MemberLevel)cbox_membLevel.SelectedItem;
                customer.MemberLevel = level.Level;
            }
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
            try
            {
                if (checkCustomerValidtion())
                {
                    //Customer aCustomer = Customer.findByName(tBox_custermorName.Text); //m_salesMgr.findCustomerByName(tBox_custermorName.Text);
                    //if (aCustomer == null)
                    //{
                    //    aCustomer = new Customer();
                    //    assignCustomerInfo(aCustomer);
                    //    m_salesMgr.addCustomer2DB(aCustomer);
                    //}
                    //if (m_salesMgr.CartCustomer == null || m_salesMgr.CartCustomer.Name != aCustomer.Name)
                    //{
                    //    m_salesMgr.CartCustomer = aCustomer;
                    //}
                    doSaveCustomer();
                    m_salesMgr.calcCart();
                    jump2CurrentOrder();
                    showCurrentOrder();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
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
            try
            {
                if (checkCustomerValidtion())
                {
                    if (tBox_custermorName.Text != "")
                    {
                        doSearchCustomerByName();
                    }
                    else if (tBox_customerPhNumber.Text != "")
                    {
                        doSearchCustomerByPhoneNumber();
                    }
                    else
                    {
                        WARNING("请输入正确的客户信息！");
                    }
                    //Customer aCustomer = Customer.findByName(tBox_custermorName.Text); //m_salesMgr.findCustomerByName(tBox_custermorName.Text);
                    //if (aCustomer != null)
                    //{
                    //    showCustomerInfo(aCustomer);
                    //    m_salesMgr.CartCustomer = aCustomer;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("未找到您要的客户信息！你确定输入信息是否正确。");
                    //}
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void showCustomerInfo(Customer aCustomer)
        {
            if (aCustomer == null) return;
            tBox_custermorName.Text = aCustomer.Name;
            tBox_customerAddress.Text = aCustomer.Address;
            tBox_customerBirthday.Text = aCustomer.Birthday.ToShortDateString();
            tBox_customerEmail.Text = aCustomer.Email;
            tBox_customerJob.Text = aCustomer.Job;
            tBox_customerPhNumber.Text = aCustomer.PhoneNumber;
            tBox_customerRegisterDay.Text = aCustomer.RegisterDate.ToShortDateString();
            //tBox_cartMemLevel.Text = aCustomer.MemberLevel.ToString();
            cbox_membLevel_Show(aCustomer.MemberLevel);
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
            try
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
            catch (Exception ex)
            {
                WARNING(ex.Message);
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

        //private void btn_PreViewCart_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (m_salesMgr.CartCustomer == null)
        //        {
        //            WARNING("订单不能没有客户信息，请输入客户信息。");
        //            return;
        //        }
        //        FrmPrint frmPrint = new FrmPrint();
        //        frmPrint.CartDetaiGrid = dgv_cartDetail;
        //        frmPrint.SetCustomer(m_salesMgr.CartCustomer);
        //        frmPrint.OrderTime = DateTime.Now;
        //        frmPrint.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        WARNING(ex.Message);
        //    }
        //}

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

        private DialogResult SelectionDlgShow(string msg)
        {
            return MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void dgv_roles_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow curRow = this.dgv_roles.Rows[e.RowIndex];
                Role data = curRow.DataBoundItem as Role;
                this.m_rolesMgr.updateDB(data);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void showCurrentUser()
        {
            string name = "";
            if (CurrentUser.Alias != null && CurrentUser.Alias != "")
            {
                name = CurrentUser.Alias;
            }
            else
            {
                name = CurrentUser.Name;
            }
            tsl_curUser.Text = "user:" + name;
            tsl_sysDate.Text = DateTime.Now.ToShortDateString();
            tsl_sysTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //tsl_sysTime.Text = DateTime.Now.ToLongTimeString();
                showCurrentUser();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void btn_searchSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                m_supplierMgr.searchSupplier(tbox_supplierName.Text);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        // 保存客户信息（存入数据库中）
        private void doSaveCustomer()
        {
            if (checkCustomerValidtion())
            {
                Customer aCustomer = Customer.findByName(tBox_custermorName.Text); //m_salesMgr.findCustomerByName(tBox_custermorName.Text);
                if (aCustomer == null)
                {
                    aCustomer = Customer.NewCustomer();
                }
                MemberLevel level = cbox_membLevel.SelectedItem as MemberLevel;
                if (aCustomer.MemberLevel != level.Level)
                {
                    if (!CurrentUser.isAdministrator())
                    {
                        WARNING("您无权修改会员级别，请与店长联系！");
                        return;
                    }
                }
                assignCustomerInfo(aCustomer);
                m_salesMgr.addCustomer2DB(aCustomer);

                //m_salesMgr.CartCustomer = aCustomer;
            }
            else
            {
                WARNING("请输入正确的客户信息！");
            }
        }

        /// <summary>
        /// 保存客户信息按钮的相应函数
        /// 函数先保存客户信息到数据库，再设置客户为当前客户(设置当前客户时，会更新相应的折扣信息)。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_saveCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                doSaveCustomer();
                Customer aCustomer = Customer.findByName(tBox_custermorName.Text);
                m_salesMgr.CartCustomer = aCustomer;
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void dgv_cartDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCell cell = dgv_cartDetail.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (dgv_cartDetail.Columns[e.ColumnIndex].Name == "discountDGVOrderDetailTextBoxColumn")
                {
                    tBox_CellEditer.Size = cell.Size;
                    tBox_CellEditer.Text = cell.Value.ToString();
                    tBox_CellEditer.Visible = true;
                    tBox_CellEditer.Location = calcCartDetailCellLocation(e.RowIndex,e.ColumnIndex);
                    tBox_CellEditer.Focus();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        void tBox_CellEditer_LostFocus(object sender, EventArgs e)
        {
            try
            {
                
                tBox_CellEditer.Visible = false;
                string value = tBox_CellEditer.Text;
                DataGridViewCell cell = dgv_cartDetail.CurrentCell;
                CartDetailRowData data = dgv_cartDetail.Rows[cell.RowIndex].DataBoundItem as CartDetailRowData;
                if (dgv_cartDetail.Columns[cell.ColumnIndex].Name == "discountDGVOrderDetailTextBoxColumn")
                {
                    int discount = int.Parse(value);
                    if (discount < CurrentUser.MinDiscount)
                    {
                        WARNING("您的折扣权限不够，请于店长联系！");
                    }
                    else
                    {
                        data.Discount = discount;
                    }
                }        
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 计算购物车中输入单元格的位置。
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private Point calcCartDetailCellLocation(int rowIndex, int columnIndex)
        {
            int x = dgv_cartDetail.Location.X;
            int y = dgv_cartDetail.Location.Y;
            int width = 0;
            int height = dgv_cartDetail.ColumnHeadersHeight;

            for (int row = 0; row < rowIndex; row++)
            {
                height += dgv_cartDetail.Rows[row].Height;
            }

            for (int col = 0; col < columnIndex; col++)
            {
                width += dgv_cartDetail.Columns[col].Width;
            }

            return new Point(x + width, y + height);
        }

        private void dgv_orderList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4)
                {
                    OrderState state = (OrderState)e.Value;
                    e.Value = Order.getStateName(state);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void cbox_memblevel_load()
        {
            List<MemberLevel> list = MemberLevel.loadData();
            cbox_membLevel.Items.Clear();
            foreach (MemberLevel lv in list)
            {
                cbox_membLevel.Items.Add(lv);
            }
        }

        private void cbox_membLevel_Show(int level)
        {
            foreach (MemberLevel lv in cbox_membLevel.Items)
            {
                if (lv.Level == level) cbox_membLevel.SelectedItem = lv;
            }
        }

        private void tsmi_PrintPrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_orderList.CurrentRow == null) return;
                DataRowView curRow = dgv_orderList.CurrentRow.DataBoundItem as DataRowView;
                weitongDataSet1.orderRow data = curRow.Row as weitongDataSet1.orderRow;
                Order curOrder = Order.findByID(data.id);
                if (curOrder == null) return;
                FrmPrint frmPrint = new FrmPrint();
                frmPrint.User = CurrentUser;
                frmPrint.Order = curOrder;
                frmPrint.ShowDialog();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void showStatistics()
        {
            lbl_curDate.Text = "今天是" + DateTime.Today.ToShortDateString();
            lbl_statisticsCurDateTotal.Text = "您今天共完成" + StatisticsMgr.countUserDayOrders(CurrentUser.ID, DateTime.Today) 
                + "笔订单，总金额" + 
                StatisticsMgr.sumUserDayOrders(CurrentUser.ID,DateTime.Today) + "元";
            lbl_statisticsCurMonthTotal.Text = "您本月共完成" + StatisticsMgr.countUserMonthOrders(CurrentUser.ID,DateTime.Today.Month)
                + "笔订单，总金额" + 
                StatisticsMgr.sumUserMonthOrders(CurrentUser.ID, DateTime.Today.Month) + "元";
        }

        /// <summary>
        /// 客户姓名输入框的按键响应函数
        /// 当用户输入名称后，按回车后，即按照输入的名称信息查找客户，
        /// 如果名称为空，则按照号码查找客户。
        /// </summary>
        private void tBox_custermorName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    if (tBox_custermorName.Text != "")
                    {
                        doSearchCustomerByName();
                    }
                    else
                    {
                        doSearchCustomerByPhoneNumber();
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void doSearchCustomerByName()
        {
            if (tBox_custermorName.Text == "")
            {
                WARNING("请输入客户姓名！");
                tBox_custermorName.Focus();
                return;
            }
            Customer aCustomer = Customer.findByName(tBox_custermorName.Text.Trim());
            if (aCustomer == null)
            {
                WARNING("未找到客户 " + tBox_custermorName.Text + " 的信息！");
                return;
            }
            m_salesMgr.CartCustomer = aCustomer;
            showCustomerInfo(aCustomer);
        }

        private void doSearchCustomerByPhoneNumber()
        {
            if (tBox_customerPhNumber.Text == "")
            {
                WARNING("请输入电话号码！");
                tBox_customerPhNumber.Focus();
                return;
            }
            string phoneNun = tBox_customerPhNumber.Text.Trim();
            Customer aCustomer = Customer.findByPhNumber(phoneNun);
            if (aCustomer == null)
            {
                WARNING("未找到指定号码 " + phoneNun + " 的客户信息！");
                return;
            }
            m_salesMgr.CartCustomer = aCustomer;
            showCustomerInfo(aCustomer);
        }

        private void tBox_customerPhNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    doSearchCustomerByPhoneNumber();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void doSearchStorgeByCode()
        {
            weitongDataSet1.storageRow aStorageInfoRow = Storage.findByCode(tBox_salesWineCode.Text.Trim());
            if (aStorageInfoRow != null)
            {
                weitongDataSet1.storageDataTable table = new weitongDataSet1.storageDataTable();
                table.ImportRow(aStorageInfoRow);
                m_salesMgr.bindStorageTable(table);
            }
            else
            {
                WARNING("未找到编码为 " + tBox_salesWineCode.Text.Trim() + " 酒的库存信息！");
            }
        }

        private void doSearchStorgeByDescription()
        {
            string description = tBox_salesWineDescription.Text.Trim();
            //if (description != "")
            //{
                weitongDataSet1.storageDataTable table = Storage.findByDescription(description);
                m_salesMgr.bindStorageTable(table);
            //}
        }

        /// <summary>
        /// 酒编码输入框的回车相应函数
        /// 如果输入了编码，则按照编码查找
        /// 如果没有输入编码，函数会试图按照描述查找
        /// 如果描述也没有输入，则提示输入适当的信息以便查找。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_salesWineCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    //string code = tBox_salesWineCode.Text.Trim();
                    //if (code != "")
                    //{
                    //    doSearchStorgeByCode();
                    //}

                    if (tBox_salesWineCode.Text.Trim() != "")
                    {
                        doSearchStorgeByCode();
                    }
                    else// if (tBox_salesWineDescription.Text.Trim() != "")
                    {
                        doSearchStorgeByDescription();
                    }
                    //else
                    //{
                    //    WARNING("请输入正确的酒信息以便于查找！");
                    //}
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 根据描述信息模糊查找库存信息
        /// 如果描述信息为空，则返回所有的库存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_salesWineDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string description = tBox_salesWineDescription.Text.Trim();
                    //if (description != "")
                    //{
                        //doSearchStorgeByCode();
                        weitongDataSet1.storageDataTable table = Storage.findByDescription(description);
                        m_salesMgr.bindStorageTable(table);
                    //}
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 酒编码框的回车相应函数
        /// 如果编码不为空，则按编码查找库存信息
        /// 如果酒编码为空，则按照描述信息查找库存信息
        /// 如果描述信息为空，则返回所有的库存信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_code_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string code = tBox_code.Text.Trim();
                    if (code != "")
                    {
                        weitongDataSet1.storageRow row = Storage.findByCode(code);
                        if (row != null)
                        {
                            weitongDataSet1.storageDataTable table = new weitongDataSet1.storageDataTable();
                            table.ImportRow(row);
                            m_wineStorageMgr.WineStorageGridView.DataSource = table;
                        }
                    }
                    else
                    {
                        tBox_description_KeyPress(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 根据描述信息模糊查找库存信息
        /// 如果描述信息为空，则会返回所有库存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_description_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string description = tBox_description.Text.Trim();
                    //if (description != "")
                    //{
                        m_wineStorageMgr.WineStorageGridView.DataSource = Storage.findByDescription(description);
                    //}
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        
        
    }
}
