using Gunit.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnit_IDE2010.GunitParser;
namespace GUnit_IDE2010.DataModel
{
    public class TestGeneratorModel:CodeGenDataModel
    {
        public TestGeneratorModel()
        {
            
            Includes += "gtest/gtest.h";
            Includes += "gmock/gmock.h";
            Includes += "tr1/tuple";
            Includes += "limits";
           
            Using += "::testing::TestWithParam";
            Using += "::testing::Values";
            Using += "::testing::Combine";
            Using += "::testing::Bool";
            Using += "::testing::Range";
            Using += "::testing::ExitedWithCode";
        }
        Methods m_method = null;
        string m_workingDirectory = "";
        public Methods Method
        {
            get { return m_method; }
            set { m_method = value; }
        }
        public string WorkingDir
        {
            get { return m_workingDirectory; }
            set { m_workingDirectory = value; }
        }





    }
}
