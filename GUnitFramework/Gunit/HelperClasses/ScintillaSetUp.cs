using ScintillaNET;
using System.Drawing;
using ClangSharp;
using System.Windows.Forms;
using System.Collections.Generic;
namespace Gunit.HelperClasses
{
    public enum HighLightType
    {
        ERROR,
        WARNING,
        NORMAL_HIGHLIGHT
    };
    public enum LanguageType
    {
        c,
        cpp,
        cs,
        css,
        html,
        java

    }
    public class CodeLocation
    {
        public string fileName = "";
        public int Line = 0;
        public int Column = 0;

    }
    public class ScintillaSetUp
    {
        private Scintilla m_scintilla;
        
        public string[] googleKeyWords =
        {
          "ASSERT_TRUE",
          "ASSERT_FALSE",
          "EXPECT_TRUE",
          "EXPECT_FALSE",
          "ASSERT_EQ","EXPECT_EQ",
          "ASSERT_NE" , "EXPECT_NE",
          "ASSERT_LT","EXPECT_LT",
          "ASSERT_LE","EXPECT_LE",
          "ASSERT_GT","EXPECT_GT",
          "ASSERT_GE","EXPECT_GE",
          "ASSERT_STREQ","EXPECT_STREQ",
          "ASSERT_STRNE","EXPECT_STRNE",
          "ASSERT_STRCASEEQ","EXPECT_STRCASEEQ",
          "ASSERT_STRCASENE","EXPECT_STRCASENE",
          "SUCCEED",
          "FAIL",
          "ADD_FAILURE",
          "ADD_FAILURE_AT",
          "ASSERT_THROW","EXPECT_THROW",
          "ASSERT_ANY_THROW","EXPECT_ANY_THROW",
          "ASSERT_NO_THROW","EXPECT_NO_THROW",
          "ASSERT_PRED1","EXPECT_PRED1",
          "ASSERT_PRED_FORMAT1","EXPECT_PRED_FORMAT1",
          "ASSERT_PRED_FORMAT2","EXPECT_PRED_FORMAT2",
          "ASSERT_FLOAT_EQ","EXPECT_FLOAT_EQ",
          "ASSERT_DOUBLE_EQ","EXPECT_DOUBLE_EQ",
          "ASSERT_NEAR","EXPECT_NEAR",
          "ASSERT_HRESULT_SUCCEEDED","EXPECT_HRESULT_SUCCEEDED",
          "ASSERT_DEATH","EXPECT_DEATH",
          "ASSERT_DEATH_IF_SUPPORTED","EXPECT_DEATH_IF_SUPPORTED",
          "ASSERT_EXIT","EXPECT_EXIT",
          "SCOPED_TRACE",
          "ASSERT_NO_FATAL_FAILURE","EXPECT_NO_FATAL_FAILURE",
          "Range",
          "Values",
          "ValuesIn",
          "Bool",
          "Combine",
          "INSTANTIATE_TEST_CASE_P",
          "TYPED_TEST_CASE",
          "TYPED_TEST",
          "TYPED_TEST_CASE_P",
          "REGISTER_TYPED_TEST_CASE_P",
          "INSTANTIATE_TYPED_TEST_CASE_P",
          "FRIEND_TEST",
          "EXPECT_FATAL_FAILURE",
          "EXPECT_NONFATAL_FAILURE",
          "EXPECT_FATAL_FAILURE_ON_ALL_THREADS",
          "EXPECT_NONFATAL_FAILURE_ON_ALL_THREADS",
          "RUN_ALL_TESTS",
          "TEST",
          "TEST_F",
          "TEST_P",
          "MOCK_METHOD0",
          "MOCK_METHOD1",
          "MOCK_METHOD2",
          "MOCK_METHOD3",
          "MOCK_METHOD4",
          "MOCK_METHOD5",
          "MOCK_METHOD6",
          "MOCK_METHOD7",
          "MOCK_METHOD8",
          "MOCK_METHOD9",
          "MOCK_METHOD10",
          "MOCK_METHOD0_T",
          "MOCK_METHOD1_T",
          "MOCK_METHOD2_T",
          "MOCK_METHOD3_T",
          "MOCK_METHOD4_T",
          "MOCK_METHOD5_T",
          "MOCK_METHOD6_T",
          "MOCK_METHOD7_T",
          "MOCK_METHOD8_T",
          "MOCK_METHOD9_T",
          "MOCK_METHOD10_T",
          "MOCK_CONST_METHOD0",
          "MOCK_CONST_METHOD1",
          "MOCK_CONST_METHOD2",
          "MOCK_CONST_METHOD3",
          "MOCK_CONST_METHOD4",
          "MOCK_CONST_METHOD5",
          "MOCK_CONST_METHOD6",
          "MOCK_CONST_METHOD7",
          "MOCK_CONST_METHOD8",
          "MOCK_CONST_METHOD9",
          "MOCK_CONST_METHOD10",
           "MOCK_CONST_METHOD0",
          "MOCK_CONST_METHOD1_T",
          "MOCK_CONST_METHOD2_T",
          "MOCK_CONST_METHOD3_T",
          "MOCK_CONST_METHOD4_T",
          "MOCK_CONST_METHOD5_T",
          "MOCK_CONST_METHOD6_T",
          "MOCK_CONST_METHOD7_T",
          "MOCK_CONST_METHOD8_T",
          "MOCK_CONST_METHOD9_T",
          "MOCK_CONST_METHOD10_T",
          "ON_CALL",
          "EXPECT_CALL",
          ".Times",
          ".WillOnce",
          ".WillRepeatedly",
          ".WillByDefault",
          "TestWithParam",
          "ExitedWithCode",
          "Return",
          "InSequence",
          "NiceMock"
                  };
        public ScintillaSetUp(Scintilla l_scintilla)
        {
            m_scintilla = l_scintilla;
            m_scintilla.AssignCmdKey(Keys.Control | Keys.F, Command.Null);
        }
        public CodeLocation GetCodeLocationFromPosition(int position)
        {
            CodeLocation location = new CodeLocation();
            location.Line = m_scintilla.LineFromPosition(position)+1;
            location.Column = position - m_scintilla.Lines[location.Line].Position;
            return location;
        }
        public void scrollToLine(CodeLocation location)
        {
            int currentLine = m_scintilla.FirstVisibleLine;
            m_scintilla.LineScroll(location.Line - currentLine - 1, 0);
        }
        public void clearHighLight()
        {
            m_scintilla.IndicatorClearRange(0, m_scintilla.TextLength);
        }
        public void clearHighLight(CodeLocation location)
        {
            int startPos = getstartPosFromSourceLocation(location);
            if (location.Line <= m_scintilla.Lines.Count)
            {
                m_scintilla.IndicatorClearRange(startPos, m_scintilla.Lines[location.Line - 1].Text.Length);
            }
            m_scintilla.Invalidate();
            
        }
        public int getstartPosFromSourceLocation(CodeLocation location)
        {
            if (location.Line <= m_scintilla.Lines.Count)
            {
                int line = location.Line;
                int Column = location.Column;
                int position = 0;
                position = m_scintilla.Lines[line - 1].Position;
                position += Column - 1;
                return position;
            }
            else
            {
                return 0;
            }
        }
        public string getCurrentWord()
        {
            return m_scintilla.SelectedText;
        }
        public void HighLightLine(CodeLocation location, HighLightType HightLightType)
        {
            // Indicators 0-7 could be in use by a lexer
           
            switch (HightLightType)
            {
                case HighLightType.ERROR:
                    m_scintilla.IndicatorCurrent = 8;
                    m_scintilla.Indicators[8].ForeColor = Color.Red;
                    m_scintilla.Indicators[8].Style = IndicatorStyle.Squiggle;
                    m_scintilla.Indicators[8].Under = true;
                    m_scintilla.Indicators[8].OutlineAlpha = 100;
                    m_scintilla.Indicators[8].Alpha = 100;
                    break;
                case HighLightType.WARNING:
                    m_scintilla.IndicatorCurrent = 9;
                    m_scintilla.Indicators[9].ForeColor = Color.Orange;
                    m_scintilla.Indicators[9].Style = IndicatorStyle.Squiggle;
                    m_scintilla.Indicators[9].Under = true;
                    m_scintilla.Indicators[9].OutlineAlpha = 100;
                    m_scintilla.Indicators[9].Alpha = 100;
                    break;
                case HighLightType.NORMAL_HIGHLIGHT:
                    clearHighLight();
                    m_scintilla.IndicatorCurrent = 10;
                    m_scintilla.Indicators[10].ForeColor = Color.Red;
                    m_scintilla.Indicators[10].Style = IndicatorStyle.DotBox;
                    m_scintilla.Indicators[10].Under = true;
                    m_scintilla.Indicators[10].OutlineAlpha = 100;
                    m_scintilla.Indicators[10].Alpha = 100;
                    break;
                default:
                    break;
            }
            // Update indicator appearance
           
            
            m_scintilla.TargetStart = getstartPosFromSourceLocation(location);
            m_scintilla.TargetEnd =   m_scintilla.Lines[location.Line-1].Text.Length;
            m_scintilla.IndicatorFillRange(m_scintilla.TargetStart, m_scintilla.TargetEnd - location.Column);

        }
        private void Scintilla_setUpScintillaLineFolding_v()
        {
            m_scintilla.Lexer = Lexer.Cpp;
            m_scintilla.Margins[0].Width = 25;
            // Instruct the lexer to calculate folding
            m_scintilla.SetProperty("fold", "1");
            m_scintilla.SetProperty("fold.compact", "1");
            m_scintilla.SetProperty("lexer.cpp.track.preprocessor", "1");
            m_scintilla.SetProperty("lexer.cpp.update.preprocessor", "1");
            // Configure a margin to display folding symbols
            m_scintilla.Margins[2].Type = MarginType.Symbol;
            m_scintilla.Margins[2].Mask = Marker.MaskFolders;
            m_scintilla.Margins[2].Sensitive = true;
            m_scintilla.Margins[2].Width = 50;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                m_scintilla.Markers[i].SetForeColor(SystemColors.ControlLight);
                m_scintilla.Markers[i].SetBackColor(Color.Blue);
            }
            // Configure folding markers with respective symbols
            m_scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            m_scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            m_scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            m_scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            m_scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            m_scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            m_scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            m_scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
            m_scintilla.StyleResetDefault();
            m_scintilla.Styles[Style.Default].Font = "Consolas";
            m_scintilla.Styles[Style.Default].Size = 10;
            m_scintilla.IndentationGuides = IndentView.LookBoth;
            m_scintilla.Styles[Style.BraceLight].BackColor = Color.DarkBlue;
            m_scintilla.Styles[Style.BraceLight].ForeColor = Color.BlueViolet;
            m_scintilla.Styles[Style.BraceBad].ForeColor = Color.Red;
            m_scintilla.StyleClearAll();
            


        }
        public void Scintialla_updateMacros(List<string> macros)
        {
            if (macros.Count> 0)
            {
                m_scintilla.SetKeywords(5, string.Join(" ",macros.ToArray()));
            }
        }
        public int Scintialla_search(int startPosition,string word)
        {
            m_scintilla.TargetStart = startPosition+1;
            m_scintilla.TargetEnd = m_scintilla.TextLength;
            return m_scintilla.SearchInTarget(word);
            


        }
        /// <summary>
        /// Init Scintilla Object
        /// </summary>
        public void Scintilla_Init()
        {
            Scintilla_setUpScintillaLineFolding_v();
            // Configure the CPP (C#) lexer styles
            m_scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            m_scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            m_scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            m_scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Green
            m_scintilla.Styles[Style.Cpp.CommentDoc].ForeColor = Color.FromArgb(0, 128, 0); // Green
            m_scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            m_scintilla.Styles[Style.Cpp.GlobalClass].ForeColor = Color.DarkCyan;
            m_scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            m_scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            m_scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            m_scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            m_scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            m_scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            m_scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            m_scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            //m_scintilla.Styles[Style.Cpp.Default + 64].BackColor = Color.LightGray;
            //m_scintilla.Styles[Style.Cpp.Comment + 64].BackColor = Color.LightGray;
            m_scintilla.Lexer = Lexer.Cpp;
            m_scintilla.Text = "";
            Scintilla_LoadLanguageSyntax(LanguageType.cpp);
            
            

        }
        /// <summary>
        /// set the syntax highlighting for a language
        /// </summary>
        /// <param name="language"></param>
        public void Scintilla_LoadLanguageSyntax(LanguageType language)
        {
            switch (language)
            {
                case LanguageType.c:
                    m_scintilla.SetKeywords(0, "if else switch case default break goto return for while do continue typedef sizeof NULL");
                    m_scintilla.SetKeywords(1, "void struct union enum char short int long double float signed unsigned const static extern auto register volatile");
                    break;
                case LanguageType.cpp:
                    m_scintilla.SetKeywords(0, "alignof and and_eq bitand bitor break case catch compl const_cast continue default delete do dynamic_cast else false for goto if namespace new not not_eq nullptr operator or or_eq reinterpret_cast return sizeof static_assert static_cast switch this throw true try typedef typeid using while xor xor_eq NULL");
                    m_scintilla.SetKeywords(1, "alignas asm auto bool char char16_t char32_t class const constexpr decltype double enum explicit export extern final float friend inline int long mutable noexcept override private protected public register short signed static struct template thread_local typename union unsigned virtual void volatile wchar_t");
                    m_scintilla.SetKeywords(3, string.Join(" ", googleKeyWords));
                    break;
                case LanguageType.cs:
                    m_scintilla.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
                    m_scintilla.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");
                    break;
                default:
                    m_scintilla.SetKeywords(0, "alignof and and_eq bitand bitor break case catch compl const_cast continue default delete do dynamic_cast else false for goto if namespace new not not_eq nullptr operator or or_eq reinterpret_cast return sizeof static_assert static_cast switch this throw true try typedef typeid using while xor xor_eq NULL");
                    m_scintilla.SetKeywords(1, "alignas asm auto bool char char16_t char32_t class const constexpr decltype double enum explicit export extern final float friend inline int long mutable noexcept override private protected public register short signed static struct template thread_local typename union unsigned virtual void volatile wchar_t");
                    break;
            }
          
        }

        
    }
}
