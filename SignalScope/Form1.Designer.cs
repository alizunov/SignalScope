namespace SignalScope
{
    partial class Form1
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
            this.OpenSignal_button = new System.Windows.Forms.Button();
            this.FileRead_progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // OpenSignal_button
            // 
            this.OpenSignal_button.Location = new System.Drawing.Point(12, 12);
            this.OpenSignal_button.Name = "OpenSignal_button";
            this.OpenSignal_button.Size = new System.Drawing.Size(75, 23);
            this.OpenSignal_button.TabIndex = 0;
            this.OpenSignal_button.Text = "Open signal";
            this.OpenSignal_button.UseVisualStyleBackColor = true;
            this.OpenSignal_button.Click += new System.EventHandler(this.OpenSignal_button_Click);
            // 
            // FileRead_progressBar
            // 
            this.FileRead_progressBar.Location = new System.Drawing.Point(93, 12);
            this.FileRead_progressBar.Name = "FileRead_progressBar";
            this.FileRead_progressBar.Size = new System.Drawing.Size(729, 23);
            this.FileRead_progressBar.TabIndex = 1;
            this.FileRead_progressBar.Click += new System.EventHandler(this.FileRead_progressBar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 678);
            this.Controls.Add(this.FileRead_progressBar);
            this.Controls.Add(this.OpenSignal_button);
            this.Name = "Form1";
            this.Text = "SignalScope";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OpenSignal_button;
        private System.Windows.Forms.ProgressBar FileRead_progressBar;
    }
}

