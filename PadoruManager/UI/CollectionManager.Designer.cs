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
            System.Windows.Forms.GroupBox groupSelectionResults;
            System.Windows.Forms.Button btnOpenCollection;
            this.entrySelectionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.btnSaveCollection = new System.Windows.Forms.Button();
            this.lbEntrysCount = new System.Windows.Forms.Label();
            this.txtCollectionSearch = new System.Windows.Forms.TextBox();
            this.btnEditCurrent = new System.Windows.Forms.Button();
            this.btnRemoveCurrent = new System.Windows.Forms.Button();
            groupSelectionResults = new System.Windows.Forms.GroupBox();
            btnOpenCollection = new System.Windows.Forms.Button();
            groupSelectionResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupSelectionResults
            // 
            groupSelectionResults.Controls.Add(this.entrySelectionPanel);
            groupSelectionResults.Location = new System.Drawing.Point(12, 39);
            groupSelectionResults.Name = "groupSelectionResults";
            groupSelectionResults.Size = new System.Drawing.Size(492, 356);
            groupSelectionResults.TabIndex = 0;
            groupSelectionResults.TabStop = false;
            groupSelectionResults.Text = "Select Entry";
            // 
            // entrySelectionPanel
            // 
            this.entrySelectionPanel.AutoScroll = true;
            this.entrySelectionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entrySelectionPanel.Location = new System.Drawing.Point(3, 16);
            this.entrySelectionPanel.Name = "entrySelectionPanel";
            this.entrySelectionPanel.Size = new System.Drawing.Size(486, 337);
            this.entrySelectionPanel.TabIndex = 1;
            // 
            // btnOpenCollection
            // 
            btnOpenCollection.Location = new System.Drawing.Point(248, 401);
            btnOpenCollection.Name = "btnOpenCollection";
            btnOpenCollection.Size = new System.Drawing.Size(125, 23);
            btnOpenCollection.TabIndex = 4;
            btnOpenCollection.Text = "Open Collection";
            btnOpenCollection.UseVisualStyleBackColor = true;
            btnOpenCollection.Click += new System.EventHandler(this.OnOpenCollectionClick);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Location = new System.Drawing.Point(222, 10);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(90, 23);
            this.btnAddNew.TabIndex = 5;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.OnAddNewClick);
            // 
            // btnSaveCollection
            // 
            this.btnSaveCollection.Location = new System.Drawing.Point(379, 401);
            this.btnSaveCollection.Name = "btnSaveCollection";
            this.btnSaveCollection.Size = new System.Drawing.Size(125, 23);
            this.btnSaveCollection.TabIndex = 6;
            this.btnSaveCollection.Text = "Save Collection";
            this.btnSaveCollection.UseVisualStyleBackColor = true;
            this.btnSaveCollection.Click += new System.EventHandler(this.OnSaveCollectionClick);
            // 
            // lbEntrysCount
            // 
            this.lbEntrysCount.AutoSize = true;
            this.lbEntrysCount.Location = new System.Drawing.Point(12, 406);
            this.lbEntrysCount.Name = "lbEntrysCount";
            this.lbEntrysCount.Size = new System.Drawing.Size(109, 13);
            this.lbEntrysCount.TabIndex = 2;
            this.lbEntrysCount.Text = "Showing 0 of 0 entrys";
            // 
            // txtCollectionSearch
            // 
            this.txtCollectionSearch.Location = new System.Drawing.Point(12, 12);
            this.txtCollectionSearch.Name = "txtCollectionSearch";
            this.txtCollectionSearch.Size = new System.Drawing.Size(204, 20);
            this.txtCollectionSearch.TabIndex = 7;
            this.txtCollectionSearch.TextChanged += new System.EventHandler(this.OnSearchTextChange);
            // 
            // btnEditCurrent
            // 
            this.btnEditCurrent.Location = new System.Drawing.Point(318, 10);
            this.btnEditCurrent.Name = "btnEditCurrent";
            this.btnEditCurrent.Size = new System.Drawing.Size(90, 23);
            this.btnEditCurrent.TabIndex = 8;
            this.btnEditCurrent.Text = "Edit";
            this.btnEditCurrent.UseVisualStyleBackColor = true;
            this.btnEditCurrent.Click += new System.EventHandler(this.OnEditCurrentClick);
            // 
            // btnRemoveCurrent
            // 
            this.btnRemoveCurrent.Location = new System.Drawing.Point(414, 9);
            this.btnRemoveCurrent.Name = "btnRemoveCurrent";
            this.btnRemoveCurrent.Size = new System.Drawing.Size(90, 23);
            this.btnRemoveCurrent.TabIndex = 9;
            this.btnRemoveCurrent.Text = "Remove";
            this.btnRemoveCurrent.UseVisualStyleBackColor = true;
            this.btnRemoveCurrent.Click += new System.EventHandler(this.OnRemoveCurrentClick);
            // 
            // CollectionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(731, 507);
            this.Controls.Add(this.btnRemoveCurrent);
            this.Controls.Add(this.btnEditCurrent);
            this.Controls.Add(this.txtCollectionSearch);
            this.Controls.Add(this.btnSaveCollection);
            this.Controls.Add(this.btnAddNew);
            this.Controls.Add(btnOpenCollection);
            this.Controls.Add(this.lbEntrysCount);
            this.Controls.Add(groupSelectionResults);
            this.MaximizeBox = false;
            this.Name = "CollectionManager";
            this.Text = "Collection Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            groupSelectionResults.ResumeLayout(false);
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
    }
}