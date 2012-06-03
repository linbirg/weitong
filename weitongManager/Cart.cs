using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace weitongManager
{
    class Cart
    {
        private User m_user = null;
        private Customer m_customer = null;
        private BindingList<CartDetailRowData> m_cartDetailList = null;


        #region public property

        public Customer Customer
        {
            get { return m_customer; }
            set {
                m_customer = value;
                if (m_customer != null) refreshDiscount();
            }
        }

        

        public BindingList<CartDetailRowData> Items
        {
            get { return m_cartDetailList; }
        }

        #endregion


        #region public methord

        /// <summary>
        /// 生成订单信息(订单和订单详情)，订单状态为未付款(当完成付款后，修改状态为完成)。
        /// </summary>
        public Order calcCart()
        {
            Order anOrder = null;
            //if (CurrentOrder != null)
            //{
            //    anOrder = CurrentOrder;
            //}
            //else
            //{
            anOrder = Order.NewOrder();
            //}

            anOrder.CustomerID = m_customer.ID;
            //anOrder.UserID = m_user.ID;
            anOrder.EffectDate = DateTime.Now;
            anOrder.State = OrderState.FOR_PAY;

            decimal amount = 0;
            anOrder.emptyDetails();
            if (m_cartDetailList == null) return anOrder;
            foreach (CartDetailRowData item in m_cartDetailList)
            {
                amount += item.Amount;
                anOrder.addDetail(item.Code, item.Units, item.Memberprice, item.Discount);
            }

            //anOrder.save();

            return anOrder;

            //reloadStorageInfo();
            //CurrentOrderID = CurrentOrder.ID;
            //bindCurrentOrder();
        }

        public delegate void WineAddedHander(object sender, EventArgs e);
        public event WineAddedHander WineAdded;

        /// <summary>
        /// 向购物车中加入指定数量的酒。如果酒已经在购物车中，则增加其数量。
        /// </summary>
        /// <param name="item"></param>
        public void addWine(string code, int units)
        {
            if (code == null || code == "") throw new InvalidWineCodeException();
            

            CartDetailRowData aItem = findCode(code);
            // 如果酒已经在购物车中，则增加酒的数量；如果酒不在购物车中，则添加指定数量的酒
            if (aItem != null)
            {
                if (aItem.Units + units > 0)
                {
                    aItem.Units += units;
                }
                else
                    throw new ZeroCartException();
            }
            else
            {
                if (units < 0) throw new ZeroCartException();
                
                weitongDataSet1.storageRow storage = Storage.findByCode(code);
                if (storage == null) throw new InvalidWineCodeException();             // 找不到指定的酒

                int discount = 100;
                if (m_customer != null) discount = m_customer.Discount;
                aItem = new CartDetailRowData(storage.code, storage.description, storage.bottle, storage.retailprice, discount, units);
                if (m_cartDetailList == null) m_cartDetailList = new BindingList<CartDetailRowData>();
                m_cartDetailList.Add(aItem);
            }

            WineAdded(this, new EventArgs());
        }

        /// <summary>
        /// 清空购物车中的酒
        /// </summary>
        public void Clear()
        {
            if (m_cartDetailList != null) m_cartDetailList.Clear();
        }

        /// <summary>
        /// 设置购物车中酒的折扣。
        /// </summary>
        /// <param name="code">酒的编码。</param>
        /// <param name="discount">折扣，折扣必须大于等于0。负的折扣没有意义。</param>
        public void setDiscount(string code, int discount)
        {
            if (discount < 0) throw new InvalidArgumentException("折扣不能为负值");
            CartDetailRowData item = findCode(code);
            if (item != null) item.Discount = discount;
        }

        public CartDetailRowData removeWine(string code)
        {
            CartDetailRowData item = findCode(code);
            m_cartDetailList.Remove(item);
            return item;
        }

        #endregion

        #region private method
        
        /// <summary>
        /// 在购物车中查找code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private CartDetailRowData findCode(string code)
        {
            if (m_cartDetailList == null || code == null || code == "") return null;

            int i = 0;
            for (; i < m_cartDetailList.Count; i++)
            {
                if (m_cartDetailList[i].Code == code) break;
            }

            if (i == m_cartDetailList.Count) return null;       // 指定的酒不在购物车中。

            return m_cartDetailList[i];
        }

        /// <summary>
        /// 根据当前的客户信息更新其享有的折扣。
        /// 如果没有客户，则不享受任何折扣。
        /// </summary>
        private void refreshDiscount()
        {
            int discount = 100;
            if (m_customer != null) discount = m_customer.Discount;
            if (m_cartDetailList != null)
            {
                foreach (CartDetailRowData item in m_cartDetailList)
                {
                    item.Discount = discount;
                }
            }
        }

        #endregion

    }
}
