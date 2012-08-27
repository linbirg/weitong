using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace weitongManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmLogin frmLogin = new FrmLogin();
            if (DialogResult.OK == frmLogin.ShowDialog())
            {
                User aUser = frmLogin.getLoginUser();
                FrmMain frmMain = new FrmMain();
                frmMain.CurrentUser = aUser;
                Application.Run(frmMain);
            }
        }
    }
}
