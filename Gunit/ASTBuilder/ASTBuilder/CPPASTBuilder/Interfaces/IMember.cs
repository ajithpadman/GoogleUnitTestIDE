using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClangSharp;
namespace CPPASTBuilder.Interfaces
{
    public interface IMember
    {
        StorageClass StorageClass
        {
            get;
            set;
        }
        AccessSpecifier AccessScope
        {
            get;
            set;
        }
        bool IsConstQualified
        {
            get;
            set;
        }
        bool IsVolatileQualified
        {
            get;
            set; 
        }
        bool IsRestrictQualified
        {
            get;
            set;
        }
       
        
    }
}
