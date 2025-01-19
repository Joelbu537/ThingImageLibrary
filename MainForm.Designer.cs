namespace ThingImageLibrary
{
    partial class MainForm
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
            this.listViewLibrary = new System.Windows.Forms.ListView();
            this.buttonLoadLibrary = new System.Windows.Forms.Button();
            this.buttonLoadKey = new System.Windows.Forms.Button();
            this.buttonGenerateNewKey = new System.Windows.Forms.Button();
            this.labelKeyStatusDecorative = new System.Windows.Forms.Label();
            this.labelKeyStatus = new System.Windows.Forms.Label();
            this.buttonCreateLibrary = new System.Windows.Forms.Button();
            this.buttonCreateDirectory = new System.Windows.Forms.Button();
            this.labelSectionDirectory = new System.Windows.Forms.Label();
            this.buttonAlterExistingDirectory = new System.Windows.Forms.Button();
            this.buttonDeleteExistingDirectory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewLibrary
            // 
            this.listViewLibrary.HideSelection = false;
            this.listViewLibrary.Location = new System.Drawing.Point(12, 12);
            this.listViewLibrary.Name = "listViewLibrary";
            this.listViewLibrary.Size = new System.Drawing.Size(960, 813);
            this.listViewLibrary.TabIndex = 0;
            this.listViewLibrary.UseCompatibleStateImageBehavior = false;
            this.listViewLibrary.SelectedIndexChanged += new System.EventHandler(this.listViewLibrary_SelectedIndexChanged);
            // 
            // buttonLoadLibrary
            // 
            this.buttonLoadLibrary.Enabled = false;
            this.buttonLoadLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonLoadLibrary.Location = new System.Drawing.Point(986, 12);
            this.buttonLoadLibrary.Name = "buttonLoadLibrary";
            this.buttonLoadLibrary.Size = new System.Drawing.Size(280, 38);
            this.buttonLoadLibrary.TabIndex = 1;
            this.buttonLoadLibrary.Text = "Load Library";
            this.buttonLoadLibrary.UseVisualStyleBackColor = true;
            this.buttonLoadLibrary.Click += new System.EventHandler(this.buttonLoadLibrary_Click);
            // 
            // buttonLoadKey
            // 
            this.buttonLoadKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonLoadKey.Location = new System.Drawing.Point(986, 783);
            this.buttonLoadKey.Name = "buttonLoadKey";
            this.buttonLoadKey.Size = new System.Drawing.Size(280, 38);
            this.buttonLoadKey.TabIndex = 2;
            this.buttonLoadKey.Text = "Load Existing Key";
            this.buttonLoadKey.UseVisualStyleBackColor = true;
            this.buttonLoadKey.Click += new System.EventHandler(this.buttonLoadKey_Click);
            // 
            // buttonGenerateNewKey
            // 
            this.buttonGenerateNewKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonGenerateNewKey.Location = new System.Drawing.Point(986, 738);
            this.buttonGenerateNewKey.Name = "buttonGenerateNewKey";
            this.buttonGenerateNewKey.Size = new System.Drawing.Size(280, 38);
            this.buttonGenerateNewKey.TabIndex = 3;
            this.buttonGenerateNewKey.Text = "Create New Key";
            this.buttonGenerateNewKey.UseVisualStyleBackColor = true;
            this.buttonGenerateNewKey.Click += new System.EventHandler(this.buttonGenerateNewKey_Click);
            // 
            // labelKeyStatusDecorative
            // 
            this.labelKeyStatusDecorative.AutoSize = true;
            this.labelKeyStatusDecorative.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKeyStatusDecorative.Location = new System.Drawing.Point(990, 705);
            this.labelKeyStatusDecorative.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelKeyStatusDecorative.Name = "labelKeyStatusDecorative";
            this.labelKeyStatusDecorative.Size = new System.Drawing.Size(132, 29);
            this.labelKeyStatusDecorative.TabIndex = 4;
            this.labelKeyStatusDecorative.Text = "Key Status:";
            // 
            // labelKeyStatus
            // 
            this.labelKeyStatus.AutoSize = true;
            this.labelKeyStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKeyStatus.ForeColor = System.Drawing.Color.Red;
            this.labelKeyStatus.Location = new System.Drawing.Point(1134, 705);
            this.labelKeyStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelKeyStatus.Name = "labelKeyStatus";
            this.labelKeyStatus.Size = new System.Drawing.Size(88, 29);
            this.labelKeyStatus.TabIndex = 5;
            this.labelKeyStatus.Text = "NONE";
            this.labelKeyStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonCreateLibrary
            // 
            this.buttonCreateLibrary.Enabled = false;
            this.buttonCreateLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonCreateLibrary.Location = new System.Drawing.Point(986, 56);
            this.buttonCreateLibrary.Name = "buttonCreateLibrary";
            this.buttonCreateLibrary.Size = new System.Drawing.Size(280, 38);
            this.buttonCreateLibrary.TabIndex = 6;
            this.buttonCreateLibrary.Text = "Create New Library";
            this.buttonCreateLibrary.UseVisualStyleBackColor = true;
            // 
            // buttonCreateDirectory
            // 
            this.buttonCreateDirectory.Enabled = false;
            this.buttonCreateDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonCreateDirectory.Location = new System.Drawing.Point(986, 165);
            this.buttonCreateDirectory.Name = "buttonCreateDirectory";
            this.buttonCreateDirectory.Size = new System.Drawing.Size(280, 38);
            this.buttonCreateDirectory.TabIndex = 7;
            this.buttonCreateDirectory.Text = "Create New Directory";
            this.buttonCreateDirectory.UseVisualStyleBackColor = true;
            // 
            // labelSectionDirectory
            // 
            this.labelSectionDirectory.AutoSize = true;
            this.labelSectionDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSectionDirectory.Location = new System.Drawing.Point(1010, 125);
            this.labelSectionDirectory.Name = "labelSectionDirectory";
            this.labelSectionDirectory.Size = new System.Drawing.Size(228, 29);
            this.labelSectionDirectory.TabIndex = 8;
            this.labelSectionDirectory.Text = "Library Directories";
            // 
            // buttonAlterExistingDirectory
            // 
            this.buttonAlterExistingDirectory.Enabled = false;
            this.buttonAlterExistingDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonAlterExistingDirectory.Location = new System.Drawing.Point(986, 209);
            this.buttonAlterExistingDirectory.Name = "buttonAlterExistingDirectory";
            this.buttonAlterExistingDirectory.Size = new System.Drawing.Size(280, 38);
            this.buttonAlterExistingDirectory.TabIndex = 9;
            this.buttonAlterExistingDirectory.Text = "Edit Existing Directory";
            this.buttonAlterExistingDirectory.UseVisualStyleBackColor = true;
            // 
            // buttonDeleteExistingDirectory
            // 
            this.buttonDeleteExistingDirectory.Enabled = false;
            this.buttonDeleteExistingDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonDeleteExistingDirectory.Location = new System.Drawing.Point(986, 253);
            this.buttonDeleteExistingDirectory.Name = "buttonDeleteExistingDirectory";
            this.buttonDeleteExistingDirectory.Size = new System.Drawing.Size(280, 38);
            this.buttonDeleteExistingDirectory.TabIndex = 10;
            this.buttonDeleteExistingDirectory.Text = "Delete Existing Directory";
            this.buttonDeleteExistingDirectory.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 844);
            this.Controls.Add(this.buttonDeleteExistingDirectory);
            this.Controls.Add(this.buttonAlterExistingDirectory);
            this.Controls.Add(this.labelSectionDirectory);
            this.Controls.Add(this.buttonCreateDirectory);
            this.Controls.Add(this.buttonCreateLibrary);
            this.Controls.Add(this.labelKeyStatus);
            this.Controls.Add(this.labelKeyStatusDecorative);
            this.Controls.Add(this.buttonGenerateNewKey);
            this.Controls.Add(this.buttonLoadKey);
            this.Controls.Add(this.buttonLoadLibrary);
            this.Controls.Add(this.listViewLibrary);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thing Image Library";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewLibrary;
        private System.Windows.Forms.Button buttonLoadLibrary;
        private System.Windows.Forms.Button buttonLoadKey;
        private System.Windows.Forms.Button buttonGenerateNewKey;
        private System.Windows.Forms.Label labelKeyStatusDecorative;
        private System.Windows.Forms.Label labelKeyStatus;
        private System.Windows.Forms.Button buttonCreateLibrary;
        private System.Windows.Forms.Button buttonCreateDirectory;
        private System.Windows.Forms.Label labelSectionDirectory;
        private System.Windows.Forms.Button buttonAlterExistingDirectory;
        private System.Windows.Forms.Button buttonDeleteExistingDirectory;
    }
}

