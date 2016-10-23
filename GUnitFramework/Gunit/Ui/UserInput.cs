using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUnitFramework.Interfaces;
namespace Gunit.Ui
{
    public partial class UserInput : Form
    {
        public string m_Data;
        public UserInput()
        {
            InitializeComponent();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            m_Data = txtLibName.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
