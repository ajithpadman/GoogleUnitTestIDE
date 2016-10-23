using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace GUnitFramework.Interfaces
{
    public interface IMockGenrator:ICGunitPlugin
    {
        string Path
        {
            get;
            set;
        }
        void generateMock(ICCodeDescription description);
    }
}
