namespace PadoruManager.UI
{
    partial class CollectionManager
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
            System.Windows.Forms.GroupBox groupSelectionResults;
            System.Windows.Forms.Button btnOpenCollection;
            System.Windows.Forms.Panel pTopControls;
            System.Windows.Forms.Panel pBottomControls;
            System.Windows.Forms.Panel pCenterControls;
            this.entrySelectionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRunPushScript = new System.Windows.Forms.Button();
            this.btnCreateToc = new System.Windows.Forms.Button();
            this.txtCollectionSearch = new System.Windows.Forms.TextBox();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.btnEditCurrent = new System.Windows.Forms.Button();
            this.btnRemoveCurrent = new System.Windows.Forms.Button();
            this.chkForceUseRemoteImage = new System.Windows.Forms.CheckBox();
            this.btnReloadCollection = new System.Windows.Forms.Button();
            this.lbEntrysCount = new System.Windows.Forms.Label();
            this.btnSaveCollection = new System.Windows.Forms.Button();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            this.padoruPreviewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            groupSelectionResults = new System.Windows.Forms.GroupBox();
            btnOpenCollection = new System.Windows.Forms.Button();
            pTopControls = new System.Windows.Forms.Panel();
            pBottomControls = new System.Windows.Forms.Panel();
            pCenterControls = new System.Windows.Forms.Panel();
            groupSelectionResults.SuspendLayout();
            pTopControls.SuspendLayout();
            pBottomControls.SuspendLayout();
            pCenterControls.SuspendLayout();
            this.padoruPreviewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupSelectionResults
            // 
            groupSelectionResults.Controls.Add(this.entrySelectionPanel);
            groupSelectionResults.Dock = System.Windows.Forms.DockStyle.Fill;
            groupSelectionResults.Location = new System.Drawing.Point(0, 2);
            groupSelectionResults.Name = "groupSelectionResults";
            groupSelectionResults.Size = new System.Drawing.Size(960, 419);
            groupSelectionResults.TabIndex = 100;
            groupSelectionResults.TabStop = false;
            groupSelectionResults.Text = "Select Entry";
            // 
            // entrySelectionPanel
            // 
            this.entrySelectionPanel.AutoScroll = true;
            this.entrySelectionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entrySelectionPanel.Location = new System.Drawing.Point(3, 16);
            this.entrySelectionPanel.Name = "entrySelectionPanel";
            this.entrySelectionPanel.Size = new System.Drawing.Size(954, 400);
            this.entrySelectionPanel.TabIndex = 100;
            // 
            // btnOpenCollection
            // 
            btnOpenCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btnOpenCollection.Location = new System.Drawing.Point(570, 7);
            btnOpenCollection.Name = "btnOpenCollection";
            btnOpenCollection.Size = new System.Drawing.Size(125, 23);
            btnOpenCollection.TabIndex = 1060;
            btnOpenCollection.Text = "Open Collection";
            btnOpenCollection.UseVisualStyleBackColor = true;
            btnOpenCollection.Click += new System.EventHandler(this.OnOpenCollectionClick);
            // 
            // pTopControls
            // 
            pTopControls.AutoSize = true;
            pTopControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pTopControls.Controls.Add(this.btnRunPushScript);
            pTopControls.Controls.Add(this.btnCreateToc);
            pTopControls.Controls.Add(this.txtCollectionSearch);
            pTopControls.Controls.Add(this.btnAddNew);
            pTopControls.Controls.Add(this.btnEditCurrent);
            pTopControls.Controls.Add(this.btnRemoveCurrent);
            pTopControls.Dock = System.Windows.Forms.DockStyle.Top;
            pTopControls.Location = new System.Drawing.Point(12, 12);
            pTopControls.Name = "pTopControls";
            pTopControls.Size = new System.Drawing.Size(960, 29);
            pTopControls.TabIndex = 1071;
            // 
            // btnRunPushScript
            // 
            this.btnRunPushScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunPushScript.Location = new System.Drawing.Point(837, 3);
            this.btnRunPushScript.Name = "btnRunPushScript";
            this.btnRunPushScript.Size = new System.Drawing.Size(120, 23);
            this.btnRunPushScript.TabIndex = 1071;
            this.btnRunPushScript.Text = "Push Changes";
            this.btnRunPushScript.UseVisualStyleBackColor = true;
            this.btnRunPushScript.Click += new System.EventHandler(this.OnRunPushScriptClick);
            // 
            // btnCreateToc
            // 
            this.btnCreateToc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateToc.Location = new System.Drawing.Point(711, 3);
            this.btnCreateToc.Name = "btnCreateToc";
            this.btnCreateToc.Size = new System.Drawing.Size(120, 23);
            this.btnCreateToc.TabIndex = 1071;
            this.btnCreateToc.Text = "Create ToC";
            this.btnCreateToc.UseVisualStyleBackColor = true;
            this.btnCreateToc.Click += new System.EventHandler(this.OnCreateTocClick);
            // 
            // txtCollectionSearch
            // 
            this.txtCollectionSearch.Location = new System.Drawing.Point(3, 5);
            this.txtCollectionSearch.Name = "txtCollectionSearch";
            this.txtCollectionSearch.Size = new System.Drawing.Size(230, 20);
            this.txtCollectionSearch.TabIndex = 10;
            this.txtCollectionSearch.TextChanged += new System.EventHandler(this.OnSearchTextChange);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Location = new System.Drawing.Point(239, 3);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(90, 23);
            this.btnAddNew.TabIndex = 20;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.OnAddNewClick);
            // 
            // btnEditCurrent
            // 
            this.btnEditCurrent.Location = new System.Drawing.Point(335, 3);
            this.btnEditCurrent.Name = "btnEditCurrent";
            this.btnEditCurrent.Size = new System.Drawing.Size(90, 23);
            this.btnEditCurrent.TabIndex = 30;
            this.btnEditCurrent.Text = "Edit";
            this.btnEditCurrent.UseVisualStyleBackColor = true;
            this.btnEditCurrent.Click += new System.EventHandler(this.OnEditCurrentClick);
            // 
            // btnRemoveCurrent
            // 
            this.btnRemoveCurrent.Location = new System.Drawing.Point(431, 3);
            this.btnRemoveCurrent.Name = "btnRemoveCurrent";
            this.btnRemoveCurrent.Size = new System.Drawing.Size(90, 23);
            this.btnRemoveCurrent.TabIndex = 40;
            this.btnRemoveCurrent.Text = "Remove";
            this.btnRemoveCurrent.UseVisualStyleBackColor = true;
            this.btnRemoveCurrent.Click += new System.EventHandler(this.OnRemoveCurrentClick);
            // 
            // pBottomControls
            // 
            pBottomControls.AutoSize = true;
            pBottomControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pBottomControls.Controls.Add(this.chkForceUseRemoteImage);
            pBottomControls.Controls.Add(this.btnReloadCollection);
            pBottomControls.Controls.Add(this.lbEntrysCount);
            pBottomControls.Controls.Add(btnOpenCollection);
            pBottomControls.Controls.Add(this.btnSaveCollection);
            pBottomControls.Controls.Add(this.chkAutoSave);
            pBottomControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            pBottomControls.Location = new System.Drawing.Point(12, 464);
            pBottomControls.Name = "pBottomControls";
            pBottomControls.Size = new System.Drawing.Size(960, 35);
            pBottomControls.TabIndex = 1072;
            // 
            // chkForceUseRemoteImage
            // 
            this.chkForceUseRemoteImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkForceUseRemoteImage.AutoSize = true;
            this.chkForceUseRemoteImage.Location = new System.Drawing.Point(360, 11);
            this.chkForceUseRemoteImage.Name = "chkForceUseRemoteImage";
            this.chkForceUseRemoteImage.Size = new System.Drawing.Size(125, 17);
            this.chkForceUseRemoteImage.TabIndex = 1072;
            this.chkForceUseRemoteImage.Text = "Force Remote Image";
            this.chkForceUseRemoteImage.UseVisualStyleBackColor = true;
            this.chkForceUseRemoteImage.CheckedChanged += new System.EventHandler(this.OnForceRemoteImageChanged);
            // 
            // btnReloadCollection
            // 
            this.btnReloadCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReloadCollection.Location = new System.Drawing.Point(701, 7);
            this.btnReloadCollection.Name = "btnReloadCollection";
            this.btnReloadCollection.Size = new System.Drawing.Size(125, 23);
            this.btnReloadCollection.TabIndex = 1071;
            this.btnReloadCollection.Text = "Reload Collection";
            this.btnReloadCollection.UseVisualStyleBackColor = true;
            this.btnReloadCollection.Click += new System.EventHandler(this.OnReloadCollectionClick);
            // 
            // lbEntrysCount
            // 
            this.lbEntrysCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbEntrysCount.AutoSize = true;
            this.lbEntrysCount.Location = new System.Drawing.Point(3, 12);
            this.lbEntrysCount.Name = "lbEntrysCount";
            this.lbEntrysCount.Size = new System.Drawing.Size(109, 13);
            this.lbEntrysCount.TabIndex = 2;
            this.lbEntrysCount.Text = "Showing 0 of 0 entrys";
            // 
            // btnSaveCollection
            // 
            this.btnSaveCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveCollection.Location = new System.Drawing.Point(832, 7);
            this.btnSaveCollection.Name = "btnSaveCollection";
            this.btnSaveCollection.Size = new System.Drawing.Size(125, 23);
            this.btnSaveCollection.TabIndex = 1070;
            this.btnSaveCollection.Text = "Save Collection";
            this.btnSaveCollection.UseVisualStyleBackColor = true;
            this.btnSaveCollection.Click += new System.EventHandler(this.OnSaveCollectionClick);
            // 
            // chkAutoSave
            // 
            this.chkAutoSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoSave.AutoSize = true;
            this.chkAutoSave.Checked = true;
            this.chkAutoSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoSave.Location = new System.Drawing.Point(491, 11);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(73, 17);
            this.chkAutoSave.TabIndex = 1051;
            this.chkAutoSave.Text = "AutoSave";
            this.chkAutoSave.UseVisualStyleBackColor = true;
            // 
            // pCenterControls
            // 
            pCenterControls.Controls.Add(groupSelectionResults);
            pCenterControls.Dock = System.Windows.Forms.DockStyle.Fill;
            pCenterControls.Location = new System.Drawing.Point(12, 41);
            pCenterControls.Name = "pCenterControls";
            pCenterControls.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            pCenterControls.Size = new System.Drawing.Size(960, 423);
            pCenterControls.TabIndex = 1073;
            // 
            // padoruPreviewContextMenu
            // 
            this.padoruPreviewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.padoruPreviewContextMenu.Name = "padoruPreviewContextMenu";
            this.padoruPreviewContextMenu.Size = new System.Drawing.Size(118, 48);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.OnEditCurrentClick);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.OnRemoveCurrentClick);
            // 
            // CollectionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 511);
            this.Controls.Add(pCenterControls);
            this.Controls.Add(pBottomControls);
            this.Controls.Add(pTopControls);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(820, 420);
            this.Name = "CollectionManager";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.Text = "Collection Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResizeEnd += new System.EventHandler(this.OnUiResizeEnd);
            groupSelectionResults.ResumeLayout(false);
            pTopControls.ResumeLayout(false);
            pTopControls.PerformLayout();
            pBottomControls.ResumeLayout(false);
            pBottomControls.PerformLayout();
            pCenterControls.ResumeLayout(false);
            this.padoruPreviewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel entrySelectionPanel;
        private System.Windows.Forms.TextBox txtCollectionSearch;
        private System.Windows.Forms.Label lbEntrysCount;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnSaveCollection;
        private System.Windows.Forms.Button btnEditCurrent;
        private System.Windows.Forms.Button btnRemoveCurrent;
        private System.Windows.Forms.CheckBox chkAutoSave;
        private System.Windows.Forms.ContextMenuStrip padoruPreviewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.Button btnCreateToc;
        private System.Windows.Forms.Button btnRunPushScript;
        private System.Windows.Forms.Button btnReloadCollection;
        private System.Windows.Forms.CheckBox chkForceUseRemoteImage;
    }
}