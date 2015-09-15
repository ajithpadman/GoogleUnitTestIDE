using System;

using System.Windows.Forms;

namespace Gunit.Ui
{
    public enum SubWindowType
    {
        LIBRARYNAME,
        MACRONAME,
        OTHER
    }
    public partial class UserInput : Form
    {
        SubWindowType m_Type = SubWindowType.LIBRARYNAME;
        ProjectSetting m_setting;
        public string Result = "";
        public UserInput(ProjectSetting setting,SubWindowType type = SubWindowType.LIBRARYNAME)
        {
            InitializeComponent();
            m_setting = setting;
            m_Type = type;
        }
        public UserInput()
        {
            InitializeComponent();
            m_setting = null;
            m_Type = SubWindowType.OTHER;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (m_setting != null)
            {
                if (m_Type == SubWindowType.LIBRARYNAME)
                {
                    m_setting.ProjectSetting_addLibraryName(txtLibName.Text);
                    this.Close();
                }
                else
                {
                    m_setting.ProjectSetting_addMacroName(txtLibName.Text);
                    this.Close();
                }
            }
            else
            {
                if (m_Type == SubWindowType.OTHER)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.ControlBox = true;
                }
            }
        }

        private void LibraryName_Load(object sender, EventArgs e)
        {

        }

        private void LibraryName_closing(object sender, FormClosingEventArgs e)
        {
            Result = txtLibName.Text;
        }
    }
}
