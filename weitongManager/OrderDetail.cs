using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weitongManager
{
    class OrderDetail
    {
        private int m_id;
        private int m_order_id;
        private string m_code;
        private int m_units;
        private decimal m_knock_price;
        private int discount;

        public int ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public int OrderID
        {
            get { return m_order_id; }
            set { m_order_id = value; }
        }

        public string Code
        {
            get { return m_code; }
            set { m_code = value; }
        }

        public int Units
        {
            get { return m_units; }
            set { m_units = value; }
        }

        public decimal KnockDownPrice
        {
            get { return m_knock_price; }
            set { m_knock_price = value; }
        }

        public int Discount
        {
            get { return discount; }
            set { discount = value; }
        }
    }
}
