using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace Gunit.Interfaces
{
    [Serializable]
    public class Project
    {
        [XmlIgnore]
        public string ProjectPath{get;set;}
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("Files")]
        public ProjectFiles Files { get; set; }
        [XmlArray("ProjectHeaders")]
        [XmlArrayItem("Include")]
        public Include[] ProjectHeaders { get; set; }

        [XmlArray("IncludePaths")]
        [XmlArrayItem("Include")]
        public Include[] IncludePaths { get; set; }

        [XmlArray("LibPaths")]
        [XmlArrayItem("Lib")]
        public Lib[] LibPaths { get; set; }

        [XmlArray("LibNames")]
        [XmlArrayItem("LibName")]
        public LibName[] LibNames { get; set; }

        [XmlArray("Defines")]
        [XmlArrayItem("Define")]
        public Define[] Defines { get; set; }

        [XmlElement("Bin")]
        public Bin Bin { get; set; }

        [XmlArray("Plugins")]
        [XmlArrayItem("Include")]
        public Include[] Plugins { get; set; }
        

    }
    [Serializable]
    public class ProjectFiles
    {
         [XmlArray("SourceFiles")]
         [XmlArrayItem("File")]
        public Gunit.Interfaces.File[] SourceFiles { get; set; }
         [XmlArray("HeaderFiles")]
         [XmlArrayItem("File")]
         public Gunit.Interfaces.File[] HeaderFiles { get; set; }
    }
    [Serializable]
    public class File
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }
    }
    [Serializable]
    public class Include
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }
    }

    [Serializable]
    public class Lib
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }
    }
    [Serializable]
    public class LibName
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }
    }
    [Serializable]
    public class Define
    {
        [XmlAttribute("value")]
        public string value { get; set; }
    }

    [Serializable]
    public class Bin
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }
    }
   
}
