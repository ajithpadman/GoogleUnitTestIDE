using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace GUnitFramework.Interfaces
{
    public enum ParserStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
    public interface IParser:ICGunitPlugin
    {
        ICCodeDescription CodeDescription
        {
            get;
            set;
        }
        ParserStatus Status
        {
            get;
            set;
        }
       
        void ParseFile(string fileNames);
    }
}
