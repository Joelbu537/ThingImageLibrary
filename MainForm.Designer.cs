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
            this.SuspendLayout();
            // 
            // listViewLibrary
            // 
            this.listViewLibrary.HideSelection = false;
            this.listViewLibrary.Location = new System.Drawing.Point(8, 8);
            this.listViewLibrary.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listViewLibrary.Name = "listViewLibrary";
            this.listViewLibrary.Size = new System.Drawing.Size(641, 530);
            this.listViewLibrary.TabIndex = 0;
            this.listViewLibrary.UseCompatibleStateImageBehavior = false;
            this.listViewLibrary.SelectedIndexChanged += new System.EventHandler(this.listViewLibrary_SelectedIndexChanged);
            // 
            // buttonLoadLibrary
            // 
            this.buttonLoadLibrary.Enabled = false;
            this.buttonLoadLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonLoadLibrary.Location = new System.Drawing.Point(657, 8);
            this.buttonLoadLibrary.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonLoadLibrary.Name = "buttonLoadLibrary";
            this.buttonLoadLibrary.Size = new System.Drawing.Size(167, 25);
            this.buttonLoadLibrary.TabIndex = 1;
            this.buttonLoadLibrary.Text = "Load Library";
            this.buttonLoadLibrary.UseVisualStyleBackColor = true;
            // 
            // buttonLoadKey
            // 
            this.buttonLoadKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonLoadKey.Location = new System.Drawing.Point(657, 509);
            this.buttonLoadKey.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLoadKey.Name = "buttonLoadKey";
            this.buttonLoadKey.Size = new System.Drawing.Size(167, 25);
            this.buttonLoadKey.TabIndex = 2;
            this.buttonLoadKey.Text = "Load Existing Key";
            this.buttonLoadKey.UseVisualStyleBackColor = true;
            this.buttonLoadKey.Click += new System.EventHandler(this.buttonLoadKey_Click);
            // 
            // buttonGenerateNewKey
            // 
            this.buttonGenerateNewKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonGenerateNewKey.Location = new System.Drawing.Point(657, 480);
            this.buttonGenerateNewKey.Margin = new System.Windows.Forms.Padding(2);
            this.buttonGenerateNewKey.Name = "buttonGenerateNewKey";
            this.buttonGenerateNewKey.Size = new System.Drawing.Size(167, 25);
            this.buttonGenerateNewKey.TabIndex = 3;
            this.buttonGenerateNewKey.Text = "Create New Key";
            this.buttonGenerateNewKey.UseVisualStyleBackColor = true;
            this.buttonGenerateNewKey.Click += new System.EventHandler(this.buttonGenerateNewKey_Click);
            // 
            // labelKeyStatusDecorative
            // 
            this.labelKeyStatusDecorative.AutoSize = true;
            this.labelKeyStatusDecorative.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKeyStatusDecorative.Location = new System.Drawing.Point(660, 458);
            this.labelKeyStatusDecorative.Name = "labelKeyStatusDecorative";
            this.labelKeyStatusDecorative.Size = new System.Drawing.Size(90, 20);
            this.labelKeyStatusDecorative.TabIndex = 4;
            this.labelKeyStatusDecorative.Text = "Key Status:";
            // 
            // labelKeyStatus
            // 
            this.labelKeyStatus.AutoSize = true;
            this.labelKeyStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKeyStatus.ForeColor = System.Drawing.Color.Red;
            this.labelKeyStatus.Location = new System.Drawing.Point(756, 458);
            this.labelKeyStatus.Name = "labelKeyStatus";
            this.labelKeyStatus.Size = new System.Drawing.Size(58, 20);
            this.labelKeyStatus.TabIndex = 5;
            this.labelKeyStatus.Text = "NONE";
            this.labelKeyStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 545);
            this.Controls.Add(this.labelKeyStatus);
            this.Controls.Add(this.labelKeyStatusDecorative);
            this.Controls.Add(this.buttonGenerateNewKey);
            this.Controls.Add(this.buttonLoadKey);
            this.Controls.Add(this.buttonLoadLibrary);
            this.Controls.Add(this.listViewLibrary);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
    }
}

