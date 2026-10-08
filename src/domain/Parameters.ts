/** Faithful port of reference/Parameters.cs. Legacy branches and unit conventions are retained. */
export class Parameters {
  DNVGL = "";
  IMONum = "";
  VesselName = "";
  BuildingYard = "";
  HullNum = "";
  LSW = "";
  EngineManufacturer = "";
  EngineType = "";
  PropellerType = "";
  NumBlades = "";
  DWT = 0;
  MCR = 0;
  ST = "";
  MPP = 0;
  a = 0;
  b = 0;
  Result = "";
  BWL = 0;
  Tm = 0;
  Lpp = 0;
  AR = 0;
  ALScor = 0;
  PerALS = 0;
  FWA = 0;
  LWA = 0;
  RatioFL = 0;
  Vnav = 0;
  Vckref = 0;
  Vck = 0;
  Vs = 0;
  NumEng = 0;
  BlockC = 0;
  Temp = 0;
  RowAir = 0;
  ViscH2o = 0;
  RowH2o = 0;
  S = 0;
  Rey = 0;
  Vw = 0;
  Hs = 0;
  K = 0;
  Cf = 0;
  Rcw = 0;
  Cair = 0;
  Rair = 0;
  Rapp = 0;
  Raw = 0;
  count = 0;
  RawHeight = 0;
  WaveAmp = 0;
  rRaw = 0;
  Bmax = 0;
  E_para = 0;
  g = 0;
  TA = 0;
  TF = 0;
  L_we = 0;
  omega = 0;
  lambda = 0;
  Fr = 0;
  alphaT = 0;
  Rawrl = 0;
  Iyy = 0;
  kyy = 0;
  a1 = 0;
  a2 = 0;
  a3 = 0;
  ARmaxOmega = 0;
  Non_w = 0;
  b1 = 0;
  d1 = 0;
  Rawml = 0;
  Rwave = 0;
  Rsubtotal = 0;
  Rtotal = 0;
  Wake = 0;
  TDF = 0;
  Dp = 0;
  outcome = "";
  J_at_KTintercept = 0;
  n_opt = 0;
  Ua = 0;
  J_interval = 0;
  J_limit = 0;
  chartYmax = 0;
  chartXmax = 0;
  J = 0;
  KQ_J = 0;
  KT_J = 0;
  KTa = 0;
  KTb = 0;
  KTc = 0;
  KQa = 0;
  KQb = 0;
  KQc = 0;
  eta0 = 0;
  Thrust0 = 0;
  c7 = 0;
  KTship0 = 0;
  RReff = 0;
  Layout = "";
  TransEff = 0;
  nrpm = 0;
  PD = 0;
  PB = 0;
  Addcom_DNVGL = "";
  Addcom_IMONum = "";
  Addcom_VesselName = "";
  Addcom_BuildingYard = "";
  Addcom_HullNum = "";
  Addcom_LightShipWeight = "";
  Addcom_EngineManufacturer = "";
  Addcom_EngineType = "";
  Addcom_DWT = "";
  Addcom_MCR = "";
  Addcom_ShipType = "";
  Addcom_a = "";
  Addcom_b = "";
  Addcom_MPP = "";
  Addcom_Result = "";
  Addcom_BWL = "";
  Addcom_Tm = "";
  Addcom_LPP = "";
  Addcom_AR = "";
  Addcom_ALScor = "";
  Addcom_PerALScor = "";
  Addcom_FWA = "";
  Addcom_LWA = "";
  Addcom_Ratio = "";
  Addcom_Vnav = "";
  Addcom_Vckref = "";
  Addcom_Vck = "";
  Addcom_Vs = "";
  Addcom_NumEngine = "";
  Addcom_BlockC = "";
  Addcom_Temp = "";
  Addcom_RowAir = "";
  Addcom_ViscH2o = "";
  Addcom_RowH2o = "";
  Addcom_S = "";
  Addcom_k = "";
  Addcom_Rey = "";
  Addcom_Vw = "";
  Addcom_Cf = "";
  Addcom_Rcw = "";
  Addcom_Rair = "";
  Addcom_Cair = "";
  Addcom_Rapp = "";
  Addcom_Gravity = "";
  Addcom_Froude = "";
  Eqn_MPP(): number {
    let ST1 = "Bulk Carrier";
    let ST2 = "Tanker/Combination Carrier";
    if (this.ST == ST1) {
      if (this.DWT > 20000 && this.DWT < 145000) {
        this.a = 0.0763;
        this.b = 3374.3;
      } else if (this.DWT > 145000) {
        this.a = 0.049;
        this.b = 7329.0;
      } else {
        this.a = 0;
        this.b = 0;
      }
      this.Cair = 0.95;
    } else if (this.ST == ST2) {
      if (this.DWT > 20000) {
        this.a = 0.0652;
        this.b = 5960.2;
      } else {
        this.a = 0;
        this.b = 0;
      }
      this.Cair = 0.98;
    } else {
      this.Cair = 0.97;
      return (this.MPP = 0);
    }
    return (this.MPP = this.a * this.DWT + this.b);
  }
  Eqn_ALScor(): number {
    let Pow_ALS = Math.pow(this.BWL / this.Lpp, 2);
    let newALScor =
      this.Lpp * this.Tm * (Math.fround(1.0) + Math.fround(25.0) * Pow_ALS);
    return (this.ALScor = newALScor);
  }
  Eqn_PerALS(): number {
    let newPerALS = (this.AR / this.Eqn_ALScor()) * 100;
    return (this.PerALS = newPerALS);
  }
  Eqn_RatioFL(): number {
    let newRatioFL = this.FWA / this.LWA;
    return (this.RatioFL = newRatioFL);
  }
  Eqn_Vckref(): number {
    let newVckref = 0;
    this.Eqn_RatioFL();
    let RFL = this.RatioFL;
    if (RFL < 0.1) {
      newVckref = 9.0;
    } else if (RFL > 0.4) {
      newVckref = 4.0;
    } else {
      newVckref = 9 + (-5 / 0.3) * (RFL - 0.1);
    }
    return (this.Vckref = newVckref);
  }
  Eqn_Vck(): number {
    return (this.Vck = this.Eqn_Vckref() - 10 * (this.Eqn_PerALS() - 0.9));
  }
  Eqn_Vs(): number {
    this.Eqn_Vck();
    if (this.Vck > this.Vnav) {
      return (this.Vs = (this.Vck * 1852) / 3600);
    } else {
      return (this.Vs = (this.Vnav * 1852) / 3600);
    }
  }
  Eqn_Rey(): number {
    return (this.Rey =
      (this.Eqn_Vs() * this.Lpp) / (this.ViscH2o * Math.pow(10, -6)));
  }
  Eqn_HsVw(): [number, number] {
    if (this.Lpp > 250) {
      this.Vw = Math.fround(19);
      this.Hs = Math.fround(5.5);
    } else if (this.Lpp < 200) {
      this.Vw = Math.fround(15.7);
      this.Hs = Math.fround(4);
    } else {
      this.Vw = ((19 - 15.7) / (250 - 200)) * (this.Lpp - 200) + 15.7;
      this.Hs = ((5.5 - 4) / (250 - 200)) * (this.Lpp - 200) + 4;
    }
    return [this.Vw, this.Hs];
  }
  Eqn_K(): number {
    return (this.K =
      -0.095 +
      (25.6 * this.BlockC) /
        (Math.pow(this.Lpp / this.BWL, 2) * Math.sqrt(this.BWL / this.Tm)));
  }
  Eqn_Cf(): number {
    return (this.Cf = 0.075 / Math.pow(Math.log10(this.Eqn_Rey()) - 2, 2));
  }
  Eqn_Rcw(): number {
    return (this.Rcw =
      (1 + this.K) *
      this.Eqn_Cf() *
      0.5 *
      this.RowH2o *
      this.S *
      (Math.pow(this.Eqn_Vs(), 2) / 1000));
  }
  Eqn_Rair(): number {
    this.Eqn_MPP();
    this.Eqn_HsVw();
    return (this.Rair =
      (this.Cair *
        0.5 *
        this.RowAir *
        this.FWA *
        Math.pow(this.Vw + this.Eqn_Vs(), 2)) /
      1000);
  }
  Eqn_L_we(): number {
    return (this.L_we = (2 / 20.0) * this.Lpp);
  }
  Eqn_omega(time: number): number {
    return (this.omega = (2 * Math.PI) / time);
  }
  Eqn_lambda(time: number): number {
    return (this.lambda =
      (2 * Math.PI) / (Math.pow(this.Eqn_omega(time), 2) / this.g));
  }
  Eqn_Fr(): number {
    this.Eqn_Vs();
    return (this.Fr = (this.Vs * 0.5144) / Math.sqrt(this.g * this.Lpp));
  }
  Eqn_alphaT(time: number): number {
    this.Eqn_lambda(time);
    if (this.lambda / this.Lpp > 2.5) {
      return (this.alphaT = 0);
    } else {
      return (this.alphaT =
        1 -
        Math.exp(
          -4 *
            Math.PI *
            (this.TA / this.Eqn_lambda(time) - this.TA / 2.5 / this.Lpp),
        ));
    }
  }
  Eqn_E_para(): number {
    return (this.E_para = Math.atan(this.Bmax / (2.0 * this.Eqn_L_we())));
  }
  Eqn_Rawrl(time: number): number {
    this.Eqn_alphaT(time);
    this.Eqn_E_para();
    this.Eqn_Fr();
    return (this.Rawrl =
      (((2.25 / 8.0) * this.Lpp) / this.Bmax) *
      this.Eqn_alphaT(time) *
      Math.pow(Math.sin(this.E_para), 2) *
      (1 + 5.0 * Math.sqrt(this.Lpp / this.Eqn_lambda(time)) * this.Fr) *
      Math.pow(0.87 / this.BlockC, 1 + 4.0 * Math.sqrt(this.Fr)));
  }
  Eqn_Iyy(): number {
    return (this.Iyy = Math.pow(this.kyy * this.Lpp, 2) * this.DWT);
  }
  Eqn_kyy(): number {
    return (this.kyy = Math.sqrt(this.Iyy / this.DWT) / this.Lpp);
  }
  Eqn_rRaw(): number {
    return (this.rRaw =
      (4 *
        this.RowH2o *
        this.g *
        Math.pow(this.WaveAmp, 2) *
        Math.pow(this.Bmax, 2)) /
      this.Lpp);
  }
  Eqn_a1(): number {
    this.Eqn_Fr();
    return (this.a1 =
      (60.3 *
        Math.pow(this.BlockC, 1.34) *
        Math.pow(4 * this.kyy, 2) *
        Math.pow(0.87 / this.BlockC, 1 + this.Fr)) /
      Math.log(this.Bmax / this.TA));
  }
  Eqn_a2(): number {
    this.Eqn_Fr();
    if (this.Fr < 0.12) {
      return (this.a2 = 0.0072 + 0.1676 * this.Fr);
    } else {
      return (this.a2 = Math.pow(this.Fr, 1.5) * Math.exp(-3.5 * this.Fr));
    }
  }
  Eqn_a3(): number {
    let a3trigo = (180 / Math.PI) * Math.atan((this.TA - this.TF) / this.Lpp);
    return (this.a3 = 1.0 + 0.25 * a3trigo);
  }
  Eqn_ARmaxOmega(time: number): number {
    this.Eqn_Fr();
    return (this.ARmaxOmega =
      2.142 *
      Math.pow(this.kyy, 1 / 3.0) *
      Math.sqrt(this.Lpp / this.Eqn_lambda(time)) *
      (1 -
        ((0.13 * 0.85) / this.BlockC) *
          (Math.log(this.Bmax / this.TA) - Math.log(2.75))));
  }
  Eqn_Non_w(time: number): number {
    if (this.Eqn_Fr() <= 0.1) {
      return (this.Non_w = this.Eqn_ARmaxOmega(time) * (this.Eqn_Fr() + 0.62));
    } else {
      return (this.Non_w =
        this.Eqn_ARmaxOmega(time) * Math.pow(this.Eqn_Fr(), 0.143));
    }
  }
  Eqn_b1d1(time: number): number {
    let Eqn_d1 = 566 * Math.pow(this.Lpp / this.Bmax, -2.66);
    if (this.BlockC < 0.75) {
      if (this.Eqn_Non_w(time) < 1.0) {
        this.b1 = 11.0;
        this.d1 = 14.0;
      } else {
        this.b1 = -8.5;
        this.d1 = Eqn_d1 * 6;
      }
    } else {
      if (this.Eqn_Non_w(time) < 1.0) {
        this.b1 = 11.0;
        this.d1 = Eqn_d1;
      } else {
        this.b1 = -8.5;
        this.d1 = Eqn_d1 * 6;
      }
    }
    return this.b1;
  }
  Eqn_Rawml(time: number): number {
    this.Eqn_b1d1(time);
    return (this.Rawml =
      Math.pow(this.Eqn_Non_w(time), this.b1) *
      Math.exp(
        (this.b1 / this.d1) * (1 - Math.pow(this.Eqn_Non_w(time), this.d1)),
      ) *
      this.Eqn_a1() *
      this.Eqn_a2() *
      this.Eqn_a3());
  }
  Eqn_Rwave(time: number): number {
    return (this.Rwave =
      ((this.Eqn_Rawml(time) + this.Eqn_Rawrl(time)) * this.Eqn_rRaw()) / 1000);
  }
  Eqn_Rsubtotal(): number {
    return (this.Rsubtotal = this.Eqn_Rair() + this.Eqn_Rcw() + this.Rapp);
  }
  Eqn_Rtotal(time: number): number {
    return (this.Rtotal = this.Eqn_Rsubtotal() + this.Eqn_Rwave(time));
  }
  Eqn_Wake(): number {
    if (this.NumEng == 1) {
      if (this.BlockC == 0.5) {
        this.Wake = 0.14;
      } else if (this.BlockC == 0.6) {
        this.Wake = 0.23;
      } else if (this.BlockC == 0.7) {
        this.Wake = 0.29;
      } else if (this.BlockC >= 0.8) {
        this.Wake = 0.35;
      }
    } else if (this.NumEng == 2) {
      if (this.BlockC == 0.5) {
        this.Wake = 0.15;
      } else if (this.BlockC == 0.6) {
        this.Wake = 0.17;
      } else if (this.BlockC == 0.7) {
        this.Wake = 0.19;
      } else if (this.BlockC >= 0.8) {
        this.Wake = 0.23;
      }
    } else {
      return (this.Wake = 0);
    }
    return this.Wake;
  }
  Eqn_TDF(): number {
    return (this.TDF = Math.fround(0.7) * this.Eqn_Wake());
  }
  Eqn_Quad(a: number, b: number, c: number, x: number): number {
    return a * Math.pow(x, 2) + b * x + c;
  }
  Eqn_SolveQuad(a: number, b: number, c: number): number {
    let root = 0,
      deno = 0,
      x1 = 0,
      x2 = 0;
    x1 = x2 = 0;
    if (a == 0) {
      x1 = -c / b;
      this.outcome = "The roots are Linear:" + x1.toString();
    } else {
      root = b * b - 4 * a * c;
      deno = 2 * a;
      if (root > 0) {
        x1 = -b / deno + Math.sqrt(root) / deno;
        x2 = -b / deno - Math.sqrt(root) / deno;
        this.outcome =
          "THE ROOTS ARE REAL AND DISTINCT ROOTS" +
          "\n" +
          "THE ROOTS ARE: " +
          x1.toString() +
          " and " +
          x2.toString();
      } else if (root == 0) {
        x1 = -b / deno;
        this.outcome =
          "THE ROOTS ARE REPEATED ROOTS" +
          "\n" +
          "THE ROOT IS: " +
          x1.toString();
      } else {
        x1 = -b / deno;
        x2 = Math.sqrt(4 * a * c - b * b) / deno;
        this.outcome =
          "THE ROOTS ARE IMAGINARY ROOTS" +
          "\n" +
          "ROOT 1: " +
          x1.toString() +
          "+i" +
          x2.toString() +
          "\n" +
          "ROOT 2: " +
          x1.toString() +
          "-i" +
          x2.toString();
      }
    }
    if (x1 > x2) {
      return x1;
    } else {
      return x2;
    }
  }
  Eqn_n_opt(): number {
    return (this.n_opt = this.Eqn_Ua() / (this.J_at_KTintercept * this.Dp));
  }
  Eqn_Ua(): number {
    return (this.Ua = this.Eqn_Vs() * (1 - this.Eqn_Wake()));
  }
  Eqn_J(): number {
    return (this.J = (this.Eqn_Ua() / this.nrpm) * this.Dp);
  }
  Eqn_KQ(J: number): number {
    return (this.KQ_J = this.Eqn_Quad(this.KQa, this.KQb, this.KQc, J));
  }
  Eqn_KT(J: number): number {
    return (this.KT_J = this.Eqn_Quad(this.KTa, this.KTb, this.KTc, J));
  }
  Eqn_eta0(J: number): number {
    return (this.eta0 =
      (10 * this.Eqn_KT(J) * J) / (this.Eqn_KQ(J) * 2 * Math.PI));
  }
  Eqn_KTship0(R: number, J: number): number {
    this.Thrust0 = (R * 1000) / (1 - this.Eqn_TDF());
    this.c7 =
      this.Thrust0 /
      (this.RowH2o * Math.pow(this.Eqn_Ua(), 2) * Math.pow(this.Dp, 2));
    return this.c7 * Math.pow(J, 2);
  }
  Eqn_TransEff(): number {
    if (this.Layout == "aft engine") {
      return (this.TransEff = 0.98);
    } else if (this.Layout == "midship") {
      return (this.TransEff = 0.97);
    } else {
      return (this.TransEff = 0);
    }
  }
  Eqn_Rps(J: number): number {
    return (this.nrpm = this.Ua / (J * this.Dp) / 60);
  }
  Eqn_PD(J: number): number {
    return (this.PD =
      (2 *
        Math.PI *
        this.RowH2o *
        Math.pow(this.Eqn_Rps(J), 3) *
        Math.pow(this.Dp, 5) *
        this.Eqn_KQ(J)) /
      1000);
  }
  Eqn_PB(J: number): number {
    return (this.PB = this.Eqn_PD(J) / this.TransEff);
  }
}
