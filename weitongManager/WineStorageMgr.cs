using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    class WineStorageMgr
    {
        public void init()
        {
            m_dataAdapter.ClearBeforeFill = true;
            try
            {
                m_dataAdapter.Fill(m_dataset.storage);
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            bindData();
        }

        // 添加库存信息
        // 
        public void addStorage(string code, string chateau = null, string country = null, string appellation = null, 
            string quality = null, string vintage = null, string description = null, string bottle = null, string score = null,
            int supplierID = -1, decimal price = 0, decimal caseprice = 0, decimal retailprice = 0, int units = 0)
        {
            // 如果已经存在酒的库存记录，则更新wines的相关信息，包括wines和storage库存信息。
            if (Storage.existsWine(code))
            {
                Wine.update(code, chateau, country, appellation, quality, vintage, description, bottle, score);
                Storage.update(code, supplierID, price, retailprice);
                
                Storage.insert_his_storage(code, supplierID, price, retailprice, units);
                Storage.plusUnits(code, units);
            }
            // 如果没有库存记录，但是已经有酒的信息了，则更新酒的信息，再插入库存记录
            else if (Wine.existsWine(code))
            {
                Wine.update(code, chateau, country, appellation, quality, vintage, description, bottle, score);
                Storage.insert(code, supplierID, price, retailprice, units);
            }
            // 否则先插入酒记录，再插入库存记录
            else
            {
                Wine.insert(code, chateau, country, appellation, quality, vintage, description, bottle, score);
                Storage.insert(code, supplierID, price, retailprice, units);
            }
            
            //m_dataset.storage.AcceptChanges();
            m_dataAdapter.ClearBeforeFill = true;
            m_dataAdapter.Fill(m_dataset.storage);
            
        }

        // 删除storage表中的相关记录
        // 删除storage表中的数据并不会删除wine和supplier表中的数据。
        public void deleteCurrentStorage()
        {
            DataRowView dataRowView = m_wineStorageGrid.CurrentRow.DataBoundItem as DataRowView;
            weitongDataSet1.storageRow dataRow = dataRowView.Row as weitongDataSet1.storageRow;
            
            deleteStorageRow(dataRow.id);
            //m_dataset.AcceptChanges();
            m_dataAdapter.ClearBeforeFill = true;
            m_dataAdapter.Fill(m_dataset.storage);

            m_wineStorageGrid.Refresh();
        }

        public weitongDataSet1.winesRow findWineByCode(string code)
        {
            string qStr = @"SELECT   id, code, chateau, country, appellation, quality, vintage, description, bottle, score
                            FROM wines
                            WHERE code = @code";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = m_dataAdapter.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;

            weitongDataSet1TableAdapters.winesTableAdapter wineAdapter = new weitongDataSet1TableAdapters.winesTableAdapter();
            // 先保存原SelectCommand,执行完本查询后再回复为原命令。
            MySqlCommand temp = wineAdapter.Adapter.SelectCommand;
            wineAdapter.Adapter.SelectCommand = queryCmd;
            weitongDataSet1.winesDataTable table = new weitongDataSet1.winesDataTable();
            wineAdapter.Adapter.Fill(table);
            wineAdapter.Adapter.SelectCommand = temp;

            if (table != null && table.Rows.Count != 0)
            {
                return table[0];
            }
            else
            {
                return null;
            }
        }


        public void reloadStorage()
        {
            m_dataAdapter.ClearBeforeFill = true;
            m_dataAdapter.Fill(m_dataset.storage);
        }

        //================================= 属性 =================================
        public DataGridView WineStorageGridView
        {
            get
            {
                return m_wineStorageGrid;
            }
            set
            {
                m_wineStorageGrid = value;
            }
        }

        public weitongDataSet1 DataSet
        {
            get { return m_dataset; }
            set { m_dataset = value; }
        }

        public weitongDataSet1TableAdapters.storageTableAdapter Adapter
        {
            get { return m_dataAdapter; }
            set { m_dataAdapter = value; }
        }


        //================================= 私有方法 =================================
        private void bindData()
        {
            m_wineStorageGrid.AutoGenerateColumns = false;
            m_wineStorageGrid.DataSource = m_dataset.storage;
           
            // bind column
            m_wineStorageGrid.Columns["codeDataGridViewTextBoxColumn"].DataPropertyName = "code";
            m_wineStorageGrid.Columns["chateauDataGridViewTextBoxColumn"].DataPropertyName = "chateau";
            m_wineStorageGrid.Columns["vintageDataGridViewTextBoxColumn"].DataPropertyName = "vintage";
            m_wineStorageGrid.Columns["appelationDataGridViewTextBoxColumn"].DataPropertyName = "appellation";
            m_wineStorageGrid.Columns["qualityDataGridViewTextBoxColumn"].DataPropertyName = "quality";
            m_wineStorageGrid.Columns["bottleDataGridViewTextBoxColumn"].DataPropertyName = "bottle";
            m_wineStorageGrid.Columns["scoreDataGridViewTextBoxColumn"].DataPropertyName = "score";
            m_wineStorageGrid.Columns["supplierDataGridViewTextBoxColumn"].DataPropertyName = "name";
            m_wineStorageGrid.Columns["unitsDataGridViewTextBoxColumn"].DataPropertyName = "units";
            m_wineStorageGrid.Columns["priceDataGridViewTextBoxColumn"].DataPropertyName = "price";
            m_wineStorageGrid.Columns["retailpriceDataGridViewTextBoxColumn"].DataPropertyName = "retailprice";
            m_wineStorageGrid.Columns["countryDataGridViewTextBoxColumn"].DataPropertyName = "country";
            m_wineStorageGrid.Columns["discriptionDataGridViewTextBoxColumn"].DataPropertyName = "description";
        }

        // 通过storage表的id，删除storage记录
        private void deleteStorageRow(int id)
        {
            //string deleteStr = @"DELETE FROM STORAGE WHERE ID=@id";
            //MySql.Data.MySqlClient.MySqlCommand deleteCmd = new MySql.Data.MySqlClient.MySqlCommand();
            //deleteCmd.Connection = m_dataAdapter.Connection;
            //deleteCmd.CommandText = deleteStr;
            //deleteCmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = id;
            //deleteCmd.Connection.Open();
            //IAsyncResult rest = deleteCmd.BeginExecuteNonQuery();
            //deleteCmd.EndExecuteNonQuery(rest);
            //deleteCmd.Connection.Close();
        }


        private DataGridView m_wineStorageGrid = null;
        private weitongDataSet1 m_dataset = null;
        private weitongDataSet1TableAdapters.storageTableAdapter m_dataAdapter = null;
    }
}
