namespace Box_Wars
{
    partial class LevelFailedScreen
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.failLabel = new System.Windows.Forms.Label();
            this.tryButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // failLabel
            // 
            this.failLabel.AutoSize = true;
            this.failLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.failLabel.Location = new System.Drawing.Point(73, 95);
            this.failLabel.Name = "failLabel";
            this.failLabel.Size = new System.Drawing.Size(663, 108);
            this.failLabel.TabIndex = 0;
            this.failLabel.Text = "GAME OVER!";
            // 
            // tryButton
            // 
            this.tryButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tryButton.Location = new System.Drawing.Point(190, 267);
            this.tryButton.Name = "tryButton";
            this.tryButton.Size = new System.Drawing.Size(151, 62);
            this.tryButton.TabIndex = 1;
            this.tryButton.Text = "Try Again";
            this.tryButton.UseVisualStyleBackColor = false;
            this.tryButton.Click += new System.EventHandler(this.tryButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.exitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.Location = new System.Drawing.Point(464, 267);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(151, 62);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // LevelFailedScreen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Purple;
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.tryButton);
            this.Controls.Add(this.failLabel);
            this.Name = "LevelFailedScreen";
            this.Size = new System.Drawing.Size(800, 500);
            this.Load += new System.EventHandler(this.LevelFailedScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label failLabel;
        private System.Windows.Forms.Button tryButton;
        private System.Windows.Forms.Button exitButton;
    }
}
