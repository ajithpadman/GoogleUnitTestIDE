using GUnit_IDE2010.DataModel;
using System;
using System.Linq;
using GUnit_IDE2010.GunitParser;
using Gunit.DataModel;
using System.IO;

namespace GUnit_IDE2010.CodeGenerator
{
    public class Member
    {
        public string member = "";
        public string memberVariable = "";
        public string value = "";


    }
    public class BoundaryValueGenerator:CodeGenBase
    {
       
        public  BoundaryValueGenerator(CodeGenDataModel model):base(model)
        {

        }
        protected Member DatatypeMinMember(DataType type, string memberName)
        {
            Member member = null;
            switch (type.TypeKind)
            {
                case (int)DataTypeKind.EnumType:
                    member = EnumTypeMinMember(type.EnumType[0], memberName);
                    break;
                case (int)DataTypeKind.ArithmeticType:
                    member = ArithmeticTypeMinMember(type.ArithmeticType[0], memberName);
                    break;
                case (int)DataTypeKind.PointerType:
                    member = PointerTypeMinMember(type.PointerType[0], memberName);
                    break;
                case (int)DataTypeKind.RecordType:
                    member = RecordTypeMinMember(type.RecordType[0], memberName);
                    break;
                case (int)DataTypeKind.ReferenceType:

                    break;
                case (int)DataTypeKind.TypedefType:
                    member = TypedefTypeMinMember(type.Typedef[0], memberName);
                    break;
                default:
                    break;
            }
            return member;
        }
        protected Member DatatypeMaxMember(DataType type, string memberName)
        {
            Member member = null;
            switch (type.TypeKind)
            {
                case (int)DataTypeKind.EnumType:
                    member = EnumTypeMaxMember(type.EnumType[0], memberName);
                    break;
                case (int)DataTypeKind.ArithmeticType:
                    member = ArithmeticTypeMaxMember(type.ArithmeticType[0], memberName);
                    break;
                case (int)DataTypeKind.PointerType:
                    member = PointerTypeMaxMember(type.PointerType[0], memberName);
                    break;
                case (int)DataTypeKind.RecordType:
                    member = RecordTypeMaxMember(type.RecordType[0], memberName);
                    break;
                case (int)DataTypeKind.ReferenceType:

                    break;
                case (int)DataTypeKind.TypedefType:
                    member = TypedefTypeMaxMember(type.Typedef[0], memberName);
                    break;
                default:
                    break;
            }
            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member ArithmeticTypeMaxMember(ArithmeticType type, string memberName)
        {
            Member member = new Member();

            member.member = "static " + type.DataType.EntityName + " " + memberName + " =  std::numeric_limits < " + type.DataType.EntityName + " >::max();";
            member.memberVariable = memberName;
            member.value = "";


            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member ArithmeticTypeMinMember(ArithmeticType type, string memberName)
        {
            Member member = new Member();
            member.member = "static " + type.DataType.EntityName + " " + memberName + " =  std::numeric_limits < " + type.DataType.EntityName + " >::min();";
            member.memberVariable = memberName;
            member.value = "";
            return member;
        }
        /// <summary>
        /// get the maximum Enum Value
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected Member EnumTypeMaxMember(EnumType type, string memberName)
        {
            ListofStrings enumvalues = new ListofStrings();
            foreach (EnumValues values in type.EnumValues)
            {
                enumvalues += values.EnumValue;
            }
            Member member = new Member();
            
            member.memberVariable = memberName;
            if (enumvalues.Count() > 0)
            {
                member.member = "static " + type.DataType.EntityName + " " + memberName + "=" + enumvalues[enumvalues.Count() - 1] + ";";
                member.value = "";
            }
            else
            {
                member.member = "static " + type.DataType.EntityName + " " + memberName + " = (" + type.DataType.EntityName + ")0;";
                member.value = "";
            }
            return member;
        }
        /// <summary>
        /// get the minimum Enum Value
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected Member EnumTypeMinMember(EnumType type, string memberName)
        {
            ListofStrings enumvalues = new ListofStrings();
            foreach (EnumValues values in type.EnumValues)
            {
                enumvalues += values.EnumValue;
            }
            Member member = new Member();
            member.member = "static " + type.DataType.EntityName + " " + memberName + ";";
            member.memberVariable = memberName;
            if (enumvalues.Count() > 0)
            {
                member.member = "static " + type.DataType.EntityName + " " + memberName + "=" + enumvalues[0] + ";";
                member.value = "";
            }
            else
            {
                member.member = "static " + type.DataType.EntityName + " " + memberName + " = (" + type.DataType.EntityName + ")0;";
                member.value = "";
            }
            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected Member PointerTypeMaxMember(PointerType type, string memberName)
        {
            Member member = new Member();
            Member valuemember = null;
            StringWriter writer = new StringWriter();
            member.memberVariable = " " + memberName;
            switch (type.PointerToDataType.TypeKind)
            {
                case (int)DataTypeKind.EnumType:
                    valuemember = EnumTypeMaxMember(type.PointerToDataType.EnumType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.ArithmeticType:
                    valuemember = ArithmeticTypeMaxMember(type.PointerToDataType.ArithmeticType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.PointerType:
                    valuemember = PointerTypeMaxMember(type.PointerToDataType.PointerType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.RecordType:
                    break;
                case (int)DataTypeKind.TypedefType:
                    valuemember = TypedefTypeMaxMember(type.PointerToDataType.Typedef[0], memberName + "_ActualType");
                    break;
                default:
                    break;
            }
            
            if (valuemember != null)
            {
                
                writer.WriteLine(valuemember.member);
                writer.WriteLine(valuemember.value);
                writer.WriteLine("static " + type.DataType.EntityName + " " + memberName + " = &" + valuemember.memberVariable + ";");
                member.member = writer.ToString();
                member.value = "";
            }
            else
            {
                writer.WriteLine("static " + type.DataType.EntityName + " " + memberName + " = NULL;" );
                member.member = writer.ToString();
                member.value = "";
            }

            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member PointerTypeMinMember(PointerType type, string memberName)
        {
            Member member = new Member();
            Member valuemember = null;
            StringWriter writer = new StringWriter();
            member.memberVariable = memberName;
            switch (type.PointerToDataType.TypeKind)
            {
                case (int)DataTypeKind.EnumType:
                    valuemember = EnumTypeMinMember(type.PointerToDataType.EnumType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.ArithmeticType:
                    valuemember = ArithmeticTypeMinMember(type.PointerToDataType.ArithmeticType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.PointerType:
                    valuemember = PointerTypeMinMember(type.PointerToDataType.PointerType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.RecordType:
                    break;
                case (int)DataTypeKind.TypedefType:
                    valuemember = TypedefTypeMinMember(type.PointerToDataType.Typedef[0], memberName + "_ActualType");
                    break;
                default:

                    break;
            }
            if (valuemember != null)
            {

                writer.WriteLine(valuemember.member);
                writer.WriteLine(valuemember.value);
                writer.WriteLine("static " + type.DataType.EntityName + " " + memberName + " = &" + valuemember.memberVariable + ";");
                member.member = writer.ToString();
                member.value = "";
            }
            else
            {
                writer.WriteLine("static " + type.DataType.EntityName + " " + memberName + " = NULL;");
                member.member = writer.ToString();
                member.value = "";
            }

            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member TypedefTypeMaxMember(Typedef type, string memberName)
        {
            Member member = new Member();
            member.memberVariable = memberName;
            Member valuemember = null;
            StringWriter writer = new StringWriter();
            switch (type.UnderlyingTypeDataType.TypeKind)
            {
                case (int)DataTypeKind.EnumType:
                    valuemember = EnumTypeMaxMember(type.UnderlyingTypeDataType.EnumType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.ArithmeticType:
                    valuemember = ArithmeticTypeMaxMember(type.UnderlyingTypeDataType.ArithmeticType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.PointerType:
                    valuemember = PointerTypeMaxMember(type.UnderlyingTypeDataType.PointerType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.RecordType:
                    valuemember = RecordTypeMaxMember(type.UnderlyingTypeDataType.RecordType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.TypedefType:
                    valuemember = TypedefTypeMaxMember(type.UnderlyingTypeDataType.Typedef[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.OtherType:
                    Console.WriteLine("Error");
                    break;
            }
            if (valuemember != null)
            {

                writer.WriteLine(valuemember.member);
                writer.WriteLine(valuemember.value);
                writer.WriteLine("static " + type.DataType.EntityName + " " + memberName + " = " + valuemember.memberVariable + ";");
                member.member = writer.ToString();
                member.value = "";
            }
            else
            {
                writer.WriteLine("static " + type.DataType.EntityName + " " + memberName + ";");
                member.member = writer.ToString();
                member.value = "";
            }
            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member TypedefTypeMinMember(Typedef type, string memberName)
        {
            Member member = new Member();
            member.memberVariable = memberName;
            Member valuemember = null;
            StringWriter writer = new StringWriter();
            switch (type.UnderlyingTypeDataType.TypeKind)
            {
                case (int)DataTypeKind.EnumType:
                    valuemember = EnumTypeMinMember(type.UnderlyingTypeDataType.EnumType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.ArithmeticType:
                    valuemember = ArithmeticTypeMinMember(type.UnderlyingTypeDataType.ArithmeticType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.PointerType:
                    valuemember = PointerTypeMinMember(type.UnderlyingTypeDataType.PointerType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.RecordType:
                    valuemember = RecordTypeMinMember(type.UnderlyingTypeDataType.RecordType[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.TypedefType:
                    valuemember = TypedefTypeMinMember(type.UnderlyingTypeDataType.Typedef[0], memberName + "_ActualType");
                    break;
                case (int)DataTypeKind.OtherType:
                    Console.WriteLine("Error");
                    break;
            }
            if (valuemember != null)
            {

                writer.WriteLine(valuemember.member);
                writer.WriteLine(valuemember.value);
                writer.WriteLine("static " + type.DataType.EntityName + " " + memberName + " = " + valuemember.memberVariable + ";");
                member.member = writer.ToString();
                member.value = "";
            }
            else
            {
                writer.WriteLine("static " + type.DataType.EntityName + " " + memberName + ";");
                member.member = writer.ToString();
                member.value = "";
            }
            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member StructTypeMinMember(Structure type, string memberName)
        {
            Member member = new Member();
            member.member = "static " + type.RecordType.DataType.EntityName + " " + memberName + ";";
            member.memberVariable = memberName;

            StringWriter writer = new StringWriter();
            int i = 0;
            foreach (StructureFields fields in type.StructureFields)
            {
                Member valuemember = null;
                valuemember = DatatypeMinMember(fields.Variables.DataType, "Field_" + i);
                writer.WriteLine(valuemember.member);
                writer.WriteLine(valuemember.value);
                writer.WriteLine(member.memberVariable + "." + fields.Variables.VariableName + " = " + valuemember.memberVariable + ";");
                i++;
            }
            member.value = writer.ToString();
            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member ClassTypeMinMember(Classes type, string memberName)
        {
            Member member = new Member();
            member.member = "static " + type.RecordType.DataType.EntityName + " " + memberName + ";";
            member.memberVariable = memberName;

            StringWriter writer = new StringWriter();
            int i = 0;
            foreach (MemberVariables fields in type.MemberVariables)
            {
                if (fields.AccessScope == (int)ClangSharp.AccessSpecifier.Public)
                {
                    Member valuemember = null;
                    valuemember = DatatypeMinMember(fields.Variables.DataType, "Field_" + i);
                    writer.WriteLine(valuemember.member);
                    writer.WriteLine(valuemember.value);
                    writer.WriteLine(member.memberVariable + "." + fields.Variables.VariableName + " = " + valuemember.memberVariable + ";");
                }
                i++;
            }
            member.value = writer.ToString();
            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member StructTypeMaxMember(Structure type, string memberName)
        {
            Member member = new Member();
            member.member = "static " + type.RecordType.DataType.EntityName + " " + memberName + ";";
            member.memberVariable = memberName;

            StringWriter writer = new StringWriter();
            int i = 0;
            foreach (StructureFields fields in type.StructureFields)
            {
                Member valuemember = null;
                valuemember = DatatypeMaxMember(fields.Variables.DataType, "Field_" + i);
                writer.WriteLine(valuemember.member);
                writer.WriteLine(valuemember.value);
                writer.WriteLine(member.memberVariable + "." + fields.Variables.VariableName + " = " + valuemember.memberVariable + ";");
                i++;
            }
            member.value = writer.ToString();
            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member ClassTypeMaxMember(Classes type, string memberName)
        {
            Member member = new Member();
            member.member = "static " + type.RecordType.DataType.EntityName + " " + memberName + ";";
            member.memberVariable = memberName;

            StringWriter writer = new StringWriter();
            int i = 0;
            foreach (MemberVariables fields in type.MemberVariables)
            {
                if (fields.AccessScope == (int)ClangSharp.AccessSpecifier.Public)
                {
                    Member valuemember = null;
                    valuemember = DatatypeMaxMember(fields.Variables.DataType, "Field_" + i);
                    writer.WriteLine(valuemember.member);
                    writer.WriteLine(valuemember.value);
                    writer.WriteLine(member.memberVariable + "." + fields.Variables.VariableName + " = " + valuemember.memberVariable + ";");
                }
                i++;
            }
            member.value = writer.ToString();
            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member RecordTypeMinMember(RecordType type, string memberName)
        {

            Member member = null;


            if (type.TypeKind == (int)ClangSharp.CursorKind.StructDecl)
            {
                member = StructTypeMinMember(type.Structure[0], memberName);
            }
            else if (type.TypeKind == (int)ClangSharp.CursorKind.ClassDecl)
            {
                member = ClassTypeMinMember(type.Classes[0], memberName);
            }



            return member;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        protected Member RecordTypeMaxMember(RecordType type, string memberName)
        {
            Member member = null;


            if (type.TypeKind == (int)ClangSharp.CursorKind.StructDecl)
            {
                member = StructTypeMaxMember(type.Structure[0], memberName);
            }
            else if (type.TypeKind == (int)ClangSharp.CursorKind.ClassDecl)
            {
                member = ClassTypeMaxMember(type.Classes[0], memberName);
            }
             return member;
        }
    }
}
