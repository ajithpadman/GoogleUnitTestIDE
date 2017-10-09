using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
using ClangSharp;
using CPPASTBuilder.Implementation;
namespace CPPASTBuilder
{
    public class TypeParser
    {
        CppParser m_parser = null;
        public TypeParser(CppParser parser)
        {
            m_parser = parser;
        }
        private IPointerType parsePointerType(ClangSharp.Type type,ClangSharp.Cursor parentCursor,ICppDataType parent)
        {
            IPointerType pointer = null;
            pointer.PointTo = parseDataType(type.Pointee, type.Declaration, pointer);
            pointer.Name = type.Spelling;
            return pointer;
        }
        private IReferenceType parseReferenceType(ClangSharp.Type type, ClangSharp.Cursor parentCursor, ICppDataType parent)
        {
            IReferenceType reference = new ReferenceType(); ;
            reference.ReferenceTo = parseDataType(type.Pointee, type.Declaration, reference);
            reference.Name = type.Spelling;
            return reference;
        }
        private IClass parseClassType(ClangSharp.Type type, ClangSharp.Cursor parentCursor, ICppDataType parent)
        {
            IClass ClassType = null;
            //avoid infinite loops in case of a member of same class type is there in the class
            if (type.Declaration.Equals(parentCursor) == false)
            {
                ClassType =  m_parser.ParseClass(type.Declaration);
            }
            else
            {
                ClassType = parent as IClass ;
            }

            return ClassType;
        }
        private IStructure parseStructureType(ClangSharp.Type type, ClangSharp.Cursor parentCursor, ICppDataType parent)
        {
            IStructure structure = null;
            //avoid infinite loops in case of a member of same class type is there in the class
            if (type.Declaration.Equals(parentCursor) == false)
            {
                structure = m_parser.ParseStructure(type.Declaration);
            }
            else
            {
                structure = parent as IStructure;
            }

            return structure;
        }
        private IEnum parseEnumType(ClangSharp.Type type, ClangSharp.Cursor parentCursor, ICppDataType parent)
        {
            IEnum enumV = new Enumeration();
            enumV.Name = type.Spelling;
            foreach (ClangSharp.Cursor child in type.Declaration.Children)
            {
                EnumElements element = new EnumElements();
                element.Name = child.Spelling;

            }
            return enumV;
        }
   
        private ITypeDef parseTypedef(ClangSharp.Type type, ClangSharp.Cursor parentCursor, ICppDataType parent)
        {
            ITypeDef typedef = new Typedef();
            typedef.Name = type.Spelling;
           
            typedef.TypedefTo = parseDataType(type.Declaration.TypedefDeclUnderlyingType, type.Declaration, typedef);
            if (typedef.TypedefTo == null)
            {
                typedef.TypedefTo = new ArithmeticType().GetUnknownType();
            }
            return typedef;
        }
        private IArithMeticType parseArithMeticType(ClangSharp.Type type, ClangSharp.Cursor parentCursor, ICppDataType parent)
        {
            IArithMeticType arithtype = new ArithmeticType();
            arithtype.Name = type.Spelling;
            
            return arithtype;
        }
        public ICppDataType parseDataType(ClangSharp.Type type, ClangSharp.Cursor parentCursor, ICppDataType parent)
        {
            ICppDataType DataType = null;
            if (type != null)
            {
                switch (type.TypeKind)
                {
                    case ClangSharp.Type.Kind.BlockPointer:
                    case ClangSharp.Type.Kind.MemberPointer:
                    case ClangSharp.Type.Kind.Pointer:
                        DataType = parsePointerType(type, parentCursor, parent);
                        break;
                    case ClangSharp.Type.Kind.Bool:
                    case ClangSharp.Type.Kind.Char16:
                    case ClangSharp.Type.Kind.Char32:
                    case ClangSharp.Type.Kind.CharS:
                    case ClangSharp.Type.Kind.CharU:
                    case ClangSharp.Type.Kind.Double:
                    case ClangSharp.Type.Kind.Float:
                    case ClangSharp.Type.Kind.Int:
                    case ClangSharp.Type.Kind.Int128:
                    case ClangSharp.Type.Kind.Long:
                    case ClangSharp.Type.Kind.LongDouble:
                    case ClangSharp.Type.Kind.LongLong:
                    case ClangSharp.Type.Kind.SChar:
                    case ClangSharp.Type.Kind.Short:
                    case ClangSharp.Type.Kind.UChar:
                    case ClangSharp.Type.Kind.UInt:
                    case ClangSharp.Type.Kind.UInt128:
                    case ClangSharp.Type.Kind.ULong:
                    case ClangSharp.Type.Kind.ULongLong:
                    case ClangSharp.Type.Kind.UShort:
                    case ClangSharp.Type.Kind.Void:
                    case ClangSharp.Type.Kind.WChar:
                        DataType = parseArithMeticType(type, parentCursor, parent);
                        break;
                    case ClangSharp.Type.Kind.Enum:
                        DataType = parseEnumType(type, parentCursor, parent);
                        break;
                    case ClangSharp.Type.Kind.ConstantArray:
                    case ClangSharp.Type.Kind.VariableArray:
                    case ClangSharp.Type.Kind.Vector:
                        break;
                    case ClangSharp.Type.Kind.Record:
                        if (type.Declaration.Kind == CursorKind.StructDecl)
                        {
                            parseStructureType(type, parentCursor, parent);
                        }
                        else if (type.Declaration.Kind == CursorKind.ClassDecl)
                        {
                            DataType = parseClassType(type, parentCursor, parent);
                        }

                        break;
                    case ClangSharp.Type.Kind.Typedef:
                        DataType = parseTypedef(type, parentCursor, parent);
                        break;
                    case ClangSharp.Type.Kind.LValueReference:
                    case ClangSharp.Type.Kind.RValueReference:
                        DataType = parseReferenceType(type, parentCursor, parent);
                        break;
                    default:
                        ArithmeticType arithmeticType = new ArithmeticType();
                        arithmeticType.Name = type.Spelling;
                        DataType = arithmeticType;

                        break;
                        

                }
            }
            return DataType;
        }
    }
}
