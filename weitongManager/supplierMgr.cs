﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace weitongManager
{
    class SupplierMgr
    {
        public void init()
        {
            m_supplierTableAdapter.Fill(m_dataSet.supplier);
        }

        public void searchSupplier(string name)
        {
            if (name == null || name == "")
            {
                // 如果输入的名称为空，则显示所有的供应商
                m_supplierTableAdapter.Fill(m_dataSet.supplier);
                //m_gridView.Refresh();
                return;
            }

            Supplier splier = Supplier.findByName(name);
            if (splier == null)
            {
                // 如果未找到，则清空
                m_dataSet.supplier.Clear();
                return;
            }

            m_supplierTableAdapter.Fill(m_dataSet.supplier);
            weitongDataSet1.supplierRow aRow = m_dataSet.supplier.FindByid(splier.ID);
            weitongDataSet1.supplierDataTable table = new weitongDataSet1.supplierDataTable();
            table.ImportRow(aRow);
            m_dataSet.supplier.Clear();
            aRow = table[0];
            m_dataSet.supplier.ImportRow(aRow);

        }

        public int insertSupplier(string name)
        {
            try
            {
                m_supplierTableAdapter.Insert(name);
                m_dataSet.supplier.AcceptChanges();
                m_supplierTableAdapter.Fill(m_dataSet.supplier);
                return 0;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public int deleteCurrentSupplier() 
        {
            DataRowView dataRowView = m_gridView.CurrentRow.DataBoundItem as DataRowView;
            DataRow dataRow = dataRowView.Row;
            try 
            {
                dataRow.Delete();
                m_supplierTableAdapter.Update(m_dataSet.supplier);
                
                m_dataSet.supplier.AcceptChanges();
                return 0;
            }
            catch (Exception e)
            {
                
            }
            return -1;
        }


        //================================= 属性 =================================
        public weitongDataSet1 DataSet 
        {
            get 
            {
                return m_dataSet;
            }
            set
            {
                m_dataSet = value;
            }
        }

        public weitongDataSet1TableAdapters.supplierTableAdapter SupplierTableAdapter
        {
            get 
            {
                return m_supplierTableAdapter;
            }
            set
            {
                m_supplierTableAdapter = value;
            }
        }

        public DataGridView GridViewControl
        {
            get
            {
                return m_gridView;
            }
            set
            {
                m_gridView = value;
            }
        }

        private weitongDataSet1 m_dataSet = null;
        private weitongDataSet1TableAdapters.supplierTableAdapter m_supplierTableAdapter = null;
        private DataGridView m_gridView = null; 
    }
}
