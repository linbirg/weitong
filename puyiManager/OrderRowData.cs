using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weitongManager
{
    class OrderRowData
    {
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


        public decimal KnockDownPrice
        {
            get { return m_knockdownPrice; }
            set { m_knockdownPrice = value; }
        }

        public decimal FavorablePrice
        {
            get { return m_favorablePrice; }
            set { m_favorablePrice = value; }
        }

        public int Units
        {
            get { return m_units; }
            set { m_units = value; }
        }

        public decimal Amount
        {
            get { return KnockDownPrice * Units; }
        }

        private string m_code;
        private string m_description;
        private decimal m_knockdownPrice;
        private decimal m_favorablePrice;
        private int m_units;
    }
}
