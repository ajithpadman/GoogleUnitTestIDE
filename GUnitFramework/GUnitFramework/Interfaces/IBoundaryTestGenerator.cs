using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace GUnitFramework.Interfaces
{
    public interface IBoundaryTestGenerator:ICGunitPlugin
    {
        void generateBoundaryTest(string fileName, ICCodeDescription desc);
    }
}
