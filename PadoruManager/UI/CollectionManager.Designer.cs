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
            this.txtCollectionSearch = new System.Windows.Forms.TextBox();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.btnEditCurrent = new System.Windows.Forms.Button();
            this.btnRemoveCurrent = new System.Windows.Forms.Button();
            this.btnCreateToc = new System.Windows.Forms.Button();
            this.lbEntrysCount = new System.Windows.Forms.Label();
            this.btnSaveCollection = new System.Windows.Forms.Button();
            this.chkEnableSaveScript = new System.Windows.Forms.CheckBox();
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
            groupSelectionResults.Size = new System.Drawing.Size(780, 389);
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
            this.entrySelectionPanel.Size = new System.Drawing.Size(774, 370);
            this.entrySelectionPanel.TabIndex = 100;
            // 
            // btnOpenCollection
            // 
            btnOpenCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btnOpenCollection.Location = new System.Drawing.Point(521, 7);
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
            pTopControls.Controls.Add(this.txtCollectionSearch);
            pTopControls.Controls.Add(this.btnAddNew);
            pTopControls.Controls.Add(this.btnEditCurrent);
            pTopControls.Controls.Add(this.btnRemoveCurrent);
            pTopControls.Dock = System.Windows.Forms.DockStyle.Top;
            pTopControls.Location = new System.Drawing.Point(12, 12);
            pTopControls.Name = "pTopControls";
            pTopControls.Size = new System.Drawing.Size(780, 29);
            pTopControls.TabIndex = 1071;
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
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNew.Location = new System.Drawing.Point(495, 3);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(90, 23);
            this.btnAddNew.TabIndex = 20;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.OnAddNewClick);
            // 
            // btnEditCurrent
            // 
            this.btnEditCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditCurrent.Location = new System.Drawing.Point(591, 3);
            this.btnEditCurrent.Name = "btnEditCurrent";
            this.btnEditCurrent.Size = new System.Drawing.Size(90, 23);
            this.btnEditCurrent.TabIndex = 30;
            this.btnEditCurrent.Text = "Edit";
            this.btnEditCurrent.UseVisualStyleBackColor = true;
            this.btnEditCurrent.Click += new System.EventHandler(this.OnEditCurrentClick);
            // 
            // btnRemoveCurrent
            // 
            this.btnRemoveCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveCurrent.Location = new System.Drawing.Point(687, 3);
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
            pBottomControls.Controls.Add(this.btnCreateToc);
            pBottomControls.Controls.Add(this.lbEntrysCount);
            pBottomControls.Controls.Add(this.btnSaveCollection);
            pBottomControls.Controls.Add(btnOpenCollection);
            pBottomControls.Controls.Add(this.chkEnableSaveScript);
            pBottomControls.Controls.Add(this.chkAutoSave);
            pBottomControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            pBottomControls.Location = new System.Drawing.Point(12, 434);
            pBottomControls.Name = "pBottomControls";
            pBottomControls.Size = new System.Drawing.Size(780, 35);
            pBottomControls.TabIndex = 1072;
            // 
            // btnCreateToc
            // 
            this.btnCreateToc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateToc.Location = new System.Drawing.Point(390, 7);
            this.btnCreateToc.Name = "btnCreateToc";
            this.btnCreateToc.Size = new System.Drawing.Size(125, 23);
            this.btnCreateToc.TabIndex = 1071;
            this.btnCreateToc.Text = "Create ToC";
            this.btnCreateToc.UseVisualStyleBackColor = true;
            this.btnCreateToc.Click += new System.EventHandler(this.OnCreateTocClick);
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
            this.btnSaveCollection.Location = new System.Drawing.Point(652, 7);
            this.btnSaveCollection.Name = "btnSaveCollection";
            this.btnSaveCollection.Size = new System.Drawing.Size(125, 23);
            this.btnSaveCollection.TabIndex = 1070;
            this.btnSaveCollection.Text = "Save Collection";
            this.btnSaveCollection.UseVisualStyleBackColor = true;
            this.btnSaveCollection.Click += new System.EventHandler(this.OnSaveCollectionClick);
            // 
            // chkEnableSaveScript
            // 
            this.chkEnableSaveScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEnableSaveScript.AutoSize = true;
            this.chkEnableSaveScript.Location = new System.Drawing.Point(227, 11);
            this.chkEnableSaveScript.Name = "chkEnableSaveScript";
            this.chkEnableSaveScript.Size = new System.Drawing.Size(78, 17);
            this.chkEnableSaveScript.TabIndex = 1050;
            this.chkEnableSaveScript.Text = "SaveScript";
            this.chkEnableSaveScript.UseVisualStyleBackColor = true;
            // 
            // chkAutoSave
            // 
            this.chkAutoSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoSave.AutoSize = true;
            this.chkAutoSave.Checked = true;
            this.chkAutoSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoSave.Location = new System.Drawing.Point(311, 11);
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
            pCenterControls.Size = new System.Drawing.Size(780, 393);
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
            this.ClientSize = new System.Drawing.Size(804, 481);
            this.Controls.Add(pCenterControls);
            this.Controls.Add(pBottomControls);
            this.Controls.Add(pTopControls);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 420);
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
        private System.Windows.Forms.CheckBox chkEnableSaveScript;
        private System.Windows.Forms.CheckBox chkAutoSave;
        private System.Windows.Forms.ContextMenuStrip padoruPreviewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.Button btnCreateToc;
    }
}