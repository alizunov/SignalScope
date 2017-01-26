using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics;

namespace SignalScope
{
    /// <summary>
    /// Usable measurements on a waveform:
    ///- zero offset and RMS noise (gated);
    ///- mean level or fit polyN (gated) and RMS noise;
    ///- front (?) maybe later
    /// </summary>

    class WFMeasurement
    {
        // Properties
        public string Name
        { get; set; }
        public double t0_LOW_gate
        { get; set; }

        public double t1_LOW_gate
        { get; set; }

        public double t0_HIGH_gate
        { get; set; }

        public double t1_HIGH_gate
        { get; set; }

        public double ZeroOffset
        { get; set; }

        public double ZeroNoiseRMS
        { get; set; }

        public double ZeroNoisePkPk
        { get; set; }

        /// <summary>
        /// Mean value for the 'flat' case (if isFitPoly = false)
        /// </summary>
        public double PulseMean
        { get; set; }

        public double PulseNoiseRMS
        { get; set; }

        public double PulseNoisePkPk
        { get; set; }

        /// <summary>
        /// Signal to Noise Ratio SNR = PulseMean / NoiseRMS
        /// </summary>
        public double SNR
        { get; set; }
        
        /// <summary>
        /// Delta Signal to Noise Ratio DSNR = (PulseMean - ZeroOffset)/ NoiseRMS
        /// </summary>
        public double DSNR
        { get; set; }

        /// <summary>
        /// Transition time between signal levels 10% - 90%
        /// </summary>
        public double Front
        { get; set; }


        /// <summary>
        /// True: use fit polyN to calculate stdev and SNR.
        /// </summary>
        public bool isFitPoly
        { get; set; }
        
        /// <summary>
        /// Order of the fit polynom
        /// </summary>
        public double FitPolyOrder
        { get; set; }

        /// <summary>
        /// Coefficients of the fit polynom
        /// </summary>
        public List<double> FitPolyCoeff
        { get; set; }

        /// <summary>
        /// Number of points in the signal fit range
        /// </summary>
        public int NpFitRange
        { get; set; }

        /// <summary>
        /// Starting point of the fit range
        /// </summary>
        public int NpFit_0
        { get; set; }

        /// <summary>
        /// List of X-values (times) of the fit range
        /// </summary>
        public List<double> XsigFit
        { get; set; }

        /// <summary>
        /// List of Y-values of the fit polynom of the order N
        /// </summary>
        public List<double> YsigFit
        { get; set; }

        /// <summary>
        /// All measurement in a list:
        /// 0   :   Offset mean
        /// 1   :   Offset RMS
        /// 2   :   Offset Pk-Pk
        /// 3   :   Signal mean
        /// 4   :   Signal RMS
        /// 5   :   Signal Pk-Pk
        /// 6   :   SNR
        /// 7   :   DSNR
        /// 8   :   Front 10-90%
        /// </summary>
        public List<double> AllParams
        { get; set; }

        /// <summary>
        /// Function Poly(double x) or Poly (double[] x)
        /// </summary>
        public double Poly(double x, double[] p)
        {
            double v = 0;
            for (int i = 0; i < p.Length; i++)
                v += p[i] * Math.Pow(x, i);
            return v;
        }
        public double[] Poly(double[] x, double[] p)
        {
            List<double> vl = new List<double>();
            for (int ix = 0; ix < x.Length; ix++)
            {
                double v = 0;
                for (int i = 0; i < p.Length; i++)
                    v += p[i] * Math.Pow(x[ix], i);
                vl.Add(v);
            }
            return vl.ToArray();
        }

        /// <summary>
        /// Fits a polynom of the order N in the lists of X and Y. Returns the array of polynom coeffs.
        /// </summary>
        public double[] FitPoly(int N, List<double> X, List<double> Y)
        {
            double[] p = { };
            // Limit the order of polinom to 100
            if (N > 100)
            {
                Console.WriteLine("FitPoly error: order of polynom {0} must be <= 100. ", N);
                return p;
            }
            try
            {
                p = Fit.Polynomial(X.ToArray(), Y.ToArray(), N);
                Console.WriteLine("FitPoly: number of fit polynom coefficients: {0}. ", p.Length);
                return p;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FitPoly error: Could not make polynomial fit of the order {0}. Original error: " + ex.Message, N);
                return null;
            }

        }


        /// <summary>
        /// Ctor with parameters
        /// </summary>
        public WFMeasurement(double t0_zero, double t1_zero, double t0_pulse, double t1_pulse, double TimeScaleCoeff, Waveform wave, int MeasCount, bool toFit = false, double Npoly = 0)
        {
            // Set the name
            Name = wave.WaveFormName + "-meas-" + MeasCount.ToString();
            // Set members relating to polinomial fit
            isFitPoly = toFit;
            FitPolyOrder = Npoly;
            NpFitRange = 0;
            // Scale input gate times to the original units (s), since waveform time units are seconds
            if (TimeScaleCoeff != 0)
            {
                t0_zero /= TimeScaleCoeff;
                t1_zero /= TimeScaleCoeff;
                t0_pulse /= TimeScaleCoeff;
                t1_pulse /= TimeScaleCoeff;
            }
            double tend_wave = wave.t(wave.Npoints);
            // 1. Check if t1 > t0 and reverse if not.
            if (t0_zero >= t1_zero)
            {
                double tt = t1_zero;
                t1_zero = t0_zero;
                t0_zero = tt;
            }
            if (t0_pulse >= t1_pulse)
            {
                double tt = t1_pulse;
                t1_pulse = t0_pulse;
                t0_pulse = tt;
            }
            // Check margins and align if neccessary:
            // Parameters are not calculated only if the entire range [t0, t1] is outside waveform time range.
            bool isGoodLowGate = true;
            bool isGoodHighGate = true;
            if (t1_zero <= wave.t0)
            {
                t1_LOW_gate = t0_LOW_gate = wave.t0;
                isGoodLowGate = false;
            }
            else if (t0_zero >= tend_wave)
            {
                t1_LOW_gate = t0_LOW_gate = tend_wave;
                isGoodLowGate = false;
            }
            else if (t0_zero < wave.t0)
            {
                t0_LOW_gate = wave.t0;
                t1_LOW_gate = t1_zero;
            }
            else  if (t1_zero > tend_wave)
            {
                t0_LOW_gate = t0_zero;
                t1_LOW_gate = tend_wave;
            }
            else // t0, t1 within the range
            {
                t0_LOW_gate = t0_zero;
                t1_LOW_gate = t1_zero;
            }

            
            // The same for te HIGH gate
            if (t1_pulse <= wave.t0)
            {
                t1_HIGH_gate = t0_HIGH_gate = wave.t0;
                isGoodHighGate = false;
            }
            else if (t0_pulse >= tend_wave)
            {
                t1_HIGH_gate = t0_HIGH_gate = tend_wave;
                isGoodHighGate = false;
            }
            else if (t0_pulse < wave.t0)
            {
                t0_HIGH_gate = wave.t0;
                t1_HIGH_gate = t1_pulse;
            }
            else  if (t1_pulse > tend_wave)
            {
                t0_HIGH_gate = t0_pulse;
                t1_HIGH_gate = tend_wave;
            }
            else // t0, t1 within the range
            {
                t0_HIGH_gate = t0_pulse;
                t1_HIGH_gate = t1_pulse;
            }

            // Proceed with calculations
            if (!isGoodLowGate)
            {
                ZeroOffset = ZeroNoiseRMS = SNR = DSNR = 0;
            }
            else // isGoodLowGate OK
            {
                double val = 0;
                double vmin = Double.MaxValue;
                double vmax = Double.MinValue;
                // ZeroOffset
                for (int ip = wave.np(t0_LOW_gate); ip < wave.np(t1_LOW_gate); ip++)
                {
                    val += wave.u(ip);
                }
                ZeroOffset = val / ( wave.np(t1_LOW_gate) - wave.np(t0_LOW_gate) );
                
                // ZeroNoiseRMS
                val = 0;
                for (int ip = wave.np(t0_LOW_gate); ip < wave.np(t1_LOW_gate); ip++)
                {
                    val += Math.Pow(wave.u(ip) - ZeroOffset, 2);
                    vmin = (wave.u(ip) - ZeroOffset < vmin) ? wave.u(ip) - ZeroOffset : vmin;
                    vmax = (wave.u(ip) - ZeroOffset > vmax) ? wave.u(ip) - ZeroOffset : vmax;
                }
                ZeroNoiseRMS = Math.Sqrt(val / (wave.np(t1_LOW_gate) - wave.np(t0_LOW_gate)));
                ZeroNoisePkPk = vmax - vmin;
            }

            if (!isGoodHighGate)
            {
                PulseMean = PulseNoiseRMS = SNR = DSNR = 0;
            }
            else if (!isFitPoly) // isGoodHighGate OK, flat-top
            {
                double val = 0;
                double vmin = Double.MaxValue;
                double vmax = Double.MinValue;
                // PulseMean
                for (int ip = wave.np(t0_HIGH_gate); ip < wave.np(t1_HIGH_gate); ip++)
                {
                    val += wave.u(ip);
                }
                PulseMean = val / ( wave.np(t1_HIGH_gate) - wave.np(t0_HIGH_gate) );
                
                // PulseNoiseRMS
                val = 0;
                for (int ip = wave.np(t0_HIGH_gate); ip < wave.np(t1_HIGH_gate); ip++)
                {
                    val += Math.Pow(wave.u(ip) - PulseMean, 2);
                    vmin = (wave.u(ip) - PulseMean < vmin) ? wave.u(ip) - PulseMean : vmin;
                    vmax = (wave.u(ip) - PulseMean > vmax) ? wave.u(ip) - PulseMean : vmax;
                }
                PulseNoiseRMS = Math.Sqrt(val / (wave.np(t1_HIGH_gate) - wave.np(t0_HIGH_gate)));
                PulseNoisePkPk = vmax - vmin;
                SNR = Math.Abs(PulseMean / PulseNoiseRMS);
                DSNR = Math.Abs((PulseMean - ZeroOffset) / PulseNoiseRMS);
            }
            else if (isFitPoly && Npoly > 0)
            {
                // Use fit polyN intead of mean level
                NpFitRange = wave.np(t1_HIGH_gate) - wave.np(t0_HIGH_gate);
                NpFit_0 = wave.np(t0_HIGH_gate);
                // List of Y samples of the signal to fit
                List<double> Ysig = new List<double>(wave.Samples.GetRange(wave.np(t0_HIGH_gate), NpFitRange));
                // Prepare list of X (times)
                XsigFit = new List<double>();
                for (int it = wave.np(t0_HIGH_gate); it < wave.np(t1_HIGH_gate); it++)
                    XsigFit.Add(wave.t(it));
                FitPolyCoeff = FitPoly((int)FitPolyOrder, XsigFit, Ysig).ToList();
                // Replace the parameter of Pulse Mean, calculate SNR and DSNR
                YsigFit = new List<double>(Poly(XsigFit.ToArray(), FitPolyCoeff.ToArray()));
                PulseMean = YsigFit.Sum() / NpFitRange;
                SNR = DSNR = 0;
                PulseNoiseRMS = 0;
                double val = 0;
                for (int it = 0; it < NpFitRange; it++)
                {
                    val += Math.Pow(wave.u(it + NpFit_0) - YsigFit.ElementAt(it), 2);
                }
                PulseNoiseRMS = Math.Sqrt(val / NpFitRange);
                SNR = Math.Abs(PulseMean / PulseNoiseRMS);
                DSNR = Math.Abs((PulseMean - ZeroOffset) / PulseNoiseRMS);

            }

            // Front 10-90%
            double DeltaU = Math.Abs(PulseMean - ZeroOffset);
            double tau_0 = t1_LOW_gate;
            double tau_1 = t0_HIGH_gate;
            if (tau_1 <= tau_0)
            {
                Front = -1;
                return;
            }
            for (double t = tau_0; t < tau_1; t += wave.dt)
            {
                if (Math.Abs(wave.u(t) - ZeroOffset) >= 0.1 * DeltaU)
                {
                    tau_0 = t;
                    break;
                }
            }
            for (double t = tau_0; t < tau_1; t += wave.dt)
            {
                if (Math.Abs(wave.u(t) - ZeroOffset) >= 0.9 * DeltaU)
                {
                    tau_1 = t;
                    break;
                }
            }
            Front = (tau_1 - tau_0) * 1e9;  // In ns
            //Console.WriteLine("Tau_0 = {0}, tau_1 = {1}, front = {2}.", tau_0, tau_1, Front);

            // Fill the consolidated parameter list
            AllParams = new List<double>();
            AllParams.Add(ZeroOffset);
            AllParams.Add(ZeroNoiseRMS);
            AllParams.Add(ZeroNoisePkPk);
            AllParams.Add(PulseMean);
            AllParams.Add(PulseNoiseRMS);
            AllParams.Add(PulseNoisePkPk);
            AllParams.Add(SNR);
            AllParams.Add(DSNR);
            AllParams.Add(Front);

            
        }



    }
}
