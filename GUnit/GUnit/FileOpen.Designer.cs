namespace GUnit
{
    partial class FileOpen
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
            this.scintilla = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // scintilla
            // 
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.IndentationGuides = ScintillaNET.IndentView.Real;
            this.scintilla.IndentWidth = 2;
            this.scintilla.Lexer = ScintillaNET.Lexer.Cpp;
            this.scintilla.Location = new System.Drawing.Point(0, 0);
            this.scintilla.Margins.Left = 10;
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(983, 485);
            this.scintilla.TabIndex = 0;
            this.scintilla.MarginClick += new System.EventHandler<ScintillaNET.MarginClickEventArgs>(this.scintilla_MarginClick);
            this.scintilla.UpdateUI += new System.EventHandler<ScintillaNET.UpdateUIEventArgs>(this.scintilla_UpdateUI);
            this.scintilla.TextChanged += new System.EventHandler(this.FileOpen_TextChanged);
            // 
            // FileOpen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 485);
            this.Controls.Add(this.scintilla);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FileOpen";
            this.Text = "Untitled";
            this.Activated += new System.EventHandler(this.FileOpen_DocumentActive);
            this.Load += new System.EventHandler(this.FileOpen_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ScintillaNET.Scintilla scintilla;
    }
}