namespace GUnit
{
    partial class AddNewFunction
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
            this.btnSubmit = new System.Windows.Forms.Button();
            this.comboAccess = new System.Windows.Forms.ComboBox();
            this.comboIsVirtual = new System.Windows.Forms.ComboBox();
            this.txtReturnValue = new System.Windows.Forms.TextBox();
            this.txtxFunctionName = new System.Windows.Forms.TextBox();
            this.lblVirtual = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAccess = new System.Windows.Forms.Label();
            this.dtArguments = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.comboUnits = new System.Windows.Forms.ComboBox();
            this.btnAddUnit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtArguments)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.Enabled = false;
            this.btnSubmit.Location = new System.Drawing.Point(384, 283);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(148, 23);
            this.btnSubmit.TabIndex = 29;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // comboAccess
            // 
            this.comboAccess.FormattingEnabled = true;
            this.comboAccess.Items.AddRange(new object[] {
            "public",
            "private"});
            this.comboAccess.Location = new System.Drawing.Point(161, 64);
            this.comboAccess.Name = "comboAccess";
            this.comboAccess.Size = new System.Drawing.Size(183, 24);
            this.comboAccess.TabIndex = 26;
            // 
            // comboIsVirtual
            // 
            this.comboIsVirtual.FormattingEnabled = true;
            this.comboIsVirtual.Items.AddRange(new object[] {
            "YES",
            "NO"});
            this.comboIsVirtual.Location = new System.Drawing.Point(161, 283);
            this.comboIsVirtual.Name = "comboIsVirtual";
            this.comboIsVirtual.Size = new System.Drawing.Size(183, 24);
            this.comboIsVirtual.TabIndex = 27;
            // 
            // txtReturnValue
            // 
            this.txtReturnValue.Location = new System.Drawing.Point(161, 157);
            this.txtReturnValue.Name = "txtReturnValue";
            this.txtReturnValue.Size = new System.Drawing.Size(183, 22);
            this.txtReturnValue.TabIndex = 23;
            // 
            // txtxFunctionName
            // 
            this.txtxFunctionName.Location = new System.Drawing.Point(161, 113);
            this.txtxFunctionName.Name = "txtxFunctionName";
            this.txtxFunctionName.Size = new System.Drawing.Size(183, 22);
            this.txtxFunctionName.TabIndex = 25;
            // 
            // lblVirtual
            // 
            this.lblVirtual.AutoSize = true;
            this.lblVirtual.Location = new System.Drawing.Point(24, 286);
            this.lblVirtual.Name = "lblVirtual";
            this.lblVirtual.Size = new System.Drawing.Size(106, 17);
            this.lblVirtual.TabIndex = 18;
            this.lblVirtual.Text = "Virtual Function";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "Arguments";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 17);
            this.label5.TabIndex = 16;
            this.label5.Text = "Return Value";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Function Name";
            // 
            // lblAccess
            // 
            this.lblAccess.AutoSize = true;
            this.lblAccess.Location = new System.Drawing.Point(27, 67);
            this.lblAccess.Name = "lblAccess";
            this.lblAccess.Size = new System.Drawing.Size(97, 17);
            this.lblAccess.TabIndex = 14;
            this.lblAccess.Text = "Access Scope";
            // 
            // dtArguments
            // 
            this.dtArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtArguments.Location = new System.Drawing.Point(161, 196);
            this.dtArguments.Name = "dtArguments";
            this.dtArguments.RowTemplate.Height = 24;
            this.dtArguments.Size = new System.Drawing.Size(183, 67);
            this.dtArguments.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "Unit Name";
            // 
            // comboUnits
            // 
            this.comboUnits.FormattingEnabled = true;
            this.comboUnits.Items.AddRange(new object[] {
            "Class Member (C++ style)",
            "Gloabl Function(C Style)"});
            this.comboUnits.Location = new System.Drawing.Point(161, 16);
            this.comboUnits.Name = "comboUnits";
            this.comboUnits.Size = new System.Drawing.Size(183, 24);
            this.comboUnits.TabIndex = 26;
            this.comboUnits.SelectedIndexChanged += new System.EventHandler(this.comboUnits_SelectedIndexChanged);
            // 
            // btnAddUnit
            // 
            this.btnAddUnit.Location = new System.Drawing.Point(384, 12);
            this.btnAddUnit.Name = "btnAddUnit";
            this.btnAddUnit.Size = new System.Drawing.Size(148, 23);
            this.btnAddUnit.TabIndex = 31;
            this.btnAddUnit.Text = "Add New Unit";
            this.btnAddUnit.UseVisualStyleBackColor = true;
            this.btnAddUnit.Click += new System.EventHandler(this.btnAddUnit_Click);
            // 
            // AddNewFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 378);
            this.Controls.Add(this.btnAddUnit);
            this.Controls.Add(this.dtArguments);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.comboUnits);
            this.Controls.Add(this.comboAccess);
            this.Controls.Add(this.comboIsVirtual);
            this.Controls.Add(this.txtReturnValue);
            this.Controls.Add(this.txtxFunctionName);
            this.Controls.Add(this.lblVirtual);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblAccess);
            this.Controls.Add(this.label1);
            this.Name = "AddNewFunction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddNewFunction";
            this.Load += new System.EventHandler(this.AddNewFunction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtArguments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.ComboBox comboAccess;
        private System.Windows.Forms.ComboBox comboIsVirtual;
        private System.Windows.Forms.TextBox txtReturnValue;
        private System.Windows.Forms.TextBox txtxFunctionName;
        private System.Windows.Forms.Label lblVirtual;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblAccess;
        private System.Windows.Forms.DataGridView dtArguments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboUnits;
        private System.Windows.Forms.Button btnAddUnit;

    }
}