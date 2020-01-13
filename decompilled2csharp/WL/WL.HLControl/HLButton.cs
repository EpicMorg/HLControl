using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	public class HLButton : HLControlBase
	{
		private bool 激活;

		private bool 按住;

		public HLButton()
		{
			base.TextChanged += delegate
			{
				_TextChanged();
			};
			base.KeyDown += _KeyDown;
			base.GotFocus += delegate
			{
				_GotFocus();
			};
			base.LostFocus += delegate
			{
				_LostFocus();
			};
			base.MouseDown += _MouseDown;
			base.MouseUp += delegate
			{
				_MouseUp();
			};
			DoubleBuffered = true;
			激活 = false;
			按住 = false;
		}

		private void _TextChanged()
		{
			if (文本.包含(Text, "\r", "\n"))
			{
				Text = 文本.替换(Text, "\r\n", " ", "\n", " ", "\r", " ");
			}
		}

		private void _KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				base.OnClick(null);
			}
		}

		private void _GotFocus()
		{
			激活 = true;
			Invalidate();
		}

		private void _LostFocus()
		{
			激活 = false;
			Invalidate();
		}

		private void _MouseDown(object sender, MouseEventArgs e)
		{
			按住 = true;
			if (激活)
			{
				Invalidate();
			}
			else
			{
				Focus();
			}
		}

		private void _MouseUp()
		{
			按住 = false;
			Invalidate();
		}

		public void PerformClick()
		{
			base.OnClick(null);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			HL辅助信息.修正Dock(this);
			base.Height = checked((int)Math.Round(10f * HL辅助信息.DPI + Font.GetHeight()));
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			Graphics graphics2 = graphics;
			HL辅助信息.绘制基础矩形(graphics, base.ClientRectangle, 按住, 激活);
			HL辅助信息.绘制文本(graphics, Text, Font, 6f * HL辅助信息.DPI, 4f * HL辅助信息.DPI, HL辅助信息.获取文本状态(base.Enabled));
			graphics2 = null;
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R2_002D1(object a0, EventArgs a1)
		{
			_TextChanged();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R2_002D2(object a0, EventArgs a1)
		{
			_GotFocus();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R2_002D3(object a0, EventArgs a1)
		{
			_LostFocus();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R2_002D4(object a0, MouseEventArgs a1)
		{
			_MouseUp();
		}
	}
}
