using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.Ui;
using Gunit.DataModel;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;
namespace Gunit.Controller
{
    class ProjectUiController:SideBarControllerBase
    {
     
        public ProjectUiController(GUnitSideBarBase view, DataModelBase model):base(view,model)
        {

        }
        public override void StartView(DockPanel panel, DockState state,bool isFakeview = false)
        {
            base.StartView(panel, state);
            
        }
       
        
    }
}
