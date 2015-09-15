using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using System.IO;
using System.Data;
namespace GUnit_IDE2010.DataModel
{
    class LocalDBCreator
    {
        private SqlCeConnection _sqlConnection;
        private string _cacheDatabase;
        private string _password;

        /// <summary>
        /// 
        /// </summary>
        public LocalDBCreator()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string ConnectionString()
        {
            return string.Format("DataSource=\"{0}\"; Password='{1}'", this._cacheDatabase, this._password);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Open()
        {
            if (_sqlConnection == null)
            {
                _sqlConnection = new SqlCeConnection(this.ConnectionString());
            }

            if (_sqlConnection.State == ConnectionState.Closed)
            {
                _sqlConnection.Open();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Close()
        {
            if (_sqlConnection != null)
            {
                if (_sqlConnection.State == ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateDatabase(string dbname, string password)
        {
            string strCommand;
            try
            {
                Open();
                strCommand = "CREATE DATABASE " + dbname;
                SqlCeCommand sqlCommand = new SqlCeCommand(strCommand, _sqlConnection);
                sqlCommand.ExecuteNonQuery();
                Close();

                _cacheDatabase = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + dbname;
                _password = password;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error in CreateDatabase(): " + exc.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Delete()
        {
            string strCommand;
            try
            {
                Open();
                strCommand = "DROP DATABASE MyTable";
                SqlCeCommand sqlCommand = new SqlCeCommand(strCommand, _sqlConnection);
                sqlCommand.ExecuteNonQuery();
                Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error in Delete(): " + exc.Message);
            }
        }
    }
}
