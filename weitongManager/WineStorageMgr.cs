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
            if (existsWineInStorage(code))
            {
                updateWine(code, chateau, country, appellation, quality, vintage, description, bottle, score);
                updateStorage(code, supplierID, price, retailprice, units);
            }
            // 如果没有库存记录，但是已经有酒的信息了，则更新酒的信息，再插入库存记录
            else if (existsWineInWines(code))
            {
                updateWine(code, chateau, country, appellation, quality, vintage, description, bottle, score);
                insertStorage(code, supplierID, price, retailprice, units);
            }
            // 否则先插入酒记录，再插入库存记录
            else
            {
                insertWine(code, chateau, country, appellation, quality, vintage, description, bottle, score);
                insertStorage(code, supplierID, price, retailprice, units);
            }
            
            m_dataset.storage.AcceptChanges();
            m_dataAdapter.Fill(m_dataset.storage);
        }

        // 删除storage表中的相关记录
        // 删除storage表中的数据并不会删除wine和supplier表中的数据。
        public void deleteCurrentStorage()
        {
            DataRowView dataRowView = m_wineStorageGrid.CurrentRow.DataBoundItem as DataRowView;
            weitongDataSet1.storageRow dataRow = dataRowView.Row as weitongDataSet1.storageRow;
            try
            {
                deleteStorageRow(dataRow.id);
                m_dataset.AcceptChanges();
                m_dataAdapter.Fill(m_dataset.storage);
            }
            catch (Exception e)
            {
            }
        }

        // 根据编码查找库存信息
        // 查找完整的库存信息
        public weitongDataSet1.storageRow findStorageByCode(string code)
        {
            string qStr = @"SELECT storage.id, storage.code, storage.price, storage.retailprice, storage.units, wines.chateau, 
                            wines.country, wines.appellation, wines.quality, wines.vintage, wines.description, wines.bottle, wines.score, 
                            supplier.name
                            FROM storage INNER JOIN
                            wines ON storage.code = wines.code LEFT OUTER JOIN
                            supplier ON storage.supplierid = supplier.id
                            WHERE storage.code = @code";
            MySql.Data.MySqlClient.MySqlCommand queryCmd = new MySql.Data.MySqlClient.MySqlCommand();
            queryCmd.Connection = m_dataAdapter.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;

            // 先保存原查询命令，再执行完本查询后，立即回复原有的查询命令
            MySqlCommand temp = m_dataAdapter.Adapter.SelectCommand;
            m_dataAdapter.Adapter.SelectCommand = queryCmd;
            
            weitongDataSet1.storageDataTable table = new weitongDataSet1.storageDataTable();
            m_dataAdapter.Adapter.Fill(table);
            m_dataAdapter.Adapter.SelectCommand = temp;

            if (table != null && table.Rows.Count != 0)
            {
                // 如果查询结果不为空，应该只有一条记录
                return table[0];                           
            }
            else
            {
                return null;
            }
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
        }

        // 添加酒的信息，code不能为空
        private void insertWine(string code, string chateau = null, string country = null, 
            string appellation = null, string quality = null, string vintage = null, 
            string description = null, string bottle = null, string score = null)
        {
            string insStr = @"INSERT INTO wines(code,chateau,country,appellation,quality,vintage,description,bottle,score) 
                                VALUES(@code,@chateau,@country,@appellation,@quality,@vintage,@description,@bottle,@score)";
            MySql.Data.MySqlClient.MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.Connection = m_dataAdapter.Connection;
            insertCmd.CommandText = insStr;
            insertCmd.Parameters.Add("@code", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = code;
            insertCmd.Parameters.Add("@chateau", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = chateau;
            insertCmd.Parameters.Add("@country", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = country;
            insertCmd.Parameters.Add("@appellation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = appellation;
            insertCmd.Parameters.Add("@quality", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = quality;
            insertCmd.Parameters.Add("@vintage", MySql.Data.MySqlClient.MySqlDbType.Year).Value = vintage;
            insertCmd.Parameters.Add("@description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = description;
            insertCmd.Parameters.Add("@bottle", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = bottle;
            insertCmd.Parameters.Add("@score", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = score;
            //m_dataAdapter.Adapter.InsertCommand = insertCmd;
            insertCmd.Connection.Open();
            IAsyncResult rest = insertCmd.BeginExecuteNonQuery();
            insertCmd.EndExecuteNonQuery(rest);
            insertCmd.Connection.Close();
        }

        // 添加库存信息
        // supplierid若为-1这表示没有设置对应的供应商
        private void insertStorage(string code, int supplierid = -1, decimal price = 0, 
            decimal retailprice = 0, int units = 0)
        {
            string insStr = @"INSERT INTO storage(code, supplierid, price, retailprice, units) 
                                VALUES(@code, @supplierid, @price, @retailprice, @units)";
            MySql.Data.MySqlClient.MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.Connection = m_dataAdapter.Connection;
            insertCmd.CommandText = insStr;
            insertCmd.Parameters.Add("@code", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = code;
            if (supplierid > 0)
            {
                insertCmd.Parameters.Add("@supplierid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = supplierid;
            }
            else
            {
                insertCmd.Parameters.Add("@supplierid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = null;
            }
            insertCmd.Parameters.Add("@price", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = price;
            //insertCmd.Parameters.Add("@caseprice", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = caseprice;
            insertCmd.Parameters.Add("@retailprice", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = retailprice;
            insertCmd.Parameters.Add("@units", MySql.Data.MySqlClient.MySqlDbType.Year).Value = units;

            //m_dataAdapter.Adapter.InsertCommand = insertCmd;
            insertCmd.Connection.Open();
            IAsyncResult rest = insertCmd.BeginExecuteNonQuery();
            insertCmd.EndExecuteNonQuery(rest);
            insertCmd.Connection.Close();
        }

        // 通过storage表的id，删除storage记录
        private void deleteStorageRow(int id)
        {
            string deleteStr = @"DELETE FROM STORAGE WHERE ID=@id";
            MySql.Data.MySqlClient.MySqlCommand deleteCmd = new MySql.Data.MySqlClient.MySqlCommand();
            deleteCmd.Connection = m_dataAdapter.Connection;
            deleteCmd.CommandText = deleteStr;
            deleteCmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = id;
            deleteCmd.Connection.Open();
            IAsyncResult rest = deleteCmd.BeginExecuteNonQuery();
            deleteCmd.EndExecuteNonQuery(rest);
            deleteCmd.Connection.Close();
        }

        // 更新酒的信息，code不能为空
        private void updateWine(string code, string chateau = null, string country = null, string appellation = null, string quality = null, string vintage = null, string description = null, string bottle = null, string score = null)
        {
            string updateStr = @"UPDATE wines SET chateau = @chateau ,country = @country, appellation = @appellation, 
                                quality = @quality, vintage = @vintage, description = @description, bottle = @bottle, score = @score
                                WHERE code = @code";
            MySql.Data.MySqlClient.MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
            updateCmd.Connection = m_dataAdapter.Connection;
            updateCmd.CommandText = updateStr;
            updateCmd.Parameters.Add("@code", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = code;
            updateCmd.Parameters.Add("@chateau", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = chateau;
            updateCmd.Parameters.Add("@country", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = country;
            updateCmd.Parameters.Add("@appellation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = appellation;
            updateCmd.Parameters.Add("@quality", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = quality;
            updateCmd.Parameters.Add("@vintage", MySql.Data.MySqlClient.MySqlDbType.Year).Value = vintage;
            updateCmd.Parameters.Add("@description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = description;
            updateCmd.Parameters.Add("@bottle", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = bottle;
            updateCmd.Parameters.Add("@score", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = score;
            updateCmd.Connection.Open();
            IAsyncResult rest = updateCmd.BeginExecuteNonQuery();
            updateCmd.EndExecuteNonQuery(rest);
            updateCmd.Connection.Close();
        }

        // 更新库存信息
        // supplierid若为-1这表示没有设置对应的供应商
        private void updateStorage(string code, int supplierid = -1, decimal price = 0, decimal retailprice = 0, int units = 0)
        {
            string updateStr = @"UPDATE storage SET supplierid = @supplierid, price = @price, 
                                                 retailprice = @retailprice, units = @units
                                 WHERE code = @code";
            MySql.Data.MySqlClient.MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
            updateCmd.Connection = m_dataAdapter.Connection;
            updateCmd.CommandText = updateStr;
            updateCmd.Parameters.Add("@code", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = code;
            if (supplierid > 0)
            {
                updateCmd.Parameters.Add("@supplierid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = supplierid;
            }
            else
            {
                updateCmd.Parameters.Add("@supplierid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = null;
            }
            updateCmd.Parameters.Add("@price", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = price;
            //updateCmd.Parameters.Add("@caseprice", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = caseprice;
            updateCmd.Parameters.Add("@retailprice", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = retailprice;
            updateCmd.Parameters.Add("@units", MySql.Data.MySqlClient.MySqlDbType.Year).Value = units;

            updateCmd.Connection.Open();
            updateCmd.ExecuteNonQuery();
            updateCmd.Connection.Close();
        }

        // 测试是否有相关酒的库存记录
        public bool existsWineInStorage(string code)
        {
            bool result = false;
            string qStr = @"SELECT EXISTS(
                                SELECT * FROM storage WHERE code = @code)";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = m_dataAdapter.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;
            queryCmd.Connection.Open();
            MySqlDataReader reader = queryCmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                result = reader.GetBoolean(0);
            }
            queryCmd.Connection.Close();

            return result;
        }

        // 测试指定的酒是否已经在wines表中。
        public bool existsWineInWines(string code)
        {
            bool result = false;
            string qStr = @"SELECT EXISTS(
                                SELECT * FROM wines WHERE code = @code)";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = m_dataAdapter.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;
            queryCmd.Connection.Open();
            MySqlDataReader reader = queryCmd.ExecuteReader();
            reader.Read();
            
            if (reader.HasRows)
            {
                result = reader.GetBoolean(0);
            }
            queryCmd.Connection.Close();

            return result;
        }


        private DataGridView m_wineStorageGrid = null;
        private weitongDataSet1 m_dataset = null;
        private weitongDataSet1TableAdapters.storageTableAdapter m_dataAdapter = null;
    }
}
