using System;
using System.Collections.Generic;

namespace Don_tKnowHowToNameThis
{
    public class Calc
    {
        public readonly string? _material;
        public readonly double _R = 8.314;
        public readonly double _W = 0.20;
        public readonly double _H = 0.005;
        public readonly double _L = 4.0;
        public readonly double _step = 0.1;
        public readonly double _p = 920;
        public readonly double _c = 2300;
        public readonly double _T0 = 120;
        public double _Vu = 1.2;
        public double _Tu = 150;
        public readonly double _mu0 = 50000;
        public readonly double _Ea = 48000;
        public readonly double _Tr = 120;
        public readonly double _n = 0.35;
        public readonly double _alphaU = 250;

        private double gammaPoit = 0;
        private double qGamma = 0;
        private double beta = 0;
        private double qAlpha = 0;
        private double F = 0;
        private double Qch = 0;

        public List<double> zCoords = new List<double>();
        public List<double> temperature = new List<double>();
        public List<double> viscosity = new List<double>();
        public int Q = 0;
        public double LostTime = 0;
        public double Lostmem = 0;
        public Calc(string mat, double W, double H, double L, double step, double p, double c, double T0, double Vu, double Tu, double mu0, double Ea, double Tr, double n, double alphaU)
        {
            _material = mat;
            _W = W;
            _H = H;
            _L = L;
            _step = step;
            _p = p;
            _c = c;
            _T0 = T0;
            _Vu = Vu;
            _Tu = Tu;
            _mu0 = mu0;
            _Ea = Ea;
            _Tr = Tr;
            _n = n;
            _alphaU = alphaU;
        }
        public Calc(string mat, double W, double H, double L, double p, double c, double T0, double mu0, double Ea, double Tr, double n, double alphaU)
        {
            _material = mat;
            _W = W;
            _H = H;
            _L = L;
            _p = p;
            _c = c;
            _T0 = T0;
            _mu0 = mu0;
            _Ea = Ea;
            _Tr = Tr;
            _n = n;
            _alphaU = alphaU;
        }

        public Calc()
        {
        }

        private void MaterialShearStrainRate()
        {
            gammaPoit = _Vu / _H;
        }
        private void SpecificHeatFluxes()
        {
            qGamma = _H * _W * _mu0 * Math.Pow(gammaPoit, _n + 1);
            beta = _Ea / (_R * (_T0 + 20 + 273) * (_Tr + 273));
            qAlpha = _W * _alphaU * (Math.Pow(beta, -1) - _Tu + _Tr);
        }
        private void VolumeFlowRateOfMaterialFlowInTheChannel()
        {
            F = 0.125 * Math.Pow(_H / _W, 2) - 0.625 * (_H / _W) + 1;
            Qch = _H * _W * _Vu * F / 2;
        }
        public void TemperatureAndViscosity(Calc calc, List<double> zCoord, List<double> temperature, List<double> viscosity, ref double time, ref double mem)
        {
            calc.MaterialShearStrainRate();
            calc.SpecificHeatFluxes();
            calc.VolumeFlowRateOfMaterialFlowInTheChannel();
            for (decimal z = 0; z <= Convert.ToDecimal(calc._L); z = z + Convert.ToDecimal(calc._step))
            {
                zCoord.Add(Convert.ToDouble(z));
                double T = calc.Temperature(Convert.ToDouble(z));
                temperature.Add(Math.Round(T, 2));
                double n = calc.Viscosity(T);
                viscosity.Add(Math.Round(n, 1));
            }

            Lostmem = mem;
            LostTime = time;
            calc.Efficiency();
            this.zCoords = zCoord;
            this.temperature = temperature;
            this.viscosity = viscosity;
        }
        private double Temperature(double z)
        {
            double T = 0;

            T = _Tr + (1 / beta) * Math.Log((beta * qGamma + _W * _alphaU) / (beta * qAlpha) * (1 - Math.Exp((-beta * qAlpha * z) / (_p * _c * Qch))) + Math.Exp(beta * (_T0 - _Tr - (qAlpha * z) / (_p * _c * Qch))));
            return T;
        }
        private double Viscosity(double T)
        {
            double n = 0;

            n = _mu0 * Math.Exp(-beta * (T - _Tr)) * Math.Pow(gammaPoit, _n - 1);

            return n;
        }
        public void Efficiency()
        {
            Q = (int)Math.Round(_p * Qch * 3600, 0);
        }
        public void Experiment(double iMin, double iMax, double iStep, double jMin, double jMax, double jStep, List<List<string>> res)
        {
            for (decimal i = Convert.ToDecimal(iMin); i <= Convert.ToDecimal(iMax); i += Convert.ToDecimal(iStep))
            {
                for (decimal j = Convert.ToDecimal(jMin); j <= Convert.ToDecimal(jMax); j += Convert.ToDecimal(jStep))
                {
                    this._Vu = (double)i;
                    this._Tu = (double)j;
                    this.MaterialShearStrainRate();
                    this.SpecificHeatFluxes();
                    this.VolumeFlowRateOfMaterialFlowInTheChannel();
                    double T = Temperature(_L);
                    double n = Viscosity(T);
                    Efficiency();
                    double Q = this.Q;
                    res.Add(new List<string> { i.ToString(), j.ToString(), T.ToString(), n.ToString(), Q.ToString() });
                }
            }
        }
        public void Experiment1(double Vu, double Tu, List<double> res)
        {
            this._Vu = Vu;
            this._Tu = Tu;
            this.MaterialShearStrainRate();
            this.SpecificHeatFluxes();
            this.VolumeFlowRateOfMaterialFlowInTheChannel();
            double T = Math.Round(Temperature(_L), 2);
            res.Add(T);
            double n = Math.Round(Viscosity(T), 1);
            res.Add(n);
            Efficiency();
            double Q = this.Q;
            res.Add(Q);
            //res = new List<double> {T, n, Q};
        }
    }
}
