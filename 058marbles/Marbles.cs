using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using MathSupport;
using OpenglSupport;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Configuration;
using System.Linq;
using Utilities;

namespace _058marbles
{
  /// <summary>
  /// Rendering data passed from MarblesWorld to MarblesRenderer
  /// </summary>
  public class MarblesRenderData
  {
    public CubeObject Cube;
    public List<SphereObject> Spheres;
    public MarblesRenderData (CubeObject walls, IEnumerable<SphereObject> spheres)
    {
      Cube = walls;
      Spheres = new List<SphereObject>(spheres);
    }
  }
  public class CubeObject
  {
    public readonly float wallBouncyness = 0.5f;
    public readonly float BaseSize = 1f;
    public readonly float RepulsionPowa = 50f;
    public readonly float Scale;
    public readonly float Alpha = 0.3f;
    public CubeObject(float scale)
    {
      Scale = scale;
    }
    private float CollisionDetectPoint(float radius) => BaseSize * Scale - radius;
    public bool Check(ref Vector3 pos, ref Vector3 vel, float mass, float radius, ref SphereCurrentType type)
    {
      bool isupdate = false;
      var detectionPoint = CollisionDetectPoint(radius);

      if (type == SphereCurrentType.MeteorMarbleAtmosphere)
      {
        if (Math.Abs(pos.X) <= detectionPoint && Math.Abs(pos.Y) <= detectionPoint && Math.Abs(pos.Z) <= detectionPoint)
        {
          type = SphereCurrentType.MeteorMarble;
        }
        if (type == SphereCurrentType.MeteorMarbleAtmosphere)
        {
          return isupdate;
        }
      }
      
      if (Math.Abs(pos.X) >= detectionPoint)
      {
        float clamped =  Arith.Clamp(pos.X, -detectionPoint, detectionPoint);
        float inWallPart = pos.X - clamped;

        vel.X = -vel.X - inWallPart * RepulsionPowa / mass;
        pos.X = clamped;
        isupdate = true;
      }
      if (Math.Abs(pos.Y) >= detectionPoint)
      {
        float clamped =  Arith.Clamp(pos.Y, -detectionPoint, detectionPoint);
        float inWallPart = pos.Y - clamped;

        vel.Y = -vel.Y - inWallPart * RepulsionPowa / mass;
        pos.Y = clamped;
        isupdate = true;
      }
      if (Math.Abs(pos.Z) >= detectionPoint)
      {
        float clamped =  Arith.Clamp(pos.Z, -detectionPoint, detectionPoint);
        float inWallPart = pos.Z - clamped;

        vel.Z = -vel.Z - inWallPart * RepulsionPowa / mass;
        pos.Z = clamped;
        isupdate = true;
      }
      return isupdate;
    }

    /// <summary>
    /// Draw colorful cube
    /// </summary>
    /// <returns>Amount to increment counter</returns>
    public int Draw ()
    {
      // color cube:
      GL.Enable(EnableCap.Blend);
      GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
      GL.Begin(PrimitiveType.Quads);

      GL.Color4(1.0f, 1.0f, 1.0f, Alpha);          // Set The Color To Green
      GL.Vertex3(1.0f*Scale, 1.0f * Scale, -1.0f * Scale);        // Top Right Of The Quad (Top)
      GL.Vertex3(-1.0f * Scale, 1.0f * Scale, -1.0f * Scale);       // Top Left Of The Quad (Top)
      GL.Vertex3(-1.0f * Scale, 1.0f * Scale, 1.0f * Scale);        // Bottom Left Of The Quad (Top)
      GL.Vertex3(1.0f * Scale, 1.0f * Scale, 1.0f * Scale);         // Bottom Right Of The Quad (Top)

      GL.Color4(1.0f, 1.0f, 1.0f, Alpha);         // Set The Color To Orange
      GL.Vertex3(1.0f * Scale, -1.0f * Scale, 1.0f * Scale);        // Top Right Of The Quad (Bottom)
      GL.Vertex3(-1.0f * Scale, -1.0f * Scale, 1.0f * Scale);       // Top Left Of The Quad (Bottom)
      GL.Vertex3(-1.0f * Scale, -1.0f * Scale, -1.0f * Scale);      // Bottom Left Of The Quad (Bottom)
      GL.Vertex3(1.0f * Scale, -1.0f * Scale, -1.0f * Scale);       // Bottom Right Of The Quad (Bottom)

      GL.Color4(1.0f, 1.0f, 1.0f, Alpha);         // Set The Color To Red
      GL.Vertex3(1.0f * Scale, 1.0f * Scale, 1.0f * Scale);         // Top Right Of The Quad (Front)
      GL.Vertex3(-1.0f * Scale, 1.0f * Scale, 1.0f * Scale);        // Top Left Of The Quad (Front)
      GL.Vertex3(-1.0f * Scale, -1.0f * Scale, 1.0f * Scale);       // Bottom Left Of The Quad (Front)
      GL.Vertex3(1.0f * Scale, -1.0f * Scale, 1.0f * Scale);        // Bottom Right Of The Quad (Front)

      GL.Color4(1.0f, 1.0f, 1.0f, Alpha);         // Set The Color To Yellow
      GL.Vertex3(1.0f * Scale, -1.0f * Scale, -1.0f * Scale);       // Bottom Left Of The Quad (Back)
      GL.Vertex3(-1.0f * Scale, -1.0f * Scale, -1.0f * Scale);      // Bottom Right Of The Quad (Back)
      GL.Vertex3(-1.0f * Scale, 1.0f * Scale, -1.0f * Scale);       // Top Right Of The Quad (Back)
      GL.Vertex3(1.0f * Scale, 1.0f * Scale, -1.0f * Scale);        // Top Left Of The Quad (Back)

      GL.Color4(1.0f, 1.0f, 1.0f, Alpha);          // Set The Color To Blue
      GL.Vertex3(-1.0f * Scale, 1.0f * Scale, 1.0f * Scale);        // Top Right Of The Quad (Left)
      GL.Vertex3(-1.0f * Scale, 1.0f * Scale, -1.0f * Scale);       // Top Left Of The Quad (Left)
      GL.Vertex3(-1.0f * Scale, -1.0f * Scale, -1.0f * Scale);      // Bottom Left Of The Quad (Left)
      GL.Vertex3(-1.0f * Scale, -1.0f * Scale, 1.0f * Scale);       // Bottom Right Of The Quad (Left)

      GL.Color4(1.0f, 1.0f, 1.0f, Alpha);        // Set The Color To Violet
      GL.Vertex3(1.0f * Scale, 1.0f * Scale, -1.0f * Scale);        // Top Right Of The Quad (Right)
      GL.Vertex3(1.0f * Scale, 1.0f * Scale, 1.0f * Scale);         // Top Left Of The Quad (Right)
      GL.Vertex3(1.0f * Scale, -1.0f * Scale, 1.0f * Scale);        // Bottom Left Of The Quad (Right)
      GL.Vertex3(1.0f * Scale, -1.0f * Scale, -1.0f * Scale);       // Bottom Right Of The Quad (Right)

      GL.End();

      return 12;
    }
  }
  public class Vertex
  {
    public Vector3 Position;
    public float S, T;
    public float R, G, B;
    public float Nx, Ny, Nz;
    public unsafe void Fill (ref float* ptr)
    {
      *ptr++ = S; *ptr++ = T;
      *ptr++ = R; *ptr++ = G; *ptr++ = B;
      *ptr++ = Nx; *ptr++ = Ny; *ptr++ = Nz;
      *ptr++ = Position.X; *ptr++ = Position.Y; *ptr++ = Position.Z;
    }

    public void Update(Vector3 diff)
    {
      Position += diff;
    }
  }

  public enum SphereCurrentType
  {
    BoringMarble,
    MeteorMarbleAtmosphere,
    MeteorMarble
  }

  public class SphereObject
  {
    //Data
    public SphereCurrentType Type = SphereCurrentType.BoringMarble;
    public float mass = 0.2f;
    public float bouncyness = 0.2f;
    public Vector3 position;
    public Vector3 velocity;

    //Graphics
    public Vertex[,] vertices;
    public float radius;
    public static int sectorCount = 36;
    public static int stackCount = 18;
    public readonly Color color;
    private float sectorStep, stackStep, lengthInv;
    public static int TriangleCount => sectorCount * stackCount * 2;

    public void UpdatePosition(Vector3 position)
    {
      var diffPos = position - this.position;

      Update(diffPos);

      this.position = position;
    }
    public void UpdateVelocity(Vector3 velocity)
    {
      this.velocity = velocity;
    }

    //data.radii[i]
    public SphereObject (float radius, Color color, Vector3 position, Vector3 velocity)
    {
      this.vertices = new Vertex[stackCount + 1, sectorCount + 1];
      // Create points
      this.radius = radius;
      this.color = color;
      this.position = position;
      this.velocity = velocity;
      this.sectorStep = 2 * (float)Math.PI / sectorCount;
      this.stackStep = (float)Math.PI / stackCount;
      this.lengthInv = 1.0f / radius;
    }

    private void Update(Vector3 diff)
    {
      for (int i = 0; i < vertices.GetLength(0); ++i)
      {
        for (int j = 0; j < vertices.GetLength(1); ++j)
        {
          vertices[i, j].Update(diff);
        }
      }
    }

    public void Create()
    {
      float x, y, z, xy;   // vertex position
      float nx, ny, nz;    // vertex normal
      float s, t;          // vertex texCoord
      float sectorAngle, stackAngle;
      for (int stack = 0; stack <= stackCount; stack++)
      {
        stackAngle = (float)Math.PI / 2 - stack * stackStep;        // starting from pi/2 to -pi/2
        xy = radius * (float)Math.Cos(stackAngle);         // r * cos(u)
        z = radius * (float)Math.Sin(stackAngle);          // r * sin(u)

        for (int sector = 0; sector <= sectorCount; sector++)
        {
          sectorAngle = sector * sectorStep;
          x = xy * (float)Math.Cos(sectorAngle);
          y = xy * (float)Math.Sin(sectorAngle);

          nx = x * lengthInv;
          ny = y * lengthInv;
          nz = z * lengthInv;

          s = (float)sector / sectorCount;
          t = (float)stack / stackCount;

          float r = color.R / 255.0f;
          float g = color.G / 255.0f;
          float b = color.B / 255.0f;

          // txt[2], color[3], normal[3], position[3]
          vertices[stack, sector] = new Vertex
          {
            Position = new Vector3(x + position.X, y + position.Y, z + position.Z),
            S = s,
            T = t,
            R = r,
            G = g,
            B = b,
            Nx = nx,
            Ny = ny,
            Nz = nz
          };
        }
      }
    }

    public unsafe void Render(ref float* ptr)
    {
      for (int sector = 0; sector < sectorCount; sector++)
      {
        for (int stack = 0; stack < stackCount; stack++)
        {
          vertices[stack, sector].Fill(ref ptr);
          vertices[stack + 1, sector].Fill(ref ptr);
          vertices[stack, sector + 1].Fill(ref ptr);

          vertices[stack, sector + 1].Fill(ref ptr);
          vertices[stack + 1, sector].Fill(ref ptr);
          vertices[stack + 1, sector + 1].Fill(ref ptr);
        }
      }
    }
  }
  
  public partial class Form1
  {
    /// <summary>
    /// Form-data initialization.
    /// </summary>
    public static void InitParams (out string name, out string param, out string tooltip,
                                    out Vector3 center, out float diameter,
                                    out bool useTexture, out bool globalColor, out bool useNormals, out bool useWireframe, out bool useMT)
    {
      name = "Miroslav Valach";
      param = "n=150, mb=0.8, mm=0.2, fb=0.2, fm=20.0, fs=2.0";
      tooltip = "n = <number-of-balls>, mb = <marble bouncy (0 - 1)>, mm = <marble mass>, fb = <meteor marble bouncy (0 - 1)>, fm = <meteor marble mass>, fs = <meteor size multiplier (ideally 0.5 - 3.0)>";

      // Scene dimensions.
      center = Vector3.Zero;
      diameter = 30.0f;

      // GUI.
      useTexture = false;
      globalColor = false;
      useNormals = false;
      useWireframe = false;
      useMT = true;
    }

    /// <summary>
    /// Simulation instance, initialized in glControl1_Load()
    /// </summary>
    MarblesWorld world = null;
    /// <summary>
    /// Renderer instance, initialized in glControl1_Load()
    /// </summary>
    MarblesRenderer renderer = null;
    /// <summary>
    /// Global instance of data for rendering.
    /// <code>renderer</code> instance must be locked for manipulation with this reference.
    /// </summary>
    MarblesRenderData data = null;
    // FPS-related stuff:
    long lastFpsTime = 0L;
    int frameCounter = 0;
    volatile int simCounter = 0;
    long primitiveCounter = 0L;
    double lastFps = 0.0;
    double lastSps = 0.0;
    double lastTps = 0.0;
    /// <summary>
    /// Simulation time of the last checkpoint in system ticks (100ns units)
    /// </summary>
    long ticksLast = DateTime.Now.Ticks;
    /// <summary>
    /// Simulation time of the last checkpoint in seconds.
    /// </summary>
    double timeLast = 0.0;

    /// <summary>
    /// Function called whenever the main application is idle..
    /// </summary>
    private void Application_Idle (object sender, EventArgs e)
    {
      while (glControl1.IsIdle)
      {
        glControl1.MakeCurrent();

        // World simulation (sets the 'data' object):
        if (simThread == null)
          Simulate();

        // Rendering (from the 'data' object);
        Render(true);

        long now = DateTime.Now.Ticks;
        long newTicks = now - lastFpsTime;
        if (newTicks > 5000000)      // more than 0.5 sec
        {
          lastFps = 0.5 * lastFps + 0.5 * (frameCounter * 1.0e7 / newTicks);
          lastSps = 0.5 * lastSps + 0.5 * (simCounter * 1.0e7 / newTicks);
          lastTps = 0.5 * lastTps + 0.5 * (primitiveCounter * 1.0e7 / newTicks);
          lastFpsTime = now;
          frameCounter = 0;
          simCounter = 0;
          primitiveCounter = 0L;

          if (lastTps < 5.0e5)
            labelFps.Text = string.Format(CultureInfo.InvariantCulture, "Fps: {0:f1}, Sps: {1:f1}, Tps: {2:f0}k",
                                           lastFps, lastSps, (lastTps * 1.0e-3));
          else
            labelFps.Text = string.Format(CultureInfo.InvariantCulture, "Fps: {0:f1}, Sps: {1:f1}, Tps: {2:f1}m",
                                           lastFps, lastSps, (lastTps * 1.0e-6));

          if (world != null)
            labelStat.Text = string.Format(CultureInfo.InvariantCulture, "T: {0:f1}s, fr: {1}{3}{4}, mrbl: {2}",
                                            world.Time, world.Frames, world.Marbles,
                                            (screencast != null) ? (" (" + screencast.Queue + ')') : "",
                                            (simThread == null) ? "" : " mt");
        }
      }
    }

    /// <summary>
    /// Prime simulation init.
    /// </summary>
    private void InitSimulation (string param)
    {
      world = new MarblesWorld(param);
      renderer = new MarblesRenderer(param);
      data = null;

      ResetSimulation(param);
    }

    /// <summary>
    /// [Re-]initialize the simulation.
    /// </summary>
    private void ResetSimulation (string param)
    {
      Snapshots.ResetFrameNumber();

      if (world != null)
        lock (world)
        {
          // ResetDataBuffers();
          world.Reset(param);
          ticksLast = DateTime.Now.Ticks;
          timeLast = 0.0;
        }
    }

    /// <summary>
    /// Pause / restart simulation.
    /// </summary>
    private void PauseRestartSimulation ()
    {
      if (world != null)
        lock (world)
          world.Running = !world.Running;
    }

    /// <summary>
    /// Update Simulation parameters.
    /// </summary>
    private void UpdateSimulation (string param)
    {
      if (world != null)
        lock (world)
          world.Update(param);
    }

    /// <summary>
    /// Simulate one frame.
    /// </summary>
    private void Simulate ()
    {
      /// <summary>New data to draw (or <code>null</code> if there is nothing to update).</summary>
      MarblesRenderData newData = null;

      if (world != null)
        lock (world)
        {
          long nowTicks = DateTime.Now.Ticks;
          if (nowTicks > ticksLast)
          {
            if (world.Running)
            {
              // 1000Hz .. 1ms .. 10000 ticks
              long minTicks = 10000000L / world.maxSpeed;       // min ticks between simulation steps
              if (nowTicks - ticksLast < (3 * minTicks) / 4)
              {
                // we are going too fast..
                int sleepMs = (int)(((5 * minTicks) / 4 - nowTicks + ticksLast) / 10000L);
                Thread.Sleep(sleepMs);
                nowTicks = DateTime.Now.Ticks;
              }
              double timeScale = checkSlow.Checked ? MarblesWorld.slow : 1.0;
              timeLast += (nowTicks - ticksLast) * timeScale * 1.0e-7;
              newData = world.Simulate(timeLast);
              simCounter++;
            }
            ticksLast = nowTicks;
          }
        }

      if (newData != null &&
           renderer != null)
        lock (renderer)
          data = newData;
    }

    /// <summary>
    /// Rendering of one frame, all the stuff.
    /// </summary>
    private void Render (bool snapshot = false)
    {
      if (!loaded)
        return;

      frameCounter++;
      OGL.useShaders = OGL.canShaders &&
                       OGL.activeProgram != null;

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      GL.PolygonMode(MaterialFace.FrontAndBack, checkWireframe.Checked ? PolygonMode.Line : PolygonMode.Fill);

      // Current camera:
      tb.GLsetCamera();

      // Scene rendering:
      RenderScene();

      if (snapshot &&
           screencast != null &&
           world != null &&
           world.Running)
        screencast.SaveScreenshotAsync(glControl1);

      glControl1.SwapBuffers();
    }

    /// <summary>
    /// OpenGL rendering code itself (separated for clarity).
    /// </summary>
    private void RenderScene ()
    {
      // Simulation scene rendering.
      MarblesRenderData rdata = null;
      if (renderer != null &&
           OGL.useShaders)
        lock (renderer)
          rdata = data;

      if (rdata != null)
      {
        OGL.FormData();

        // Simulated scene.
        primitiveCounter += renderer.Render(OGL, rdata);

        // Light source.
        GL.PointSize(5.0f);
        GL.Begin(PrimitiveType.Points);
        GL.Color3(1.0f, 1.0f, 1.0f);
        GL.Vertex3(OGL.lightPosition);
        GL.End();

        primitiveCounter++;

        primitiveCounter += rdata.Cube.Draw();
      }
      else
      {
        CubeObject cube = new CubeObject(10);
        primitiveCounter += cube.Draw();
      }
    }

    #region IOEvents
    private void glControl1_MouseDown (object sender, MouseEventArgs e)
    {
      if (world == null ||
           !world.MouseButtonDown(e))
        tb.MouseDown(e);
    }
    private void glControl1_MouseUp (object sender, MouseEventArgs e)
    {
      if (world == null ||
           !world.MouseButtonUp(e))
        tb.MouseUp(e);
    }
    private void glControl1_MouseMove (object sender, MouseEventArgs e)
    {
      if (world == null ||
           !world.MousePointerMove(e))
        tb.MouseMove(e);
    }
    private void glControl1_MouseWheel (object sender, MouseEventArgs e)
    {
      if (world == null ||
           !world.MouseWheelChange(e))
        tb.MouseWheel(e);
    }
    private void glControl1_KeyDown (object sender, KeyEventArgs e)
    {
      tb.KeyDown(e);
    }
    private void glControl1_KeyUp (object sender, KeyEventArgs e)
    {
      // Simulation has the priority.
      if (world == null ||
           !world.KeyHandle(e))
        tb.KeyUp(e);
    }
    #endregion
  }


  /// <summary>
  /// Renderer: can interpret MarblesRenderData and converts them into
  /// OpenGL commands..
  /// Locked for manipulation with Form1.data pointer.
  /// </summary>
  public class MarblesRenderer
  {
    public MarblesRenderer (string param) { }

    /// <summary>
    /// Number of marbles (triangles) VBOs are allocated for..
    /// </summary>
    int lastMarbles = -1;

    void RenderInit (OpenglState OGL)
    {
      // Scene rendering from VBOs.
      OGL.SetVertexAttrib(true);

      // Using GLSL shaders.
      GL.UseProgram(OGL.activeProgram.Id);

      // Uniforms.

      // Camera, projection, ..
      Matrix4 modelView    = OGL.GetModelView();
      Matrix4 modelViewInv = OGL.GetModelViewInv();
      Matrix4 projection   = OGL.GetProjection();
      Vector3 eye          = OGL.GetEyePosition();

      // Give matrices to shaders.
      GL.UniformMatrix4(OGL.activeProgram.GetUniform("matrixModelView"), false, ref modelView);
      GL.UniformMatrix4(OGL.activeProgram.GetUniform("matrixProjection"), false, ref projection);

      // Lighting constants.
      GL.Uniform3(OGL.activeProgram.GetUniform("globalAmbient"), ref OGL.globalAmbient);
      GL.Uniform3(OGL.activeProgram.GetUniform("lightColor"), ref OGL.whiteLight);
      GL.Uniform3(OGL.activeProgram.GetUniform("lightPosition"), ref OGL.lightPosition);
      GL.Uniform3(OGL.activeProgram.GetUniform("eyePosition"), ref eye);
      GL.Uniform3(OGL.activeProgram.GetUniform("Ka"), ref OGL.matAmbient);
      GL.Uniform3(OGL.activeProgram.GetUniform("Kd"), ref OGL.matDiffuse);
      GL.Uniform3(OGL.activeProgram.GetUniform("Ks"), ref OGL.matSpecular);
      GL.Uniform1(OGL.activeProgram.GetUniform("shininess"), OGL.matShininess);

      // Global color handling.
      bool useColors = !OGL.useGlobalColor;
      GL.Uniform1(OGL.activeProgram.GetUniform("globalColor"), useColors ? 0 : 1);

      // Use varying normals?
      bool useNormals = OGL.useNormals;
      GL.Uniform1(OGL.activeProgram.GetUniform("useNormal"), useNormals ? 1 : 0);
      GlInfo.LogError("set-uniforms");

      // Texture handling.
      bool useTexture = OGL.useTexture;
      GL.Uniform1(OGL.activeProgram.GetUniform("useTexture"), useTexture ? 1 : 0);
      GL.Uniform1(OGL.activeProgram.GetUniform("texSurface"), 0);
      if (useTexture)
      {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, OGL.texName);
      }
      GlInfo.LogError("set-texture");

      // [txt] [colors] [normals] vertices
      GL.BindBuffer(BufferTarget.ArrayBuffer, OGL.VBOid[0]);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, OGL.VBOid[1]);
    }

    /// <summary>
    /// Renders the simulated data from the special object.
    /// </summary>
    /// <param name="OGL">Current OpenGL state object.</param>
    /// <param name="data">Data to render.</param>
    /// <returns>Number of actually drawn primitives.</returns>
    public long Render (OpenglState OGL, MarblesRenderData data)
    {
      RenderInit(OGL);

      int marbles = data.Spheres.Count;
      int stride = sizeof( float ) * 11;   // float[2] txt, float[3] color, float[3] normal, float[3] position
      int triangles = SphereObject.TriangleCount * marbles;
      int vertexCount = triangles * 3;

      if (marbles != lastMarbles)
      {
        RenderInitializeBuffer(OGL, vertexCount, stride, triangles);

        lastMarbles = marbles;
      }

      // Refill vertex buffer.
      GL.BindBuffer(BufferTarget.ArrayBuffer, OGL.VBOid[0]);
      IntPtr videoMemoryPtr = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);

      unsafe
      {
        float* ptr = (float*)videoMemoryPtr.ToPointer();
        for (int i = 0; i < marbles; i++)
        {
          data.Spheres[i].Render(ref ptr);
        }
      }

      RenderFinish(OGL, vertexCount, stride);
      return triangles;
    }

    private void RenderInitializeBuffer(OpenglState OGL, int vertexCount, int stride, int triangles)
    {
      int vertexBufferSize = vertexCount * stride;
      // Relocate the buffers.

      // Vertex array: [ txt color coord ]
      GL.BindBuffer(BufferTarget.ArrayBuffer, OGL.VBOid[0]);
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)vertexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      // Fill index array.
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, OGL.VBOid[1]);
      GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)((triangles * 3) * sizeof(uint)), IntPtr.Zero, BufferUsageHint.StaticDraw);
      IntPtr videoMemoryPtr = GL.MapBuffer(BufferTarget.ElementArrayBuffer, BufferAccess.WriteOnly);
      unsafe
      {
        uint* ptr = (uint*)videoMemoryPtr.ToPointer();
        for (uint i = 0; i < triangles * 3; i++)
          *ptr++ = i;
      }
      GL.UnmapBuffer(BufferTarget.ElementArrayBuffer);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    private void RenderFinish(OpenglState OGL, int vertexCount, int stride)
    {
      GL.UnmapBuffer(BufferTarget.ArrayBuffer);

      // Index buffer.
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, OGL.VBOid[1]);

      // Set attribute pointers.
      IntPtr p = IntPtr.Zero;
      if (OGL.activeProgram.HasAttribute("texCoords"))
        GL.VertexAttribPointer(OGL.activeProgram.GetAttribute("texCoords"), 2, VertexAttribPointerType.Float, false, stride, p);
      p += Vector2.SizeInBytes;

      if (OGL.activeProgram.HasAttribute("color"))
        GL.VertexAttribPointer(OGL.activeProgram.GetAttribute("color"), 3, VertexAttribPointerType.Float, false, stride, p);
      p += Vector3.SizeInBytes;

      if (OGL.activeProgram.HasAttribute("normal"))
        GL.VertexAttribPointer(OGL.activeProgram.GetAttribute("normal"), 3, VertexAttribPointerType.Float, false, stride, p);
      p += Vector3.SizeInBytes;

      GL.VertexAttribPointer(OGL.activeProgram.GetAttribute("position"), 3, VertexAttribPointerType.Float, false, stride, p);
      GlInfo.LogError("triangles-set-attrib-pointers");

      // The drawing command itself.
      GL.DrawElements(PrimitiveType.Triangles, vertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
      GlInfo.LogError("triangles-draw-elements");

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
      GL.UseProgram(0);

      OGL.SetVertexAttrib(false);
    }
  }

  /// <summary>
  /// Simulation class. Holds internal state of the virtual world,
  /// generated rendering data.
  /// Doesn't know anything about OpenGL.
  /// </summary>
  public class MarblesWorld
  {
    #region Data
    public CubeObject Walls = new CubeObject(10);
    public Vector3 Gravity = new Vector3(0,-1,0) * 10;
    public float Speed = 1f;
    /// <summary>
    /// All our marbles.
    /// </summary>
    List<SphereObject> MyMarbles = new List<SphereObject>();
    /// <summary>
    /// Required number of marbles.
    /// </summary>
    int marbles;
    /// <summary>
    /// Actual number of active marbles.
    /// </summary>
    public int Marbles => MyMarbles.Count;
    /// <summary>
    /// Lock-protected simulation state.
    /// Pause-related stuff could be stored/handled elsewhere.
    /// </summary>
    public bool Running { get; set; }
    /// <summary>
    /// Number of simulated frames so far.
    /// </summary>
    public int Frames { get; private set; }
    /// <summary>
    /// Current sim-world time.
    /// </summary>
    public double Time { get; private set; }
    /// <summary>
    /// Maximum simulation speed in sims per second.
    /// </summary>
    public int maxSpeed = 1000;
    /// <summary>
    /// Random generator instance.
    /// </summary>
    RandomJames rnd;
    /// <summary>
    /// Slow motion coefficient.
    /// </summary>
    public static double slow = 0.25;
    #endregion

    public MarblesWorld (string param)
    {
      rnd = new RandomJames(144);
      MyMarbles = new List<SphereObject>();

      Frames = 0;
      Time = 0.0;
      Running = false;
    }

    /// <summary>
    /// [Re-]initialization of the world.
    /// </summary>
    /// <param name="param">String parameter from user, e.g.: number of marbles.</param>
    public void Reset (string param)
    {
      Running = false;

      // Param parsing.
      Update(param);

      rnd.Reset(144);
      MyMarbles.Clear();

      UpdateMarbles();

      Frames = 0;
      Time = 0.0;
      Running = true;
    }

    /// <summary>
    /// Update number of simulated marbles, 'Marbles' is requested to be equal to 'marbles'.
    /// </summary>
    void UpdateMarbles ()
    {
      if (marbles < Marbles)
      {
        // Truncate the arrays.
        int remove = Marbles - marbles;
        MyMarbles.RemoveRange(marbles, remove);
        return;
      }

      // Extend the arrays.
      for (int i = Marbles; i < marbles; i++)
      {
        var radius = rnd.RandomFloat(0.4f, 1.0f);
        var color = Color.FromArgb(rnd.RandomInteger(0, 100),
                                    rnd.RandomInteger(0, 255),
                                    rnd.RandomInteger(0, 255));
        var position = new Vector3(rnd.RandomFloat(-10.0f, 10.0f),
                                  rnd.RandomFloat(-10.0f, 10.0f),
                                  rnd.RandomFloat(-10.0f, 10.0f));
        var velocity = Speed * new Vector3(rnd.RandomFloat(-2.0f, 2.0f),
                                     rnd.RandomFloat(-2.0f, 2.0f),
                                     rnd.RandomFloat(-2.0f, 2.0f));


        var marble = new SphereObject(radius, color, position, velocity)
        {
          bouncyness =  initBouncy,
          mass = initMass
        };
        marble.Create();
        MyMarbles.Add(marble);
      }
    }

    private float meteorSizeMulti = 1f;
    private float initBouncy = 0.2f;
    private float initMass = 0.2f;
    private float initMeteorBouncy = 0.8f;
    private float initMeteorMass = 20.0f;

    /// <summary>
    /// Update simulation parameters.
    /// </summary>
    /// <param name="param">User-provided parameter string.</param>
    public void Update (string param)
    {
      // Input params.
      Dictionary<string, string> p = Util.ParseKeyValueList( param );
      if (p.Count == 0)
        return;


      if (Util.TryParse(p, "mb", ref initBouncy))
      {
        initBouncy = 1 - initBouncy;
      }
      if (!Util.TryParse(p, "mm", ref initMass))
      {

      }
      if (!Util.TryParse(p, "fb", ref initMeteorBouncy))
      {
        initMeteorBouncy = 1 - initMeteorBouncy;
      }
      if (!Util.TryParse(p, "fm", ref initMeteorMass))
      {

      }
      if (!Util.TryParse(p, "fs", ref meteorSizeMulti))
      {

      }

        // Simulation: number of marbles.
        if (Util.TryParse(p, "n", ref marbles))
      {
        marbles = Arith.Clamp(marbles, 1, 10000);

        if (Running)
          UpdateMarbles();
      }

      // Simulation: maximum simulation speed (simulations per second).
      if (Util.TryParse(p, "speed", ref maxSpeed))
      {
        maxSpeed = Arith.Clamp(maxSpeed, 5, 1000);
      }

      // Global: screencast.
      bool recent = false;
      if (Util.TryParse(p, "screencast", ref recent) &&
           (Form1.screencast != null) != recent)
        Form1.StartStopScreencast(recent);

      // {{ TODO: more parameter-parsing?

      // }}
    }

    /// <summary>
    /// Do one step of simulation.
    /// </summary>
    /// <param name="time">Required target time in seconds.</param>
    /// <returns>Data for rendering or <code>null</code> if nothing has changed.</returns>
    public MarblesRenderData Simulate (double time)
    {
      if (!Running)
        return null;

      if (time > Time)
      {
        Frames++;
        float dTime = (float)( time - Time );

        // Simulate (update) the world.
        // Update position
        for (int i = 0; i < MyMarbles.Count; i++)
        {
          Vector3 vel = MyMarbles[i].velocity;
          Vector3 pos = MyMarbles[i].position + vel * dTime;
          MyMarbles[i].UpdatePosition(pos);
          MyMarbles[i].UpdateVelocity(vel + Gravity * dTime * MyMarbles[i].mass);
        }
        // Detect colisions
        for (int i = 0; i < MyMarbles.Count; i++)
        {
          // Collision detection
          var first = MyMarbles[i];
          if (first.Type == SphereCurrentType.MeteorMarbleAtmosphere)
            continue;
          for (int j = i + 1; j < MyMarbles.Count; j++)
          {
            var second = MyMarbles[j];
            if (second.Type == SphereCurrentType.MeteorMarbleAtmosphere)
              continue;

            var normal = (first.position - second.position);
            float diff = normal.Length;

            // Resolve collision
            if (diff <= first.radius + second.radius)
            {
              var tm = 1 - diff / (first.radius + second.radius);
              first.UpdatePosition(first.position + normal * tm);
              normal.Normalize();

              Vector3 relativeVelocity = second.velocity - first.velocity;
              float velocityAlongNormal = Vector3.Dot(relativeVelocity, normal);
              //if (velocityAlongNormal > 0)
              //  continue;

              float restitution = Math.Min(first.bouncyness, second.bouncyness);
              float impulse = -(1 + restitution) * velocityAlongNormal;
              impulse /= 1 / first.mass + 1 / second.mass;

              // Apply impulse
              Vector3 appliedImpulse = impulse * normal;
              var vel1 = first.velocity - 1 / first.mass * appliedImpulse;
              var vel2 = second.velocity + 1 / second.mass * appliedImpulse;

              first.UpdateVelocity(vel1);
              second.UpdateVelocity(vel2);
            }
          }
        }
        
        for (int i = 0; i < MyMarbles.Count; i++)
        {
          Vector3 pos = MyMarbles[i].position;
          Vector3 vel = MyMarbles[i].velocity;
          if (Walls.Check(ref pos, ref vel, MyMarbles[i].mass, MyMarbles[i].radius, ref MyMarbles[i].Type))
          {
            MyMarbles[i].UpdatePosition(pos);
            MyMarbles[i].UpdateVelocity(vel * Walls.wallBouncyness);
          }
        }
        Time = time;
      }

      // Return the current data in a new MarblesRenderData instance.
      return new MarblesRenderData(Walls, MyMarbles);
    }

    private void SpawnMeteor ()
    {
      var radius = rnd.RandomFloat(0.9f, 1.0f) * meteorSizeMulti;

      var xRange = 10 - radius;
      var zRange = 10 - radius;
      var yRange = 10 - radius;


      var color = Color.FromArgb(rnd.RandomInteger(200, 255), rnd.RandomInteger(0, 0), rnd.RandomInteger(0, 0));
      var xLocationStart = rnd.RandomFloat(-20.0f, 20.0f);
      var zLocationStart = rnd.RandomFloat(-20.0f, 20.0f);
      var yLocationStart = rnd.RandomFloat(30.0f, 60.0f);
      var position = new Vector3(xLocationStart, yLocationStart, zLocationStart);
      var target = new Vector3(rnd.RandomFloat(-xRange, xRange), -yRange, rnd.RandomFloat(-zRange, zRange));

      var velocity = (target - position) * 3;//Speed/100 * new Vector3(rnd.RandomFloat(-2.0f, -2.0f), rnd.RandomFloat(-2.0f, -2.0f), rnd.RandomFloat(-2.0f, -2.0f));
      var marble = new SphereObject(radius, color, position, velocity)
      {
        mass = initMeteorMass,
        bouncyness = initMeteorBouncy
      };
      marble.Type = SphereCurrentType.MeteorMarbleAtmosphere;
      marble.Create();
      MyMarbles.Add(marble);
    }

    #region IOEvents

    /// <summary>
    /// Handles mouse-button push.
    /// </summary>
    /// <returns>True if handled.</returns>
    public bool MouseButtonDown (MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        SpawnMeteor();
      }

      if (e.Button != MouseButtons.Right)
        return false;

      return false;
    }

    /// <summary>
    /// Handles mouse-button release.
    /// </summary>
    /// <returns>True if handled.</returns>
    public bool MouseButtonUp (MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return false;

      return false;
    }

    /// <summary>
    /// Handles mouse move.
    /// </summary>
    /// <returns>True if handled.</returns>
    public bool MousePointerMove (MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return false;

      return false;
    }

    /// <summary>
    /// Handles mouse wheel change.
    /// </summary>
    /// <returns>True if handled.</returns>
    public bool MouseWheelChange (MouseEventArgs e)
    {
      // Warning: if you handle this event, Trackball won't get it..

      return false;
    }

    /// <summary>
    /// Handles keyboard key release.
    /// </summary>
    /// <returns>True if handled.</returns>
    public bool KeyHandle (KeyEventArgs e)
    {
      return false;
    }
    #endregion
  }

  /// <summary>
  /// Data object containing all OpenGL-related values.
  /// </summary>
  public class OpenglState
  {
    /// <summary>
    /// Can we use shaders?
    /// </summary>
    public bool canShaders = false;

    /// <summary>
    /// Are we currently using shaders?
    /// </summary>
    public bool useShaders = false;

    /// <summary>
    /// Vertex array VBO (colors, normals, coords), index array VBO
    /// </summary>
    public uint[] VBOid = null;

    /// <summary>
    /// Global GLSL program repository.
    /// </summary>
    public Dictionary<string, GlProgramInfo> programs = new Dictionary<string, GlProgramInfo>();

    /// <summary>
    /// Current (active) GLSL program.
    /// </summary>
    public GlProgram activeProgram = null;

    /// <summary>
    /// Associated GLControl object.
    /// </summary>
    GLControl glC;

    /// <summary>
    /// Associated Form (camera handling) object.
    /// </summary>
    Form1 form;

    public bool useNormals     = false;
    public bool useTexture     = false;
    public bool useGlobalColor = false;

    /// <summary>
    /// Get data (rendering options) from the form.
    /// </summary>
    public void FormData ()
    {
      useNormals = form.checkNormals.Checked;
      useTexture = form.checkTexture.Checked;
      useGlobalColor = form.checkGlobalColor.Checked;
    }

    public OpenglState (Form1 f, GLControl glc)
    {
      form = f;
      glC = glc;
    }

    /// <summary>
    /// OpenGL init code.
    /// </summary>
    public void InitOpenGL ()
    {
      // log OpenGL info just for curiosity:
      GlInfo.LogGLProperties();

      // general OpenGL:
      glC.VSync = true;
      GL.ClearColor(Color.FromArgb(30, 40, 90));
      GL.Enable(EnableCap.DepthTest);
      GL.ShadeModel(ShadingModel.Flat);

      // VBO init:
      VBOid = new uint[2];           // one big buffer for vertex data, another buffer for tri/line indices
      GL.GenBuffers(2, VBOid);
      GlInfo.LogError("VBO init");

      // shaders:
      canShaders = SetupShaders();

      // texture:
      GenerateTexture();
    }

    bool SetupShaders ()
    {
      activeProgram = null;

      foreach (var programInfo in programs.Values)
        if (programInfo.Setup())
          activeProgram = programInfo.program;

      if (activeProgram == null)
        return false;

      GlProgramInfo defInfo;
      if (programs.TryGetValue("default", out defInfo) &&
           defInfo.program != null)
        activeProgram = defInfo.program;

      return true;
    }

    // Generated texture.
    const int TEX_SIZE = 128;
    const int TEX_CHECKER_SIZE = 8;
    static Vector3 colWhite = new Vector3( 0.85f, 0.75f, 0.15f );
    static Vector3 colBlack = new Vector3( 0.15f, 0.15f, 0.60f );
    static Vector3 colShade = new Vector3( 0.15f, 0.15f, 0.15f );

    /// <summary>
    /// Texture handle
    /// </summary>
    public int texName = 0;

    /// <summary>
    /// Generate the texture.
    /// </summary>
    public void GenerateTexture ()
    {
      GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
      texName = GL.GenTexture();
      GL.BindTexture(TextureTarget.Texture2D, texName);

      Vector3[] data = new Vector3[ TEX_SIZE * TEX_SIZE ];
      for (int y = 0; y < TEX_SIZE; y++)
        for (int x = 0; x < TEX_SIZE; x++)
        {
          int i = y * TEX_SIZE + x;
          bool odd = ((x / TEX_CHECKER_SIZE + y / TEX_CHECKER_SIZE) & 1) > 0;
          data[i] = odd ? colBlack : colWhite;
          // add some fancy shading on the edges:
          if ((x % TEX_CHECKER_SIZE) == 0 || (y % TEX_CHECKER_SIZE) == 0)
            data[i] += colShade;
          if (((x + 1) % TEX_CHECKER_SIZE) == 0 || ((y + 1) % TEX_CHECKER_SIZE) == 0)
            data[i] -= colShade;
        }

      GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, TEX_SIZE, TEX_SIZE, 0, PixelFormat.Rgb, PixelType.Float, data);

      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);

      GlInfo.LogError("create-texture");
    }

    public void DestroyTexture ()
    {
      if (texName != 0)
      {
        GL.DeleteTexture(texName);
        texName = 0;
      }
    }

    public static int Align (int address)
    {
      return ((address + 15) & -16);
    }

    // Appearance.
    public Vector3 globalAmbient = new Vector3(   0.2f,  0.2f,  0.2f );
    public Vector3 matAmbient    = new Vector3(   1.0f,  0.8f,  0.3f );
    public Vector3 matDiffuse    = new Vector3(   1.0f,  0.8f,  0.3f );
    public Vector3 matSpecular   = new Vector3(   0.8f,  0.8f,  0.8f );
    public float matShininess    = 100.0f;
    public Vector3 whiteLight    = new Vector3(   1.0f,  1.0f,  1.0f );
    public Vector3 lightPosition = new Vector3( -20.0f, 10.0f, 15.0f );
    public Vector3 eyePosition   = new Vector3(   0.0f,  0.0f, 10.0f );

    public void SetLightEye (float size)
    {
      size *= 0.4f;
      lightPosition = new Vector3(-2.0f * size, size, 1.5f * size);
      eyePosition = new Vector3(0.0f, 0.0f, size);
    }

    public Matrix4 GetModelView ()
    {
      return form.tb.ModelView;
    }

    public Matrix4 GetModelViewInv ()
    {
      return form.tb.ModelViewInv;
    }

    public Matrix4 GetProjection ()
    {
      return form.tb.Projection;
    }

    public Vector3 GetEyePosition ()
    {
      return form.tb.Eye;
    }

    // Attribute/vertex arrays.
    public void SetVertexAttrib (bool on)
    {
      if (activeProgram != null)
        if (on)
          activeProgram.EnableVertexAttribArrays();
        else
          activeProgram.DisableVertexAttribArrays();
    }

    public void InitShaderRepository ()
    {
      programs.Clear();
      GlProgramInfo pi;

      // Default program.
      pi = new GlProgramInfo("default", new GlShaderInfo[] {
        new GlShaderInfo( ShaderType.VertexShader,   "vertex.glsl",   "058marbles" ),
        new GlShaderInfo( ShaderType.FragmentShader, "fragment.glsl", "058marbles" ) });
      programs[pi.name] = pi;

      // Put more programs here.
      // pi = new GlProgramInfo( ..
      //   ..
      // programs[ pi.name ] = pi;
    }

    public void DestroyResources ()
    {
      DestroyTexture();

      activeProgram = null;
      programs.Clear();

      if (VBOid != null &&
           VBOid[0] != 0)
      {
        GL.DeleteBuffers(2, VBOid);
        VBOid = null;
      }
    }
  }
}
