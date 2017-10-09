using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{

    public interface IClass : ICppDataType
    {
        List<string> TypeTemplateParam
        {
            get;
            set;
        }
        bool IsTempleteClass
        {
            get;
            set;
        }
        List<IClass> Parents
        {
            get;
            set;
        }
        List<IClass> ChildClasses
        {
            get;
            set;
        }
        List<IMemberMethod> MemeberMethods
        {
            get;
            set;
        }
        List<IMemberVariable> MemberVariables
        {
            get;
            set;
        }

    }
}
