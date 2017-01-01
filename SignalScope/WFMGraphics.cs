using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZedGraph;

namespace SignalScope
{
    /// <summary>
    /// Graphical representation of a WFMeasurement object.
    /// Atached to the curve of the waveform.
    /// </summary>
    class WFMGraphics
    {
        // Properties
        public CurveItem WFcurve
        { get; set; }

        // Private members
        private string[] MeasTags = { "Offset mean", "Offset RMS", "Signal mean", "Signal RMS", "SNR", "DSNR", "Front 10%-90%" };

        private bool[] MeasFlags = { false, false, false, false, false, false, false };


        private LineObj MarginLowGate_left
        { get; set; }

        private LineObj MarginLowGate_right
        { get; set; }

        private LineObj MarginHighGate_left
        { get; set; }

        private LineObj MarginHighGate_right
        { get; set; }

        /// <summary>
        /// Arrow from the text box pointing to the curve. This one corresponds to the 'LowGate' measurements.
        /// </summary>
        private ArrowObj ArrowLowGate
        { get; set; }

        /// <summary>
        /// Arrow from the text box pointing to the curve. This one corresponds to the 'HighGate' measurements.
        /// </summary>
        private ArrowObj ArrowHighGate
        { get; set; }

        /// <summary>
        /// Text for the 'LowGate' measurements
        /// </summary>
        private TextObj TextLowGate
        { get; set; }

        /// <summary>
        /// Text for the 'HighGate' measurements
        /// </summary>
        private TextObj TextHighGate
        { get; set; }

        /// <summary>
        /// Coordinates on the curve where ArrowLowGate points from.
        /// </summary>
        private PointPair Coord_from_ArrowLowGate
        { get; set; }

        /// <summary>
        /// Coordinates on the curve where ArrowLowGate points to.
        /// </summary>
        private PointPair Coord_to_ArrowLowGate
        { get; set; }

        /// <summary>
        /// Coordinates on the curve where ArrowHighGate points from.
        /// </summary>
        private PointPair Coord_from_ArrowHighGate
        { get; set; }

        /// <summary>
        /// Coordinates on the curve where ArrowHighGate points to.
        /// </summary>
        private PointPair Coord_to_ArrowHighGate
        { get; set; }

        private bool isShowLowGateMeas
        { get; set; }

        private bool isShowHighGateMeas
        { get; set; }

        /// <summary>
        /// Ctor with parameters
        /// </summary>
        public WFMGraphics(WFMeasurement WFMeas, CurveItem crv)
        {
            WFcurve = crv;

            // Assembly text of measurement
        }



    }
}
