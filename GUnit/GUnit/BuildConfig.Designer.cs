namespace GUnit
{
    partial class BuildConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBinPath = new System.Windows.Forms.TextBox();
            this.btnBin = new System.Windows.Forms.Button();
            this.dtGridInclude = new System.Windows.Forms.DataGridView();
            this.dtLib = new System.Windows.Forms.DataGridView();
            this.dtLibNames = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridInclude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtLib)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtLibNames)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Include Paths";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Libray Paths";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 331);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Target Path";
            this.label4.Visible = false;
            // 
            // txtBinPath
            // 
            this.txtBinPath.Enabled = false;
            this.txtBinPath.Location = new System.Drawing.Point(153, 331);
            this.txtBinPath.Name = "txtBinPath";
            this.txtBinPath.Size = new System.Drawing.Size(411, 22);
            this.txtBinPath.TabIndex = 10;
            this.txtBinPath.Visible = false;
            // 
            // btnBin
            // 
            this.btnBin.Location = new System.Drawing.Point(570, 331);
            this.btnBin.Name = "btnBin";
            this.btnBin.Size = new System.Drawing.Size(75, 23);
            this.btnBin.TabIndex = 11;
            this.btnBin.Text = "Browse";
            this.btnBin.UseVisualStyleBackColor = true;
            this.btnBin.Visible = false;
            this.btnBin.Click += new System.EventHandler(this.btnBin_Click);
            // 
            // dtGridInclude
            // 
            this.dtGridInclude.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridInclude.Location = new System.Drawing.Point(153, 55);
            this.dtGridInclude.Name = "dtGridInclude";
            this.dtGridInclude.RowTemplate.Height = 24;
            this.dtGridInclude.Size = new System.Drawing.Size(492, 82);
            this.dtGridInclude.TabIndex = 13;
            this.dtGridInclude.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.BuildCfg_IncludeCellClick);
            this.dtGridInclude.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGridInclude_CellContentClick);
            this.dtGridInclude.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellValueChange);
            // 
            // dtLib
            // 
            this.dtLib.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtLib.Location = new System.Drawing.Point(153, 143);
            this.dtLib.Name = "dtLib";
            this.dtLib.RowTemplate.Height = 24;
            this.dtLib.Size = new System.Drawing.Size(492, 88);
            this.dtLib.TabIndex = 14;
            this.dtLib.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.BldCfg_LibAdded);
            // 
            // dtLibNames
            // 
            this.dtLibNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtLibNames.Location = new System.Drawing.Point(153, 237);
            this.dtLibNames.Name = "dtLibNames";
            this.dtLibNames.RowTemplate.Height = 24;
            this.dtLibNames.Size = new System.Drawing.Size(492, 88);
            this.dtLibNames.TabIndex = 14;
            this.dtLibNames.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.BldCfg_LibNameChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 265);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Libray Names";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(544, 360);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(101, 30);
            this.btnOk.TabIndex = 15;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // BuildConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 450);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dtLibNames);
            this.Controls.Add(this.dtLib);
            this.Controls.Add(this.dtGridInclude);
            this.Controls.Add(this.btnBin);
            this.Controls.Add(this.txtBinPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "BuildConfig";
            this.Text = "Build Configuration";
            this.Load += new System.EventHandler(this.BuildConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridInclude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtLib)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtLibNames)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBinPath;
        private System.Windows.Forms.Button btnBin;
        private System.Windows.Forms.DataGridView dtGridInclude;
        private System.Windows.Forms.DataGridView dtLib;
        private System.Windows.Forms.DataGridView dtLibNames;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
    }
}