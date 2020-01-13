using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	public class HLTabsHeader : HLControlBase
	{
		private bool 开始;

		private int 边缘;

		private ushort 标签宽;

		private TabControl tabs;

		private 程序.计时器 计时器;

		public ushort TabHeaderWidth
		{
			get
			{
				return 标签宽;
			}
			set
			{
				object A = value;
				类型.设最小值(ref A, 30f * HL辅助信息.DPI);
				value = Conversions.ToUShort(A);
				A = value;
				类型.设最大值(ref A, (double)base.Width / 3.0);
				value = Conversions.ToUShort(A);
				if (value != 标签宽)
				{
					标签宽 = value;
					Invalidate();
				}
			}
		}

		public TabControl RealTabControl
		{
			get
			{
				return tabs;
			}
			set
			{
				if (!开始)
				{
					tabs = value;
					if (!Information.IsNothing(value))
					{
						TabControl tabControl = tabs;
						tabControl.SelectedIndexChanged += delegate
						{
							Invalidate();
						};
						tabControl = null;
					}
				}
			}
		}

		public HLTabsHeader()
		{
			base.MouseUp += _MouseUp;
			DoubleBuffered = true;
			开始 = false;
			checked
			{
				标签宽 = (ushort)Math.Round(100f * HL辅助信息.DPI);
				边缘 = (int)Math.Round(3f * HL辅助信息.DPI);
				tabs = null;
				计时器 = new 程序.计时器(10, delegate
				{
					FixTabs();
				});
				计时器.启用 = false;
				计时器.工作次数 = 4u;
			}
		}

		private void _MouseUp(object sender, MouseEventArgs e)
		{
			checked
			{
				if (!类型.为空(tabs))
				{
					int num = (int)((float)e.X / ((float)unchecked((int)TabHeaderWidth) * HL辅助信息.DPI));
					if (num < tabs.TabCount)
					{
						tabs.SelectedIndex = num;
					}
				}
			}
		}

		private void FixTabs()
		{
			if (开始 && 类型.非空(tabs) && 类型.非空(FindForm()) && tabs.Visible && FindForm().WindowState != FormWindowState.Minimized)
			{
				TabControl tabControl = tabs;
				Graphics graphics = tabControl.CreateGraphics();
				Rectangle clientRectangle = tabControl.ClientRectangle;
				graphics.Clear(HL辅助信息.基础绿);
				HL辅助信息.绘制基础矩形(graphics, clientRectangle);
				if (类型.非空(tabControl.SelectedTab))
				{
					tabControl.SelectedTab.BackColor = HL辅助信息.基础绿;
				}
				tabControl = null;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (类型.为空(FindForm()))
			{
				return;
			}
			object A = base.Width;
			类型.设最小值(ref A, 90f * HL辅助信息.DPI);
			base.Width = Conversions.ToInteger(A);
			checked
			{
				int num = (int)Math.Round(Font.GetHeight() + 12f * HL辅助信息.DPI);
				A = num;
				类型.设最小值(ref A, 30f * HL辅助信息.DPI);
				num = (base.Height = Conversions.ToInteger(A));
				if (类型.为空(tabs) || tabs.TabPages.Count < 1)
				{
					float num3 = (float)时间.当日时间戳();
					HL辅助信息.绘制基础矩形(e.Graphics, base.ClientRectangle, 按下: false, 黑框: false, Color.Red);
					return;
				}
				if (!开始 && 程序.本程序.真的运行中())
				{
					开始 = true;
					TabControl tabControl = tabs;
					tabControl.Visible = base.Visible;
					tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
					tabControl.Top = (int)Math.Round((float)base.Bottom - 30f * HL辅助信息.DPI);
					tabControl.Left = base.Left;
					tabControl.Width = base.Width;
					tabControl = null;
					BringToFront();
				}
				if (开始)
				{
					计时器.启用 = true;
				}
				Graphics graphics = e.Graphics;
				int num4 = 0;
				int selectedIndex = tabs.SelectedIndex;
				Graphics graphics2 = graphics;
				float num5 = (float)unchecked((int)TabHeaderWidth) * HL辅助信息.DPI;
				int num6 = tabs.TabPages.Count - 1;
				Rectangle rectangle;
				for (int i = 0; i <= num6; i++)
				{
					TabPage tabPage = tabs.TabPages[i];
					try
					{
						tabPage.BackColor = HL辅助信息.基础绿;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						ProjectData.ClearProjectError();
					}
					rectangle = new Rectangle(num4, Conversions.ToInteger(Interaction.IIf(i == selectedIndex, 0, (double)边缘 * 1.5)), (int)Math.Round(num5), base.Height);
					graphics2.FillRectangle(HL辅助信息.基础绿笔刷, rectangle);
					graphics2.DrawLine(HL辅助信息.边缘白笔, HL辅助信息.左上角(rectangle), HL辅助信息.左下角(rectangle));
					graphics2.DrawLine(HL辅助信息.边缘白笔, HL辅助信息.左上角(rectangle), HL辅助信息.右上角(rectangle));
					graphics2.DrawLine(HL辅助信息.暗色笔, HL辅助信息.右上角(rectangle), HL辅助信息.右下角(rectangle));
					HL辅助信息.绘制文本(graphics, tabPage.Text, Font, num4 + 边缘, (float)((double)base.Height * 0.2), HL辅助信息.获取文本状态(base.Enabled, i == selectedIndex));
					num4 = (int)Math.Round((float)num4 + (num5 + HL辅助信息.线宽));
					if (num4 > base.Width)
					{
						break;
					}
				}
				num5 += HL辅助信息.线宽;
				rectangle = base.ClientRectangle;
				graphics2.DrawLine(HL辅助信息.暗色笔, HL辅助信息.点((int)Math.Round((float)(selectedIndex + 1) * num5), base.Height), HL辅助信息.右下角(rectangle));
				if (selectedIndex > 0)
				{
					graphics2.DrawLine(HL辅助信息.暗色笔, HL辅助信息.左下角(rectangle), HL辅助信息.点((int)Math.Round((float)selectedIndex * num5), base.Height));
				}
				graphics2 = null;
			}
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R5_002D1(object a0, EventArgs a1)
		{
			FixTabs();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R11_002D2(object a0, EventArgs a1)
		{
			Invalidate();
		}

		[CompilerGenerated]
		private void _Lambda_0024__11_002D0()
		{
			Invalidate();
		}
	}
}
