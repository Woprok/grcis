// RetroStyle script.
// fast,script=RetroStyle.cs,color_count=4,linear_interpolation=0.0
// fast,script=RetroStyle.cs,color_count=4,linear_interpolation=0.5
// fast,script=RetroStyle.cs,color_count=8,linear_interpolation=0.0
// fast,script=RetroStyle.cs,color_count=8,linear_interpolation=0.5
// fast,script=RetroStyle.cs,color_count=16,linear_interpolation=0.0
// fast,script=RetroStyle.cs,color_count=16,linear_interpolation=0.5
// fast,script=RetroStyle.cs,color_count=1024,linear_interpolation=0.0
// fast,script=RetroStyle.cs,color_count=1024,linear_interpolation=0.5
//Usings
using System;
using System.Linq;
//Internal Variables
public static Random ColorRandom = new Random();

public struct RGB
{
  public int R;
  public int G;
  public int B;
  public RGB(int r, int g, int b)
  {
    R = r; G = g; B = b;
  }
  public bool Equals(RGB rgb)
  {
    return R == rgb.R && G == rgb.G && B == rgb.B;
  }
  public override bool Equals(object obj)
  {
    if (obj is RGB rgb)
      return Equals(rgb);
    else
      return false;
  }
  public override int GetHashCode()
  {
    return R * 1000000 + G * 1000 + B * 1;
  }
}

// Static class, to simplify access for ColorArray
public static class ColorData
{
  public static HashSet<RGB> ColorArray = null;

  public static void generate_colors(int color_count)
  {
    var color_set = new HashSet<RGB>();
    // Clearly this is dump way to generate colors, but it will work, through really big count is most likely going to take really long.
    while (color_set.Count < color_count)
    {
      color_set.Add(new RGB(ColorRandom.Next(0, 256), ColorRandom.Next(0, 256), ColorRandom.Next(0, 256)));
    }
    ColorData.ColorArray = color_set;
  }
}


// Any global pre-processing is allowed here.
formula.contextCreate = (in Bitmap input, in string param) =>
{
  if (string.IsNullOrEmpty(param))
    return null;

  Dictionary<string, string> p = Util.ParseKeyValueList(param);
  Dictionary<string, object> sc = new Dictionary<string, object>();

  int color_count = 16;
  if (Util.TryParse(p, "color_count", ref color_count))
    sc["color_count"] = Util.Clamp(color_count, 1, 16581375);

  float linear_interpolation = 1.0f;
  if (Util.TryParse(p, "linear_interpolation", ref linear_interpolation))
    linear_interpolation = Util.Clamp(linear_interpolation, 0.0f, 1.0f);
  sc["linear_interpolation"] = linear_interpolation;

  sc["tooltip"] = "color_count=<int> .. amount of colors to generate (default=16)\n" +
                  "linear_interpolation=<float> .. linear interpolation coefficient (1.0 - no effect, 0.5 - average of colors, 0 - closests color(default))";

  ColorData.generate_colors(color_count);
  return sc;
};

double difference(float r, float g, float b, RGB rgb)
{
  double h1, s1, v1;
  double h2, s2, v2;

  simple_RGBtoHSV(rgb,out h2,out s2,out v2);
  Arith.RGBtoHSV(r, g, b,out h1,out s1,out v1);
  var x = (Math.Sin(h1 / 360 * 2 * Math.PI) * s1 * v1 - Math.Sin(h2 / 360 * 2 * Math.PI) * s2 * v2);
  var y = (Math.Cos(h1 / 360 * 2 * Math.PI) * s1 * v1 - Math.Cos(h2 / 360 * 2 * Math.PI) * s2 * v2);
  var z = (v1 - v2);
  return x * x + y * y + z * z;
}

void simple_RGBtoHSV(RGB rgb, out double hue, out double saturation, out double value)
{
  Arith.RGBtoHSV(rgb.R / 255.0, rgb.G / 255.0, rgb.B / 255.0, out double h, out double s, out double v);
  hue = h;
  saturation = s;
  value = v;
}

RGB get_closest_color(float r, float g, float b)
{
  return ColorData.ColorArray.OrderBy(rgb => difference(r, g, b, rgb)).First();
}

// Replacing original color with random color + some interpolation
formula.pixelTransform0 = (
  in ImageContext ic,
  ref float R,
  ref float G,
  ref float B) =>
{
  //Get params
  int color_count = 16;
  Util.TryParse(ic.context, "color_count", ref color_count);
  float linear_interpolation = 1.0f;
  Util.TryParse(ic.context, "linear_interpolation", ref linear_interpolation);

  //Do retro magic
  var rgb = get_closest_color(R, G, B);

  R = Util.Clamp(R * linear_interpolation + (rgb.R / 255.0f) * (1 - linear_interpolation), 0.0f, 1.0f);
  G = Util.Clamp(G * linear_interpolation + (rgb.G / 255.0f) * (1 - linear_interpolation), 0.0f, 1.0f);
  B = Util.Clamp(B * linear_interpolation + (rgb.B / 255.0f) * (1 - linear_interpolation), 0.0f, 1.0f);

  // Output color was modified.
  return true;
};

// Just create something random.
formula.pixelCreate = (
  in ImageContext ic,
  out float R,
  out float G,
  out float B) =>
{
  var rgb = ColorData.ColorArray.ToList()[ColorRandom.Next(ColorData.ColorArray.Count)];
  R = rgb.R / 255.0f;
  G = rgb.G / 255.0f;
  B = rgb.B / 255.0f;
};
