using System;
using System.Drawing;
using MathSupport;
using CircleCanvas;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Utilities;

namespace _083animation
{
  public class Animation
  {
    /// <summary>
    /// Form data initialization.
    /// </summary>
    /// <param name="name">Your first-name and last-name.</param>
    /// <param name="wid">Initial image width in pixels.</param>
    /// <param name="hei">Initial image height in pixels.</param>
    /// <param name="from">Start time (t0)</param>
    /// <param name="to">End time (for animation length normalization).</param>
    /// <param name="fps">Frames-per-second.</param>
    /// <param name="param">Optional text to initialize the form's text-field.</param>
    /// <param name="tooltip">Optional tooltip = param help.</param>
    public static void InitParams (out string name, out int wid, out int hei, out double from, out double to, out double fps, out string param, out string tooltip)
    {
      // {{

      name = "Miroslav Valach";
      wid = 800;
      hei = 600;
      param = "LENGTH_MULTI=2.0,SPIRAL_COUNT=8,RADIUS=20.0,SATURATION=1.0,INTERVAL_BASE=0.04, POWER=4.0, SPEED=1.0";
      tooltip =
                "<LENGTH_MULTI> .. length of whirlwind" + "/n"
              + "<SPIRAL_COUNT> .. count of whirlwind" + "/n"
              + "<RADIUS> .. size of single circle in whirlwind" + "/n"
              + "<SATURATION> .. color saturation" + "/n"
              + "<INTERVAL_BASE> .. reverse interval change between circles"  + "\n"
              + "<POWER> .. affects how quickly discs disappear"  + "\n"
              + "<SPEED> .. animation speed (double)";

      // Animation.
      from =  0.0;
      to   = 10.0;
      fps  = 60.0;

      // Form params.
      //param = "objects=1200,seed=12,speed=1.0";
      //tooltip = "objects=<int>, seed=<long>, speed=<double>";

      // }}
    }

    /// <summary>
    /// Global initialization. Called before each animation batch
    /// or single-frame computation.
    /// </summary>
    /// <param name="width">Width of the future canvas in pixels.</param>
    /// <param name="height">Height of the future canvas in pixels.</param>
    /// <param name="start">Start time (t0)</param>
    /// <param name="end">End time (for animation length normalization).</param>
    /// <param name="fps">Required fps.</param>
    /// <param name="param">Text parameter field from the form.</param>
    public static void InitAnimation (int width, int height, double start, double end, double fps, string param)
    {
      // {{ TODO: put your init code here
      // DONE
      // }}
    }

    /// <summary>
    /// Draw single animation frame.
    /// </summary>
    /// <param name="c">Canvas to draw to.</param>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="start">Start time (t0)</param>
    /// <param name="end">End time (for animation length normalization).</param>
    /// <param name="param">Optional string parameter from the form.</param>
    public static void DrawFrame (Canvas c, double time, double start, double end, string param)
    {
      c.SetAntiAlias(true);
      c.Clear(Color.White);

      Whirlwind whirlwind = new Whirlwind(c);

      ParseFromParam(whirlwind, param);

      whirlwind.DrawWhirlwinds(time * whirlwind.speed);
    }

    static void ParseFromParam (Whirlwind whirlwind, string param)
    {
      Dictionary<string, string> p = Util.ParseKeyValueList(param);

      double LENGTH_MULTI = 2.0;
      Util.TryParse(p, "LENGTH_MULTI", ref LENGTH_MULTI);
      whirlwind.lengthMulti = LENGTH_MULTI;

      double SPIRAL_COUNT = 8;
      Util.TryParse(p, "SPIRAL_COUNT", ref SPIRAL_COUNT);
      whirlwind.spiralCount = SPIRAL_COUNT;

      double RADIUS = 20.0;
      Util.TryParse(p, "RADIUS", ref RADIUS);
      whirlwind.radius = RADIUS;

      double SATURATION = 1.0;
      Util.TryParse(p, "SATURATION", ref SATURATION);
      whirlwind.saturation = SATURATION;

      double INTERVAL_BASE = 0.04;
      Util.TryParse(p, "INTERVAL_BASE", ref INTERVAL_BASE);
      whirlwind.interval_base = INTERVAL_BASE;

      double SPEED = 1.0;
      Util.TryParse(p, "SPEED", ref SPEED);
      whirlwind.speed = SPEED;

      double POWER = 4.0;
      Util.TryParse(p, "POWER", ref POWER);
      whirlwind.power = POWER;
    }

    public class Whirlwind
    {
      private readonly Canvas canvas;

      public Whirlwind (Canvas canvas)
      {
        this.canvas = canvas;
        xMid = canvas.Width / 2.0;
        yMid = canvas.Height / 2.0;
      }

      //Parametrized Values
      public double lengthMulti = 2;
      public double spiralCount = 8;
      public double radius = 20.0;
      public double saturation = 1.0;
      public double interval_base = 0.04;
      public double distance_change = 0.25;
      public double power = 4.0;

      // Following values are calculated.
      public double xMid;
      public double yMid;
      public double hue;
      public double rotation;
      public double speed;
      public double m => length * (2.0 / 3.0) * 10;
      public double interval (double i) => interval_base * i;
      public double length => lengthMulti * Math.PI;

      public void DrawWhirlwinds (double time)
      {
        for (double i = 0; i < spiralCount; i++)
        {
          double percentage = i / spiralCount;
          hue = 360 * percentage;
          rotation = 2 * Math.PI * percentage;
          SingleWhirlwind(time);
        }
      }

      public void SingleWhirlwind (double time)
      {
        List<double> discs = new List<double>();
        for (double discPos = distance_change; discPos < length; discPos += distance_change)
        {
          discs.Add((discPos + time) % length);
        }
        discs.Sort();

        for (int i = 1; i < discs.Count; i++)
        {
          (double x, double y) = Whirl(m, discs[i] + interval(i), rotation, xMid, yMid);
          double r, g, b, alpha;
          alpha = 1 - Math.Pow(discs[i] / length, power);
          Arith.HSVtoRGB(hue, saturation, discs[i] / length, out r, out g, out b);
          canvas.SetColor(Color.FromArgb((int) (alpha * 255), (int)(r * 255), (int)(g * 255), (int)(b * 255)));
          canvas.FillDisc((float)x, (float)y, (float)(radius * (discs[i] / length)));
        }
      }
    }

    public static (double, double) Whirl (double m, double t, double rotation, double xMid, double yMid)
    {
      double x = m * t * Math.Sin(t + rotation) + xMid;
      double y = m * t * Math.Cos(t + rotation) + yMid;
      return (x, y);
    }

  }
}
