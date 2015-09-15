using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUnit_IDE2010.DataModel;
using System.Data.SqlServerCe;
using GUnit_IDE2010.DataBase;
using System.Windows.Forms;
namespace Gunit.DataModel
{
    public class DBManager:DataBaseInterface
    {

        public DBManager()
        {

        }
        public DBManager(string dbPath)
        {
            if (File.Exists(dbPath) == false)
            {
                createDataBase(dbPath);
            }
        }
       
        
        #region OLD_CODE
        //static string m_connectionString = "";
        //static SqlCeConnection m_Connection;
        //static string m_DBName = "";
        //public static void DBManager_disconnect()
        //{
        //    if (m_Connection != null)
        //    {
               
        //          m_Connection.Close();
            
        //    }
        //}
       
        //public static bool DBManager_IsFileModified(string fileName)
        //{
        //    bool result = false;
        //    if (m_DB != null)
        //    {

        //    }

            
            //if (m_Connection != null)
            //{
            //    try
            //    {
            //        DateTime currTime = File.GetLastWriteTime(fileName);
            //        m_Connection.Close();
            //        m_Connection.Open();
            //        string checkString = "SELECT count(*) FROM ProjectFiles WHERE FilePath LIKE @FileName and LastModifiedTime = @LastTime;";
            //        SqlCeCommand checkCmd = new SqlCeCommand(checkString, m_Connection);
            //        checkCmd.Parameters.AddWithValue("@FileName", fileName);
            //        checkCmd.Parameters.AddWithValue("@LastTime", currTime);
            //        int temp = Convert.ToInt32(checkCmd.ExecuteScalar().ToString());
            //        if (temp == 0)
            //        {
            //            result = true;
            //        }
            //        else
            //        {
            //            result = false;
            //        }
            //    }
            //    catch
            //    {

            //    }
            //    finally
            //    {
            //        m_Connection.Close();
            //    }

            //}
         
            //return result; 

        //}
        
        //public static string DBmanager_getConnectionString()
        //{
        //    return m_connectionString;
        //}
        
    //    public static void DBManager_Connect(string l_dbPath)
    //    {
    //        try
    //        {
    //            m_DBName = Path.GetFileNameWithoutExtension(l_dbPath);
    //            m_connectionString = "Data Source=" + l_dbPath;
    //            m_DB = new GUnitDB(m_connectionString);
                
    //            //m_Connection = new SqlCeConnection(m_connectionString);
    //            //m_Connection.Open();
    //        }
    //        catch
    //        {
    //            throw new Exception("Unable To Connect to Database");
    //        }
    //    }
      
    //    public static string CreateDataBase(string projectName,string projectPath)
    //    {
    //        string dir = Path.GetDirectoryName(projectPath);
    //        string dbPath = dir+"\\"+projectName + ".sdf";
    //        string connStr = "Data Source = "+dbPath;
    //        m_connectionString = connStr;
    //        if (File.Exists(dbPath))
    //        {
    //            File.Delete(dbPath);
    //        }

    //        SqlCeEngine engine = new SqlCeEngine(connStr);
    //        engine.CreateDatabase();
    //        SqlCeConnection conn = null;
    //        try
    //        {
    //            conn = new SqlCeConnection(connStr);
    //            conn.Open();
    //            CreateTables();
               

    //        }
    //        catch
    //        {

    //        }
    //        finally
    //        {
    //            conn.Close();

    //        }
    //        return dbPath;
    //        #region oldcode
    //        //String str;
    //        //string l_dbName = projectName;
    //        //string l_dbPathBase = Path.GetDirectoryName(projectPath) + "\\" + l_dbName;
    //        //string l_dbPath = l_dbPathBase + ".mdf";
    //        //SqlConnection myConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Integrated security=SSPI;");
    //        //if (File.Exists(l_dbPath))
    //        //{
    //        //    string detach = @"sys.sp_detach_db '" + l_dbName + "'";
    //        //    SqlCommand dropCmd =  new SqlCommand(detach, myConn); 

    //        //    try
    //        //    {
    //        //        myConn.Open();

    //        //        if (CheckDatabaseExists(l_dbName))
    //        //        {
    //        //            // dropCmd.ExecuteNonQuery();
    //        //            dropCmd.CommandText = "USE " + l_dbName + ";" +
    //        //            "ALTER DATABASE[" + l_dbName + "] SET OFFLINE WITH ROLLBACK IMMEDIATE;" +
    //        //            "DROP DATABASE[" + l_dbName + "];";
    //        //            dropCmd.ExecuteNonQuery();
    //        //        }

    //        //    }
    //        //    catch (System.Exception ex)
    //        //    {
    //        //        dropCmd.CommandText = detach;
    //        //        dropCmd.ExecuteNonQuery();
    //        //        //throw new Exception("Databse Cant be Deleted: " + l_dbName + ex.ToString());
    //        //    }
    //        //    finally
    //        //    {
    //        //        if (myConn.State == ConnectionState.Open)
    //        //        {
    //        //            myConn.Close();

    //        //        }
    //        //    }
    //        //}

    //        //str = "CREATE DATABASE " + l_dbName + " ON PRIMARY " +
    //        //    "(NAME = " + l_dbName + ", " +
    //        //    "FILENAME = '" + l_dbPath + "', " +
    //        //    "SIZE = 5MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
    //        //    "LOG ON (NAME = " + l_dbName + "_Log, " +
    //        //    "FILENAME = '" + l_dbPathBase + ".ldf', " +
    //        //    "SIZE = 5MB, " +
    //        //    "MAXSIZE = 8MB, " +
    //        //    "FILEGROWTH = 10%)";

    //        //SqlCommand myCommand = new SqlCommand(str, myConn);
    //        //try
    //        //{
    //        //    myConn.Open();
    //        //    myCommand.ExecuteNonQuery();
                
    //        //}
    //        //catch (System.Exception ex)
    //        //{
    //        //    throw new Exception("Databse Cant be Created: " + ex.ToString());

    //        //}
    //        //finally
    //        //{
    //        //    if (myConn.State == ConnectionState.Open)
    //        //    {
    //        //        myConn.Close();
    //        //        CreateTables( l_dbName);
    //        //    }
    //        //}
            
    //       // return l_dbPath;
    //        #endregion


    //    }
    //    private static void CreateTables()
    //    {
           
    //        SqlCeConnection conn = new SqlCeConnection(m_connectionString);
    //        conn.Close();
    //        conn.Open();
    //        string sqlew = File.ReadAllText(@"DataBaseSchema.sql");
    //        string[] sqlcmds = sqlew.Split(';');
           
           
    //        try
    //        {
    //            foreach (string cm in sqlcmds)
    //            {
    //                SqlCeCommand cmdNew = new SqlCeCommand(cm + ";", conn);
    //                try
    //                {
    //                    cmdNew.ExecuteNonQuery();
    //                }
    //                catch (Exception err)
    //                {
    //                }
    //            }
               
    //        }
    //        catch (Exception err)
    //        {
    //            throw new Exception("Table Can't be Created: " + err.ToString());
    //        }
    //        finally
    //        {
    //            if (conn.State == ConnectionState.Open)
    //            {
    //                conn.Close();
    //            }
    //        }
    //    }
        
    //    public static void DBManager_AddFiles(string fileName)
    //    {

    //        #region OLD Code
    //        //if (m_Connection != null)
    //        //{
    //        //    m_Connection.Close();
    //        //    m_Connection.Open();
    //        //    string checkString = "SELECT count(*) FROM ProjectFiles WHERE FilePath LIKE @FileName;";
    //        //    SqlCeCommand checkCmd = new SqlCeCommand(checkString, m_Connection);
    //        //    checkCmd.Parameters.AddWithValue("@FileName", fileName);
    //        //    int temp = Convert.ToInt32(checkCmd.ExecuteScalar().ToString());
    //        //    if (temp == 0)
    //        //    {
    //        //        string query = "insert into ProjectFiles (FilePath,LastModifiedTime) values (@FileName,@LastModifiedTime)";
    //        //        DateTime time = File.GetLastWriteTime(fileName);
    //        //        SqlCeCommand myCommand = new SqlCeCommand(query, m_Connection);
    //        //        myCommand.Parameters.AddWithValue("@FileName", fileName);
    //        //        myCommand.Parameters.AddWithValue("@LastModifiedTime", time);
    //        //        myCommand.ExecuteNonQuery();
    //        //    }
    //        //    else
    //        //    {
    //        //        string query = "update  ProjectFiles set FilePath = @FileName, LastModifiedTime = @LastModifiedTime where FilePath LIKE @FileName";
    //        //        DateTime time = File.GetLastWriteTime(fileName);
    //        //        SqlCeCommand myCommand = new SqlCeCommand(query, m_Connection);
    //        //        myCommand.Parameters.AddWithValue("@FileName", fileName);
    //        //        myCommand.Parameters.AddWithValue("@LastModifiedTime", time);
    //        //        myCommand.ExecuteNonQuery();
    //        //    }
    //        //    m_Connection.Close();
    //        //}
    //        #endregion

    //    }
    //    public static DataTable DBManager_getCalledMethods(int ParentmethodID)
    //    {
    //        DataTable dt = new DataTable();
    //        #region OLD_CODE
    //        //if (m_Connection != null)
    //        //{
    //        //    m_Connection.Close();
    //        //    m_Connection.Open();
    //        //    string checkString = "SELECT * FROM CalledMethodsTable WHERE ParentFile LIKE @Parent;";
    //        //    SqlCeCommand checkCmd = new SqlCeCommand(checkString, m_Connection);
    //        //    checkCmd.Parameters.AddWithValue("@Parent", fileName);
    //        //    SqlCeDataAdapter da = new SqlCeDataAdapter(checkCmd);
    //        //    da.Fill(dt);
    //        //    m_Connection.Close();
    //        //}
    //        #endregion
    //        return dt;
    //    }
    //    /// <summary>
    //    /// Get all functions defined in the file
    //    /// </summary>
    //    /// <param name="filePath">location of the file</param>
    //    /// <returns>Datatable containing functions list</returns>
    //    public static DataTable DBManager_getFunctionDefinition(string filePath)
    //    {
    //        DataTable dt = new DataTable();
    //        if (File.Exists(filePath))
    //        {
    //            if (m_Connection != null)
    //            {
    //                m_Connection.Close();
    //                m_Connection.Open();
    //                string checkString = "SELECT * FROM MethodsTable WHERE  ParentFile LIKE @ParentFile and IsDefined = @IsDefined;";
    //                SqlCeCommand checkCmd = new SqlCeCommand(checkString, m_Connection);
    //                checkCmd.Parameters.AddWithValue("@ParentFile", filePath);
    //                checkCmd.Parameters.AddWithValue("@IsDefined", true);
    //                SqlCeDataAdapter da = new SqlCeDataAdapter(checkCmd);
    //                da.Fill(dt);
    //                m_Connection.Close();
    //            }
    //        }
    //        return dt;
            
           
    //    }
    //    /// <summary>
    //    /// get all tokens referred in the project
    //    /// </summary>
    //    /// <returns></returns>
    //    public static ListofStrings DBManager_getTokens()
    //    {
          
    //        ListofStrings l_Tokens = new ListofStrings();
    //        if (m_Connection != null)
    //        {
    //            DataTable dt = new DataTable();
    //            m_Connection.Close();
    //            m_Connection.Open();
    //            string checkString = "SELECT MethodName FROM MethodsTable WHERE  IsDefined = @IsDefined;";
    //            SqlCeCommand checkCmd = new SqlCeCommand(checkString, m_Connection);
    //            checkCmd.Parameters.AddWithValue("@IsDefined", true);
    //            SqlCeDataAdapter da = new SqlCeDataAdapter(checkCmd);
    //            da.Fill(dt);
    //            foreach (DataRow row in dt.Rows)
    //            {
    //                l_Tokens.Add(row[0] as string);
    //            }
    //            m_Connection.Close();
    //        }
    //        return l_Tokens;
    //    }
    //    /// <summary>
    //    /// Get the functionDefinition
    //    /// </summary>
    //    /// <param name="method"></param>
    //    /// <returns></returns>
    //    public static DataTable DBManager_getFunctionDefinition(MethodPrototype method)
    //    {
    //        DataTable dt = new DataTable();
    //        if (m_Connection != null)
    //        {
    //            m_Connection.Close();
    //            m_Connection.Open();
    //            string checkString = "SELECT * FROM Methods WHERE  MethodName LIKE @MethodName and ReturnType LIKE @ReturnType and Parameters LIKE @Parameters and IsDefined = @IsDefined;";
    //            SqlCeCommand checkCmd = new SqlCeCommand(checkString, m_Connection);
    //            checkCmd.Parameters.AddWithValue("@MethodName", method.m_MethodName);
    //            checkCmd.Parameters.AddWithValue("@ReturnType", method.m_returnType);
    //            checkCmd.Parameters.AddWithValue("@Parameters", method.m_parameters);
    //            checkCmd.Parameters.AddWithValue("@IsDefined", true);
    //            SqlCeDataAdapter da = new SqlCeDataAdapter(checkCmd);
    //            da.Fill(dt);
    //            m_Connection.Close();
    //        }
    //        return dt;
    //    }
    //    /// <summary>
    //    /// Remove a file from the database
    //    /// </summary>
    //    /// <param name="fileName"></param>
    //    public static void DBManager_removeFile(string fileName,bool isFileRemovedFromProject = false)
    //    {
    //        if (m_Connection != null)
    //        {
    //            m_Connection.Close();
    //            m_Connection.Open();
    //            try
    //            {
    //                string checkString = "SELECT count(*) FROM ProjectFiles WHERE FilePath LIKE @FileName;";
    //                SqlCeCommand checkCmd = new SqlCeCommand(checkString, m_Connection);
    //                checkCmd.Parameters.AddWithValue("@FileName", fileName);
    //                int temp = Convert.ToInt32(checkCmd.ExecuteScalar().ToString());
    //                if (temp != 0 && isFileRemovedFromProject)
    //                {
    //                    string query = "delete from ProjectFiles where FilePath LIKE @FileName";
    //                    SqlCeCommand myCommand = new SqlCeCommand(query, m_Connection);
    //                    myCommand.Parameters.AddWithValue("@FileName", fileName);
    //                    myCommand.ExecuteNonQuery();
    //                }
    //                temp = 0;
    //                checkString = "SELECT count(*) FROM Methods WHERE ParentFile LIKE @FileName;";
    //                checkCmd = new SqlCeCommand(checkString, m_Connection);
    //                checkCmd.Parameters.AddWithValue("@FileName", fileName);
    //                temp = Convert.ToInt32(checkCmd.ExecuteScalar().ToString());
    //                if (temp != 0)
    //                {
    //                    string query = "delete from Methods where ParentFile LIKE @FileName";
    //                    SqlCeCommand myCommand = new SqlCeCommand(query, m_Connection);
    //                    myCommand.Parameters.AddWithValue("@FileName", fileName);
    //                    myCommand.ExecuteNonQuery();
    //                }
    //                checkString = "SELECT count(*) FROM CalledMethodsTable WHERE ParentFile LIKE @FileName;";
    //                checkCmd = new SqlCeCommand(checkString, m_Connection);
    //                checkCmd.Parameters.AddWithValue("@FileName", fileName);
    //                temp = Convert.ToInt32(checkCmd.ExecuteScalar().ToString());
    //                if (temp != 0)
    //                {
    //                    string query = "delete from CalledMethodsTable where ParentFile LIKE @FileName";
    //                    SqlCeCommand myCommand = new SqlCeCommand(query, m_Connection);
    //                    myCommand.Parameters.AddWithValue("@FileName", fileName);
    //                    myCommand.ExecuteNonQuery();
    //                }
    //            }
    //            catch
    //            {
    //                throw new Exception("Unable to delete entries from Table");
    //            }
    //            m_Connection.Close();
    //        }
    //    }
        
    //    /// <summary>
    //    /// Add called method into database
    //    /// </summary>
    //    /// <param name="result"></param>
    //    public static void DBManager_addCalledMethod(Parser.ParseFunctionResult result)
    //    {
    //        if (m_Connection != null)
    //        {
    //            m_Connection.Close();
    //            m_Connection.Open();
    //            try
    //            {
    //                if (string.IsNullOrWhiteSpace(result.EntityName) == false)
    //                {

    //                    string query = "insert into CalledMethodsTable (MethodName,ReturnType,Parameters,ParentFile,LineNumberInFile,ColumnNumberInFile,ParentName) values(@MethodName,@ReturnType";
    //                    query += ",@Parameters,@ParentFile,@LineNumberInFile,@ColumnNumberInFile,@ParentName)";
    //                    SqlCeCommand myCommand = new SqlCeCommand(query, m_Connection);
    //                    myCommand.Parameters.AddWithValue("@MethodName", result.EntityName);
    //                    myCommand.Parameters.AddWithValue("@ReturnType", result.ResultType);
    //                    string param = String.Join(",", result.Parameters.ToArray());
    //                    myCommand.Parameters.AddWithValue("@Parameters", param);
    //                    myCommand.Parameters.AddWithValue("@ParentFile", result.LocationInFile.File.Name);
    //                    myCommand.Parameters.AddWithValue("@LineNumberInFile", result.LocationInFile.Line);
    //                    myCommand.Parameters.AddWithValue("@ColumnNumberInFile", result.LocationInFile.Column);
    //                    myCommand.Parameters.AddWithValue("@ParentName", result.ParentName);
    //                    myCommand.ExecuteNonQuery();
    //                }
    //            }
    //            catch (Exception err)
    //            {
    //                Console.WriteLine(err.ToString());
    //            }
    //            m_Connection.Close();
    //        }
    //    }
    //    /// <summary>
    //    /// get the location of the function in the file
    //    /// </summary>
    //    /// <param name="method">Description of the method</param>
    //    /// <returns>Location of the function in file</returns>
    //    public static CodeLocation getFunctionLocation(MethodPrototype method)
    //    {
    //        CodeLocation location = new CodeLocation();
    //        DataTable dt = new DataTable();
    //        if (m_Connection != null)
    //        {
    //            m_Connection.Close();
    //            m_Connection.Open();
    //            string checkString = "SELECT ParentFile,LineNumberInFile,ColumnNumberInFile FROM CalledMethodsTable WHERE  MethodName LIKE @MethodName and ReturnType LIKE @ReturnType and Parameters LIKE @Parameters and ParentFile LIKE @ParentFile;";
    //            SqlCeCommand checkCmd = new SqlCeCommand(checkString, m_Connection);
    //            checkCmd.Parameters.AddWithValue("@MethodName", method.m_MethodName);
    //            checkCmd.Parameters.AddWithValue("@ReturnType", method.m_returnType);
    //            checkCmd.Parameters.AddWithValue("@Parameters", method.m_parameters);
    //            checkCmd.Parameters.AddWithValue("@ParentFile", method.m_parent);
    //            SqlCeDataAdapter da = new SqlCeDataAdapter(checkCmd);
    //            da.Fill(dt);
    //            m_Connection.Close();
    //        }
    //        if (dt.Rows.Count >= 1)
    //        {
    //            location.fileName = (dt.Rows[0][0]).ToString();
    //            location.Line = Convert.ToInt32((dt.Rows[0][1]).ToString());
    //            location.Column = Convert.ToInt32((dt.Rows[0][2]).ToString());
    //        }
    //        return location;

    //    }
    //    /// <summary>
    //    /// Adding a method to the DataBase
    //    /// </summary>
    //    /// <param name="result"></param>
    //    public static void DBManager_addMethod(Parser.ParseFunctionResult result)
    //    {
    //        if (m_Connection != null)
    //        {
    //            m_Connection.Close();
    //            m_Connection.Open();
    //            try
    //            {
    //                string query = "insert into MethodsTable (MethodName,ReturnType,Parameters,ParentFile,LineNumberInFile,ColumnNumberInFile,ParentName,IsDefined,IsVirtual,IsPureVirtual,AccessScope,StorageClass,MethodType) values(@MethodName,@ReturnType";
    //                query += ",@Parameters,@ParentFile,@LineNumberInFile,@ColumnNumberInFile,@ParentName,@IsDefined,@IsVirtual,@IsPureVirtual,@AccessScope,@StorageClass,@MethodType)";
    //                SqlCeCommand myCommand = new SqlCeCommand(query, m_Connection);
    //                myCommand.Parameters.AddWithValue("@MethodName", result.EntityName);
    //                myCommand.Parameters.AddWithValue("@ReturnType", result.ResultType);
    //                string param = String.Join(",", result.Parameters.ToArray());
    //                myCommand.Parameters.AddWithValue("@Parameters", param);
    //                myCommand.Parameters.AddWithValue("@ParentFile", result.FileName);
    //                myCommand.Parameters.AddWithValue("@LineNumberInFile", result.LocationInFile.Line);
    //                myCommand.Parameters.AddWithValue("@ColumnNumberInFile", result.LocationInFile.Column);
    //                myCommand.Parameters.AddWithValue("@ParentName", result.ParentName);
    //                myCommand.Parameters.AddWithValue("@IsDefined", result.IsDefined);
    //                myCommand.Parameters.AddWithValue("@IsVirtual", result.IsVirtualMethod);
    //                myCommand.Parameters.AddWithValue("@IsPureVirtual", result.IsPureVirtualMethod);
    //                myCommand.Parameters.AddWithValue("@AccessScope", (int)result.AccessScope);
    //                myCommand.Parameters.AddWithValue("@StorageClass", (int)result.StorageClassSpecifier);
    //                myCommand.Parameters.AddWithValue("@MethodType", (int)result.ParseResultType);
    //                myCommand.ExecuteNonQuery();
    //            }
    //            catch (Exception err)
    //            {

    //            }
    //            m_Connection.Close();
    //        }

        //    }
        #endregion
        /// <summary>
        /// Create the DataBase
        /// </summary>
        /// <param name="dbPath">Path to the DataBaseFile</param>
        public void createDataBase(string dbPath)
        {
           
            string connStr = "Data Source = " + dbPath+";File Mode=read write;";
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }

            SqlCeEngine engine = new SqlCeEngine(connStr);
            engine.CreateDatabase();
            SqlCeConnection conn = null;
            try
            {
                conn = new SqlCeConnection(connStr);
                conn.Open();
                CreateTables(connStr, @"DataBaseSchema.sql");


            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
                Application.Exit();
            }
            finally
            {
                conn.Close();

            }
              
        }
        /// <summary>
        /// Create the Tables according to the the Schema
        /// </summary>
        /// <param name="dbpath"></param>
        private void CreateTables(string dbpath,string schema)
        {
            if (File.Exists(schema) == false)
            {
                throw new FileNotFoundException("DataBaseSchemaNotFound");
            }
            else
            {
                SqlCeConnection conn = new SqlCeConnection(dbpath);
                conn.Close();
                conn.Open();
                string sqlew = File.ReadAllText(schema);
                string[] sqlcmds = sqlew.Split(';');
                string currentquerry = "";
                try
                {
                    foreach (string cm in sqlcmds)
                    {
                        if (string.IsNullOrWhiteSpace(cm) == false && cm != "\n" && cm != "\r" && cm != "\t")
                        {
                            currentquerry = cm;
                            SqlCeCommand cmdNew = new SqlCeCommand(cm + ";", conn);
                            cmdNew.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception err)
                {
                    throw new Exception("Table Can't be Created: CurrentQuerry ="+ currentquerry+" :"+ err.ToString());
                }
                finally
                {
                   conn.Close();
                }
            }
        }
        /// <summary>
        /// Connect to the Database
        /// </summary>
        /// <param name="DBPath"></param>
        public GUnitDB connectToDataBase(string DBPath)
        {
            GUnitDB l_db = null;
            if (File.Exists(DBPath))
            {
                l_db = new GUnitDB("Data Source = " + DBPath);
              
            }
            return l_db;
        }

      
    }
        
}
