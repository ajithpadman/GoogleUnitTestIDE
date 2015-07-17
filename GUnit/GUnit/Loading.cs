using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace GUnit
{
    public partial class Loading : Form
    {
        GUnit m_parent;
        public Loading(GUnit l_parent)
        {
            InitializeComponent();
            m_parent = l_parent;
        }
        public void Loading_setLabel(string fileInfo)
        {
            if (lblStatus.InvokeRequired)
            {
                try
                {
                    lblStatus.Invoke((MethodInvoker)delegate
                    {
                        lblStatus.Text = fileInfo;
                        lblStatus.Invalidate();
                        lblStatus.Update();
                        lblStatus.Refresh();
                        Application.DoEvents();
                    });
                }
                catch
                {

                }

            }

            else
            {

                lblStatus.Text = fileInfo;

            }
        }
        public void Loading_setProgressValue()
        {

            if (openProjectProgress.InvokeRequired)
            {
                try
                {
                    openProjectProgress.Invoke((MethodInvoker)delegate
                    {

                        if (openProjectProgress.Value >= openProjectProgress.Maximum - 1)
                        {
                            this.FormClose();
                        }
                        else
                        {
                            openProjectProgress.Value++;
                        }
                        openProjectProgress.Invalidate();
                        openProjectProgress.Update();
                        openProjectProgress.Refresh();
                        Application.DoEvents();
                    });
                }
                catch
                {

                }

            }

            else
            {



                if (openProjectProgress.Value >= openProjectProgress.Maximum - 1)
                {
                    this.FormClose();
                }
                else
                {
                    openProjectProgress.Value++;
                }
                

            }

        }
        public void Loading_setProgressMax(int fileCount)
        {

            if (openProjectProgress.InvokeRequired)
            {
                try
                {
                    openProjectProgress.Invoke((MethodInvoker)delegate
                    {
                        openProjectProgress.Maximum = fileCount;
                        openProjectProgress.Minimum = 0;
                        openProjectProgress.Value = 0;
                        openProjectProgress.Invalidate();
                        openProjectProgress.Update();
                        openProjectProgress.Refresh();
                        Application.DoEvents();
                    });
                }
                catch
                {

                }

            }

            else
            {

                openProjectProgress.Maximum = fileCount;
                openProjectProgress.Minimum = 0;
                openProjectProgress.Value = 0;
            }

        }
        private void Loading_UpdateFileInfo(string fileName)
        {
            Loading_setProgressValue();
            if (File.Exists(fileName))
            {
                Loading_setLabel(Path.GetFileName(fileName) + " Processing Complete");
            }
            else
            {
                Loading_setLabel(fileName);
            }
        }
        private void Loading_Load(object sender, EventArgs e)
        {
            m_parent.Enabled = false;
            m_parent.evProcessComplete += (FormClose);
            m_parent.evCloseAllForms += (FormClose);
            m_parent.evNewStatus += (Loading_setLabel);
            m_parent.evfileOpenComplete += Loading_UpdateFileInfo;
            Application.DoEvents();
        }
        private void Loading_CloseEvents()
        {
            m_parent.evProcessComplete -= (FormClose);
            m_parent.evCloseAllForms -= (FormClose);
            m_parent.evNewStatus -= (Loading_setLabel);
            m_parent.evfileOpenComplete -= Loading_UpdateFileInfo;
        }
        private void FormClose()
        {
           
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Loading_CloseEvents();
                        this.m_parent.Enabled = true;
                        this.m_parent.Enabled = true;
                        this.Close();
                        Application.DoEvents();
                    });
                }
                catch
                {

                }

            }

            else
            {

                this.Loading_CloseEvents();
                this.m_parent.Enabled = true;
                this.m_parent.Enabled = true;
                this.Close();
            }
        }

        private void Loading_FormExit(object sender, FormClosedEventArgs e)
        {
            this.FormClose();
        }
    }
}
