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
	[DefaultEvent("CheckedChanged")]
	public class HLCheckBox : HLControlBase
	{
		public delegate void CheckedChangedEventHandler(HLCheckBox sender, HLValueEventArgs e);

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
					值 = value;
					bool flag = 值;
					CheckedChangedEvent?.Invoke(this, new HLValueEventArgs(值, value));
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

		public HLCheckBox()
		{
			base.MouseDown += _MouseDown;
			DoubleBuffered = true;
			值 = false;
		}

		private void _MouseDown(object sender, MouseEventArgs e)
		{
			bool A = Checked;
			类型.反转(ref A);
			Checked = A;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			HL辅助信息.修正Dock(this);
			checked
			{
				行高 = (int)Math.Round(Font.GetHeight());
				base.Height = (int)Math.Round((float)行高 + 2f * HL辅助信息.DPI);
				base.OnPaint(e);
				Graphics graphics = e.Graphics;
				Rectangle clientRectangle = base.ClientRectangle;
				Graphics graphics2 = graphics;
				base.Width = (int)Math.Round(graphics2.MeasureString(Text, Font).Width + (float)base.Height);
				float num = (float)((double)行高 * 0.8);
				float num2 = ((float)行高 - num) / 2f + 2f * HL辅助信息.DPI;
				if (base.Enabled && Checked)
				{
					num -= 4f * HL辅助信息.DPI;
					Pen pen = new Pen(HL辅助信息.白色, 3f * HL辅助信息.DPI);
					Point point = HL辅助信息.点((int)Math.Round((double)num2 + (double)num * 0.4), (int)Math.Round(num2 + num));
					graphics2.DrawLine(pen, HL辅助信息.点((int)Math.Round(num2 + 2f * HL辅助信息.DPI), (int)Math.Round((double)num2 + (double)num * 0.5)), point);
					graphics2.DrawLine(pen, point, HL辅助信息.点((int)Math.Round(num2 + num), (int)Math.Round((double)num2 + (double)num * 0.2)));
					num += 4f * HL辅助信息.DPI;
				}
				HL辅助信息.绘制圆角矩形(graphics, HL辅助信息.边缘灰笔, new Rectangle((int)Math.Round(num2), (int)Math.Round(num2), (int)Math.Round(num), (int)Math.Round(num)), (int)Math.Round(5f * HL辅助信息.DPI));
				num = (float)((double)num * 0.6);
				num2 = ((float)行高 - num) / 2f;
				HL辅助信息.绘制文本(graphics, Text, Font, 行高, 0f, HL辅助信息.获取文本状态(base.Enabled, Checked));
				graphics2 = null;
			}
		}
	}
}
