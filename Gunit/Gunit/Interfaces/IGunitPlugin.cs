using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Gunit.Model;
namespace Gunit.Interfaces
{
    public interface IGunitPlugin
    {
        string PluginName { get;  }
        string Description { get;  }
        void Init(IProjectModel model);
        void DeInit();
        void Save();
        UserControl getView();
       
        
        
        
    }
}
