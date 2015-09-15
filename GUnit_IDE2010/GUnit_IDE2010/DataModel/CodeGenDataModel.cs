using Gunit.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnit_IDE2010.DataModel
{
    public class CodeGenDataModel
    {
        private string m_filePath = "";
        private string m_description = "";
        private ListofStrings m_Includes = new ListofStrings();
        private ListofStrings m_Using = new ListofStrings();
        private ListofStrings m_UsingNamespace = new ListofStrings();

        public CodeGenDataModel()
        {

        }
        public string  FilePath
        {
            get { return m_filePath; }
            set { m_filePath = value; }
        }
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }
        public ListofStrings Includes
        {
            get { return m_Includes; }
            set { m_Includes = value; }
        }
        public ListofStrings Using
        {
            get { return m_Using; }
            set { m_Using = value; }
        }
        public ListofStrings UsingNamespace
        {
            get { return m_UsingNamespace; }
            set { m_UsingNamespace = value; }
        }

    }
}
