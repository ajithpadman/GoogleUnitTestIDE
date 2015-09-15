
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


public enum VariableScope
{
    MEMBER,
    GLOBAL,
    LOCAL
}
public partial class GUnitDB : System.Data.Linq.DataContext
{
    public void TruncateTable(string table,string primarykey )
    {
        this.ExecuteCommand("DELETE FROM " + table);
        this.ExecuteCommand("ALTER TABLE " + table + " ALTER COLUMN " + primarykey + " IDENTITY (1,1)");
       
    }
    public void TruncateAllTable()
    {
        TruncateTable("ProjectFiles", "ID");
        TruncateTable("Namespaces", "ID");
        TruncateTable("Classes","ID");
        TruncateTable("GlobalVariables", "ID");
        TruncateTable("GlobalMethods", "ID");
        TruncateTable("MemberMethods", "ID");
        TruncateTable("MemberVariables", "ID");
        TruncateTable("Variables", "ID");
        TruncateTable("Methods", "ID");
        TruncateTable("MethodCalls","ID");
            
    }
    
}