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
using GUnit_IDE2010.DataModel;
namespace GUnit_IDE2010.Ui
{
    public partial class HtmlBrowser : GUnitSideBarBase
    {
        public HtmlBrowser()
        {
            InitializeComponent();
        }
        public HtmlBrowser(DataModelBase model):base(model)
        {
            InitializeComponent();
        }

        private void HtmlBrowser_Load(object sender, EventArgs e)
        {
            this.CloseButtonVisible = true;
            browserControl.Navigate(((HTMlBrowserModel)m_model).URL);
            m_model.PropertyChanged += HtmlBrowser_PropertyChanged;
            
        }
        private void HtmlBrowser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "URL_CHANGED":
                    browserControl.Navigate(((HTMlBrowserModel)m_model).URL);
                    break;
                default:
                    break;
            }
        }
        public override void closeProject()
        {
            
            base.closeProject();
            this.Close();

        }

    }
}
