﻿namespace PadoruManager.UI
{
    partial class PadoruEditor
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
            System.Windows.Forms.GroupBox groupImage;
            System.Windows.Forms.Button btnBrowseImagePath;
            System.Windows.Forms.Label lbPath;
            System.Windows.Forms.Label lbUrl;
            System.Windows.Forms.GroupBox groupCharacter;
            System.Windows.Forms.Label lbSelectedMalId;
            System.Windows.Forms.Label lbMalName;
            System.Windows.Forms.Label lbMalId;
            System.Windows.Forms.Label lbName;
            System.Windows.Forms.GroupBox groupCredits;
            System.Windows.Forms.Label lbSource;
            System.Windows.Forms.Label lbCreator;
            System.Windows.Forms.GroupBox groupMalPreview;
            System.Windows.Forms.Button btnCancel;
            System.Windows.Forms.Button btnFinish;
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.txtImageUrl = new System.Windows.Forms.TextBox();
            this.txtSelectedMalId = new System.Windows.Forms.TextBox();
            this.txtSelectedMalName = new System.Windows.Forms.TextBox();
            this.cbMalIdSelector = new System.Windows.Forms.ComboBox();
            this.chkCharacterFemale = new System.Windows.Forms.CheckBox();
            this.txtCharacterName = new System.Windows.Forms.TextBox();
            this.txtImageSource = new System.Windows.Forms.TextBox();
            this.txtImageCreator = new System.Windows.Forms.TextBox();
            this.ppvMalResultPreview = new PadoruManager.UI.PadoruPreview();
            groupImage = new System.Windows.Forms.GroupBox();
            btnBrowseImagePath = new System.Windows.Forms.Button();
            lbPath = new System.Windows.Forms.Label();
            lbUrl = new System.Windows.Forms.Label();
            groupCharacter = new System.Windows.Forms.GroupBox();
            lbSelectedMalId = new System.Windows.Forms.Label();
            lbMalName = new System.Windows.Forms.Label();
            lbMalId = new System.Windows.Forms.Label();
            lbName = new System.Windows.Forms.Label();
            groupCredits = new System.Windows.Forms.GroupBox();
            lbSource = new System.Windows.Forms.Label();
            lbCreator = new System.Windows.Forms.Label();
            groupMalPreview = new System.Windows.Forms.GroupBox();
            btnCancel = new System.Windows.Forms.Button();
            btnFinish = new System.Windows.Forms.Button();
            groupImage.SuspendLayout();
            groupCharacter.SuspendLayout();
            groupCredits.SuspendLayout();
            groupMalPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupImage
            // 
            groupImage.Controls.Add(btnBrowseImagePath);
            groupImage.Controls.Add(this.txtImagePath);
            groupImage.Controls.Add(lbPath);
            groupImage.Controls.Add(lbUrl);
            groupImage.Controls.Add(this.txtImageUrl);
            groupImage.Location = new System.Drawing.Point(15, 15);
            groupImage.Name = "groupImage";
            groupImage.Size = new System.Drawing.Size(414, 81);
            groupImage.TabIndex = 0;
            groupImage.TabStop = false;
            groupImage.Text = "Image";
            // 
            // btnBrowseImagePath
            // 
            btnBrowseImagePath.Location = new System.Drawing.Point(387, 47);
            btnBrowseImagePath.Name = "btnBrowseImagePath";
            btnBrowseImagePath.Size = new System.Drawing.Size(20, 20);
            btnBrowseImagePath.TabIndex = 30;
            btnBrowseImagePath.Text = "^";
            btnBrowseImagePath.UseVisualStyleBackColor = true;
            btnBrowseImagePath.Click += new System.EventHandler(this.OnBrowseImagePathClick);
            // 
            // txtImagePath
            // 
            this.txtImagePath.Location = new System.Drawing.Point(41, 48);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(340, 20);
            this.txtImagePath.TabIndex = 20;
            // 
            // lbPath
            // 
            lbPath.AutoSize = true;
            lbPath.Location = new System.Drawing.Point(6, 51);
            lbPath.Name = "lbPath";
            lbPath.Size = new System.Drawing.Size(33, 13);
            lbPath.TabIndex = 4;
            lbPath.Text = "Path*";
            // 
            // lbUrl
            // 
            lbUrl.AutoSize = true;
            lbUrl.Location = new System.Drawing.Point(6, 25);
            lbUrl.Name = "lbUrl";
            lbUrl.Size = new System.Drawing.Size(33, 13);
            lbUrl.TabIndex = 0;
            lbUrl.Text = "URL*";
            // 
            // txtImageUrl
            // 
            this.txtImageUrl.Location = new System.Drawing.Point(41, 22);
            this.txtImageUrl.Name = "txtImageUrl";
            this.txtImageUrl.Size = new System.Drawing.Size(366, 20);
            this.txtImageUrl.TabIndex = 10;
            // 
            // groupCharacter
            // 
            groupCharacter.Controls.Add(this.txtSelectedMalId);
            groupCharacter.Controls.Add(lbSelectedMalId);
            groupCharacter.Controls.Add(this.txtSelectedMalName);
            groupCharacter.Controls.Add(lbMalName);
            groupCharacter.Controls.Add(this.cbMalIdSelector);
            groupCharacter.Controls.Add(lbMalId);
            groupCharacter.Controls.Add(this.chkCharacterFemale);
            groupCharacter.Controls.Add(lbName);
            groupCharacter.Controls.Add(this.txtCharacterName);
            groupCharacter.Location = new System.Drawing.Point(15, 102);
            groupCharacter.Name = "groupCharacter";
            groupCharacter.Size = new System.Drawing.Size(299, 204);
            groupCharacter.TabIndex = 1;
            groupCharacter.TabStop = false;
            groupCharacter.Text = "Character";
            // 
            // txtSelectedMalId
            // 
            this.txtSelectedMalId.Location = new System.Drawing.Point(70, 149);
            this.txtSelectedMalId.Name = "txtSelectedMalId";
            this.txtSelectedMalId.ReadOnly = true;
            this.txtSelectedMalId.Size = new System.Drawing.Size(223, 20);
            this.txtSelectedMalId.TabIndex = 42;
            this.txtSelectedMalId.TabStop = false;
            // 
            // lbSelectedMalId
            // 
            lbSelectedMalId.AutoSize = true;
            lbSelectedMalId.Location = new System.Drawing.Point(4, 152);
            lbSelectedMalId.Name = "lbSelectedMalId";
            lbSelectedMalId.Size = new System.Drawing.Size(43, 13);
            lbSelectedMalId.TabIndex = 41;
            lbSelectedMalId.Text = "MAL ID";
            // 
            // txtSelectedMalName
            // 
            this.txtSelectedMalName.Location = new System.Drawing.Point(70, 175);
            this.txtSelectedMalName.Name = "txtSelectedMalName";
            this.txtSelectedMalName.ReadOnly = true;
            this.txtSelectedMalName.Size = new System.Drawing.Size(223, 20);
            this.txtSelectedMalName.TabIndex = 40;
            this.txtSelectedMalName.TabStop = false;
            // 
            // lbMalName
            // 
            lbMalName.AutoSize = true;
            lbMalName.Location = new System.Drawing.Point(4, 178);
            lbMalName.Name = "lbMalName";
            lbMalName.Size = new System.Drawing.Size(60, 13);
            lbMalName.TabIndex = 12;
            lbMalName.Text = "MAL Name";
            // 
            // cbMalIdSelector
            // 
            this.cbMalIdSelector.FormattingEnabled = true;
            this.cbMalIdSelector.Location = new System.Drawing.Point(68, 122);
            this.cbMalIdSelector.Name = "cbMalIdSelector";
            this.cbMalIdSelector.Size = new System.Drawing.Size(225, 21);
            this.cbMalIdSelector.TabIndex = 30;
            this.cbMalIdSelector.SelectedIndexChanged += new System.EventHandler(this.OnMalIdSelectorChange);
            // 
            // lbMalId
            // 
            lbMalId.AutoSize = true;
            lbMalId.Location = new System.Drawing.Point(4, 125);
            lbMalId.Name = "lbMalId";
            lbMalId.Size = new System.Drawing.Size(43, 13);
            lbMalId.TabIndex = 10;
            lbMalId.Text = "MAL ID";
            // 
            // chkCharacterFemale
            // 
            this.chkCharacterFemale.AutoSize = true;
            this.chkCharacterFemale.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCharacterFemale.Location = new System.Drawing.Point(9, 49);
            this.chkCharacterFemale.Name = "chkCharacterFemale";
            this.chkCharacterFemale.Size = new System.Drawing.Size(77, 17);
            this.chkCharacterFemale.TabIndex = 20;
            this.chkCharacterFemale.Text = "Is Female?";
            this.chkCharacterFemale.UseVisualStyleBackColor = true;
            // 
            // lbName
            // 
            lbName.AutoSize = true;
            lbName.Location = new System.Drawing.Point(6, 26);
            lbName.Name = "lbName";
            lbName.Size = new System.Drawing.Size(39, 13);
            lbName.TabIndex = 7;
            lbName.Text = "Name*";
            // 
            // txtCharacterName
            // 
            this.txtCharacterName.Location = new System.Drawing.Point(70, 23);
            this.txtCharacterName.Name = "txtCharacterName";
            this.txtCharacterName.Size = new System.Drawing.Size(223, 20);
            this.txtCharacterName.TabIndex = 10;
            this.txtCharacterName.TextChanged += new System.EventHandler(this.OnCharacterNameChange);
            this.txtCharacterName.Leave += new System.EventHandler(this.OnCharacterNameEditEnd);
            // 
            // groupCredits
            // 
            groupCredits.Controls.Add(lbSource);
            groupCredits.Controls.Add(this.txtImageSource);
            groupCredits.Controls.Add(lbCreator);
            groupCredits.Controls.Add(this.txtImageCreator);
            groupCredits.Location = new System.Drawing.Point(15, 312);
            groupCredits.Name = "groupCredits";
            groupCredits.Size = new System.Drawing.Size(414, 74);
            groupCredits.TabIndex = 2;
            groupCredits.TabStop = false;
            groupCredits.Text = "Credits";
            // 
            // lbSource
            // 
            lbSource.AutoSize = true;
            lbSource.Location = new System.Drawing.Point(6, 48);
            lbSource.Name = "lbSource";
            lbSource.Size = new System.Drawing.Size(41, 13);
            lbSource.TabIndex = 9;
            lbSource.Text = "Source";
            // 
            // txtImageSource
            // 
            this.txtImageSource.Location = new System.Drawing.Point(53, 45);
            this.txtImageSource.Name = "txtImageSource";
            this.txtImageSource.Size = new System.Drawing.Size(354, 20);
            this.txtImageSource.TabIndex = 20;
            // 
            // lbCreator
            // 
            lbCreator.AutoSize = true;
            lbCreator.Location = new System.Drawing.Point(6, 22);
            lbCreator.Name = "lbCreator";
            lbCreator.Size = new System.Drawing.Size(41, 13);
            lbCreator.TabIndex = 7;
            lbCreator.Text = "Creator";
            // 
            // txtImageCreator
            // 
            this.txtImageCreator.Location = new System.Drawing.Point(53, 19);
            this.txtImageCreator.Name = "txtImageCreator";
            this.txtImageCreator.Size = new System.Drawing.Size(240, 20);
            this.txtImageCreator.TabIndex = 10;
            // 
            // groupMalPreview
            // 
            groupMalPreview.Controls.Add(this.ppvMalResultPreview);
            groupMalPreview.Location = new System.Drawing.Point(320, 102);
            groupMalPreview.Name = "groupMalPreview";
            groupMalPreview.Size = new System.Drawing.Size(109, 204);
            groupMalPreview.TabIndex = 4;
            groupMalPreview.TabStop = false;
            groupMalPreview.Text = "MAL Preview";
            // 
            // ppvMalResultPreview
            // 
            this.ppvMalResultPreview.DisplayName = "<Name>";
            this.ppvMalResultPreview.Location = new System.Drawing.Point(6, 19);
            this.ppvMalResultPreview.Name = "ppvMalResultPreview";
            this.ppvMalResultPreview.PreviewImage = null;
            this.ppvMalResultPreview.Size = new System.Drawing.Size(96, 176);
            this.ppvMalResultPreview.TabIndex = 10;
            this.ppvMalResultPreview.ThickBorders = false;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(15, 392);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(117, 23);
            btnCancel.TabIndex = 1010;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // btnFinish
            // 
            btnFinish.Location = new System.Drawing.Point(312, 392);
            btnFinish.Name = "btnFinish";
            btnFinish.Size = new System.Drawing.Size(117, 23);
            btnFinish.TabIndex = 1000;
            btnFinish.Text = "Finish";
            btnFinish.UseVisualStyleBackColor = true;
            btnFinish.Click += new System.EventHandler(this.OnFinishClick);
            // 
            // PadoruEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(956, 548);
            this.Controls.Add(btnFinish);
            this.Controls.Add(btnCancel);
            this.Controls.Add(groupMalPreview);
            this.Controls.Add(groupCredits);
            this.Controls.Add(groupCharacter);
            this.Controls.Add(groupImage);
            this.MaximizeBox = false;
            this.Name = "PadoruEditor";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.Text = "Padoru Entry Editor";
            this.Load += new System.EventHandler(this.OnLoad);
            groupImage.ResumeLayout(false);
            groupImage.PerformLayout();
            groupCharacter.ResumeLayout(false);
            groupCharacter.PerformLayout();
            groupCredits.ResumeLayout(false);
            groupCredits.PerformLayout();
            groupMalPreview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.TextBox txtImageUrl;
        private System.Windows.Forms.CheckBox chkCharacterFemale;
        private System.Windows.Forms.TextBox txtCharacterName;
        private System.Windows.Forms.ComboBox cbMalIdSelector;
        private System.Windows.Forms.TextBox txtSelectedMalName;
        private System.Windows.Forms.TextBox txtImageSource;
        private System.Windows.Forms.TextBox txtImageCreator;
        private PadoruPreview ppvMalResultPreview;
        private System.Windows.Forms.TextBox txtSelectedMalId;
    }
}