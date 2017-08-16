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
using ICSharpCode.AvalonEdit;
namespace Gunit.View
{
    /// <summary>
    /// Interaction logic for CodeEditor.xaml
    /// </summary>
    public partial class CodeEditor : UserControl
    {
        MainWindow _Window; 
        private int lastSearchIndex = 0;
        Gunit.Utils.HighlightRenderer _Renderer;
        public CodeEditor(MainWindow win)
        {
            InitializeComponent();
            _Window = win;
            _Renderer = new Gunit.Utils.HighlightRenderer(txtCode);
            txtCode.TextArea.TextView.BackgroundRenderers.Add(_Renderer);
        }
        public TextEditor Editor
        {
            get { return txtCode; }
        }
        public void Save(string filename)
        {
            txtCode.Save(filename);
        }
        public void HighlightLine(Color color, int lineNumber)
        {
            if (lineNumber >= 1 && lineNumber <= txtCode.Document.LineCount)
            {
                txtCode.TextArea.Caret.Offset = txtCode.Document.GetOffset(lineNumber, 0);
                txtCode.TextArea.Caret.BringCaretToView();
                _Renderer.CurrentLine = lineNumber;
                _Renderer.HighlightColor = color;
                txtCode.TextArea.TextView.InvalidateLayer(ICSharpCode.AvalonEdit.Rendering.KnownLayer.Selection);
            }
        }
        int find()
        {
            int lineNumber = -1;
            if (string.IsNullOrEmpty(txtSearchBar.Text))
            {
                lastSearchIndex = 0;
                return lineNumber;
            }
            string editorText = txtCode.Document.Text;
            if (string.IsNullOrEmpty(editorText))
            {
                lastSearchIndex = 0;
                return lineNumber;
            }
            if (lastSearchIndex >= editorText.Count())
            {
                lastSearchIndex = 0;
            }
            int nIndex = editorText.IndexOf(txtSearchBar.Text, lastSearchIndex);
            if (nIndex != -1)
            {

                var line = txtCode.Document.GetLineByOffset(nIndex);
                lastSearchIndex = nIndex + txtSearchBar.Text.Length;
                lineNumber = line.LineNumber;
            }
            else
            {
                lastSearchIndex = 0;
            }
            return lineNumber;
        }
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            int LineNumber = find();
            HighlightLine(Colors.LightCoral, LineNumber);
        }
       
    }
}
