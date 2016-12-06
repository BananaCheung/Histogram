using System.Drawing;

namespace Histogram
{
    partial class HistogramWin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
         this.lblSize = new System.Windows.Forms.Label();
         this.lblTB = new System.Windows.Forms.Label();
         this.lblTG = new System.Windows.Forms.Label();
         this.lblTR = new System.Windows.Forms.Label();
         this.lblCInfo = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // lblSize
         // 
         this.lblSize.AutoSize = true;
         this.lblSize.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblSize.ForeColor = System.Drawing.Color.White;
         this.lblSize.Location = new System.Drawing.Point(2, 139);
         this.lblSize.Margin = new System.Windows.Forms.Padding(48, 0, 48, 0);
         this.lblSize.Name = "lblSize";
         this.lblSize.Size = new System.Drawing.Size(119, 14);
         this.lblSize.TabIndex = 7;
         this.lblSize.Text = "Size:1400 x 6000";
         // 
         // lblTB
         // 
         this.lblTB.AutoSize = true;
         this.lblTB.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblTB.ForeColor = System.Drawing.Color.White;
         this.lblTB.Location = new System.Drawing.Point(161, 153);
         this.lblTB.Margin = new System.Windows.Forms.Padding(48, 0, 48, 0);
         this.lblTB.Name = "lblTB";
         this.lblTB.Size = new System.Drawing.Size(70, 14);
         this.lblTB.TabIndex = 1;
         this.lblTB.Text = "B:5000000";
         // 
         // lblTG
         // 
         this.lblTG.AutoSize = true;
         this.lblTG.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblTG.ForeColor = System.Drawing.Color.White;
         this.lblTG.Location = new System.Drawing.Point(82, 153);
         this.lblTG.Margin = new System.Windows.Forms.Padding(48, 0, 48, 0);
         this.lblTG.Name = "lblTG";
         this.lblTG.Size = new System.Drawing.Size(70, 14);
         this.lblTG.TabIndex = 6;
         this.lblTG.Text = "G:5000000";
         // 
         // lblTR
         // 
         this.lblTR.AutoSize = true;
         this.lblTR.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblTR.ForeColor = System.Drawing.Color.White;
         this.lblTR.Location = new System.Drawing.Point(3, 153);
         this.lblTR.Name = "lblTR";
         this.lblTR.Size = new System.Drawing.Size(70, 14);
         this.lblTR.TabIndex = 4;
         this.lblTR.Text = "R:5000000";
         // 
         // lblCInfo
         // 
         this.lblCInfo.AutoSize = true;
         this.lblCInfo.BackColor = System.Drawing.Color.Transparent;
         this.lblCInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblCInfo.ForeColor = System.Drawing.Color.White;
         this.lblCInfo.Location = new System.Drawing.Point(48, 24);
         this.lblCInfo.Name = "lblCInfo";
         this.lblCInfo.Size = new System.Drawing.Size(0, 14);
         this.lblCInfo.TabIndex = 0;
         // 
         // HistogramWin
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.BackColor = System.Drawing.Color.Black;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.lblCInfo);
         this.Controls.Add(this.lblTB);
         this.Controls.Add(this.lblTG);
         this.Controls.Add(this.lblTR);
         this.Controls.Add(this.lblSize);
         this.DoubleBuffered = true;
         this.Name = "HistogramWin";
         this.Size = new System.Drawing.Size(264, 170);
         this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.HistogramWin_MouseDoubleClick);
         this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.histogramPictureBox_MouseDown);
         this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.histogramPictureBox_MouseMove);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
      private System.Windows.Forms.Label lblSize;
      private System.Windows.Forms.Label lblTB;
      private System.Windows.Forms.Label lblTG;
      private System.Windows.Forms.Label lblTR;
      private System.Windows.Forms.Label lblCInfo;
   }
}
