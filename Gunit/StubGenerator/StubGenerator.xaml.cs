using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gunit.Model;
using Gunit.Interfaces;
using ASTBuilder.ConcreteClasses;
using ASTBuilder.Interfaces;
using System.IO;
using Gunit.Utils;
using System.ComponentModel;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Utils;
namespace StubGenerator
{
    public class CodeGenerator
    {
        string mockName;
        public string MockName { get { return mockName; } }
        private List<string> functionArgumentTypes(ICFunction function)
        {
            List<string> arguments = new List<string>();
            foreach (ICVariable param in function.Parameters)
            {
                arguments.Add(param.Type.Name);
            }
            return arguments;
        }
        private void writeFunctionDefinition(StreamWriter writer, ICFunction method, string mockClassName)
        {
            List<string> arguments = functionArgumentTypes(method);
            arguments.RemoveAll(str => String.IsNullOrEmpty(str));
            string returntype = "void";
            if (string.IsNullOrWhiteSpace(method.ReturnValue.Name) == false)
            {
                returntype = method.ReturnValue.Name;
            }
            writer.Write(CodeTemplates.returnFunctionHeader(method.Name));
            string parameters = "";

            if (arguments.Count() > 0)
            {
                writer.Write(returntype + " " + method.Name + "");
                int count = 0;
                List<string> withValues = new List<string>();
                List<string> Values = new List<string>();
                foreach (string argument in arguments)
                {
                    withValues.Add(argument + " l_arg" + count);
                    Values.Add(" l_arg" + count);
                    count++;
                }
                writer.Write("(" + String.Join(",", withValues.ToArray()) + ")");
                parameters = String.Join(",", Values.ToArray());





            }
            else
            {
                writer.WriteLine(returntype + " " + method.Name + "()");


            }
            writer.WriteLine("{");
            if (returntype != "void")
            {
                writer.WriteLine("  if(NULL != " + mockClassName + "::mp_Instance)");
                writer.WriteLine("  {");
                writer.WriteLine("    return " + mockClassName + "::mp_Instance->mocked_" + method.Name + "(" + parameters + ");");
                writer.WriteLine("  }");
                writer.WriteLine("  else");
                writer.WriteLine("  {");
                writer.WriteLine("    " + mockClassName + " l_MockObject;");

                writer.WriteLine("    return l_MockObject.mocked_" + method.Name + "(" + parameters + ");");
                writer.WriteLine("  }");
            }
            else
            {
                writer.WriteLine("  if(NULL != " + mockClassName + "::mp_Instance)");
                writer.WriteLine("  {");
                writer.WriteLine("     " + mockClassName + "::mp_Instance->mocked_" + method.Name + "(" + parameters + ");");
                writer.WriteLine("  }");
                writer.WriteLine("  else");
                writer.WriteLine("  {");
                writer.WriteLine("    " + mockClassName + " l_MockObject;");
                writer.WriteLine("      l_MockObject.mocked_" + method.Name + "(" + parameters + ");");
                writer.WriteLine("  }");
            }

            writer.WriteLine("}");

        }
        public void generateMock(ICCodeDescription description, string path)
        {
            mockName = "Mock_" + System.IO.Path.GetFileNameWithoutExtension(description.FileName);

            StreamWriter mock_header = new StreamWriter(path + "\\" + mockName + ".h");
            StreamWriter mock_source = new StreamWriter(path + "\\" + mockName + ".cpp");
            mock_header.WriteLine(CodeTemplates.writeFileHeader(mockName + ".h", "mock for Google test creation"));
            mock_source.WriteLine(CodeTemplates.writeFileHeader(mockName + ".cpp", "mock for Google test creation"));

            mock_source.WriteLine("#include \"" + mockName + ".h\"");
            mock_source.WriteLine(mockName + "* " + mockName + "::mp_Instance = 0;");
            mock_header.WriteLine("#ifndef " + mockName.ToUpper());
            mock_header.WriteLine("#define " + mockName.ToUpper());
            mock_header.WriteLine("#include \"gmock/gmock.h\"");
            mock_source.WriteLine(CodeTemplates.writeModuleHeader("\\class " + mockName, "Mock class for the module " + mockName));
            mock_header.WriteLine("class " + mockName + "{");
            mock_header.WriteLine("public:");
            mock_header.WriteLine(" static " + mockName + " *mp_Instance;");
            mock_header.WriteLine(" " + mockName + "(){mp_Instance = NULL;}");
            mock_header.WriteLine(" virtual" + "~" + mockName + "(){}");
            mock_header.WriteLine(" inline void RegisterMock(" + mockName + " *mock){mp_Instance = mock;}");
            mock_header.WriteLine(" inline void UnRegisterMock(){mp_Instance = NULL;}");

            try
            {
                foreach (ICFunction function in description.Functions)
                {
                    List<string> arguments = functionArgumentTypes(function);
                    arguments.RemoveAll(str => String.IsNullOrEmpty(str));
                    int argumentCount = arguments.Count();
                    if (argumentCount <= 10 && argumentCount > 0)
                    {
                        if (function.ReturnValue != null)
                        {
                            mock_header.WriteLine(" MOCK_METHOD" + argumentCount + "(mocked_" + function.Name + "," + function.ReturnValue.Name + "(" + String.Join(",", arguments.ToArray()) + "));");
                            writeFunctionDefinition(mock_source, function, mockName);
                        }
                    }
                    else
                    {
                        if (argumentCount == 0)
                        {
                            if (function.ReturnValue != null)
                            {
                                mock_header.WriteLine(" MOCK_METHOD" + argumentCount + "(mocked_" + function.Name + "," + function.ReturnValue.Name + "());");
                                writeFunctionDefinition(mock_source, function, mockName);
                            }
                        }
                        else
                        {
                            Console.WriteLine(function.ReturnValue.Name + " " + function.Name + " (" + String.Join(",", arguments.ToArray()) + ")\nCannot be Mocked as Number of Arguments Exceeds 10");
                        }
                    }
                }
            }
            catch
            {

            }

            mock_header.WriteLine("};");
            mock_header.WriteLine("#endif");
            mock_header.Close();
            mock_source.Close();


        }

    }

    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class StubGenerator : UserControl,IGunitPlugin
    {
        IProjectModel m_model;
        ICCodeDescription CodeDescription { get; set; }
        ASTBuilder.ASTBuilder m_ast;
        public StubGenerator()
        {
            InitializeComponent();
        }


        
        private IHighlightingDefinition _highlightdef = null;
        
        public IHighlightingDefinition HighlightDef
        {
            get { return this._highlightdef; }
            set
            {
                if (this._highlightdef != value)
                {
                    this._highlightdef = value;
                   
                }
            }
        }
        public string Description
        {
            get { return "Generate Mock Classes using Gmock standard after parsing the Interface"; }
        }

        public void Init(IProjectModel model)
        {
            m_model = model;
            m_model.PropertyChanged += new PropertyChangedEventHandler(m_model_PropertyChanged);
        }

        public void DeInit()
        {
            
        }
        void m_model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           



        }
        public UserControl getView()
        {
            return this;
        }

        public void propertyChangedEvent(string eventName)
        {
           
        }
        private TextDocument readMockFile(string _filePath)
        {
            if (System.IO.File.Exists(_filePath))
            {
               
                this.HighlightDef = HighlightingManager.Instance.GetDefinition("C++");

                using (FileStream fs = new FileStream(_filePath,
                                           FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader reader = FileReader.OpenStream(fs, Encoding.UTF8))
                    {
                        return  new TextDocument(reader.ReadToEnd());
                    }
                }
                
              
            }
            return null;
        }
        public string PluginName
        {
            get { return " C MockGenerator"; }
        }
        private void btnParser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (null != m_model)
                {
                    m_ast = new ASTBuilder.ASTBuilder(m_model.SelectedFile);
                    m_ast.IncludePaths.AddRange(m_model.IncludePaths.ToArray());
                    m_ast.Defines.AddRange(m_model.Defines.ToArray());
                    m_ast.PreIncludeFiles.AddRange(m_model.PreHeaderFiles.ToArray());
                    m_ast.ParseFile();
                    CodeDescription = m_ast.CodeDescription;
                    CodeGenerator generator = new CodeGenerator();
                    generator.generateMock(CodeDescription, System.IO.Path.GetDirectoryName(m_model.ProjectPath));
                    string header = System.IO.Path.GetDirectoryName(m_model.ProjectPath) + "\\" + generator.MockName + ".h";
                    string source = System.IO.Path.GetDirectoryName(m_model.ProjectPath) + "\\" + generator.MockName + ".cpp";

                    txtMockFileDisplay.Document = readMockFile(header);
                    txtMockFileDisplay.SyntaxHighlighting = this.HighlightDef;
                    txtMockFileSrcDisplay.Document = readMockFile(source);
                    txtMockFileSrcDisplay.SyntaxHighlighting = this.HighlightDef;
                }
            }
            catch
            {

            }
        }


        public void Save()
        {
           string mockName =  "Mock_" + System.IO.Path.GetFileNameWithoutExtension(CodeDescription.FileName);
           string header = System.IO.Path.GetDirectoryName(m_model.ProjectPath) + "\\" + mockName + ".h";
           string source = System.IO.Path.GetDirectoryName(m_model.ProjectPath) + "\\" + mockName + ".cpp";
           txtMockFileDisplay.Save(header);
           txtMockFileSrcDisplay.Save(source);
        }


        public void ChangeTheme(string color)
        {
            throw new NotImplementedException();
        }
    }
}
