using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gunit.DataModel;

using System.Runtime.InteropServices;
using System.Threading;

namespace Gunit.Ui
{
    public partial class ConsoleUi : GUnitSideBarBase
    {
       
        public ConsoleUi(ConsoleDataModel model)
            : base(model)
        {
            InitializeComponent();
        }

        
        /// <summary>
        /// 
        /// Overiding the function to handle propety change in model
        /// </summary>
        /// <param name="propertyName"></param>
        public override void propertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case "mode":
                case "ConsoleUpdate":
                     ConsoleUi_UpdateText(((ConsoleDataModel)m_model).Mode);
                
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Update the Condsole Text
        /// </summary>
        /// <param name="mode"></param>
        private void ConsoleUi_UpdateConsole(ConsoleMode mode)
        {
            ListOfConsoleData l_list = new ListOfConsoleData();
            txtConsole.Text = "";
            switch (mode)
            {
                case ConsoleMode.NORMAL:
                    l_list .AddRange( ((ConsoleDataModel)m_model).ConsoleLines);
                    txtConsole.ForeColor = Color.Black;
                    txtConsole.BackColor = Color.White;
                    break;
                case ConsoleMode.ERROR:
                    l_list .AddRange(((ConsoleDataModel)m_model).Errors);
                    txtConsole.ForeColor = Color.Red;
                    txtConsole.BackColor = Color.Black;
                    break;
                case ConsoleMode.WARNING:
                    l_list.AddRange(((ConsoleDataModel)m_model).Warnings);
                    txtConsole.ForeColor = Color.Orange;
                    txtConsole.BackColor = Color.Blue;
                    break;
                case ConsoleMode.EXCEPTION:
                    l_list.AddRange(((ConsoleDataModel)m_model).Exceptions);
                    txtConsole.ForeColor = Color.Blue;
                    txtConsole.BackColor = Color.Cyan;
                    break;
                default:
                    l_list = null;
                    break;
            }
            if (l_list != null)
            {
               

                    foreach (string listElement in l_list)
                    {
                        try
                        {
                            txtConsole.AppendText(listElement);
                            txtConsole.AppendText("\n");
                            Application.DoEvents();
                        }catch
                        {

                        }
                       
                    }
                    //txtConsole.SelectionStart = txtConsole.Text.Length; //Set the current caret position at the end
                    //txtConsole.ScrollToCaret();
               
            }
            else
            {
                txtConsole.Text = "";
            }
        }
        /// <summary>
        /// Update the text in Console thread safe
        /// </summary>
        /// <param name="mode"></param>
        public void ConsoleUi_UpdateText(ConsoleMode mode)
        {
           
            if (this.txtConsole.InvokeRequired)
            {
                txtConsole.Invoke((MethodInvoker)delegate
                { ConsoleUi_UpdateConsole(mode); });
               
            }
            else
            {
                ConsoleUi_UpdateConsole(mode);
            }
        }
        public override void ParserRunning()
        {
            base.ParserRunning();
            if (this.txtConsole.InvokeRequired)
            {
                txtConsole.Invoke((MethodInvoker)delegate
                {
                    ((ConsoleDataModel)m_model).ConsoleLines.Clear();
                    ((ConsoleDataModel)m_model).Warnings.Clear();
                    ((ConsoleDataModel)m_model).Errors.Clear();
                    ((ConsoleDataModel)m_model).Exceptions.Clear();
                    txtConsole.Text = "";
                });

            }
            else
            {
                txtConsole.Clear();
            }
        }
        public override void closeProject()
        {
            base.closeProject();
            ConsoleUi_Clear();
        }
        private void ConsoleUi_Load(object sender, EventArgs e)
        {
           ((ConsoleDataModel)m_model).Mode = ConsoleMode.NORMAL;
           ((ConsoleDataModel)m_model).ConsoleLines += "Test";
        }

        private void btnConsole_Click(object sender, EventArgs e)
        {
            ((ConsoleDataModel)m_model).Mode = ConsoleMode.NORMAL;
        }

        private void btnWarning_Click(object sender, EventArgs e)
        {
            ((ConsoleDataModel)m_model).Mode = ConsoleMode.WARNING;
        }

        private void btnError_Click(object sender, EventArgs e)
        {
            ((ConsoleDataModel)m_model).Mode = ConsoleMode.ERROR;
        }
        public void ConsoleUi_Clear()
        {
            ((ConsoleDataModel)m_model).ConsoleLines.Clear();
            ((ConsoleDataModel)m_model).Warnings.Clear();
            ((ConsoleDataModel)m_model).Errors.Clear();
            ((ConsoleDataModel)m_model).Exceptions.Clear();
            txtConsole.Text = "";
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ConsoleUi_Clear();
        }
    }
}
