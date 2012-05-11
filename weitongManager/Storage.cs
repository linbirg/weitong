using System;
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
    }
}
