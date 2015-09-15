using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gunit.Ui;
using Gunit.DataModel;
namespace GUnit_IDE2010.Ui
{
    public partial class Document_Search : Form
    {
        DocumentDataModel m_Model;
        public Document_Search()
        {
            InitializeComponent();
        }
        public Document_Search(DocumentDataModel documentModel)
        {
            InitializeComponent();
            m_Model = documentModel;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text) == false)
            {
                m_Model.SearchText = txtSearch.Text;
            }
        }

        private void DocumentSearch_Close(object sender, FormClosedEventArgs e)
        {
            m_Model.SearchStart = 0;
        }
    }
}
