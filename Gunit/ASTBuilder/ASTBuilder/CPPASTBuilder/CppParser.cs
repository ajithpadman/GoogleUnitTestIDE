using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
using ClangSharp;
using CPPASTBuilder.Implementation;
namespace CPPASTBuilder
{
    public class CppParser
    {
        TypeParser m_typeParser;
        public CppParser()
        {
            m_typeParser = new TypeParser(this);
        }
        public IStructure ParseStructure(Cursor cursor)
        {
            IStructure structureObject = null;
            IMemberVariable variable = null;
            if (null != cursor)
            {
                structureObject = new Structure();

                foreach (Cursor child in cursor.Children)
                {
                    switch (child.Kind)
                    {
                        case CursorKind.FieldDecl:
                            variable = visitmemberVariable(child, structureObject);
                            if (variable != null)
                            {
                                structureObject.Fields.Add(variable);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return structureObject;
        }
        public IClass ParseClass(Cursor cursor)
        {
            IClass ClassObject = null;
            IClass ParentClass = null;
            IClass childClass = null;
            IMemberMethod method = null;
            IMemberVariable variable = null;
            
            if (null != cursor)
            {
                ClassObject = new Class();
                ClassObject.Name = cursor.Spelling;
                if (cursor.Kind == CursorKind.ClassTemplate)
                {
                    ClassObject.IsTempleteClass = true;
                    
                }
                foreach (Cursor child in cursor.Children)
                {
                    switch (child.Kind)
                    {
                        case CursorKind.CxxMethod:
                            method = visitmemberMethod(child, ClassObject);
                            if (method != null)
                            {
                                ClassObject.MemeberMethods.Add(method);
                            }
                            break;
                        case CursorKind.TemplateTypeParameter:
                            ClassObject.TypeTemplateParam.Add(child.Spelling);
                            break;
                        case CursorKind.CxxBaseSpecifier:
                            ParentClass = ParseClass(child.Definition);
                            if (null != ParentClass)
                            {
                                ClassObject.Parents.Add(ParentClass);
                            }
                            break;
                        case CursorKind.ClassDecl:
                            childClass = ParseClass(child);
                            if (null != ParentClass)
                            {
                                ClassObject.ChildClasses.Add(childClass);
                            }
                            break;
                        case CursorKind.FieldDecl:
                            variable = visitmemberVariable(child, ClassObject);
                            if (variable != null)
                            {
                                ClassObject.MemberVariables.Add(variable);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return ClassObject;
        }
        public INamespace ParseNameSpace(Cursor cursor)
        {
            INamespace NameSpaceObject = null;
            INamespace childNameSpace = null;
            IClass childClass = null;
            if (null != cursor)
            {
                NameSpaceObject = new Namespace();
                NameSpaceObject.Name = cursor.Spelling;
                foreach (Cursor child in cursor.Children)
                {
                    switch (child.Kind)
                    {
                        case CursorKind.Namespace:
                            childNameSpace = ParseNameSpace(child);
                            if (null != childNameSpace)
                            {
                                NameSpaceObject.Namespaces.Add(childNameSpace);
                            }
                            break;
                        case CursorKind.ClassDecl:
                        case CursorKind.ClassTemplate:
                            childClass = ParseClass(child);
                            if (null != childClass)
                            {
                                NameSpaceObject.Classes.Add(childClass);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return NameSpaceObject;
        }
        private IMemberMethod visitmemberMethod(Cursor cursor,IClass parentClass)
        {
            IMemberMethod memberMethod = null;
            if (cursor != null)
            {
                memberMethod = new MemberMethod();
                memberMethod.AccessScope = cursor.AccessSpecifier;
                memberMethod.IsConstQualified = cursor.IsConstCxxMethod;
                memberMethod.IsRestrictQualified = cursor.Type.IsRestrictQualifiedType;
                memberMethod.IsVolatileQualified = cursor.Type.IsVolatileQualifiedType;
                memberMethod.StorageClass = cursor.StorageClassSpecifier;
                memberMethod.MethodName = cursor.Spelling;
                memberMethod.IsVirtual = cursor.IsVirtualCxxMethod;
                memberMethod.IsPureVirtual = cursor.IsPureVirtualCxxMethod;
                memberMethod.ReturnValue = m_typeParser.parseDataType(cursor.ResultType, cursor.SemanticParent, parentClass);
                if (null == memberMethod.ReturnValue)
                {
                    memberMethod.ReturnValue = new ArithmeticType().GetUnknownType();
                }

                for (uint i = 0; i < cursor.NumArguments; i++)
                {
                    Cursor argumentCursor = cursor.GetArgument(i);
                    if (argumentCursor != null)
                    {
                        ICppDataType type = m_typeParser.parseDataType(argumentCursor.Type, cursor.SemanticParent, parentClass);
                        if (type != null)
                        {
                            memberMethod.Parameters.Add(type);
                        }
                        else
                        {
                            
                            memberMethod.Parameters.Add(new ArithmeticType().GetUnknownType());
                        }

                    }
                    else
                    {
                        
                        memberMethod.Parameters.Add(new ArithmeticType().GetUnknownType());
                    }

                }

            }
            return memberMethod;
        }
        private IMemberVariable visitmemberVariable(Cursor cursor,IClass parentClass)
        {
            IMemberVariable memberVariable = null;
            if (cursor != null)
            {
                memberVariable = new MemberVariable();
                memberVariable.DataType = m_typeParser.parseDataType(cursor.Type, cursor.SemanticParent, parentClass);
                memberVariable.AccessScope = cursor.AccessSpecifier;
                memberVariable.IsConstQualified = cursor.Type.IsConstQualifiedType;
                memberVariable.IsRestrictQualified = cursor.Type.IsRestrictQualifiedType;
                memberVariable.IsVolatileQualified = cursor.Type.IsVolatileQualifiedType;
                memberVariable.StorageClass = cursor.StorageClassSpecifier;
                memberVariable.VariableName = cursor.Spelling;
            }
            return memberVariable;
        }
        private IMemberVariable visitmemberVariable(Cursor cursor,IStructure parentStructure)
        {
            IMemberVariable memberVariable = null;
            if (cursor != null)
            {
                memberVariable = new MemberVariable();
                memberVariable.DataType = m_typeParser.parseDataType(cursor.Type, cursor.SemanticParent, parentStructure);
                memberVariable.AccessScope = cursor.AccessSpecifier;
                memberVariable.IsConstQualified = cursor.Type.IsConstQualifiedType;
                memberVariable.IsRestrictQualified = cursor.Type.IsRestrictQualifiedType;
                memberVariable.IsVolatileQualified = cursor.Type.IsVolatileQualifiedType;
                memberVariable.StorageClass = cursor.StorageClassSpecifier;
                memberVariable.VariableName = cursor.Spelling;
            }
            return memberVariable;
        }
    }
}
