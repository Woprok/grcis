using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _012animation
{
  /// <summary>
  /// Solid color disc.
  /// </summary>
  public class Disc
  {
    public double cx;
    public double cy;
    public double radius;
    public Color color;
    public Disc(double x, double y, double r, Color c)
    {
      cx = x;
      cy = y;
      radius = r;
      color = c;
    }
  }

  public class Animation
  {
    #region Unchanged
    /// <summary>
    /// CGG logo colors.
    /// </summary>
    protected static Color[] COLORS =
    {
      Color.FromArgb(0x71, 0x21, 0x6d),
      Color.FromArgb(0xe8, 0x75, 0x05)
    };
    /// <summary>
    /// Final discs' geometries { cx, cy, radius }.
    /// </summary>
    protected static double[,] DISC_DATA = new double[,]
    {
      {  59.2317,  77.2244, 2.1480 },
      {  29.5167,  69.7424, 4.1070 },
      {  50.0857,  90.1954, 4.4050 },
      {  29.5177,  49.3654, 3.0170 },
      {  38.0227,  87.1904, 3.5160 },
      {  53.8837,  98.0334, 3.5160 },
      {  78.9077,  97.6174, 3.5160 },
      {  36.4977,  72.5804, 1.9350 },
      {  60.7857,  93.7054, 2.1020 },
      {  86.1887,  94.2194, 3.5160 },
      {  40.5107,  79.7254, 2.1480 },
      {  37.2237,  35.1484, 2.1480 },
      {  66.0817, 100.9204, 2.1490 },
      {  70.4577, 101.4664, 1.5330 },
      {  56.2357,  92.1694, 1.5330 },
      {  44.7487,  77.3954, 1.5330 },
      {  54.0487,  84.2384, 1.5320 },
      {  76.9757,  90.9514, 1.5330 },
      {  82.2157,  88.7444, 1.5330 },
      {  67.4197,  88.4774, 1.5320 },
      {  48.5097,  78.6024, 1.2590 },
      {  50.6307,  83.4184, 1.2590 },
      {  51.5867,  77.6684, 1.2600 },
      {  53.6277,  74.7974, 1.1050 },
      {  54.0487,  71.5134, 0.8840 },
      {  53.0627,  69.0624, 0.6160 },
      {  92.6397,  85.9514, 1.0090 },
      {  92.1357,  90.4004, 2.6920 },
      {  97.6217,  83.6214, 3.5170 },
      { 102.7087,  71.6204, 3.5160 },
      { 101.3077,  78.2894, 2.1480 },
      {  96.8637,  77.4394, 1.3270 },
      {  99.2907,  65.3304, 2.1480 },
      {  76.2987,  67.0884, 1.4210 },
      {  83.8597,  64.4754, 1.7710 },
      {  92.1817,  64.6674, 2.5340 },
      {  87.6617,  68.6074, 2.1490 },
      {  96.4247,  69.4334, 2.1470 },
      {  83.3117,  68.7374, 1.3950 },
      {  79.0617,  63.9114, 1.5330 },
      {  92.9847,  73.0534, 1.5320 },
      {  92.0437,  69.1544, 1.2590 },
      {  99.6267,  61.2294, 1.2600 },
      { 105.5807,  61.2294, 1.2590 },
      { 103.2607,  59.8614, 0.6160 },
      {  87.6187,  64.7184, 1.1640 },
      {  96.0087,  62.6084, 0.9250 },
      { 102.5717,  62.1864, 0.9240 },
      {  79.0617,  70.0544, 0.9860 },
      {  88.7847,  86.8944, 0.9870 },
      {  70.8887,  94.6564, 4.1510 },
      { 104.8957,  65.4684, 2.1480 },
      {  62.5257,  36.2424, 1.2590 },
      {  48.6047,  72.8824, 3.4750 },
      {  42.8639,  93.2835, 2.1470 },
      { 105.0609,  57.4165, 1.2600 },
      { 104.3889,  53.7185, 0.9240 },
      {  33.3619,  41.6875, 4.0540 },
      {  38.0979,  66.9685, 2.6320 },
      {  47.2959,  32.9445, 3.4930 },
      {  54.0759,  26.2755, 3.5160 },
      {  59.2599,  83.8605, 2.5450 },
      {  53.2559,  55.6815, 3.5160 },
      {  35.6559,  56.9115, 2.1480 },
      {  41.4969,  74.1405, 1.9350 },
      {  38.6299,  49.7946, 2.1470 },
      {  54.0709,  33.2505, 2.1490 },
      {  60.3729,  23.6845, 2.1480 },
      {  68.1619,  22.9995, 2.1480 },
      {  79.6469,  25.4605, 2.1470 },
      {  73.0149,  39.6136, 1.5330 },
      {  43.2439,  65.3215, 1.5330 },
      {  35.4669,  61.7655, 1.5330 },
      {  41.2919,  30.9945, 1.5320 },
      {  47.6439,  26.8275, 1.5330 },
      {  41.7009,  36.4695, 1.5330 },
      {  43.6149,  88.2946, 1.5330 },
      {  46.5559,  96.9765, 1.5330 },
      {  58.9989, 100.9415, 1.5320 },
      {  43.8209,  83.8495, 1.5320 },
      {  56.9479,  88.0876, 1.5320 },
      {  64.4689,  96.8395, 1.5330 },
      {  60.9139,  89.0446, 1.5330 },
      {  63.9609,  85.3825, 1.5330 },
      {  65.5629,  47.3385, 1.2590 },
      {  41.6339,  40.9125, 1.2590 },
      {  84.5749,  26.4135, 1.2590 },
      {  88.1299,  28.6055, 1.2590 },
      {  68.7079,  30.5215, 1.2590 },
      {  46.5549,  44.6055, 1.2590 },
      {  31.9809,  54.1835, 1.2590 },
      {  91.4069,  31.3415, 1.2590 },
      {  72.6149,  22.7615, 1.2600 },
      {  95.5089,  35.7165, 1.2590 },
      {  43.2449,  70.0016, 1.2590 },
      {  45.9389,  67.7875, 0.8280 },
      {  38.3909,  76.1325, 0.8290 },
      {  33.5079,  64.9095, 1.0120 },
      {  62.4469,  80.0485, 0.9860 },
      {  64.4689,  92.1225, 0.9860 },
      {  74.2449, 100.9415, 0.9850 },
      {  59.3819,  96.9765, 0.9850 },
      {  47.6159,  83.8645, 0.9850 },
      {  51.0689,  67.7256, 0.6160 },
      {  48.5379,  67.1715, 0.6160 },
      {  50.6569,  80.4176, 0.6160 },
      {  52.8739,  81.3165, 0.6160 },
      {  69.8029,  53.9026, 0.6150 },
      {  83.2019,  46.5195, 0.6160 },
      {  82.0219,  44.3315, 0.6160 },
      {  78.1429,  52.3985, 0.6160 },
      {  78.2799,  44.3305, 0.6160 },
      {  57.5629,  46.1085, 0.6160 },
      {  73.4259,  62.7285, 0.6150 },
      {  84.9129,  71.6425, 0.6160 },
      {  90.1479,  72.1605, 0.6160 },
      {  92.2099,  82.8805, 0.6160 },
      {  61.9379,  66.3465, 0.9250 },
      {  46.3099,  81.1435, 0.9240 },
      {  60.6739,  56.9105, 0.9240 },
      {  54.0769,  42.0065, 0.9230 },
      {  94.6879,  48.2965, 0.9240 },
      {  44.6419,  50.8955, 0.9240 },
      {  34.1619,  48.1945, 0.9240 },
      {  35.1529,  52.5565, 0.9250 },
      {  91.8169,  35.9225, 0.6150 },
      {  99.2009,  39.6815, 0.6170 },
      {  77.8699,  34.2125, 0.6160 },
      {  75.2609,  23.9505, 0.6160 },
      {  72.8099,  26.8285, 0.6150 },
      {  84.2959,  37.4935, 0.6160 },
      {  49.1539,  50.3475, 0.6150 },
      {  46.5549,  54.3135, 0.6160 },
      {  51.8889,  61.1505, 0.6160 },
      {  48.6329,  81.4155, 0.6150 },
      {  64.3029,  89.0446, 0.6160 },
      {  70.7589,  88.7715, 0.6160 },
      {  79.0259,  87.1266, 0.6160 },
      {  87.8229,  89.6195, 0.6160 },
      {  62.1439, 102.1725, 0.9860 },
      {  61.7329,  98.7536, 0.9850 },
      {  55.1709,  79.8825, 1.2590 },
      {  32.9449,  79.7035, 4.0080 },
      {  28.7889,  59.8885, 4.4040 },
      {  85.5819,  88.9855, 1.0100 },
      {  97.1309,  73.6945, 1.5330 },
      {  80.6229,  66.8545, 1.1050 },
      {  81.1029,  92.4005, 0.9860 },
      {  95.8939,  65.7575, 0.6160 },
      {  96.2149,  88.6015, 0.9240 },
      {  99.4209,  75.3705, 0.6160 },
      { 104.2069,  75.9176, 0.6170 },
      {  94.2499,  75.8786, 0.9860 },
      {  94.3039,  79.3955, 0.9850 },
      {  81.1029,  78.9485, 0.6150 },
      {  89.3899,  82.7275, 0.8290 },
      {  84.5739,  77.2465, 0.9860 }
    };
    /// <summary>
    /// Number of disc having the 1st color.
    /// </summary>
    protected const int FIRST_COLOR = 54;
    protected static double minX, maxX, minY, maxY;
    protected static double maxR;
    protected static List<Disc> discs;
    protected static double kxy;
    protected static double dx, dy;
    protected const double SIZE = 0.9;
    protected static void SetViewport(int width, int height)
    {
      double k = (width * SIZE) / (maxX - minX);
      kxy = (height * SIZE) / (maxY - minY);
      if (k < kxy) kxy = k;
      dx = 0.5 * (width - (minX + maxX) * kxy);
      dy = 0.5 * (height - (minY + maxY) * kxy);
    }
    protected static float X(double x) => (float)(x * kxy + dx);
    protected static float Y(double y) => (float)(y * kxy + dy);
    public static void InitAnimation2 (int width, int height, double start, double end, double fps)
    {
      // Disc data initialization:
      minX = minY = double.MaxValue;
      maxX = maxY = maxR = double.MinValue;
      discs = new List<Disc>();

      for (int i = 0; i < DISC_DATA.Length / 3; i++)
      {
        double x = DISC_DATA[i, 0];
        double y = DISC_DATA[i, 1];
        double r = DISC_DATA[i, 2];
        discs.Add(new Disc(x, y, r, i < FIRST_COLOR ? COLORS[0] : COLORS[1]));
        if (x - r < minX)
          minX = x - r;
        if (x + r > maxX)
          maxX = x + r;
        if (y - r < minY)
          minY = y - r;
        if (y + r > maxY)
          maxY = y + r;
        if (r > maxR)
          maxR = r;
      }
      SetViewport(width, height);
    }
    public static Bitmap RenderFrame2 (int width, int height, double time, double start, double end)
    {
      Bitmap result = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
      Graphics gr = Graphics.FromImage(result);
      SolidBrush br = new SolidBrush(Color.White);
      gr.FillRectangle(br, 0, 0, width, height);
      gr.SmoothingMode = SmoothingMode.AntiAlias;

      double tim = (time - start) / (end - start);
      float radius = (float)(maxR * tim);

      foreach (Disc d in discs)
      {
        br.Color = d.color;
        float r = (float)(Math.Min(radius, d.radius) * kxy);
        gr.FillEllipse(br, X(d.cx) - r, Y(d.cy) - r, r + r, r + r);
      }

      return result;
    }
    #endregion

    #region Entities & Tweens
    private interface IParametric
    {
      void Parametric (double t, out double x, out double y);
    }
    private class Line : IParametric
    {
      private readonly double fromX;
      private readonly double fromY;
      private readonly double toX;
      private readonly double toY;

      public Line (double fromX, double fromY, double toX, double toY)
      {
        this.fromX = fromX;
        this.fromY = fromY;
        this.toX = toX;
        this.toY = toY;
      }

      public void Parametric (double t, out double x, out double y)
      {
        x = (toX - fromX) * t + fromX;
        y = (toY - fromY) * t + fromY;
      }
    }
    private class Twister : IParametric
    {
      private readonly double middleX;
      private readonly double middleY;
      private readonly double startAngle;
      private readonly double size;

      public Twister (double middleX, double middleY, double startAngle, double size)
      {
        this.middleX = middleX;
        this.middleY = middleY;
        this.startAngle = startAngle;
        this.size = size;
      }

      public void Parametric (double t, out double x, out double y)
      {
        t = 1 - t;
        t *= TwisterSpinningMax;
        x = t * size * Math.Cos(t + startAngle) / TwisterSpinningMax + middleX;
        y = t * size * Math.Sin(t + startAngle) / TwisterSpinningMax + middleY;
      }
    }

    private static List<(double, double, double, double)> bounceValues = new List<(double, double, double, double)>();
    private static void InitBouncies(int discCount)
    {
      for (int i = 0; i < discCount; i++)
      {
        bounceValues.Add((
          cosxPow(),
          period(),
          speed(),
          xPow()
          ));
      }
    }
    /// <summary>
    /// Item1
    /// </summary>
    private static double period() => 2.5d + rnd.Next(0);
    /// <summary>
    /// Item2
    /// </summary>
    private static double speed() => 20d + rnd.Next(3) * 10;
    /// <summary>
    /// Item3
    /// </summary>
    private static double cosxPow() => 4d + rnd.NextDouble() * 2;
    /// <summary>
    /// Item4
    /// </summary>
    private static double xPow() => 2d + rnd.Next(3);

    private static double BounceDiscs (double t, int i)
    {
      return 1 - Math.Abs(
        Math.Cos(
          Math.Pow(t, bounceValues[i].Item1) //cosxpow
          *
          bounceValues[i].Item2 //period
          *
          Math.PI
        )
        *
        Math.Pow(bounceValues[i].Item3, -Math.Pow(t, bounceValues[i].Item4)) // speed, xpow
      );
    }
    private static double SmoothWait (double t, int i) => t;
    private static double SmoothStart2 (double t, int i) => t * t;
    private static double SmoothStartEnd (double t, int i) => -(Math.Cos(Math.PI * t) - 1) / 2;
    private static double SmoothStop (double t, int i) => 1 - ((1 - t) * (1 - t));
    #endregion

    private static (double, double) GetMid()
    {
      double lminX = double.MaxValue;
      double lminY = double.MaxValue;
      double lmaxX = double.MinValue;
      double lmaxY = double.MinValue;
      for (int i = 0; i < DISC_DATA.Length / 3; i++)
      {
        lmaxX = Math.Max(lmaxX, DISC_DATA[i, 0]);
        lmaxY = Math.Max(lmaxY, DISC_DATA[i, 1]);
        lminX = Math.Min(lminX, DISC_DATA[i, 0]);
        lminY = Math.Min(lminY, DISC_DATA[i, 1]);
      }
      return ((lmaxX - lminX) / 2d + lminX, (lmaxY - lminY) / 2d + lminY);
    }

    // Create 4 phases
    private static List<List<IParametric>> phases = new List<List<IParametric>>()
    {
      new List<IParametric>(),
      new List<IParametric>(),
      new List<IParametric>(),
      new List<IParametric>(),
      new List<IParametric>(),
    };
    private static List<Func<double, int, double>> phasesTweens = new List<Func<double, int, double>>()
    {
      BounceDiscs,
      SmoothStartEnd,
      SmoothWait,
      SmoothStop,
      SmoothStop,
    };
    private static List<double> timeValues = new List<double>() {  };

    // Global
    private static Random rnd = new Random();

    // Parametric Settings
    private const double ImageScale = 10.0;
    private const double ShootRange = 0.66d;
    private const double TwisterSpinningMax = 1.5 * Math.PI;
    private static List<double> timeShares = new List<double>() { 0.15, 0.15, 0.05, 0.20, 0.45 };
    private static double GetRandomStartingAngle () => rnd.NextDouble() * 2 * Math.PI;

    /// <summary>
    /// Initialize form parameters.
    /// </summary>
    public static void InitParams(out string name, out int wid, out int hei, out double from, out double to, out double fps)
    {
      // Author.
      name = "Miroslav Valach";
      // Single frame.
      wid = 1920;
      hei = 1080;
      // Animation.
      from = 0.0;
      to = 30.0;
      fps = 60.0;

      // Scale Image
      for (int i = 0; i < DISC_DATA.Length / 3; i++)
      {
        DISC_DATA[i, 0] *= ImageScale;
        DISC_DATA[i, 1] *= ImageScale;
        DISC_DATA[i, 2] *= ImageScale;
      }

      double sum = 0d;
      // Calc phase
      for (int i = 0; i < timeShares.Count; i++)
      {
        timeValues.Add(sum);
        sum += timeShares[i];
      }
      timeValues.Add(1d);

      InitBouncies(DISC_DATA.Length / 3);
    }


    /// <summary>
    /// Global initialization. Called before each animation batch
    /// or single-frame computation.
    /// </summary>
    /// <param name="width">Width of future frames in pixels.</param>
    /// <param name="height">Height of future frames in pixels.</param>
    /// <param name="start">Start time (t0)</param>
    /// <param name="end">End time (for animation length normalization).</param>
    /// <param name="fps">Required fps.</param>
    public static void InitAnimation(int width, int height, double start, double end, double fps)
    {
      discs = new List<Disc>();

      (double, double) imageMid = (width / 2d, height / 2d);
      (double, double) discMid = GetMid();
      double distX = (imageMid.Item1 - discMid.Item1);
      double distY = (imageMid.Item2 - discMid.Item2);
      discMid.Item1 += distX;
      discMid.Item2 += distY;

      for (int i = 0; i < DISC_DATA.Length / 3; i++)
      {
        double originalX = (DISC_DATA[i, 0] + distX);
        double originalY = (DISC_DATA[i, 1] + distY);

        float x = (float)rnd.NextDouble() * width;
        float y = -(float)rnd.NextDouble() * height / 2;

        (double, double) shootDirection = (originalX - discMid.Item1, originalY - discMid.Item2);
        double shootDistance = Math.Sqrt(shootDirection.Item1 * shootDirection.Item1 + shootDirection.Item2 * shootDirection.Item2);
        shootDirection.Item1 /= shootDistance;
        shootDirection.Item2 /= shootDistance;

        phases[0].Add(new Line(x, y, x, height));

        phases[1].Add(new Line(x, height, originalX, originalY));

        phases[2].Add(new Line(originalX, originalY, originalX, originalY));

        phases[3].Add(new Line(originalX, originalY, originalX + shootDirection.Item1 * (width * ShootRange), originalY + shootDirection.Item2 * (height * ShootRange)));

        phases[4].Add(new Twister(originalX, originalY, GetRandomStartingAngle(), width * 0.75));

        Color color = (i < FIRST_COLOR) ? COLORS[0] : COLORS[1];
        discs.Add(new Disc(0, 0, DISC_DATA[i, 2], color));
      }
    }

    private static int GetPhaseFromTime (double time)
    {
      int i = -1;
      double passed = 0;
      while (passed <= time)
      {
        i++;
        passed += timeShares[i];
      }
      return i;
    }

    /// <summary>
    /// Draw single animation frame.
    /// </summary>
    /// <param name="width">Required frame width in pixels.</param>
    /// <param name="height">Required frame height in pixels.</param>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="start">Start time (t0)</param>
    /// <param name="end">End time (for animation length normalization).</param>
    public static Bitmap RenderFrame(int width, int height, double time, double start, double end)
    {
      double percTime = time / (end - start);
      int currentPhase = GetPhaseFromTime(percTime);
      double t = (percTime - timeValues[currentPhase]) / (timeValues[currentPhase + 1] - timeValues[currentPhase]);

      Bitmap result = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
      Graphics gr = Graphics.FromImage(result);
      gr.SmoothingMode = SmoothingMode.AntiAlias;
      gr.Clear(Color.White);
      SolidBrush br = new SolidBrush(Color.White);

      List<IParametric> trajectories = phases[currentPhase];
      Func<double, int, double> tween = phasesTweens[currentPhase];

      for (int i = 0; i < discs.Count; i++)
      {
        Disc d = discs[i];
        trajectories[i].Parametric(tween(t, i), out d.cx, out d.cy);
        br.Color = d.color;
        gr.FillEllipse(br, (float)(d.cx - d.radius), (float)(d.cy - d.radius), (float)(d.radius * 2), (float)(d.radius * 2));
      }

      return result;
    }
  }
}
