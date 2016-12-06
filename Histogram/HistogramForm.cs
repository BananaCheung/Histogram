using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Histogram
{
   public class HistogramForm : Form
   {
      private readonly int[] _bData = new int[256];
      private int _currentIndex;
      private readonly int[] _gData = new int[256];
      private Bitmap _image;
      private int _maxValue;
      private int _minValue;
      private double _average;

      private readonly int[] _rData = new int[256];
      private Rectangle drawArea = new Rectangle( 3, 3, 256, 135 );
      private HistogramImage hist = new HistogramImage();


      private Label lblCInfo;
      private Label lblSize;
      private Label lblTB;
      private Label lblTG;
      private Label lblTR;
      private System.ComponentModel.IContainer components;
      private readonly Point[] selectPoint = new Point[2];

      public HistogramForm()
      {
         InitialControl();
      }

      public HistogramForm( ref Bitmap image )
      {
         InitialControl();
         _image = image;
         BitmapToBytes();
      }


      public byte[] Data { set; get; }

      public DrawChannel DrawChannel { get; set; }

      // RGB各值大小
      public int BlueSize { get; private set; }
      public int GreenSize { get; private set; }
      public int RedSize { get; private set; }

      public Bitmap Image
      {
         set { _image = value; }
      }

      public int GraphieOffset { get; set; }


      private void InitialControl()
      {
         // 
         // lblSize
         // 
         lblSize = new Label();
         lblSize.Location = new Point( 2, 139 );
         lblSize.AutoSize = true;
         lblSize.Font = new Font( "Consolas", 9F );
         lblSize.ForeColor = Color.White;
         lblSize.Name = "lblSize";
         lblSize.Text = "Size:1400 x 6000";

         // 
         // lblTB
         // 
         lblTB = new Label();
         lblTB.Location = new Point( 161, 153 );
         lblTB.AutoSize = true;
         lblTB.Font = new Font( "Consolas", 9F );
         lblTB.ForeColor = Color.White;
         lblTB.Name = "lblTB";
         lblTB.Text = "B:5000000";

         // 
         // lblTG
         //  
         lblTG = new Label();
         lblTG.Location = new Point( 82, 153 );
         lblTG.AutoSize = true;
         lblTG.Font = new Font( "Consolas", 9F );
         lblTG.ForeColor = Color.White;
         lblTG.Name = "lblTG";
         lblTG.Text = "G:5000000";

         // 
         // lblTR
         // 
         lblTR = new Label();
         lblTR.Location = new Point( 3, 153 );
         lblTR.AutoSize = true;
         lblTR.Font = new Font( "Consolas", 9F );
         lblTR.ForeColor = Color.White;
         lblTR.Name = "lblTR";
         lblTR.Text = "R:5000000";

         // 
         // lblCInfo
         // 
         lblCInfo = new Label();
         lblCInfo.Location = new Point( 48, 24 );
         lblCInfo.AutoSize = true;
         lblCInfo.Font = new Font( "Consolas", 9F );
         lblCInfo.BackColor = Color.FromArgb( 125, 0, 0, 0 );
         lblCInfo.ForeColor = Color.White;
         lblCInfo.Name = "lblCInfo";

         // 
         // HistogramWin
         // 
         BackColor = Color.Black;
         //BorderStyle = BorderStyle.FixedSingle;
         Controls.Add( lblCInfo );
         Controls.Add( lblTB );
         Controls.Add( lblTG );
         Controls.Add( lblTR );
         Controls.Add( lblSize );
         DoubleBuffered = true;
         Name = "HistFrom";
         //AutoSize = true;
         Size = new Size( 280, 208 );
         //MaximumSize = new Size( 280, 208 );
         //MinimumSize = new Size( 280, 208 );
         SetStyle( ControlStyles.FixedHeight, false );
         MaximizeBox = false;
         Margin = new Padding( 3 );
         FormBorderStyle = FormBorderStyle.FixedSingle;
         MouseDoubleClick += HistogramWin_MouseDoubleClick;
         MouseDown += histogramPictureBox_MouseDown;
         MouseMove += histogramPictureBox_MouseMove;


      }

      /// <summary>
      ///    将图片的RGB值转换为byte数组,并计算RGB各值的数量
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
         _average = (_rData.Average( d => d ) + _gData.Average( d => d ) + _bData.Average( d => d )) /
         3;
      }

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
      ///    显示当下选择点的直方图信息
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
            $"  R:{_rData[_currentIndex]}\n" +
            $"  G:{_gData[_currentIndex]}\n" +
            $"  B:{_bData[_currentIndex]}\t";

         int x = e.X < 128 + drawArea.X ? drawArea.Right - lblCInfo.Width : 2;

         lblCInfo.Location = new Point( x, drawArea.X );
         lblCInfo.Visible = true;
         lblCInfo.Text = str;
      }

      public void UpdateHistogram( Graphics g )
      {
         Bitmap histBmp = new Bitmap( drawArea.Width, drawArea.Height );
         Graphics gph = Graphics.FromImage( histBmp );
         gph.Clear( Color.FromArgb( 30, 30, 30 ) );

         Rectangle histRectangle = new Rectangle( 0, 0, histBmp.Width, histBmp.Height );
         BitmapData histBitmapData = histBmp.LockBits( histRectangle, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb );
         IntPtr histPtr = histBitmapData.Scan0;
         int bytes = Math.Abs( histBitmapData.Stride ) * histBmp.Height;
         byte[] rgbValue = new byte[bytes];
         Marshal.Copy( histPtr, rgbValue, 0, bytes );


         for (int x = 0; x < histBmp.Width; x++)
         {
            for (int y = 0; y < histBmp.Height; y++)
            {
               int pixelIndex = y * Math.Abs( histBitmapData.Stride ) + x * 3;

               if (drawArea.Height - y <= _bData[x] * Scale)
               {
                  rgbValue[pixelIndex + 0] = 250;


               }


               if (drawArea.Height - y <= _gData[x] * Scale)
               {
                  rgbValue[pixelIndex + 1] = 250;


               }


               if (drawArea.Height - y <= _rData[x] * Scale)
               {
                  rgbValue[pixelIndex + 2] = 250;


               }

            }
         }



         Marshal.Copy( rgbValue, 0, histPtr, bytes );
         histBmp.UnlockBits( histBitmapData );
         g.DrawImage( histBmp, new Point( drawArea.Left, drawArea.Top ) );
      }

      private int[] SmothArray( int[] array )
      {
         int size = array.Length;
         int[] newArray = new int[size];

         for (int i = 0; i < size - 5; i++)
         {
            newArray[i] = (array[i] + array[i + 1] + array[i + 2] + array[i + 3] + array[i + 4]) / 5;
         }

         newArray[size - 1] = (array[size - 1] + array[size - 2] + array[size - 3] + array[size - 4] + array[size - 5]) / 5;
         newArray[size - 2] = (array[size - 2] + array[size - 3] + array[size - 4] + array[size - 5] + array[size - 6]) / 5;
         newArray[size - 3] = (array[size - 3] + array[size - 4] + array[size - 5] + array[size - 6] + array[size - 7]) / 5;
         newArray[size - 4] = (array[size - 4] + array[size - 5] + array[size - 6] + array[size - 7] + array[size - 8]) / 5;
         newArray[size - 5] = (array[size - 5] + array[size - 6] + array[size - 7] + array[size - 8] + array[size - 9]) / 5;


         return newArray;
      }

      public double Scale { get; set; }


      protected override void OnPaint( PaintEventArgs e )
      {
         base.OnPaint( e );


         // 1. 画直方图
         // 1.1 画背景
         //Brush brush = new SolidBrush( Color.FromArgb( 30, 30, 30 ) );
         //e.Graphics.FillRectangle( brush, drawArea );
         // 1.2 画曲线
         UpdateHistogram( e.Graphics );


         // 2. 画格子
         int offset = drawArea.X;
         const float width = 256.0F / 4;
         const float height = 135.0F;
         Pen pen = new Pen( Color.LightGray, 0.1f ) { DashStyle = DashStyle.Dot };
         //e.Graphics.DrawLine( pen, width * 0 + offset, offset, width * 0 + offset, height + offset );
         e.Graphics.DrawLine( pen, width * 1 + offset, offset, width * 1 + offset, height + offset );
         e.Graphics.DrawLine( pen, width * 2 + offset, offset, width * 2 + offset, height + offset );
         e.Graphics.DrawLine( pen, width * 3 + offset, offset, width * 3 + offset, height + offset );
         //e.Graphics.DrawLine( pen, width * 4 + offset, offset, width * 4 + offset, height + offset );


         // 3. 画选择条
         Pen selPen = new Pen( Color.White ) { DashStyle = DashStyle.Solid };
         e.Graphics.DrawLine( selPen, selectPoint[0], selectPoint[1] );
      }

      private void InitializeComponent()
      {
         ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
         this.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
         this.ResumeLayout(false);

      }
   }
}