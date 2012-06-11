using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace weitongManager
{
    public partial class FrmToolChangeCode : Form
    {
        public FrmToolChangeCode()
        {
            InitializeComponent();
        }

        private void WARNING(string msg)
        {
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private DialogResult SelectionDlgShow(string msg)
        {
            return MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void resetAllCheckBox()
        {
            ckb_his_storage.Checked = false;
            ckb_order_wines.Checked = false;
            ckb_storage.Checked = false;
            ckb_wines.Checked = false;
        }

        private void btn_ChangeCode_Click(object sender, EventArgs e)
        {
            try
            {
                string oldCode = tBox_OldCode.Text.Trim();
                string newCode = tBox_newCode.Text.Trim();
                resetAllCheckBox();
                if (DialogResult.Yes == SelectionDlgShow("您确定要将所有表的原编码 " + oldCode + " 修改为新的编码 " + newCode + "吗？"))
                {
                    if (Wine.existsWine(newCode) || Storage.existsWine(newCode))
                    {
                        WARNING("新编码 " + newCode + " 已经存在，您无法替换！");
                        return;
                    }

                    Wine aWine = Wine.findByCode(oldCode);
                    if (aWine == null)
                    {
                        WARNING("未找到编码为" + oldCode + "的信息！");
                        ckb_wines.Checked = true;
                    }
                    else
                    {
                        change_wines_code(oldCode, newCode);
                        ckb_wines.Checked = true;
                    }

                    change_storage_code(oldCode, newCode);
                    ckb_storage.Checked = true;
                    
                    change_his_storage_code(oldCode, newCode);
                    ckb_his_storage.Checked = true;
                    change_order_wines_code(oldCode, newCode);
                    ckb_order_wines.Checked = true;

                    WARNING("修改已完成！");
                }
                
                
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }




        private int enable_foreign_key_check(MySqlConnection conn)
        {
            return set_foreign_key_check(conn,true);
        }

        private int disable_foreign_key_check(MySqlConnection conn)
        {
            return set_foreign_key_check(conn,false);
        }

        /// <summary>
        /// 设置数据库是否需要检查外键约束。foreign_key_check对于不同的连接，值不一样。
        /// </summary>
        /// <param name="bCheck"></param>
        private int set_foreign_key_check(MySqlConnection conn, bool bCheck = true)
        {
            int icheck = 1;
            if (!bCheck) icheck = 0;

            string setSQL = @"SET foreign_key_checks = @icheck";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = setSQL;
            cmd.Parameters.Add("@icheck", MySqlDbType.Int32).Value = icheck;

            cmd.ExecuteNonQuery();

            string qrySql = @"SELECT @@foreign_key_checks";
            cmd.CommandText = qrySql;
            MySqlDataReader reader = cmd.ExecuteReader();
            icheck = -1;
            if (reader.Read())
            {
                icheck = reader.GetInt32(0);
                reader.Close();
            }
            return icheck;
        }

        /// <summary>
        /// 将指定表中的code替换为新的code
        /// </summary>
        /// <param name="table"></param>
        /// <param name="oldCode"></param>
        /// <param name="newCode"></param>
        private void change_table_code(string table, string oldCode, string newCode)
        {
            string cmdStr = @"UPDATE " + table + " SET code = @newCode WHERE code = @oldCode";

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = ConnSingleton.Connection;
            cmd.CommandText = cmdStr;

            cmd.Parameters.Add("@newCode", MySqlDbType.VarChar).Value = newCode;
            cmd.Parameters.Add("@oldCode", MySqlDbType.VarChar).Value = oldCode;

            try
            {    
                cmd.Connection.Open();
                disable_foreign_key_check(cmd.Connection);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                enable_foreign_key_check(cmd.Connection);
                cmd.Connection.Close();
            }
        }

        private void change_wines_code(string oldCode, string newCode)
        {
            change_table_code("wines", oldCode, newCode);
        }

        private void change_storage_code(string oldCode, string newCode)
        {
            change_table_code("storage", oldCode, newCode);
        }

        private void change_his_storage_code(string oldCode, string newCode)
        {
            change_table_code("his_storage", oldCode, newCode);
        }

        private void change_order_wines_code(string oldCode, string newCode)
        {
            change_table_code("order_wines", oldCode, newCode);
        }

        
    }
}
