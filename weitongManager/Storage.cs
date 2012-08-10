using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    /// <summary>
    /// 库存类，需要维护两个表，一个是storage,一个是his_storage。
    /// </summary>
    class Storage
    {

        #region public static method
        /// <summary>
        /// 根据酒的编码查找库存信息，结果或者为空，或者为一条库存记录。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static weitongDataSet1.storageRow findByCode(string code)
        {
            weitongDataSet1.storageRow row = null;
            string qStr = @"SELECT storage.id, storage.code, storage.price, storage.retailprice, storage.units, wines.chateau, 
                            wines.country, wines.appellation, wines.quality, wines.vintage, wines.description, wines.bottle, wines.score, 
                            supplier.name
                            FROM storage INNER JOIN
                            wines ON storage.code = wines.code LEFT OUTER JOIN
                            supplier ON storage.supplierid = supplier.id
                            WHERE storage.code = @code";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = ConnSingleton.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;

            try
            {
                queryCmd.Connection.Open();
                MySqlDataReader reader = queryCmd.ExecuteReader();
                weitongDataSet1.storageDataTable table = new weitongDataSet1.storageDataTable();
                table.Load(reader);
                if (table.Count > 0) row = table[0];
            }
            finally
            {
                queryCmd.Connection.Close();
            }

            return row;
        }

        /// <summary>
        /// 根据编号查找历史信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns>包含历史信息的列表</returns>
        public static List<HisStorage> findHistoryByCode(string code)
        {
            List<HisStorage> list = null;
            string qStr = @"SELECT id,code,supplierid,price,retailprice,units,effectdate FROM his_storage WHERE code=@code";
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = qStr;
            cmd.Connection = ConnSingleton.Connection;

            cmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;
            try
            {
                cmd.Connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (list == null) list = new List<HisStorage>();
                    HisStorage his = new HisStorage();
                    his.ID = reader.GetInt32("id");
                    his.Code = reader.GetString("code");
                    Wine aWine = Wine.findByCode(his.Code);
                    if (aWine != null) his.Description = aWine.Description;
                    int supplier_id = reader.GetInt32("supplierid");
                    Supplier splr = Supplier.findByID(supplier_id);
                    if (splr != null) his.Supplier = splr.Name;
                    his.Price = reader.GetDecimal("price");
                    his.RetailPrice = reader.GetDecimal("retailprice");
                    his.Units = reader.GetInt32("units");
                    his.EffectDate = reader.GetDateTime("effectdate");
                    list.Add(his);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return list;
        }

        /// <summary>
        /// 根据描述查找库存信息，查找方式为模糊查询
        /// </summary>
        /// <param name="description"></param>
        /// <returns>返回包含库存信息的表</returns>
        public static weitongDataSet1.storageDataTable findByDescription(string description)
        {
            weitongDataSet1.storageDataTable table = new weitongDataSet1.storageDataTable();
            string qStr = @"SELECT storage.id, storage.code, storage.price, storage.retailprice, storage.units, wines.chateau, 
                            wines.country, wines.appellation, wines.quality, wines.vintage, wines.description, wines.bottle, wines.score, 
                            supplier.name
                            FROM wines INNER JOIN
                            storage ON wines.code = storage.code LEFT OUTER JOIN
                            supplier ON storage.supplierid = supplier.id
                            WHERE wines.description like @description";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = ConnSingleton.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@description", MySqlDbType.VarChar).Value = "%"+description+"%";

            try
            {
                queryCmd.Connection.Open();
                MySqlDataReader reader = queryCmd.ExecuteReader();
                //table = ;
                table.Load(reader);
                
            }
            finally
            {
                queryCmd.Connection.Close();
            }

            return table;
        }

        /// <summary>
        /// 根据酒庄查找库存信息，查找方式为模糊查询
        /// </summary>
        /// <param name="description"></param>
        /// <returns>返回包含库存信息的表</returns>
        public static weitongDataSet1.storageDataTable findByChateau(string chateau)
        {
            weitongDataSet1.storageDataTable table = new weitongDataSet1.storageDataTable();
            string qStr = @"SELECT storage.id, storage.code, storage.price, storage.retailprice, storage.units, wines.chateau, 
                            wines.country, wines.appellation, wines.quality, wines.vintage, wines.description, wines.bottle, wines.score, 
                            supplier.name
                            FROM wines INNER JOIN
                            storage ON wines.code = storage.code LEFT OUTER JOIN
                            supplier ON storage.supplierid = supplier.id
                            WHERE wines.chateau like @chateau";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = ConnSingleton.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@chateau", MySqlDbType.VarChar).Value = "%" + chateau + "%";

            try
            {
                queryCmd.Connection.Open();
                MySqlDataReader reader = queryCmd.ExecuteReader();
                //table = ;
                table.Load(reader);

            }
            finally
            {
                queryCmd.Connection.Close();
            }

            return table;
        }

        /// <summary>
        /// 根据年份查找库存信息，查找方式为模糊查询
        /// </summary>
        /// <param name="description"></param>
        /// <returns>返回包含库存信息的表</returns>
        public static weitongDataSet1.storageDataTable findByVintage(string vintage)
        {
            weitongDataSet1.storageDataTable table = new weitongDataSet1.storageDataTable();
            string qStr = @"SELECT storage.id, storage.code, storage.price, storage.retailprice, storage.units, wines.chateau, 
                            wines.country, wines.appellation, wines.quality, wines.vintage, wines.description, wines.bottle, wines.score, 
                            supplier.name
                            FROM wines INNER JOIN
                            storage ON wines.code = storage.code LEFT OUTER JOIN
                            supplier ON storage.supplierid = supplier.id
                            WHERE wines.vintage = @vintage";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = ConnSingleton.Connection;
            queryCmd.CommandText = qStr;
            queryCmd.Parameters.Add("@vintage", MySqlDbType.Int32).Value = Int32.Parse(vintage);

            try
            {
                queryCmd.Connection.Open();
                MySqlDataReader reader = queryCmd.ExecuteReader();
                //table = ;
                table.Load(reader);

            }
            finally
            {
                queryCmd.Connection.Close();
            }

            return table;
        }

        // 
        /// <summary>
        /// 测试是否有相关酒的库存记录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool existsWine(string code)
        {
            bool result = false;
            string qStr = @"SELECT EXISTS(
                                SELECT * FROM storage WHERE code = @code)";
            MySqlCommand queryCmd = new MySqlCommand();
            queryCmd.Connection = ConnSingleton.Connection;
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

        // 增加酒的库存（如果为负数则为减少）。
        /// <summary>
        /// 修改指定酒的库存数量。如果修改后库存数量为负，则抛出ZeroStorageException异常。
        /// 如果指定的酒不存在，则抛出InvalidWineCodeException.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="plus"></param>
        public static void plusUnits(string code, int plus = 1)
        {
            string updateStr = @"UPDATE storage SET units = units + @plus WHERE code = @code";
            string qUnitsStr = @"SELECT units FROM storage WHERE code = @code";
            MySqlCommand updateCmd = new MySqlCommand();
            updateCmd.CommandText = updateStr;
            updateCmd.Connection = ConnSingleton.Connection;
            updateCmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;
            updateCmd.Parameters.Add("@plus", MySqlDbType.Int32).Value = plus;

            try
            {
                updateCmd.Connection.Open();
                updateCmd.CommandText = qUnitsStr;
                MySqlDataReader reader = updateCmd.ExecuteReader();
                if (reader.Read())
                {
                    int units = reader.GetInt32("units");
                    reader.Close();
                    if (units + plus >= 0)
                    {
                        updateCmd.CommandText = updateStr;
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new ZeroStorageException("你真的确定你要修改库存为负么，请不要开玩笑。");
                    }
                }
                else
                {
                    throw new InvalidWineCodeException();
                }
            }
            //catch (Exception ex)
            //{
            //}
            finally
            {
                updateCmd.Connection.Close();
            }
        }

        public static void necUnits(string code, int nec = 1)
        {
            plusUnits(code, -nec);
        }

        // 添加库存信息
        // supplierid若为-1这表示没有设置对应的供应商
        /// <summary>
        /// 添加库存信息,同时添加历史库存信息中的记录。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="supplierid"></param>
        /// <param name="price"></param>
        /// <param name="retailprice"></param>
        /// <param name="units"></param>
        public static void insert(string code, int supplierid = -1, decimal price = 0,
            decimal retailprice = 0, int units = 0)
        {
            insert_his_storage(code, supplierid, price, retailprice, units);
            insert_table("storage", code, supplierid, price, retailprice, units);
        }

        /// <summary>
        /// 向历史库存加入记录，入库数量不能为负值。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="supplierid"></param>
        /// <param name="price"></param>
        /// <param name="retailprice"></param>
        /// <param name="units">入库数量，入库数量不能为负值</param>
        public static void insert_his_storage(string code, int supplierid = -1, decimal price = 0,
            decimal retailprice = 0, int units = 0)
        {
            insert_table("his_storage", code, supplierid, price, retailprice, units);
        }

        // 更新库存信息
        // supplierid若为-1这表示没有设置对应的供应商
        /// <summary>
        /// 更新库存记录，但不更新库存数量。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="supplierid"></param>
        /// <param name="price"></param>
        /// <param name="retailprice"></param>
        /// <param name="units"></param>
        public static void update(string code, int supplierid = -1, decimal price = 0, decimal retailprice = 0)
        {
            string updateStr = @"UPDATE storage SET supplierid = @supplierid, price = @price, 
                                                 retailprice = @retailprice
                                 WHERE code = @code";
            MySql.Data.MySqlClient.MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
            updateCmd.Connection = ConnSingleton.Connection;
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
            //updateCmd.Parameters.Add("@units", MySql.Data.MySqlClient.MySqlDbType.Year).Value = units;

            try
            {
                updateCmd.Connection.Open();
                updateCmd.ExecuteNonQuery();
            }
            finally
            {
                updateCmd.Connection.Close();
            }
        }

        #endregion


        #region private static method

        // 添加库存信息
        // supplierid若为-1这表示没有设置对应的供应商
        /// <summary>
        /// 添加历史库存信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="supplierid"></param>
        /// <param name="price"></param>
        /// <param name="retailprice"></param>
        /// <param name="units"></param>
        private static void insert_table(string table, string code, int supplierid = -1, decimal price = 0,
            decimal retailprice = 0, int units = 0)
        {
            if (units < 0) throw new ZeroStorageException("添加库存不能为负值");
            string columns = @"(code, supplierid, price, retailprice, units";
            string values = @"(@code, @supplierid, @price, @retailprice, @units";
            
            if (table == "his_storage")
            {
                columns += ",effectdate)";
                values += ",@effectdate)";
            }
            else
            {
                columns += ")";
                values += ")";
            }
            string insStr = @"INSERT INTO " + table + columns +  
                                "VALUES" + values;

            MySql.Data.MySqlClient.MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
            insertCmd.Connection = ConnSingleton.Connection;
            insertCmd.CommandText = insStr;
            //insertCmd.Parameters.Add("@table", MySqlDbType.VarChar).Value = table;
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

            if (table == "his_storage")
            {
                insertCmd.Parameters.Add("@effectdate", MySqlDbType.DateTime).Value = DateTime.Now;
            }
            //m_dataAdapter.Adapter.InsertCommand = insertCmd;
            try
            {
                insertCmd.Connection.Open();
                insertCmd.ExecuteNonQuery();
            }
            finally
            {
                insertCmd.Connection.Close();
            }

        }
        #endregion
    }
    
}
