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
        private UserMgr m_userMgr = null;

        private List<TabPage> m_pages = null;


        public FrmMain()
        {
            InitializeComponent();
            CenterToScreen();
            tBox_CellEditer.LostFocus += new EventHandler(tBox_CellEditer_LostFocus);
            tBox_OrdersCellEditor.LostFocus += new EventHandler(tBox_OrdersCellEditor_LostFocus);
            tBox_StorageEditer.LostFocus += new EventHandler(tBox_StorageEditer_LostFocus);

            // 使能修改编码按钮
            if (DateTime.Now.Date > new DateTime(2012, 6, 16, 0, 0, 0))
            {
                btn_ChangeCode.Enabled = false;
                btn_ChangeCode.Visible = false;
            }
        }

        public User CurrentUser
        {
            get { return m_currentUser; }
            set { 
                m_currentUser = value;
                if (m_currentUser != null)
                {
                    showCurrentUser();
                    if (!m_currentUser.isAdministrator())
                    {
                        hideAllAdminPages();
                    }
                }
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                if (Config.isUnderControl)
                {
                    this.Text = Config.Name + "(" + Config.suffix + ")-" + Config.Version;
                    tsmi_delStorage.Visible = false;
                }
                else
                {
                    this.Text = Config.Name + "-" + Config.Version;
                }
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
                set_all_storage_box_state(true);

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
                m_salesMgr.OrderTable = m_salesMgr.DataSet.order;

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
                // 初始化订单查询的日期选择
                init_order_list_timepiker();

                // 加载用户信息
                m_userMgr = new UserMgr();
                m_userMgr.UserGridView = dgv_Users;
                m_userMgr.init();
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

        /// <summary>
        /// 使能所有的库存输入框。
        /// </summary>
        private void set_all_storage_box_state(bool bState)
        {
            tBox_supplier.Enabled = bState;
            tBox_code.Enabled = bState;
            tBox_chateau.Enabled = bState;
            tBox_country.Enabled = bState;
            tBox_appellation.Enabled = bState;
            tBox_quality.Enabled = bState;
            tBox_Vintage.Enabled = bState;

            tBox_description.Enabled = bState;
            tBox_bottle.Enabled = bState;
            tBox_score.Enabled = bState;

            tBox_price.Enabled = bState;
            
            tBox_retailprice.Enabled = bState;
            tBox_units.Enabled = bState;
        }

        private void btn_addStorage_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_seachStorage.Enabled)
                {
                    set_all_storage_box_state(true);
                    btn_addStorage.Enabled = false;
                    btn_OK.Visible = true;
                    btn_cancel.Visible = true;
                    btn_OK.Focus();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 根据是入库状态执行相应的入库工作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                // 入库
                if (!btn_addStorage.Enabled)
                {
                    string code = tBox_code.Text.Trim();
                    doAddStorage();
                    //set_all_storage_box_state(false);
                    btn_addStorage.Enabled = true;
                    btn_OK.Visible = false;
                    btn_cancel.Visible = false;
                    dgv_storage_setSelected(code);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 在grid中设置库存信息中指定酒为选中行。
        /// </summary>
        /// <param name="code"></param>
        private void dgv_storage_setSelected(string code)
        {
            foreach (DataGridViewRow row in dgv_storage.Rows)
            {
                DataRowView dataRowView = row.DataBoundItem as DataRowView;
                weitongDataSet1.storageRow dataRow = dataRowView.Row as weitongDataSet1.storageRow;
                if (dataRow.code == code)
                {
                    row.Selected = true;
                    scrollGrid(dgv_storage);
                    break;
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                btn_addStorage.Enabled = true;
                btn_OK.Visible = false;
                btn_cancel.Visible = false;
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 查看历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi_deleteStorage_Click(object sender, EventArgs e)
        {
            try
            {
                //m_wineStorageMgr.deleteCurrentStorage();

                DataRowView dataRowView = this.dgv_storage.CurrentRow.DataBoundItem as DataRowView;
                weitongDataSet1.storageRow dataRow = dataRowView.Row as weitongDataSet1.storageRow;
                FrmHisStorage history = new FrmHisStorage();
                history.Wine = dataRow.code;
                history.ShowDialog();
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
            tBox_supplier.Text = row.IsnameNull() ? "" : row.name;
            tBox_price.Text = row.price.ToString();
            tBox_retailprice.Text = row.retailprice.ToString();
            showWineInfo(row.code, row.chateau, row.IsvintageNull() ? 0:row.vintage, row.appellation, 
                row.quality, row.score, row.description, row.country);
        }

        private void showWineInfo(string code, string chateau, int vintage, string appellation, 
            string quality, string score, string description, string country)
        {
            tBox_code.Text = code;
            tBox_chateau.Text = chateau;
            tBox_country.Text = country;
            tBox_score.Text = score;
            tBox_Vintage.Text = vintage == 0 ? "--" : vintage.ToString();
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
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 执行添加库存的工作。
        /// </summary>
        private void doAddStorage()
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

            int supplier_id = (aSpler == null ? -1 : aSpler.ID);

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
            int units = Int32.Parse(tBox_units.Text.Trim());
            
            if (Storage.existsWine(code) || Wine.existsWine(code))
            {
                if (DialogResult.Yes == SelectionDlgShow("编码为 " + code + " 名称为 " + description + " 的酒已经存在，您是要 增加库存 吗？"))
                {
                    if (DialogResult.Yes == SelectionDlgShow("您要入库的数量是 "+ units.ToString()))
                    {
                        m_wineStorageMgr.addStorage(code, chateau, country, appellation, quality, vintage, description, bottle, score,
                        supplier_id, price, caseprice, retailprice, units);
                    }
                }
            }
            else
            {
                if (DialogResult.Yes == SelectionDlgShow("您确定要 新增入库 编码为 " + code + " 名称为 " + description + " 的酒吗？"))
                {
                    if (DialogResult.Yes == SelectionDlgShow("您的数量是 " + units.ToString() + " ?"))
                    {
                        m_wineStorageMgr.addStorage(code, chateau, country, appellation, quality, vintage, description, bottle, score,
                            supplier_id, price, caseprice, retailprice, units);
                    }
                }
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
                m_salesMgr.reloadStorage(this.m_salesMgr.DataSet.storage);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_addToCart_Click(object sender, EventArgs e)
        {
            try
            {
                m_salesMgr.addCurrentStorage2Cart();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void assignCustomerInfo(Customer customer)
        {
            if (customer == null) return;
            customer.Name = tBox_custermorName.Text;
            customer.Address = tBox_customerAddress.Text;
            //if (tBox_customerBirthday.Text != "")
            //{
                customer.Birthday = picker_customerBirthday.Value.Date;
            //}
            
            customer.Email = tBox_customerEmail.Text;
            customer.Job = tBox_customerJob.Text;
            customer.PhoneNumber = tBox_customerPhNumber.Text;
            customer.RegisterDate = picker_customerRegDate.Value;
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


        
        /// <summary>
        /// 结算购物车
        /// 函数首先确定联系人信息，如果是新的联系人信息，则添加到客户表中(默认为等级1的会员)。
        /// 再生成订单信息，订单状态为未付款(当完成付款后，修改状态为完成)。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CalcOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkCustomerValidtion())
                {
                    doSaveCustomer();
                    Order newOrder = m_salesMgr.calcCart();
                    FrmOrderPreview FrmPrev = new FrmOrderPreview();
                    FrmPrev.Order = newOrder;
                    if (DialogResult.OK == FrmPrev.ShowDialog())
                    {
                        m_salesMgr.CurrentOrder = FrmPrev.Order;
                        jump2CurrentOrder();
                        showCurrentOrder();
                        m_salesMgr.reloadOrderList();
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        // 
        /// <summary>
        /// 验证输入的客户信息是否正确.
        /// 不合法的客户信息包括：
        /// 1. 姓名和电话都为空；
        /// 2. 电话不为空且不符合电话的格式；
        /// 3. 邮箱不为空且不符合邮箱的格式
        /// 
        /// </summary>
        /// <returns></returns>
        private bool checkCustomerValidtion()
        {
            bool ret = false;
            if (tBox_custermorName.Text == ""&&tBox_customerPhNumber.Text.Trim() == "")
            {
                WARNING("请输入正确的客户名称或手机号！");
            }
            else if (tBox_customerPhNumber.Text.Trim() != "")
            {
                if (util.isPhoneNumber(tBox_customerPhNumber.Text.Trim()))
                {
                    ret = true;
                }
                else
                {
                    WARNING("请输入正确的手机号！");
                }
            }
            else
            {
                ret = true;
            }

            if (tBox_customerEmail.Text.Trim() != "")
            {
                // 验证邮箱的格式
                if (!util.isMailAdress(tBox_customerEmail.Text.Trim()))
                {
                    WARNING("请输入正确的邮箱地址！");
                    ret = false;
                }
            }
            
            //else 
            //{
            //    ret = true;
            //}
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
            //tBox_customerBirthday.Text = aCustomer.Birthday.ToShortDateString();
            
            tBox_customerEmail.Text = aCustomer.Email;
            tBox_customerJob.Text = aCustomer.Job;
            tBox_customerPhNumber.Text = aCustomer.PhoneNumber;
            //tBox_customerRegisterDay.Text = aCustomer.RegisterDate.ToShortDateString();
            
            //tBox_cartMemLevel.Text = aCustomer.MemberLevel.ToString();
            cbox_membLevel_Show(aCustomer.MemberLevel);

            try
            {
                picker_customerBirthday.Value = aCustomer.Birthday;
                picker_customerRegDate.Value = aCustomer.RegisterDate;
            }
            catch (Exception ex)
            {
                //log(ex.message)
            }
        }


        /// <summary>
        /// 显示当前订单。
        /// </summary>
        private void showCurrentOrder()
        {
            showOrderCustomer(m_salesMgr.CartCustomer);
            //m_salesMgr.bindCurrentOrder();
            //lbl_currentOrderAmount.Text = m_salesMgr.getCurrentOrderAmount().ToString();
            //lbl_currentOrderFavorablePrice.Text = "0";
            //lbl_curOrderTotal.Text = m_salesMgr.getCurrentOrderAmount().ToString();//m_salesMgr.CurrentOrderAmount.ToString();
            
            if (m_salesMgr.CurrentOrder == null)
            {
                btn_continueOrder.Enabled = false;
                btn_CompleteOrder.Enabled = false;
                btn_cancelOrder.Enabled = false;

                lbl_currentOrderAmount.Text ="0";
                lbl_currentOrderFavorablePrice.Text = "0";
                lbl_curOrderTotal.Text = "0";
                tBox_actualMoney.Text = "0";
                tBox_change.Text = "0";
            }
            else
            {
                m_salesMgr.generateCurrentOrderDetails();
                lbl_currentOrderAmount.Text = m_salesMgr.CurrentOrder.Amount.ToString();
                lbl_currentOrderFavorablePrice.Text = "0";
                lbl_curOrderTotal.Text = m_salesMgr.CurrentOrder.Amount.ToString();
                tBox_actualMoney.Text = m_salesMgr.CurrentOrder.Amount.ToString();
                tBox_change.Text = "0";

                enableCurrentOrderBtnByState(m_salesMgr.CurrentOrder.State);
            }

            dgv_currentOrder.Refresh();
        }

        private void enableCurrentOrderBtnByState(OrderState state)
        {
            if (state == OrderState.FOR_PAY)
            {
                btn_continueOrder.Enabled = true;
                btn_CompleteOrder.Enabled = true;
                btn_cancelOrder.Enabled = true;
            }
            else
            {
                btn_continueOrder.Enabled = true;
                btn_CompleteOrder.Enabled = false;
                btn_cancelOrder.Enabled = false;
            }
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
            //picker_customerRegDate.Value = aCustomer.RegisterDate;
        }

        private void btn_CompleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                decimal money = Decimal.Parse(tBox_actualMoney.Text);
                
                decimal change = Decimal.Parse(tBox_change.Text);
                if (change < 0)
                {
                    WARNING("您的款还不够呢！");
                    return;
                }
                decimal recved = money - change;
                if (recved == 0)
                {
                    DialogResult rst = SelectionDlgShow("您确定订单免费么？");//, "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (rst != DialogResult.Yes) return;
                }
                if (money <0||recved < 0)
                {
                    WARNING("您确定要倒找客人钱吗？");
                    return;
                }
                m_salesMgr.completeCurrentOrder(recved);
                //enableCurrentOrderBtnByState(m_salesMgr.CurrentOrder.State);
                //m_salesMgr.updateOrderList();
                m_salesMgr.reloadOrderList();
                jump2OrderList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void jump2OrderList()
        {
            //m_salesMgr.reloadOrderList();
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
                Order anOrder = Order.findByID(data.id);
                if(anOrder != null)
                {
                    m_salesMgr.CartCustomer = Customer.findByID(anOrder.CustomerID);
                    m_salesMgr.CurrentOrder = anOrder;
                    enableCurrentOrderBtnByState(anOrder.State);
                    showCurrentOrder();
                    jump2CurrentOrder();
                }
                else
                {
                    WARNING("指定的订单不存在，请于管理员联系！");
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
            try
            {
                if (m_salesMgr.CurrentOrder != null && m_salesMgr.CurrentOrder.isCompleted)
                {
                    MessageBox.Show("订单已完成付款，无法撤销！");
                }
                else
                {
                    // 修改库存，并更新订单状态为撤销
                    m_salesMgr.cancelOrder(m_salesMgr.CurrentOrder);
                    //m_salesMgr.updateOrderList();
                    m_salesMgr.reloadOrderList();
                    jump2OrderList();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_actualMoney_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_salesMgr.CurrentOrder == null) return;

                decimal money = Decimal.Parse(tBox_actualMoney.Text);
                tBox_change.Text = (money - m_salesMgr.CurrentOrder.Amount).ToString();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 
        /// <summary>
        /// 继续购物响应函数。
        /// 继续购物，根据现有的订单信息生成购物车。
        /// 现有订单如果是待付款，则直接生成购物车；
        /// 现有订单如果是已完成或者取消订单，则根据完成订单新生成待付款订单，再生成购物车.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_curOrderContinue_Click(object sender, EventArgs e)
        {
            try
            {
                Order newOrder = m_salesMgr.CurrentOrder;
                if (m_salesMgr.CurrentOrder != null)
                {
                    if (m_salesMgr.CurrentOrder.State != OrderState.FOR_PAY)
                    {
                        newOrder = Order.NewOrder(m_salesMgr.CurrentOrder);

                        //// 使能三个按钮
                        //btn_CompleteOrder.Enabled = false;
                        //btn_continueOrder.Enabled = false;
                        //btn_cancelOrder.Enabled = false;
                        //enableCurrentOrderBtnByState(OrderState.FOR_PAY);
                    }
                    m_salesMgr.CartCustomer = Customer.findByID(newOrder.CustomerID);
                    m_salesMgr.CurrentOrder = newOrder;
                    m_salesMgr.generateCartDetail();
                    enableCurrentOrderBtnByState(OrderState.FOR_PAY);
                    showCustomerInfo(m_salesMgr.CartCustomer);
                    jump2CurrentCart();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
                m_salesMgr.initCart();
            }
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
                dgv_cartDetail.Refresh();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_cartNecUnits_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_cartDetail.CurrentRow == null) return;
                CartDetailRowData curRow = dgv_cartDetail.CurrentRow.DataBoundItem as CartDetailRowData;
                m_salesMgr.plusCartWineUnits(curRow.Code, -1);
                dgv_cartDetail.Refresh();
            }
            catch (ZeroCartException ex)
            {
                if (DialogResult.Yes == SelectionDlgShow("酒的数量为零，是否从购物车中删除？"))
                {
                    m_salesMgr.deleteCartDetailRow(dgv_cartDetail.CurrentRow.Index);
                }
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
                WARNING("出现异常：" + ex.Message);//, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    SelectionDlgShow("您确定删除级别" + data.Level.ToString() + "的信息么？"))//, "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
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

        #region page

        /// <summary>
        /// 隐藏所以管理员的页面（目前就是入库与配置页面）。
        /// </summary>
        private void hideAllAdminPages()
        {
            hideTabPage("tabPage_storage");
            hideTabPage("tp_supplier");
        }

        /// <summary>
        /// 隐藏tabpage
        /// </summary>
        /// <param name="name"></param>
        private void hideTabPage(string name)
        {
            TabPage page = tab_mainControl.TabPages[name];
            if (page == null) return;
            tab_mainControl.TabPages.Remove(page);
            keep_page(page);
        }

        /// <summary>
        /// 将页面保留到页面列表m_pages中。
        /// </summary>
        /// <param name="page"></param>
        private void keep_page(TabPage page)
        {
            if (page == null) return;
            
            TabPage temp = find_tab_page(page.Name);
            if (temp == null)
            {
                if (m_pages == null) m_pages = new List<TabPage>();
                m_pages.Add(page);
            }
        }

        private TabPage find_tab_page(string name)
        {
            TabPage page = null;

            if (m_pages == null) return null;

            foreach (TabPage aPage in m_pages)
            {
                if (aPage.Name == name)
                {
                    page = aPage;
                }
            }
            return page;
        }

        #endregion

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
        /// <summary>
        /// 保存客户信息.
        /// 首先按照客户名称查找客户，再按照电话查找，如果未找到则时新客户。
        /// 否则是对老客户信息的修改。
        /// </summary>
        private void doSaveCustomer()
        {
            if (checkCustomerValidtion())
            {
                Customer aCustomer = Customer.findByName(tBox_custermorName.Text); //m_salesMgr.findCustomerByName(tBox_custermorName.Text);
                if (aCustomer == null)
                {
                    aCustomer = Customer.findByPhNumber(tBox_customerPhNumber.Text);
                    if (aCustomer == null)
                    {
                        aCustomer = Customer.NewCustomer();
                    }
                    else
                    {
                        if (DialogResult.No == SelectionDlgShow("您确定要修改客户" + aCustomer.Name + "的信息么？"))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (DialogResult.No == SelectionDlgShow("您确定要修改客户" + aCustomer.Name + "的信息么？"))
                    {
                        return;
                    }
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

                // 老客户，则每次保存会根据其消费重新调整其级别。
                if (aCustomer.ID > 0)
                {
                    
                    int total = (int)aCustomer.queryOrderAmount();
                    MemberLevel matchLevel = MemberLevel.findByAmount(total);

                    if (matchLevel != null)
                    {
                        MemberLevel curLv = MemberLevel.findByLevel(aCustomer.MemberLevel);
                        if (matchLevel.MinConsuption > curLv.MinConsuption)
                        {
                            aCustomer.MemberLevel = matchLevel.Level;
                        }
                    }
                }
                m_salesMgr.addCustomer2DB(aCustomer);

                //m_salesMgr.CartCustomer = aCustomer;
            }
            //else
            //{
            //    WARNING("请输入正确的客户信息！");
            //}
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
                if (dgv_cartDetail.Columns[e.ColumnIndex].Name == "discountDGVOrderDetailTextBoxColumn"
                    || dgv_cartDetail.Columns[e.ColumnIndex].Name == "unitsDGVOrderDetailTextBoxColumn")
                {
                    tBox_CellEditer.Size = cell.Size;
                    tBox_CellEditer.Text = cell.Value.ToString();
                    tBox_CellEditer.Visible = true;
                    tBox_CellEditer.Location = calcCartDetailCellLocation(e.RowIndex,e.ColumnIndex);
                    tBox_CellEditer.Focus();
                }
                //else if (dgv_cartDetail.Columns[e.ColumnIndex].Name == "unitsDGVOrderDetailTextBoxColumn")
                //{
                //    //WARNING(cell.Value.ToString());
                //}
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
                string value = tBox_CellEditer.Text.Trim();
                DataGridViewCell cell = dgv_cartDetail.CurrentCell;
                CartDetailRowData data = dgv_cartDetail.Rows[cell.RowIndex].DataBoundItem as CartDetailRowData;
                if (dgv_cartDetail.Columns[cell.ColumnIndex].Name == "discountDGVOrderDetailTextBoxColumn")
                {
                    int discount = int.Parse(value);
                    if (discount < CurrentUser.MinDiscount)
                    {
                        WARNING("您的折扣权限不够，请与店长联系！");
                    }
                    else
                    {
                        data.Discount = discount;
                    }
                }
                else if (dgv_cartDetail.Columns[cell.ColumnIndex].Name == "unitsDGVOrderDetailTextBoxColumn")
                {
                    int units = int.Parse(value);
                    int delta = units - data.Units;
                    m_salesMgr.plusCartWineUnits(data.Code, delta);
                }
            }
            catch (ZeroCartException ex)
            {
                if (DialogResult.Yes == SelectionDlgShow("购物车中的数量为零或者负值，是否从购物车中删除？"))
                {
                    m_salesMgr.deleteCartDetailRow(dgv_cartDetail.CurrentRow.Index);
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
            return calcDGVCellLocation(dgv_cartDetail, rowIndex, columnIndex);
            //int x = dgv_cartDetail.Location.X;
            //int y = dgv_cartDetail.Location.Y;
            //int width = 0;
            //int height = dgv_cartDetail.ColumnHeadersHeight;

            //for (int row = 0; row < rowIndex; row++)
            //{
            //    height += dgv_cartDetail.Rows[row].Height;
            //}

            //for (int col = 0; col < columnIndex; col++)
            //{
            //    width += dgv_cartDetail.Columns[col].Width;
            //}

            //return new Point(x + width, y + height);
        }

        private void dgv_orderList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dgv_orderList.Columns[e.ColumnIndex].Name == "orderListOrderState")
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
            cbox_membLevel.SelectedIndex = 1;
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
        /// 函数按照 编码-》描述-》酒庄-》年份的顺序查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_salesWineCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    if (tBox_salesWineCode.Text.Trim() != "")
                    {
                        doSearchStorgeByCode();
                    }
                    else if (tBox_salesWineDescription.Text.Trim() != "")
                    {
                        doSearchStorgeByDescription();
                    }
                    else if(tBox_salesChateau.Text.Trim() != "")
                    {
                        tBox_salesChateau_KeyPress(sender, e);
                    }
                    else if (tBox_salesVintage.Text.Trim() != "")
                    {
                        tBox_salesVintage_KeyPress(sender, e);
                    }
                    else if (tBox_sale_appelation.Text.Trim() != "")
                    {
                        tBox_sale_appelation_KeyPress(sender, e);
                    }
                    else if (tbox_price_low.Text.Trim() != "" || tbox_price_high.Text.Trim() != "")
                    {
                        tbox_price_high_KeyPress(sender, e);
                    }
                    else if (tbox_sale_country.Text.Trim() != "")
                    {
                        tbox_sale_country_KeyPress(sender, e);
                    }
                    else
                    {
                        WARNING("请输入适当的信息以便查找！");
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
        /// 根据销售酒庄查找库存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_salesChateau_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string cheteau = this.tBox_salesChateau.Text.Trim();
                    
                    weitongDataSet1.storageDataTable table = Storage.findByChateau(cheteau);
                    m_salesMgr.bindStorageTable(table);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 根据销售的酒的年份查找库存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_salesVintage_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string vintage = this.tBox_salesVintage.Text.Trim();

                    weitongDataSet1.storageDataTable table = Storage.findByVintage(vintage);
                    m_salesMgr.bindStorageTable(table);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        

        private void tabCtrl_Order_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (e.TabPage.Name == "tabPg_Cart")
                {
                   // WARNING("tabPg_Cart");
                }
                else if (e.TabPage.Name == "tabPg_CurrentOrder")
                {
                    //WARNING("tabPg_CurrentOrder");
                    showCurrentOrder();
                }
                else if (e.TabPage.Name == "tabPg_OrderList")
                {
                    //m_salesMgr.reloadOrderList();
                    showStatistics();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void dgv_storageInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dgv_storageInfo.Columns[e.ColumnIndex].Name == "unitsDGVStorageInfoTextBoxColumn")
                {
                    int units = 1;
                    units = (int)e.Value;
                    Color backColor = dgv_storageInfo.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor;
                    //Color selectionBackColor = dgv_storageInfo.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor;
                    if (units <= 0)
                    {
                        backColor = Color.Yellow;
                        //selectionBackColor = Color.Yellow;
                    }
                    
                    dgv_storageInfo.Rows[e.RowIndex].DefaultCellStyle.BackColor = backColor;
                    //dgv_storageInfo.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = selectionBackColor;
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_deleteCartDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == SelectionDlgShow("你确定要从购物车中删除吗？"))
                {
                    m_salesMgr.deleteCartDetailRow(dgv_cartDetail.CurrentRow.Index);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void FrmMain_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {

                if (tab_mainControl.SelectedTab.Name == "tabPage_sale")
                {
                    clearSalesTBoxByKey(e.KeyCode);
                }
                else if (tab_mainControl.SelectedTab.Name == "tabPage_storage")
                {
                    clearStorageTBoxByKey(e.KeyCode);
                }

                // F1快捷键，设置code为焦点（便于输入code查找）
                // 快捷键设置: F1--编码查找，F2--名称查找，F3--庄园查找，F4--年份查找
                if (e.KeyCode == Keys.F1)
                {
                    if (tab_mainControl.SelectedTab.Name == "tabPage_sale")
                    {
                        tBox_salesWineCode.Focus();
                    }
                    else if (tab_mainControl.SelectedTab.Name == "tabPage_storage")
                    {
                        tBox_code.Focus();
                    }
                }
                else if (e.KeyCode == Keys.F2)
                {
                    if (tab_mainControl.SelectedTab.Name == "tabPage_sale")
                    {
                        this.tBox_salesWineDescription.Focus();
                    }
                    else if (tab_mainControl.SelectedTab.Name == "tabPage_storage")
                    {
                        this.tBox_description.Focus();
                    }
                }
                else if (e.KeyCode == Keys.F3)
                {
                    if (tab_mainControl.SelectedTab.Name == "tabPage_sale")
                    {
                        this.tBox_salesChateau.Focus();
                    }
                    else if (tab_mainControl.SelectedTab.Name == "tabPage_storage")
                    {
                        this.tBox_chateau.Focus();
                    }
                }
                else if(e.KeyCode == Keys.F4)
                {
                    if (tab_mainControl.SelectedTab.Name == "tabPage_sale")
                    {
                        this.tBox_salesVintage.Focus();
                    }
                    else if (tab_mainControl.SelectedTab.Name == "tabPage_storage")
                    {
                        this.tBox_Vintage.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 根据快捷键清除销售面板的查找条件
        /// </summary>
        /// <param name="keyCode"></param>
        private void clearSalesTBoxByKey(Keys keyCode)
        {
            if (keyCode == Keys.F1)
            {
                
                tBox_salesWineDescription.Text = "";
                tBox_salesChateau.Text = "";
                tBox_salesVintage.Text = "";
            }
            else if (keyCode == Keys.F2)
            {
                tBox_salesWineCode.Text = "";
                
                tBox_salesChateau.Text = "";
                tBox_salesVintage.Text = "";
            }
            else if (keyCode == Keys.F3)
            {
                tBox_salesWineCode.Text = "";
                tBox_salesWineDescription.Text = "";
                
                tBox_salesVintage.Text = "";
            }
            else if (keyCode == Keys.F4)
            {
                tBox_salesWineCode.Text = "";
                tBox_salesWineDescription.Text = "";
                tBox_salesChateau.Text = "";
                
            }
        }

        /// <summary>
        /// 清除所有销售面板的tbox
        /// </summary>
        private void clearAllSalesTBox()
        {
            tBox_salesWineCode.Text = "";
            tBox_salesVintage.Text = "";
            tBox_salesChateau.Text = "";
            tbox_sale_country.Text = "";
            tBox_sale_appelation.Text = "";
            tbox_price_low.Text = "";
            tbox_price_high.Text = "";
            tBox_salesWineDescription.Text = "";
        }

        /// <summary>
        /// 根据快捷键清除入库面板的查找条件
        /// </summary>
        /// <param name="keyCode"></param>
        private void clearStorageTBoxByKey(Keys keyCode)
        {
            // 清空所有的内容
            
            if (keyCode == Keys.F1
                || keyCode == Keys.F2
                || keyCode == Keys.F3
                || keyCode == Keys.F4)
            {
                clearAllStorageTBox();
                //this.tBox_description.Text = "";
                //tBox_chateau.Text = "";
                //tBox_Vintage.Text = "";
            }
            //else if (keyCode == Keys.F2)
            //{
            //    tBox_code.Text = "";

            //    tBox_chateau.Text = "";
            //    tBox_Vintage.Text = "";
            //}
            //else if (keyCode == Keys.F3)
            //{
            //    tBox_code.Text = "";
            //    tBox_description.Text = "";

            //    tBox_Vintage.Text = "";
            //}
            //else if (keyCode == Keys.F4)
            //{
            //    tBox_code.Text = "";
            //    tBox_description.Text = "";
            //    tBox_chateau.Text = "";

            //}
        }

        private void dgv_Users_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    int role_id = (int)e.Value;
                    //long ticks = DateTime.Now.Ticks;
                    // 如果通过查找id的方式，每次都查数据库，第一次大概40000*100ns（4毫秒）,以后基本稳定在10000*100ns（1毫秒）
                    // 1毫秒=1000微妙
                    // 1微妙=1000毫微秒（ns）
                    // 
                    e.Value = Role.getNameByID(role_id);    
                    //ticks = DateTime.Now.Ticks - ticks;
                    //WARNING(ticks.ToString());
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_deleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_Users.CurrentRow == null) return;
                User aUser = dgv_Users.CurrentRow.DataBoundItem as User;
                if (aUser == null) return;
                if (DialogResult.Yes == SelectionDlgShow("您确定删除用户 " + aUser.Name + " 的信息么？"))
                {
                    m_userMgr.deleteUser(aUser.ID);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_addUser_Click(object sender, EventArgs e)
        {
            try
            {
                FrmNewUser userFrm = new FrmNewUser();
                User newUser = User.NewUser();
                userFrm.User = newUser;
                if (DialogResult.OK == userFrm.ShowDialog())
                {
                    newUser.save();
                    m_userMgr.reLoadUsers();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_editUser_Click(object sender, EventArgs e)
        {
            try
            {
                User aUser = dgv_Users.CurrentRow.DataBoundItem as User;
                if (aUser == null) return;
                FrmNewUser userFrm = new FrmNewUser();

                userFrm.User = aUser;
                if (DialogResult.OK == userFrm.ShowDialog())
                {
                    aUser.save();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        #region order handle area
        private void dgv_orderList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCell cell = dgv_orderList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (dgv_orderList.Columns[e.ColumnIndex].Name == "orderListOrderComment")
                {
                    tBox_OrdersCellEditor.Size = cell.Size;
                    tBox_OrdersCellEditor.Text = cell.Value.ToString();
                    tBox_OrdersCellEditor.Visible = true;
                    tBox_OrdersCellEditor.Location = calcDGVCellLocation(dgv_orderList, e.RowIndex, e.ColumnIndex);
                    tBox_OrdersCellEditor.Focus();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 计算指定grid的某一cell的位置
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        private Point calcDGVCellLocation(DataGridView dgv, int rowIndex, int columnIndex)
        {
            int x = dgv.Location.X;
            int y = dgv.Location.Y;
            int cellX = dgv.GetCellDisplayRectangle(columnIndex, rowIndex, false).X;
            int cellY = dgv.GetCellDisplayRectangle(columnIndex, rowIndex, false).Y;

            //int width = 0;
            //int height = dgv.ColumnHeadersHeight;

            //for (int row = 0; row < rowIndex; row++)
            //{
            //    if(dgv.Rows[row].Displayed) height += dgv.Rows[row].Height;
            //}

            //for (int col = 0; col < columnIndex; col++)
            //{
            //    if(dgv.Columns[col].Displayed) width += dgv.Columns[col].Width;
            //}

            return new Point(x + cellX, y + cellY);
        }

        private void tBox_OrdersCellEditor_LostFocus(object sender,EventArgs e)
        {
            try
            {
                tBox_OrdersCellEditor.Visible = false;

                string value = tBox_OrdersCellEditor.Text.Trim();
                DataGridViewCell cell = dgv_orderList.CurrentCell;

                //DataRowView curRow = dgv_orderList.CurrentRow.DataBoundItem as DataRowView;
                //weitongDataSet1.orderRow data = curRow.Row as weitongDataSet1.orderRow;

                DataRowView row = dgv_orderList.Rows[cell.RowIndex].DataBoundItem as DataRowView;
                weitongDataSet1.orderRow data = row.Row as weitongDataSet1.orderRow;
                int id = data.id;
                if (dgv_orderList.Columns[cell.ColumnIndex].Name == "orderListOrderComment")
                {
                    Order.updateComment(data.id, value);
                }
                m_salesMgr.reloadOrderList();
                dgv_orderList_setSelected(id);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 在订单列表中，设置指定的订单为选中行。如果该该订单不在列表中，则不作任何操作。
        /// </summary>
        /// <param name="code"></param>
        private void dgv_orderList_setSelected(int id)
        {
            foreach (DataGridViewRow row in dgv_orderList.Rows)
            {
                DataRowView dataRowView = row.DataBoundItem as DataRowView;
                weitongDataSet1.orderRow dataRow = dataRowView.Row as weitongDataSet1.orderRow;
                if (dataRow.id == id)
                {
                    row.Selected = true;
                    scrollGrid(dgv_orderList);
                    break;
                }
            }
        }

        /// <summary>
        /// 将grid中选中的行移到可视区,选中的行在中间位置。
        /// </summary>
        /// <param name="grid"></param>
        private void scrollGrid(DataGridView grid)
        {
            int halfWay = (grid.DisplayedRowCount(false) / 2);
            if (grid.FirstDisplayedScrollingRowIndex + halfWay > grid.SelectedRows[0].Index ||
                (grid.FirstDisplayedScrollingRowIndex + grid.DisplayedRowCount(false) - halfWay) <= grid.SelectedRows[0].Index)
            {
                int targetRow = grid.SelectedRows[0].Index;

                targetRow = Math.Max(targetRow - halfWay, 0);
                grid.FirstDisplayedScrollingRowIndex = targetRow;

            }
        }

        private void tBox_OrdersCellEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    tBox_OrdersCellEditor_LostFocus(tBox_OrdersCellEditor, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 根据条件查找订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SearchOrder_Click(object sender, EventArgs e)
        {
            try
            {
                string order_id_str = tBox_OrderID.Text.Trim();
                string cust_name = tBox_OrderCustomer.Text.Trim();
                string code = tBox_OrderCode.Text.Trim();
                string description = tBox_OrderDescription.Text.Trim();
                string amount_low_str = tBox_OrderAmountLow.Text.Trim();
                string amount_high_str = tBox_OrderAmount_high.Text.Trim();
                string order_date_low_str = picker_OrderDate_low.Text.Trim();
                string order_date_high_str = picker_OrderDate_high.Text.Trim();
                
                string wine = code;
                List<Wine> wines = new List<Wine>();
                if (wine == "")
                {
                    wine = description;
                    wines = Wine.findByDescription(wine);
                }
                else
                {
                    Wine aWine = Wine.findByCode(wine);
                    if (aWine == null)
                    {
                        WARNING("未能找到编码为"+wine+"的相关信息！");
                        return;
                    }

                    wines.Add(aWine);
                }

                if (wine!="" && wines.Count == 0)
                {
                    WARNING("未找到酒的信息！");
                    return;
                }

                int amount_low = int.MinValue;
                int amount_high = int.MaxValue;
                if (amount_low_str != "") amount_low = int.Parse(amount_low_str);
                if (amount_high_str != "") amount_high = int.Parse(amount_high_str);
                

                DateTime order_date_low = picker_OrderDate_low.Value;
                DateTime order_date_high = picker_OrderDate_high.Value;
                
                //if (order_date_low_str != "") order_date_low = DateTime.Parse(order_date_low_str);
                //if (order_date_high_str != "") order_date_high = DateTime.Parse(order_date_high_str);
                

                List<Order> list = new List<Order>();
                // order_id存在时忽略其他条件。
                if (order_id_str != "")
                {
                    int id = int.Parse(order_id_str);
                    Order anOrder = Order.findByID(id);
                    
                    if (anOrder != null) list.Add(anOrder);
                }
                // C4,4
                else if (cust_name != ""&&wine!="")
                {
                    
                    Customer cust = Customer.findByName(cust_name);
                    if (cust == null) 
                    {
                        WARNING("找不到客户"+cust_name+"的信息！");
                        return;
                    }
                    list = Order.find_by_customer_wine_date(cust.ID,wines,order_date_low,order_date_high,amount_low,amount_high);
                }
                //C4,3
                else if (cust_name != "")
                {
                    Customer cust = Customer.findByName(cust_name);
                    if (cust == null)
                    {
                        WARNING("找不到客户" + cust_name + "的信息！");
                        return;
                    }
                    list = Order.find_by_customer_date(cust.ID, order_date_low, order_date_high,amount_low,amount_high);
                }
                else if (wine != "")
                {
                    list = Order.find_by_wine_date(wines, order_date_low, order_date_high, amount_low, amount_high);
                }
                else
                {
                    list = Order.find_by_date(order_date_low, order_date_high, amount_low, amount_high);
                }
                m_salesMgr.OrderTable = util.Order2Table(list);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }


        /// <summary>
        /// 初始化订单查询中的日期选择其，以当前月的月初和月末分别初始化两个picker
        /// </summary>
        private void init_order_list_timepiker()
        {
            picker_OrderDate_low.Value = util.FirstDayOfMonth(DateTime.Now);
            picker_OrderDate_high.Value = util.LastDayOfMonth(DateTime.Now);
        }

        private void tBox_OrderID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    btn_SearchOrder_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_OrderCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    btn_SearchOrder_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_OrderCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    btn_SearchOrder_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_OrderDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    btn_SearchOrder_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_OrderAmount_high_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    btn_SearchOrder_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        #endregion


        private void dgv_storage_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCell cell = dgv_storage.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (dgv_storage.Columns[e.ColumnIndex].Name != "codeDataGridViewTextBoxColumn"
                    && dgv_storage.Columns[e.ColumnIndex].Name != "unitsDataGridViewTextBoxColumn")
                {
                    tBox_StorageEditer.Size = cell.Size;
                    tBox_StorageEditer.Text = cell.Value.ToString();
                    tBox_StorageEditer.Visible = true;
                    tBox_StorageEditer.Location = calcDGVCellLocation(dgv_storage, e.RowIndex, e.ColumnIndex);
                    tBox_StorageEditer.Focus();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_StorageEditer_LostFocus(object sender, EventArgs e)
        {
            try
            {
                tBox_StorageEditer.Visible = false;

                string value = tBox_StorageEditer.Text.Trim();
                DataGridViewCell cell = dgv_storage.CurrentCell;
                //cell.Value = value;
                

                DataRowView row = dgv_storage.Rows[cell.RowIndex].DataBoundItem as DataRowView;
                weitongDataSet1.storageRow data = row.Row as weitongDataSet1.storageRow;
                string code = data.code;

                // 酒除了编码和数量以外的所有信息都可以修改。

                // 如果data.name是DBNULL，则按照空值""来查找。
                string spler_name = "";
                if (!data.IsnameNull())
                {
                    spler_name = data.name;
                }
                Supplier spler = Supplier.findByName(spler_name);
                int spler_id = -1;
                if (spler != null) spler_id = spler.ID;

                if (dgv_storage.Columns[cell.ColumnIndex].Name == "chateauDataGridViewTextBoxColumn")
                {
                    Wine.update(data.code, value, data.country, data.appellation, data.quality, data.vintage.ToString(), data.description, data.bottle, data.score);
                    
                    //Storage.update(data.code, spler_id, data.price, data.retailprice);
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "discriptionDataGridViewTextBoxColumn")
                {
                    Wine.update(data.code, data.chateau, data.country, data.appellation, data.quality, data.vintage.ToString(), value, data.bottle, data.score);
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "vintageDataGridViewTextBoxColumn")
                {
                    Wine.update(data.code, data.chateau, data.country, data.appellation, data.quality, value, data.description, data.bottle, data.score);
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "appelationDataGridViewTextBoxColumn")
                {
                    Wine.update(data.code, data.chateau, data.country, value, data.quality, data.vintage.ToString(), data.description, data.bottle, data.score);
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "qualityDataGridViewTextBoxColumn")
                {
                    Wine.update(data.code, data.chateau, data.country, data.appellation, value, data.vintage.ToString(), data.description, data.bottle, data.score);
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "bottleDataGridViewTextBoxColumn")
                {
                    Wine.update(data.code, data.chateau, data.country, data.appellation, data.quality, data.vintage.ToString(), data.description, value, data.score);
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "scoreDataGridViewTextBoxColumn")
                {
                    Wine.update(data.code, data.chateau, data.country, data.appellation, data.quality, data.vintage.ToString(), data.description, data.bottle, value);
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "countryDataGridViewTextBoxColumn")
                {
                    Wine.update(data.code, data.chateau, value, data.appellation, data.quality, data.vintage.ToString(), data.description, data.bottle, data.score);
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "supplierDataGridViewTextBoxColumn")
                {
                    if (spler_name != value)
                    {
                        if (DialogResult.Yes == SelectionDlgShow("您是要将供应商由 "+ spler_name + " 改为 " + value+" 吗？"))
                        {
                            spler = Supplier.findByName(value);
                            //spler_id = -1;
                            if (spler == null)
                            {
                                if (DialogResult.Yes == SelectionDlgShow("供应商" + value + "信息不在库中，你要保存供应商信息么？"))
                                {
                                    spler = Supplier.NewSupplier();
                                    spler.Name = value;
                                    spler.save();
                                }
                            }
                            if (spler != null) spler_id = spler.ID;
                            Storage.update(data.code, spler_id, data.price, data.retailprice);
                        }
                    }
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "priceDataGridViewTextBoxColumn")
                {
                    decimal price = decimal.Parse(value);
                    Storage.update(data.code, spler_id, price, data.retailprice);
                }
                else if (dgv_storage.Columns[cell.ColumnIndex].Name == "retailpriceDataGridViewTextBoxColumn")
                {
                    decimal retail_price = decimal.Parse(value);
                    Storage.update(data.code, spler_id, data.price, retail_price);
                }

                m_wineStorageMgr.reloadStorage();

                dgv_storage_setSelected(code);
                //m_salesMgr.reloadOrderList();
                //dgv_orderList_setSelected(id);
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_StorageEditer_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    tBox_StorageEditer_LostFocus(sender, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void btn_GenerateCode_Click(object sender, EventArgs e)
        {
            try
            {
                FrmGenerateCode frmGCode = new FrmGenerateCode();
                if (DialogResult.OK == frmGCode.ShowDialog())
                {
                    string randomCode = frmGCode.Code;
                    if (randomCode == null || randomCode.Length != 13)
                    {
                        WARNING("编码长度不是13位的。");
                    }
                    else
                    {
                        tBox_code.Text = randomCode;
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void btn_ChangeCode_Click(object sender, EventArgs e)
        {
            try
            {
                FrmToolChangeCode frm_tool = new FrmToolChangeCode();
                frm_tool.Show();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void btn_WineList_Click(object sender, EventArgs e)
        {
            try
            {
                FrmWineList list = new FrmWineList();
                list.DataSet = weitongDataSet1;
                list.ShowDialog();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void btn_checkCustomers_Click(object sender, EventArgs e)
        {
            try
            {
                FrmCustomerList custFrm = new FrmCustomerList();
                if (DialogResult.OK == custFrm.ShowDialog())
                {
                    m_salesMgr.CartCustomer = custFrm.SelectedCustomer;
                    showCustomerInfo(m_salesMgr.CartCustomer);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }


        /// <summary>
        /// 删除库存记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi_delStorage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentUser.isAdministrator())
                {
                    WARNING("您无权删除库存记录，请与店长联系！");
                    return;
                }

                if (dgv_storage.CurrentRow == null) return;
                DataRowView dataRowView = dgv_storage.CurrentRow.DataBoundItem as DataRowView;
                weitongDataSet1.storageRow dataRow = dataRowView.Row as weitongDataSet1.storageRow;

                if (DialogResult.Yes == SelectionDlgShow("您确定要删除吗？\r编号:" + dataRow.code + "\r名称：" + dataRow.description))
                {
                    Storage.deleteAllStorage(dataRow.code);
                    m_wineStorageMgr.reloadStorage();
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }


        #region sale page search box handler
        /// <summary>
        /// 查找产区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_sale_appelation_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string appelation = this.tBox_sale_appelation.Text.Trim();
                    if (appelation == "")
                    {
                        WARNING("请输入产区信息！");
                        return;
                    }
                    weitongDataSet1.storageDataTable table = Storage.findByAppelation(appelation);
                    m_salesMgr.bindStorageTable(table);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tbox_sale_country_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string country = this.tbox_sale_country.Text.Trim();
                    if (country == "")
                    {
                        WARNING("请输入国家信息！");
                        return;
                    }
                    weitongDataSet1.storageDataTable table = Storage.findByCountry(country);
                    m_salesMgr.bindStorageTable(table);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tbox_price_low_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if(e.KeyChar == '\r') tbox_price_high.Focus();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tbox_price_high_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    int low_price = Int32.MinValue;
                    int high_price = Int32.MaxValue;

                    string low_str = tbox_price_low.Text.Trim();
                    string high_str = tbox_price_high.Text.Trim();

                    if (low_str != "") low_price = Int32.Parse(low_str);
                    if (high_str != "") high_price = Int32.Parse(high_str);

                    if (low_str == "" && high_str == "")
                    {
                        WARNING("请输入价格！");
                        return;
                    }

                    weitongDataSet1.storageDataTable table = Storage.findByPrice(low_price, high_price);
                    m_salesMgr.bindStorageTable(table);
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_sale_appelation_Enter(object sender, EventArgs e)
        {
            try
            {
                clearAllSalesTBox();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_salesVintage_Enter(object sender, EventArgs e)
        {
            try
            {
                clearAllSalesTBox();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tbox_sale_country_Enter(object sender, EventArgs e)
        {
            try
            {
                clearAllSalesTBox();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tbox_price_low_Enter(object sender, EventArgs e)
        {
            try
            {
                clearAllSalesTBox();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_salesWineDescription_Enter(object sender, EventArgs e)
        {
            try
            {
                clearAllSalesTBox();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_salesChateau_Enter(object sender, EventArgs e)
        {
            try
            {
                clearAllSalesTBox();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        #endregion

        #region storage page search box handler

        /// <summary>
        /// 清除入库面板上所有项的内容
        /// </summary>
        private void clearAllStorageTBox()
        {
            tBox_chateau.Text = "";

            tBox_code.Text = "";

            tBox_Vintage.Text = "";

            tBox_appellation.Text = "";

            tBox_country.Text = "";
            tBox_score.Text = "";
            tBox_bottle.Text = "";
            tBox_quality.Text = "";

            tBox_description.Text = "";

            tBox_supplier.Text = "";

            tBox_retailprice.Text = "";
            tBox_units.Text = "";
            tBox_price.Text = "";
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
                        else
                        {
                            WARNING("未找到酒的信息！");
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
                    if (description != "")
                    {
                        m_wineStorageMgr.WineStorageGridView.DataSource = Storage.findByDescription(description);
                    }
                    else
                    {
                        tBox_chateau_KeyPress(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 根据入库面板的酒庄信息查找库存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_chateau_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string chateau = this.tBox_chateau.Text.Trim();
                    if (chateau != "")
                    {
                        m_wineStorageMgr.WineStorageGridView.DataSource = Storage.findByChateau(chateau);
                    }
                    else
                    {
                        tBox_Vintage_KeyPress(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        /// <summary>
        /// 根据入库面板的年份信息查找库存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBox_Vintage_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string vintage = this.tBox_Vintage.Text.Trim();
                    if (vintage != "")
                    {
                        m_wineStorageMgr.WineStorageGridView.DataSource = Storage.findByVintage(vintage);
                    }
                    else
                    {
                        tBox_appellation_KeyPress(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_appellation_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string appellation = this.tBox_appellation.Text.Trim();
                    if (appellation != "")
                    {
                        m_wineStorageMgr.WineStorageGridView.DataSource = Storage.findByAppelation(appellation);
                    }
                    else
                    {
                        tBox_country_KeyPress(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_country_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string country = this.tBox_country.Text.Trim();
                    if (country != "")
                    {
                        m_wineStorageMgr.WineStorageGridView.DataSource = Storage.findByCountry(country);
                    }
                    else
                    {
                        tBox_supplier_KeyPress(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tBox_supplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == '\r')
                {
                    string supplier = this.tBox_supplier.Text.Trim();
                    //if (supplier != "")
                    //{
                    m_wineStorageMgr.WineStorageGridView.DataSource = Storage.findBySupplier(supplier);
                    //}
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        #endregion

        

        

    }
}
