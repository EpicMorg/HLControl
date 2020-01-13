using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	public class HLProgressBar : HLControlBase
	{
		private int 值;

		private int 最大;

		private int 最小;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _AutoReset;

		[DefaultValue(100)]
		public int Maximum
		{
			get
			{
				return 最大;
			}
			set
			{
				if (value != 最大)
				{
					最大 = value;
					FixValue();
				}
			}
		}

		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return 最小;
			}
			set
			{
				if (value != 最小)
				{
					最小 = value;
					FixValue();
				}
			}
		}

		[DefaultValue(0)]
		public int Value
		{
			get
			{
				return 值;
			}
			set
			{
				if (value != 值)
				{
					值 = value;
					FixValue();
				}
			}
		}

		[DefaultValue(false)]
		public bool AutoReset
		{
			get;
			set;
		}

		public HLProgressBar()
		{
			DoubleBuffered = true;
			最大 = 100;
			最小 = 0;
			值 = 0;
		}

		private void FixValue()
		{
			checked
			{
				if (最大 == 最小)
				{
					最大++;
				}
				else if (最大 < 最小)
				{
					ref int reference = ref 最大;
					object A = reference;
					ref int reference2 = ref 最小;
					object B = reference2;
					类型.互换(ref A, ref B);
					reference2 = Conversions.ToInteger(B);
					reference = Conversions.ToInteger(A);
				}
				if (值 < 最小)
				{
					值 = 最小;
				}
				else if (值 > 最大)
				{
					值 = 最大;
				}
				if (值 >= 最大 && AutoReset)
				{
					值 = 0;
				}
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			HL辅助信息.修正Dock(this, 允许横向: false);
			base.OnPaint(e);
			checked
			{
				int num = (int)Math.Round(30f * HL辅助信息.DPI);
				int num2 = (int)Math.Round(12f * HL辅助信息.DPI);
				int num3 = (int)Math.Round(6f * HL辅助信息.DPI);
				object A = base.Height;
				类型.设最小值(ref A, num);
				base.Height = Conversions.ToInteger(A);
				A = base.Width;
				类型.设最小值(ref A, num);
				base.Width = Conversions.ToInteger(A);
				if (unchecked(checked(base.Width - num3) % num2) != 0)
				{
					base.Width = (int)Math.Round((double)num2 * Conversion.Int((double)base.Width / (double)num2) + (double)num3);
				}
				Graphics graphics = e.Graphics;
				Graphics graphics2 = graphics;
				HL辅助信息.绘制基础矩形(graphics, base.ClientRectangle, 按下: true, 黑框: false, HL辅助信息.内容绿);
				if (Value == Minimum)
				{
					return;
				}
				num = (int)Math.Round((float)base.Height - 12f * HL辅助信息.DPI);
				num2 = (int)Math.Round(8f * HL辅助信息.DPI);
				int num4 = (int)Conversion.Int((double)((float)base.Width / ((float)num2 + 4f * HL辅助信息.DPI)) + 0.5);
				float num5 = (float)((double)(Value - Minimum) / (double)(Maximum - Minimum));
				int num6 = (int)Conversion.Int((double)(num5 * (float)num4) - 0.5);
				num3 = (int)Math.Round(4f * HL辅助信息.DPI);
				if (num6 > 0)
				{
					int num7 = num6;
					for (int i = 1; i <= num7; i++)
					{
						graphics2.FillRectangle(HL辅助信息.内容黄笔刷, num3, 5f * HL辅助信息.DPI, num2, num);
						num3 = (int)Math.Round((float)num3 + ((float)num2 + 4f * HL辅助信息.DPI));
					}
				}
				graphics2 = null;
			}
		}
	}
}
