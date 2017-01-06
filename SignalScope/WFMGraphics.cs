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
        static private System.Drawing.Color Color_GateBox_fill = System.Drawing.Color.Lavender;

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
        /// Polynomial fit witin the specified range in WFMeasurement object
        /// </summary>
        private LineItem FitCurve
        { get; set; }
            
        /// <summary>
        /// Ctor with parameters
        /// </summary>
        public WFMGraphics(WFMeasurement WFMeas, GraphPane gp, CurveItem crv, double TimeScale, string[] MeasTags, bool[] MeasFlags)
        {
            WFcurve = crv;

            // Assembly text of measurement
            // !!! Important: string[] MeasTags must match the list of all measurements defined in the WFMeasurement class !!!
            string mtext_lg = "";
            string mtext_hg = "";
            string pattern = "Offset"; // Will be used to split items between 'Low Gate" and 'High Gate' groups
            int digits = 3;
            for (int it=0; it < MeasTags.Length; it++)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(MeasTags[it], pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)) // Put text to 'Low Gate'
                    mtext_lg += (MeasFlags[it]) ? MeasTags[it] + "=" + Math.Round(WFMeas.AllParams.ElementAt(it), digits).ToString() + "; " : "";
                else
                    mtext_hg += (MeasFlags[it]) ? MeasTags[it] + "=" + Math.Round(WFMeas.AllParams.ElementAt(it), digits).ToString() + "; " : "";
            }
            // Define the pulse sign depending on relation between the offset and the pulse mean:
            double psign = Math.Sign(WFMeas.PulseMean - WFMeas.ZeroOffset);

            double xscale = crv.GetXAxis(gp).Scale.Max - crv.GetXAxis(gp).Scale.Min;
            double yscale = crv.GetYAxis(gp).Scale.Max - crv.GetYAxis(gp).Scale.Min;

            double xtext_lg = WFMeas.t0_LOW_gate * TimeScale + 0.2 * xscale;
            double ytext_lg = WFMeas.ZeroOffset + psign * 0.2 * yscale;
            double xtext_hg = WFMeas.t0_HIGH_gate * TimeScale + 0.2 * xscale;
            double ytext_hg = WFMeas.PulseMean - psign * 0.2 * yscale;

            TextLowGate = new TextObj(mtext_lg, xtext_lg, ytext_lg, CoordType.AxisXYScale, AlignH.Center, AlignV.Center);
            TextLowGate.Tag = WFMeas.Name + " Low Gate text";
            // Border ON
            TextLowGate.FontSpec.Border.IsVisible = true;
            TextLowGate.FontSpec.FontColor = System.Drawing.Color.Black;
            TextLowGate.FontSpec.Border.Color = crv.Color;
            TextLowGate.FontSpec.Size = 6;

            TextHighGate = new TextObj(mtext_hg, xtext_hg, ytext_hg, CoordType.AxisXYScale, AlignH.Center, AlignV.Center);
            TextHighGate.Tag = WFMeas.Name + " High Gate text";
            // Border ON
            TextHighGate.FontSpec.Border.IsVisible = true;
            TextHighGate.FontSpec.FontColor = System.Drawing.Color.Black;
            TextHighGate.FontSpec.Border.Color = crv.Color;
            TextHighGate.FontSpec.Size = 6;
            // Add to the displayed object list
            gp.GraphObjList.Add(TextLowGate);
            gp.GraphObjList.Add(TextHighGate);


            // Create boxes displaying measurements time gates
            // Low Gate:
            double bwidth_lg = Math.Abs((WFMeas.t1_LOW_gate - WFMeas.t0_LOW_gate) * TimeScale);
            double bheight_lg = Math.Abs(0.1 * (crv.GetYAxis(gp).Scale.Max - crv.GetYAxis(gp).Scale.Min));
            double bx_lg = WFMeas.t0_LOW_gate * TimeScale;
            double by_lg = WFMeas.ZeroOffset + bheight_lg / 2;
            BoxLowGate = new BoxObj(bx_lg, by_lg, bwidth_lg, bheight_lg, crv.Color, Color_GateBox_fill, Color_GateBox_fill);
            BoxLowGate.Tag = WFMeas.Name + " Low Gate box";
            // High Gate:
            double bwidth_hg = Math.Abs((WFMeas.t1_HIGH_gate - WFMeas.t0_HIGH_gate) * TimeScale);
            double bheight_hg = bheight_lg;
            double bx_hg = WFMeas.t0_HIGH_gate * TimeScale;
            double by_hg = WFMeas.PulseMean + bheight_hg / 2;
            BoxHighGate = new BoxObj(bx_hg, by_hg, bwidth_hg, bheight_hg, crv.Color, Color_GateBox_fill, Color_GateBox_fill);
            BoxHighGate.Tag = WFMeas.Name + " High Gate box";
            // Add to the displayed object list and
            gp.GraphObjList.Add(BoxLowGate);
            gp.GraphObjList.Add(BoxHighGate);
            BoxLowGate.ZOrder = ZOrder.E_BehindCurves;
            BoxHighGate.ZOrder = ZOrder.E_BehindCurves;
            

            // Create arrows
            float ArrowHeadSize = 10;
            ArrowLowGate = new ArrowObj(crv.Color, ArrowHeadSize, xtext_lg, ytext_lg, bx_lg + 0.05 * xscale, by_lg - bheight_lg / 2);
            ArrowLowGate.Tag = WFMeas.Name + " Low Gate arrow";
            ArrowHighGate = new ArrowObj(crv.Color, ArrowHeadSize, xtext_hg, ytext_hg, bx_hg + 0.05 * xscale, by_hg - bheight_hg / 2);
            ArrowHighGate.Tag = WFMeas.Name + " High Gate arrow";
            // Add to the displayed object list
            gp.GraphObjList.Add(ArrowLowGate);
            gp.GraphObjList.Add(ArrowHighGate);

            // Add curve for the fit polinom if the WFMeasurement.isFitPoly == true
            if (WFMeas.isFitPoly)
            {
                PointPairList fit_ppl = new PointPairList();
                // Need a loop to scale time values (multiply by TimeScale)
                for (int it = 0; it < WFMeas.NpFitRange; it++)
                    fit_ppl.Add(WFMeas.XsigFit.ElementAt(it) * TimeScale, WFMeas.YsigFit.ElementAt(it));
                string fitName = WFMeas.Name + "-fit";
                FitCurve = gp.AddCurve(fitName, fit_ppl, System.Drawing.Color.Black, SymbolType.None);
                FitCurve.Line.Width = 1;
                // Why this doen't work ??
                int index = gp.CurveList.IndexOf(fitName);
                gp.CurveList.Move(index, 1);
            }

            //Console.WriteLine("Low Gate: arrow ({0}, {1}) --> ({2}, {3}); text: " + TextLowGate.Text, xtext_lg, ytext_lg, bx_lg, by_lg);
            //Console.WriteLine("High Gate: arrow ({0}, {1}) --> ({2}, {3}); text: " + TextHighGate.Text, xtext_hg, ytext_hg, bx_hg, by_hg);
            //Console.WriteLine("Low gate box x, y, w, h: {0}, {1}, {2}, {3}.", bx_lg, by_lg, bwidth, bheight);

        }



    }
}
