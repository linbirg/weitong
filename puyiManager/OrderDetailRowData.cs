using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weitongManager
{
    class CartDetailRowData
    {
        public CartDetailRowData()
        {
            // 默认构造函数
        }
        public CartDetailRowData(string code, string description, string bottle, decimal price, int discount = 100, int units = 1)
        {
            m_code = code;
            m_description = description;
            m_bottle = bottle;
            m_price = price;
            m_discount = discount;
            m_units = units;
        }

        public string Code
        {
            get { return m_code; }
            set { m_code = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public string Bottle
        {
            get { return m_bottle; }
            set { m_bottle = value; }
        }

        public decimal Price
        {
            get { return m_price; }
            set { m_price = value; }
        }

        public int Discount
        {
            get { return m_discount; }
            set { m_discount = value; }
        }

        public int Units
        {
            get { return m_units; }
            set { m_units = value; }
        }

        public decimal Memberprice
        {
            get { return m_price * m_discount / 100; }
        }

        public decimal Amount
        {
            get { return Memberprice * m_units; }
        }

        private string m_code = null;
        private string m_description = null;
        private string m_bottle = null;
        private decimal m_price = 0;
        private int m_discount = 100;
        private int m_units = 0;
    }
}
