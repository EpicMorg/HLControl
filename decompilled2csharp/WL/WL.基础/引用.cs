using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WL.基础
{
	/// <summary>
	/// 引用其他的DLL的函数，并封装好一些常用的用法
	/// </summary>
	[StandardModule]
	public sealed class 引用
	{
		[DllImport("user32.dll")]
		private static extern bool ReleaseCapture();

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr HWnd, int Msg, int WParam, int IParam);

		/// <summary>
		/// 对窗口进行拖放，通常用于无边框的窗体，写在MouseDown事件里使用
		/// </summary>
		public static void 拖动窗口(Form f)
		{
			if (!类型.为空(f))
			{
				try
				{
					ReleaseCapture();
					SendMessage(f.Handle, 274, 61458, 0);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					程序.出错(ex2);
					ProjectData.ClearProjectError();
				}
			}
		}

		/// <summary>
		/// 对控件发送滚动信号，使用 EM_LINESCROLL 
		/// </summary>
		public static void 滚动(Control 控件, bool 垂直, int 数量)
		{
			if (!类型.为空(控件))
			{
				int iParam = 0;
				int wParam = 0;
				if (垂直)
				{
					iParam = 数量;
				}
				else
				{
					wParam = 数量;
				}
				try
				{
					SendMessage(控件.Handle, 182, wParam, iParam);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					程序.出错(ex2);
					ProjectData.ClearProjectError();
				}
			}
		}

		/// <summary>
		/// 获得文本框的行数，使用 EM_LINEINDEX 
		/// </summary>
		public static int 获得文本框行数(Control 控件)
		{
			if (类型.为空(控件) || 控件.Text.Length < 1)
			{
				return 0;
			}
			int num = 0;
			try
			{
				return SendMessage(控件.Handle, 186, 0, 0);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				程序.出错(ex2);
				ProjectData.ClearProjectError();
			}
			return 0;
		}

		[DllImport("wininet.dll")]
		private static extern bool InternetGetCookieExA(string lpszUrl, string lpszCookieName, StringBuilder lpszCookieData, ref int lpdwSize, int dwFlags, IntPtr lpReserved);

		/// <summary>
		/// 从IE浏览器里获得指定网站的Cookie，网站应该是http://或者https://开头的完整链接
		/// </summary>
		public static string 获取IEcookie(string 网站)
		{
			int lpdwSize = 512;
			StringBuilder stringBuilder = new StringBuilder(lpdwSize);
			if (InternetGetCookieExA(网站, null, stringBuilder, ref lpdwSize, 8192, IntPtr.Zero))
			{
				return stringBuilder.ToString();
			}
			return "";
		}
	}
}
