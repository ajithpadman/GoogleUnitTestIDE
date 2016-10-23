using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
using CPPASTBuilder.Interfaces;
using System.ComponentModel;
using System.Data;
namespace GUnitFramework.Interfaces
{
    public enum ProjectStatus
    {
        NEW,
        OPEN,
        SAVE,
        CLOSE
       
    }
    public enum FileType
    {
        SourceFile,
        HeaderFile,
        PreInclude,
        Root
    }
    public delegate void onParserOutput(ICCodeDescription desc);
    public delegate void onPluginLoaded(ICGunitPlugin plugin);
    public delegate void onBuildStarted();
    public delegate void onBuidComplete();
    public delegate void onProjectStatus(ProjectStatus status ,object data);
    public interface ICGunitHost:INotifyPropertyChanged
    {
        string CurrentFileInEditor
        {
            get;
            set;
        }
        List<ICCodeDescription> CodeDescriptions
        {
            get;
            set;
        }
        List<ICppCodeDescription> CPPCodeDescriptions
        {
            get;
            set;
        }
        List<ICGunitPlugin> PluginList
        {
            get;
            
        }
        List<ICGunitPlugin> SpecialPluginList
        {
            get;
            
        }
        ICProjectData ProjectData
        {
            get;
            set;

        }
        ICodeBuilder ProjectBuilder
        {
            get;
            
        }
        ICppParser CPPCodeParser
        {
            get;
        }
        IParser CodeParser
        {
            get;
        }
        List<ITestReportGenerator> AvailableTestReportGenerators
        {
            get;
            
        }
        ITestReportGenerator CurrentTestReportGenerator
        {
            get;
            
        }
        ItestRunner TestRunner
        {
            get;
            
        }
        IBoundaryTestGenerator BoundaryTestGenerator
        {
            get;
            
        }
        ICoverageAnalyser CoverageAnalyser
        {
            get;
            
        }
        IMockGenrator MockGenerator
        {
            get;
            
        }
        List<string> SelectedFiles
        {
            get;
        }
         DataTable IncludePathsTable
        {
            get;

        }
         DataTable LibraryPathsTable
        {
            get;
        }
         DataTable LibraryNameTable
        {
            get;
        }
         DataTable MacroDataTable
        {
            get;
        }

         void ProjectDataModel_addIncludePath(string path);
               
         void ProjectDataModel_addLibraryPath(string path);
                
         void ProjectDataModel_addLibraryName(string path);

         void ProjectDataModel_addMacroName(string path);

         void ProjectDataModel_RemoveIncludePath(DataRow index, string value);

         void ProjectDataModel_RemoveLibraryPath(DataRow index, string value);

         void ProjectDataModel_RemoveLibraryName(DataRow index, string value);

         void ProjectDataModel_RemoveMacroName(DataRow index, string value);
         void addSelectedFiles(string file);
         void removeSelectedFiles(string file);
         void AddOrRemoveFile(bool IsFileAdded, FileType l_fileType, string fileName);

        event onParserOutput evParserOutput;
        event onBuildStarted evBuildStarted;
        event onBuidComplete evBuildComplete;
        event onProjectStatus evProjectStatus;
        event onPluginLoaded evPluginLoaded;

        
    }
}
