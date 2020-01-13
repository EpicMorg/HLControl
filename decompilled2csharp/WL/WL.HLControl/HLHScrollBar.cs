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
	public class HLHScrollBar : HLControlBase
	{
		public delegate void ValueChangedEventHandler(HLHScrollBar sender, HLValueEventArgs e);

		private bool 按住上;

		private bool 按住;

		private bool 按住下;

		private int 值;

		private int 滚动一次;

		private int 最大;

		private int 最小;

		private int 上一个值;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ValueChangedEventHandler ValueChangedEvent;

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
				FixValue();
			}
		}

		[DefaultValue(1)]
		public int SmallChange
		{
			get
			{
				return 滚动一次;
			}
			set
			{
				滚动一次 = value;
				FixValue();
			}
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

		public HLHScrollBar()
		{
			base.MouseDown += _MouseDown;
			base.MouseWheel += _MouseWheel;
			base.MouseMove += _MouseMove;
			base.MouseUp += _MouseUp;
			DoubleBuffered = true;
			按住上 = false;
			按住下 = false;
			按住 = false;
			值 = 0;
			滚动一次 = 1;
			最大 = 100;
			最小 = 0;
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
				if (滚动一次 >= 最大)
				{
					滚动一次 = 最大 - 1;
				}
				if (滚动一次 <= 最小)
				{
					滚动一次 = 最小 + 1;
				}
				Invalidate();
				if (上一个值 != Value)
				{
					ValueChangedEvent?.Invoke(this, new HLValueEventArgs(上一个值, Value));
					上一个值 = Value;
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

		private void _MouseDown(object sender, MouseEventArgs e)
		{
			int x = e.X;
			int height = base.Height;
			checked
			{
				if (x <= height)
				{
					按住上 = true;
					Value -= 滚动一次;
				}
				else if (x >= base.Width - height)
				{
					按住下 = true;
					Value += 滚动一次;
				}
				else
				{
					_MouseMove(RuntimeHelpers.GetObjectValue(sender), e);
				}
			}
		}

		private void _MouseWheel(object sender, MouseEventArgs e)
		{
			checked
			{
				if (e.Delta > 0)
				{
					Value -= 滚动一次;
				}
				else
				{
					Value += 滚动一次;
				}
			}
		}

		private void _MouseMove(object sender, MouseEventArgs e)
		{
			checked
			{
				if (e.Button != 0 && !按住上 && !按住下)
				{
					int x = e.X;
					int height = base.Height;
					if (!按住 && x > height && x < base.Width - height)
					{
						按住 = true;
					}
					if (按住)
					{
						Value = (int)Math.Round((double)(x - height) / (double)(base.Width - 2 * height) * (double)(Maximum - Minimum) + (double)Minimum);
					}
				}
			}
		}

		private void _MouseUp(object sender, MouseEventArgs e)
		{
			if (按住 || 按住上 || 按住下)
			{
				按住上 = false;
				按住 = false;
				按住下 = false;
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			HL辅助信息.修正Dock(this);
			object A = base.Width;
			类型.设最小值(ref A, 50f * HL辅助信息.DPI);
			base.Width = Conversions.ToInteger(A);
			A = base.Height;
			类型.设最小值(ref A, 10f * HL辅助信息.DPI);
			base.Height = Conversions.ToInteger(A);
			A = base.Height;
			类型.设最大值(ref A, (double)base.Width / 5.0);
			base.Height = Conversions.ToInteger(A);
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			int height = base.Height;
			checked
			{
				int num = base.Width - 2 * height;
				int num2 = (int)Math.Round(2.2 * (double)height);
				Graphics graphics2 = graphics;
				HL辅助信息.绘制基础矩形(graphics, new Rectangle(0, 0, height, height), 按住上);
				HL辅助信息.绘制基础矩形(graphics, new Rectangle(base.Width - height, 0, height, height), 按住下);
				A = 0.4 * (double)base.Height;
				Font font = new Font("Segoe UI", Conversions.ToSingle(类型.设最大值(ref A, 20)));
				SizeF sizeF = graphics2.MeasureString("◀", font);
				int num3 = (int)Math.Round((double)((float)base.Height - sizeF.Width) * 0.5);
				int num4 = (int)Math.Round((double)((float)base.Height - sizeF.Height) * 0.5);
				HL辅助信息.绘制文本(graphics, "◀", font, num3, num4, HL辅助信息.获取文本状态(base.Enabled));
				HL辅助信息.绘制文本(graphics, "▶", font, base.Width - height + num3, num4, HL辅助信息.获取文本状态(base.Enabled));
				graphics2.FillRectangle(HL辅助信息.滚动绿笔刷, new Rectangle(height, 0, num, height));
				if (base.Enabled)
				{
					float num5 = (float)((double)(Value - Minimum) / (double)(Maximum - Minimum));
					num = (int)Math.Round(num5 * ((float)(num - num2) - 4f * HL辅助信息.DPI) + (float)height + 1f * HL辅助信息.DPI);
					HL辅助信息.绘制基础矩形(graphics, new Rectangle(num, 0, num2, height));
				}
				graphics2 = null;
			}
		}
	}
}
