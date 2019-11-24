namespace PadoruManager.UI
{
    partial class ManagerConfigSetupWizard
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
            System.Windows.Forms.Label lbGithubUser;
            System.Windows.Forms.Label lbGithubRepo;
            System.Windows.Forms.Label lbToCRoot;
            System.Windows.Forms.GroupBox groupDetails;
            System.Windows.Forms.Label lbRepoRootRaw;
            System.Windows.Forms.Label lbRepoRoot;
            System.Windows.Forms.Button btnSave;
            this.txtRepoRootRaw = new System.Windows.Forms.TextBox();
            this.txtRepoRoot = new System.Windows.Forms.TextBox();
            this.txtGithubUser = new System.Windows.Forms.TextBox();
            this.txtGithubRepo = new System.Windows.Forms.TextBox();
            this.txtToCRoot = new System.Windows.Forms.TextBox();
            lbGithubUser = new System.Windows.Forms.Label();
            lbGithubRepo = new System.Windows.Forms.Label();
            lbToCRoot = new System.Windows.Forms.Label();
            groupDetails = new System.Windows.Forms.GroupBox();
            lbRepoRootRaw = new System.Windows.Forms.Label();
            lbRepoRoot = new System.Windows.Forms.Label();
            btnSave = new System.Windows.Forms.Button();
            groupDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbGithubUser
            // 
            lbGithubUser.AutoSize = true;
            lbGithubUser.Location = new System.Drawing.Point(21, 15);
            lbGithubUser.Name = "lbGithubUser";
            lbGithubUser.Size = new System.Drawing.Size(117, 13);
            lbGithubUser.TabIndex = 0;
            lbGithubUser.Text = "Your Github Username:";
            // 
            // lbGithubRepo
            // 
            lbGithubRepo.AutoSize = true;
            lbGithubRepo.Location = new System.Drawing.Point(12, 41);
            lbGithubRepo.Name = "lbGithubRepo";
            lbGithubRepo.Size = new System.Drawing.Size(126, 13);
            lbGithubRepo.TabIndex = 2;
            lbGithubRepo.Text = "Your Github Repo Name:";
            // 
            // lbToCRoot
            // 
            lbToCRoot.AutoSize = true;
            lbToCRoot.Location = new System.Drawing.Point(18, 67);
            lbToCRoot.Name = "lbToCRoot";
            lbToCRoot.Size = new System.Drawing.Size(120, 13);
            lbToCRoot.TabIndex = 4;
            lbToCRoot.Text = "Table of Contents Root:";
            // 
            // groupDetails
            // 
            groupDetails.Controls.Add(this.txtRepoRootRaw);
            groupDetails.Controls.Add(lbRepoRootRaw);
            groupDetails.Controls.Add(this.txtRepoRoot);
            groupDetails.Controls.Add(lbRepoRoot);
            groupDetails.Location = new System.Drawing.Point(12, 90);
            groupDetails.Name = "groupDetails";
            groupDetails.Size = new System.Drawing.Size(501, 79);
            groupDetails.TabIndex = 6;
            groupDetails.TabStop = false;
            groupDetails.Text = "Details";
            // 
            // txtRepoRootRaw
            // 
            this.txtRepoRootRaw.Location = new System.Drawing.Point(132, 45);
            this.txtRepoRootRaw.MaxLength = 100;
            this.txtRepoRootRaw.Name = "txtRepoRootRaw";
            this.txtRepoRootRaw.Size = new System.Drawing.Size(356, 20);
            this.txtRepoRootRaw.TabIndex = 10;
            // 
            // lbRepoRootRaw
            // 
            lbRepoRootRaw.AutoSize = true;
            lbRepoRootRaw.Location = new System.Drawing.Point(14, 48);
            lbRepoRootRaw.Name = "lbRepoRootRaw";
            lbRepoRootRaw.Size = new System.Drawing.Size(112, 13);
            lbRepoRootRaw.TabIndex = 9;
            lbRepoRootRaw.Text = "Raw Repo Root URL:";
            // 
            // txtRepoRoot
            // 
            this.txtRepoRoot.Location = new System.Drawing.Point(132, 19);
            this.txtRepoRoot.MaxLength = 100;
            this.txtRepoRoot.Name = "txtRepoRoot";
            this.txtRepoRoot.Size = new System.Drawing.Size(356, 20);
            this.txtRepoRoot.TabIndex = 8;
            // 
            // lbRepoRoot
            // 
            lbRepoRoot.AutoSize = true;
            lbRepoRoot.Location = new System.Drawing.Point(39, 22);
            lbRepoRoot.Name = "lbRepoRoot";
            lbRepoRoot.Size = new System.Drawing.Size(87, 13);
            lbRepoRoot.TabIndex = 7;
            lbRepoRoot.Text = "Repo Root URL:";
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(374, 175);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(139, 23);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save Config";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new System.EventHandler(this.OnSaveClick);
            // 
            // txtGithubUser
            // 
            this.txtGithubUser.Location = new System.Drawing.Point(144, 12);
            this.txtGithubUser.MaxLength = 100;
            this.txtGithubUser.Name = "txtGithubUser";
            this.txtGithubUser.Size = new System.Drawing.Size(162, 20);
            this.txtGithubUser.TabIndex = 1;
            this.txtGithubUser.TextChanged += new System.EventHandler(this.OnCommonRepoTextChange);
            // 
            // txtGithubRepo
            // 
            this.txtGithubRepo.Location = new System.Drawing.Point(144, 38);
            this.txtGithubRepo.MaxLength = 100;
            this.txtGithubRepo.Name = "txtGithubRepo";
            this.txtGithubRepo.Size = new System.Drawing.Size(162, 20);
            this.txtGithubRepo.TabIndex = 3;
            this.txtGithubRepo.TextChanged += new System.EventHandler(this.OnCommonRepoTextChange);
            // 
            // txtToCRoot
            // 
            this.txtToCRoot.Location = new System.Drawing.Point(144, 64);
            this.txtToCRoot.MaxLength = 100;
            this.txtToCRoot.Name = "txtToCRoot";
            this.txtToCRoot.Size = new System.Drawing.Size(162, 20);
            this.txtToCRoot.TabIndex = 5;
            this.txtToCRoot.Text = "table-of-contents";
            // 
            // ManagerConfigSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(756, 361);
            this.Controls.Add(btnSave);
            this.Controls.Add(groupDetails);
            this.Controls.Add(this.txtToCRoot);
            this.Controls.Add(lbToCRoot);
            this.Controls.Add(this.txtGithubRepo);
            this.Controls.Add(lbGithubRepo);
            this.Controls.Add(this.txtGithubUser);
            this.Controls.Add(lbGithubUser);
            this.MaximizeBox = false;
            this.Name = "ManagerConfigSetupWizard";
            this.Text = "Manager Config Setup Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            groupDetails.ResumeLayout(false);
            groupDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGithubUser;
        private System.Windows.Forms.TextBox txtGithubRepo;
        private System.Windows.Forms.TextBox txtToCRoot;
        private System.Windows.Forms.TextBox txtRepoRootRaw;
        private System.Windows.Forms.TextBox txtRepoRoot;
    }
}