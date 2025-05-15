using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FYP_Library
{
       public class Parameters
       {
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Ship,Engine,Prop info~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              public string DNVGL { get; set; }
              public string IMONum { get; set; }
              public string VesselName { get; set; }
              public string BuildingYard { get; set; }
              public string HullNum { get; set; }
              public string LSW { get; set; }
              public string EngineManufacturer { get; set; }
              public string EngineType { get; set; }
              public string PropellerType { get; set; }
              public string NumBlades { get; set; }



              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Assessment 1~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Section 1~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              //Deadweight Tonnage, DWT
              public double DWT { get; set; }
              //Maximum Continous Rating (MCR)
              public double MCR { get; set; }
              //Ship Type
              public string ST { get; set; }
              //Minimum propulsion power, Assessment 1
              public double MPP { get; set; }
              public double a;
              public double b;
              public double Eqn_MPP()
              {
                     //float a; float b;
                     string ST1 = "Bulk Carrier";
                     string ST2 = "Tanker/Combination Carrier";
                     if (ST == ST1)
                     {
                            if (DWT > 20000 && DWT < 145000)
                            {
                                   a = 0.0763d;
                                   b = 3374.3d;
                            }
                            else if (DWT > 145000)
                            {
                                   a = 0.049d;
                                   b = 7329.0d;
                            }
                            else
                            {
                                   a = 0d;
                                   b = 0d;
                            }
                            Cair = 0.95d;
                     }
                     else if (ST == ST2)
                     {
                            if (DWT > 20000)
                            {
                                   a = 0.0652d;
                                   b = 5960.2d;
                            }
                            else
                            {
                                   a = 0d;
                                   b = 0d;
                            }
                            Cair = 0.98d;
                     }
                     else
                     {
                            Cair = 0.97d;
                            return MPP = 0d;
                     }
                     return MPP = (a * DWT) + b;

              }
              public string Result { get; set; } //Satisfactory or unsatisfactory
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~End of Assessment 1~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              //
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Assessment 2~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Section 2~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              //Breadth on water line
              public double BWL { get; set; }
              //Scantling draft at midship, Tm
              public double Tm { get; set; }
              //Ship Length, Lpp
              public double Lpp { get; set; }
              //Actual Rudder Area, AR
              public double AR { get; set; }
              //Submerged Lateral Area of Ship Corrected for Breadth Effect, ALS,cor
              public double ALScor { get; set; }
              
              public double Eqn_ALScor()
              {
                     var Pow_ALS = (double)Math.Pow((BWL / Lpp), 2);
                     var newALScor = Lpp * Tm * (1.0f + (25.0f * Pow_ALS));
                     return ALScor = newALScor;
              }
              //Percentage of Rudder area to submerged lateral area of the ship
              public double PerALS { get; set; }
              
              public double Eqn_PerALS()
              {
                     var newPerALS = AR / Eqn_ALScor() *100d;
                     return PerALS = newPerALS;
              }

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Section 3~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              //Required ship speed of advance
              public double FWA { get; set; }
              //Frontal Windage Area
              public double LWA { get; set; }
              //Lateral Windage Area
              public double RatioFL { get; set; }
              //Ratio between FWA and LWA
              public double Eqn_RatioFL()
              {
                     var newRatioFL = FWA / LWA;
                     return RatioFL = newRatioFL;
              }
              public double Vnav { get; set; }
              //minimum navigation speed, Vnav
              public double Vckref { get; set; }
              //reference course keeping speed, Vck,ref
              public double Eqn_Vckref()
              {
                     double newVckref;
                     Eqn_RatioFL();
                     var RFL = RatioFL;
                     if (RFL < 0.1d)
                     {
                            newVckref = 9.0d;
                     }
                     else if (RFL > 0.4d)
                     {
                            newVckref = 4.0d;
                     }
                     else
                     {
                            newVckref = (double)(9d + ((-5 / 0.3) * (RFL - 0.1d)));
                     }
                     return Vckref = newVckref;
              }
              public double Vck { get; set; }
              //minimum course keeping speed, Vck
              public double Eqn_Vck()
              {
                     return Vck = Eqn_Vckref() - (10 * (Eqn_PerALS() - 0.9d));

              }
              //Required advance speed, Vs in knots
              public double Vs { get; set; }
              //Convert to Vs to m/s
              public double Eqn_Vs() 
              {
                     Eqn_Vck();
                     if ( Vck > Vnav)
                     {
                            return Vs = Vck * 1852 / 3600;
                     }
                     else
                     {
                            return Vs = Vnav * 1852 / 3600;
                     }
                     
              }
              //Number of engines
              public double NumEng { get; set; }
              //Block Coefficient
              public double BlockC { get; set; }
              

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Section 4~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              //Procedure of assessment of installed power
              //Temperature
              public double Temp { get; set; }
              //Density Of Air
              public double RowAir { get; set; }
              //Kinematic viscosity of water, v
              public double ViscH2o { get; set; }
              //Density of water
              public double RowH2o { get; set; }
              //Wetted area of bare hull, S
              public double S { get; set; }
              //Renolds Number
              public double Rey { get; set; }
              public double Eqn_Rey()
              {

                     return Rey = (double)(Eqn_Vs() * Lpp / (ViscH2o * Math.Pow(10, -6)));
                      
              }
              //Mean wind speed, Vw
              public double Vw { get; set; }

              //Significant wave height, Hs
              public double Hs { get; set; }
              public (double, double) Eqn_HsVw()
              {
                     if (Lpp > 250)
                     {
                            Vw = 19f;
                            Hs = 5.5f;
                     }
                     else if (Lpp < 200)
                     {
                            Vw = 15.7f;
                            Hs = 4f;
                     }
                     else
                     {
                            Vw = (double)((((19 - 15.7) / (250 - 200)) * (Lpp - 200)) + 15.7);
                            Hs = (double)((5.5 - 4) / (250 - 200) * (Lpp - 200) + 4);
                     }
                     return (Vw, Hs);
              }
              //Form factor, k
              public double K { get; set; }
              public double Eqn_K()
              {
                     //Eqn_Wake();
                     return K = (double)(-0.095d + (25.6d * BlockC) / (Math.Pow((Lpp / BWL), 2) * Math.Sqrt(BWL / Tm)));
              }
              //Frictional Resistance Coefficient, Cf
              public double Cf { get; set; }
              public double Eqn_Cf()
              {
                     return Cf = (double)(0.075 / Math.Pow((Math.Log10(Eqn_Rey()) - 2), 2));

              }

              //Resistance in calm water or sum of bare hull resistance in calm water, Rcw
              public double Rcw { get; set; }
              public double Eqn_Rcw()
              {
                     //=(1+IF(C58=0,k_emp,k))*C_F*0.5*rho_w*S*(V_s^2)/1000
                     return Rcw = (double)((1 + K) * Eqn_Cf() * 0.5 * RowH2o * S * (Math.Pow(Eqn_Vs(), 2) / 1000));
              }

              //Aerodynamic resistance coefficient
              public double Cair { get; set; }
              //Aerodynamic resistance, Rair
              public double Rair { get; set; }
              
              public double Eqn_Rair()
              {
                     Eqn_MPP(); //fetch C_air based on Ship Type
                      //fetch Vs
                     Eqn_HsVw(); //fetch Vw
                     return Rair = (double)((Cair * 0.5 * RowAir * FWA * Math.Pow((Vw + Eqn_Vs()), 2)) /1000);
              }
              //Resistance due to appendages, Rapp
              public double Rapp;



              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Section 5~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              //Added Resistance, Raw
              public double Raw { get; set; }

              public int count { get; set; }
              public double RawHeight { get; set; }
              //Approximation of the added resistance of ships with small draft or in ballast condition by empirical formula Shukui Liu 2017"

              public double WaveAmp { get; set; } //wave amplitude or incident wave amplitude
              public double rRaw { get; set; } //constant for rRaw=4*ro*g*amp*amp*Bmax*Bmax/Lpp
              public double Bmax { get; set; } //Maximum Beam
              public double E_para { get; set; } //angle of waterline entrance
              public double g { get; set; } //gravity
              public double TA { get; set; } //fore perpendicular
              public double TF { get; set; } //aft perpendicular

              //formulas
              public double L_we { get; set; } //length of waterline entrance
              public double Eqn_L_we()
              {
                     return L_we = (double)(2 / 20.0 * Lpp);
              }
              public double omega { get; set; } //https://en.wikipedia.org/wiki/Dispersion_(water_waves)
              public double Eqn_omega(double time) //insert Peak wave
              {
                     return omega = (double)(2* Math.PI / time);
              }
              public double lambda { get; set; } //wave length, deep water dispersion 

              public double Eqn_lambda(double time)
              {
                     return lambda = (double)(2 * Math.PI / (Math.Pow(Eqn_omega(time), 2) / g));
                     //T^2 = (2pie)^2 / f^2
              }
              public double Fr { get; set; } //Froude number
              public double Eqn_Fr()
              {
                     Eqn_Vs();
                     return Fr = (double)(Vs * 0.5144 / Math.Sqrt(g * Lpp));
              }

              //~~~~~~~~~~~~~~~break~~~~~~~~~~~~~~~//

              public double alphaT { get; set; } //draft correction coefficient
              public double Eqn_alphaT(double time)
              {
                     Eqn_lambda(time);
                     if ((lambda / Lpp) > 2.5)
                     {
                            return alphaT = 0;
                     }
                     else
                     {
                            return alphaT = (double) (1 - Math.Exp(-4 * Math.PI * (TA / Eqn_lambda(time) - TA / 2.5 / Lpp)));
                     }
              }
              public double Eqn_E_para()
              {
                     return E_para = (double)(Math.Atan(Bmax / (2.0 * Eqn_L_we())));
                     //In degree
              }
              
              public double Rawrl { get; set; } //Added resistance in regular waves due to reflection / diffraction effect
              public double Eqn_Rawrl(double time)
              {
                     //Eqn_Wake();
                     Eqn_alphaT(time);
                     Eqn_E_para();
                     Eqn_Fr();
                     return Rawrl = (double)(2.25 / 8.0 * Lpp / Bmax * Eqn_alphaT(time) * Math.Pow(Math.Sin(E_para),2) * (1 + 5.0 * Math.Sqrt(Lpp / Eqn_lambda(time)) * Fr) * Math.Pow((0.87 / BlockC), (1 + 4.0 * Math.Sqrt(Fr))));
              }
              public double Iyy { get; set; }
              public double Eqn_Iyy()
              {
                     //return Iyy = (double)Math.Pow((0.25 * Lpp), 2); //https://www.boatdesign.net/threads/how-to-calculate-the-moment-of-inertia-for-a-half-ship.57153/
                     return Iyy = (double)(Math.Pow(kyy*Lpp , 2)* DWT);
              }
              public double kyy { get; set; } //pitch coefficient, nondimensional longitudinal mass radius of gyration(pitch), as percentage of LPP
              public double Eqn_kyy()
              {
                     return kyy = (double)(Math.Sqrt(Iyy / DWT) / Lpp);
              }
              public double Eqn_rRaw()
              {
                     return rRaw = (double)(4 * RowH2o * g * Math.Pow(WaveAmp, 2) * Math.Pow(Bmax, 2) / Lpp);
              }
              public double a1 { get; set; } //form factor
              public double Eqn_a1()
              {
                     Eqn_Fr();
                     return a1 = (double)(60.3 * Math.Pow(BlockC , 1.34) * Math.Pow((4 * kyy), 2) * Math.Pow((0.87 / BlockC), (1 + Fr)) / Math.Log(Bmax / TA));
              }
              public double a2 { get; set; } //speed factor
              public double Eqn_a2()
              {
                     Eqn_Fr();
                     if (Fr < 0.12)
                     {
                            return a2 = (double)(0.0072 + 0.1676 * Fr);
                     }
                     else
                     {
                            return a2 = (double)(Math.Pow(Fr, 1.5) * Math.Exp(-3.5 * Fr));
                     }
              }
              public double a3 { get; set; } //trim factor
              public double Eqn_a3()
              {
                     var a3trigo = (double)(180 / Math.PI * Math.Atan((TA - TF) / Lpp));
                     return a3 = (double)(1.0 + 0.25 * a3trigo);
                     // unit is in degree
              }
              public double ARmaxOmega { get; set; } //ARmax derived frequency quantity
              public double Eqn_ARmaxOmega(double time)
              {
                     Eqn_Fr();
                     return ARmaxOmega = (double)(2.142 * Math.Pow(kyy, (1 / 3.0)) * Math.Sqrt(Lpp / Eqn_lambda(time)) * (1 - 0.13 * 0.85 / BlockC * (Math.Log(Bmax / TA) - Math.Log(2.75))));
              }
              public double Non_w { get; set; } //dimensionless frequency coefficient or derived frequency quantity
              public double Eqn_Non_w(double time)
              {
                     
                     if (Eqn_Fr() <= 0.10)
                     {
                           return Non_w = (double)(Eqn_ARmaxOmega(time) * (Eqn_Fr() + 0.62));
                     }
                     else
                     {
                            return Non_w = (double)(Eqn_ARmaxOmega(time) * Math.Pow(Eqn_Fr(), 0.143));
                     }
              }
              public double b1 { get; set; }
              public double d1 { get; set; }
              public double Eqn_b1d1(double time)
              {
                     
                     var Eqn_d1 = (double)(566 * Math.Pow((Lpp / Bmax), -2.66));
                     if (BlockC < 0.75)
                     {
                            
                            if (Eqn_Non_w(time) < 1.0)
                            {
                                   b1 = 11.0;
                                   d1 = 14.0;
                            }
                            else
                            {
                                   b1 = -8.50;
                                   d1 = (double)(Eqn_d1 * 6);
                            }
                     }
                     else
                     {
                            if (Eqn_Non_w(time) < 1.0)
                            {
                                   b1 = 11.0;
                                   d1 = (double)(Eqn_d1);
                            }
                            else
                            {
                                   b1 = -8.50;
                                   d1 = (double)(Eqn_d1 * 6);
                            }
                     }
                     return b1;
              }
              public double Rawml { get; set; } //Added resistance in regular waves due to motion / radiation effect
              public double Eqn_Rawml(double time)
              {
                     Eqn_b1d1(time);
                     return Rawml = (double)(Math.Pow(Eqn_Non_w(time), b1) * Math.Exp(b1 / d1 * (1 - Math.Pow(Eqn_Non_w(time), d1))) * Eqn_a1() * Eqn_a2() * Eqn_a3());
              }
              public double Rwave { get; set; } //total added resistance in regular waves
              public double Eqn_Rwave(double time)
              {
                     return Rwave = (double)(((Eqn_Rawml(time) + Eqn_Rawrl(time)) * Eqn_rRaw())/1000); // (Rawml + Rawrl) multiple by constant 
              }
              public double Rsubtotal { get; set; }
              public double Eqn_Rsubtotal()
              {
                     return Rsubtotal = Eqn_Rair() + Eqn_Rcw() + Rapp;
              }
              public double Rtotal { get; set; }
              public double Eqn_Rtotal(double time)
              {
                     return Rtotal = Eqn_Rsubtotal() + Eqn_Rwave(time); //Units in KN
              }

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Section 6~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              //Recommended values for wake fraction, w
              public double Wake { get; set; }
              public double Eqn_Wake()
              {
                     if (NumEng == 1d)
                     {
                            if (BlockC == 0.5d)
                            {
                                   Wake = 0.14d;
                            }
                            else if (BlockC == 0.6d)
                            {
                                   Wake = 0.23d;
                            }
                            else if (BlockC == 0.7d)
                            {
                                   Wake = 0.29d;
                            }
                            else if (BlockC >= 0.8d)
                            {
                                   Wake = 0.35d;
                            }
                     }
                     else if (NumEng == 2d)
                     {
                            if (BlockC == 0.5d)
                            {
                                   Wake = 0.15d;
                            }
                            else if (BlockC == 0.6d)
                            {
                                   Wake = 0.17d;
                            }
                            else if (BlockC == 0.7d)
                            {
                                   Wake = 0.19d;
                            }
                            else if (BlockC >= 0.8d)
                            {
                                   Wake = 0.23d;
                            }
                     }
                     else
                     {
                            return Wake = 0;
                     }
                     return Wake;
              }

              //Thurst Deduction Factor, t
              public double TDF { get; set; }
              public double Eqn_TDF()
              {
                     return TDF = 0.7f * Eqn_Wake();
              }

              
              //Propeller Diameter, Dp
              public double Dp { get; set; }

              //Required delivered power to propeller, PD(watts)

              public double Eqn_Quad(double a, double b, double c, double x)
              {
                     return ((a * Math.Pow(x, 2)) + (b * x) + c);
              }
              //KT intercept, usingquadratic eqn method to solve for J
              public string outcome { get; set; }
              public double J_at_KTintercept { get; set; }
              public double Eqn_SolveQuad(double a, double b, double c)
              {
                     double root, deno, x1, x2;
                     x1 = x2 = 0;
                     
                     if (a == 0)
                     {
                            x1 = -c / b;
                            outcome = ("The roots are Linear:"+ x1.ToString());
                     }
                     else
                     {
                            root = (b * b) - (4 * a * c);
                            deno = 2 * a;
                            if (root > 0)
                            {
                                   x1 = (-b / deno) + (Math.Sqrt(root) / deno);
                                   x2 = (-b / deno) - (Math.Sqrt(root) / deno);
                                   outcome = ("THE ROOTS ARE REAL AND DISTINCT ROOTS" + "\n" + "THE ROOTS ARE: " + x1.ToString() + " and " + x2.ToString());
                            }
                            else if (root == 0)
                            {
                                   x1 = -b / deno;
                                   outcome = ("THE ROOTS ARE REPEATED ROOTS"+"\n"+"THE ROOT IS: " + x1.ToString());
                            }
                            else
                            {
                                   x1 = -b / deno;
                                   x2 = ((Math.Sqrt((4 * a * c) - (b * b))) / deno);
                                   outcome = ("THE ROOTS ARE IMAGINARY ROOTS" + "\n" + "ROOT 1: " + x1.ToString() + "+i" + x2.ToString() + "\n" + "ROOT 2: " + x1.ToString() + "-i" + x2.ToString());
                            }
                     }
                     if (x1 > x2)
                     {
                             return x1;
                     }
                     else
                     {
                            return x2;
                     }


              }
              ////optimum shaft speed, n_opt, using J_at_KTintercept
              public double n_opt { get; set; }
              public double Eqn_n_opt()
              {
                     return n_opt = (Eqn_Ua() / (J_at_KTintercept * Dp));
              }


              //Speed, Ua
              public double Ua { get; set; }
              public double Eqn_Ua()
              {
                     return Ua = (double)(Eqn_Vs() * (1 - Eqn_Wake()));
                     //ua=vs(1-w)
              }
              

              public double J_interval { get; set; }
              public double J_limit { get; set; }
              public double chartYmax { get; set; }
              public double chartXmax { get; set; }


              //Advance Coefficient, J
              public double J { get; set; }
              public double Eqn_J()
              {
                     return J = (double)(Eqn_Ua() / nrpm * Dp);
              }

              
              //KQ(J) = oper water propeller torque coefficient curve
              public double KQ_J { get; set; }

              public double Eqn_KQ(double J)
              {
                     return KQ_J=Eqn_Quad(KQa, KQb, KQc, J);
              }
              //KT(J) = open water propeller thurst coefficient
              public double KT_J { get; set; }
              public double Eqn_KT(double J)
              {
                     return KT_J=Eqn_Quad(KTa, KTb, KTc, J);
              }
              // a, b, c coeficients for KT and KQ 
              public double KTa { get; set; }
              public double KTb { get; set; }
              public double KTc { get; set; }
              public double KQa { get; set; }
              public double KQb { get; set; }
              public double KQc { get; set; }

              //eta0(J) = propeller efficiency
              public double eta0 { get; set; }
              public double Eqn_eta0(double J)
              {
                     return eta0 = (double)(10*Eqn_KT(J) * J / (Eqn_KQ(J) * 2 * Math.PI));
              }

              //Propeller Thrust at indexed 0, T0
              public double Thrust0 { get; set; }
              //constant c7
              public double c7 { get; set; }
              //Optimum propeller speed, KTship
              public double KTship0 { get; set; }
              public double Eqn_KTship0(double R, double J) //resistance & advance coefficient
              {
                     Thrust0 = (double)(R*1000 / (1 - Eqn_TDF()));
                     c7 = (double)(Thrust0 / (RowH2o * Math.Pow(Eqn_Ua(), 2) * Math.Pow(Dp, 2)));
                     return (double)(c7 * Math.Pow(J, 2));
              }
              

              //Relative rotative efficiency 
              public double RReff { get; set; }
              //Engine layout
              public string Layout { get; set; }
              //Transmission efficiency,ηs
              public double TransEff { get; set; }
              public double Eqn_TransEff()
              {
                     if (Layout == "aft engine")
                     {
                            return TransEff = 0.98;
                     }
                     else if (Layout == "midship")
                     {
                            return TransEff = 0.97;
                     }
                     else
                     {
                            return TransEff = 0;
                     }
              }

              //Required rotation rate of propeller, n (rps)
              public double nrpm { get; set; }
              public double Eqn_Rps(double J)
              {
                     return nrpm = (double)(Ua / (J * Dp))/60;
              }
              //Required delivered power , Pd (kW)
              public double PD { get; set; }
              public double Eqn_PD(double J)
              {
                     return PD = (double)(2 * Math.PI * RowH2o * Math.Pow(Eqn_Rps(J), 3) * Math.Pow(Dp, 5) * Eqn_KQ(J))/1000;
              }
              //Brake Power, PB (kW)
              public double PB { get; set; }
              public double Eqn_PB(double J)
              {
                     return PB = (double)(Eqn_PD(J)/TransEff);
              }

              // ~~~~~~~~~~~~~~~~~~~~~~~~~break~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~End of Assessment 2~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


              //Add Comment 
              public string Addcom_DNVGL { get; set; }
              public string Addcom_IMONum { get; set; }
              public string Addcom_VesselName { get; set; }
              public string Addcom_BuildingYard { get; set; }
              public string Addcom_HullNum { get; set; }
              public string Addcom_LightShipWeight { get; set; }
              public string Addcom_EngineManufacturer { get; set; }
              public string Addcom_EngineType { get; set; }
              //
              public string Addcom_DWT { get; set; }
              public string Addcom_MCR { get; set; }
              public string Addcom_ShipType { get; set; }
              public string Addcom_a { get; set; }
              public string Addcom_b { get; set; }
              public string Addcom_MPP { get; set; }
              public string Addcom_Result { get; set; }
              //
              public string Addcom_BWL { get; set; }
              public string Addcom_Tm { get; set; }
              public string Addcom_LPP { get; set; }
              public string Addcom_AR { get; set; }
              public string Addcom_ALScor { get; set; }
              public string Addcom_PerALScor { get; set; }
              //
              public string Addcom_FWA { get; set; }
              public string Addcom_LWA { get; set; }
              public string Addcom_Ratio { get; set; }
              public string Addcom_Vnav { get; set; }
              public string Addcom_Vckref { get; set; }
              //
              public string Addcom_Vck { get; set; }
              public string Addcom_Vs { get; set; }
              public string Addcom_NumEngine { get; set; }
              public string Addcom_BlockC { get; set; }
              //
              public string Addcom_Temp { get; set; }
              public string Addcom_RowAir { get; set; }
              public string Addcom_ViscH2o { get; set; }
              public string Addcom_RowH2o { get; set; }
              public string Addcom_S { get; set; }
              public string Addcom_k { get; set; }
              public string Addcom_Rey { get; set; }
              public string Addcom_Vw { get; set; }
              public string Addcom_Cf { get; set; }
              public string Addcom_Rcw { get; set; }
              public string Addcom_Rair { get; set; }
              public string Addcom_Cair { get; set; }
              public string Addcom_Rapp { get; set; }
              public string Addcom_Gravity { get; set; }
              public string Addcom_Froude { get; set; }


       }
}
