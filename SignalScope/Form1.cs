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
        Waveform[] ScopeChannels;
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
                        // Insert code to read the stream here.
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
                        List<string> WaveText = new List<string>();
                        System.IO.StreamReader stReader = new System.IO.StreamReader(myStream);
                        string str1 = "";
                        while ( (str1 = stReader.ReadLine()) != null )
                        {
                            char[] delimiterChars = { ' ', ',', ':', '\t' };
                            string[] words = str1.Split(delimiterChars);
                            if (words[0] == "LECROYMAUI")
                            {
                                stReader.ReadLine();
                                stReader.ReadLine();
                                stReader.ReadLine();
                                str1 = stReader.ReadLine(); // Time, Ampl
                                words = str1.Split(delimiterChars);
                            }
                            else if (words[0] == "SomethingElse")
                            {
                                // Skip several lines.
                                //str1 = stReader.ReadLine(); // Time, Ampl
                                //words = str1.Split(delimiterChars);
                            }
                            else
                            {
                                int Nsignals = words.Length - 1;
                                // Continue here ..
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
