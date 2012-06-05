using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weitongManager
{
   
    class HisStorage
    {
        private int m_id;
        private string m_code;
        private string m_supplier;
        private decimal m_price;
        private decimal m_retail_price;
        private int m_units;
        private DateTime m_effectdate;
        private string m_description;

        public int ID
        {
            get { return m_id; }
            set { m_id = value; }
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

        public string Supplier
        {
            get { return m_supplier; }
            set { m_supplier = value; }
        }

        public decimal Price
        {
            get { return m_price; }
            set { m_price = value; }
        }

        public decimal RetailPrice
        {
            get { return m_retail_price; }
            set { m_retail_price = value; }
        }

        public int Units
        {
            get { return m_units; }
            set { m_units = value; }
        }

        public decimal Amount
        {
            get { return m_price * m_units; }
        }

        public DateTime EffectDate
        {
            get { return m_effectdate; }
            set { m_effectdate = value; }
        }
    }
}
