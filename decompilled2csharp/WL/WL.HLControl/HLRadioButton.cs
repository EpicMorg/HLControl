using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	[DefaultEvent("CheckedChanged")]
	public class HLRadioButton : HLControlBase
	{
		public delegate void CheckedChangedEventHandler(HLRadioButton sender, HLValueEventArgs e);

		private bool 值;

		private int 行高;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private CheckedChangedEventHandler CheckedChangedEvent;

		[DefaultValue(false)]
		public bool Checked
		{
			get
			{
				return 值;
			}
			set
			{
				if (value != 值)
				{
					if (类型.非空(base.Parent))
					{
						IEnumerator enumerator = default(IEnumerator);
						try
						{
							enumerator = base.Parent.Controls.GetEnumerator();
							while (enumerator.MoveNext())
							{
								Control control = (Control)enumerator.Current;
								if (control.GetType() == GetType() && control.GetHashCode() != GetHashCode())
								{
									HLRadioButton hLRadioButton = (HLRadioButton)control;
									hLRadioButton.Checked = false;
								}
							}
						}
						finally
						{
							if (enumerator is IDisposable)
							{
								(enumerator as IDisposable).Dispose();
							}
						}
					}
					int num = 0 - (值 ? 1 : 0);
					值 = value;
					CheckedChangedEvent?.Invoke(this, new HLValueEventArgs(num, 值));
					Invalidate();
				}
			}
		}

		public event CheckedChangedEventHandler CheckedChanged
		{
			[CompilerGenerated]
			add
			{
				CheckedChangedEventHandler checkedChangedEventHandler = CheckedChangedEvent;
				CheckedChangedEventHandler checkedChangedEventHandler2;
				do
				{
					checkedChangedEventHandler2 = checkedChangedEventHandler;
					CheckedChangedEventHandler value2 = (CheckedChangedEventHandler)Delegate.Combine(checkedChangedEventHandler2, value);
					checkedChangedEventHandler = Interlocked.CompareExchange(ref CheckedChangedEvent, value2, checkedChangedEventHandler2);
				}
				while ((object)checkedChangedEventHandler != checkedChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				CheckedChangedEventHandler checkedChangedEventHandler = CheckedChangedEvent;
				CheckedChangedEventHandler checkedChangedEventHandler2;
				do
				{
					checkedChangedEventHandler2 = checkedChangedEventHandler;
					CheckedChangedEventHandler value2 = (CheckedChangedEventHandler)Delegate.Remove(checkedChangedEventHandler2, value);
					checkedChangedEventHandler = Interlocked.CompareExchange(ref CheckedChangedEvent, value2, checkedChangedEventHandler2);
				}
				while ((object)checkedChangedEventHandler != checkedChangedEventHandler2);
			}
		}

		public HLRadioButton()
		{
			base.MouseDown += _MouseDown;
			DoubleBuffered = true;
			值 = false;
		}

		private void _MouseDown(object sender, MouseEventArgs e)
		{
			Checked = true;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			HL辅助信息.修正Dock(this);
			checked
			{
				行高 = (int)Math.Round(Font.GetHeight());
				base.Height = 行高;
				base.OnPaint(e);
				Graphics graphics = e.Graphics;
				Rectangle clientRectangle = base.ClientRectangle;
				Graphics graphics2 = graphics;
				base.Width = (int)Math.Round(graphics2.MeasureString(Text, Font).Width + (float)base.Height);
				float num = (float)((double)行高 * 0.6);
				float num2 = ((float)行高 - num) / 2f;
				graphics2.DrawEllipse(HL辅助信息.边缘灰笔, new RectangleF(num2, num2, num, num));
				num = (float)((double)num * 0.6);
				num2 = ((float)行高 - num) / 2f;
				if (Checked && base.Enabled)
				{
					graphics2.FillEllipse(HL辅助信息.白色笔刷, new RectangleF(num2, num2, num, num));
				}
				HL辅助信息.绘制文本(graphics, Text, Font, 行高, 0f, HL辅助信息.获取文本状态(base.Enabled, Checked));
				graphics2 = null;
			}
		}
	}
}
