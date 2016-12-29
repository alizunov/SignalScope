using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalScope
{
    /// <summary>
    /// Usable measurements on a waveform:
    /// - mean;
    /// - rms;
    /// - stdev (noise)
    /// - ...
    /// All measurements are gated.
    /// </summary>

    class WFMeasurement
    {
        /// <summary>
        /// Types of measurements:
        ///  - average : just average over samples within the gate
        ///  - mean : average over (sample - offset)
        /// </summary>
        public string[] AvailableMeasurement = new string[] { "average",
            "offset",
            "mean",
            "RMS",
            "stdev",
            "SNR"
        };

        // Properties
        public double t0_gate
        { get; set; }

        public double t1_gate
        { get; set; }

        public string TypeOfMeasurement
        { get; set; }

        /// <summary>
        /// True: use fit polyN to calculate stdev and SNR.
        /// </summary>
        public bool isFitCurveStdev
        { get; set; }

    }
}
