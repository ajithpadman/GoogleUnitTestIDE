using Gunit.DataModel;
using GUnit_IDE2010.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUnit_IDE2010.CodeGenerator
{
    public class BoundaryTestGenerator:IDisposable
    {
       
        private ListofFiles m_requestedFiles = new ListofFiles();
        private List<Classes> m_requestedClasses = new List<Classes>();
        private GUnitDB m_DbCtx = null;
      
        ListofStrings GlobalmethodLimitList = new ListofStrings();
        
        public BoundaryTestGenerator(string l_dbPath)
        {
            m_DbCtx = new GUnitDB("Data Source = " + l_dbPath + ";File Mode=read write;");

        }
        /// <summary>
        /// List of files  whose Global Functions need to be Tested
        /// </summary>
        public ListofFiles FilesList
        {
            get { return m_requestedFiles; }
            set { m_requestedFiles = value; }
        }
        /// <summary>
        /// List of classes whose Member methods need to be tested
        /// </summary>
        public List<Classes> ClassList
        {
            get { return m_requestedClasses; }
            set { m_requestedClasses = value; }
        }
       
        private IEnumerable<GlobalMethods> getMethodsInFile(string fileName)
        {
            return from m in m_DbCtx.GlobalMethods where m.Methods.IsDefined == true && m.Methods.FilePath == fileName&&m.Methods.StorageClass != (int)ClangSharp.StorageClass.SC_Static  select m;
        }
        private IEnumerable<Classes> getClassesFile(string fileName)
        {
            return from m in m_DbCtx.Classes where  m.FilePath == fileName select m;
        }
        private IEnumerable<MemberMethods>getMemberMethods(Classes l_class)
        {
            return from m in m_DbCtx.MemberMethods where m.Classes == l_class select m;
        }
        public  void processFile(string fileName,string workingDir)
        {
            IEnumerable<GlobalMethods> methods = getMethodsInFile(fileName);
            IEnumerable<Classes> classes = getClassesFile(fileName);
            foreach (GlobalMethods m in methods)
            {
                MethodParamFixtureBuilder fixtureBuilder = new MethodParamFixtureBuilder(m.Methods, workingDir);
                //fixtureBuilder.GenerateFixture();
                TestGeneratorModel model = new TestGeneratorModel();
                model.Method = m.Methods;
                model.WorkingDir = workingDir;
                TestGenerator gen = new TestGenerator(model);
                gen.GenerateCode();

            }
            foreach(Classes l_class in classes)
            {
                IEnumerable<MemberMethods> membermethods = getMemberMethods(l_class);
                foreach(MemberMethods method in membermethods)
                {
                    if(method.Methods.AccessScope == 1)
                    {
                        MethodParamFixtureBuilder fixtureBuilder = new MethodParamFixtureBuilder(method.Methods, workingDir);
                        //fixtureBuilder.GenerateFixture(l_class);
                        TestGeneratorModel model = new TestGeneratorModel();
                        model.Method = method.Methods;
                        model.WorkingDir = workingDir;
                        TestGenerator gen = new TestGenerator(model);
                        gen.GenerateCode();
                    }
                }
            }
            
        }
        
        public void Dispose()
        {
            m_DbCtx.Dispose();
        }
    }
}
