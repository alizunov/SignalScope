using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignalScope
{
    public partial class Form1 : Form
    {
        // Data members
        Waveform[] waves;


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
                        char[] delimiterChars = { ' ', ',', ':', '\t' };
                        string[] words = str1.Split(delimiterChars);
                        int Nwavefoms = 0;   // Number of waveforms

                        if (words[0] == "LECROYMAUI")   // LeCroy .TRC text file
                        {
                            stReader.ReadLine();
                            stReader.ReadLine();
                            stReader.ReadLine();
                            str1 = stReader.ReadLine(); // Time, Ampl
                            words = str1.Split(delimiterChars);
                            Nwavefoms = words.Length - 1;
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

                        }
                        else // Binary?
                        {
                            MessageBox.Show("Not recognized pattern: binary file? String:  " + str1);
                        }

                        if (Nwavefoms > 0)
                        {
                            List<double>[] cols = new List<double>[Nwavefoms + 1];  // +1 for time column
                            while ( (str1 = stReader.ReadLine()) != null )
                            {
                                words = str1.Split(delimiterChars);
                                for (int icol = 0; icol < words.Length; icol++)
                                    cols[icol].Add(Convert.ToDouble(words[icol]));
                            }
                            // Cont here ..
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
