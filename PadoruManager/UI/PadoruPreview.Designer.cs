namespace PadoruManager.UI
{
    partial class PadoruPreview
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.imgPadoru = new System.Windows.Forms.PictureBox();
            this.lbName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgPadoru)).BeginInit();
            this.SuspendLayout();
            // 
            // imgPadoru
            // 
            this.imgPadoru.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.imgPadoru.Location = new System.Drawing.Point(3, 3);
            this.imgPadoru.Name = "imgPadoru";
            this.imgPadoru.Size = new System.Drawing.Size(90, 120);
            this.imgPadoru.TabIndex = 0;
            this.imgPadoru.TabStop = false;
            this.imgPadoru.Click += new System.EventHandler(this.OnAnyClick);
            // 
            // lbName
            // 
            this.lbName.Location = new System.Drawing.Point(3, 126);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(90, 44);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "<Name>";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbName.Click += new System.EventHandler(this.OnAnyClick);
            // 
            // PadoruPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.imgPadoru);
            this.Name = "PadoruPreview";
            this.Size = new System.Drawing.Size(96, 173);
            ((System.ComponentModel.ISupportInitialize)(this.imgPadoru)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imgPadoru;
        private System.Windows.Forms.Label lbName;
    }
}
