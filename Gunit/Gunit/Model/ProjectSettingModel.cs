using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gunit.View;
using System.Xml.Serialization;
namespace Gunit.Model
{
    public class ProjectSettingModel
    {
        ProjectViewModel m_model;
       
        public ProjectSettingModel(ProjectViewModel model)
        {
            m_model = model;
            
        }
        public void addIncludePaths(object path)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.SelectedPath = Path.GetDirectoryName(m_model.ProjectPath);
            DialogResult result = browser.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (m_model.IncludePaths.Contains(browser.SelectedPath) == false)
                {
                    m_model.IncludePaths.Add(browser.SelectedPath);
                }
            }
        }
        public void RemoveIncludePath(object data)
        {

         
                if (m_model.SelectedIncludePath >= 0)
                {
                    m_model.IncludePaths.RemoveAt(m_model.SelectedIncludePath);
                    m_model.SelectedIncludePath = -1;
                }
           
        }
        public void addLibPaths(object path)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.SelectedPath = Path.GetDirectoryName(m_model.ProjectPath);
            DialogResult result = browser.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (m_model.LibraryPaths.Contains(browser.SelectedPath) == false)
                {
                    m_model.LibraryPaths.Add(browser.SelectedPath);
                }
            }
        }
        public void RemoveLibPath(object data)
        {

            if (m_model.SelectedLibraryPath >= 0)
            {
                m_model.LibraryPaths.RemoveAt(m_model.SelectedLibraryPath);
                m_model.SelectedLibraryPath = -1;
            }
        }
        public void AddLibName(object data)
        {
            UserInput input = new UserInput("Enter Library Name");
            input.ShowDialog();
            if (string.IsNullOrWhiteSpace(input.Value) == false)
            {
                m_model.LibNames.Add(input.Value);
            }
        }
        public void RemoveLibName(object data)
        {

            if (m_model.SelectedLibraryName >= 0)
            {
                m_model.LibNames.RemoveAt(m_model.SelectedLibraryName);
                m_model.SelectedLibraryName = -1;
            }
        }

        public void AddMacro(object data)
        {
            UserInput input = new UserInput("Enter Macro Define");
            input.ShowDialog();
            if (string.IsNullOrWhiteSpace(input.Value) == false)
            {
                m_model.Defines.Add(input.Value);
            }
        }
        public void RemoveMacro(object data)
        {

            if (m_model.SelectedDefine >= 0)
            {
                m_model.Defines.RemoveAt(m_model.SelectedDefine);
                m_model.SelectedDefine = -1;
            }
        }

       

    }
}
