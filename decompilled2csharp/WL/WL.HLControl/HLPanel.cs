using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace WL.HLControl
{
	public class HLPanel : Panel
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__3_002D0
		{
			public HLVScrollBar _0024W2;

			public HLPanel _0024VB_0024Me;

			public _Closure_0024__3_002D0(_Closure_0024__3_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024W2 = arg0._0024W2;
				}
			}

			internal void _Lambda_0024__0(HLVScrollBar sender, HLValueEventArgs e)
			{
				int num = Conversions.ToInteger(Operators.SubtractObject(e.NewValue, e.OldValue));
				checked
				{
					IEnumerator enumerator = default(IEnumerator);
					try
					{
						enumerator = _0024VB_0024Me.Controls.GetEnumerator();
						while (enumerator.MoveNext())
						{
							Control control = (Control)enumerator.Current;
							if (control.GetHashCode() != _0024W2.GetHashCode())
							{
								control.Top -= num;
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
			}
		}

		private uint 边缘;

		private HLVScrollBar 滚动条;

		private bool 自动滚动;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _Border;

		[DefaultValue(true)]
		public bool Border
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public new bool AutoScroll
		{
			get
			{
				return 自动滚动;
			}
			set
			{
				if (value != 自动滚动)
				{
					自动滚动 = value;
					Invalidate();
				}
			}
		}

		public HLPanel()
		{
			base.MouseWheel += _MouseWheel;
			DoubleBuffered = true;
			checked
			{
				边缘 = (uint)Math.Round(HL辅助信息.线宽);
				Border = true;
				自动滚动 = false;
				滚动条 = new HLVScrollBar();
				base.Controls.Add(滚动条);
				HLVScrollBar hLVScrollBar = 滚动条;
				hLVScrollBar.Visible = false;
				hLVScrollBar.Left = 0;
				hLVScrollBar.Top = 0;
				hLVScrollBar.Width = (int)Math.Round(25f * HL辅助信息.DPI);
				hLVScrollBar.Height = base.Height;
				hLVScrollBar.SmallChange = (int)Math.Round(15f * HL辅助信息.DPI);
				hLVScrollBar.Dock = DockStyle.Right;
				hLVScrollBar.ValueChanged += delegate(HLVScrollBar sender, HLValueEventArgs e)
				{
					int num = Conversions.ToInteger(Operators.SubtractObject(e.NewValue, e.OldValue));
					IEnumerator enumerator = default(IEnumerator);
					try
					{
						enumerator = base.Controls.GetEnumerator();
						while (enumerator.MoveNext())
						{
							Control control = (Control)enumerator.Current;
							if (control.GetHashCode() != hLVScrollBar.GetHashCode())
							{
								control.Top -= num;
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
				};
			}
		}

		private void _MouseWheel(object sender, MouseEventArgs e)
		{
			if (滚动条.Visible)
			{
				滚动条.PerformMouseWheel(RuntimeHelpers.GetObjectValue(sender), e);
			}
		}

		public void ResetScroll()
		{
			滚动条.Value = 滚动条.Minimum;
		}

		public void PerformScroll(int value)
		{
			checked
			{
				滚动条.Value += value;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			AutoSize = false;
			base.BorderStyle = BorderStyle.None;
			checked
			{
				base.Padding = new Padding((int)边缘);
				BackColor = HL辅助信息.基础绿;
				if (AutoScroll)
				{
					int num = 0;
					int num2 = 0;
					IEnumerator enumerator = default(IEnumerator);
					try
					{
						enumerator = base.Controls.GetEnumerator();
						while (enumerator.MoveNext())
						{
							Control control = (Control)enumerator.Current;
							if (control.GetHashCode() != 滚动条.GetHashCode())
							{
								if (control.Bottom > num)
								{
									num = control.Bottom;
								}
								if (control.Top < num2)
								{
									num2 = control.Top;
								}
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
					num = (int)Math.Round((float)num + ((float)(-num2 - base.Height) + 20f * HL辅助信息.DPI));
					HLVScrollBar hLVScrollBar = 滚动条;
					hLVScrollBar.Visible = (num > 0);
					hLVScrollBar.Minimum = 0;
					hLVScrollBar.Maximum = num;
					hLVScrollBar = null;
				}
				else
				{
					滚动条.Visible = false;
				}
				Graphics graphics = e.Graphics;
				Rectangle clientRectangle = base.ClientRectangle;
				Graphics graphics2 = graphics;
				clientRectangle.Height = (int)(unchecked((long)clientRectangle.Height) - unchecked((long)边缘));
				clientRectangle.Width = (int)(unchecked((long)clientRectangle.Width) - unchecked((long)边缘));
				if (Border)
				{
					HL辅助信息.绘制基础矩形(graphics, clientRectangle, 按下: false, 黑框: false, HL辅助信息.基础绿);
				}
				graphics2 = null;
			}
		}
	}
}
