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

        /// <summary>
        /// Fill color for time gate boxes.
        /// </summary>
        static private System.Drawing.Color Color_GateBox_fill = System.Drawing.Color.Beige;

        private BoxObj BoxLowGate
        { get; set; }

        private BoxObj BoxHighGate
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
        /// Ctor with parameters
        /// </summary>
        public WFMGraphics(WFMeasurement WFMeas, GraphPane gp, CurveItem crv, string[] MeasTags, bool[] MeasFlags)
        {
            WFcurve = crv;

            // Assembly text of measurement
            string mtext_lg = "";
            string mtext_hg = "";
            string pattern = "Offset"; // Will be used to split items between 'Low Gate" and 'High Gate' groups
            for (int it=0; it < MeasTags.Length; it++)
            {
                if ( System.Text.RegularExpressions.Regex.IsMatch(MeasTags[it], pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase) ) // Put text to 'Low Gate'
                    mtext_lg += (MeasFlags[it]) ? MeasTags[it] + " " : "";
                else
                    mtext_hg += (MeasFlags[it]) ? MeasTags[it] + " " : "";
            }
            double xscale = crv.GetXAxis(gp).Scale.Max - crv.GetXAxis(gp).Scale.Min;
            double yscale = crv.GetYAxis(gp).Scale.Max - crv.GetYAxis(gp).Scale.Min;
            double xtext_lg = (WFMeas.t0_LOW_gate + WFMeas.t1_LOW_gate) / 2 + 0.2 * xscale;
            double ytext_lg = WFMeas.ZeroOffset + 0.2 * yscale;
            double xtext_hg = (WFMeas.t0_HIGH_gate + WFMeas.t1_HIGH_gate) / 2 + 0.2 * xscale;
            double ytext_hg = WFMeas.PulseMean + 0.2 * yscale;
            TextLowGate = new TextObj(mtext_lg, xtext_lg, ytext_lg, CoordType.AxisXYScale, AlignH.Center, AlignV.Center);
            // Border ON
            TextLowGate.FontSpec.Border.IsVisible = true;
            TextLowGate.FontSpec.FontColor = crv.Color;
            TextLowGate.FontSpec.Border.Color = crv.Color;
            // Add to the displayed object list
            gp.GraphObjList.Add(TextLowGate);

            // Create boxes displaying measurements time gates
            // Low Gate:
            double bwidth = WFMeas.t1_LOW_gate - WFMeas.t0_LOW_gate;
            double bheight = 0.1 * (crv.GetYAxis(gp).Scale.Max - crv.GetYAxis(gp).Scale.Min);
            double bx_lg = (WFMeas.t0_LOW_gate + WFMeas.t1_LOW_gate) / 2;
            double by_lg = WFMeas.ZeroOffset;
            BoxLowGate = new BoxObj(bx_lg, by_lg, bwidth, bheight, crv.Color, Color_GateBox_fill, Color_GateBox_fill);
            // High Gate:
            bwidth = WFMeas.t1_HIGH_gate - WFMeas.t0_HIGH_gate;
            double bx_hg = (WFMeas.t0_HIGH_gate + WFMeas.t1_HIGH_gate) / 2;
            double by_hg = WFMeas.PulseMean;
            BoxHighGate = new BoxObj(bx_hg, by_hg, bwidth, bheight, crv.Color, Color_GateBox_fill, Color_GateBox_fill);

            // Create arrows
            float ArrowHeadSize = 50;
            ArrowLowGate = new ArrowObj(crv.Color, ArrowHeadSize, xtext_lg, ytext_lg, bx_lg, by_lg);
            ArrowHighGate = new ArrowObj(crv.Color, ArrowHeadSize, xtext_hg, ytext_hg, bx_hg, by_hg);
            // Add to the displayed object list
            gp.GraphObjList.Add(ArrowLowGate);
            gp.GraphObjList.Add(ArrowHighGate);


        }



    }
}
