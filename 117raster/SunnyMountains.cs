// Simple script that attempts to generate sun and mountains.
// It's basically lot of RNG.
// This one will result in reasonable image.
// fast,create,script=SunnyMountains.cs,wid=1000,hei=500,sun=true,sm=3.0,mm=1.0,polar=350,maxGen=7,sunMinLevel=200,cSun=[255;255;0],cSky=[135;206;235],cSnow=[255;255;255],cHill=[128;128;128],cGreen=[0;128;0]
// This one was used to debug most of the code.
// fast,create,script=SunnyMountains.cs,wid=1000,hei=500,sun=true,sm=3.0,mm=1.0,polar=350,maxGen=5,sunMinLevel=0,cSun=[255;255;0],cSky=[135;206;235],cSnow=[255;255;255],cHill=[128;128;128],cGreen=[0;128;0]
// Cursed.
// fast,create,script=SunnyMountains.cs,wid=1000,hei=500,sun=true,sm=3.0,mm=1.0,polar=350,maxGen=7,sunMinLevel=200,cSun=[0;0;0],cSky=[255;0;0],cSnow=[255;255;255],cHill=[128;0;128],cGreen=[0;0;0]
using System;
using System.CodeDom;
using System.Diagnostics;

// Internal variables
Random MountainRNG = new Random();
// Sun
Vector2 SunPosition;
double SunMaxDistance;
// Mountains
int[,] Image;
int ImageX;
int ImageY;
const int PolarLevel = 150;
const int MaxGenDefault = 5;
const double SunMulti = 3.0;
const double MountMulti = 1.0;

// Common func
(float, float, float) ColorLerp(Color min, Color max, double percentage, double multi)
{
  double R = (min.R + (max.R - min.R) * percentage * multi) / 255;
  double G = (min.G + (max.G - min.G) * percentage * multi) / 255;
  double B = (min.B + (max.B - min.B) * percentage * multi) / 255;
  return ((float)R, (float)G, (float)B);
}
double GetPercentage(double distance, double maxDistance) => distance / maxDistance;

// Sun func
double GetMaxDistanceForSun(Vector2 sunPoint, int maxWidth, int maxHeight)
{
  int width = maxWidth - 1;
  int height = maxHeight - 1;
  var leftUpper = Vector2.Distance(sunPoint, new Vector2(0, 0));
  var leftDown = Vector2.Distance(sunPoint, new Vector2(0, height));
  var rightUpper = Vector2.Distance(sunPoint, new Vector2(width, 0));
  var rightDown = Vector2.Distance(sunPoint, new Vector2(width, height));
  return Math.Max(Math.Max(leftDown, leftUpper), Math.Max(rightDown, rightUpper));
}
void DefineSun(int maxWidth, int maxHeight, bool randomizeLocation, int sunMinLevel)
{
  if (randomizeLocation)
  {
    SunPosition = new Vector2(MountainRNG.Next(0, maxWidth), MountainRNG.Next(0, maxHeight - sunMinLevel));
  }
  else
  {
    SunPosition = new Vector2(maxWidth / 2, maxHeight / 2);
  }
  SunMaxDistance = GetMaxDistanceForSun(SunPosition, maxWidth, maxHeight);
}
(float, float, float) PrintSun(int x, int y, double multi, Color sun, Color sky)
{
  double SunCurrentDistance = Vector2.Distance(SunPosition, new Vector2(x, y));
  return ColorLerp(sun, sky, GetPercentage(SunCurrentDistance, SunMaxDistance), multi);
}

// Mountain func
void GenerateMountains(int minMountainHeight, int maxMountainHeight, int setValue = 1)
{
  int currentHeight = ImageY - 1;
  int direction = 0;
  int CurrentY = ImageY - 1;
  for (int x = 0; x < ImageX; x++) //for all members of x axis
  {
    if (MountainRNG.Next(0, ImageX / 2) != 0)
      continue;
    //Define Mountain
    int mountainHeight = MountainRNG.Next(minMountainHeight, maxMountainHeight);
    //Fill Top
    Image[x, mountainHeight] = Image[x, mountainHeight] == 0 ? setValue : Image[x, mountainHeight];
    int lastFilledLeftX = x;
    int lastFilledRightX = x;
    int mountainLeftAngle = MountainRNG.Next(1, 4);
    int mountainRightAngle = MountainRNG.Next(1, 4);
    if (MountainRNG.Next(0, 2) != 0)
    {
      //Smaller than 45
      for (int y = mountainHeight; y < ImageY; y++)
      {
        //Fill Mountain Down Left
        if (lastFilledLeftX > 0)
          Image[lastFilledLeftX, y] = Image[lastFilledLeftX, y] == 0 ? setValue : Image[lastFilledLeftX, y];
        //Fill Mountain Down Right
        if (lastFilledRightX < ImageX)
          Image[lastFilledRightX, y] = Image[lastFilledRightX, y] == 0 ? setValue : Image[lastFilledRightX, y];

        if (y % mountainLeftAngle == 0)
          lastFilledLeftX--;
        if (y % mountainRightAngle == 0)
          lastFilledRightX++;
      }
    }
    else
    {
      //Larger than 45
      for (int y = mountainHeight; y < ImageY; y++)
      {
        //Fill Mountain Down Left
        for (int xl = lastFilledLeftX; xl > lastFilledLeftX - mountainLeftAngle && xl > 0; xl--)
        {
          Image[xl, y] = Image[xl, y] == 0 ? setValue : Image[xl, y];
        }
        //Fill Mountain Down Right
        for (int xr = lastFilledRightX; xr < lastFilledRightX + mountainRightAngle && xr < ImageX; xr++)
        {
          Image[xr, y] = Image[xr, y] == 0 ? setValue : Image[xr, y];
        }

        lastFilledLeftX -= mountainLeftAngle;
        lastFilledRightX += mountainRightAngle;
      }
    }
  }
}

/// <summary>
/// [0,0]-[X,0]
/// |         |
/// [0,Y]-[X,Y]
/// </summary>
void CreateMountains(int width, int height, bool randomizeSunLocation, int maxGeneration, int sunMinLevel)
{
  // Save size and create image.
  ImageX = width;
  ImageY = height;
  Image = new int[width, height];
  // Define sun.
  DefineSun(width, height, randomizeSunLocation, sunMinLevel);
  // Define mountains.
  double moveY = ImageY / maxGeneration;
  int generationMax = maxGeneration - 1;
  int generationMin = (generationMax + generationMax % 2) / 2;
  for (int i = 0; i <= MountainRNG.Next(generationMin, generationMax); i++)
  {
    GenerateMountains((int)Math.Max(ImageY - moveY * (i + 1), 0), (int)Math.Max(ImageY - moveY * i, 0), i + 1);
  }
}

//polarLevelStart = 0
(float, float, float) PrintMountain(int x, int y, int polarLevelEnd, double multi, Color snow, Color hill, Color green)
{
  if (y < polarLevelEnd) //whiteness
  {
    double distanceFromPolarStart = Vector2.Distance(new Vector2(x, y), new Vector2(x, 0));

    return ColorLerp(snow, hill, GetPercentage(distanceFromPolarStart, polarLevelEnd), multi);
  }
  else //greenery
  {
    double distanceFromPolarEnd = Vector2.Distance(new Vector2(x, y), new Vector2(x, polarLevelEnd));

    return ColorLerp(hill, green, GetPercentage(distanceFromPolarEnd, ImageY), multi);
  }
}

// Return Y position if possible otherwise -1
// Find highest mountain point of first found generation or return self if it's mountain point
(int, int) GetHigherMountainPoint(int x, int currentY)
{
  int closestGeneration = -1;
  int returnPoint = -1;
  // go from bottom to skies
  for (int y = currentY; y > 0; y--) 
  {
    // find earliest generation
    if (Image[x, y] > 0 && closestGeneration == -1)
    {
      closestGeneration = Image[x, y];
      returnPoint = y;
    }
    else if(Image[x, y] > 0 && closestGeneration >= Image[x, y]) // lower as first generation, update
    {
      closestGeneration = Image[x, y];
      returnPoint = y;
    }
  }
  return (returnPoint, closestGeneration);
}


// SunnyMountainsShow.
formula.pixelCreate = (in ImageContext ic, out float R, out float G, out float B) =>
{
  // Get polar level
  int polarLocation = PolarLevel;
  Util.TryParse(ic.context, "polar", ref polarLocation);
  // Get polar level
  double sm = SunMulti;
  Util.TryParse(ic.context, "sm", ref sm);
  double mm = MountMulti;
  Util.TryParse(ic.context, "mm", ref mm);

  (float, float, float) colorTuple;
  // Is mountain ?
  (int, int) maxPoint = GetHigherMountainPoint(ic.x, ic.y);
  if (maxPoint.Item1 != -1)
  {
    Color snowColor = Color.White;
    Color hillColor = Color.Gray;
    Color greenColor = Color.Green;
    if (ic.context.TryGetValue("cSnow", out object fgo1) && fgo1 is int[] fg1)
      snowColor = Color.FromArgb(fg1[0], fg1[1], fg1[2]);
    if (ic.context.TryGetValue("cHill", out object fgo2) && fgo2 is int[] fg2)
      hillColor = Color.FromArgb(fg2[0], fg2[1], fg2[2]);
    if (ic.context.TryGetValue("cGreen", out object fgo3) && fgo3 is int[] fg3)
      greenColor = Color.FromArgb(fg3[0], fg3[1], fg3[2]);
    colorTuple = PrintMountain(ic.x, ic.y, polarLocation, mm, snowColor, hillColor, greenColor);
    // Make each generation look little different.
    colorTuple.Item1 *= (1f + maxPoint.Item2 / 10f);
    colorTuple.Item2 *= (1f + maxPoint.Item2 / 10f);
    colorTuple.Item3 *= (1f + maxPoint.Item2 / 10f);
  }
  else // Otherwise is Sky or Sun
  {
    Color sunColor = Color.Yellow;
    Color skyColor = Color.SkyBlue;
    if (ic.context.TryGetValue("cSun", out object fgo1) && fgo1 is int[] fg1)
      sunColor = Color.FromArgb(fg1[0], fg1[1], fg1[2]);
    if (ic.context.TryGetValue("cSky", out object fgo2) && fgo2 is int[] fg2)
      skyColor = Color.FromArgb(fg2[0], fg2[1], fg2[2]);
    colorTuple = PrintSun(ic.x, ic.y, sm, sunColor, skyColor);
  }

  R = colorTuple.Item1;
  G = colorTuple.Item2;
  B = colorTuple.Item3;
};

formula.contextCreate = (in Bitmap input, in string param) =>
{
  if (string.IsNullOrEmpty(param))
    return null;

  Dictionary<string, string> parameters = Util.ParseKeyValueList(param);
  Dictionary<string, object> sc = new Dictionary<string, object>();

  //Generation needs width and height
  int learnWidth = 0;
  Util.TryParse(parameters, "wid", ref learnWidth);
  int learnHeight = 0;
  Util.TryParse(parameters, "hei", ref learnHeight);

  //Randomize Sun Location
  bool sunLocation = true;
  Util.TryParse(parameters, "sun", ref sunLocation);
  sc["sun"] = sunLocation;

  //Minimum Sun Location
  int sunMinLevel = 0;
  Util.TryParse(parameters, "sunMinLevel", ref sunMinLevel);
  sc["sunMinLevel"] = sunMinLevel;

  //Generation needs width and height
  double sunMulti = SunMulti;
  Util.TryParse(parameters, "sm", ref sunMulti);
  sc["sm"] = sunLocation;
  double mountMulti = MountMulti;
  Util.TryParse(parameters, "mm", ref mountMulti);
  sc["mm"] = sunLocation;

  //Polar Level Location
  int polarLocation = PolarLevel;
  Util.TryParse(parameters, "polar", ref polarLocation);
  sc["polar"] = polarLocation;

  //Max Level Location
  int maxGen = MaxGenDefault;
  Util.TryParse(parameters, "maxGen", ref maxGen);
  sc["maxGen"] = maxGen;


  // colors
  int R = 0, G = 0, B = 0;
  R = Color.Yellow.R;
  G = Color.Yellow.G;
  B = Color.Yellow.B;
  if (TryParse(parameters, "cSun", ref R, ref G, ref B, ';'))
    sc["cSun"] = new int[] { (int)R, (int)G, (int)B };
  R = Color.SkyBlue.R;
  G = Color.SkyBlue.G;
  B = Color.SkyBlue.B;
  if (TryParse(parameters, "cSky", ref R, ref G, ref B, ';'))
    sc["cSky"] = new int[] { (int)R, (int)G, (int)B };
  R = Color.White.R;
  G = Color.White.G;
  B = Color.White.B;
  if (TryParse(parameters, "cSnow", ref R, ref G, ref B, ';'))
    sc["cSnow"] = new int[] { (int)R, (int)G, (int)B };
  R = Color.Gray.R;
  G = Color.Gray.G;
  B = Color.Gray.B;
  if (TryParse(parameters, "cHill", ref R, ref G, ref B, ';'))
    sc["cHill"] = new int[] { (int)R, (int)G, (int)B };
  R = Color.Green.R;
  G = Color.Green.G;
  B = Color.Green.B;
  if (TryParse(parameters, "cGreen", ref R, ref G, ref B, ';'))
    sc["cGreen"] = new int[] { (int)R, (int)G, (int)B };

  //This will generate sun and mountains.
  CreateMountains(learnWidth, learnHeight, sunLocation, maxGen, sunMinLevel);

  sc["tooltip"] = "Width and Height were tested only as 1000 to 500, other should do fine hopefully.\n" +
                  "sun=<bool> .. randomize sun location (default true)\n" +
                  "sm=<double> .. sun multiplication constant (default 3.0), lower values == larger sun, larger values == smaller sun\n" +
                  "mm=<double> .. mountain multiplication constant (default 1.0), not recommended to change\n" +
                  "polar=<int> .. defines end of polar highpoint (default 350), end of snow (0 is up, Height is down, choose wisely), limited by Image Height\n" +
                  "maxGen=<int> .. defines max amount of generation to generate (default 5 == 1 2 3 4 5), min amount of generation is half of max, expects values larger than 3\n" +
                  "sunMinLevel=<int> .. defines minimum sun location (0 mean anywhere, Image Height - 1 mean always on top), limited by Image Height\n" +
                  "cSun=[R;G;B] .. sun(yellow) color\n" +
                  "cSky=[R;G;B] .. sky(skyBlue) color\n" +
                  "cSnow=[R;G;B] .. snow(white) color\n" +
                  "cHill=[R;G;B] .. hill(gray) color\n" +
                  "cGreen=[R;G;B] .. greenery(green) color";

  return sc;
};

public static bool TryParse(Dictionary<string, string> rec, string key, ref int R, ref int G, ref int B, char sep = ';')
{
  if (rec == null || !rec.TryGetValue(key, out string sval))
    return false;

  List<int> resulti = Util.ParseIntList(sval, sep);
  if (resulti != null)
  {
    // Integer color components from [0, 255]
    if (resulti.Count < 3)
      return false;
    R = (resulti[0]);
    G = (resulti[1]);
    B = (resulti[2]);
    return true;
  }

  List<float> resultf = Util.ParseFloatList(sval, sep);
  if (resultf != null &&
      resultf.Count >= 3)
  {
    // Floating color components w/o limits (for HDR).
    R = Math.Max((int)(resultf[0] * 255.0f), 255);
    G = Math.Max((int)(resultf[1] * 255.0f), 255);
    B = Math.Max((int)(resultf[2] * 255.0f), 255);
    return true;
  }

  return false;
}
