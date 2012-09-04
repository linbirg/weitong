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
    public partial class FrmVersionView : Form
    {
        public FrmVersionView()
        {
            InitializeComponent();
        }

        private void FrmVersionView_Load(object sender, EventArgs e)
        {
            lbl_Version.Text = Config.Version;
        }
    }
}
