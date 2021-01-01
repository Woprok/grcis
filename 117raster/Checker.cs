// Text params -> script context.
// Any global pre-processing is allowed here.
// fast,create,script=Checker.cs,wid=640,hei=480,fg=[1.0;0.9;0.5],freq=5000,angle=45
formula.contextCreate = (in Bitmap input, in string param) =>
{
  if (string.IsNullOrEmpty(param))
    return null;

  Dictionary<string, string> p = Util.ParseKeyValueList(param);
  Dictionary<string, object> sc = new Dictionary<string, object>();

  // freq=<float>
  float freq = 500.0f;
  if (Util.TryParse(p, "freq", ref freq))
    freq = Util.Clamp(freq, 1.0f, 100000.0f);
  sc["freq"] = freq;

  // angle=<float>
  double angle = 0.0f;
  Util.TryParse(p, "angle", ref angle);
  angle = Arith.DegreeToRadian(angle);
  sc["vv"] = Math.Cos(angle);
  sc["uv"] = Math.Sin(angle);

  // bg=[R;G;B]
  float R = 0.0f, G = 0.0f, B = 0.0f;
  if (Util.TryParse(p, "bg", ref R, ref G, ref B, ';'))
    sc["bg"] = new float[] { R, G, B };

  // fg=[R;G;B]
  R = G = B = 1.0f;
  if (Util.TryParse(p, "fg", ref R, ref G, ref B, ';'))
    sc["fg"] = new float[] { R, G, B };

  sc["tooltip"] = "freq=<float> .. frequency for image generation (default=500)\r" +
                  "bg=[R;G;B] .. bacground color\r" +
                  "fg=[R;G;B] .. foreground color\r" +
                  "angle=<float> .. pattern angle in degrees";

  return sc;
};

// Checkerboard.
formula.pixelCreate = (
  in ImageContext ic,
  out float R,
  out float G,
  out float B) =>
{
  // Custom frequency,
  float frequency = 500.0f;
  Util.TryParse(ic.context, "freq", ref frequency);

  double mul = frequency / ic.width;
  long ord = 0L;

  if (ic.y > 0)
  {
    double u = mul * (ic.x - ic.width / 2) / ic.y;
    double v = frequency / ic.y;
    double vv = 1.0;
    Util.TryParse(ic.context, "vv", ref vv);
    double uv = 0.0;
    Util.TryParse(ic.context, "uv", ref uv);
    ord = (long)(Math.Round(vv * v + uv * u) + Math.Round(uv * v - vv * u));
  }

  // Output color.
  if ((ord & 1L) == 0)
  {
    if (ic.context.TryGetValue("fg", out object fgo) &&
        fgo is float[] fg)
    {
      R = fg[0];
      G = fg[1];
      B = fg[2];
    }
    else
      R = G = B = 1.0f;
  }
  else
  {
    if (ic.context.TryGetValue("bg", out object bgo) &&
        bgo is float[] bg)
    {
      R = bg[0];
      G = bg[1];
      B = bg[2];
    }
    else
      R = G = B = 0.0f;
  }
};
