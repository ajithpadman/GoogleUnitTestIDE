using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
namespace GUnit
{
    public partial class Console : DockContent
    {
        private GUnit m_parent;
        public Console(GUnit parent)
        {
            InitializeComponent();
            m_parent = parent;
        }
        private void Console_closeEvents()
        {
            m_parent.evConsoleUpdate -= (updateText);
            m_parent.evCloseAllForms -= (CloseThisForm);
            m_parent.evConsoleTextColorUpdate -= (updateTextColor);
        }
        private void Console_Load(object sender, EventArgs e)
        {
            m_parent.evConsoleUpdate += (updateText);
            m_parent.evCloseAllForms += (CloseThisForm);
            m_parent.evConsoleTextColorUpdate += (updateTextColor);
        }
        public void CloseThisForm()
        {
            Console_closeEvents();
            this.Close();
        }
        public void updateTextColor(string newValue, Color color)
        {

            if (txtConsole.InvokeRequired)
            {
                try
                {
                    txtConsole.Invoke((MethodInvoker)delegate
                    {
                        int lineNum = 0;
                        foreach (string line in txtConsole.Lines)
                        {
                            if (line.Contains(newValue))
                            {
                                txtConsole.Select(txtConsole.GetFirstCharIndexFromLine(lineNum), line.Length);
                                txtConsole.SelectionColor = color;
                            }
                            lineNum++;
                        }
                      
                    });
                }
                catch
                {

                }

            }

            else
            {

                int lineNum = 0;
                foreach (string line in txtConsole.Lines)
                {
                    if (line.Contains(newValue))
                    {
                        txtConsole.Select(txtConsole.GetFirstCharIndexFromLine(lineNum), line.Length);
                        txtConsole.SelectionColor = color;
                    }
                    lineNum++;
                }

            }

        }
        public void updateText(string newValue)
        {
            
            if (txtConsole.InvokeRequired)
            {
                try
                {
                    txtConsole.Invoke((MethodInvoker)delegate
                    {
                        txtConsole.Text += newValue;
                        // this.Invalidate();
                        //this.Update();
                        //this.Refresh();
                        //Application.DoEvents();
                    });
                }
                catch
                {

                }

            }

            else
            {

                txtConsole.Text += newValue;

            }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Console_ClearConsole();
        }
        public void Console_ClearConsole()
        {
            if (txtConsole.InvokeRequired)
            {
                try
                {
                    txtConsole.Invoke((MethodInvoker)delegate
                    {
                        txtConsole.Text = "";
                        // this.Invalidate();
                        //this.Update();
                        //this.Refresh();
                        //Application.DoEvents();
                    });
                }
                catch
                {

                }

            }

            else
            {

                txtConsole.Text = "";

            }
            
        }
        private void txtConsole_TextChanged(object sender, EventArgs e)
        {
            txtConsole.SelectionStart = txtConsole.Text.Length; //Set the current caret position at the end
            txtConsole.ScrollToCaret();
        }
    }
}
