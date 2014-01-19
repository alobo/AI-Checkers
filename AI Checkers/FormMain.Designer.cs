namespace AICheckers
{
    partial class FormMain
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
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.boardPanel2 = new AICheckers.BoardPanel();
            this.SuspendLayout();
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 33;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // boardPanel2
            // 
            this.boardPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.boardPanel2.Location = new System.Drawing.Point(12, 12);
            this.boardPanel2.Name = "boardPanel2";
            this.boardPanel2.Size = new System.Drawing.Size(400, 400);
            this.boardPanel2.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 424);
            this.Controls.Add(this.boardPanel2);
            this.Name = "FormMain";
            this.Text = "AI Checkers";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerRefresh;
        private BoardPanel boardPanel2;
    }
}

