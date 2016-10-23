using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace GUnitFramework.Interfaces
{
    public enum ImplementationMode
    {
        FakeClassWithGmockCalls,
        OriginalGmockImplementation
    }
    public interface ICppMockGenerator:ICGunitPlugin
    {
        ImplementationMode ImplementationMode
        {
            get;
            set;
        }
        void GenerateMock(IClass Class);
       



    }
}
