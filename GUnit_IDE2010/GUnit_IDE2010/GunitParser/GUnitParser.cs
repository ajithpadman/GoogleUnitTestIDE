using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.DataModel;
using ClangSharp;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace GUnit_IDE2010.GunitParser
{
    public enum DataTypeKind
    {
        OtherType,
        ArithmeticType,
        RecordType,
        PointerType,
        TypedefType,
        ReferenceType,
        EnumType


    }
    public class GUnitParser:IDisposable
    {
      
       
        Index m_Index = null;

        string m_ConnectionString = "";
        public GUnitParser(string connString)
        {
            m_ConnectionString = connString;
            m_Index = new Index(true, true);
           
        }
       
       
       
       
        
        public TreeNode CreateAST(TranslationUnit unit,string fileName, TreeNode Parent,ProjectFiles parentFile)
        {
           
            ClangSharp.Cursor root = null;
            if (unit != null )
            {
                
                ProjectFiles file = new ProjectFiles();
                file.FilePath = fileName;
                file.LastModifiedTime = System.IO.File.GetLastWriteTime(fileName);
                root = unit.Cursor;
                setImageIndex(Parent, FunctionTreeNodeType.FileNode);
                ParseCursor(root, Parent, fileName, parentFile);
              
               
              


            }
            return Parent;
        }
        /// <summary>
        /// Create the Translation Unit for a file
        /// </summary>
        /// <param name="l_index">Index on to which the TU need to be created</param>
        /// <param name="fileName">Absolute path to the file</param>
        /// <param name="cmdLines">Command line arguments</param>
        /// <returns></returns>
        private TranslationUnit CreateTU(Index l_index, string fileName, ListofStrings cmdLines)
        {
            TranslationUnit unit = null;
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                unit = l_index.CreateTranslationUnit(fileName, cmdLines.ToArray(), null, TranslationUnitFlags.DetailedPreprocessingRecord);
                watch.Stop();
               

            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            return unit;
        }
        private MemberMethods parseFilecxxmethod(ClangSharp.Cursor cursor,TreeNode currentParent,ProjectFiles parentFile)
        {
            ProjectFiles file = null;


            GUnitDB dbCtx = new GUnitDB(m_ConnectionString);
            try
            {
                IEnumerable<ProjectFiles> fileDb = (IEnumerable<ProjectFiles>)from p in dbCtx.ProjectFiles
                                                                              where p.FilePath == cursor.SemanticParent.Location.File.Name
                                                                              select p;


                if (fileDb.Count() > 0)
                {
                    file = fileDb.ElementAt(0);
                }
            }
            catch
            {
                
            }
            if (file == null)
            {
                file = new ProjectFiles();
                file.FilePath = cursor.SemanticParent.Location.File.Name;
                file.LastModifiedTime = System.IO.File.GetLastWriteTime(file.FilePath);
                dbCtx.ProjectFiles.InsertOnSubmit(file);
                
            }
            else
            {

            }

            Classes parentClass = visitClassDecl(
                                                  cursor.SemanticParent,
                                                  new TreeNode(),
                                                  file.FilePath,
                                                  file
                                                  );
            dbCtx.SubmitChanges();
            return VisitMemberMethod(cursor, currentParent, parentClass, parentFile);
                    
        }
        private Classes visitClassDecl(ClangSharp.Cursor cursor, TreeNode currentParent, string currentFile, ProjectFiles parentFile)
        {
            Classes classes = new Classes();
            classes.ColumnNo = cursor.Location.Column;
            classes.EntityName = cursor.Spelling;
            classes.FilePath = cursor.Location.File.Name;
            classes.Line = cursor.Location.Line;
            classes.ProjectFiles = parentFile;
            TreeNode node = new TreeNode(classes.EntityName);
            node.Tag = classes;
            node.ToolTipText = classes.EntityName;
            setImageIndex(node, FunctionTreeNodeType.ClassNode);
            foreach (ClangSharp.Cursor child in cursor.Children)
            {
                if (isLinkedList(cursor.Type, child.Type) == false)
                {
                    ClassVisitor(child, node, classes, parentFile);
                }
            }
            if (null != currentParent)
            {
                currentParent.Nodes.Add(node);
            }

            
            return classes;
           
        }
        private MemberMethods VisitMemberMethod(ClangSharp.Cursor cursor, TreeNode Parent, Classes parentClass, ProjectFiles parentFile)
        {
         
            MemberMethods memberMethod = new MemberMethods();
            memberMethod.IsPureVirtual = cursor.IsPureVirtualCxxMethod;
            memberMethod.IsVirtual = cursor.IsVirtualCxxMethod;
            memberMethod.IsStaticCxxMethod = cursor.IsStaticCxxMethod;
            Methods m = FunctionVisitor(cursor, Parent, parentFile, memberMethod);
            memberMethod.Methods = m;
            memberMethod.Classes = parentClass;
          
            return memberMethod;
        }
        private GlobalMethods GlobalMethodsVisitor(ClangSharp.Cursor cursor,TreeNode currentParent,ProjectFiles parentFile)
        {
            
            GlobalMethods gm = new GlobalMethods();
            Methods m = FunctionVisitor(cursor, currentParent, parentFile, gm);
            gm.Methods = m;
            gm.ProjectFiles = parentFile;
           
            return gm;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="currentParent"></param>
        /// <param name="currentFile"></param>
        public void ParseCursor(ClangSharp.Cursor cursor,TreeNode currentParent,string currentFile,ProjectFiles parentFile)
        {
            
            switch (cursor.Kind)
            {
                case CursorKind.CxxMethod:

                    parseFilecxxmethod(cursor, currentParent, parentFile);
                    break;
                case CursorKind.FunctionDecl:
                   
                    GlobalMethodsVisitor(cursor, currentParent, parentFile);
                   
                    
                 
                    break;
                case CursorKind.ClassDecl:
                    visitClassDecl(cursor, currentParent, currentFile, parentFile);
                    
                    break;
                case CursorKind.VarDecl:
                    GlobalVariables gv = new GlobalVariables();
                    Variables v = VariableVisitor(cursor, currentParent, parentFile);
                    gv.Variables = v;
                    gv.ProjectFiles = parentFile;
                   
                    
                    break;
                case CursorKind.Namespace:
                    Namespaces name = new Namespaces();
                    name.ColumnNo = cursor.Location.Column;
                    name.EntityName = cursor.Spelling;
                    name.FilePath = cursor.Location.File.Name;
                    name.Line = cursor.Location.Line;
                    TreeNode Namenode = new TreeNode(name.EntityName);
                    setImageIndex(Namenode, FunctionTreeNodeType.NamespaceNode);
                    foreach (ClangSharp.Cursor child in cursor.Children)
                    {
                        NamespaceVisitor(child, Namenode, name, parentFile);
                    }
                    currentParent.Nodes.Add(Namenode);
                    
                    break;
                case CursorKind.TypedefDecl:
                    VisitTypedefDeclairation(cursor, currentParent,parentFile);
                    break;
                default:
                    foreach (ClangSharp.Cursor child in cursor.Children)
                    {
                        if (child.Location.File.Name.Replace("/","\\") == currentFile)
                        {

                            ParseCursor(child, currentParent, currentFile, parentFile);
                        }
                    }
                    break;
            }
            

        }
        private void VisitTypedefDeclairation(ClangSharp.Cursor cursor, TreeNode currentParent, ProjectFiles parentFile)
        {
            string TypedefName = cursor.Spelling;
            ClangSharp.Type.Kind UnderlyingKind = cursor.TypedefDeclUnderlyingType.Canonical.TypeKind;
            string underlyingType = cursor.TypedefDeclUnderlyingType.Canonical.Spelling;

        }
        private void setImageIndex(TreeNode node, FunctionTreeNodeType nodeType)
        {
            switch (nodeType)
            {
                case FunctionTreeNodeType.FileNode:
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                    break;
                case FunctionTreeNodeType.GlobalVariableNode:
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    break;
                case FunctionTreeNodeType.GlobalFunctionNode:
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                    break;
                case FunctionTreeNodeType.CalledMethodNode:
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                    break;
                case FunctionTreeNodeType.NamespaceNode:
                    node.ImageIndex = 4;
                    node.SelectedImageIndex = 4;
                    break;
                case FunctionTreeNodeType.ClassNode:
                    node.ImageIndex = 5;
                    node.SelectedImageIndex = 5;
                    break;
                case FunctionTreeNodeType.memberVariableNode:
                    node.ImageIndex = 6;
                    node.SelectedImageIndex = 6;
                    break;
                case FunctionTreeNodeType.memberMethodNode:
                    node.ImageIndex = 7;
                    node.SelectedImageIndex = 7;
                    break;
                case FunctionTreeNodeType.TypedefNode:
                    node.ImageIndex =8;
                    node.SelectedImageIndex = 8;
                    break;
              
                default:
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                    break;
            }
        }
        /// <summary>
        /// IS linked List
        /// </summary>
        /// <param name="parentType"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        private bool isLinkedList(ClangSharp.Type parentType, ClangSharp.Type fieldType)
        {
           
            if(fieldType.TypeKind == parentType.TypeKind)
            {
                return true;
            }
            else if(fieldType.TypeKind == ClangSharp.Type.Kind.Pointer)
            {
               return  isLinkedList(parentType, fieldType.Pointee);
            }
            else
            {
                return false;
            }
            
        }
        Typedef VisitTypedef(ClangSharp.Type l_type, DataType type)
        {
            Typedef typ = new Typedef();
            typ.DataType = type;
            typ.UnderlyingTypeDataType = VisitDataType(l_type.Declaration, l_type.Declaration.TypedefDeclUnderlyingType.Canonical);
            return typ;
        }
        ReferenceType VisitReferenceType(ClangSharp.Type l_type, DataType type)
        {
            ReferenceType refType = new ReferenceType();
            refType.DataType = type;
            refType.ReferenceToDataType = VisitDataType(l_type.Pointee.Declaration, l_type.Pointee);
            return refType;
        }
        EnumType VisitEnumType(ClangSharp.Type l_type,DataType type)
        {
            EnumType enumType = new EnumType();
            foreach(ClangSharp.Cursor child in l_type.Declaration.Children)
            {
                EnumValues value = new EnumValues();
                value.EnumType = enumType;
                value.EnumValue = child.Spelling;
            }
            enumType.DataType = type;
            return enumType;
        }
        Structure VisitStructureType(ClangSharp.Type l_type, RecordType type)
        {
            Structure str = new Structure();

            foreach (ClangSharp.Cursor child in l_type.Declaration.Children)
            {
                if (child.Kind == CursorKind.FieldDecl)
                {
                   // if (isLinkedList(l_type, child.Type) == false)
                    {
                        StructureFields field = new StructureFields();
                        ProjectFiles file = new ProjectFiles();
                        file.FilePath = child.Location.File.Name;

                        field.Variables = VariableVisitor(child, null, file);
                        field.Structure = str;
                    }
                }
            }
            str.RecordType = type;
            return str;
        }
        RecordType VisitRecordType(ClangSharp.Type l_type,DataType type)
        {
            RecordType record = new RecordType();
            if (l_type.Declaration.Kind == CursorKind.StructDecl)
            {
                VisitStructureType(l_type, record);
            }
            else if (l_type.Declaration.Kind == CursorKind.ClassDecl)
            {
                ProjectFiles file = new ProjectFiles();
                file.FilePath = l_type.Declaration.Location.File.Name;
                //visitClassDecl(l_type.Declaration, null, file.FilePath, file);
            }
            record.DataType = type;
            record.TypeKind = (int)l_type.TypeKind;
            return record;
        }
        ArithmeticType VisitArithMeticType(ClangSharp.Type l_type, DataType type)
        {
            ArithmeticType arithmeticType = new ArithmeticType();
            arithmeticType.DataType = type;
            return arithmeticType;

        }
        PointerType VisitPointerType(ClangSharp.Type l_type, DataType type)
        {
            PointerType ptrType = new PointerType();
            ptrType.DataType = type;
            
            ptrType.PointerToDataType = VisitDataType(l_type.Pointee.Declaration, l_type.Pointee);
            if(ptrType.PointerToDataType.IsConstQualified)
            { ptrType.DataType.IsConstQualified = true; }
            return ptrType;
        }
        DataType VisitDataType(ClangSharp.Cursor cursor, ClangSharp.Type l_type)
        {
            DataType type = new DataType();
            type.EntityName = l_type.Spelling;
            type.IsConstQualified = l_type.IsConstQualifiedType;
           
              

            switch (l_type.TypeKind)
            {
                case ClangSharp.Type.Kind.BlockPointer:
                case ClangSharp.Type.Kind.Pointer:
                case ClangSharp.Type.Kind.MemberPointer:
                    VisitPointerType(l_type, type);
                    type.TypeKind = (int)DataTypeKind.PointerType;
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
                
                    VisitArithMeticType(l_type, type);
                    type.TypeKind = (int)DataTypeKind.ArithmeticType;
                    break;
                case ClangSharp.Type.Kind.Record:
                    VisitRecordType(l_type, type);
                    type.TypeKind = (int)DataTypeKind.RecordType;
                    break;
                case ClangSharp.Type.Kind.Enum:
                    VisitEnumType(l_type, type);
                    type.TypeKind = (int)DataTypeKind.EnumType;
                    break;
                case ClangSharp.Type.Kind.LValueReference:
                case ClangSharp.Type.Kind.RValueReference:
                    VisitReferenceType(l_type, type);
                    type.TypeKind = (int)DataTypeKind.ReferenceType;
                    break;
                case ClangSharp.Type.Kind.Typedef:
                    VisitTypedef(l_type, type);
                    type.TypeKind = (int)DataTypeKind.TypedefType;
                    break;
                case ClangSharp.Type.Kind.Unexposed:
                    if(l_type.Declaration.Kind == CursorKind.StructDecl)
                    {
                        VisitRecordType(l_type, type);
                        type.TypeKind = (int)DataTypeKind.RecordType;
                    }
                    else if (l_type.Declaration.Kind == CursorKind.ClassDecl)
                    {
                        VisitRecordType(l_type, type);
                        type.TypeKind = (int)DataTypeKind.RecordType;
                    }
                    else if(l_type.Declaration.Kind == CursorKind.EnumDecl)
                    {
                        VisitEnumType(l_type.Canonical, type);
                        type.TypeKind = (int)DataTypeKind.EnumType;
                    }
                        break;
                default:
                    break;


            }
            return type;
        }
       Arguments VisitArguments(ClangSharp.Cursor cursor)
        {
            Arguments args = new Arguments();
            args.DataType = VisitDataType(cursor, cursor.Type);
            return args;
        }

        /// <summary>
        /// Visit the Function declairation or Definition
        /// </summary>
        /// <param name="cursor">FunctionDecl cursor</param>
        /// <param name="parent">Parent for FunctionDecl Cursor</param>
        /// <returns>Cursor.ChildVisitResult.Continue</returns>
        private Methods FunctionVisitor(ClangSharp.Cursor cursor, TreeNode parentNode, ProjectFiles parentFile,Object tag = null)
        {
          
            Methods method = new Methods();
            method.AccessScope = (int)cursor.AccessSpecifier;
            method.ColumnNo = cursor.Location.Column;
            method.EntityName = cursor.Spelling;
            method.IsConstMethod = cursor.IsConstCxxMethod;
            method.IsDefined = cursor.IsDefinition;
            method.Line = cursor.Location.Line;
            method.FilePath = cursor.Location.File.Name;
         
            int argCount = cursor.NumArguments;
            ListofStrings param = new ListofStrings();
            ListofStrings underlyingparam = new ListofStrings();
            for (uint i = 0; i < argCount; i++)
            {
                ClangSharp.Cursor cur = cursor.GetArgument(i);

                method.Arguments.Add(VisitArguments(cur));
                underlyingparam += cur.Type.Canonical.Spelling;
                param += (cur.Type.Spelling);
            }
            method.Parameters = String.Join(",", param.ToArray());
            method.UnderlyingParamKind = String.Join(",", underlyingparam.ToArray());
            method.ReturnType = cursor.ResultType.Spelling;
            method.UnderlyingReturnKind = cursor.ResultType.Canonical.Spelling;
            method.StorageClass = (int)cursor.StorageClassSpecifier;
            method.DataType = VisitDataType(cursor, cursor.ResultType);

            IEnumerable<ClangSharp.Cursor> CalledList = from child in cursor.Descendants
                                                 where (child.Kind == CursorKind.CallExpr)
                                                        select child;
            TreeNode FunctionNode = new TreeNode(method.ReturnType + " " + method.EntityName + "(" + method.Parameters+")");
            FunctionNode.Tag = tag;
            if (cursor.Kind == CursorKind.FunctionDecl)
            {
                setImageIndex(FunctionNode, FunctionTreeNodeType.GlobalFunctionNode);
                
            }
            else if (cursor.Kind == CursorKind.CxxMethod)
            {
                FunctionNode.Text = cursor.SemanticParent.Spelling + "::" + FunctionNode.Text;
               
                setImageIndex(FunctionNode, FunctionTreeNodeType.memberMethodNode);
                
            }
            FunctionNode.ToolTipText = FunctionNode.Text;
            foreach (ClangSharp.Cursor child in CalledList)
            {

                switch (child.Kind)
                {
                    case CursorKind.VariableRef:
                        Console.Write("");
                        break;
                    case CursorKind.CallExpr:
                        MethodCalls called = new MethodCalls();
                        called.ColumnNo = child.Location.Column;
                        called.EntitiyName = child.Spelling;
                        called.FilePath = child.Location.File.Name;
                        called.Line = child.Location.Line;
                        int args = child.Referenced.NumArguments;
                        ListofStrings arguments = new ListofStrings();
                        for (uint i = 0; i < args; i++)
                        {
                            ClangSharp.Cursor cur = child.Referenced.GetArgument(i);
                            arguments += (cur.Type.Spelling);
                        }
                        called.ParentFilePath = child.Referenced.Location.File.Name;
                        called.IsDefinedInParent = child.Referenced.IsDefinition;
                        called.IsCxxMethod = (child.Referenced.Kind == CursorKind.CxxMethod);
                        object calledTag = called;
                        string toolTip = "";
                        if (called.IsCxxMethod)
                        {
                            MemberMethods m = parseFilecxxmethod(child.Referenced, null, parentFile);
                            calledTag = m;
                            toolTip = m.Methods.EntityName;
                        }
                        else
                        {
                            GlobalMethods gm = GlobalMethodsVisitor(child.Referenced, null, parentFile);
                            calledTag = gm;
                            toolTip = gm.Methods.EntityName;
                        }
                        called.ParentName = child.Referenced.Spelling;
                        called.Parameters = String.Join(",", arguments.ToArray());
                        called.ReturnType = child.Referenced.ResultType.Spelling;
                        called.DataType = VisitDataType(child, child.Referenced.ResultType);
                        called.UnderlyingReturnKind = child.Referenced.ResultType.Canonical.Spelling;
                        called.Methods = method; 
                        TreeNode CalledFunctionNode = new TreeNode(called.ReturnType+" "+called.EntitiyName+"("+called.Parameters+")");
                        setImageIndex(CalledFunctionNode, FunctionTreeNodeType.CalledMethodNode);
                        ///insert object in to database
                      
                        FunctionNode.Nodes.Add(CalledFunctionNode);
                        CalledFunctionNode.Tag = calledTag;
                        CalledFunctionNode.ToolTipText = toolTip;
                        if (child.Referenced.Definition.IsNull == true)
                        {
                            CalledFunctionNode.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            CalledFunctionNode.ForeColor = System.Drawing.Color.Green;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (parentNode != null)
            {
                parentNode.Nodes.Add(FunctionNode);
            }
         
            

            return method;
        }


        private DataTypeKind getTypeKind( ClangSharp.Type l_type)
        {
            DataTypeKind typeKind = DataTypeKind.OtherType;
            switch (l_type.TypeKind)
            {
                case ClangSharp.Type.Kind.BlockPointer:
                case ClangSharp.Type.Kind.Pointer:
                case ClangSharp.Type.Kind.MemberPointer:

                    typeKind = DataTypeKind.PointerType;
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


                    typeKind = DataTypeKind.ArithmeticType;
                    break;
                case ClangSharp.Type.Kind.Record:

                    typeKind = DataTypeKind.RecordType;
                    break;
                case ClangSharp.Type.Kind.Enum:

                    typeKind = DataTypeKind.EnumType;
                    break;
                case ClangSharp.Type.Kind.LValueReference:
                case ClangSharp.Type.Kind.RValueReference:

                    typeKind = DataTypeKind.ReferenceType;
                    break;
                case ClangSharp.Type.Kind.Typedef:

                    typeKind = DataTypeKind.TypedefType;
                    break;
                case ClangSharp.Type.Kind.Unexposed:
                    if (l_type.Declaration.Kind == CursorKind.StructDecl)
                    {

                        typeKind = DataTypeKind.RecordType;
                    }
                    else if (l_type.Declaration.Kind == CursorKind.ClassDecl)
                    {

                        typeKind = DataTypeKind.RecordType;
                    }
                    else if (l_type.Declaration.Kind == CursorKind.EnumDecl)
                    {

                        typeKind = DataTypeKind.EnumType;
                    }
                    break;
                default:
                    break;

            }
            return typeKind;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        private Variables VariableVisitor(ClangSharp.Cursor cursor, TreeNode parent, ProjectFiles parentFile)
        {
        
            Variables variable = new Variables();
            
            variable.ColumnNo = cursor.Location.Column;
            variable.FilePath = cursor.Location.File.Name;
            variable.Line = cursor.Location.Line;
            variable.StorageClass = (int)cursor.StorageClassSpecifier;
            variable.VariableName = cursor.Spelling;
            variable.VariableType = cursor.Type.Spelling;
            variable.TypeKind = (int)getTypeKind(cursor.Type);

            TreeNode node = new TreeNode(variable.VariableName + ":" + variable.VariableType);
            if (cursor.Kind == CursorKind.FieldDecl)
            {
                setImageIndex(node, FunctionTreeNodeType.memberVariableNode);
            }
            else if (cursor.Kind == CursorKind.VarDecl)
            {
                setImageIndex(node, FunctionTreeNodeType.GlobalVariableNode);
            }
            if (parent != null)
            {
                parent.Nodes.Add(node);
            }
            return variable;

        }
        /// <summary>
        /// Visit the classes in the file
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private void ClassVisitor(ClangSharp.Cursor cursor,TreeNode Parent,Classes parentClass,ProjectFiles parentFile)
        {
          

            switch (cursor.Kind)
            {

                case CursorKind.CxxMethod:
                    VisitMemberMethod(cursor, Parent, parentClass, parentFile);
                    
                    
                    break;
                case CursorKind.FieldDecl:
                    
                    Variables v = VariableVisitor(cursor, Parent, parentFile);
                    MemberVariables mv = new MemberVariables();
                    mv.AccessScope = (int)cursor.AccessSpecifier;
                    mv.Classes = parentClass;
                    mv.Variables = v;
                   
                    
                    break;
                case CursorKind.ClassDecl:
                    visitClassDecl(cursor, Parent, parentFile.FilePath, parentFile);
                   
                    break;
                default:
                    foreach (ClangSharp.Cursor child in cursor.Children)
                    {
                        if(cursor.Type.Spelling != child.Type.Spelling)
                        ClassVisitor(child, Parent, parentClass, parentFile);
                    }
                    break;

            }

        }
        private void NamespaceVisitor(ClangSharp.Cursor cursor, TreeNode Parent, Namespaces parentClass, ProjectFiles parentFile)
        {
           
            switch (cursor.Kind)
            {
                case CursorKind.VarDecl:
                    GlobalVariables gv = new GlobalVariables();
                    Variables v = VariableVisitor(cursor, Parent, parentFile);
                    gv.Variables = v;
                    gv.ProjectFiles = parentFile;
                  
                    
                    break;
                case CursorKind.FunctionDecl:
                    GlobalMethodsVisitor(cursor, Parent, parentFile);
                    break;
                case CursorKind.ClassDecl:
                    visitClassDecl(cursor, Parent, parentFile.FilePath, parentFile);
                    
                    
                    
                    break;
                case CursorKind.Namespace:
                    Namespaces name = new Namespaces();
                    name.ColumnNo = cursor.Location.Column;
                    name.EntityName = cursor.Spelling;
                    name.FilePath = cursor.Location.File.Name;
                    name.Line = cursor.Location.Line;
                    name.ProjectFiles = parentFile;
                    TreeNode node1 = new TreeNode(name.EntityName);
                    setImageIndex(node1, FunctionTreeNodeType.NamespaceNode);
                   // m_DataBase.Classes.InsertOnSubmit(classes);
                    foreach (ClangSharp.Cursor child in cursor.Children)
                    {
                        NamespaceVisitor(child, node1, name, parentFile);
                    }
                    Parent.Nodes.Add(node1);
                    
                    break;
                default:
                    foreach (ClangSharp.Cursor child in cursor.Children)
                    {

                        NamespaceVisitor(child, Parent, parentClass, parentFile);
                    }
                    break;
            }
        }

        public void Dispose()
        {
            if(m_Index != null)
            {
                m_Index.Dispose();
                m_Index = null;
            }
        }
    }

}
