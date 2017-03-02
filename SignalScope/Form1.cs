using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using ZedGraph;

namespace SignalScope
{
    public partial class Form1 : Form
    {
        // Data members
        List<Waveform> waves;
        List<WFMeasurement> wmeas;
        List<WFMGraphics> wmeasG;
        GraphPane gp;

        Color[] WFMcolor = new Color[] {
            Color.Aquamarine,
            Color.Gold,
            Color.BlueViolet,
            Color.DarkRed,
            Color.Blue,
            Color.Coral,
            Color.CornflowerBlue,
            Color.Crimson,
            Color.Cyan,
            Color.DarkBlue,
            Color.DarkGreen,
            Color.DarkCyan,
            Color.Orange,
            Color.Green,
            Color.Red };

        // Names of individual measurement parameters in the checkedlistbox. Can be displayed or hidden.
        string[] MeasNames = { "Offset mean",
            "Offset RMS",
            "Offset Pk-Pk",
            "Signal mean",
            "Signal RMS",
            "Signal Pk-Pk",
            "SNR",
            "DSNR",
            "Front 10-90%" };

        // List of Y-min (actual) values of all existing curves. Updates upon adding a new curve or modification of an existing one.
        List<double> YminCurves = new List<double>();
        // List of Y-max (actual) values of all existing curves. Updates upon adding a new curve or modification of an existing one.
        List<double> YmaxCurves = new List<double>();

        // Properties
        public double TimeModifier
        { get; set; }

        // *******************************
        // *** Ctor of the main window ***
        public Form1()
        {
            // Methods of Designer support
            InitializeComponent();

            zedGraphControl1.Visible = false;
            WaveformPlots_groupBox.Enabled = false;

            // Create list of waveforms
            waves = new List<Waveform>();

            // Create list of waveform measurements
            wmeas = new List<WFMeasurement>();

            // Create list of Graphs for waveform measurement
            wmeasG = new List<WFMGraphics>();

            // Create Graph Pane
            gp = zedGraphControl1.GraphPane;

            // Definition of time units combobox items
            TimeUnits_comboBox.Items.AddRange(new object[] {"s", "ms", "us", "ns"});
            TimeUnits_comboBox.SelectedIndex = 0;

            // NumericalUpDown entries for gate times
            Meas_LowGate_t0_numericUpDown.DecimalPlaces = 9;
            Meas_LowGate_t1_numericUpDown.DecimalPlaces = 9;
            Meas_HighGate_t0_numericUpDown.DecimalPlaces = 9;
            Meas_HighGate_t1_numericUpDown.DecimalPlaces = 9;

            // Init pane for graphics (title, fonts, axis, etc.)
            InitGraphPane();

            // Fill the checkedlistbox of individual measurement parameters
            DisplayMeas_checkedListBox.Items.AddRange(MeasNames);
            // Default state: Checked
            for (int it = 0; it < DisplayMeas_checkedListBox.Items.Count; it++)
                DisplayMeas_checkedListBox.SetItemChecked(it, true);
            // Changes the selection mode from double-click to single click.
            DisplayMeas_checkedListBox.CheckOnClick = true;

        }
        // *** Ctor of the main window ***
        // *******************************


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Read input file: lets assume this is a text file and read accordingly. Fallback to binary if wrong.
        /// </summary>
        private void OpenSignal_button_Click(object sender, EventArgs e)
        {
            System.IO.Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            // Enable selection of multiple files
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    foreach (string file in openFileDialog1.FileNames)
                    {
                        if ((myStream = File.Open(file, FileMode.Open)) != null)
                        {
                            System.IO.StreamReader stReader = new System.IO.StreamReader(myStream);

                            string str1 = stReader.ReadLine();
                            // Discover the header ..
                            // We can handle the following text records from scopes:
                            // 1. Tektronix's CSV:
                            // s, Volts1, Volts2, Volts3, Volts4
                            // -1.944e-006, 0, 0, 0, 0
                            // ...
                            // 2. LeCroy's TRC:
                            //LECROYMAUI,0,Waveform
                            //Segments,1,SegmentSize,25002
                            //Segment,TrigTime,TimeSinceSegment1
                            //#1,24-Feb-2016 16:34:36,0                 
                            //Time, Ampl1, Ampl2, Ampl3, Ampl4
                            //-5.0003623e-006,-0.03, 0, 0, 0
                            // ...
                            bool isTextFile = true;
                            string pattern = @"\s*\t*,\s*\t*";
                            string[] words = Regex.Split(str1, pattern);
                            int Nwavefoms = 0;   // Number of waveforms

                            if (words[0].Substring(0, 6) == "LECROY")   // LeCroy .TRC or .csv Excel/ASCII file
                            {
                                stReader.ReadLine();
                                stReader.ReadLine();
                                stReader.ReadLine();
                                str1 = stReader.ReadLine(); // Time, Ampl
                                words = Regex.Split(str1, pattern);
                                Nwavefoms = words.Length - 1;
                                Console.WriteLine("Lecroy TRC file header with {0} columns: " + str1, words.Length);
                            }
                            else if (words[0] == "SomethingElse")
                            {
                                // Skip several lines.
                                //str1 = stReader.ReadLine(); // Time, Ampl
                                //words = str1.Split(delimiterChars);
                            }
                            else if (words[0] == "s" && words.Length > 1)   // Tektronix .CSV text file
                            {
                                Nwavefoms = words.Length - 1;
                                Console.WriteLine("Tektronix CSV file header with {0} columns: " + str1, words.Length);
                            }
                            else // Binary?
                            {
                                MessageBox.Show("No text pattern matches, assumed binary file. String:  " + str1);
                                // Add code to parse binary header !
                                isTextFile = false;
                            }

                            if (Nwavefoms > 0)
                            {
                                //MessageBox.Show("Columns in the input file: " + words.Length.ToString() + ", header string: " + str1);
                                List<List<double>> cols = new List<List<double>>();
                                for (int icol = 0; icol < Nwavefoms + 1; icol++)    // +1 for time column
                                    cols.Add(new List<double>());

                                // Get the file size in bytes
                                FileInfo fi = new FileInfo(openFileDialog1.FileName);
                                long FileSize = fi.Length;
                                Console.WriteLine("File size: {0} bytes", FileSize);

                                if (isTextFile)
                                {
                                    int StrSize = 0;
                                    int StrNumPercent = 0;
                                    int StrCount = 0;
                                    // Init the progress bar
                                    FileRead_progressBar.Minimum = 0;
                                    FileRead_progressBar.Maximum = 100;
                                    FileRead_progressBar.Step = 1;
                                    FileRead_progressBar.Value = 0;
                                    // *** Readout TEXT loop ***
                                    while ((str1 = stReader.ReadLine()) != null)
                                    {
                                        if (StrSize == 0)
                                        {
                                            StrSize = str1.Length * sizeof(Char);
                                            StrNumPercent = (int)FileSize / StrSize / 100;
                                        }
                                        if (++StrCount >= StrNumPercent)
                                        {
                                            FileRead_progressBar.PerformStep();
                                            StrCount = 0;
                                        }
                                        words = Regex.Split(str1, pattern);
                                        for (int icol = 0; icol < Nwavefoms + 1; icol++)
                                        {
                                            //Console.WriteLine("Word count: {0}, words: " + words[0] + " " + words[1], words.Length);
                                            cols[icol].Add(Convert.ToDouble(words[icol]));
                                        }
                                    }
                                    // *** Readout TEXT loop ***
                                }
                                else
                                {
                                    MessageBox.Show("Readout of a binary file not implemented yet.");
                                }

                                double tstart = cols.ElementAt(0).ElementAt(0);
                                double tend = cols.ElementAt(0).Last();

                                // Create waveforms and curves
                                for (int iwave = 0; iwave < Nwavefoms; iwave++)
                                {
                                    waves.Add(new Waveform("Wave-" + waves.Count, cols.ElementAt(1), tstart, tend));
                                    AddCurveFromWFM(waves.Last());
                                }
                                // cols can be disposed
                                cols.Clear();
                                Console.WriteLine("{0} waveforms created: {1} samples.", Nwavefoms, waves.Last().Npoints);
                                // Make the ZedGraph visible and WFM groupbox active
                                if (!zedGraphControl1.Visible)
                                    zedGraphControl1.Visible = true;
                                if (!WaveformPlots_groupBox.Enabled)
                                    WaveformPlots_groupBox.Enabled = true;
                            }
                        }
                        myStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void FileRead_progressBar_Click(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void WaveformPlots_groupBox_Enter(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sets ZedGraph pane (axis, title, etc).
        /// </summary>
        private void InitGraphPane()
        {
            // Font sizes
            int labelsXfontSize = 10;
            int labelsYfontSize = 10;

            int titleXFontSize = 12;
            int titleYFontSize = 12;

            int legendFontSize = 6;

            // X axis title and labels fonts
            gp.XAxis.Title.FontSpec.IsUnderline = false;
            gp.XAxis.Title.FontSpec.IsBold = false;
            gp.XAxis.Title.FontSpec.Size = titleXFontSize;
            gp.XAxis.Scale.FontSpec.Size = labelsXfontSize;

            // Y axis title and labels fonts
            gp.YAxis.Title.Text = "Signal, V";
            gp.YAxis.Title.FontSpec.IsUnderline = false;
            gp.YAxis.Title.FontSpec.IsBold = false;
            gp.YAxis.Title.FontSpec.Size = titleYFontSize;
            gp.YAxis.Scale.FontSpec.Size = labelsYfontSize;

            // Pane title
            gp.Title.Text = "";

            // Curve legend
            gp.Legend.FontSpec.Size = legendFontSize;

            // Clear curve list
            gp.CurveList.Clear();
        }

        /// <summary>
        /// Calculates X-min for all curves in the list
        /// </summary>
        double XminCurveList(CurveList crvlist)
        {
            double xmin = Double.MaxValue;
            try
            {
                foreach (CurveItem crv in crvlist)
                {
                    xmin = (crv.Points[0].X < xmin) ? crv.Points[0].X : xmin;
                }
                return xmin;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calculating X-min for curves. Original message: " + ex.Message);
                return Double.MinValue;
            }
        }

        /// <summary>
        /// Calculates X-max for all curves in the list
        /// </summary>
        double XmaxCurveList(CurveList crvlist)
        {
            double xmax = Double.MinValue;
            try
            {
                foreach (CurveItem crv in crvlist)
                {
                    xmax = (crv.Points[crv.Points.Count - 1].X > xmax) ? crv.Points[crv.Points.Count - 1].X : xmax;
                }
                return xmax;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calculating X-max for curves. Original message: " + ex.Message);
                return Double.MaxValue;
            }
        }


        /// <summary>
        /// Add a new curve from the waveform
        /// </summary>
        private void AddCurveFromWFM(Waveform wave)
        {
            try
            {
                PointPairList ppl = new PointPairList();
                for (int ip = 0; ip < wave.Npoints; ip++)
                {
                    double x = wave.t(ip) * TimeModifier;
                    ppl.Add(x, wave.Samples.ElementAt(ip));
                }
                int colindex = (int)gp.CurveList.Count % WFMcolor.Length;
                LineItem crv = gp.AddCurve(wave.WaveFormName, ppl, WFMcolor[colindex], SymbolType.None);
                // Add items to the lists of Y-min and Y-max
                double ymin = Double.MaxValue;
                double ymax = Double.MinValue;
                foreach (PointPair pp in ppl)
                {
                    ymin = (pp.Y < ymin) ? pp.Y : ymin;
                    ymax = (pp.Y > ymax) ? pp.Y : ymax;
                }
                YminCurves.Add(ymin);
                YmaxCurves.Add(ymax);

                crv.Line.Width = 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating new curve from WFM {0}. Original message: " + ex.Message, wave.WaveFormName);
                return;
            }

            // Get new x-min and x-max, update X-axis scale limits
            double xmin = XminCurveList(gp.CurveList);
            double xmax = XmaxCurveList(gp.CurveList);
            gp.XAxis.Scale.Min = xmin;
            gp.XAxis.Scale.Max = xmax;

            // Recalculate y-min and y-max, update Y-axis scale limits
            double c1 = 1.2;
            double c2 = 0.9;
            double Ymin = YminCurves.Min();
            double Ymax = YmaxCurves.Max();
            Ymin *= (Ymin >= 0) ? c2 : c1;
            Ymax *= (Ymax >= 0) ? c1 : c2;
            gp.YAxis.Scale.Min = Ymin;
            gp.YAxis.Scale.Max = Ymax;

            // Update axes and graph pane
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

            // Add item to the actrive curve combobox
            ActiveCurve_comboBox.Items.Add(gp.CurveList.Last().Label.Text);
            // and make the last added curve active
            ActiveCurve_comboBox.SelectedItem = gp.CurveList.Last().Label.Text;
        }

        /// <summary>
        /// Modify (scale) a curve
        /// </summary>
        private void ModCurve(CurveItem curve, string NewName, double Xscale, double Yscale, Color NewColor, double NewWidth)
        {
            try
            {
                // Change label (legend entity) if NewName is not empty
                curve.Label.Text = (NewName == "") ? curve.Label.Text : NewName;
                // Change point pair list
                PointPairList ppl = new PointPairList();
                for (int ip = 0; ip < curve.Points.Count; ip++)
                {
                    ppl.Add(curve.Points[ip].X * Xscale, curve.Points[ip].Y * Yscale);
                }
                curve.Points = ppl;

                // Change color and width if not zero
                curve.Color = (NewColor.IsSystemColor) ? NewColor : curve.Color;
                if (curve.IsLine)
                {
                    LineItem li = (LineItem)curve;
                    li.Line.Width = (NewWidth > 0) ? (float)NewWidth : li.Line.Width;
                }
                else
                    Console.WriteLine("Error modifying curve: {0} is not a LineItem object", curve.Label.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error modifying curve {0}. Original message: " + ex.Message, curve.Label.Text);
            }

            // Get new x-min and x-max, update X-axis scale limits
            double xmin = XminCurveList(gp.CurveList);
            double xmax = XmaxCurveList(gp.CurveList);
            gp.XAxis.Scale.Min = xmin;
            gp.XAxis.Scale.Max = xmax;

            // Recalculate y-min and y-max, update Y-axis scale limits
            double c1 = 1.2;
            double c2 = 0.9;
            double Ymin = YminCurves.Min();
            double Ymax = YmaxCurves.Max();
            Ymin *= (Ymin >= 0) ? c2 : c1;
            Ymax *= (Ymax >= 0) ? c1 : c2;
            gp.YAxis.Scale.Min = Ymin;
            gp.YAxis.Scale.Max = Ymax;

            // Update axes and graph pane
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        /// <summary>
        /// Sets left/right limits for each of 4 'Gate' fields from the curve X-axis scale
        /// </summary>
        private void SetGateMarginsFromCurveXaxis(CurveItem crv, bool isSetValue)
        {
            // Scale measurement 'Gate' time margins for the active curve
            Meas_LowGate_t0_numericUpDown.Minimum = Convert.ToDecimal(crv.GetXAxis(gp).Scale.Min);
            Meas_LowGate_t0_numericUpDown.Maximum = Convert.ToDecimal(crv.GetXAxis(gp).Scale.Max);
            Meas_LowGate_t0_numericUpDown.Increment = Convert.ToDecimal(1 / crv.Points.Count);
            if (isSetValue)
                Meas_LowGate_t0_numericUpDown.Value = Meas_LowGate_t0_numericUpDown.Minimum;
            Meas_LowGate_t1_numericUpDown.Minimum = Convert.ToDecimal(crv.GetXAxis(gp).Scale.Min);
            Meas_LowGate_t1_numericUpDown.Maximum = Convert.ToDecimal(crv.GetXAxis(gp).Scale.Max);
            Meas_LowGate_t1_numericUpDown.Increment = Convert.ToDecimal(1 / crv.Points.Count);

            //MessageBox.Show("Min: " + Meas_LowGate_t0_numericUpDown.Minimum.ToString() + ", max: " + Meas_LowGate_t0_numericUpDown.Maximum.ToString());

            Meas_HighGate_t0_numericUpDown.Minimum = Convert.ToDecimal(crv.GetXAxis(gp).Scale.Min);
            Meas_HighGate_t0_numericUpDown.Maximum = Convert.ToDecimal(crv.GetXAxis(gp).Scale.Max);
            Meas_HighGate_t0_numericUpDown.Increment = Convert.ToDecimal(1 / crv.Points.Count);
            Meas_HighGate_t1_numericUpDown.Minimum = Convert.ToDecimal(crv.GetXAxis(gp).Scale.Min);
            Meas_HighGate_t1_numericUpDown.Maximum = Convert.ToDecimal(crv.GetXAxis(gp).Scale.Max);
            Meas_HighGate_t1_numericUpDown.Increment = Convert.ToDecimal(1 / crv.Points.Count);
            if (isSetValue)
                Meas_HighGate_t1_numericUpDown.Value = Convert.ToDecimal(crv.GetXAxis(gp).Scale.Max);

        }


        /// <summary>
        /// Update time axis labels and title depending on units chosen
        /// </summary>
        private void TimeUnits_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Calculate the time scaling coeff from the previous time modifier and a new one
            double TimeModifierPrev = (TimeModifier != 0) ? TimeModifier : 1;
            TimeModifier = Math.Pow(10, 3 * TimeUnits_comboBox.SelectedIndex);
            double TimeScalingCoeff = TimeModifier / TimeModifierPrev;
            // Change Time axis text
            gp.XAxis.Title.Text = "Time, " + TimeUnits_comboBox.SelectedItem;
            // Rescale time axes for all waveforms
            foreach (CurveItem crv in gp.CurveList)
            {
                ModCurve(crv, "", TimeScalingCoeff, 1, crv.Color, 0);
                //Console.WriteLine("ModCurve: curve {0}, time mod = {1}, prev = {2}, time scale = {3}", crv.Label.Text, TimeModifier, TimeModifierPrev, TimeScalingCoeff);
            }
            // Scale measurement 'Gate' time margins for the active curve (if there is acive curve)
            if (ActiveCurve_comboBox.SelectedItem != null)
            {
                string name = ActiveCurve_comboBox.SelectedItem.ToString();
                CurveItem acrv = FindCurveByName(name, gp.CurveList);
                SetGateMarginsFromCurveXaxis(acrv, true);
            }
            // Update graph pane
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

        }

        /// <summary>
        /// Searches curves in the list by name
        /// </summary>
        private CurveItem FindCurveByName(string crv_name, CurveList crv_list)
        {
            try
            {
                foreach (CurveItem crv in crv_list)
                    if (crv.Label.Text == crv_name)
                        return crv;
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FindCurveByName: error. Original message: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Searches curves in the list by name
        /// </summary>
        private Waveform FindWaveByName(string wave_name, List<Waveform> wfms)
        {
            try
            {
                foreach (Waveform wv in wfms) 
                    if (wv.WaveFormName == wave_name)
                        return wv;
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FindWaveByName: error. Original message: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Searches WFMeasurement in the list by name
        /// </summary>
        private WFMeasurement FindMeasByName(string meas_name, List<WFMeasurement> wfms)
        {
            try
            {
                foreach (WFMeasurement wv in wfms) 
                    if (wv.Name == meas_name)
                        return wv;
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FindMeasByName: error. Original message: " + ex.Message);
                return null;
            }
        }

        private void ActiveCurve_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get active curve by its name
            CurveItem acrv = FindCurveByName(ActiveCurve_comboBox.SelectedItem.ToString(), gp.CurveList);

            // Set limits
            bool isSetValue = (Meas_LowGate_t0_numericUpDown.Value == 0 && Meas_HighGate_t1_numericUpDown.Value == 0) ? true : false;
            SetGateMarginsFromCurveXaxis(acrv, isSetValue);

        }

        private void Meas_control_groupBox_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Meas_LowGate_t0_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Meas_LowGate_t1_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Meas_HighGate_t0_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Meas_HighGate_t1_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void AddMeas_button_Click(object sender, EventArgs e)
        {
            try
            {
                List<bool> flags = new List<bool>();
                flags.Clear();
                for (int it = 0; it < DisplayMeas_checkedListBox.Items.Count; it++)
                {
                    bool state = (DisplayMeas_checkedListBox.GetItemCheckState(it) == CheckState.Checked) ? true : false;
                    flags.Add(state);
                }

                // Check the current time modifier:
                // = 1 - times are set in s
                // = 10^3 - times are set in ms
                // ..
                // Displayed values will be scaled upon change of time units (time modifier)
                double t0_lg = Convert.ToDouble(Meas_LowGate_t0_numericUpDown.Value);
                double t1_lg = Convert.ToDouble(Meas_LowGate_t1_numericUpDown.Value);
                double t0_hg = Convert.ToDouble(Meas_HighGate_t0_numericUpDown.Value);
                double t1_hg = Convert.ToDouble(Meas_HighGate_t1_numericUpDown.Value);

                // Find active Waveform by the name of the active CURVE (from combo)
                Waveform wave = FindWaveByName(ActiveCurve_comboBox.SelectedItem.ToString(), waves);

                // Add measurement
                bool fitFlag = UseFitPoly_checkBox.Checked;
                // Order of the fit polynom
                double fitNpoly = 9;
                wmeas.Add(new WFMeasurement(t0_lg, t1_lg, t0_hg, t1_hg, TimeModifier, wave, wmeas.Count, fitFlag, fitNpoly));

                //MessageBox.Show("Waveform " + wave.WaveFormName
                //    + ": offset = " + wmeas.Last().ZeroOffset.ToString()
                //    + ", pulse mean = " + wmeas.Last().PulseMean.ToString());

                // Add graphics
                wmeasG.Add(new WFMGraphics(wmeas.Last(), gp, FindCurveByName(ActiveCurve_comboBox.SelectedItem.ToString(), gp.CurveList), TimeModifier, MeasNames, flags.ToArray()));

                // Update graph pane
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();

                // Update ActiveMeas_comboBox
                ActiveMeas_comboBox.Items.Add(wmeas.Last().Name);
                // Select last added
                ActiveMeas_comboBox.SelectedItem = wmeas.Last().Name;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Add measurement: error. Original message: " + ex.Message);
            }
            
        }

        private void DisplayMeas_checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ActiveMeas_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Deletes all measurement objects, updates lists and comboboxes
        /// </summary>
        private void DeleteMeas(WFMeasurement meas)
        {
            try
            {
                if (!wmeas.Contains(meas))
                {
                    MessageBox.Show("No measurement " + meas.Name + " in the list.");
                    return;
                }
                string base_name = meas.Name;
                if (base_name == "")
                    return;
                // Delete graph objects:
                for (int igo = gp.GraphObjList.Count - 1; igo >= 0; igo--)
                {
                    GraphObj go = gp.GraphObjList.ElementAt(igo);
                    string tag = go.Tag.ToString();
                    if (tag.Contains(base_name))
                        gp.GraphObjList.RemoveAt(igo);
                }
                // Update graph pane
                zedGraphControl1.Invalidate();

                // Remove WFMeasurement from the list
                int mindex = wmeas.IndexOf(meas);
                wmeas.Remove(meas);
                // Remove WFMGraphics
                wmeasG.RemoveAt(mindex);
                // Update ActiveMeas_comboBox
                ActiveMeas_comboBox.Items.RemoveAt(mindex);
                int newindex = (ActiveMeas_comboBox.Items.Count > 0) ? ActiveMeas_comboBox.Items.Count - 1 : -1;
                ActiveMeas_comboBox.SelectedIndex = newindex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete measurement error. Original message: " + ex.Message);
            }

        }

        private void DeleteMeas_button_Click(object sender, EventArgs e)
        {
            if (wmeas.Count == 0)
            {
                MessageBox.Show("List of measurements is empty.");
                return;
            }
            WFMeasurement meas = FindMeasByName(ActiveMeas_comboBox.SelectedItem.ToString(), wmeas);
            DeleteMeas(meas);
        }

        /// <summary>
        /// Deletes all measurement objects, updates lists and comboboxes
        /// </summary>
        private void DeleteWave(Waveform wave)
        {
            if (wave == null)
            {
                MessageBox.Show("Delete waveform error: null waveform.");
                return;
            }
            try
            {
                if (!waves.Contains(wave))
                {
                    MessageBox.Show("No waveform " + wave.WaveFormName + " in the list.");
                    return;
                }
                // Delete all measurements associated with this waveform
                for (int iw = wmeas.Count - 1; iw >= 0; iw--)
                {
                    if (wmeas.ElementAt(iw).Name.Contains(wave.WaveFormName))
                        DeleteMeas(wmeas.ElementAt(iw));
                }
                // Delete curve
                CurveItem crv = FindCurveByName(wave.WaveFormName, gp.CurveList);
                // Update graph pane
                zedGraphControl1.Invalidate();

                if (crv != null)
                {
                    gp.CurveList.Remove(crv);
                }
                // Update the waveform list
                int windex = waves.IndexOf(wave);
                waves.Remove(wave);
                // Update the combobox
                ActiveCurve_comboBox.Items.RemoveAt(windex);
                ActiveCurve_comboBox.SelectedIndex = (ActiveCurve_comboBox.Items.Count > 0) ? ActiveCurve_comboBox.Items.Count - 1 : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete waveform error. Original message: " + ex.Message);
            }

        }


        private void DeleteWave_button_Click(object sender, EventArgs e)
        {
            string wave_name = ActiveCurve_comboBox.SelectedItem.ToString();
            DeleteWave(FindWaveByName(wave_name, waves));
        }

        private void ClearMeas_button_Click(object sender, EventArgs e)
        {
            for (int im = wmeas.Count - 1; im >= 0; im--)
                DeleteMeas(wmeas.ElementAt(im));
        }

        private void ClearWaves_button_Click(object sender, EventArgs e)
        {
            for (int iw = waves.Count - 1; iw >= 0; iw--)
                DeleteWave(waves.ElementAt(iw));
        }

        private void UseFitPoly_checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SavePNG_button_Click(object sender, EventArgs e)
        {
            zedGraphControl1.SaveAs();
        }

        private void ActiveCurve_SetColor_button_Click(object sender, EventArgs e)
        {
            ColorDialog AColorDialog = new ColorDialog();
            AColorDialog.AllowFullOpen = true;
            // Allows the user to get help. (The default is false.)
            AColorDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            AColorDialog.Color = Color.Red;

            // Update the text box color if the user clicks OK 
            if (AColorDialog.ShowDialog() == DialogResult.OK)
            {
                string wave_name = ActiveCurve_comboBox.SelectedItem.ToString();
                CurveItem crv = FindCurveByName(wave_name, gp.CurveList);
                crv.Color = AColorDialog.Color;
                // Update graph pane
                zedGraphControl1.Invalidate();
           }


        }
    }
}
