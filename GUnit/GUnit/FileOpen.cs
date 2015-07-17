using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using ScintillaNET;
using System.Threading;
using System.IO;
namespace GUnit
{
    public partial class FileOpen : DockContent
    {
        int lastCaretPos = 0;
        public string filePath = "";
        GUnitData m_data;
        string m_changeFileText = "";
        string m_NormalFileText = "";
        GUnit m_parent;
        public delegate void onFIleModified(FileInfo file);
        public event onFIleModified evFileModified; 
        FileInfo fileData = null;
        private const int BOOKMARK_MARGIN = 1; // Conventionally the symbol margin
        private const int BOOKMARK_MARKER = 3; // Arbitrary. Any valid index would work.
        public void clearHighLight()
        {
            scintilla.IndicatorClearRange(0, scintilla.TextLength);
        }
        public void HighLightLine(int line, Color color, bool bclearHighLight = true,  int num = 8 )
        {
            if (bclearHighLight == true)
            {
                clearHighLight();
            }
            // Indicators 0-7 could be in use by a lexer
            // so we'll use indicator 8 to highlight words.
             int NUM = num;

            // Remove all uses of our indicator
            scintilla.IndicatorCurrent = NUM;


            // Update indicator appearance
            scintilla.Indicators[NUM].Style = IndicatorStyle.StraightBox;
            scintilla.Indicators[NUM].Under = true;
            scintilla.Indicators[NUM].ForeColor = color;
            scintilla.Indicators[NUM].OutlineAlpha = 50;
            scintilla.Indicators[NUM].Alpha = 20;
            string text = "";
            for (int i = 0; i < line - 1; i++)
            {
                text += scintilla.Lines[i].Text;
            }
                // Search the document
            scintilla.TargetStart = text.Length;
            scintilla.TargetEnd = scintilla.Lines[line - 1].Text.Length;
            scintilla.IndicatorFillRange(scintilla.TargetStart, scintilla.TargetEnd);
           
        }
        public void HighlightWord(string text)
        {
            // Indicators 0-7 could be in use by a lexer
            // so we'll use indicator 8 to highlight words.
            const int NUM = 8;

            // Remove all uses of our indicator
            scintilla.IndicatorCurrent = NUM;
            

            // Update indicator appearance
            scintilla.Indicators[NUM].Style = IndicatorStyle.StraightBox;
            scintilla.Indicators[NUM].Under = true;
            scintilla.Indicators[NUM].ForeColor = Color.BlueViolet;
            scintilla.Indicators[NUM].OutlineAlpha = 50;
            scintilla.Indicators[NUM].Alpha = 30;

            // Search the document
            scintilla.TargetStart = 0;
            scintilla.TargetEnd = scintilla.TextLength;
            scintilla.SearchFlags = SearchFlags.None;
            while (scintilla.SearchInTarget(text) != -1)
            {
                // Mark the search results with the current indicator
                scintilla.IndicatorFillRange(scintilla.TargetStart, scintilla.TargetEnd - scintilla.TargetStart);

                // Search the remainder of the document
                scintilla.TargetStart = scintilla.TargetEnd;
                scintilla.TargetEnd = scintilla.TextLength;
            }
        }
        private void scintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            // Has the caret changed position?
            var caretPos = scintilla.CurrentPosition;
            if (lastCaretPos != caretPos)
            {
                lastCaretPos = caretPos;
                var bracePos1 = -1;
                var bracePos2 = -1;

                // Is there a brace to the left or right?
                if (caretPos > 0 && IsBrace(scintilla.GetCharAt(caretPos - 1)))
                    bracePos1 = (caretPos - 1);
                else if (IsBrace(scintilla.GetCharAt(caretPos)))
                    bracePos1 = caretPos;

                if (bracePos1 >= 0)
                {
                    // Find the matching brace
                    bracePos2 = scintilla.BraceMatch(bracePos1);
                    if (bracePos2 == Scintilla.InvalidPosition)
                    {
                        scintilla.BraceBadLight(bracePos1);
                        scintilla.HighlightGuide = 0;
                    }
                    else
                    {
                        scintilla.BraceHighlight(bracePos1, bracePos2);
                        scintilla.HighlightGuide = scintilla.GetColumn(bracePos1);
                    }
                }
                else
                {
                    // Turn off brace matching
                    scintilla.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                    scintilla.HighlightGuide = 0;
                }
            }
        }
        public FileOpen(GUnit parent,string fileName)
        {
            
            InitializeComponent();
            m_parent = parent;
            m_data = m_parent.m_data;
            this.filePath = fileName;
            this.Text = Path.GetFileName(fileName);
          
           
        }
        public void FileOpen_close(FileInfo data)
        {
            if (data.m_fileName == this.filePath)
            {
                this.Close();
            }
            
        }
        private void UpdateLineNumbers(int startingAtLine,string value)
        {
          
            scintilla.Lines[startingAtLine].MarginStyle = Style.LineNumber;
            scintilla.Lines[startingAtLine].MarginText = value;
            
       
        }
        public void FileOpen_HighlightCoverage(Coverage coverageReport)
        {
           
            foreach (LineStatus line in coverageReport.m_LineStatus)
            {
                scintilla.Margins[0].Type = MarginType.Text;
                scintilla.Margins[0].Width = 35;
                
                if (line.m_isExecutable)
                {
                    if (line.m_ExecutionCount > 0)
                    {
                        string exeCount =  line.m_ExecutionCount.ToString();
                        UpdateLineNumbers((int)line.m_lineNumber - 1, exeCount);
                        HighLightLine((int)line.m_lineNumber, Color.Green, false, 8);

                    }
                    else
                    {
                        scintilla.Lines[(int)line.m_lineNumber - 1].MarginStyle = Style.LineNumber;
                        scintilla.Lines[(int)line.m_lineNumber - 1].MarginText = line.m_ExecutionCount.ToString();
                        HighLightLine((int)line.m_lineNumber, Color.Red, false, 9);
                    }
                }
                else
                {
                    scintilla.Lines[(int)line.m_lineNumber - 1].MarginStyle = Style.LineNumber;
                    scintilla.Lines[(int)line.m_lineNumber - 1].MarginText = "-";
                }
                
            }
        }
        private void FileOpen_Load(object sender, EventArgs e)
        {
            FileOpen_setUpScintilla_v();
            m_parent.evTextHighlightUpdate += new GUnit.onSyntaxHighlightUpdate(setCodeKeyWords);
            m_parent.evCloseDocument += new GUnit.onDocumentClose(FileOpen_close);
            m_parent.evSaveProject += new GUnit.onSaveProject(FileOpen_SaveProject);
            m_parent.evCloseAllForms += new GUnit.onCloseAllForms(CloseThisForm);
            m_NormalFileText = this.Text;
            m_changeFileText = this.Text + "*";
           
        }
        public void CloseThisForm()
        {

            this.Close();
        }
        public void FileOpen_SaveProject(ProjectInfo file)
        {
            
            this.Text = m_NormalFileText;
            
        }

        public void FileOpen_DisplayFunction(int lineNumber)
        {
            int currentLine = scintilla.FirstVisibleLine;
            scintilla.LineScroll(lineNumber - currentLine -1, 0);
            HighLightLine(lineNumber,Color.Blue);
            
        }
        public void updateSourceText(string newValue)
        {
           
            if (scintilla.InvokeRequired)
            {
               
                scintilla.Invoke((MethodInvoker)delegate { scintilla.Text = newValue; });

            }

            else
            {

                scintilla.Text = newValue;

            }
            const int padding = 2;
            var maxLineNumberCharLength = scintilla.Lines.Count.ToString().Length;
            scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
        }
        private void scintilla_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == BOOKMARK_MARGIN)
            {
                // Do we have a marker for this line?
                const uint mask = (1 << BOOKMARK_MARKER);
                var line = scintilla.Lines[scintilla.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & mask) > 0)
                {
                    // Remove existing bookmark
                    line.MarkerDelete(BOOKMARK_MARKER);
                }
                else
                {
                    // Add bookmark
                    line.MarkerAdd(BOOKMARK_MARKER);
                }
            }
        }
        public void FileOpen_setScintillaText_v(string text)
        {
            scintilla.Text += text +"\n"; 
        }
        private void FileOpen_setUpScintillaLineFolding_v()
        {
            scintilla.Lexer = Lexer.Cpp;
            scintilla.Margins[0].Width = 25;
            // Instruct the lexer to calculate folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 50;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLight);
                scintilla.Markers[i].SetBackColor(Color.Blue);
            }
            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;
         
            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.IndentationGuides = IndentView.LookBoth;
            scintilla.Styles[Style.BraceLight].BackColor = Color.DarkBlue;
            scintilla.Styles[Style.BraceLight].ForeColor = Color.BlueViolet;
            scintilla.Styles[Style.BraceBad].ForeColor = Color.Red;
            scintilla.StyleClearAll();
           

        }
        private static bool IsBrace(int c)
        {
            switch (c)
            {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '<':
                case '>':
                    return true;
            }

            return false;
        }
        public void setCodeKeyWords()
        {
            string stdTypes = "";
            scintilla.SetKeywords(0, "void if else switch case default break goto return for while do continue typedef sizeof NULL alignof and and_eq bitand bitor break case catch compl const_cast continue default delete do dynamic_cast else false for goto if namespace new not not_eq nullptr operator or or_eq reinterpret_cast return sizeof static_assert static_cast switch this throw true try typedef typeid using while xor xor_eq NULL enum");
            foreach (string str in m_data.m_standardTypes)
            {
                stdTypes += str +" ";
            }
            foreach (string str in m_data.m_standardQualifiers)
            {
                stdTypes += str + " ";
            }
            foreach (string str in m_data.m_standardUtestMacros)
            {
                stdTypes += str + " ";
            }
            scintilla.SetKeywords(1, stdTypes);
        }
        private void FileOpen_setUpScintilla_v()
        {
            FileOpen_setUpScintillaLineFolding_v();
            // Configure the CPP (C#) lexer styles
            scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Green
            scintilla.Styles[Style.Cpp.CommentDoc].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            scintilla.Lexer = Lexer.Cpp;
            scintilla.Text = "";
            // Set the keywords
            setCodeKeyWords();
            
        }

        private void FileOpen_DocumentActive(object sender, EventArgs e)
        {
            FileInfo file = m_parent.m_data.GUnitData_getFileInformation(this.filePath);
            m_parent.GUnit_UpdateDocumentFocusChange(file);
            
        }
        
        private void FileOpen_TextChanged(object sender, EventArgs e)
        {
            FileInfo data = m_parent.m_data.GUnitData_getFileInformation(this.filePath);
            if (data != null)
            {
                if (scintilla.Text != data.m_text)
                {
                    m_parent.m_data.m_IsProjectChanged = true;
                    data.m_IsDirty = true;
                     data.m_text = scintilla.Text;
                    this.Text = m_changeFileText;
                    m_parent.m_data.GUnitData_UpdateProjectTable(data.m_fileName, data);
                }
            }
        }
    }
}
