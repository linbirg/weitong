﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    class Storage
    {
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
                        throw new ZeroStorageException("库存不足");
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
    }
}
