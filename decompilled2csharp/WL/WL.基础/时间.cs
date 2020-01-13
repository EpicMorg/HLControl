using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Threading;

namespace WL.基础
{
	/// <summary>
	/// 和时间日期有关的模块
	/// </summary>
	[StandardModule]
	public sealed class 时间
	{
		private static Dictionary<int, double> _0024STATIC_0024过频_00240228D_0024d;

		private static StaticLocalInitFlag _0024STATIC_0024过频_00240228D_0024d_0024Init;

		/// <summary>
		/// 把时间转换为 UNIX 时间，默认不包括毫秒（毫秒是伪造的）
		/// </summary>
		public static ulong 转时间戳(DateTime 时间, bool 包括毫秒 = false)
		{
			checked
			{
				ulong num = (ulong)DateAndTime.DateDiff(DateInterval.Second, new DateTime(621355968000000000L), 时间.ToUniversalTime());
				if (包括毫秒)
				{
					float num2 = (float)DateAndTime.Timer;
					num = (ulong)Math.Round(((float)(double)num + num2 - (float)(long)Math.Round(num2)) * 1000f);
				}
				return num;
			}
		}

		/// <summary>
		/// 返回 Microsoft.VisualBasic.Timer 自午夜以来经过的值，该值表示的秒数。
		/// </summary>
		public static double 当日时间戳()
		{
			return DateAndTime.Timer;
		}

		/// <summary>
		/// 把当前时间转换为 UNIX 时间，精确到秒
		/// </summary>
		public static ulong 当前时间戳(bool 包括毫秒 = false)
		{
			return 转时间戳(DateAndTime.Now, 包括毫秒);
		}

		/// <summary>
		/// 把时间戳变成 DateTime，只支持秒级别的时间戳
		/// </summary>
		public static DateTime 时间戳转出(ulong 时间戳)
		{
			DateTime dateTime = new DateTime(621355968000000000L);
			if (decimal.Compare(new decimal(时间戳), 1000484608441m) > 0)
			{
				时间戳 = checked((ulong)Math.Round((double)时间戳 / 1000.0));
			}
			return dateTime.AddSeconds(时间戳).ToLocalTime();
		}

		/// <summary>
		/// 把时间格式化后输出字符串，用Y年，M月，D天，h时，m分钟，s秒，并且年强制为4位数字，其他强制为2位数字
		/// </summary>
		public static string 时间格式化(DateTime 时间, string 格式 = "Y-M-D h-m-s")
		{
			return 文本.正则.替换(文本.替换(格式, "m", "f"), "Y+", 文本.补左(时间.Year.ToString(), 4u, "0"), "M+", 文本.补左(时间.Month.ToString(), 2u, "0"), "D+", 文本.补左(时间.Day.ToString(), 2u, "0"), "h+", 文本.补左(时间.Hour.ToString(), 2u, "0"), "f+", 文本.补左(时间.Minute.ToString(), 2u, "0"), "s+", 文本.补左(时间.Second.ToString(), 2u, "0"));
		}

		/// <summary>
		/// 把秒数变成时间文字，默认从秒起步
		/// </summary>
		public static string 时间文字(long 秒数, bool 一天起步 = false)
		{
			秒数 = Math.Abs(秒数);
			if (一天起步 && 秒数 < 90000)
			{
				return "1天";
			}
			long num = 秒数;
			if (num < 1)
			{
				return "瞬间";
			}
			if (num < 61)
			{
				return Conversions.ToString(秒数) + "秒";
			}
			if (num < 4200)
			{
				return Conversions.ToString(Conversion.Fix((double)秒数 / 60.0)) + "分钟";
			}
			if (num < 90000)
			{
				return Conversions.ToString(Conversion.Fix((double)秒数 / 60.0 / 60.0)) + "小时";
			}
			if (num < 6912000)
			{
				return Conversions.ToString(Conversion.Fix((double)秒数 / 60.0 / 60.0 / 24.0)) + "天";
			}
			if (num < 32832000)
			{
				return Conversions.ToString(Conversion.Fix((double)秒数 / 60.0 / 60.0 / 24.0 / 30.0)) + "个月";
			}
			return Conversions.ToString(Conversion.Fix((double)秒数 / 60.0 / 60.0 / 24.0 / 365.0)) + "年";
		}

		/// <summary>
		/// 如果同一个ID访问这个函数的时间间隔过小，就返回true
		/// </summary>
		public static bool 过频(int id, double 时间)
		{
			if (_0024STATIC_0024过频_00240228D_0024d_0024Init == null)
			{
				Interlocked.CompareExchange(ref _0024STATIC_0024过频_00240228D_0024d_0024Init, new StaticLocalInitFlag(), null);
			}
			bool lockTaken = false;
			try
			{
				Monitor.Enter(_0024STATIC_0024过频_00240228D_0024d_0024Init, ref lockTaken);
				if (_0024STATIC_0024过频_00240228D_0024d_0024Init.State == 0)
				{
					_0024STATIC_0024过频_00240228D_0024d_0024Init.State = 2;
					_0024STATIC_0024过频_00240228D_0024d = new Dictionary<int, double>();
				}
				else if (_0024STATIC_0024过频_00240228D_0024d_0024Init.State == 2)
				{
					throw new IncompleteInitialization();
				}
			}
			finally
			{
				_0024STATIC_0024过频_00240228D_0024d_0024Init.State = 1;
				if (lockTaken)
				{
					Monitor.Exit(_0024STATIC_0024过频_00240228D_0024d_0024Init);
				}
			}
			if (!_0024STATIC_0024过频_00240228D_0024d.ContainsKey(id))
			{
				_0024STATIC_0024过频_00240228D_0024d.Add(id, 当日时间戳());
			}
			else
			{
				double num = _0024STATIC_0024过频_00240228D_0024d[id];
				double num2 = 当日时间戳();
				if (num2 - num < 时间)
				{
					return true;
				}
				_0024STATIC_0024过频_00240228D_0024d[id] = num2;
			}
			return false;
		}
	}
}
