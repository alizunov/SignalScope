using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignalScope
{
    public partial class Form1 : Form
    {
        // Data members
        List<Waveform> waves;


        // *******************************
        // *** Ctor of the main window ***
        public Form1()
        {
            InitializeComponent();
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

            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
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
                        string pattern = @"[,\s*]";
                        string[] words = Regex.Split(str1, pattern);
                        int Nwavefoms = 0;   // Number of waveforms

                        if (words[0] == "LECROYMAUI")   // LeCroy .TRC text file
                        {
                            stReader.ReadLine();
                            stReader.ReadLine();
                            stReader.ReadLine();
                            str1 = stReader.ReadLine(); // Time, Ampl
                            words = Regex.Split(str1, pattern);
                            Nwavefoms = words.Length - 1;
                            Console.WriteLine("Presumably, a Lecroy TRC file header with {0} columns: " + str1, words.Length);
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
                            Console.WriteLine("Presumably, a Tektronix CSV file header with {0} columns: " + str1, words.Length);
                        }
                        else // Binary?
                        {
                            MessageBox.Show("Not recognized pattern: binary file? String:  " + str1);
                        }

                        if (Nwavefoms > 0)
                        {
                            //MessageBox.Show("Columns in the input file: " + words.Length.ToString() + ", header string: " + str1);
                            List< List<double> > cols = new List< List<double> >();
                            for (int icol = 0; icol < Nwavefoms + 1; icol++)    // +1 for time column
                                cols.Add(new List<double>());
                            while ( (str1 = stReader.ReadLine()) != null )
                            {
                                words = Regex.Split(str1, pattern);
                                for (int icol = 0; icol < Nwavefoms + 1; icol++)
                                {
                                    Console.WriteLine("Readout: " + str1);
                                    cols[icol].Add(Convert.ToDouble(words[icol]));
                                }
                            }
                            double tstart = cols.ElementAt(0).ElementAt(0);
                            double tend = cols.ElementAt(0).Last();
                            for (int iwave=0; iwave<Nwavefoms; iwave++)
                            {
                                // Creating Waveform objects
                                waves[iwave] = new Waveform("Waveform " + iwave.ToString(), cols.ElementAt(iwave + 1), tstart, tend);
                            }
                            // cols can be disposed
                            cols.Clear();
                        }
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
    }
}
