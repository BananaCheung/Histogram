using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
   public partial class Form1 : Form
   {
      private Histogram.HistogramForm _histogramForm;

      public Form1()
      {
         InitializeComponent();
      }

      private void button1_Click( object sender, EventArgs e )
      {
         Bitmap bmp = Image.FromFile( @"C:\Users\Banana Cheung\OneDrive\PHOTO\DSC_0148.jpg" ) as Bitmap;
         _histogramForm = new Histogram.HistogramForm( ref bmp );
         _histogramForm.Scale = Convert.ToDouble( textBox1.Text );
         _histogramForm.Show();
      }

      private void button2_Click( object sender, EventArgs e )
      {
         Text = _histogramForm.Size.ToString();
      }
   }
}
