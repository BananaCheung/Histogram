using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Histogram
{
   public class HistogramImage : PictureBox
   {
      private Bitmap histBitmap = new Bitmap( 256, 135 );

      public int CurrentIndex
      {
         get { return _currentIndex; }
      }



      private DrawChannel _currentMode;

      public HistogramImage() { InitialStyle(); }
      Point[] selectPoint = new Point[2];
      private byte[] _RgbData;
      private byte[] _rData;
      private byte[] _gData;
      private byte[] _bData;
      private int _currentIndex;

      void InitialStyle()
      {
         this.Size = new Size( 256, 135 );

         this.BackColor = Color.FromArgb( 90, 90, 90 );

         this.MouseDown += HistogramImage_MouseDown;
         this.MouseMove += HistogramImage_MouseMove;


      }

      private void HistogramImage_MouseDown( object sender, MouseEventArgs e )
      {
         if (e.Button == MouseButtons.Right)
         {
            selectPoint[0] = new Point( 0, 0 );
            selectPoint[1] = new Point( 0, 0 );
         }
         else if (e.Button == MouseButtons.Left)
         {
            selectPoint[0] = new Point( e.Location.X, 0 );
            selectPoint[1] = new Point( e.Location.X, this.Height );

            _currentIndex = e.X;

         }
         this.Invalidate();
      }

      private void HistogramImage_MouseMove( object sender, MouseEventArgs e )
      {
         if (e.Button != MouseButtons.Left) { return; }

         if (this.ClientRectangle.Contains( e.Location ))
         {
            selectPoint[0] = new Point( e.Location.X, 0 );
            selectPoint[1] = new Point( e.Location.X, this.Height );

            _currentIndex = e.X;
         }
         this.Invalidate();
      }

      public byte[] RgbData
      {
         set { _RgbData = value; }
         get { return _RgbData; }
      }


      //private void ScaledData( byte[] inRData, byte[] inGData, byte[] inBData )
      //{

      //   if (Height >= inData.Max( s => s ))
      //   {
      //      return inData;
      //   }



      //   int dataLength = inData.Length;
      //   double average = inData.Average( s => s );
      //   double scale = Height / average;

      //   IEnumerable<byte> scaledData =
      //      from d in inData
      //      select (byte)(d * scale);

      //   return scaledData.ToArray();
      //}



      #region another
      /*
      public void DrawHistogram( byte[] data, DrawChannel mode )
      {
         byte[] inputData = ScaledData( data );

         Rectangle rect = new Rectangle( 0, 0, histBitmap.Width, histBitmap.Height );
         BitmapData bmpData = histBitmap.LockBits( rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb );

         IntPtr ptr = bmpData.Scan0;

         int bytes = Math.Abs( bmpData.Stride ) * histBitmap.Height;
         byte[] rgbValues = new byte[bytes];

         if (mode == DrawChannel.Rgb)
         {
            int idx = 0;
            for (int x = 0; x < histBitmap.Width; x++)
            {
               for (int y = 0; y < histBitmap.Height; y++)
               {
                  // B value
                  rgbValues[idx] = y <= inputData[x] ? (byte)255 : (byte)0;
                  // G value
                  rgbValues[idx++] = y <= inputData[x + 1] ? (byte)255 : (byte)0;
                  // R value
                  rgbValues[idx++] = y <= inputData[x + 2] ? (byte)255 : (byte)0;
               }
            }
         }
         else
         {
            int idx = 0;
            for (int x = 0; x < histBitmap.Width; x++)
            {
               for (int y = 0; y < histBitmap.Height; y++)
               {
                  // B value
                  rgbValues[idx] = y <= inputData[x] ? (byte)255 : (byte)0;
                  // G value
                  rgbValues[idx++] = y <= inputData[x] ? (byte)255 : (byte)0;
                  // R value
                  rgbValues[idx++] = y <= inputData[x] ? (byte)255 : (byte)0;
               }
            }
         }

         Marshal.Copy( rgbValues, 0, ptr, bytes );

         histBitmap.UnlockBits( bmpData );
      }
      */
      #endregion

      public void DrawHistogram( Bitmap imgSource, DrawChannel mode )
      {
         if (imgSource == null) { return; }

         // 源图
         Rectangle imgRect = new Rectangle( 0, 0, imgSource.Width, imgSource.Height );
         BitmapData imgBitmapData = imgSource.LockBits( imgRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb );
         IntPtr imgPtr = imgBitmapData.Scan0;
         int imgBytes = Math.Abs( imgBitmapData.Stride ) * imgSource.Height;
         byte[] imgRgbValue = new byte[imgBytes];
         Marshal.Copy( imgPtr, imgRgbValue, 0, imgBytes );
         imgSource.UnlockBits( imgBitmapData );


         using (StreamWriter sw = File.CreateText( "abc.txt" ))
         {
            for (int i = 0; i < imgRgbValue.Length; i += 3)
            {
               sw.Write( imgRgbValue[i + 2] + "," );
               sw.Write( imgRgbValue[i + 1] + "," );
               sw.Write( imgRgbValue[i] + "," );
               sw.WriteLine();
            }
         }


         // 直方图
         BitmapData histBitmapData = histBitmap.LockBits( new Rectangle( 0, 0, histBitmap.Width, histBitmap.Height ),
            ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb );
         IntPtr histPtr = histBitmapData.Scan0;
         int histBytes = Math.Abs( histBitmapData.Stride ) * histBitmap.Height;
         byte[] histRgbValue = new byte[histBytes];

         byte[] rgbcounts = new byte[256];
         for (int index = 0; index < imgRgbValue.Length; index += 3)
         {
            byte t = imgRgbValue[index];
            rgbcounts[t]++;
         }


         for (int i = 0; i < histRgbValue.Length; i += 3)
         {
            int currentLine = i / Math.Abs( histBitmapData.Stride );
            int xPixel = i / (currentLine + 1) % 3;

            byte current = rgbcounts[xPixel];

            // B value
            histRgbValue[i + 0] = currentLine >= current ? (byte)255 : (byte)0;

            // G value                          
            //histRgbValue[i + 1] = currentLine >= current ? (byte)255 : (byte)0;

            //// R value                          
            //histRgbValue[i + 2] = currentLine >= current ? (byte)255 : (byte)0;
         }



         Marshal.Copy( histRgbValue, 0, histPtr, histBytes );

         histBitmap.UnlockBits( histBitmapData );

         Invalidate();
      }


      protected override void OnPaint( PaintEventArgs e )
      {
         base.OnPaint( e );

         e.Graphics.DrawImage( histBitmap, new Point( 0, 0 ) );

         int width = this.Width / 4;
         int height = this.Height;
         Pen pen = new Pen( Color.LightGray, 0.1f );
         pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
         e.Graphics.DrawLine( pen, width + 1, 1, width + 1, height + 1 );
         e.Graphics.DrawLine( pen, width + 1 * 2, 1, width * 2 + 1, height + 1 );
         e.Graphics.DrawLine( pen, width + 1 * 3, 1, width * 3 + 1, height + 1 );

         pen = new Pen( Color.White );
         pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
         e.Graphics.DrawLine( pen, selectPoint[0], selectPoint[1] );
      }


   }
}
