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
            this.components = new System.ComponentModel.Container();
            this.OpenSignal_button = new System.Windows.Forms.Button();
            this.FileRead_progressBar = new System.Windows.Forms.ProgressBar();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.WaveformPlots_groupBox = new System.Windows.Forms.GroupBox();
            this.ClearWaves_button = new System.Windows.Forms.Button();
            this.DeleteWave_button = new System.Windows.Forms.Button();
            this.Meas_control_groupBox = new System.Windows.Forms.GroupBox();
            this.UseFitPoly_checkBox = new System.Windows.Forms.CheckBox();
            this.ClearMeas_button = new System.Windows.Forms.Button();
            this.DeleteMeas_button = new System.Windows.Forms.Button();
            this.ActiveMeas_comboBox = new System.Windows.Forms.ComboBox();
            this.DisplayMeas_checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.AddMeas_button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Meas_HighGate_t1_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Meas_HighGate_t0_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Meas_LowGate_t1_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.Meas_LowGate_t0_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ActiveCurve_comboBox = new System.Windows.Forms.ComboBox();
            this.TimeUnits_comboBox = new System.Windows.Forms.ComboBox();
            this.WaveformPlots_groupBox.SuspendLayout();
            this.Meas_control_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Meas_HighGate_t1_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Meas_HighGate_t0_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Meas_LowGate_t1_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Meas_LowGate_t0_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenSignal_button
            // 
            this.OpenSignal_button.Location = new System.Drawing.Point(12, 12);
            this.OpenSignal_button.Name = "OpenSignal_button";
            this.OpenSignal_button.Size = new System.Drawing.Size(75, 23);
            this.OpenSignal_button.TabIndex = 0;
            this.OpenSignal_button.Text = "Read file";
            this.OpenSignal_button.UseVisualStyleBackColor = true;
            this.OpenSignal_button.Click += new System.EventHandler(this.OpenSignal_button_Click);
            // 
            // FileRead_progressBar
            // 
            this.FileRead_progressBar.Location = new System.Drawing.Point(93, 12);
            this.FileRead_progressBar.Name = "FileRead_progressBar";
            this.FileRead_progressBar.Size = new System.Drawing.Size(1098, 23);
            this.FileRead_progressBar.TabIndex = 1;
            this.FileRead_progressBar.Click += new System.EventHandler(this.FileRead_progressBar_Click);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 41);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(1179, 750);
            this.zedGraphControl1.TabIndex = 2;
            this.zedGraphControl1.UseExtendedPrintDialog = true;
            this.zedGraphControl1.Load += new System.EventHandler(this.zedGraphControl1_Load);
            // 
            // WaveformPlots_groupBox
            // 
            this.WaveformPlots_groupBox.Controls.Add(this.ClearWaves_button);
            this.WaveformPlots_groupBox.Controls.Add(this.DeleteWave_button);
            this.WaveformPlots_groupBox.Controls.Add(this.Meas_control_groupBox);
            this.WaveformPlots_groupBox.Controls.Add(this.ActiveCurve_comboBox);
            this.WaveformPlots_groupBox.Controls.Add(this.TimeUnits_comboBox);
            this.WaveformPlots_groupBox.ForeColor = System.Drawing.Color.Blue;
            this.WaveformPlots_groupBox.Location = new System.Drawing.Point(12, 797);
            this.WaveformPlots_groupBox.Name = "WaveformPlots_groupBox";
            this.WaveformPlots_groupBox.Size = new System.Drawing.Size(1179, 176);
            this.WaveformPlots_groupBox.TabIndex = 3;
            this.WaveformPlots_groupBox.TabStop = false;
            this.WaveformPlots_groupBox.Text = "Waveforms";
            this.WaveformPlots_groupBox.Enter += new System.EventHandler(this.WaveformPlots_groupBox_Enter);
            // 
            // ClearWaves_button
            // 
            this.ClearWaves_button.ForeColor = System.Drawing.Color.Red;
            this.ClearWaves_button.Location = new System.Drawing.Point(71, 141);
            this.ClearWaves_button.Name = "ClearWaves_button";
            this.ClearWaves_button.Size = new System.Drawing.Size(108, 23);
            this.ClearWaves_button.TabIndex = 4;
            this.ClearWaves_button.Text = "Delete all curves";
            this.ClearWaves_button.UseVisualStyleBackColor = true;
            this.ClearWaves_button.Click += new System.EventHandler(this.ClearWaves_button_Click);
            // 
            // DeleteWave_button
            // 
            this.DeleteWave_button.ForeColor = System.Drawing.Color.Red;
            this.DeleteWave_button.Location = new System.Drawing.Point(71, 114);
            this.DeleteWave_button.Name = "DeleteWave_button";
            this.DeleteWave_button.Size = new System.Drawing.Size(108, 23);
            this.DeleteWave_button.TabIndex = 3;
            this.DeleteWave_button.Text = "Delete active curve";
            this.DeleteWave_button.UseVisualStyleBackColor = true;
            this.DeleteWave_button.Click += new System.EventHandler(this.DeleteWave_button_Click);
            // 
            // Meas_control_groupBox
            // 
            this.Meas_control_groupBox.Controls.Add(this.UseFitPoly_checkBox);
            this.Meas_control_groupBox.Controls.Add(this.ClearMeas_button);
            this.Meas_control_groupBox.Controls.Add(this.DeleteMeas_button);
            this.Meas_control_groupBox.Controls.Add(this.ActiveMeas_comboBox);
            this.Meas_control_groupBox.Controls.Add(this.DisplayMeas_checkedListBox);
            this.Meas_control_groupBox.Controls.Add(this.AddMeas_button);
            this.Meas_control_groupBox.Controls.Add(this.label4);
            this.Meas_control_groupBox.Controls.Add(this.label3);
            this.Meas_control_groupBox.Controls.Add(this.Meas_HighGate_t1_numericUpDown);
            this.Meas_control_groupBox.Controls.Add(this.Meas_HighGate_t0_numericUpDown);
            this.Meas_control_groupBox.Controls.Add(this.Meas_LowGate_t1_numericUpDown);
            this.Meas_control_groupBox.Controls.Add(this.label2);
            this.Meas_control_groupBox.Controls.Add(this.Meas_LowGate_t0_numericUpDown);
            this.Meas_control_groupBox.Controls.Add(this.label1);
            this.Meas_control_groupBox.Location = new System.Drawing.Point(185, 13);
            this.Meas_control_groupBox.Name = "Meas_control_groupBox";
            this.Meas_control_groupBox.Size = new System.Drawing.Size(713, 157);
            this.Meas_control_groupBox.TabIndex = 2;
            this.Meas_control_groupBox.TabStop = false;
            this.Meas_control_groupBox.Text = "Measurements";
            this.Meas_control_groupBox.Enter += new System.EventHandler(this.Meas_control_groupBox_Enter);
            // 
            // UseFitPoly_checkBox
            // 
            this.UseFitPoly_checkBox.AutoSize = true;
            this.UseFitPoly_checkBox.ForeColor = System.Drawing.Color.Black;
            this.UseFitPoly_checkBox.Location = new System.Drawing.Point(9, 75);
            this.UseFitPoly_checkBox.Name = "UseFitPoly_checkBox";
            this.UseFitPoly_checkBox.Size = new System.Drawing.Size(67, 17);
            this.UseFitPoly_checkBox.TabIndex = 13;
            this.UseFitPoly_checkBox.Text = "Fit signal";
            this.UseFitPoly_checkBox.UseVisualStyleBackColor = true;
            this.UseFitPoly_checkBox.CheckedChanged += new System.EventHandler(this.UseFitPoly_checkBox_CheckedChanged);
            // 
            // ClearMeas_button
            // 
            this.ClearMeas_button.ForeColor = System.Drawing.Color.Red;
            this.ClearMeas_button.Location = new System.Drawing.Point(117, 128);
            this.ClearMeas_button.Name = "ClearMeas_button";
            this.ClearMeas_button.Size = new System.Drawing.Size(128, 23);
            this.ClearMeas_button.TabIndex = 12;
            this.ClearMeas_button.Text = "Clear all measurements";
            this.ClearMeas_button.UseVisualStyleBackColor = true;
            this.ClearMeas_button.Click += new System.EventHandler(this.ClearMeas_button_Click);
            // 
            // DeleteMeas_button
            // 
            this.DeleteMeas_button.ForeColor = System.Drawing.Color.Red;
            this.DeleteMeas_button.Location = new System.Drawing.Point(117, 101);
            this.DeleteMeas_button.Name = "DeleteMeas_button";
            this.DeleteMeas_button.Size = new System.Drawing.Size(128, 23);
            this.DeleteMeas_button.TabIndex = 11;
            this.DeleteMeas_button.Text = "Delete measurement";
            this.DeleteMeas_button.UseVisualStyleBackColor = true;
            this.DeleteMeas_button.Click += new System.EventHandler(this.DeleteMeas_button_Click);
            // 
            // ActiveMeas_comboBox
            // 
            this.ActiveMeas_comboBox.FormattingEnabled = true;
            this.ActiveMeas_comboBox.Location = new System.Drawing.Point(518, 18);
            this.ActiveMeas_comboBox.Name = "ActiveMeas_comboBox";
            this.ActiveMeas_comboBox.Size = new System.Drawing.Size(121, 21);
            this.ActiveMeas_comboBox.TabIndex = 10;
            this.ActiveMeas_comboBox.Text = "Active measurement";
            this.ActiveMeas_comboBox.SelectedIndexChanged += new System.EventHandler(this.ActiveMeas_comboBox_SelectedIndexChanged);
            // 
            // DisplayMeas_checkedListBox
            // 
            this.DisplayMeas_checkedListBox.FormattingEnabled = true;
            this.DisplayMeas_checkedListBox.Location = new System.Drawing.Point(423, 12);
            this.DisplayMeas_checkedListBox.Name = "DisplayMeas_checkedListBox";
            this.DisplayMeas_checkedListBox.Size = new System.Drawing.Size(89, 139);
            this.DisplayMeas_checkedListBox.TabIndex = 9;
            this.DisplayMeas_checkedListBox.SelectedIndexChanged += new System.EventHandler(this.DisplayMeas_checkedListBox_SelectedIndexChanged);
            // 
            // AddMeas_button
            // 
            this.AddMeas_button.ForeColor = System.Drawing.Color.Purple;
            this.AddMeas_button.Location = new System.Drawing.Point(6, 101);
            this.AddMeas_button.Name = "AddMeas_button";
            this.AddMeas_button.Size = new System.Drawing.Size(105, 23);
            this.AddMeas_button.TabIndex = 8;
            this.AddMeas_button.Text = "Add measurement";
            this.AddMeas_button.UseVisualStyleBackColor = true;
            this.AddMeas_button.Click += new System.EventHandler(this.AddMeas_button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Teal;
            this.label4.Location = new System.Drawing.Point(257, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "to:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Teal;
            this.label3.Location = new System.Drawing.Point(49, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Signal from:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Meas_HighGate_t1_numericUpDown
            // 
            this.Meas_HighGate_t1_numericUpDown.Location = new System.Drawing.Point(286, 45);
            this.Meas_HighGate_t1_numericUpDown.Name = "Meas_HighGate_t1_numericUpDown";
            this.Meas_HighGate_t1_numericUpDown.Size = new System.Drawing.Size(127, 20);
            this.Meas_HighGate_t1_numericUpDown.TabIndex = 5;
            this.Meas_HighGate_t1_numericUpDown.ValueChanged += new System.EventHandler(this.Meas_HighGate_t1_numericUpDown_ValueChanged);
            // 
            // Meas_HighGate_t0_numericUpDown
            // 
            this.Meas_HighGate_t0_numericUpDown.Location = new System.Drawing.Point(118, 45);
            this.Meas_HighGate_t0_numericUpDown.Name = "Meas_HighGate_t0_numericUpDown";
            this.Meas_HighGate_t0_numericUpDown.Size = new System.Drawing.Size(127, 20);
            this.Meas_HighGate_t0_numericUpDown.TabIndex = 4;
            this.Meas_HighGate_t0_numericUpDown.ValueChanged += new System.EventHandler(this.Meas_HighGate_t0_numericUpDown_ValueChanged);
            // 
            // Meas_LowGate_t1_numericUpDown
            // 
            this.Meas_LowGate_t1_numericUpDown.Location = new System.Drawing.Point(286, 19);
            this.Meas_LowGate_t1_numericUpDown.Name = "Meas_LowGate_t1_numericUpDown";
            this.Meas_LowGate_t1_numericUpDown.Size = new System.Drawing.Size(127, 20);
            this.Meas_LowGate_t1_numericUpDown.TabIndex = 3;
            this.Meas_LowGate_t1_numericUpDown.ValueChanged += new System.EventHandler(this.Meas_LowGate_t1_numericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Teal;
            this.label2.Location = new System.Drawing.Point(257, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "to:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // Meas_LowGate_t0_numericUpDown
            // 
            this.Meas_LowGate_t0_numericUpDown.Location = new System.Drawing.Point(118, 20);
            this.Meas_LowGate_t0_numericUpDown.Name = "Meas_LowGate_t0_numericUpDown";
            this.Meas_LowGate_t0_numericUpDown.Size = new System.Drawing.Size(127, 20);
            this.Meas_LowGate_t0_numericUpDown.TabIndex = 1;
            this.Meas_LowGate_t0_numericUpDown.ValueChanged += new System.EventHandler(this.Meas_LowGate_t0_numericUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Teal;
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zero level from:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ActiveCurve_comboBox
            // 
            this.ActiveCurve_comboBox.ForeColor = System.Drawing.Color.Purple;
            this.ActiveCurve_comboBox.FormattingEnabled = true;
            this.ActiveCurve_comboBox.Location = new System.Drawing.Point(87, 19);
            this.ActiveCurve_comboBox.Name = "ActiveCurve_comboBox";
            this.ActiveCurve_comboBox.Size = new System.Drawing.Size(92, 21);
            this.ActiveCurve_comboBox.TabIndex = 1;
            this.ActiveCurve_comboBox.Text = "Active curve";
            this.ActiveCurve_comboBox.SelectedIndexChanged += new System.EventHandler(this.ActiveCurve_comboBox_SelectedIndexChanged);
            // 
            // TimeUnits_comboBox
            // 
            this.TimeUnits_comboBox.FormattingEnabled = true;
            this.TimeUnits_comboBox.Location = new System.Drawing.Point(6, 19);
            this.TimeUnits_comboBox.Name = "TimeUnits_comboBox";
            this.TimeUnits_comboBox.Size = new System.Drawing.Size(75, 21);
            this.TimeUnits_comboBox.TabIndex = 0;
            this.TimeUnits_comboBox.Tag = "";
            this.TimeUnits_comboBox.Text = "Time units";
            this.TimeUnits_comboBox.SelectedIndexChanged += new System.EventHandler(this.TimeUnits_comboBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 985);
            this.Controls.Add(this.WaveformPlots_groupBox);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.FileRead_progressBar);
            this.Controls.Add(this.OpenSignal_button);
            this.Name = "Form1";
            this.Text = "SignalScope";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.WaveformPlots_groupBox.ResumeLayout(false);
            this.Meas_control_groupBox.ResumeLayout(false);
            this.Meas_control_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Meas_HighGate_t1_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Meas_HighGate_t0_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Meas_LowGate_t1_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Meas_LowGate_t0_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OpenSignal_button;
        private System.Windows.Forms.ProgressBar FileRead_progressBar;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.GroupBox WaveformPlots_groupBox;
        private System.Windows.Forms.ComboBox TimeUnits_comboBox;
        private System.Windows.Forms.ComboBox ActiveCurve_comboBox;
        private System.Windows.Forms.GroupBox Meas_control_groupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Meas_LowGate_t0_numericUpDown;
        private System.Windows.Forms.NumericUpDown Meas_LowGate_t1_numericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown Meas_HighGate_t0_numericUpDown;
        private System.Windows.Forms.NumericUpDown Meas_HighGate_t1_numericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button AddMeas_button;
        private System.Windows.Forms.CheckedListBox DisplayMeas_checkedListBox;
        private System.Windows.Forms.ComboBox ActiveMeas_comboBox;
        private System.Windows.Forms.Button DeleteMeas_button;
        private System.Windows.Forms.Button DeleteWave_button;
        private System.Windows.Forms.Button ClearMeas_button;
        private System.Windows.Forms.Button ClearWaves_button;
        private System.Windows.Forms.CheckBox UseFitPoly_checkBox;
    }
}

