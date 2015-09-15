namespace Gunit.Ui
{
    partial class Document
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.documentContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.definitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.declairationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scintilla = new ScintillaNET.Scintilla();
            this.documentContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // documentContext
            // 
            this.documentContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToToolStripMenuItem});
            this.documentContext.Name = "documentContext";
            this.documentContext.Size = new System.Drawing.Size(116, 28);
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.definitionToolStripMenuItem,
            this.declairationToolStripMenuItem});
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(115, 24);
            this.goToToolStripMenuItem.Text = "Go to";
            // 
            // definitionToolStripMenuItem
            // 
            this.definitionToolStripMenuItem.Name = "definitionToolStripMenuItem";
            this.definitionToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.definitionToolStripMenuItem.Text = "Definition";
            this.definitionToolStripMenuItem.Click += new System.EventHandler(this.definitionToolStripMenuItem_Click);
            // 
            // declairationToolStripMenuItem
            // 
            this.declairationToolStripMenuItem.Name = "declairationToolStripMenuItem";
            this.declairationToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.declairationToolStripMenuItem.Text = "Declairation";
            // 
            // scintilla
            // 
            this.scintilla.ContextMenuStrip = this.documentContext;
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.IndentationGuides = ScintillaNET.IndentView.LookForward;
            this.scintilla.IndentWidth = 2;
            this.scintilla.Lexer = ScintillaNET.Lexer.Cpp;
            this.scintilla.Location = new System.Drawing.Point(0, 0);
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(687, 404);
            this.scintilla.TabIndex = 0;
            this.scintilla.Text = "scintilla1";
            this.scintilla.CharAdded += new System.EventHandler<ScintillaNET.CharAddedEventArgs>(this.scintilla_CharAdded);
            this.scintilla.Painted += new System.EventHandler<System.EventArgs>(this.Document_painted);
            this.scintilla.UpdateUI += new System.EventHandler<ScintillaNET.UpdateUIEventArgs>(this.Document_UIupdate);
            this.scintilla.TextChanged += new System.EventHandler(this.Document_TextChanged);
            this.scintilla.Click += new System.EventHandler(this.Document_Click);
            this.scintilla.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Scintilla_KeyDown);
            // 
            // Document
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 404);
            this.CloseButtonVisible = false;
            this.Controls.Add(this.scintilla);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Name = "Document";
            this.Text = "Document";
            this.Activated += new System.EventHandler(this.Document_activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Document_CloseForm);
            this.Load += new System.EventHandler(this.Document_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Document_KeyDown);
            this.documentContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ScintillaNET.Scintilla scintilla;
        private System.Windows.Forms.ContextMenuStrip documentContext;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem definitionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem declairationToolStripMenuItem;




    }
}