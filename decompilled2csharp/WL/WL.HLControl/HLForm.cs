using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WL.My.Resources;
using WL.基础;

namespace WL.HLControl
{
	[DefaultEvent("Load")]
	[Designer("System.Windows.Forms.Design.FormDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[DesignerCategory("Form")]
	[DesignTimeVisible(false)]
	[InitializationEvent("Load")]
	[ToolboxItem(false)]
	[ToolboxItemFilter("System.Windows.Forms.Control.TopLevel")]
	public class HLForm : Form
	{
		public static readonly Icon SteamIcon = Resources.SteamLogo;

		private Rectangle 关闭按钮区域;

		private Rectangle 最小化按钮区域;

		private int LastX;

		private int LastY;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _AutoScroll;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _AutoSize;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _ShowSteamIcon;

		[Browsable(false)]
		public override bool AutoScroll
		{
			get;
			set;
		}

		[Browsable(false)]
		public override bool AutoSize
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool ShowSteamIcon
		{
			get;
			set;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (程序.本程序.真的运行中())
				{
					createParams.Style = 131072;
				}
				return createParams;
			}
		}

		public HLForm()
		{
			base.Load += _Load;
			base.MouseUp += _MouseUp;
			base.MouseDown += _MouseDown;
			base.Move += _Move;
			LastX = -1;
			LastY = -1;
			DoubleBuffered = true;
			_SetDefault();
			Font = new Font("Microsoft Yahei", 12f);
			ShowSteamIcon = true;
		}

		private void _SetDefault()
		{
			BackColor = HL辅助信息.基础绿;
			checked
			{
				int num = (int)Math.Round(10f * HL辅助信息.DPI);
				base.Padding = new Padding(num, num * 5, num, num);
				MinimumSize = new Size(200, 80);
				base.AutoScaleMode = AutoScaleMode.None;
				base.FormBorderStyle = FormBorderStyle.None;
				base.StartPosition = FormStartPosition.CenterScreen;
				base.MaximizeBox = false;
			}
		}

		private void _Load(object sender, EventArgs e)
		{
			_SetDefault();
			float dPI = HL辅助信息.DPI;
			checked
			{
				if (dPI > 1f)
				{
					base.Height = (int)Math.Round((float)base.Height * dPI);
					base.Width = (int)Math.Round((float)base.Width * dPI);
					IEnumerator enumerator = default(IEnumerator);
					try
					{
						enumerator = base.Controls.GetEnumerator();
						while (enumerator.MoveNext())
						{
							Control dPI2 = (Control)enumerator.Current;
							SetDPI(dPI2);
						}
					}
					finally
					{
						if (enumerator is IDisposable)
						{
							(enumerator as IDisposable).Dispose();
						}
					}
					base.Left = (int)Math.Round((double)(系统.系统信息.屏幕分辨率.Width - base.Width) * 0.5);
					base.Top = (int)Math.Round((double)(系统.系统信息.屏幕分辨率.Height - base.Height) * 0.5);
				}
			}
		}

		private void _MouseUp(object sender, MouseEventArgs e)
		{
			if (关闭按钮区域.Contains(e.Location))
			{
				if (base.IsMdiChild)
				{
					Hide();
				}
				else
				{
					Close();
				}
			}
			else if (最小化按钮区域.Contains(e.Location))
			{
				base.WindowState = FormWindowState.Minimized;
			}
			LastX = -1;
			LastY = -1;
		}

		private void _MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Y < 最小化按钮区域.Bottom && e.X < 最小化按钮区域.X)
			{
				引用.拖动窗口(this);
			}
		}

		private void SetDPI(Control c)
		{
			float dPI = HL辅助信息.DPI;
			Control control = c;
			checked
			{
				Control control2;
				(control2 = control).Left = (int)Math.Round((float)control2.Left * dPI);
				(control2 = control).Top = (int)Math.Round((float)control2.Top * dPI);
				(control2 = control).Height = (int)Math.Round((float)control2.Height * dPI);
				(control2 = control).Width = (int)Math.Round((float)control2.Width * dPI);
				if (!HL辅助信息.是HL控件(c) && c.HasChildren)
				{
					IEnumerator enumerator = default(IEnumerator);
					try
					{
						enumerator = c.Controls.GetEnumerator();
						while (enumerator.MoveNext())
						{
							Control dPI2 = (Control)enumerator.Current;
							SetDPI(dPI2);
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
				control = null;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			Graphics graphics2 = graphics;
			HL辅助信息.绘制基础矩形(graphics, base.ClientRectangle);
			checked
			{
				int num = (int)Math.Round(24f * HL辅助信息.DPI);
				int num2 = (int)Math.Round(8f * HL辅助信息.DPI);
				Rectangle c = 关闭按钮区域 = new Rectangle(base.Width - num2 - num, (int)Math.Round((double)num2 * 1.5), num, num);
				HL辅助信息.绘制基础矩形(graphics, c);
				int num3 = (int)Math.Round(9f * HL辅助信息.DPI);
				string name = Font.Name;
				object A = 10f * HL辅助信息.DPI;
				Font font = new Font(name, Conversions.ToSingle(类型.设最大值(ref A, 13)));
				HL辅助信息.绘制文本(graphics, Text, font, 31f * HL辅助信息.DPI, num3, HL辅助信息.获取文本状态(base.Enabled));
				if (base.ShowIcon)
				{
					Icon icon = (!ShowSteamIcon) ? base.Icon : SteamIcon;
					int num4 = 16;
					if (font.GetHeight() > 120f)
					{
						num4 = 128;
					}
					else if (font.GetHeight() > 60f)
					{
						num4 = 64;
					}
					else if (font.GetHeight() > 30f)
					{
						num4 = 32;
					}
					graphics2.DrawIcon(icon, new Rectangle(num3, (int)Math.Round((float)num3 + 2f * HL辅助信息.DPI), num4, num4));
				}
				num3 = (int)Math.Round(7f * HL辅助信息.DPI);
				Pen 按钮灰笔 = HL辅助信息.按钮灰笔;
				graphics2.DrawLine(按钮灰笔, HL辅助信息.左上角(c, num3), HL辅助信息.右下角(c, num3));
				graphics2.DrawLine(按钮灰笔, HL辅助信息.左下角(c, num3), HL辅助信息.右上角(c, num3));
				c.X -= num + num2;
				最小化按钮区域 = c;
				HL辅助信息.绘制基础矩形(graphics, c);
				num3 = (int)Math.Round((double)c.Y + (double)num * 0.5);
				graphics2.DrawLine(按钮灰笔, HL辅助信息.点((int)Math.Round((double)c.X + (double)num * 0.3), num3), HL辅助信息.点((int)Math.Round((double)c.X + (double)num * 0.7), num3));
				graphics2 = null;
			}
		}

		private void _Move(object sender, EventArgs e)
		{
			checked
			{
				if (base.Visible && base.WindowState != FormWindowState.Minimized)
				{
					if (base.Top < 0)
					{
						base.Top = 0;
					}
					int num = 150;
					if (base.Right < num)
					{
						base.Left = -base.Width + num;
					}
					num = 系统.系统信息.屏幕分辨率.Width - num;
					if (base.Left > num)
					{
						base.Left = num;
					}
					num = 系统.系统信息.屏幕分辨率.Height - 50;
					if (base.Top > num)
					{
						base.Top = num;
					}
				}
			}
		}
	}
}
