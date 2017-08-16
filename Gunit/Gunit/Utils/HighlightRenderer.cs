using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit;
using System.Windows.Media;
using System.Windows;

namespace Gunit.Utils
{
    public class HighlightRenderer:IBackgroundRenderer
    {
        TextEditor _editor;
        public HighlightRenderer(TextEditor editor)
        {
            _editor = editor;
            CurrentLine = 1;
        }
        public int CurrentLine
        {
            get;
            set;
        }
        Color _HighlightColor = Colors.LightBlue;
        public Color HighlightColor
        {
            get { return _HighlightColor; }
            set { _HighlightColor = value; }
        }
        public void Draw(TextView textView, System.Windows.Media.DrawingContext drawingContext)
        {
           
            if (_editor.Document == null)
                return;
            if (_editor.Document.Text == "")
                return;

            textView.EnsureVisualLines();
            if (CurrentLine <= _editor.Document.LineCount && CurrentLine >=1)
            {
                var currentLine = _editor.Document.GetLineByNumber(CurrentLine);
                foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine))
                {
                    drawingContext.DrawRectangle(
                        new SolidColorBrush(HighlightColor), null,
                        new Rect(rect.Location, new Size(textView.ActualWidth - 32, rect.Height)));
                }
            }
            else
            {
                CurrentLine = -1;
            }
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Selection; }
        }
    }
}
