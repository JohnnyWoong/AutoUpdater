namespace KentCraftAutoUpdater
{
    partial class AutoUpdate
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
            this.bgwUpdate = new System.ComponentModel.BackgroundWorker();
            this.pbUpdate = new System.Windows.Forms.ProgressBar();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbUpdateAll = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // bgwUpdate
            // 
            this.bgwUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwUpdate_DoWork);
            this.bgwUpdate.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwUpdate_ProgressChanged);
            this.bgwUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwUpdate_RunWorkerCompleted);
            // 
            // pbUpdate
            // 
            this.pbUpdate.Location = new System.Drawing.Point(18, 94);
            this.pbUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.pbUpdate.Name = "pbUpdate";
            this.pbUpdate.Size = new System.Drawing.Size(602, 34);
            this.pbUpdate.TabIndex = 0;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.Location = new System.Drawing.Point(18, 36);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(183, 25);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "正在更新,请稍候...";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(258, 215);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 34);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pbUpdateAll
            // 
            this.pbUpdateAll.Location = new System.Drawing.Point(18, 157);
            this.pbUpdateAll.Name = "pbUpdateAll";
            this.pbUpdateAll.Size = new System.Drawing.Size(602, 34);
            this.pbUpdateAll.TabIndex = 5;
            // 
            // AutoUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(638, 276);
            this.Controls.Add(this.pbUpdateAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.pbUpdate);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "AutoUpdate";
            this.Text = "自动更新";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AutoUpdate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker bgwUpdate;
        private System.Windows.Forms.ProgressBar pbUpdate;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar pbUpdateAll;
    }
}