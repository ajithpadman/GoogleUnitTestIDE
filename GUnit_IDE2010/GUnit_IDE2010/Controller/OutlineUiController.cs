using Gunit.DataModel;
using Gunit.Ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace Gunit.Controller
{
    public class OutlineUiController:SideBarControllerBase
    {
        public OutlineUiController(GUnitSideBarBase view, DataModelBase model):base(view,model)
        {

        }
        public override void StartView(DockPanel panel, DockState state, bool isFakeView = false)
        {
             base.StartView(panel, state, isFakeView);

        }

       

       
       
    }
}
