using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.DataModel;
namespace GUnit_IDE2010.DataModel
{
    public class HTMlBrowserModel:DataModelBase
    {
        private string m_Url = "";
        
        public HTMlBrowserModel():base()
        {

        }
        public override void Datamodel_openProject()
        {
            base.Datamodel_openProject();
            m_Url = "";
        }
        public override void Datamodel_newProject()
        {
            base.Datamodel_newProject();
            m_Url = "";
        }
        public override void Datamodel_closeProject()
        {
            base.Datamodel_closeProject();
            m_Url = "";
        }
        public string URL
        {
            get { return m_Url; }
            set
            {
                m_Url = value;
                FirePropertyChange("URL_CHANGED");
            }
        }
    }
}
