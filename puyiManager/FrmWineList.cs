using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace weitongManager
{
    public partial class FrmWineList : Form
    {
        public FrmWineList()
        {
            InitializeComponent();
            CenterToScreen();
        }

        public weitongDataSet1 DataSet
        {
            get { return m_dataset; }
            set { 
                m_dataset = value;
                if (m_dataset != null) m_table = m_dataset.storage;
            }
        }

        private void WARNING(string msg)
        {
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private DialogResult SelectionDlgShow(string msg)
        {
            return MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void FrmWineList_Load(object sender, EventArgs e)
        {
            try
            {
                weitongDataSet1TableAdapters.winesTableAdapter adapter = new weitongDataSet1TableAdapters.winesTableAdapter();
                adapter.Fill(m_dataset.wines);
                bindWines();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void bindWines()
        {
            dgv_wineList.AutoGenerateColumns = false;
            dgv_wineList.DataSource = m_table;

            dgv_wineList.Columns["codeDGVStorageInfoTextBoxColumn"].DataPropertyName = "code";
            dgv_wineList.Columns["chateauDGVStorageInfoTextBoxColumn"].DataPropertyName = "chateau";
            dgv_wineList.Columns["vintageDGVStorageInfoTextBoxColumn"].DataPropertyName = "vintage";
            dgv_wineList.Columns["appelltionDGVStorageInfoTextBoxColumn"].DataPropertyName = "appellation";
            dgv_wineList.Columns["qualityDGVStorageInfoTextBoxColumn"].DataPropertyName = "quality";
            dgv_wineList.Columns["bottleDGVStorageInfoTextBoxColumn"].DataPropertyName = "bottle";
            dgv_wineList.Columns["scoreDGVStorageInfoTextBoxColumn"].DataPropertyName = "score";

            dgv_wineList.Columns["unitsDGVStorageInfoTextBoxColumn"].DataPropertyName = "units";
            dgv_wineList.Columns["descriptionDGVStorageInfoTextBoxColumn"].DataPropertyName = "description";
            dgv_wineList.Columns["retailpriceDGVStorageInfoTextBoxColumn"].DataPropertyName = "retailprice";
            dgv_wineList.Columns["countryDGVStorageInfoTextBoxColumn"].DataPropertyName = "country";

            //dgv_wineList.Columns["codeDGVStorageInfoTextBoxColumn"].DataPropertyName = "";
        }


        /// <summary>
        /// 另存新档按钮
        /// </summary>
        private void SaveAs() //另存新档按钮   导出成Excel
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";

            saveFileDialog.FilterIndex = 0;

            saveFileDialog.RestoreDirectory = true;

            saveFileDialog.CreatePrompt = true;

            saveFileDialog.Title = "Export Excel File To";


            saveFileDialog.ShowDialog();


            Stream myStream;

            myStream = saveFileDialog.OpenFile();

            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("gb2312"));

            //StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

            string str = "";

            try
            {
                //写标题

                for (int i = 0; i < this.dgv_wineList.ColumnCount; i++)
                {

                    if (i > 0)
                    {

                        str += "\t";

                    }

                    str += dgv_wineList.Columns[i].HeaderText;

                }

                sw.WriteLine(str);

                //写内容

                for (int j = 0; j < dgv_wineList.Rows.Count; j++)
                {
                    string tempStr = "";

                    for (int k = 0; k < dgv_wineList.Columns.Count; k++)
                    {

                        if (k > 0)
                        {

                            tempStr += "\t";

                        }

                        tempStr += dgv_wineList.Rows[j].Cells[k].Value.ToString();

                    }



                    sw.WriteLine(tempStr);

                }

                sw.Close();

                myStream.Close();

            }

            catch (Exception e)
            {

                MessageBox.Show(e.ToString());

            }

            finally
            {

                sw.Close();

                myStream.Close();

            }

        }


        //第二种方法:引用Excel组件,先定义Excel对象(实例),再根据DataGridView中数据,一格一格去填充Excel对象(实例),单元格,最后保存Excel对象(实例)[调用对象Save()方法].

         /// <summary>
         /// 另存新档按钮   导出成Excel
         /// </summary>
        private void SaveToExcel() //另存新档按钮   导出成Excel
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";

            saveFileDialog.FilterIndex = 0;

            saveFileDialog.RestoreDirectory = true;

            saveFileDialog.CreatePrompt = true;

            saveFileDialog.Title = "Export Excel File To";


            saveFileDialog.ShowDialog();

            string strName = saveFileDialog.FileName;



            System.Reflection.Missing miss = System.Reflection.Missing.Value;


            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbooks books = (Microsoft.Office.Interop.Excel.Workbooks)excel.Workbooks;

            Microsoft.Office.Interop.Excel.Workbook book = (Microsoft.Office.Interop.Excel.Workbook)(books.Add(miss));

            Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)book.ActiveSheet;

            sheet.Name = "test";


            int colIndex = 0;

            foreach (DataGridViewColumn column in this.dgv_wineList.Columns)
            {

                colIndex++;

                excel.Cells[1, colIndex] = column.HeaderText;

            }


            for (int i = 0; i < dgv_wineList.Rows.Count; i++)
            {

                for (int j = 0; j < dgv_wineList.Columns.Count; j++)
                {

                    excel.Cells[i + 2, j + 1] = dgv_wineList.Rows[i].Cells[j].Value.ToString().Trim();

                }

            }



            sheet.SaveAs(strName, miss, miss, miss, miss, miss, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, miss, miss);



            book.Close(false, miss, miss);

            books.Close();

            excel.Quit();


            //System.Runtime.InteropServices.Marshal.ReleaseComObject();   

            System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(books);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

            GC.Collect();

        }

        private weitongDataSet1.storageDataTable m_table = null;
        private weitongDataSet1 m_dataset = null;

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                SaveToExcel();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void tsmi_rmove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_wineList.CurrentRow != null)
                {
                    //DataRowView row = this.dgv_wineList.CurrentRow.DataBoundItem as DataRowView;
                    //weitongDataSet1.winesRow data = row.Row as weitongDataSet1.winesRow;

                    dgv_wineList.Rows.Remove(dgv_wineList.CurrentRow);
                    
                }
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }
    }
}
