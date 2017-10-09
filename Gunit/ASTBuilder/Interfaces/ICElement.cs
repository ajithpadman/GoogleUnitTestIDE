using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public enum CAccessSpecifier
    {
        Global,
        Local
    }
    /// <summary>
    /// 
    /// </summary>
    public enum CStorageClass
    {
        Invalid,
        None,
        Extern,
        Static,
        Auto,
        Register
    }
    /// <summary>
    /// Element in a C source code can be a function or a variable  
    /// </summary>
    public interface ICElement
    {
        CAccessSpecifier AccessSpecifier
        {
            get;
            set;
        }
        CStorageClass StorageClass
        {
            get;
            set;
        }
    }
}
