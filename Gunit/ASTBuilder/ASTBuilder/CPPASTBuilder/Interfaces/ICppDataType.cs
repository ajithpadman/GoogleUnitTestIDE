using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClangSharp;
namespace CPPASTBuilder.Interfaces
{
    public enum CppDataTypeKind
    {
        ArithMeticType,
        PointerType,
        StructureType,
        ClassType,
        EnumType,
        TypedefType,
        ReferenceType,
        ArrayType

    }
    public interface ICppDataType
    {
        
        string Name
        {
            get;
            set;
        }
        CppDataTypeKind Kind
        {
            get;
           
        }

    }
}
