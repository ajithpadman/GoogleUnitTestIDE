using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClangSharp;
namespace ASTBuilder.Interfaces
{
    public interface IDataParser
    {
        ICDataType ParseDataType(Cursor cursor, Cursor parent = null);
        IPointer visitPointerType(Cursor cursor, Cursor parent = null);
        IArithmeticType visitArithMeticType(Cursor cursor);
        IRecord visitRecordType(Cursor cursor);
        ICFunction visitFunctionType(Cursor cursor);
        ICVariable visitVariableType(Cursor cursor, Cursor parent = null);
        IEnum visitEnumType(Cursor cursor);
        ITypedef visitTypedefType(Cursor cursor);
        IMacros visitMacroType(Cursor cursor);
        IArray visitArrayType(Cursor cursor);
    }
}
