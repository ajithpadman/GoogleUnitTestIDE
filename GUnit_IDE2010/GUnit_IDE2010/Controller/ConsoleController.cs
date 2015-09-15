using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.Ui;
using Gunit.DataModel;

namespace Gunit.Controller
{
    public class ConsoleController:SideBarControllerBase
    {
        public ConsoleController(GUnitSideBarBase view, DataModelBase model)
            : base(view, model)
        {

        }
    }
}
