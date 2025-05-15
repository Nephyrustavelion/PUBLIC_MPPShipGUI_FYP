using FYP_Library;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using iText.IO.Image;

using Microsoft.VisualBasic;
/*
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
*/


namespace WindowsFormsApp1FYP_WindowsForm
{
       public partial class Forms : Form
       {
              public Parameters classic { get; set; }
              public object AutoClosingMessageBox { get; private set; }

              public Forms()
              {
              InitializeComponent();
              classic = new Parameters();

              Constants();
              Run5();
              //SAMPLE_A();

              }
              private void Constants()
              {
              //assumed, (@ 15°C and 1atm=1013.3mbar)
              classic.RowAir = 1.225;
              In_txtRowAir.Text = classic.RowAir.ToString();
              classic.RowH2o = 1025.8;
              In_txtRowH2o.Text = classic.RowH2o.ToString();
              classic.Temp = 15;
              In_txtTemp.Text = classic.Temp.ToString();
              classic.ViscH2o = 1.184;
              In_txtViscH2o.Text = classic.ViscH2o.ToString();
              classic.g = 9.807;
              In_txtg.Text = classic.g.ToString();

              In_txtK.Text = "nil";
              In_txtIyy.Text = "nil";
              In_txtkyy.Text = "nil";
              In_txtIyy.BackColor = SystemColors.ButtonHighlight;
              In_txtkyy.BackColor = SystemColors.Info;
              In_txtRapp.Text = 0.ToString();
              classic.Raw = 0;
              classic.count = 0;
              classic.RawHeight = 0;

              //
              classic.RReff = 1.0;
              In_txtRReff.Text = classic.RReff.ToString();
              //
              classic.chartXmax = 1.1;
              chartXmaxUPDOWN.Text = classic.chartXmax.ToString();
              classic.chartYmax = 0.9;
              chartYmaxUPDOWN.Text = classic.chartYmax.ToString();
              }

              //https://stackoverflow.com/questions/3977497/stripping-out-non-numeric-characters-in-string
              //https://codereview.stackexchange.com/questions/85053/extracting-a-decimal-from-a-string
              //https://stackoverflow.com/questions/3575331/how-to-extract-decimal-number-from-string-in-c-sharp

              private static string GetNumbers(string input)
              {
              return new string(input.Where(c => char.IsDigit(c)).ToArray());
              }
              /*
              private static double GetNumbers(string input)
              {
                     var doubleArray = Regex.Split(input, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "");
                     return doubleArray(input);
              }
              */
              //TEST A Button >> to insert fixed values
              private void SAMPLE_A()
              {
              //override Run0 data, Page 1
              classic.DNVGL = "n/a";
              classic.IMONum = "9123123";
              classic.VesselName = "Titanic Sample";
              classic.BuildingYard = "Mighty Dry Dock";
              classic.HullNum = "ABXXX";
              classic.LSW = "65432";
              classic.EngineManufacturer = "PowPower Co. Ltd";
              classic.EngineType = "Titanic Super 9000MXZ";

              //
              In_txtDNVGL.Text = classic.DNVGL.ToString();
              In_txtIMONum.Text = classic.IMONum.ToString();
              In_txtVesselName.Text = classic.VesselName.ToString();
              In_txtBuildingYard.Text = classic.BuildingYard.ToString();
              In_txtHullNum.Text = classic.HullNum.ToString();
              In_txtLSW.Text = classic.LSW.ToString();
              In_txtEngineManufacturer.Text = classic.EngineManufacturer.ToString();
              In_txtEngineType.Text = classic.EngineType.ToString();

              //override Run1 data, Page 1
              classic.DWT = 82025;
              classic.MCR = 9801;
              classic.ST = "Bulk Carrier";
              //
              In_txtDWT.Text = classic.DWT.ToString();
              In_txtMCR.Text = classic.MCR.ToString();
              In_listboxShipType.Text = classic.ST;

              //override Run2 data, Page 1
              classic.BWL = 32;
              classic.Tm = 14.45;
              classic.Lpp = 225;
              classic.AR = 51;
              //
              In_txtBWL.Text = classic.BWL.ToString();
              In_txtTm.Text = classic.Tm.ToString();
              In_txtLpp.Text = classic.Lpp.ToString();
              In_txtAR.Text = classic.AR.ToString();


              //override Run3 data, Page 1
              classic.FWA = 905;
              classic.LWA = 578;
              classic.Vnav = 4.0;
              classic.Vckref = 4.0;
              classic.NumEng = 1;
              classic.BlockC = 0.884;
              //
              In_txtFWA.Text = classic.FWA.ToString();
              In_txtLWA.Text = classic.LWA.ToString();
              In_txtVnav.Text = classic.Vnav.ToString();
              In_txtNumEng.Text = classic.NumEng.ToString();
              In_txtBlockC.Text = classic.BlockC.ToString();
              Out_txtVckref.Text = classic.Vckref.ToString();

              //override Run4 data, Page 2
              classic.S = 8591;
              classic.K = classic.Eqn_K();
              In_txtS.Text = classic.S.ToString();
              In_txtK.Text = Math.Round(classic.K, 5).ToString();
              classic.Rapp = 25;
              In_txtRapp.Text = classic.Rapp.ToString();

              //override Run5 data, Page 2
              classic.TA = 1;
              classic.TF = 2;
              classic.WaveAmp = 2;
              classic.kyy = 0.25;
              classic.E_para = 35.42;
              //
              In_txtBmax.Text = classic.BWL.ToString();
              In_txtTA.Text = classic.TA.ToString();
              In_txtTF.Text = classic.TF.ToString();
              In_txtWA.Text = classic.WaveAmp.ToString();
              In_txtkyy.Text = classic.kyy.ToString();
              In_txtE_para.Text = classic.E_para.ToString();

              //new Raw
              for (int j = 0; j < 18; j++)
              {
              double D2 = j * 0.5 + 7;
              classic.Raw = Math.Round((-1.2686 * Math.Pow(D2, 3) + 34.011 * Math.Pow(D2, 2) - 284.75 * (D2) + 1536.3),3);
              Intxt_Raw.Text = classic.Raw.ToString();
              System.Diagnostics.Debug.WriteLine(classic.Raw);

              Run5_addrow();
              }


              //override Run6 data
              classic.PropellerType = "FPP";
              classic.NumBlades = "5";
              classic.Dp = 7.2;
              classic.Layout = "midship";
              classic.nrpm = 81.7;
              classic.J_limit = 2.0;
              classic.J_interval = 0.05;

              //~~~~~~Using Pitch ratio 1.0 for MAU4-70~~~~~~~~~
              classic.KTa = -0.083683;
              classic.KTb = -0.359590;
              classic.KTc = 0.477629;
              classic.KQa = -0.132168;
              classic.KQb = -0.447287;
              classic.KQc = 0.695993;

              //
              In_txtPropType.Text = classic.PropellerType.ToString();
              In_txtNumBlades.Text = classic.NumBlades.ToString();
              In_txtDp.Text = classic.Dp.ToString();
              In_listboxLayout.Text = classic.Layout;
              In_txtJ_Limit.Text = classic.J_limit.ToString();
              In_J_Interval.Text = classic.J_interval.ToString();
              In_txtRPM.Text = classic.nrpm.ToString();

              In_txtKTa.Text = classic.KTa.ToString();
              In_txtKTb.Text = classic.KTb.ToString();
              In_txtKTc.Text = classic.KTc.ToString();
              In_txtKQa.Text = classic.KQa.ToString();
              In_txtKQb.Text = classic.KQb.ToString();
              In_txtKQc.Text = classic.KQc.ToString();
              //

              /*
              //Comments
              //to set as comment before publish!!
              classic.Addcom_DNVGL = "Sample DNVGL";
              classic.Addcom_IMONum = "Sample IMONum";
              classic.Addcom_VesselName = "Sample VesselName";
              classic.Addcom_BuildingYard = "Sample BuildingYard";
              classic.Addcom_HullNum = "Sample HullNum";
              classic.Addcom_LightShipWeight = "Sample LightShipWeight";
              classic.Addcom_EngineManufacturer = "Sample EngineManufacturer";
              classic.Addcom_EngineType = "Sample EngineType";
              classic.Addcom_DWT = "Sample DWT";
              classic.Addcom_MCR = "Sample MCR";
              classic.Addcom_ShipType = "Sample ShipType";
              classic.Addcom_a = "Sample a";
              classic.Addcom_b = "Sample b";
              classic.Addcom_MPP = "Sample MPP";
              classic.Addcom_Result = "Sample Result";
              classic.Addcom_BWL = "Sample BWL";
              classic.Addcom_Tm = "Sample Tm";
              classic.Addcom_LPP = "Sample LPP";
              classic.Addcom_AR = "Sample AR";
              classic.Addcom_ALScor = "Sample ALScor";
              classic.Addcom_PerALScor = "Sample PerALScor";
              classic.Addcom_FWA = "Sample FWA";
              classic.Addcom_LWA = "Sample LWA";
              classic.Addcom_Ratio = "Sample Ratio";
              classic.Addcom_Vnav = "Sample Vnav";
              classic.Addcom_Vckref = "Sample Vckref";
              classic.Addcom_Vck = "Sample Vck";
              classic.Addcom_Vs = "Sample Vs";
              classic.Addcom_NumEngine = "Sample NumEngine";
              classic.Addcom_BlockC = "Sample BlockC";
              classic.Addcom_Temp = "Sample Temp";
              classic.Addcom_RowAir = "Sample RowAir";
              classic.Addcom_ViscH2o = "Sample ViscH2o";
              classic.Addcom_RowH2o = "Sample RowH2o";
              classic.Addcom_S = "Sample s";
              classic.Addcom_k = "Sample k";
              classic.Addcom_Rey = "Sample Rey";
              classic.Addcom_Vw = "Sample Vw";
              classic.Addcom_Cf = "Sample Cf";
              classic.Addcom_Rcw = "Sample Rcw";
              classic.Addcom_Rair = "Sample Rair";
              classic.Addcom_Cair = "Sample Cair";
              classic.Addcom_Rapp = "Sample Rapp";
              */

              RunALL();
              System.Diagnostics.Debug.WriteLine("Sample A ran");
              //MessageBox.Show("    In the next step, choose the location to save the sampled PDF    ", "Sample", MessageBoxButtons.OK, MessageBoxIcon.None);
              //ExportPDF();
              }
              private void SAMPLE_B()
              {
              //override Run1 data, Page 1
              classic.DWT = 208001;
              classic.MCR = 14360;
              classic.ST = "Bulk Carrier";
              //
              In_txtDWT.Text = classic.DWT.ToString();
              In_txtMCR.Text = classic.MCR.ToString();
              In_listboxShipType.Text = classic.ST;

              //override Run2 data, Page 1
              classic.BWL = 50;
              classic.Tm = 18.2;
              classic.Lpp = 295;
              classic.AR = 92;
              //
              In_txtBWL.Text = classic.BWL.ToString();
              In_txtTm.Text = classic.Tm.ToString();
              In_txtLpp.Text = classic.Lpp.ToString();
              In_txtAR.Text = classic.AR.ToString();


              //override Run3 data, Page 1
              classic.FWA = 853;
              classic.LWA = 3078;
              classic.Vnav = 4.0;
              classic.Vckref = 6.0;
              classic.NumEng = 1;
              classic.BlockC = 0.855;
              //
              In_txtFWA.Text = classic.FWA.ToString();
              In_txtLWA.Text = classic.LWA.ToString();
              In_txtVnav.Text = classic.Vnav.ToString();
              In_txtNumEng.Text = classic.NumEng.ToString();
              In_txtBlockC.Text = classic.BlockC.ToString();
              Out_txtVckref.Text = classic.Vckref.ToString();

              //override Run4 data, Page 2
              classic.S = 21738;
              classic.K = classic.Eqn_K();
              //
              In_txtS.Text = classic.S.ToString();
              In_txtK.Text = Math.Round(classic.K, 5).ToString();


              //override Run5 data, Page 2
              classic.TA = 1;
              classic.TF = 2;
              classic.WaveAmp = 2;
              classic.kyy = 0.25;
              //
              In_txtBmax.Text = classic.BWL.ToString();
              In_txtTA.Text = classic.TA.ToString();
              In_txtTF.Text = classic.TF.ToString();
              In_txtWA.Text = classic.WaveAmp.ToString();
              In_txtkyy.Text = classic.kyy.ToString();
              classic.Eqn_E_para();
              In_txtE_para.Text = Math.Round(classic.Eqn_E_para(), 6).ToString();

              //override Run6 data
              classic.nrpm = 90;
              classic.Dp = 8.4;
              }
              private Boolean RunInput1()
              {
              if (In_listboxShipType.Text == string.Empty ||
                     In_txtDWT.Text == string.Empty ||
                     In_txtMCR.Text == string.Empty)
              {
              MessageBox.Show("No value(s) found!" + "\n" + "Check Section 1", "Empty Inputs!", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return false;
              }
              else
              {
              In_txtShipType.Text = In_listboxShipType.Text; //input, transferring Ship type to invisible box
              classic.DWT = float.Parse(In_txtDWT.Text);
              if (classic.DWT < 20000)
              {
              MessageBox.Show("Procedure doesn't apply! Read more under regulation 21 of MARPOL Annex VI during phase 0 and phase 1", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
              classic.MCR = float.Parse(In_txtMCR.Text);
              classic.ST = In_listboxShipType.Text;
              return true;
              }

              }

              private Boolean RunInput2()
              {
              if (In_txtBWL.Text == string.Empty ||
                     In_txtTm.Text == string.Empty ||
                     In_txtLpp.Text == string.Empty ||
                     In_txtAR.Text == string.Empty)
              {
              MessageBox.Show("No value(s) found!" + "\n" + "Check Section 2", "Empty Inputs!", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return false;
              }
              else
              {
              classic.BWL = float.Parse(In_txtBWL.Text);
              classic.Tm = float.Parse(In_txtTm.Text);
              classic.Lpp = float.Parse(In_txtLpp.Text);
              classic.AR = float.Parse(In_txtAR.Text);
              return true;
              }
              }
              private Boolean RunInput3()
              {
              if (In_txtFWA.Text == string.Empty ||
                     In_txtLWA.Text == string.Empty ||
                     In_txtVnav.Text == string.Empty ||
                     In_txtNumEng.Text == string.Empty ||
                     In_txtBlockC.Text == string.Empty)
              {
              MessageBox.Show("No value(s) found!" + "\n" + "Check Section 3", "Empty Inputs!", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return false;
              }
              else
              {
              classic.FWA = float.Parse(In_txtFWA.Text);
              classic.LWA = float.Parse(In_txtLWA.Text);
              classic.Vnav = float.Parse(In_txtVnav.Text);
              classic.NumEng = float.Parse(In_txtNumEng.Text);
              classic.BlockC = float.Parse(In_txtBlockC.Text);
              return true;
              }
              }
              private Boolean RunInput4()
              {
              if (In_txtRowAir.Text == string.Empty ||
                     In_txtViscH2o.Text == string.Empty ||
                     In_txtRowH2o.Text == string.Empty ||
                     In_txtS.Text == string.Empty ||
                     In_txtRapp.Text == string.Empty)
              {
              MessageBox.Show("No value(s) found!" + "\n" + "Check Section 4", "Empty Inputs!", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return false;
              }
              else
              {
              classic.RowAir = float.Parse(In_txtRowAir.Text);
              classic.ViscH2o = float.Parse(In_txtViscH2o.Text);
              classic.RowH2o = float.Parse(In_txtRowH2o.Text);
              classic.S = float.Parse(In_txtS.Text);
              classic.Rapp = float.Parse(In_txtRapp.Text);

              if (GetNumbers(In_txtK.Text) == null || In_txtK.Text == "nil")
              {
              classic.K = classic.Eqn_K();
              }
              else if (float.Parse(GetNumbers(In_txtK.Text)) > 0)
              {
              classic.K = float.Parse(In_txtK.Text);
              }
              else
              {
              MessageBox.Show("Check for 'k'" + "\n" + "Hover mouse over text 'K' to see instructions", "Wrong Inputs", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
              return true;
              }

              }
              /*
              private Boolean RunInput5() //old
              {
              if (In_txtkyy.Text == string.Empty ||
              In_txtIyy.Text == string.Empty)
              {
              kyyiyyInfo();
              return false;
              }
              if (In_txtg.Text == string.Empty ||
                     In_txtTA.Text == string.Empty ||
                     In_txtTF.Text == string.Empty ||
                     In_txtWA.Text == string.Empty ||
                     In_txtE_para.Text == string.Empty ||
                     In_txtBmax.Text == string.Empty)
              {
              MessageBox.Show("No value(s) found!" + "\n" + "Check Section 5", "Empty Inputs!", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return false;
              }
              else
              {
              classic.g = float.Parse(In_txtg.Text);

              return true;
              }
              }
              */
              private Boolean RunInput5()
              {
              
              if (In_txtg.Text == string.Empty)
              {
              MessageBox.Show("No value(s) found!" + "\n" + "Check Section 5", "Empty Inputs!", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return false;
              }
              else
              {
              classic.g = float.Parse(In_txtg.Text);

              return true;
              }
              }
              private Boolean RunInput6()
              {
              //System.Diagnostics.Debug.WriteLine("checkpoint input 6");
              if (In_txtDp.Text == string.Empty ||
              In_listboxLayout.Text == string.Empty ||
              In_txtRReff.Text == string.Empty ||
              In_txtJ_Limit.Text == string.Empty ||
              In_J_Interval.Text == string.Empty ||
              In_listboxLayout.Text == string.Empty)
              {
              MessageBox.Show("No value(s) found!" + "\n" + "Check Section 6", "Empty Inputs!", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return false;
              }
              else
              {
              if (In_txtEngineManufacturer.Text == string.Empty)
              {
              classic.EngineManufacturer = "nil";
              }
              else
              {
              classic.EngineManufacturer = In_txtEngineManufacturer.Text;
              }
              if (In_txtEngineType.Text == string.Empty)
              {
              classic.EngineType = "nil";
              }
              else
              {
              classic.EngineType = In_txtEngineType.Text;
              }

              classic.Dp = float.Parse(In_txtDp.Text);
              //In_txtShipType.Text = In_listboxShipType.Text; //input, transferring Ship type to invisible box
              classic.Layout = In_listboxLayout.Text;
              classic.RReff = float.Parse(In_txtRReff.Text);
              classic.J_limit = float.Parse(In_txtJ_Limit.Text);
              classic.J_interval = float.Parse(In_J_Interval.Text);

              classic.KTa = float.Parse(In_txtKTa.Text);
              classic.KTb = float.Parse(In_txtKTb.Text);
              classic.KTc = float.Parse(In_txtKTc.Text);
              classic.KQa = float.Parse(In_txtKQa.Text);
              classic.KQb = float.Parse(In_txtKQb.Text);
              classic.KQc = float.Parse(In_txtKQc.Text);
              return true;
              }
              }
              private void RunALL()
              {
              Run0();
              Run1();
              Run6();
              //MessageBox.Show("         Run Complete         ");
              }
              private void Run0()
              {
              //0RunInputs
              if (In_txtDNVGL.Text == string.Empty)
              {
              classic.DNVGL = "nil";
              }
              else
              {
              classic.DNVGL = In_txtDNVGL.Text;
              }

              if (In_txtIMONum.Text == string.Empty)
              {
              classic.IMONum = "nil";
              }
              else
              {
              classic.IMONum = In_txtIMONum.Text;
              }

              if (In_txtVesselName.Text == string.Empty)
              {
              classic.VesselName = "nil";
              }
              else
              {
              classic.VesselName = In_txtVesselName.Text;
              }
              if (In_txtBuildingYard.Text == string.Empty)
              {
              classic.BuildingYard = "nil";
              }
              else
              {
              classic.BuildingYard = In_txtBuildingYard.Text;
              }
              if (In_txtHullNum.Text == string.Empty)
              {
              classic.HullNum = "nil";
              }
              else
              {
              classic.HullNum = In_txtHullNum.Text;
              }
              if (In_txtLSW.Text == string.Empty)
              {
              classic.LSW = "nil";
              }
              else
              {
              classic.LSW = In_txtLSW.Text;
              }
              if (In_txtEngineManufacturer.Text == string.Empty)
              {
              classic.EngineManufacturer = "nil";
              }
              else
              {
              classic.EngineManufacturer = In_txtEngineManufacturer.Text;
              }
              if (In_txtEngineType.Text == string.Empty)
              {
              classic.EngineType = "nil";
              }
              else
              {
              classic.EngineType = In_txtEngineType.Text;
              //System.Diagnostics.Debug.WriteLine("Checkpoint SubRun0");
              }
              //System.Diagnostics.Debug.WriteLine("Checkpoint Run0");

              }
              private void Run1()
              {
              if (RunInput1())
              {
              //outputs
              double limitMax;
              
              if (classic.MCR < classic.Eqn_MPP())
              {
              Out_txtResult.BackColor = Color.Red;
              Out_txtResult.Text = "UNSATISFACTORY";
              //graph Y axis limits
              limitMax = Math.Round(classic.Eqn_MPP());
              }
              else
              {
              Out_txtResult.BackColor = Color.Green;
              Out_txtResult.Text = "SATISFACTORY";
              //graph Y axis limits
              limitMax = Math.Round(float.Parse(In_txtMCR.Text));
              }
              

              //outputs text
              Out_txtMPP.Text = Math.Round(classic.Eqn_MPP()).ToString();
              classic.Eqn_MPP();
              Out_txta.Text = classic.a.ToString();
              Out_txtb.Text = classic.b.ToString();

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              //Chart 1 >> MPP and MCR
              chartMPP.Series.Clear();
              var LineGraph_MCR = chartMPP.Series.Add("MCR");
              var LineGraph_MPP = chartMPP.Series.Add("MPP");
              chartMPP.Series["MCR"].ChartType = SeriesChartType.Point;
              chartMPP.Series["MCR"].MarkerSize = 5;
              chartMPP.Series["MCR"].MarkerStyle = MarkerStyle.Circle;
              chartMPP.Series["MCR"].Color = Color.Orange;
              chartMPP.Series["MPP"].ChartType = SeriesChartType.Line;
              chartMPP.ChartAreas[0].AxisY.Minimum = Math.Round((classic.a * 20000) + classic.b - 1000);
              chartMPP.ChartAreas[0].AxisY.Maximum = Math.Round(limitMax * 1.1);
              chartMPP.ChartAreas[0].AxisX.Minimum = 0;
              chartMPP.ChartAreas[0].AxisX.Maximum = Math.Round(classic.DWT * 1.1);
              chartMPP.ChartAreas[0].AxisY.Interval = 1000;
              chartMPP.ChartAreas[0].AxisX.Interval = 20000;
              LineGraph_MCR.Points.AddXY(classic.DWT, Single.Parse(In_txtMCR.Text));


              for (int i = 20000; i < classic.DWT; i++)
              {
              var newGraphMPP = (double)((classic.a * i) + classic.b);
              LineGraph_MPP.Points.AddXY(i, newGraphMPP);
              }
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              if (classic.DWT < 20000)
              {
              Out_txtResult.BackColor = Color.Red;
              Out_txtResult.Text = "UNSATISFACTORY";
              chartMPP.Series.Clear();
              Out_txtMPP.Text = "Error";
              Out_txta.Text = "Error";
              Out_txtb.Text = "Error";
              }
              }
              }
              private void Run2()
              {
              if (RunInput2())
              {

              //output text
              Out_txtALScor.Text = Math.Round(classic.Eqn_ALScor()).ToString();
              Out_txtPerALS.Text = Math.Round(classic.Eqn_PerALS(), 3).ToString();
              //
              In_txtBmax.Text = In_txtBWL.Text;
              classic.Eqn_HsVw();
              }
              }
              private void Run3()
              {
              Run2();
              if (RunInput3())
              {

              //output text
              Out_txtRatioFL.Text = Math.Round(classic.Eqn_RatioFL(), 3).ToString();
              Out_txtVckref.Text = classic.Eqn_Vckref().ToString();
              Out_txtVck.Text = Math.Round(classic.Eqn_Vck(), 3).ToString();
              Out_txtVs.Text = Math.Round(classic.Eqn_Vs(), 3).ToString();
              }
              }
              private void Run4()
              {
              Run3();
              if (RunInput4())
              {

              //output text
              if (In_txtK.Text == string.Empty || In_txtK.Text == "nil")
              {
              In_txtK.Text = Math.Round(classic.Eqn_K(), 5).ToString();
              }
              Out_txtRey.Text = (Math.Round(classic.Eqn_Rey()) / Math.Pow(10, 5)).ToString();
              classic.Eqn_HsVw();
              Out_txtVw.Text = Math.Round(classic.Vw, 2).ToString();
              Out_txtCf.Text = Math.Round(classic.Eqn_Cf(), 6).ToString();
              Out_txtRcw.Text = Math.Round(classic.Eqn_Rcw(), 3).ToString();
              Out_txtRair.Text = Math.Round(classic.Eqn_Rair(), 3).ToString();
              Out_txtCair.Text = classic.Cair.ToString();


              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Chart 3  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              chartRsubtotal.Series.Clear();

              var StackedGraph_Rapp = chartRsubtotal.Series.Add("Rapp");
              var StackedGraph_Rair = chartRsubtotal.Series.Add("Rair");
              var StackedGraph_Rcw = chartRsubtotal.Series.Add("Rcw");

              if (classic.Rapp > 0)
              {
              chartRsubtotal.Series["Rapp"].IsValueShownAsLabel = true;
              }
              else
              {
              chartRsubtotal.Series["Rapp"].IsValueShownAsLabel = false;
              }
              chartRsubtotal.Series["Rair"].IsValueShownAsLabel = true;
              chartRsubtotal.Series["Rcw"].IsValueShownAsLabel = true;

              chartRsubtotal.Series["Rapp"].ChartType = SeriesChartType.StackedBar;
              chartRsubtotal.Series["Rair"].ChartType = SeriesChartType.StackedBar;
              chartRsubtotal.Series["Rcw"].ChartType = SeriesChartType.StackedBar;

              chartRsubtotal.ChartAreas[0].AxisY.Minimum = 0;
              chartRsubtotal.ChartAreas[0].AxisY.Maximum = 1.1 * Math.Round(classic.Eqn_Rair() + classic.Eqn_Rcw() + classic.Rapp);
              chartRsubtotal.ChartAreas[0].AxisY.Interval = 50;

              StackedGraph_Rapp.Points.AddXY("total", float.Parse(In_txtRapp.Text));
              StackedGraph_Rair.Points.AddXY("total", Math.Round(float.Parse(Out_txtRair.Text)));
              StackedGraph_Rcw.Points.AddXY("total", Math.Round(float.Parse(Out_txtRcw.Text)));

              Out_txtRsubtotal.Text = (Math.Round(classic.Eqn_Rair() + classic.Eqn_Rcw() + classic.Rapp)).ToString();
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              }
              }



              private void Run5_addrow()
              {
              Run4();
              if (RunInput5())
              {
              {
              int count = classic.count;
              double Raw = classic.Raw;
              //dataGridView_Raw.Rows[0].Cells["Raw"].Style.BackColor = Color.LightYellow;
              //dataGridView_Raw.Rows[count].Cells["Raw"].Style.BackColor = Color.LightYellow;
              if (Intxt_Raw.Text == string.Empty || Intxt_Raw.Text == "nil" || float.Parse(Intxt_Raw.Text) == 0)
              {
              //insert null
              dataGridView_Raw.Rows[count].Cells["Raw"].Value = "nil";
              dataGridView_Raw.Rows[count].Cells["Rtotal"].Value = "nil";
              dataGridView_Raw.Rows[count].Cells["Thrust"].Value = "nil";

              Out_txtstatus.Text = " No data added";

              var StackedColumn_Rapp = chartRAW.Series["Rapp"];
              var StackedColumn_Rair = chartRAW.Series["Rair"];
              var StackedColumn_Rcw = chartRAW.Series["Rcw"];
              var StackedColumn_Raw = chartRAW.Series["Raw"];

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ R_total StackedBarGraph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              StackedColumn_Raw.Points.AddXY(("Tp[ " + (count * 0.5 + 7).ToString() + " ]"), "");
              StackedColumn_Rcw.Points.AddXY(("Tp[ " + (count * 0.5 + 7).ToString() + " ]"), "");
              StackedColumn_Rair.Points.AddXY(("Tp[ " + (count * 0.5 + 7).ToString() + " ]"), "");
              StackedColumn_Rapp.Points.AddXY(("Tp[ " + (count * 0.5 + 7).ToString() + " ]"), "");
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~StackedBarGraph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              }
              else if (count < 18)
              {
              if (Out_txtRcw.Text != string.Empty && Out_txtRair.Text != string.Empty && In_txtRapp.Text != string.Empty)
              {
              Out_txtstatus.Text = "";
              Out_txtFroude.Text = Math.Round(classic.Eqn_Fr(), 6).ToString();
              classic.Eqn_HsVw();
              Out_txtHs.Text = classic.Hs.ToString();
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              Raw = float.Parse(Intxt_Raw.Text);
              dataGridView_Raw.Rows[count].Cells["Raw"].Value = Math.Round(Raw, 2) + " kN";
              dataGridView_Raw.Rows[count].Cells["Rtotal"].Value = Math.Round(classic.Eqn_Rsubtotal() + Raw) + " kN";
              var Thrust = (classic.Eqn_Rsubtotal() + Raw) / (1 - classic.Eqn_TDF());
              dataGridView_Raw.Rows[count].Cells["Thrust"].Value = Math.Round(Thrust) + " kN";
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              //Chart 3 >> TOTAL Resistances
              //order matters
              var StackedColumn_Rapp = chartRAW.Series["Rapp"];
              var StackedColumn_Rair = chartRAW.Series["Rair"];
              var StackedColumn_Rcw = chartRAW.Series["Rcw"];
              var StackedColumn_Raw = chartRAW.Series["Raw"];

              if (classic.Rapp > 0)
              {
              StackedColumn_Rapp.IsValueShownAsLabel = true;
              }
              else
              {
              StackedColumn_Rapp.IsValueShownAsLabel = false;
              }

              StackedColumn_Rapp.ChartType = SeriesChartType.StackedColumn;
              StackedColumn_Rair.ChartType = SeriesChartType.StackedColumn;
              StackedColumn_Rcw.ChartType = SeriesChartType.StackedColumn;
              StackedColumn_Raw.ChartType = SeriesChartType.StackedColumn;

              StackedColumn_Rair.IsValueShownAsLabel = true;
              StackedColumn_Rcw.IsValueShownAsLabel = true;
              StackedColumn_Raw.IsValueShownAsLabel = true;

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ R_total StackedBarGraph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              StackedColumn_Raw.Points.AddXY(("Tp[ " + (count * 0.5 + 7).ToString() + " ]"), Math.Round(Raw));
              StackedColumn_Rcw.Points.AddXY(("Tp[ " + (count * 0.5 + 7).ToString() + " ]"), Math.Round(float.Parse(Out_txtRcw.Text)));
              StackedColumn_Rair.Points.AddXY(("Tp[ " + (count * 0.5 + 7).ToString() + " ]"), Math.Round(float.Parse(Out_txtRair.Text)));
              StackedColumn_Rapp.Points.AddXY(("Tp[ " + (count * 0.5 + 7).ToString() + " ]"), Math.Round(float.Parse(In_txtRapp.Text)));
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~StackedBarGraph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\

              chartRAW.ChartAreas[0].AxisY.Minimum = 0;
              chartRAW.ChartAreas[0].AxisX.Interval = 1;
              chartRAW.ChartAreas[0].AxisX.Minimum = 0;
              chartRAW.ChartAreas[0].AxisX.Maximum = classic.count + 2;

              if (Raw >= classic.RawHeight)
              {
              classic.RawHeight = Raw;
              chartRAW.ChartAreas[0].AxisY.Maximum = Math.Floor(1.05 * (classic.Eqn_Rair() + classic.Eqn_Rcw() + classic.Rapp + classic.RawHeight));
              chartRAW.ChartAreas[0].AxisY.Interval = Math.Ceiling((classic.Eqn_Rair() + classic.Eqn_Rcw() + classic.Rapp + classic.RawHeight) / 10);
              }
              else
              {
              chartRAW.ChartAreas[0].AxisY.Maximum = Math.Floor(1.05 * (classic.Eqn_Rair() + classic.Eqn_Rcw() + classic.Rapp + classic.RawHeight));
              chartRAW.ChartAreas[0].AxisY.Interval = Math.Ceiling((classic.Eqn_Rair() + classic.Eqn_Rcw() + classic.Rapp + classic.RawHeight) / 10);
              }
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              }
              }
              }
              System.Diagnostics.Debug.WriteLine("count = " + classic.count);
              System.Diagnostics.Debug.WriteLine("Raw = " + Raw);
              classic.count++;
              }
              else
              {
              classic.Rcw = 0;
              classic.Rair = 0;
              }
              
              }

              private void Run5_minusrow()
              {
              Out_txtstatus.Text = "";
              int count = classic.count;
              if (count >= 1)
              {
              System.Diagnostics.Debug.WriteLine("count = " + classic.count);
              dataGridView_Raw.Rows[count].Cells["Raw"].Style.BackColor = Color.White;
              DialogResult dialogResult = MessageBox.Show("    Confirm?    ", "Some Title", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              classic.count--;

              dataGridView_Raw.Rows[count].Cells["Raw"].Value = "";
              dataGridView_Raw.Rows[count].Cells["Rtotal"].Value = "";
              dataGridView_Raw.Rows[count].Cells["Thrust"].Value = "";
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~StackedBarGraph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              var StackedColumn_Rapp = chartRAW.Series["Rapp"];
              var StackedColumn_Rair = chartRAW.Series["Rair"];
              var StackedColumn_Rcw = chartRAW.Series["Rcw"];
              var StackedColumn_Raw = chartRAW.Series["Raw"];

              StackedColumn_Raw.Points.RemoveAt(count);
              StackedColumn_Rcw.Points.RemoveAt(count);
              StackedColumn_Rair.Points.RemoveAt(count);
              StackedColumn_Rapp.Points.RemoveAt(count);

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~StackedBarGraph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              }
              else if (dialogResult == DialogResult.No)
              {
              //do nothing
              }
              }
              else
              {
              MessageBox.Show("     It is the lowest row!    ", " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
              }

              public void Run5()
              {

              //Output text
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              dataGridView_Raw.Rows.Add(18);
              for (int i = 0; i < 18; i++)
              {
              dataGridView_Raw.Rows[i].Cells["Tps"].Value = Math.Round((double)(i * 0.5 + 7), 1);

              }

              }




              private void Run6()
              {
              //Run5();
              if (RunInput6())
              {
              Out_txtFroude.Text = Math.Round(classic.Eqn_Fr(), 6).ToString();
              classic.Eqn_HsVw();
              Out_txtHs.Text = classic.Hs.ToString();

              RunJKTKQ_TableGraph();

              //Output Text
              Out_txtWake.Text = classic.Eqn_Wake().ToString();
              Out_txtTDF.Text = Math.Round(classic.Eqn_TDF(), 3).ToString();
              Out_txtUa.Text = Math.Round(classic.Eqn_Ua(), 2).ToString();
              Out_txtTransEff.Text = classic.Eqn_TransEff().ToString();
              //KT interception
              classic.J_at_KTintercept = classic.Eqn_SolveQuad((classic.KTa - classic.c7), classic.KTb, classic.KTc);


              //KT intercept, relies on KTa,KTb,KTc
              var LineGraph_KTintercept = chartJKTKQ.Series.Add("KTintercept");
              chartJKTKQ.Series["KTintercept"].ChartType = SeriesChartType.Point;
              chartJKTKQ.Series["KTintercept"].MarkerSize = 5;
              chartJKTKQ.Series["KTintercept"].MarkerStyle = MarkerStyle.Square;
              chartJKTKQ.Series["KTintercept"].Color = Color.Red;
              LineGraph_KTintercept.Points.AddXY(classic.J_at_KTintercept, classic.Eqn_KT(classic.J_at_KTintercept));
              //Output text
              Out_txtKTintercept.Text = "x: " + Math.Round(classic.J_at_KTintercept, 3).ToString() + "\n" + "y: " + Math.Round((classic.Eqn_KT(classic.J_at_KTintercept)), 3).ToString();

              //optimum shaft speed, n_opt
              var LineGraph_n_opt = chartJKTKQ.Series.Add("Optimal shaft speed");
              chartJKTKQ.Series["Optimal shaft speed"].ChartType = SeriesChartType.Point;
              chartJKTKQ.Series["Optimal shaft speed"].MarkerSize = 5;
              chartJKTKQ.Series["Optimal shaft speed"].MarkerStyle = MarkerStyle.Diamond;
              chartJKTKQ.Series["Optimal shaft speed"].Color = Color.DarkRed;
              LineGraph_n_opt.Points.AddXY(classic.J_at_KTintercept, classic.Eqn_eta0(classic.J_at_KTintercept));
              //Output text
              Out_txtn_opt.Text = "x: " + Math.Round(classic.J_at_KTintercept, 3).ToString() + "\n" + "y: " + Math.Round(classic.Eqn_eta0(classic.J_at_KTintercept), 3).ToString();

              }
              }
              private void RunJKTKQ_TableGraph()
              {
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              //Chart 4 >> J,KT,KQ,KTship
              chartJKTKQ.Series.Clear();
              var LineGraph_KT = chartJKTKQ.Series.Add("KT");
              var LineGraph_KQ = chartJKTKQ.Series.Add("10KQ");
              var LineGraph_eta0 = chartJKTKQ.Series.Add("eta0");
              var LineGraph_KTship = chartJKTKQ.Series.Add("KTship");
              chartJKTKQ.Series["KT"].ChartType = SeriesChartType.Line;
              chartJKTKQ.Series["10KQ"].ChartType = SeriesChartType.Line;
              chartJKTKQ.Series["eta0"].ChartType = SeriesChartType.Line;
              chartJKTKQ.Series["KTship"].ChartType = SeriesChartType.Line;


              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              this.dataGridView_J.DataSource = null;
              this.dataGridView_J.Rows.Clear();
              //Loop
              int length = (int)(classic.J_limit / classic.J_interval);
              for (int i = 0; i < length; i++)
              {
              double[] J = new double[length];
              double[] KT = new double[length];
              double[] KQ = new double[length];
              double[] eta0 = new double[length];
              double[] KTship = new double[length];
              //Advance Coefficient, J
              J[i] = Math.Round((i * classic.J_interval), 2);
              //Thrust Coefficient, KT
              classic.KT_J = classic.Eqn_KT(J[i]);
              KT[i] = Math.Round(classic.KT_J, 3);
              //Torque Coefficient, KQ, not in 10KQ
              classic.KQ_J = classic.Eqn_KQ(J[i]);
              KQ[i] = Math.Round(classic.KQ_J, 3);
              //Propeller Eff%,eta0
              classic.eta0 = classic.Eqn_eta0(J[i]);
              eta0[i] = Math.Round(classic.eta0, 3);
              //Optimum propeller speed
              int b = 7; //Peak wave for resistance to calculate
              classic.KTship0 = classic.Eqn_KTship0(classic.Eqn_Rtotal(b), J[i]);
              KTship[i] = Math.Round(classic.KTship0, 3);
              //Plot table
              dataGridView_J.Rows.Add(J[i], KT[i], KQ[i], eta0[i], KTship[i]);
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              LineGraph_KT.Points.AddXY(J[i], classic.KT_J);
              LineGraph_KQ.Points.AddXY(J[i], classic.KQ_J);
              LineGraph_eta0.Points.AddXY(J[i], classic.eta0);
              LineGraph_KTship.Points.AddXY(J[i], classic.KTship0);

              //Line graph scaling
              classic.chartXmax = J[i];
              RunChart4YScaling();
              RunChart4XScaling();


              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Graph~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              if (i > 2 && classic.eta0 < 0)
              {
              //System.Diagnostics.Debug.WriteLine("Checkpoint 2");
              //Trim table to the maximum of i value
              dataGridView_J.RowCount = i + 1;
              //Set Table to auto size
              dataGridView_J.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridView_J.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridView_J.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridView_J.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridView_J.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              break;
              }
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

              }
              //Line graph shifting the Legend
              //https://docs.devexpress.com/WindowsForms/115948/controls-and-libraries/chart-control/legends/adding-legends
              //https://help.syncfusion.com/windowsforms/chart/chart-legend-and-legend-items

              }
              private void EngineLoadDiagram()
              {
              /*
              //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              this.dataGridViewPower.DataSource = null;
              this.dataGridViewPower.Rows.Clear();
              for (int z = 1; z < 10; z++)
              {
                     int distance = 10;
                     double[] Tps = new double[distance];
                     double[] PT = new double[distance];
                     double[] Jpwr = new double[distance];
                     double[] n = new double[distance];
                     double[] PD = new double[distance];
                     double[] PB = new double[distance];
                     double[] PBmax = new double[distance];
                     //Peak Wave, Tps
                     Tps[z] = (z +6);
                     //Thrust
                     PT[z] = Math.Round((classic.Eqn_Rtotal(z + 6) / (1 - classic.Eqn_TDF())),2);
                     //KT/n^2
                     double alpha;
                     alpha = PT[z] / (classic.RowH2o * Math.Round(classic.Dp, 4));
                     //Advance Coefficient
                     double jerry;
                     jerry = classic.Eqn_SolveQuad(classic.KTa, classic.KTb, classic.KTc - classic.Eqn_KT()
                     Jpwr[z] = Math.Round((z * classic.J_interval), 2);
                     //Rev per minute, n
                     classic.nrpm = classic.Eqn_Rps(J[z]);
                     n[z] = Math.Round(classic.nrpm);
                     //Delivered Power,PD
                     classic.PD = classic.Eqn_PD(J[z]);
                     PD[z] = Math.Round(classic.PD,2);
                     //Brake Power, PB
                     classic.PB = classic.Eqn_PB(J[z]);
                     PB[z] = Math.Round(classic.PB,2);
                     //max Brake Power, PBmax


                     //Plot table
                     dataGridViewPower.Rows.Add(Tps[z], PT[z], Jpwr[z], n[z], PD[z], PB[z]);

              }
              // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Table~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
              */
              }
              private void Excel_Load_graph() //Excel Load
              {
              /*
              //To read the excel file of the Load diagrams
              string filePath = string.Empty;
              string fileExt = string.Empty;

              filePath = @"C:\Users\Neone\Desktop\1NTU Files\Y4S2\1FYP\Propeller Excel";
              fileExt = Path.GetExtension(filePath);
              //
              var dtexcel = dataGridViewEL1;
              var dtexcel2 = dataSet;

              //
              string conn = string.Empty;
              if (fileExt.CompareTo(".xls") == 0)
              {
                     conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1';"; //for below excel 2007  

              }
              else
              {
                     conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes';"; //for above excel 2007  
                     System.Diagnostics.Debug.WriteLine("Reached here 1");
              }


              /////////////////////
              OleDbConnection con = new OleDbConnection(conn);

              OleDbDataAdapter myDataAdapter2 = new OleDbDataAdapter("Select * from [Abs load table$]", con); //here we read data from sheet1 
              myDataAdapter2.Fill(dtexcel2); //fill excel data into dataTable



              // Using DataTable, plot the curves accordingly with respect to each column name
              // Since X is a common variable across all lines, read it once only.       
              // 3rd Graph Plot
              string G3_y_Axis_header1 = "P continuous";
              string G3_y_Axis_header2 = "Max# Speed";
              string G3_y_Axis_header3 = "Prop# curve S";
              string G3_y_Axis_header4 = "Prop# curve L";
              string G3_y_Axis_header5 = "Propeller Design Point";
              string G3_y_Axis_header6 = "Service Propulsion Point";
              string G3_y_Axis_header7 = "Specified Propulsion Point";
              string G3_y_Axis_header8 = "Prop# Curve Calm Water";
              string G3_y_Axis_header9 = "Prop# Curve, Hs=3m, Vair=10m/s";
              string G3_y_Axis_header10 = "Prop# Curve, Hs=4#2m, Vair=15m/s";
              string G3_x_Axis_header1 = "Shaft Speed";


              //Plot the Boundary Lines of each Variable

              List<string> G3_x1_value = new List<string>(dtexcel2.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_x1_value.Add(row[G3_x_Axis_header1].ToString());

              List<string> G3_y1_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y1_value.Add(row[G3_y_Axis_header1].ToString());

              List<string> G3_y2_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y2_value.Add(row[G3_y_Axis_header2].ToString());

              List<string> G3_y3_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y3_value.Add(row[G3_y_Axis_header3].ToString());

              List<string> G3_y4_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y4_value.Add(row[G3_y_Axis_header4].ToString());

              List<string> G3_y5_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y5_value.Add(row[G3_y_Axis_header5].ToString());

              List<string> G3_y6_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y6_value.Add(row[G3_y_Axis_header6].ToString());

              List<string> G3_y7_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y7_value.Add(row[G3_y_Axis_header7].ToString());

              List<string> G3_y8_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y8_value.Add(row[G3_y_Axis_header8].ToString());

              List<string> G3_y9_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y9_value.Add(row[G3_y_Axis_header9].ToString());

              List<string> G3_y10_value = new List<string>(dtexcel.Rows.Count);
              foreach (DataRow row in dtexcel2.Rows)
                     G3_y10_value.Add(row[G3_y_Axis_header10].ToString());

              chart_EngineLoad.Series.Add("P Continuous");
              chart_EngineLoad.Series.Add("P Max Speed");
              chart_EngineLoad.Series.Add("Prop. Curve S");
              chart_EngineLoad.Series.Add("Prop. Curve L");
              chart_EngineLoad.Series.Add("Propeller Design Point");
              chart_EngineLoad.Series.Add("Service Propulsion Point");
              chart_EngineLoad.Series.Add("Specified Propulsion Point");
              chart_EngineLoad.Series.Add("Prop. Curve in Calm Water");
              chart_EngineLoad.Series.Add("Prop. Curve, Hs=3m, Vair=10m/s");
              chart_EngineLoad.Series.Add("Prop. Curve, Hs=4.2m, Vair=15m/s");

              chart_EngineLoad.Series["P Continuous"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
              chart_EngineLoad.Series["P Max Speed"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
              chart_EngineLoad.Series["Prop. Curve S"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
              chart_EngineLoad.Series["Prop. Curve L"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
              chart_EngineLoad.Series["Propeller Design Point"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
              chart_EngineLoad.Series["Service Propulsion Point"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
              chart_EngineLoad.Series["Specified Propulsion Point"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
              chart_EngineLoad.Series["Prop. Curve in Calm Water"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
              chart_EngineLoad.Series["Prop. Curve, Hs=3m, Vair=10m/s"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
              chart_EngineLoad.Series["Prop. Curve, Hs=4.2m, Vair=15m/s"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;



              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["P Continuous"].Points.AddXY(G3_x1_value[i], G3_y1_value[i]);
              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["P Max Speed"].Points.AddXY(G3_x1_value[i], G3_y2_value[i]);
              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["Prop. Curve S"].Points.AddXY(G3_x1_value[i], G3_y3_value[i]);
              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["Prop. Curve L"].Points.AddXY(G3_x1_value[i], G3_y4_value[i]);
              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["Propeller Design Point"].Points.AddXY(G3_x1_value[i], G3_y5_value[i]);
              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["Service Propulsion Point"].Points.AddXY(G3_x1_value[i], G3_y6_value[i]);
              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["Specified Propulsion Point"].Points.AddXY(G3_x1_value[i], G3_y7_value[i]);
              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["Prop. Curve in Calm Water"].Points.AddXY(G3_x1_value[i], G3_y8_value[i]);
              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["Prop. Curve, Hs=3m, Vair=10m/s"].Points.AddXY(G3_x1_value[i], G3_y9_value[i]);
              for (int i = 0; i < dtexcel2.Rows.Count; i++)
                     chart_EngineLoad.Series["Prop. Curve, Hs=4.2m, Vair=15m/s"].Points.AddXY(G3_x1_value[i], G3_y10_value[i]);
              */
              }

              private void In_J_Interval_SelectedIndexChanged(object sender, EventArgs e)
              {
              classic.J_interval = float.Parse(In_J_Interval.Text);
              RunJKTKQ_TableGraph();
              }
              private void textBox4_TextChanged(object sender, EventArgs e)
              {
              if (In_txtJ_Limit.Text != string.Empty)
              {
              classic.J_limit = float.Parse(In_txtJ_Limit.Text);
              }
              RunJKTKQ_TableGraph();
              }
              private void chartYmaxUPDOWN_SelectedItemChanged(object sender, EventArgs e)
              {
              classic.chartYmax = float.Parse(chartYmaxUPDOWN.Text);
              RunChart4YScaling();
              }
              private void chartXmaxUPDOWN_SelectedItemChanged(object sender, EventArgs e)
              {
              classic.chartXmax = float.Parse(chartXmaxUPDOWN.Text);
              RunChart4XScaling();
              }
              private void RunChart4YScaling() //Line graph manual scaling for Y
              {
              chartJKTKQ.ChartAreas[0].AxisY.Minimum = 0;
              chartJKTKQ.ChartAreas[0].AxisY.Maximum = classic.chartYmax;
              chartJKTKQ.ChartAreas[0].AxisY.Interval = 0.1;
              }
              private void RunChart4XScaling() //Line graph manual scaling for X
              {
              chartJKTKQ.ChartAreas[0].AxisX.Minimum = 0;
              chartJKTKQ.ChartAreas[0].AxisX.Maximum = classic.chartXmax;
              chartJKTKQ.ChartAreas[0].AxisX.Interval = 0.1;
              }

              private void In_buttonExportPDF_Click(object sender, EventArgs e)
              {
              //ExportPDF(PdfWriter writer);
              ExportPDF();
              }
              private static void formatCells(PdfPTable table, string text)
              {
              BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
              iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

              PdfPCell cell = new PdfPCell(new Phrase(text, times));
              cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
              cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
              table.AddCell(cell);
              }

              //https://foxlearn.com/articles/how-to-create-a-pdf-document-file-in-csharp-199.html
              //https://www.c-sharpcorner.com/UploadFile/f2e803/basic-pdf-creation-using-itextsharp-part-i/

              //https://www.c-sharpcorner.com/blogs/export-datagridview-data-to-pdf-in-c-sharp
              private void ExportPDF() //Save as PDF
              {
              using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF file|*.pdf", ValidateNames = true })
              {
              if (sfd.ShowDialog() == DialogResult.OK)
              {

              Document doc = new Document(PageSize.A4, 50f, 50f, 50f, 50f);
              try
              {
              //Save pdf file
              PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
              doc.Open();


              //Help
              /*
               * PdfContentByte cb = writer.DirectContent;
              cb.BeginText();
              //https://stackoverflow.com/questions/10717130/how-to-write-to-pdf-document-using-itextsharp
              var bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
              cb.SetColorFill(BaseColor.BLACK);
              cb.SetFontAndSize(bf, 18);
              */
              //Also, need to add straight line, but not working.
              // Help

              doc.Add(new Paragraph("Minimum Propulsion power"));//Sentence
              doc.Add(new Paragraph("following MEPC/Res.232(65) '2013 Interim guidelines for determining minimum propulsion power to maintain the manoeuvrability of ships in adverse conditions'")); //Sentence
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Ship Data"));
              doc.Add(new Paragraph(" ")); //spacing



              PdfPTable tbl1 = new PdfPTable(3);
              tbl1.DefaultCell.Padding = 3;
              tbl1.HorizontalAlignment = Element.ALIGN_CENTER;
              float tableWidth = 500f;
              float[] widthNoUnits = new float[] { 35f, 40f, 25f }; // for table with no units
              float[] widthsUnits = new float[] { 35f, 30f, 10f, 25f }; // for other tables with units
              tbl1.TotalWidth = tableWidth;
              tbl1.LockedWidth = true;
              tbl1.SetWidths(widthNoUnits);
              /*
              formatCells(tbl1, "test");
              formatCells(tbl1, "test2");
              formatCells(tbl1, "test3");
              */
              tbl1.AddCell("Title"); //Row 1
              tbl1.AddCell("Data");
              tbl1.AddCell("Comment");
              tbl1.AddCell("DNVGL"); //Row 1
              tbl1.AddCell(In_txtDNVGL.Text);
              tbl1.AddCell(classic.Addcom_DNVGL);
              tbl1.AddCell("IMO Number"); //Row 2
              tbl1.AddCell(In_txtIMONum.Text);
              tbl1.AddCell(classic.Addcom_IMONum);
              tbl1.AddCell("Vessel Name"); //Row 3
              tbl1.AddCell(In_txtVesselName.Text);
              tbl1.AddCell(classic.Addcom_VesselName);
              tbl1.AddCell("Ship Type"); //Row 4
              tbl1.AddCell(In_listboxShipType.Text);
              tbl1.AddCell(classic.Addcom_ShipType);
              tbl1.AddCell("Building Yard"); //Row 5
              tbl1.AddCell(In_txtBuildingYard.Text);
              tbl1.AddCell(classic.Addcom_BuildingYard);
              tbl1.AddCell("Hull Number"); //Row 6
              tbl1.AddCell(In_txtHullNum.Text);
              tbl1.AddCell(classic.Addcom_HullNum);

              doc.Add(tbl1); //Table 1 insert
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Environmental Factors")); // Sentence
              doc.Add(new Paragraph(" ")); //spacing

              PdfPTable tbl2 = new PdfPTable(4);
              tbl2.DefaultCell.Padding = 3;
              tbl2.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl2.TotalWidth = tableWidth;
              tbl2.LockedWidth = true;
              tbl2.SetWidths(widthsUnits);

              tbl2.AddCell("Parameters");//Heading row
              tbl2.AddCell("Value");
              tbl2.AddCell("Unit");
              tbl2.AddCell("Comment");
              tbl2.AddCell("Density of Air (ρ air)"); //Row 1
              tbl2.AddCell(In_txtRowAir.Text);
              tbl2.AddCell("kg/m^3");
              tbl2.AddCell(classic.Addcom_RowAir);
              tbl2.AddCell("Density of Water (ρ H2O)"); //Row 2
              tbl2.AddCell(In_txtRowH2o.Text);
              tbl2.AddCell("kg/m^3");
              tbl2.AddCell(classic.Addcom_RowH2o);
              tbl2.AddCell("Temperature (Temp)"); //Row 3
              tbl2.AddCell(In_txtTemp.Text);
              tbl2.AddCell("°C");
              tbl2.AddCell(classic.Addcom_Temp);
              tbl2.AddCell("Visocity of Water (ν H2O)"); //Row 4
              tbl2.AddCell(In_txtViscH2o.Text);
              tbl2.AddCell("m^2/s");
              tbl2.AddCell(classic.Addcom_ViscH2o);
              tbl2.AddCell("Gravity (g)"); //Row 5
              tbl2.AddCell(In_txtTemp.Text);
              tbl2.AddCell("m/s^2");
              tbl2.AddCell(classic.Addcom_Gravity);
              doc.Add(tbl2); //Table 2 insert
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Principal Particulars")); //Sentence
              doc.Add(new Paragraph(" ")); //spacing

              PdfPTable tbl3 = new PdfPTable(4);
              tbl3.DefaultCell.Padding = 3;
              tbl3.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl3.TotalWidth = tableWidth;
              tbl3.LockedWidth = true;
              tbl3.SetWidths(widthsUnits);

              tbl3.AddCell("Parameters");//Heading row
              tbl3.AddCell("Value");
              tbl3.AddCell("Unit");
              tbl3.AddCell("Comment");
              tbl3.AddCell("Length Between Perpendiculars (LPP)");//Row 1
              tbl3.AddCell(In_txtLpp.Text);
              tbl3.AddCell("m");
              tbl3.AddCell(classic.Addcom_LPP);
              tbl3.AddCell("Breadth on water line (BWL)");//Row 2
              tbl3.AddCell(In_txtBWL.Text);
              tbl3.AddCell("m");
              tbl3.AddCell(classic.Addcom_BWL);
              tbl3.AddCell("Scantling draft at midship (Tm)");//Row 3
              tbl3.AddCell(In_txtTm.Text);
              tbl3.AddCell("m");
              tbl3.AddCell(classic.Addcom_Tm);
              tbl3.AddCell("Light Ship Weight");//Row 4
              tbl3.AddCell(In_txtLSW.Text);
              tbl3.AddCell("ton");
              tbl3.AddCell(classic.Addcom_LightShipWeight);
              tbl3.AddCell("Deadweight Tonnage (DWT)");//Row 5
              tbl3.AddCell(In_txtDWT.Text);
              tbl3.AddCell("ton");
              tbl3.AddCell(classic.Addcom_DWT);
              tbl3.AddCell("Block Coefficient (BlockC)");//Row 6
              tbl3.AddCell(In_txtBlockC.Text);
              tbl3.AddCell(" ");
              tbl3.AddCell(classic.Addcom_BlockC);
              tbl3.AddCell("Wetted surface area (S)");//Row 7
              tbl3.AddCell(In_txtS.Text);
              tbl3.AddCell("m^2");
              tbl3.AddCell(classic.Addcom_S);
              tbl3.AddCell("Frontal Windage Area (FWA)");//Row 8
              tbl3.AddCell(In_txtFWA.Text);
              tbl3.AddCell("m^2");
              tbl3.AddCell(classic.Addcom_FWA);
              tbl3.AddCell("Lateral Windage Area (LWA)");//Row 9
              tbl3.AddCell(In_txtLWA.Text);
              tbl3.AddCell("m^2");
              tbl3.AddCell(classic.Addcom_LWA);
              tbl3.AddCell("Rudder Total Area (AR)");//Row 10
              tbl3.AddCell(In_txtAR.Text);
              tbl3.AddCell("m^2");
              tbl3.AddCell(classic.Addcom_AR);
              doc.Add(tbl3); //Table 3 insert
              doc.Add(new Paragraph(" ")); //spacing
              doc.NewPage(); 
              doc.Add(new Paragraph("Main Engine Particulars")); //Sentence
              doc.Add(new Paragraph(" ")); //spacing

              PdfPTable tbl4 = new PdfPTable(4);
              tbl4.DefaultCell.Padding = 3;
              tbl4.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl4.TotalWidth = tableWidth;
              tbl4.LockedWidth = true;
              tbl4.SetWidths(widthsUnits);

              tbl4.AddCell("Parameters"); //Heading row
              tbl4.AddCell("Value");
              tbl4.AddCell("Unit");
              tbl4.AddCell("Comment");
              tbl4.AddCell("Number of Engines");//Row 1
              tbl4.AddCell(In_txtNumEng.Text);
              tbl4.AddCell("");
              tbl4.AddCell(classic.Addcom_NumEngine);
              tbl4.AddCell("Manufacturer");//Row 2
              tbl4.AddCell(In_txtEngineManufacturer.Text);
              tbl4.AddCell("");
              tbl4.AddCell(classic.Addcom_EngineManufacturer);
              tbl4.AddCell("Engine Type");//Row 3
              tbl4.AddCell(In_txtEngineType.Text);
              tbl4.AddCell("");
              tbl4.AddCell(classic.Addcom_EngineType);
              tbl4.AddCell("Maximum Continuous" + "\n" + " Rating (MCR)");//Row 4
              tbl4.AddCell(In_txtMCR.Text);
              tbl4.AddCell("kW");
              tbl4.AddCell(classic.Addcom_MCR);
              tbl4.AddCell("RPM of main propulsion" + "\n" + " engine at MCR after" + "\n" + " installation of EPL ");//Row 5
              tbl4.AddCell(In_txtRPM.Text);
              tbl4.AddCell("rev/min");
              tbl4.AddCell(" "); // INSERT RPM 
              doc.Add(tbl4); // Table 4 insert

              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Propeller Particulars")); //Sentence
              doc.Add(new Paragraph(" ")); //spacing

              PdfPTable tbl5 = new PdfPTable(4);
              tbl5.DefaultCell.Padding = 3;
              tbl5.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl5.TotalWidth = tableWidth;
              tbl5.LockedWidth = true;
              tbl5.SetWidths(widthsUnits);

              tbl5.AddCell("Parameters"); //Heading row
              tbl5.AddCell("Value");
              tbl5.AddCell("Unit");
              tbl5.AddCell("Comment");
              tbl5.AddCell("Propeller Type");//Row 1
              tbl5.AddCell(In_txtPropType.Text);
              tbl5.AddCell(" ");
              tbl5.AddCell("");// INSERT PROP TYPE 
              tbl5.AddCell("Number of blades");//Row 2
              tbl5.AddCell(In_txtNumBlades.Text);
              tbl5.AddCell(" ");
              tbl5.AddCell("");// INSERT NUM BLADES 
              tbl5.AddCell("Diameter (Dp)");//Row 3
              tbl5.AddCell(In_txtDp.Text);
              tbl5.AddCell("m");
              tbl5.AddCell("");// INSERT DP 
              doc.Add(tbl5); // Table 5 insert

              doc.Add(new Paragraph(" ")); //spacing

              doc.NewPage();

              doc.Add(new Paragraph("Assessment Level 1 – Minimum Power Line"));
              doc.Add(new Paragraph(" ")); //spacing

              PdfPTable tbl6 = new PdfPTable(4);
              tbl6.DefaultCell.Padding = 3;
              tbl6.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl6.TotalWidth = tableWidth;
              tbl6.LockedWidth = true;
              tbl6.SetWidths(widthsUnits);

              tbl6.AddCell("Parameters"); //Heading row
              tbl6.AddCell("Value");
              tbl6.AddCell("Unit");
              tbl6.AddCell("Comment");
              tbl6.AddCell("DWT");//Row 1
              tbl6.AddCell(In_txtDWT.Text);
              tbl6.AddCell("ton");
              tbl6.AddCell(classic.Addcom_DWT);
              tbl6.AddCell("MCR");//Row 2
              tbl6.AddCell(In_txtMCR.Text);
              tbl6.AddCell("kW");
              tbl6.AddCell(classic.Addcom_MCR);
              tbl6.AddCell("a");//Row 3
              tbl6.AddCell(Out_txta.Text);
              tbl6.AddCell(" ");
              tbl6.AddCell(classic.Addcom_a);
              tbl6.AddCell("b");//Row 4
              tbl6.AddCell(Out_txtb.Text);
              tbl6.AddCell(" ");
              tbl6.AddCell(classic.Addcom_b);
              doc.Add(tbl6); // Table 6 insert
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Minimum Power Line Value" + "\n" + "= a x (DWT) + b" + "\n" + "= " + Out_txta.Text + " x " + In_txtDWT.Text + " + " + Out_txtb.Text + "\n" + "= " + Out_txtMPP.Text + " kw"));
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("The relationship between MCR and MPP can be shown in the graph below")); //spacing
              doc.Add(new Paragraph(" ")); //spacing
              //ChartMPP
              using (MemoryStream memoryStream = new MemoryStream())
              {
              chartMPP.SaveImage(memoryStream, ChartImageFormat.Png);
              iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer());
              img.ScalePercent(100f);
              img.Alignment = Element.ALIGN_CENTER;
              doc.Add(img);
              }
              doc.Add(new Paragraph(" ")); //spacing
              if (Out_txtResult.Text == "UNSATISFACTORY")
              {
              doc.Add(new Paragraph("The requirement according to assessment level 1 is not fulfilled."));
              }
              else if (Out_txtResult.Text == "SATISFACTORY")
              {
              doc.Add(new Paragraph("The requirement according to assessment level 1 is fulfilled."));
              }
              else
              {
              doc.Add(new Paragraph(" !ERROR! "));
              }
              doc.Add(new Paragraph(" ")); //spacing

              doc.NewPage();

              doc.Add(new Paragraph("Assessment Level 2 – Simplified Assessment"));
              doc.Add(new Paragraph(" ")); //spacing

              PdfPTable tbl7 = new PdfPTable(4);
              tbl7.DefaultCell.Padding = 3;
              tbl7.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl7.TotalWidth = tableWidth;
              tbl7.LockedWidth = true;
              tbl7.SetWidths(widthsUnits);

              tbl7.AddCell("Parameters"); //Heading row
              tbl7.AddCell("Value");
              tbl7.AddCell("Unit");
              tbl7.AddCell("Comment");
              tbl7.AddCell("Breadth on water line (BWL)");//Row 1
              tbl7.AddCell(In_txtBWL.Text);
              tbl7.AddCell("m");
              tbl7.AddCell(classic.Addcom_BWL);
              tbl7.AddCell("Scantling draft at midship (Tm)");//Row 2
              tbl7.AddCell(In_txtTm.Text);
              tbl7.AddCell("m");
              tbl7.AddCell(classic.Addcom_Tm);
              tbl7.AddCell("Length Between Perpendiculars (LPP)");//Row 3
              tbl7.AddCell(In_txtLpp.Text);
              tbl7.AddCell("m");
              tbl7.AddCell(classic.Addcom_LPP);
              tbl7.AddCell("Rudder Total Area (AR)");//Row 4
              tbl7.AddCell(In_txtAR.Text);
              tbl7.AddCell("m^2");
              tbl7.AddCell(classic.Addcom_AR);
              doc.Add(tbl7); // Table 7 insert
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Using the above parameters, ALScor and % AR / ALS,cor can be calculated."));
              doc.Add(new Paragraph(" ")); //spacing

              PdfPTable tbl8 = new PdfPTable(4);
              tbl8.DefaultCell.Padding = 3;
              tbl8.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl8.TotalWidth = tableWidth;
              tbl8.LockedWidth = true;
              tbl8.SetWidths(widthsUnits);

              tbl8.AddCell("Submerged lateral area of ship corrected for breadth effect, (ALScor)");//Row 5
              tbl8.AddCell(Out_txtALScor.Text);
              tbl8.AddCell("m^2");
              tbl8.AddCell(classic.Addcom_ALScor);
              tbl8.AddCell("Percentage of Rudder area to submerged lateral area of the ship (%AR/ALS,cor)");//Row 6
              tbl8.AddCell(Out_txtPerALS.Text);
              tbl8.AddCell("%");
              tbl8.AddCell(classic.Addcom_PerALScor);
              doc.Add(tbl8); // Table 8 insert
              doc.Add(new Paragraph("Now the required speed of advance (Vs) can be computed."));
              doc.Add(new Paragraph(" ")); //spacing

              PdfPTable tbl9 = new PdfPTable(4);
              tbl9.DefaultCell.Padding = 3;
              tbl9.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl9.TotalWidth = tableWidth;
              tbl9.LockedWidth = true;
              tbl9.SetWidths(widthsUnits);

              tbl9.AddCell("Parameters"); //Heading row
              tbl9.AddCell("Value");
              tbl9.AddCell("Unit");
              tbl9.AddCell("Comment");
              tbl9.AddCell("Frontal Windage Area (FWA)");//Row 1
              tbl9.AddCell(In_txtFWA.Text);
              tbl9.AddCell("m^2");
              tbl9.AddCell(classic.Addcom_FWA);
              tbl9.AddCell("Lateral Windage Area (LWA)");//Row 2
              tbl9.AddCell(In_txtLWA.Text);
              tbl9.AddCell("m^2");
              tbl9.AddCell(classic.Addcom_LWA);
              tbl9.AddCell("Ratio");//Row 3
              tbl9.AddCell(Out_txtRatioFL.Text);
              tbl9.AddCell(" ");
              tbl9.AddCell(classic.Addcom_Ratio);
              tbl9.AddCell("Minimum Navigation" + "\n" + "Speed (Vnav)");//Row 4
              tbl9.AddCell(In_txtVnav.Text);
              tbl9.AddCell("m/s");
              tbl9.AddCell(classic.Addcom_Vnav);
              tbl9.AddCell("Reference Course Keeping Speed (Vck,ref)");//Row 4
              tbl9.AddCell(Out_txtVckref.Text);
              tbl9.AddCell("m/s");
              tbl9.AddCell(classic.Addcom_Vckref);
              tbl9.AddCell("Minimum Course Keeping Speed (Vck)");//Row 4
              tbl9.AddCell(Out_txtVck.Text);
              tbl9.AddCell("m/s");
              tbl9.AddCell(classic.Addcom_Vck);
              tbl9.AddCell("Required Advance Speed,(Vs)");//Row 4
              tbl9.AddCell(Out_txtVs.Text);
              tbl9.AddCell("m/s");
              tbl9.AddCell(classic.Addcom_Vs);
              doc.Add(tbl9); // Table 9 insert
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("The required speed of advance (Vs) is " + Out_txtVs.Text + " m/s"));
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("In the procedure of assessment of installed power, the total resistance can be found."));
              doc.Add(new Paragraph(" ")); //spacing

              PdfPTable tbl10 = new PdfPTable(4);
              tbl10.DefaultCell.Padding = 3;
              tbl10.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl10.TotalWidth = tableWidth;
              tbl10.LockedWidth = true;
              tbl10.SetWidths(widthsUnits);

              tbl10.AddCell("Parameters"); //Heading row
              tbl10.AddCell("Value");
              tbl10.AddCell("Unit");
              tbl10.AddCell("Comment");
              tbl10.AddCell("No. of Engines");//Row 1
              tbl10.AddCell(In_txtNumEng.Text);
              tbl10.AddCell(" ");
              tbl10.AddCell(classic.Addcom_NumEngine);
              tbl10.AddCell("Block Coefficient (BlockC)");//Row 2
              tbl10.AddCell(In_txtBlockC.Text);
              tbl10.AddCell(" ");
              tbl10.AddCell(classic.Addcom_BlockC);
              tbl10.AddCell("Temperature (Temp)");//Row 3
              tbl10.AddCell(In_txtTemp.Text);
              tbl10.AddCell("°C");
              tbl10.AddCell(classic.Addcom_Temp);
              tbl10.AddCell("Density of Air (ρ air)");//Row 4
              tbl10.AddCell(In_txtRowAir.Text);
              tbl10.AddCell("kg/m^3");
              tbl10.AddCell(classic.Addcom_RowAir);
              tbl10.AddCell("Visocity of Water (ν H2O)");//Row 5
              tbl10.AddCell(In_txtViscH2o.Text + " x 10^(-6)");
              tbl10.AddCell("m^2/s");
              tbl10.AddCell(classic.Addcom_ViscH2o);
              tbl10.AddCell("Density of Water (ρ H2O)");//Row 6
              tbl10.AddCell(In_txtRowH2o.Text);
              tbl10.AddCell("Kg/m^3");
              tbl10.AddCell(classic.Addcom_RowH2o);
              tbl10.AddCell("Wetted area of bare hull (S)");//Row 7
              tbl10.AddCell(In_txtS.Text);
              tbl10.AddCell("m^2");
              tbl10.AddCell(classic.Addcom_S);
              tbl10.AddCell("Form factor (k)");//Row 8
              tbl10.AddCell(In_txtK.Text);
              tbl10.AddCell(" ");
              tbl10.AddCell(classic.Addcom_k);

              tbl10.AddCell("Reynold Number (Rn)");//Row 9
              tbl10.AddCell(Out_txtRey.Text + " x 10^5");
              tbl10.AddCell(" ");
              tbl10.AddCell(classic.Addcom_Rey);
              tbl10.AddCell("Mean wind speed (Vw)");//Row 10
              tbl10.AddCell(Out_txtVw.Text);
              tbl10.AddCell("m/s");
              tbl10.AddCell(classic.Addcom_Vw);
              tbl10.AddCell("Frictional Resistance Coefficient (Cf)");//Row 11
              tbl10.AddCell(Out_txtCf.Text);
              tbl10.AddCell(" ");
              tbl10.AddCell(classic.Addcom_Cf);
              tbl10.AddCell("Resistance in calm water (Rcw)");//Row 12
              tbl10.AddCell(Out_txtRcw.Text);
              tbl10.AddCell("kN");
              tbl10.AddCell(classic.Addcom_Rcw);
              tbl10.AddCell("Aerodynamic resistance (Rair)");//Row 13
              tbl10.AddCell(Out_txtRair.Text);
              tbl10.AddCell("kN");
              tbl10.AddCell(classic.Addcom_Rair);
              tbl10.AddCell("Aerodynamic resistance coefficient (Cair)");//Row 14
              tbl10.AddCell(Out_txtCair.Text);
              tbl10.AddCell(" ");
              tbl10.AddCell(classic.Addcom_Cair);
              tbl10.AddCell("Resistance due to appendages (Rapp)");//Row 15
              tbl10.AddCell(In_txtRapp.Text);
              tbl10.AddCell("kN");
              tbl10.AddCell(classic.Addcom_Rapp);
              doc.Add(tbl10); // Table 10 insert
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Below is the stacked bar chart to illustrate the subtotal for Rcw + Rair + Rapp."));
              //chartRsubtotal
              using (MemoryStream memoryStream = new MemoryStream())
              {
              chartRsubtotal.SaveImage(memoryStream, ChartImageFormat.Png);
              iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer());
              img.ScalePercent(100f);
              img.Alignment = Element.ALIGN_CENTER;
              doc.Add(img);
              }

              Paragraph Rsubtotal = new Paragraph(string.Format(
      "Subtotal for Rcw + Rair + Rapp" + "\n" + "= " + Out_txtRcw.Text + " + " + Out_txtRair.Text + " + " + In_txtRapp.Text + "\n" + "= " + Out_txtRsubtotal.Text + " kN"));
              Rsubtotal.Alignment = Element.ALIGN_LEFT;
              doc.Add(Rsubtotal);
              doc.Add(new Paragraph(" ")); //spacing

              doc.NewPage(); //New page
              /*
              doc.Add(new Paragraph("Instead of using ISO 15016:2015 STAWAVE 2 method, the method found in Approximation of the added resistance of ships with small draft or in ballast condition by empirical formula, by Shukui Liu and Apostolos Papanikolaou, will be used."));
              doc.Add(new Paragraph(" ")); //spacing
              */
              PdfPTable tbl11 = new PdfPTable(3);
              tbl11.DefaultCell.Padding = 3;
              tbl11.WidthPercentage = 100;
              tbl11.HorizontalAlignment = Element.ALIGN_CENTER;

              tbl11.AddCell("Parameters"); //Heading row
              tbl11.AddCell("Value");
              tbl11.AddCell("Unit");
              tbl11.AddCell("Gravity (g)");//Row 1
              tbl11.AddCell(In_txtg.Text);
              tbl11.AddCell("m/s^2");
              /*
              tbl11.AddCell("Maximum Beam");//Row 2
              tbl11.AddCell(In_txtBmax.Text);
              tbl11.AddCell("m");
              tbl11.AddCell("After Perpendicular (Ta)");//Row 3
              tbl11.AddCell(In_txtTA.Text);
              tbl11.AddCell("m");
              tbl11.AddCell("Fore perpendicular (Tf)");//Row 4
              tbl11.AddCell(In_txtTF.Text);
              tbl11.AddCell("m");
              tbl11.AddCell("Wave Amplitude (W.A.)");//Row 5
              tbl11.AddCell(In_txtWA.Text);
              tbl11.AddCell(" ");
              tbl11.AddCell("Inertia at Y axis (Iyy)");//Row 6
              tbl11.AddCell(In_txtIyy.Text);
              tbl11.AddCell("m^4");
              tbl11.AddCell("Radius of Gyration (Kyy)");//Row 7
              tbl11.AddCell(In_txtkyy.Text);
              tbl11.AddCell(" ");
              tbl11.AddCell("Angle of waterline entrance (∠W.E.)");//Row 8
              tbl11.AddCell(In_txtE_para.Text);
              tbl11.AddCell("degree");
              */
              tbl11.AddCell("Froude Number (Fr)");//Row 9
              tbl11.AddCell(Out_txtFroude.Text);
              tbl11.AddCell(" ");
              /*
              tbl11.AddCell("Significant wave height (Hs)");//Row 10
              tbl11.AddCell(Out_txtHs.Text);
              tbl11.AddCell("m");
              */
              doc.Add(tbl11); // Table 11 insert





              PdfPTable pdf_dataGridView_Raw = new PdfPTable(dataGridView_Raw.Columns.Count);
              pdf_dataGridView_Raw.DefaultCell.Padding = 3;
              pdf_dataGridView_Raw.WidthPercentage = 100;
              pdf_dataGridView_Raw.HorizontalAlignment = Element.ALIGN_CENTER;

              foreach (DataGridViewColumn column in dataGridView_Raw.Columns)
              {
              PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
              pdf_dataGridView_Raw.AddCell(cell);
              }

              foreach (DataGridViewRow row in dataGridView_Raw.Rows)
              {
              foreach (DataGridViewCell cell in row.Cells)
              {
              if (cell.Value == null)
              {
              // your cell value is null, do something in null value case
              pdf_dataGridView_Raw.AddCell("");
              }
              else
              {
              pdf_dataGridView_Raw.AddCell(cell.Value.ToString());
              }

              }
              }


              /*
              //~~~~~~creating table for all four"dataGridViewTPnew", "dataGridViewRAWnew", "dataGridViewR_Totalnew", "dataGridViewThrust" ~~~~~~
              //ONE~~~dataGridView_Raw

              PdfPTable pdf_dataGridView_Raw = new PdfPTable(dataGridView_Raw.Columns.Count);
              pdf_dataGridView_Raw.DefaultCell.Padding = 3;
              pdf_dataGridView_Raw.WidthPercentage = 100;
              pdf_dataGridView_Raw.HorizontalAlignment = Element.ALIGN_LEFT;

              foreach (DataGridViewColumn column in dataGridView_Raw.Columns)
              {
              PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
              pdf_dataGridView_Raw.AddCell(cell);
              }

              foreach (DataGridViewRow row in dataGridView_Raw.Rows)
              {
              foreach (DataGridViewCell cell in row.Cells)
              {
              pdf_dataGridView_Raw.AddCell(cell.Value.ToString());
              }
              }
              
              //TWO~~~dataGridViewRAWnew
              PdfPTable pdf_dataGridViewRAWnew = new PdfPTable(dataGridViewRAWnew.Columns.Count);
              pdf_dataGridViewRAWnew.DefaultCell.Padding = 3;
              pdf_dataGridViewRAWnew.WidthPercentage = 100;
              pdf_dataGridViewRAWnew.HorizontalAlignment = Element.ALIGN_CENTER;

              foreach (DataGridViewColumn column in dataGridViewRAWnew.Columns)
              {
              PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
              pdf_dataGridViewRAWnew.AddCell(cell);
              }

              foreach (DataGridViewRow row in dataGridViewRAWnew.Rows)
              {
              foreach (DataGridViewCell cell in row.Cells)
              {
              pdf_dataGridViewRAWnew.AddCell(cell.Value.ToString());
              }
              }

              //THREE~~~dataGridViewR_Totalnew
              PdfPTable pdf_dataGridViewR_Totalnew = new PdfPTable(dataGridViewR_Totalnew.Columns.Count);
              pdf_dataGridViewR_Totalnew.DefaultCell.Padding = 3;
              pdf_dataGridViewR_Totalnew.WidthPercentage = 100;
              pdf_dataGridViewR_Totalnew.HorizontalAlignment = Element.ALIGN_LEFT;

              foreach (DataGridViewColumn column in dataGridViewR_Totalnew.Columns)
              {
              PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
              pdf_dataGridViewR_Totalnew.AddCell(cell);
              }

              foreach (DataGridViewRow row in dataGridViewR_Totalnew.Rows)
              {
              foreach (DataGridViewCell cell in row.Cells)
              {
              pdf_dataGridViewR_Totalnew.AddCell(cell.Value.ToString());
              }
              }

              //FOUR~~~dataGridViewThrust
              PdfPTable pdf_dataGridViewThrust = new PdfPTable(dataGridViewThrust.Columns.Count);
              pdf_dataGridViewThrust.DefaultCell.Padding = 3;
              pdf_dataGridViewThrust.WidthPercentage = 100;
              pdf_dataGridViewThrust.HorizontalAlignment = Element.ALIGN_CENTER;

              foreach (DataGridViewColumn column in dataGridViewThrust.Columns)
              {
              PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
              pdf_dataGridViewThrust.AddCell(cell);
              }
              foreach (DataGridViewRow row in dataGridViewThrust.Rows)
              {
              foreach (DataGridViewCell cell in row.Cells)
              {
              pdf_dataGridViewThrust.AddCell(cell.Value.ToString());
              }
              }
              */
              //~~~~~~creating table ~~~~~~

              //add doc to pdf
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Below is the table for Peak wave period (Tps), Added Resistance (Raw), Total Resistance (Rtotal) and Thrust."));
              doc.Add(pdf_dataGridView_Raw);
              doc.Add(new Paragraph("The value of thrust deduction factor tdf obtained from empirical formula is " + Out_txtTDF.Text + ". By applying the thrust deduction factor, the propeller thrust T can be determined from the total resistance at each peak period."));
              doc.NewPage();
              doc.Add(new Paragraph("The stacked graph show below depicts the Total Resistance at each peak wave period."));
              /*
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Added Resistance"));
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(pdf_dataGridViewRAWnew);
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Total Resistance"));
              doc.Add(new Paragraph("The total resistance RTotal is the sum of Rcw, Rair, Rapp and Raw."));
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(pdf_dataGridViewR_Totalnew);
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(new Paragraph("Thrust"));
              doc.Add(new Paragraph(" ")); //spacing
              doc.Add(pdf_dataGridViewThrust);
              doc.Add(new Paragraph(" ")); //spacing
              */
              //chartRTOTAL
              using (MemoryStream memoryStream = new MemoryStream())
              {
              chartRAW.SaveImage(memoryStream, ChartImageFormat.Png);
              iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer());
              img.ScalePercent(100f);
              img.Alignment = Element.ALIGN_CENTER;
              doc.Add(img);
              }
              doc.NewPage();

              PdfPTable tbl12 = new PdfPTable(3);
              tbl12.DefaultCell.Padding = 3;
              tbl12.WidthPercentage = 100;
              tbl12.HorizontalAlignment = Element.ALIGN_CENTER;
              tbl12.AddCell("Parameters"); //Heading row
              tbl12.AddCell("Value");
              tbl12.AddCell("Unit");
              tbl12.AddCell("Propeller Diameter (Dp)");//Row 1
              tbl12.AddCell(In_txtDp.Text);
              tbl12.AddCell("m");
              tbl12.AddCell("Engine Layout");//Row 2
              tbl12.AddCell(In_listboxLayout.Text);
              tbl12.AddCell(" ");
              tbl12.AddCell("Relative rotative efficiency (RReff)");//Row 3
              tbl12.AddCell(In_txtRReff.Text);
              tbl12.AddCell(" ");
              tbl12.AddCell("RPM");//Row 4
              tbl12.AddCell(In_txtRPM.Text);
              tbl12.AddCell("rev per minute");
              tbl12.AddCell("Recommended values for wake fraction (w)");//Row 5
              tbl12.AddCell(Out_txtWake.Text);
              tbl12.AddCell(" ");
              tbl12.AddCell("Thurst deduction factor (tdf)");//Row 6
              tbl12.AddCell(Out_txtTDF.Text);
              tbl12.AddCell(" ");
              tbl12.AddCell("Speed (Ua)");//Row 7
              tbl12.AddCell(Out_txtUa.Text);
              tbl12.AddCell("m/s");
              tbl12.AddCell("Transmission Efficiency (TransEff)");//Row 8
              tbl12.AddCell(Out_txtUa.Text);
              doc.Add(tbl12);
              doc.Add(new Paragraph("From the known required speed of advance and wake, the speed Ua is " + Out_txtUa.Text + " m/s."));
              doc.Add(new Paragraph(" ")); //spacing

              
              /*
              doc.Add(new Paragraph("Instead of manual inputs of KT and KQ, the polynomial function of KT and KQ will be used."));
              doc.Add(new Paragraph("The polynomial functions to plot KT and KQ are:"));
              Paragraph KT = new Paragraph(string.Format("KT: (" + In_txtKTa.Text + ")J^2 + (" + In_txtKTb.Text + ")J + (" + In_txtKTc.Text + ")"));
              KT.Alignment = Element.ALIGN_CENTER;
              doc.Add(KT);
              Paragraph KQ = new Paragraph(string.Format("KJ: (" + In_txtKQa.Text + ")J^2 + (" + In_txtKQb.Text + ")J + (" + In_txtKQc.Text + ")"));
              KQ.Alignment = Element.ALIGN_CENTER;
              doc.Add(KQ);
              */
              doc.Add(new Paragraph(" ")); //spacing

              doc.Add(new Paragraph("The relationship between Advance Coefficient(J), Thrust Coefficient(KT), Torque Coefficient(KQ), Propeller Efficiency(eta0), Optimum propeller speed(nopt) can be illustrated using the table and graph shown below."));
              doc.Add(new Paragraph(" ")); //spacing

              doc.Add(new Paragraph(" "));

              //~~~~dataGridView_J
              PdfPTable pdf_dataGridView_J = new PdfPTable(dataGridView_J.Columns.Count);
              pdf_dataGridView_J.DefaultCell.Padding = 3;
              pdf_dataGridView_J.WidthPercentage = 100;
              pdf_dataGridView_J.HorizontalAlignment = Element.ALIGN_CENTER;
              foreach (DataGridViewColumn column in dataGridView_J.Columns)
              {
              PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
              pdf_dataGridView_J.AddCell(cell);
              }
              foreach (DataGridViewRow row in dataGridView_J.Rows)
              {
              foreach (DataGridViewCell cell in row.Cells)
              {
              if (cell.Value == null)
              {
              // your cell value is null, do something in null value case
              pdf_dataGridView_J.AddCell("");
              }
              else
              {
              pdf_dataGridView_J.AddCell(cell.Value.ToString());
              }
              }
              }
              doc.Add(pdf_dataGridView_J);

              //chartJKTKQ
              using (MemoryStream memoryStream = new MemoryStream())
              {
              chartJKTKQ.SaveImage(memoryStream, ChartImageFormat.Png);
              iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer());
              img.ScalePercent(100f);
              img.Alignment = Element.ALIGN_RIGHT;
              doc.Add(img);
              }

              doc.Add(new Paragraph("The interception coordinates for KTship with KT curve are x: " + Math.Round(classic.J_at_KTintercept, 3).ToString() + " and y: " + Math.Round((classic.Eqn_KT(classic.J_at_KTintercept)), 3).ToString() + "."));
              doc.Add(new Paragraph("Thus, the coordinates for Optimal shaft speed are x: " + Math.Round(classic.J_at_KTintercept, 3).ToString() + " and y: " + Math.Round(classic.Eqn_eta0(classic.J_at_KTintercept), 3).ToString() + "."));












              Paragraph END = new Paragraph(string.Format(      "~~~    ~~~    END    ~~~    ~~~"));
              END.Alignment = Element.ALIGN_CENTER;
              doc.Add(END);
              doc.Add(new Paragraph(" ")); //spacing


              }
              catch (Exception ex)
              {
              MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
              finally
              {
              doc.Close();
              }


              /*
              // Must have write permissions to the path folder
              iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4.Rotate());
              PdfWriter writer = new PdfWriter("C:\\demo.pdf");
              PdfDocument pdf = new PdfDocument(writer);
              Document document = new Document(pdf);
              Paragraph header = new Paragraph("HEADER")
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFontSize(20);

              document.Add(header);
              document.Close();
              */
              }
              }
              }
              //RUN Button
              private void In_buttonRUN_Click(object sender, EventArgs e)
              {
              RunALL();

              }
              private void In_Run0_Click(object sender, EventArgs e)
              {
              Run0();
              MessageBox.Show("    Info saved    ", " ", MessageBoxButtons.OK, MessageBoxIcon.None);
              }
              private void In_Run1_Click(object sender, EventArgs e)
              {
              Run1();
              }
              private void In_Run2_Click(object sender, EventArgs e)
              {
              Run2();
              }
              private void In_Run3_Click(object sender, EventArgs e)
              {
              Run3();
              }
              private void In_Run4_Click(object sender, EventArgs e)
              {
              Run4();
              }
              private void In_Run5_Click(object sender, EventArgs e)
              {
              Run5();
              }
              private void In_Run6_Click(object sender, EventArgs e)
              {
              Run6();
              }
              //Clear all inputs and reset constants
              private void ClearALL()
              {
              Clear0();
              Clear1();
              Clear2();
              Clear3();
              Clear4();
              Clear5();
              Clear6();
              Constants();
              }
              private void Clear0()
              {
              //0input
              In_txtDNVGL.Text = "";
              In_txtIMONum.Text = "";
              In_txtVesselName.Text = "";
              In_txtBuildingYard.Text = "";
              In_txtHullNum.Text = "";
              In_txtLSW.Text = "";
              In_txtEngineManufacturer.Text = "";
              In_txtEngineType.Text = "";
              }
              private void Clear1()
              {
              //1input
              In_txtDWT.Text = "";
              In_listboxShipType.Text = "";
              In_txtShipType.Text = "";
              In_txtMCR.Text = "";
              In_txtShipType.Text = "";
              //1outputs text
              Out_txtResult.Text = "";
              Out_txtResult.BackColor = Color.White;
              Out_txtMPP.Text = "";
              Out_txta.Text = "";
              Out_txtb.Text = "";
              chartMPP.Series.Clear();
              }
              private void Clear2()
              {
              //2input
              In_txtBWL.Text = "";
              In_txtTm.Text = "";
              In_txtLpp.Text = "";
              In_txtAR.Text = "";
              //2output text
              Out_txtALScor.Text = "";
              Out_txtPerALS.Text = "";
              }
              private void Clear3()
              {
              //3input
              In_txtFWA.Text = "";
              In_txtLWA.Text = "";
              In_txtVnav.Text = "";
              In_txtNumEng.Text = "";
              In_txtBlockC.Text = "";
              //3output text
              Out_txtRatioFL.Text = "";
              Out_txtVckref.Text = "";
              Out_txtVck.Text = "";
              Out_txtVs.Text = "";
              }
              private void Clear4()
              {
              //4input
              In_txtRowAir.Text = "1.225";
              In_txtTemp.Text = "15";
              In_txtViscH2o.Text = "1.184";
              In_txtRowH2o.Text = "1025.8";
              In_txtS.Text = "";
              In_txtRapp.Text = "";
              In_txtK.Text = "";
              
              //4output text
              Out_txtRey.Text = "";
              Out_txtVw.Text = "";
              Out_txtCf.Text = "";
              Out_txtRcw.Text = "";
              Out_txtRair.Text = "";
              Out_txtCair.Text = "";
              chartRsubtotal.Series.Clear();
              }
              private void Clear5()
              {
              //5input
              In_txtg.Text = "9.807";
              //5output text
              Out_txtFroude.Text = "";

              this.dataGridView_Raw.DataSource = null;
              this.dataGridView_Raw.Rows.Clear();
              chartRAW.Series.Clear();
              chartRAW.Series.Add("Rapp");
              chartRAW.Series.Add("Rair");
              chartRAW.Series.Add("Rcw");
              chartRAW.Series.Add("Raw");
              chartRAW.Series["Raw"].Color = Color.LimeGreen;
              classic.count = 0;
              classic.Raw = 0;
              classic.RawHeight = 0;
              Run5();
              }
              private void Clear6()
              {
              //6input
              In_txtPropType.Text = "";
              In_txtNumBlades.Text = "";
              In_txtDp.Text = "";
              In_listboxLayout.Text = "";
              In_txtRReff.Text = "";
              In_txtRPM.Text = "";
              In_listboxLayout.Text = "";
              In_txtJ_Limit.Text = "";
              In_J_Interval.Text = "";
              In_txtKTa.Text = "";
              In_txtKTb.Text = "";
              In_txtKTc.Text = "";
              In_txtKQa.Text = "";
              In_txtKQb.Text = "";
              In_txtKQc.Text = "";

              //6output
              Out_txtWake.Text = "";
              Out_txtTDF.Text = "";
              Out_txtUa.Text = "";
              Out_txtTransEff.Text = "";
              chartJKTKQ.Series.Clear();
              this.dataGridView_J.DataSource = null;
              this.dataGridView_J.Rows.Clear();

              Out_txtn_opt.Text = "";
              Out_txtKTintercept.Text = "";
              //to be updated
              }

              private void In_ClearALL_Click(object sender, EventArgs e)
              {
              DialogResult dialogResult = MessageBox.Show("    CONFIRM to CLEAR ALL?    ", "Reset ALL", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
              ClearALL();
              }
              else if (dialogResult == DialogResult.No)
              {

              }

              }


              private void In_Clear0_Click(object sender, EventArgs e)
              {
              DialogResult dialogResult = MessageBox.Show("    Proceed to clear?    ", "Reset", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
              Clear0();
              }
              else if (dialogResult == DialogResult.No)
              {

              }
              }
              private void In_Clear1_Click(object sender, EventArgs e)
              {
              DialogResult dialogResult = MessageBox.Show("    Proceed to clear?    ", "Reset", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
              Clear1();
              }
              else if (dialogResult == DialogResult.No)
              {

              }
              }
              private void In_Clear2_Click(object sender, EventArgs e)
              {
              DialogResult dialogResult = MessageBox.Show("    Proceed to clear?    ", "Reset", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
              Clear2();
              }
              else if (dialogResult == DialogResult.No)
              {

              }
              }
              private void In_Clear3_Click(object sender, EventArgs e)
              {
              DialogResult dialogResult = MessageBox.Show("    Proceed to clear?    ", "Reset", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
              Clear3();
              }
              else if (dialogResult == DialogResult.No)
              {

              }
              }
              private void In_Clear4_Click(object sender, EventArgs e)
              {
              DialogResult dialogResult = MessageBox.Show("    Proceed to clear?    ", "Reset", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
              Clear4();
              }
              else if (dialogResult == DialogResult.No)
              {

              }
              }
              private void In_Clear5_Click(object sender, EventArgs e)
              {
              DialogResult dialogResult = MessageBox.Show("    Proceed to clear?    ", "Reset", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
              Clear5();
              }
              else if (dialogResult == DialogResult.No)
              {

              }
              }
              private void In_Clear6_Click(object sender, EventArgs e)
              {
              DialogResult dialogResult = MessageBox.Show("    Proceed to clear?    ", "Reset", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
              Clear6();
              }
              else if (dialogResult == DialogResult.No)
              {

              }
              }
              private void Out_txtKTintercept_Click(object sender, EventArgs e)
              {
              MessageBox.Show(classic.outcome);
              }
              private void In_buttonTESTA_Click(object sender, EventArgs e)
              {
              SAMPLE_A();
              }
              private void In_buttonTESTB_Click(object sender, EventArgs e)
              {
              SAMPLE_B();
              }

              private void kyyiyyInfo()
              {
              MessageBox.Show("Check for 'kyy' and 'Iyy'" + "\n" + "Hover mouse over text 'kyy' or 'Iyy' to see instructions", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
              }
              private void labelIyy_Click(object sender, EventArgs e)
              {
              In_txtkyy.Text = "nil";
              In_txtkyy.BackColor = SystemColors.ButtonHighlight;
              In_txtIyy.BackColor = SystemColors.Info;
              }

              private void labelKyy_Click(object sender, EventArgs e)
              {
              In_txtIyy.Text = "nil";
              In_txtIyy.BackColor = SystemColors.ButtonHighlight;
              In_txtkyy.BackColor = SystemColors.Info;
              }
              private void labelE_para_Click(object sender, EventArgs e)
              {
              runE_para();
              }
              private void runE_para() //angle of waterline entrance
              {
              if (In_txtBmax.Text == string.Empty || In_txtLpp.Text == string.Empty)
              {
              MessageBox.Show("Check for 'Bmax' and 'Lpp'" + "\n" + "Hover mouse over text '∠W.E.'to see instructions", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
              }
              else
              {
              In_txtE_para.Text = Math.Round(((180 / Math.PI) * classic.Eqn_E_para()), 2).ToString();
              }
              }
              //~~~~~~~~~~~~~~~~~~ COMMENT + sign in form ~~~~~~~~~~~~~~~~~~
              private void AddCom_DNVGL_Click(object sender, EventArgs e)
              {
              classic.Addcom_DNVGL = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_IMONum_Click(object sender, EventArgs e)
              {
              classic.Addcom_IMONum = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_VesselName_Click(object sender, EventArgs e)
              {
              classic.Addcom_VesselName = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_BuildingYard_Click(object sender, EventArgs e)
              {
              classic.Addcom_BuildingYard = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_HullNum_Click(object sender, EventArgs e)
              {
              classic.Addcom_HullNum = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_LSW_Click(object sender, EventArgs e)
              {
              classic.Addcom_LightShipWeight = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_EngineManufacturer_Click(object sender, EventArgs e)
              {
              classic.Addcom_EngineManufacturer = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_EngineType_Click(object sender, EventArgs e)
              {
              classic.Addcom_EngineType = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_DWT_Click(object sender, EventArgs e)
              {
              classic.Addcom_DWT = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_MCR_Click(object sender, EventArgs e)
              {
              classic.Addcom_MCR = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_ShipType_Click(object sender, EventArgs e)
              {
              classic.Addcom_ShipType = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_a_Click(object sender, EventArgs e)
              {
              classic.Addcom_a = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_b_Click(object sender, EventArgs e)
              {
              classic.Addcom_b = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_MPP_Click(object sender, EventArgs e)
              {
              classic.Addcom_MPP = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Result_Click(object sender, EventArgs e)
              {
              classic.Addcom_Result = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_BWL_Click(object sender, EventArgs e)
              {
              classic.Addcom_BWL = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_TM_Click(object sender, EventArgs e)
              {
              classic.Addcom_Tm = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_LPP_Click(object sender, EventArgs e)
              {
              classic.Addcom_LPP = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_AR_Click(object sender, EventArgs e)
              {
              classic.Addcom_AR = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_ALScor_Click(object sender, EventArgs e)
              {
              classic.Addcom_ALScor = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_PerALScor_Click(object sender, EventArgs e)
              {
              classic.Addcom_PerALScor = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_FWA_Click(object sender, EventArgs e)
              {
              classic.Addcom_FWA = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_LWA_Click(object sender, EventArgs e)
              {
              classic.Addcom_LWA = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Ratio_Click(object sender, EventArgs e)
              {
              classic.Addcom_Ratio = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Vnav_Click(object sender, EventArgs e)
              {
              classic.Addcom_Vnav = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Vckref_Click(object sender, EventArgs e)
              {
              classic.Addcom_Vckref = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Vck_Click(object sender, EventArgs e)
              {
              classic.Addcom_Vck = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Vs_Click(object sender, EventArgs e)
              {
              classic.Addcom_Vs = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_NumEng_Click(object sender, EventArgs e)
              {
              classic.Addcom_NumEngine = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_BlockC_Click(object sender, EventArgs e)
              {
              classic.Addcom_BlockC = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Temp_Click(object sender, EventArgs e)
              {
              classic.Addcom_Temp = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_RowAir_Click(object sender, EventArgs e)
              {
              classic.Addcom_RowAir = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_ViscH2o_Click(object sender, EventArgs e)
              {
              classic.Addcom_ViscH2o = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_RowH2o_Click(object sender, EventArgs e)
              {
              classic.Addcom_RowH2o = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_S_Click(object sender, EventArgs e)
              {
              classic.Addcom_S = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_k_Click(object sender, EventArgs e)
              {
              classic.Addcom_k = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Rey_Click(object sender, EventArgs e)
              {
              classic.Addcom_Rey = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Vw_Click(object sender, EventArgs e)
              {
              classic.Addcom_Vw = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Cf_Click(object sender, EventArgs e)
              {
              classic.Addcom_Cf = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Rcw_Click(object sender, EventArgs e)
              {
              classic.Addcom_Rcw = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Rair_Click(object sender, EventArgs e)
              {
              classic.Addcom_Rair = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Cair_Click(object sender, EventArgs e)
              {
              classic.Addcom_Cair = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Rapp_Click(object sender, EventArgs e)
              {
              classic.Addcom_Rapp = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Gravity_Click(object sender, EventArgs e)
              {
              classic.Addcom_Gravity = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }

              private void AddCom_Froude_Click(object sender, EventArgs e)
              {
              classic.Addcom_Froude = Microsoft.VisualBasic.Interaction.InputBox("     Input comment below    ",
                       "    Comment    ",
                       "Default",
                       0,
                       0);
              }


              private void In_RAW_Click(object sender, EventArgs e)
              {
              Run5_addrow();
              }

              private void In_minusRAW_Click(object sender, EventArgs e)
              {
              Run5_minusrow();
              }

             
       }
}
