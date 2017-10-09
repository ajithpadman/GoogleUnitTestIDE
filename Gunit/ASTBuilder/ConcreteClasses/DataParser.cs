using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
using ClangSharp;
namespace ASTBuilder.ConcreteClasses
{
    public class DataParser:IDataParser
    {
        public ICDataType ParseDataType(ClangSharp.Cursor cursor,ClangSharp.Cursor parent = null)
        {
            
            try
            {
                ICDataType data = null;
                if (parent != null && cursor != null)
                {
                    if (cursor.Type.Equals(parent.Type))
                    {
                        return visitArithMeticType(cursor);
                    }
                }
                
                
                    switch (cursor.Type.TypeKind)
                    {
                        case ClangSharp.Type.Kind.BlockPointer:
                        case ClangSharp.Type.Kind.Pointer:
                        case ClangSharp.Type.Kind.MemberPointer:
                            data = visitPointerType(cursor,parent);
                            break;
                        case ClangSharp.Type.Kind.Bool:
                        case ClangSharp.Type.Kind.CharU:
                        case ClangSharp.Type.Kind.UChar:
                        case ClangSharp.Type.Kind.Char16:
                        case ClangSharp.Type.Kind.Char32:
                        case ClangSharp.Type.Kind.UShort:
                        case ClangSharp.Type.Kind.UInt:
                        case ClangSharp.Type.Kind.ULong:
                        case ClangSharp.Type.Kind.ULongLong:
                        case ClangSharp.Type.Kind.UInt128:
                        case ClangSharp.Type.Kind.CharS:
                        case ClangSharp.Type.Kind.SChar:
                        case ClangSharp.Type.Kind.WChar:
                        case ClangSharp.Type.Kind.Short:
                        case ClangSharp.Type.Kind.Int:
                        case ClangSharp.Type.Kind.Long:
                        case ClangSharp.Type.Kind.LongLong:
                        case ClangSharp.Type.Kind.Int128:
                        case ClangSharp.Type.Kind.Float:
                        case ClangSharp.Type.Kind.Double:
                        case ClangSharp.Type.Kind.LongDouble:
                            data = visitArithMeticType(cursor);
                            break;
                        case ClangSharp.Type.Kind.Record:
                            data = visitRecordType(cursor);
                            break;
                        case ClangSharp.Type.Kind.IncompleteArray:
                        case ClangSharp.Type.Kind.ConstantArray:
                            data = visitArrayType(cursor);
                            break;

                        case ClangSharp.Type.Kind.Enum:
                            data = visitEnumType(cursor);
                            break;
                        case ClangSharp.Type.Kind.Typedef:
                            data = visitTypedefType(cursor);
                            break;
                        case ClangSharp.Type.Kind.Unexposed:
                            if (cursor.Type.Declaration.Kind == CursorKind.StructDecl)
                            {
                                data = visitRecordType(cursor);
                            }
                            else if (cursor.Type.Declaration.Kind == CursorKind.EnumDecl)
                            {
                                data = visitEnumType(cursor);
                            }
                            break;
                        default:
                            data = visitArithMeticType(cursor);
                            break;

                    }
                
                if (cursor.Type.IsConstQualifiedType)
                {
                    if (null != data)
                    {
                        data.isConstQualified = true;
                    }
                }
                return data;
            }
            catch(Exception err)
            {
                Console.WriteLine(err.ToString());
                return null;
            }
        }

        public IPointer visitPointerType(ClangSharp.Cursor cursor, ClangSharp.Cursor parent = null)
        {
            IPointer pointer = new Pointer();
            pointer.Name = cursor.Type.Spelling;
            ICDataType arithmetic = returnArithMetic(cursor.Type.Pointee);
            if (null == arithmetic)
            {
                if(cursor.Type.Pointee.Declaration.Kind != CursorKind.NoDeclFound)
                {
                    pointer.PointTo = ParseDataType(cursor.Type.Pointee.Declaration, parent);
                }
                else 
                {
                    if (cursor.Type.Pointee.TypeKind == ClangSharp.Type.Kind.Pointer)
                    {
                        pointer.PointTo = visitPointerType(cursor.Type.Pointee);
                    }
                    else
                    {

                    }
                }

                
            }
            else
            {
                pointer.PointTo = arithmetic;
            }
       
            return pointer;
        }
        private IPointer visitPointerType(ClangSharp.Type type)
        {
            IPointer pointer = new Pointer();
            pointer.Name = type.Spelling;
            ICDataType arithmetic = returnArithMetic(type.Pointee);
            if (null == arithmetic)
            {
                if (type.Pointee.Declaration.Kind != CursorKind.NoDeclFound)
                {
                    pointer.PointTo = ParseDataType(type.Pointee.Declaration);
                }
                else
                {
                    if (type.Pointee.TypeKind == ClangSharp.Type.Kind.Pointer)
                    {
                        pointer.PointTo = visitPointerType(type.Pointee);
                    }
                   
                }
            }
            else
            {
                pointer.PointTo = arithmetic;
            }
           
            return pointer;
        }
        public IArithmeticType visitArithMeticType(ClangSharp.Cursor cursor)
        {
            IArithmeticType arithmetic = new ArithmeticType();
            arithmetic.Name = cursor.Type.Spelling;
           
            return arithmetic;

        }

        public IRecord visitRecordType(ClangSharp.Cursor cursor)
        {
            IRecord record = new Record();

            if (cursor.Type.Declaration.Kind == CursorKind.StructDecl)
            {
                record.Name = cursor.Spelling;
                foreach (Cursor child in cursor.Type.Declaration.Children)
                {
                    if (child.Kind == CursorKind.FieldDecl)
                    {
                        record.Elements.Add(visitVariableType(child, cursor.Type.Declaration));
                    }
                }
            }
            return record;
        }
        public IRecord visitRecordType(ClangSharp.Type type)
        {
            IRecord record = new Record();

            if (type.Declaration.Kind == CursorKind.StructDecl)
            {
                record.Name = type.Declaration.Spelling;
                foreach (Cursor child in type.Declaration.Children)
                {
                    if (child.Kind == CursorKind.FieldDecl)
                    {
                        record.Elements.Add(visitVariableType(child, type.Declaration));
                    }
                }
            }
            return record;
        }
        
        private ICDataType getReturnType(ClangSharp.Type type)
        {
            ICDataType data = null;
            data = returnArithMetic(type);
            if (data != null)
            {
                return data;
            }
            else
            {
                switch (type.TypeKind)
                {
                    case ClangSharp.Type.Kind.BlockPointer:
                    case ClangSharp.Type.Kind.Pointer:
                    case ClangSharp.Type.Kind.MemberPointer:
                        data = visitPointerType(type);
                        break;
                    case ClangSharp.Type.Kind.Record:
                        data = visitRecordType(type);
                        break;
                    case ClangSharp.Type.Kind.Enum:
                        data = visitEnumType(type);
                        break;
                    case ClangSharp.Type.Kind.Typedef:
                        data = visitTypedefType(type);
                        break;
                    case ClangSharp.Type.Kind.Unexposed:
                        if (type.Declaration.Kind == CursorKind.StructDecl)
                        {
                            data = visitRecordType(type);
                        }
                        else if (type.Declaration.Kind == CursorKind.EnumDecl)
                        {
                            data = visitEnumType(type);
                        }
                        break;
                    default:
                        data = new ArithmeticType();
                        break;

                }
                return data;
               
            }
        }
        public ICFunction visitFunctionType(ClangSharp.Cursor cursor)
        {
            ICFunction function = new CFunction();
            function.Name = cursor.Spelling;
            function.ReturnValue = getReturnType(cursor.ResultType);
            int argCount = cursor.NumArguments;
            for (uint i = 0; i < argCount; i++)
            {
               ClangSharp.Cursor cur = cursor.GetArgument(i);
               function.Parameters.Add( visitVariableType(cur));
            }
            foreach (Cursor child in cursor.Descendants)
            {
                if (child.Kind == CursorKind.VarDecl)
                {
                    ICVariable variable = visitVariableType(child);
                    variable.AccessSpecifier = CAccessSpecifier.Local;
                    function.LocalVariables.Add(variable);
                }
                else if (CursorKind.CallExpr == child.Kind)
                {
                    if (cursor.Equals( child.Referenced) == false)
                    {
                        if (child.Definition.Kind != CursorKind.Constructor)
                        {
                            function.CalledFunctions.Add(visitFunctionType(child.Referenced));
                        }
                    }
                }
            }
           

            return function;

        }

        public ICVariable visitVariableType(ClangSharp.Cursor cursor, ClangSharp.Cursor parent = null)
        {
            ICVariable variable = new CVariable();
            variable.Name = cursor.Spelling;
            if (cursor.StorageClassSpecifier == StorageClass.SC_Invalid)
            {
                variable.StorageClass = CStorageClass.Invalid;
            }
            else if (cursor.StorageClassSpecifier == StorageClass.SC_Extern)
            {
                variable.StorageClass = CStorageClass.Extern;
            }
            else if (cursor.StorageClassSpecifier == StorageClass.SC_Auto)
            {
                variable.StorageClass = CStorageClass.Auto;
            }
            else if (cursor.StorageClassSpecifier == StorageClass.SC_None)
            {
                variable.StorageClass = CStorageClass.None;
            }
            else if (cursor.StorageClassSpecifier == StorageClass.SC_Register)
            {
                variable.StorageClass = CStorageClass.Register;
            }
            else if (cursor.StorageClassSpecifier == StorageClass.SC_Static)
            {
                variable.StorageClass = CStorageClass.Static;
            }
            variable.Type = ParseDataType(cursor, parent);
            return variable;// throw new NotImplementedException();
        }

        public IEnum visitEnumType(ClangSharp.Cursor cursor)
        {
            IEnum enumV = new Enum();
            enumV.Name = cursor.Spelling;
            foreach (ClangSharp.Cursor child in cursor.Type.Declaration.Children)
            {
                EnumElement element = new EnumElement();
                element.Name = child.Spelling;
                enumV.Elements.Add(element);
                
            }
            return enumV;

            
        }
        public IEnum visitEnumType(ClangSharp.Type type)
        {
            IEnum enumV = new Enum();
            foreach (ClangSharp.Cursor child in type.Declaration.Children)
            {
                EnumElement element = new EnumElement();
                element.Name = child.Spelling;

            }
            return enumV;
        }
        private IArithmeticType returnArithMetic(ClangSharp.Type type)
        {
            IArithmeticType arithmetic = null;
            switch(type.TypeKind)
            {
                case ClangSharp.Type.Kind.Bool:
                case ClangSharp.Type.Kind.CharU:
                case ClangSharp.Type.Kind.UChar:
                case ClangSharp.Type.Kind.Char16:
                case ClangSharp.Type.Kind.Char32:
                case ClangSharp.Type.Kind.UShort:
                case ClangSharp.Type.Kind.UInt:
                case ClangSharp.Type.Kind.ULong:
                case ClangSharp.Type.Kind.ULongLong:
                case ClangSharp.Type.Kind.UInt128:
                case ClangSharp.Type.Kind.CharS:
                case ClangSharp.Type.Kind.SChar:
                case ClangSharp.Type.Kind.WChar:
                case ClangSharp.Type.Kind.Short:
                case ClangSharp.Type.Kind.Int:
                case ClangSharp.Type.Kind.Long:
                case ClangSharp.Type.Kind.LongLong:
                case ClangSharp.Type.Kind.Int128:
                case ClangSharp.Type.Kind.Float:
                case ClangSharp.Type.Kind.Double:
                case ClangSharp.Type.Kind.LongDouble:
                case ClangSharp.Type.Kind.Void:
                    arithmetic = new ArithmeticType();
                    arithmetic.Name = type.Spelling;
                    break;
            }
            return arithmetic;
        }
        public IArray visitArrayType(Cursor cursor)
        {
            IArray array = new Array();
            array.Name = cursor.Type.Spelling;
            ICDataType arithmetic = returnArithMetic(cursor.Type.ArrayElementType);
            if (arithmetic != null)
            {
                array.ArrayElementType = arithmetic;
            }
            else
            {
                array.ArrayElementType = ParseDataType(cursor.Type.ArrayElementType.Declaration);
            }
            return array;
        }
        public ITypedef visitTypedefType(ClangSharp.Cursor cursor)
        {
            ITypedef typedef = new Typedef();
            typedef.Name = cursor.Type.Spelling;
            ICDataType arithmetic = returnArithMetic(cursor.Type.Declaration.TypedefDeclUnderlyingType);
            if (arithmetic == null)
            {
                typedef.TypedefOf = ParseDataType(cursor.Type.Declaration.TypedefDeclUnderlyingType.Declaration);
            }
            else
            {
                arithmetic.Name = cursor.Type.Declaration.TypedefDeclUnderlyingType.Spelling;
                typedef.TypedefOf = arithmetic;
            }
            return typedef;
        }
        public ITypedef visitTypedefType(ClangSharp.Type type)
        {
            ITypedef typedef = new Typedef();
            typedef.Name = type.Spelling;
            ICDataType arithmetic = returnArithMetic(type.Declaration.TypedefDeclUnderlyingType);
            if (arithmetic == null)
            {
                typedef.TypedefOf = ParseDataType(type.Declaration.TypedefDeclUnderlyingType.Declaration);
            }
            else
            {
                arithmetic.Name = type.Declaration.TypedefDeclUnderlyingType.Spelling;
                typedef.TypedefOf = arithmetic;
            }
            return typedef;
        }

        public IMacros visitMacroType(ClangSharp.Cursor cursor)
        {
            IMacros macro = new Macro();
            return macro;
        }


        
    }
}
