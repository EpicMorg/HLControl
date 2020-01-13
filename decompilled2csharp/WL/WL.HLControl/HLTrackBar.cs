using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	[DefaultEvent("ValueChanged")]
	public class HLTrackBar : HLControlBase
	{
		public delegate void ValueChangedEventHandler(HLTrackBar sender, HLValueEventArgs e);

		private int 值;

		private int 最大;

		private int 最小;

		private int 可触;

		private bool 按住;

		private float 边缘;

		private int 上一个值;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ValueChangedEventHandler ValueChangedEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private HLLabel _HighLightLabel;

		[DefaultValue(100)]
		public int Maximum
		{
			get
			{
				return 最大;
			}
			set
			{
				最大 = value;
				FixValue();
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
				最小 = value;
				FixValue();
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
				值 = value;
				if (类型.非空(HighLightLabel))
				{
					HighLightLabel.HighLight = true;
				}
				FixValue();
			}
		}

		public HLLabel HighLightLabel
		{
			get;
			set;
		}

		public event ValueChangedEventHandler ValueChanged
		{
			[CompilerGenerated]
			add
			{
				ValueChangedEventHandler valueChangedEventHandler = ValueChangedEvent;
				ValueChangedEventHandler valueChangedEventHandler2;
				do
				{
					valueChangedEventHandler2 = valueChangedEventHandler;
					ValueChangedEventHandler value2 = (ValueChangedEventHandler)Delegate.Combine(valueChangedEventHandler2, value);
					valueChangedEventHandler = Interlocked.CompareExchange(ref ValueChangedEvent, value2, valueChangedEventHandler2);
				}
				while ((object)valueChangedEventHandler != valueChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				ValueChangedEventHandler valueChangedEventHandler = ValueChangedEvent;
				ValueChangedEventHandler valueChangedEventHandler2;
				do
				{
					valueChangedEventHandler2 = valueChangedEventHandler;
					ValueChangedEventHandler value2 = (ValueChangedEventHandler)Delegate.Remove(valueChangedEventHandler2, value);
					valueChangedEventHandler = Interlocked.CompareExchange(ref ValueChangedEvent, value2, valueChangedEventHandler2);
				}
				while ((object)valueChangedEventHandler != valueChangedEventHandler2);
			}
		}

		public HLTrackBar()
		{
			base.MouseMove += _MouseMove;
			base.MouseDown += _MouseDown;
			base.MouseUp += _MouseUp;
			DoubleBuffered = true;
			最大 = 100;
			最小 = 0;
			值 = 0;
			可触 = 0;
			按住 = false;
			边缘 = 6f * HL辅助信息.DPI;
			上一个值 = 0;
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
				Invalidate();
				if (上一个值 != 值)
				{
					ValueChangedEvent?.Invoke(this, new HLValueEventArgs(上一个值, 值));
					上一个值 = 值;
				}
			}
		}

		public void ChangeValueWithoutRaiseEvent(int v)
		{
			object A = v;
			类型.设最大值(ref A, 最大);
			v = Conversions.ToInteger(A);
			A = v;
			类型.设最小值(ref A, 最小);
			v = Conversions.ToInteger(A);
			if (v != 值)
			{
				值 = v;
				Invalidate();
			}
		}

		private void _MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != 0)
			{
				_MouseDown(RuntimeHelpers.GetObjectValue(sender), e);
			}
		}

		private void _MouseDown(object sender, MouseEventArgs e)
		{
			int x = e.X;
			if (x >= 0 && x <= base.Width)
			{
				Value = checked((int)Math.Round((double)x / (double)base.Width * (double)(Maximum - Minimum) + (double)Minimum));
			}
		}

		private void _MouseUp(object sender, MouseEventArgs e)
		{
			if (按住)
			{
				按住 = false;
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			HL辅助信息.修正Dock(this);
			base.OnPaint(e);
			checked
			{
				base.Height = (int)Math.Round(30f * HL辅助信息.DPI);
				object A = base.Width;
				类型.设最小值(ref A, base.Height);
				base.Width = Conversions.ToInteger(A);
				Graphics graphics = e.Graphics;
				Graphics graphics2 = graphics;
				int num = (int)Math.Round(20f * HL辅助信息.DPI);
				int num2 = (int)Math.Round(边缘);
				int num3 = (int)Math.Round((double)(num - num2) * 0.5);
				Rectangle c = new Rectangle(0, num3, base.Width, num2);
				num3 += num2 * 2;
				HL辅助信息.绘制基础矩形(graphics, c, 按下: true, 黑框: false, Color.Black);
				for (int num4 = (int)Math.Round(4f * HL辅助信息.DPI); num4 < base.Width; num4 = (int)Math.Round((float)num4 + 15f * HL辅助信息.DPI))
				{
					graphics2.DrawLine(HL辅助信息.细线灰笔, HL辅助信息.点(num4, num3), HL辅助信息.点(num4, num3 + num2));
				}
				float num5 = (float)((double)(Value - Minimum) / (double)(Maximum - Minimum));
				c = new Rectangle((int)Math.Round((double)num5 * ((double)base.Width - (double)边缘 * 1.7)), 0, (int)Math.Round((double)num2 * 1.5), num);
				可触 = c.Left;
				HL辅助信息.绘制基础矩形(graphics, c, 按下: false, 黑框: false, HL辅助信息.基础绿);
				graphics2 = null;
			}
		}
	}
}
