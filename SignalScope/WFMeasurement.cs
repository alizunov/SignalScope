using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalScope
{
    /// <summary>
    /// Usable measurements on a waveform:
    ///- zero offset and RMS noise (gated);
    ///- mean level or fit polyN (gated) and RMS noise;
    ///- front (?)
    /// </summary>

    class WFMeasurement
    {
        // Properties
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

        /// <summary>
        /// Mean value for the 'flat' case (if isFitPoly = false)
        /// </summary>
        public double PulseMean
        { get; set; }

        public double PulseNoiseRMS
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
        /// True: use fit polyN to calculate stdev and SNR.
        /// </summary>
        public bool isFitPoly
        { get; set; }
        
        /// <summary>
        /// Coefficients of the fit polynom
        /// </summary>
        public List<double> FitPolyCoeff
        { get; set; }

        /// <summary>
        /// Ctor with parameters
        /// </summary>
        public WFMeasurement(double t0_zero, double t1_zero, double t0_pulse, double t1_pulse, Waveform wave)
        {
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
                }
                ZeroNoiseRMS = Math.Sqrt(val / (wave.np(t1_LOW_gate) - wave.np(t0_LOW_gate)));
            }

            if (!isGoodHighGate)
            {
                PulseMean = PulseNoiseRMS = SNR = DSNR = 0;
            }
            else if (!isFitPoly) // isGoodHighGate OK, flat-top
            {
                double val = 0;
                // PulseMean
                for (int ip = wave.np(t0_HIGH_gate); ip < wave.np(t1_HIGH_gate); ip++)
                {
                    val += wave.u(ip);
                }
                PulseMean = val / ( wave.np(t1_LOW_gate) - wave.np(t0_LOW_gate) );
                
                // PulseNoiseRMS
                val = 0;
                for (int ip = wave.np(t0_HIGH_gate); ip < wave.np(t1_HIGH_gate); ip++)
                {
                    val += Math.Pow(wave.u(ip) - PulseMean, 2);
                }
                PulseNoiseRMS = Math.Sqrt(val / (wave.np(t1_LOW_gate) - wave.np(t0_LOW_gate)));
                SNR = Math.Abs(PulseMean / PulseNoiseRMS);
                DSNR = Math.Abs((PulseMean - ZeroOffset) / PulseNoiseRMS);
            }

            
        }



    }
}
