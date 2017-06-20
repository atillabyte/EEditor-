namespace EEditor
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.NewestLabel = new System.Windows.Forms.Label();
            this.UpdaterButton = new System.Windows.Forms.Button();
            this.ChangelogRichTextBox = new System.Windows.Forms.RichTextBox();
            this.UsingLabel = new System.Windows.Forms.Label();
            this.BugsOrFeatureButton = new System.Windows.Forms.Button();
            this.WikiButton = new System.Windows.Forms.Button();
            this.HomepageButton = new System.Windows.Forms.Button();
            this.ForumButton = new System.Windows.Forms.Button();
            this.AboutLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.DownloadButton);
            this.groupBox1.Controls.Add(this.NewestLabel);
            this.groupBox1.Controls.Add(this.UpdaterButton);
            this.groupBox1.Controls.Add(this.ChangelogRichTextBox);
            this.groupBox1.Controls.Add(this.UsingLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 214);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Updates and changelog";
            // 
            // DownloadButton
            // 
            this.DownloadButton.Location = new System.Drawing.Point(353, 172);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(97, 23);
            this.DownloadButton.TabIndex = 3;
            this.DownloadButton.Text = "Download latest";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.Download_Click);
            // 
            // NewestLabel
            // 
            this.NewestLabel.AutoSize = true;
            this.NewestLabel.Location = new System.Drawing.Point(94, 177);
            this.NewestLabel.Name = "NewestLabel";
            this.NewestLabel.Size = new System.Drawing.Size(49, 13);
            this.NewestLabel.TabIndex = 6;
            this.NewestLabel.Text = "Newest: ";
            // 
            // UpdaterButton
            // 
            this.UpdaterButton.Location = new System.Drawing.Point(234, 172);
            this.UpdaterButton.Name = "UpdaterButton";
            this.UpdaterButton.Size = new System.Drawing.Size(113, 23);
            this.UpdaterButton.TabIndex = 2;
            this.UpdaterButton.Text = "Check for updates";
            this.UpdaterButton.UseVisualStyleBackColor = true;
            this.UpdaterButton.Click += new System.EventHandler(this.Updater_Click);
            // 
            // ChangelogRichTextBox
            // 
            this.ChangelogRichTextBox.Location = new System.Drawing.Point(9, 19);
            this.ChangelogRichTextBox.Name = "ChangelogRichTextBox";
            this.ChangelogRichTextBox.ReadOnly = true;
            this.ChangelogRichTextBox.Size = new System.Drawing.Size(441, 147);
            this.ChangelogRichTextBox.TabIndex = 1;
            this.ChangelogRichTextBox.Text = "";
            // 
            // UsingLabel
            // 
            this.UsingLabel.AutoSize = true;
            this.UsingLabel.Location = new System.Drawing.Point(6, 177);
            this.UsingLabel.Name = "UsingLabel";
            this.UsingLabel.Size = new System.Drawing.Size(40, 13);
            this.UsingLabel.TabIndex = 5;
            this.UsingLabel.Text = "Using: ";
            // 
            // BugsOrFeatureButton
            // 
            this.BugsOrFeatureButton.Location = new System.Drawing.Point(353, 74);
            this.BugsOrFeatureButton.Name = "BugsOrFeatureButton";
            this.BugsOrFeatureButton.Size = new System.Drawing.Size(97, 23);
            this.BugsOrFeatureButton.TabIndex = 3;
            this.BugsOrFeatureButton.Text = "Bugs/features";
            this.BugsOrFeatureButton.UseVisualStyleBackColor = true;
            this.BugsOrFeatureButton.Click += new System.EventHandler(this.BugsOrFeature_Click);
            // 
            // WikiButton
            // 
            this.WikiButton.Location = new System.Drawing.Point(353, 45);
            this.WikiButton.Name = "WikiButton";
            this.WikiButton.Size = new System.Drawing.Size(97, 23);
            this.WikiButton.TabIndex = 2;
            this.WikiButton.Text = "Wiki";
            this.WikiButton.UseVisualStyleBackColor = true;
            this.WikiButton.Click += new System.EventHandler(this.Wiki_Click);
            // 
            // HomepageButton
            // 
            this.HomepageButton.Location = new System.Drawing.Point(353, 103);
            this.HomepageButton.Name = "HomepageButton";
            this.HomepageButton.Size = new System.Drawing.Size(97, 23);
            this.HomepageButton.TabIndex = 1;
            this.HomepageButton.Text = "Homepage";
            this.HomepageButton.UseVisualStyleBackColor = true;
            this.HomepageButton.Click += new System.EventHandler(this.HomePage_Click);
            // 
            // ForumButton
            // 
            this.ForumButton.Location = new System.Drawing.Point(353, 16);
            this.ForumButton.Name = "ForumButton";
            this.ForumButton.Size = new System.Drawing.Size(97, 23);
            this.ForumButton.TabIndex = 0;
            this.ForumButton.Text = "Forum topic";
            this.ForumButton.UseVisualStyleBackColor = true;
            this.ForumButton.Click += new System.EventHandler(this.Forum_Click);
            // 
            // AboutLabel
            // 
            this.AboutLabel.Location = new System.Drawing.Point(7, 16);
            this.AboutLabel.Name = "AboutLabel";
            this.AboutLabel.Size = new System.Drawing.Size(340, 116);
            this.AboutLabel.TabIndex = 0;
            this.AboutLabel.Text = resources.GetString("AboutLabel.Text");
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.BugsOrFeatureButton);
            this.groupBox4.Controls.Add(this.AboutLabel);
            this.groupBox4.Controls.Add(this.WikiButton);
            this.groupBox4.Controls.Add(this.ForumButton);
            this.groupBox4.Controls.Add(this.HomepageButton);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(458, 135);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "General info";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 374);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About EEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.About_FormClosed);
            this.Load += new System.EventHandler(this.About_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox ChangelogRichTextBox;
        private System.Windows.Forms.Button ForumButton;
        private System.Windows.Forms.Button DownloadButton;
        private System.Windows.Forms.Button UpdaterButton;
        private System.Windows.Forms.Label NewestLabel;
        private System.Windows.Forms.Label UsingLabel;
        private System.Windows.Forms.Button WikiButton;
        private System.Windows.Forms.Label AboutLabel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button BugsOrFeatureButton;
        private System.Windows.Forms.Button HomepageButton;
    }
}