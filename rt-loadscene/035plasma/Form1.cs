﻿using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Scene3D;
using System.Globalization;
using System.Drawing.Imaging;

namespace _035plasma
{
  public partial class Form1 : Form
  {
    static readonly string rev = "$Rev$".Split( ' ' )[ 1 ];

    /// <summary>
    /// Global thread variable. Valid while the simulation is in progress..
    /// </summary>
    protected Thread aThread = null;

    /// <summary>
    /// Global simulation object. Simulation of one frame.
    /// </summary>
    protected Simulation sim = null;

    /// <summary>
    /// Break-variable. Has to be checked on regular basis.
    /// </summary>
    volatile protected bool cont = true;

    /// <summary>
    /// Simulation's horizontal size.
    /// </summary>
    protected int width;

    /// <summary>
    /// Simulation's vertical size.
    /// </summary>
    protected int height;

    /// <summary>
    /// Save individual frames?
    /// </summary>
    protected bool saveFrames;

    /// <summary>
    /// Just for fun..
    /// </summary>
    protected FpsMeter fps = new FpsMeter();

    delegate void SetImageCallback ( Bitmap newImage );

    protected void setImage ( Bitmap newImage )
    {
      Image old = pictureBox1.Image;
      pictureBox1.Image = newImage;
      pictureBox1.Invalidate();
      if ( old != null )
        old.Dispose();
    }

    protected void SetImage ( Bitmap newImage )
    {
      if ( pictureBox1.InvokeRequired )
      {
        SetImageCallback si = new SetImageCallback( SetImage );
        BeginInvoke( si, new object[] { newImage } );
      }
      else
        setImage( newImage );
    }

    delegate void SetTextCallback ( string text );

    protected void SetText ( string text )
    {
      if ( labelElapsed.InvokeRequired )
      {
        SetTextCallback st = new SetTextCallback( SetText );
        BeginInvoke( st, new object[] { text } );
      }
      else
        labelElapsed.Text = text;
    }

    delegate void StopSimulationCallback ();

    protected void StopSimulation ()
    {
      if ( sim == null || aThread == null ) return;

      if ( buttonStart.InvokeRequired )
      {
        StopSimulationCallback ea = new StopSimulationCallback( StopSimulation );
        BeginInvoke( ea );
      }
      else
      {
        // actually stop the simulation thread:
        cont = false;
        aThread.Join();
        aThread = null;

        // GUI stuff:
        buttonStart.Enabled = true;
        buttonReset.Enabled = true;
        buttonStop.Enabled = false;
      }
    }

    public Form1 ()
    {
      InitializeComponent();
      Text += " (rev: " + rev + ')';
    }

    public void Simulation ()
    {
      if ( sim == null ||
           sim.Width != width ||
           sim.Height != height )
        sim = new Simulation( width, height );

      fps.Start();
      float fp = 0.0f;

      while ( cont )
      {
        sim.Simulate();
        Bitmap frame = sim.Visualize();
        SetImage( frame );

        float newFp = fps.Frame();
        if ( sim.Frame % 32 == 0 ) fp = newFp;
        SetText( string.Format( CultureInfo.InvariantCulture, "Frame: {0} (FPS = {1:f1})",
                                sim.Frame, fp ) );

        if ( saveFrames )
        {
          string fileName = string.Format( "out{0:0000}.png", sim.Frame );
          using ( Bitmap bmp = (Bitmap)frame.Clone() )
          {
            bmp.Save( fileName, ImageFormat.Png );
          }
        }
      }

      fps.Stop();
    }

    private void buttonStart_Click ( object sender, EventArgs e )
    {
      if ( aThread != null ) return;

      buttonStart.Enabled = false;
      buttonReset.Enabled = false;
      buttonStop.Enabled  = true;
      cont = true;

      // simulation properties:
      width = (int)numericXres.Value;
      height = (int)numericYres.Value;
      saveFrames = checkAnim.Checked;

      aThread = new Thread( new ThreadStart( this.Simulation ) );
      aThread.Start();
    }

    private void buttonStop_Click ( object sender, EventArgs e )
    {
      StopSimulation();
    }

    private void buttonReset_Click ( object sender, EventArgs e )
    {
      if ( sim == null || aThread != null )
        return;

      sim.Reset();
      SetText( "Frame: 0 (FPS = 0.0)" );
    }

    private void Form1_FormClosing ( object sender, FormClosingEventArgs e )
    {
      StopSimulation();
    }

    private void pictureBox1_MouseMove ( object sender, MouseEventArgs e )
    {
      if ( sim != null && e.Button == MouseButtons.Left )
        if ( sim.MouseMove( e.Location ) && aThread == null )
          SetImage( sim.Visualize() );
    }

    private void pictureBox1_MouseDown ( object sender, MouseEventArgs e )
    {
      if ( sim != null && e.Button == MouseButtons.Left )
        if ( sim.MouseDown( e.Location ) && aThread == null )
          SetImage( sim.Visualize() );
    }

    private void pictureBox1_MouseUp ( object sender, MouseEventArgs e )
    {
      if ( sim != null && e.Button == MouseButtons.Left )
        if ( sim.MouseUp( e.Location ) && aThread == null )
          SetImage( sim.Visualize() );
    }
  }
}
