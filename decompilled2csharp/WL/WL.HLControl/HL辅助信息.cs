using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	[StandardModule]
	internal sealed class HL辅助信息
	{
		public enum HL文本状态
		{
			正常白,
			黄色高亮,
			副文本黯淡,
			禁用
		}

		public static readonly float DPI = 系统.系统信息.DPI;

		public static readonly float 线宽 = DPI * 3f;

		public static readonly float 细线宽 = DPI * 2f;

		public static readonly float 虚线宽 = DPI * 1f;

		public static readonly Color 基础绿 = Color.FromArgb(76, 88, 68);

		public static readonly SolidBrush 基础绿笔刷 = new SolidBrush(基础绿);

		public static readonly Color 内容绿 = Color.FromArgb(62, 70, 55);

		public static readonly SolidBrush 内容绿笔刷 = new SolidBrush(内容绿);

		public static readonly Color 白色 = Color.FromArgb(255, 255, 255);

		public static readonly Pen 白色笔 = new Pen(白色, 线宽);

		public static readonly SolidBrush 白色笔刷 = new SolidBrush(白色);

		public static readonly Color 淡色 = Color.FromArgb(160, 170, 149);

		public static readonly SolidBrush 淡色笔刷 = new SolidBrush(淡色);

		public static readonly Color 暗色 = Color.FromArgb(40, 46, 34);

		public static readonly Pen 暗色笔 = new Pen(暗色, 线宽);

		public static readonly SolidBrush 暗色笔刷 = new SolidBrush(暗色);

		public static readonly Color 边缘白 = Color.FromArgb(134, 145, 128);

		public static readonly Pen 边缘白笔 = new Pen(边缘白, 线宽);

		public static readonly SolidBrush 边缘白笔刷 = new SolidBrush(边缘白);

		public static readonly Pen 黑边框 = new Pen(Color.Black, 虚线宽);

		public static readonly Pen 黑虚线边框 = new Pen(Color.Black, 虚线宽)
		{
			DashStyle = DashStyle.Dot
		};

		public static readonly Color 内容黄 = Color.FromArgb(196, 181, 80);

		public static readonly SolidBrush 内容黄笔刷 = new SolidBrush(内容黄);

		public static readonly Color 内容白 = Color.FromArgb(216, 222, 211);

		public static readonly Pen 内容白笔 = new Pen(内容白, 线宽);

		public static readonly SolidBrush 内容白笔刷 = new SolidBrush(内容白);

		public static readonly Color 块黄 = Color.FromArgb(149, 136, 49);

		public static readonly SolidBrush 块黄笔刷 = new SolidBrush(块黄);

		public static readonly Color 滚动绿 = Color.FromArgb(90, 106, 80);

		public static readonly SolidBrush 滚动绿笔刷 = new SolidBrush(滚动绿);

		public static readonly Color 禁用底色 = Color.FromArgb(117, 128, 111);

		public static readonly SolidBrush 禁用底色笔刷 = new SolidBrush(禁用底色);

		public static readonly Color 边缘灰 = Color.FromArgb(162, 156, 154);

		public static readonly Pen 边缘灰笔 = new Pen(边缘灰, 细线宽);

		public static readonly SolidBrush 边缘灰笔刷 = new SolidBrush(边缘灰);

		public static readonly Color 细线灰 = Color.FromArgb(127, 140, 127);

		public static readonly Pen 细线灰笔 = new Pen(细线灰, 细线宽);

		public static readonly Color 按钮灰 = Color.FromArgb(193, 191, 189);

		public static readonly Pen 按钮灰笔 = new Pen(按钮灰, 细线宽);

		private static Bitmap _0024STATIC_0024文本宽度_002402CE1280A9_0024b;

		private static StaticLocalInitFlag _0024STATIC_0024文本宽度_002402CE1280A9_0024b_0024Init;

		private static Graphics _0024STATIC_0024文本宽度_002402CE1280A9_0024g;

		private static StaticLocalInitFlag _0024STATIC_0024文本宽度_002402CE1280A9_0024g_0024Init;

		public static Point 点(int x, int y)
		{
			return new Point(x, y);
		}

		public static PointF 点F(int x, int y)
		{
			return new PointF(x, y);
		}

		public static Pen 笔(Color 颜色, float 宽度 = 1f)
		{
			return new Pen(颜色, 宽度);
		}

		public static SolidBrush 笔刷(Color 颜色)
		{
			return new SolidBrush(颜色);
		}

		public static Point 左上角(Rectangle c, int 差 = 0)
		{
			return checked(点(c.Left + 差, c.Top + 差));
		}

		public static Point 左下角(Rectangle c, int 差 = 0)
		{
			return checked(点(c.Left + 差, c.Bottom - 差));
		}

		public static Point 右上角(Rectangle c, int 差 = 0)
		{
			return checked(点(c.Right - 差, c.Top + 差));
		}

		public static Point 右下角(Rectangle c, int 差 = 0)
		{
			return checked(点(c.Right - 差, c.Bottom - 差));
		}

		public static void 绘制基础矩形(Graphics g, Rectangle c, bool 按下 = false, bool 黑框 = false, Color 内容颜色 = default(Color))
		{
			if (类型.为空(内容颜色))
			{
				内容颜色 = 基础绿;
			}
			Graphics graphics = g;
			graphics.FillRectangle(new SolidBrush(内容颜色), c);
			Pen pen = (Pen)Interaction.IIf(按下, 暗色笔, 边缘白笔);
			Pen pen2 = (Pen)Interaction.IIf(按下, 边缘白笔, 暗色笔);
			graphics.DrawLine(pen, 左上角(c), 左下角(c));
			graphics.DrawLine(pen, 左上角(c), 右上角(c));
			graphics.DrawLine(pen2, 右上角(c), 右下角(c));
			graphics.DrawLine(pen2, 左下角(c), 右下角(c));
			if (黑框)
			{
				graphics.DrawRectangle(黑边框, c);
				float num = DPI * 4f;
				graphics.DrawRectangle(黑虚线边框, num, num, (float)c.Width - num * 2f, (float)c.Height - num * 2f);
			}
			graphics = null;
		}

		public static float 文本宽度(string 文本, Font 字体)
		{
			if (类型.为空(文本, 字体))
			{
				return 0f;
			}
			if (_0024STATIC_0024文本宽度_002402CE1280A9_0024b_0024Init == null)
			{
				Interlocked.CompareExchange(ref _0024STATIC_0024文本宽度_002402CE1280A9_0024b_0024Init, new StaticLocalInitFlag(), null);
			}
			bool lockTaken = false;
			try
			{
				Monitor.Enter(_0024STATIC_0024文本宽度_002402CE1280A9_0024b_0024Init, ref lockTaken);
				if (_0024STATIC_0024文本宽度_002402CE1280A9_0024b_0024Init.State == 0)
				{
					_0024STATIC_0024文本宽度_002402CE1280A9_0024b_0024Init.State = 2;
					_0024STATIC_0024文本宽度_002402CE1280A9_0024b = new Bitmap(1, 1);
				}
				else if (_0024STATIC_0024文本宽度_002402CE1280A9_0024b_0024Init.State == 2)
				{
					throw new IncompleteInitialization();
				}
			}
			finally
			{
				_0024STATIC_0024文本宽度_002402CE1280A9_0024b_0024Init.State = 1;
				if (lockTaken)
				{
					Monitor.Exit(_0024STATIC_0024文本宽度_002402CE1280A9_0024b_0024Init);
				}
			}
			if (_0024STATIC_0024文本宽度_002402CE1280A9_0024g_0024Init == null)
			{
				Interlocked.CompareExchange(ref _0024STATIC_0024文本宽度_002402CE1280A9_0024g_0024Init, new StaticLocalInitFlag(), null);
			}
			bool lockTaken2 = false;
			try
			{
				Monitor.Enter(_0024STATIC_0024文本宽度_002402CE1280A9_0024g_0024Init, ref lockTaken2);
				if (_0024STATIC_0024文本宽度_002402CE1280A9_0024g_0024Init.State == 0)
				{
					_0024STATIC_0024文本宽度_002402CE1280A9_0024g_0024Init.State = 2;
					_0024STATIC_0024文本宽度_002402CE1280A9_0024g = Graphics.FromImage(_0024STATIC_0024文本宽度_002402CE1280A9_0024b);
				}
				else if (_0024STATIC_0024文本宽度_002402CE1280A9_0024g_0024Init.State == 2)
				{
					throw new IncompleteInitialization();
				}
			}
			finally
			{
				_0024STATIC_0024文本宽度_002402CE1280A9_0024g_0024Init.State = 1;
				if (lockTaken2)
				{
					Monitor.Exit(_0024STATIC_0024文本宽度_002402CE1280A9_0024g_0024Init);
				}
			}
			float num = _0024STATIC_0024文本宽度_002402CE1280A9_0024g.MeasureString(文本, 字体).Width - 2f;
			if (num < 0f)
			{
				num = 0f;
			}
			return num;
		}

		public static void 修正Dock(Control c, bool 允许横向 = true, bool 允许竖向 = false)
		{
			DockStyle dock = c.Dock;
			bool flag = true;
			switch (dock)
			{
			case DockStyle.None:
				return;
			case DockStyle.Top:
				flag = 允许横向;
				break;
			case DockStyle.Bottom:
				flag = 允许横向;
				break;
			case DockStyle.Left:
				flag = 允许竖向;
				break;
			case DockStyle.Right:
				flag = 允许竖向;
				break;
			case DockStyle.Fill:
				flag = (允许竖向 && 允许横向);
				break;
			}
			if (!flag)
			{
				c.Dock = DockStyle.None;
			}
		}

		public static HL文本状态 获取文本状态(bool 启用, bool 高光 = false, bool 黯淡 = false)
		{
			if (!启用)
			{
				return HL文本状态.禁用;
			}
			if (高光)
			{
				return HL文本状态.黄色高亮;
			}
			if (黯淡)
			{
				return HL文本状态.副文本黯淡;
			}
			return HL文本状态.正常白;
		}

		/// <summary>
		/// 状态： 0正常白 1黄色高亮 2副文本黯淡 3禁用
		/// </summary>
		public static void 绘制文本(Graphics g, string t, Font f, float x, float y, HL文本状态 状态 = HL文本状态.正常白)
		{
			checked
			{
				if (!类型.为空(g, t, f))
				{
					Graphics graphics = g;
					Color color = 内容白;
					switch (状态)
					{
					case HL文本状态.黄色高亮:
						color = 内容黄;
						break;
					case HL文本状态.副文本黯淡:
						color = 淡色;
						break;
					case HL文本状态.禁用:
						color = 暗色;
						graphics.DrawString(t, f, 禁用底色笔刷, 点F((int)Math.Round(x + DPI), (int)Math.Round(y + DPI)));
						break;
					}
					graphics.DrawString(t, f, new SolidBrush(color), 点F((int)Math.Round(x), (int)Math.Round(y)));
					graphics = null;
				}
			}
		}

		public static void 绘制圆角矩形(Graphics g, Pen p, Rectangle 矩形, int 半径)
		{
			矩形.Offset(-1, -1);
			checked
			{
				Rectangle rect = new Rectangle(矩形.Location, new Size(半径 - 1, 半径 - 1));
				GraphicsPath graphicsPath = new GraphicsPath();
				graphicsPath.AddArc(rect, 180f, 90f);
				rect.X = 矩形.Right - 半径;
				graphicsPath.AddArc(rect, 270f, 90f);
				rect.Y = 矩形.Bottom - 半径;
				graphicsPath.AddArc(rect, 0f, 90f);
				rect.X = 矩形.Left;
				graphicsPath.AddArc(rect, 90f, 90f);
				graphicsPath.CloseFigure();
				g.DrawPath(p, graphicsPath);
			}
		}

		public static bool 是HL控件(Control c)
		{
			if (类型.为空(c))
			{
				return false;
			}
			string text = c.GetType().ToString();
			if (text.StartsWith("WL.HLControl") && !文本.包含(text.ToLower(), "panel"))
			{
				return true;
			}
			return false;
		}
	}
}
