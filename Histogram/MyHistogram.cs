using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Histogram
{
   public partial class HistogramWin : UserControl
   {
      Point[] selectPoint = new Point[2];
      HistogramImage hist = new HistogramImage();
      private int _currentIndex;
      private int[] _rData = new int[256];
      private int[] _gData = new int[256];
      private int[] _bData = new int[256];
      private Rectangle drawArea = new Rectangle( 3, 3, 256, 135 );
      private Bitmap _image;
      private int _maxValue = 0;
      private int _minValue = 0;

      public HistogramWin()
      {
         InitializeComponent();
      }

      public HistogramWin( ref Bitmap bmp )
      {
         _image = bmp;
         //DrawChannel = channel;
         //_pixelFormat = pf;

         InitializeComponent();
         BitmapToBytes();
      }

      /// <summary>
      /// 将图片的RGB值转换为byte数组,并计算RGB各值的数量
      /// </summary>
      private void BitmapToBytes()
      {
         Rectangle rect = new Rectangle( 0, 0, _image.Width, _image.Height );
         BitmapData bmpData = _image.LockBits( rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb );
         IntPtr ptr = bmpData.Scan0;
         int bytes = bmpData.Stride * _image.Height;
         Data = new byte[bytes];
         Marshal.Copy( ptr, Data, 0, bytes );
         _image.UnlockBits( bmpData );

         for (int i = 0; i < bytes; i += 3)
         {
            int value = Data[i + 0];
            _bData[value]++;

            value = Data[i + 1];
            _gData[value]++;

            value = Data[i + 2];
            _rData[value]++;
         }
         RedSize = _rData.Length;
         GreenSize = _gData.Length;
         BlueSize = _bData.Length;

         int rMin = _rData.Min( d => d );
         int gMin = _gData.Min( d => d );
         int bMin = _bData.Min( d => d );
         int rMax = _rData.Max( d => d );
         int gMax = _gData.Max( d => d );
         int bMax = _bData.Max( d => d );

         _maxValue = Math.Max( Math.Max( rMax, gMax ), bMax );
         _minValue = Math.Min( Math.Min( rMin, gMin ), bMin );
      }


      public byte[] Data { set; get; }

      // 单击图表区域显示信息
      private void histogramPictureBox_MouseDown( object sender, MouseEventArgs e )
      {
         if (e.Button == MouseButtons.Left && drawArea.Contains( e.Location ))
         {
            ShowInfo( e );
            Invalidate();
         }
      }

      // 在图表区域移动并显示信息
      private void histogramPictureBox_MouseMove( object sender, MouseEventArgs e )
      {
         if (e.Button == MouseButtons.Left && drawArea.Contains( e.Location ))
         {
            ShowInfo( e );
            Invalidate();
         }
      }

      // 双击图表区域取消显示信息
      private void HistogramWin_MouseDoubleClick( object sender, MouseEventArgs e )
      {
         if (drawArea.Contains( e.Location ) && e.Button == MouseButtons.Left)
         {
            selectPoint[0] = new Point( -10, 0 );
            selectPoint[1] = new Point( -10, 0 );

            lblCInfo.Visible = false;

            Invalidate();
         }
      }

      /// <summary>
      /// 显示当下选择点的直方图信息
      /// </summary>
      /// <param name="e">光标在直方图的位置</param>
      private void ShowInfo( MouseEventArgs e )
      {
         int offset = drawArea.X;
         selectPoint[0] = new Point( e.Location.X, offset );
         selectPoint[1] = new Point( e.Location.X, drawArea.Height + offset );

         _currentIndex = e.X - offset;

         string str =
            $"Loc:{_currentIndex}\n" +
            $"  R:{600}\n" +
            $"  G:{700}\n" +
            $"  B:{80}\t";

         int x = e.X < 128 + drawArea.X ? drawArea.Right - lblCInfo.Width : 2;

         lblCInfo.Location = new Point( x, drawArea.X );
         lblCInfo.Visible = true;
         lblCInfo.Text = str;
      }

      public DrawChannel DrawChannel { get; set; }

      // RGB各值大小
      public int BlueSize { get; private set; }
      public int GreenSize { get; private set; }
      public int RedSize { get; private set; }

      public Bitmap Image { set { _image = value; } }

      public int GraphieOffset { get; set; }

      public void UpdateHistogram( Graphics g )
      {
         int xOffset = drawArea.X;
         int yUp = drawArea.Top;
         int yDn = drawArea.Bottom;
         int height = drawArea.Height;
         for (int i = xOffset; i < 256 + xOffset; i++)
         {
            Point dnPoint = new Point( i, yDn );
            Point upPoint = new Point( i, yUp );
         }
      }


      protected override void OnPaint( PaintEventArgs e )
      {
         base.OnPaint( e );


         // 1. 画直方图
         // 1.1 画背景
         Brush brush = new SolidBrush( Color.FromArgb( 30, 30, 30 ) );
         e.Graphics.FillRectangle( brush, drawArea );
         // 1.2 画曲线
         UpdateHistogram( e.Graphics );


         // 2. 画格子
         int offset = drawArea.X;
         const float width = 256.0F / 4;
         const float height = 135.0F;
         Pen pen = new Pen( Color.LightGray, 0.1f ) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
         //e.Graphics.DrawLine( pen, width * 0 + offset, offset, width * 0 + offset, height + offset );
         e.Graphics.DrawLine( pen, width * 1 + offset, offset, width * 1 + offset, height + offset );
         e.Graphics.DrawLine( pen, width * 2 + offset, offset, width * 2 + offset, height + offset );
         e.Graphics.DrawLine( pen, width * 3 + offset, offset, width * 3 + offset, height + offset );
         //e.Graphics.DrawLine( pen, width * 4 + offset, offset, width * 4 + offset, height + offset );


         // 3. 画选择条
         Pen selPen = new Pen( Color.White ) { DashStyle = System.Drawing.Drawing2D.DashStyle.Solid };
         e.Graphics.DrawLine( selPen, selectPoint[0], selectPoint[1] );
      }




   }
}
