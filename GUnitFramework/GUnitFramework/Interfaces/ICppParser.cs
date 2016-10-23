using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
using CPPASTBuilder.Implementation;
namespace GUnitFramework.Interfaces
{
    public interface ICppParser:ICGunitPlugin
    {

        ICppCodeDescription parseFile(string fileName);
       
    }
}
