using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
namespace GUnitFramework.Interfaces
{
    public enum PluginType
    {
        Parser,
        Builder,
        CoverageAnalyser,
        BoundaryTestGenerator,
        TestRunner,
        TestReportGenerator,
        MockGenerator,
        SpecialPlugin
        
    }
    public interface ICGunitPlugin
    {
        PluginType PluginType
        {
            get;
            set;
        }
        string PluginName
        {
            get;
            set;
        }
        ICGunitHost Owner
        {
            get;
            set;
        }
        bool registerCallBack(ICGunitHost host);
        bool HandleProjectSession(ProjectStatus status);
        void Show(WeifenLuo.WinFormsUI.Docking.DockPanel dock, WeifenLuo.WinFormsUI.Docking.DockState state);
    }
}
