using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
namespace GUnit
{
    public partial class TestReport : DockContent
    {
        string m_url;
        public TestReport(string url)
        {
            InitializeComponent();
            m_url = url;
        }

        private void TestReport_Load(object sender, EventArgs e)
        {
             browser.Navigate(m_url);
        }
    }
}
