using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.Ui;
using Gunit.DataModel;
using System.Collections;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;
namespace Gunit.Controller
{
    /// <summary>
    /// A central Controller for all Documents
    /// </summary>
    public class DocumentManagerController:SideBarControllerBase
    {
       
        
       
        /// <summary>
        /// Constructor for DocumentManager
        /// </summary>
        public DocumentManagerController(GUnitSideBarBase view,DataModelBase model):base(view,model)
        {
           
        }
        public override void StartView(DockPanel panel, DockState state, bool isFakeView = false)
        {
            base.StartView(panel, state, isFakeView);
            
        }
        
       
        


    }
}
