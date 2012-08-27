using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace weitongManager
{
    public partial class FrmGenerateCode : Form
    {

        private string m_code = "";

        public FrmGenerateCode()
        {
            InitializeComponent();
            CenterToScreen();
        }

        public string Code
        {
            get { return m_code + Ean13.CalculateChecksum(m_code); }
            set {
                m_code = value;

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

        private void btn_Generate_Click(object sender, EventArgs e)
        {
            try
            {
                string code = util.GetRandomString(12, false, true, false,false);
                while (Storage.existsWine(code) || Wine.existsWine(code))
                {
                    code = util.GetRandomString(12);
                }
                lbl_Code.Text = code;
                
                Code = code;
                showCode();
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                WARNING(ex.Message);
            }
        }

        private void showCode()
        {
            tBox_code1.Text = m_code.Substring(0, 4);
            tBox_code2.Text = m_code.Substring(4, 4);
            tBox_code3.Text = m_code.Substring(8, 4);
            int sum = Ean13.CalculateChecksum(m_code);
            tbox_paires.Text = sum.ToString();
        }
    }
}
