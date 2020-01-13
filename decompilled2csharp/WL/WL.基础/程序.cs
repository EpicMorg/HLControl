#define DEBUG
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management.Automation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using WL.My;

namespace WL.基础
{
	/// <summary>
	/// 与本程序还有其他程序的相关操作的模块
	/// </summary>
	[StandardModule]
	public sealed class 程序
	{
		/// <summary>
		/// 本程序的相关信息
		/// </summary>
		public sealed class 本程序
		{
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private static bool _控制台输出;

			private static Process _0024STATIC_0024get_进程_0024001281DD_0024p;

			private static StaticLocalInitFlag _0024STATIC_0024get_进程_0024001281DD_0024p_0024Init;

			private static string _0024STATIC_0024get_路径_002400E_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_路径_002400E_0024m_0024Init;

			private static Icon _0024STATIC_0024get_图标_002400128085_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_图标_002400128085_0024m_0024Init;

			/// <summary>
			/// 获取本程序的Process类
			/// </summary>
			public static Process 进程
			{
				get
				{
					if (_0024STATIC_0024get_进程_0024001281DD_0024p_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_进程_0024001281DD_0024p_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_进程_0024001281DD_0024p_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_进程_0024001281DD_0024p_0024Init.State == 0)
						{
							_0024STATIC_0024get_进程_0024001281DD_0024p_0024Init.State = 2;
							_0024STATIC_0024get_进程_0024001281DD_0024p = Process.GetCurrentProcess();
						}
						else if (_0024STATIC_0024get_进程_0024001281DD_0024p_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_进程_0024001281DD_0024p_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_进程_0024001281DD_0024p_0024Init);
						}
					}
					return _0024STATIC_0024get_进程_0024001281DD_0024p;
				}
			}

			/// <summary>
			/// 获得本程序的PID
			/// </summary>
			/// <returns></returns>
			public static uint PID => checked((uint)进程.Id);

			/// <summary>
			/// 获取本程序的文件路径
			/// </summary>
			/// <returns></returns>
			public static string 路径
			{
				get
				{
					if (_0024STATIC_0024get_路径_002400E_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_路径_002400E_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_路径_002400E_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_路径_002400E_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_路径_002400E_0024m_0024Init.State = 2;
							_0024STATIC_0024get_路径_002400E_0024m = 文件.路径标准化(MyProject.Application.Info.DirectoryPath);
						}
						else if (_0024STATIC_0024get_路径_002400E_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_路径_002400E_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_路径_002400E_0024m_0024Init);
						}
					}
					return _0024STATIC_0024get_路径_002400E_0024m;
				}
			}

			/// <summary>
			/// 获得本程序的文件名，不包含.exe
			/// </summary>
			/// <returns></returns>
			public static string 文件名 => 进程.ProcessName;

			/// <summary>
			/// 是否以 Console.Writeline 来操作输出 sub
			/// </summary>
			/// <returns></returns>
			public static bool 控制台输出
			{
				get;
				set;
			} = false;


			/// <summary>
			/// 获得本程序的主文件图标
			/// </summary>
			public static Icon 图标
			{
				get
				{
					if (_0024STATIC_0024get_图标_002400128085_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_图标_002400128085_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_图标_002400128085_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_图标_002400128085_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_图标_002400128085_0024m_0024Init.State = 2;
							_0024STATIC_0024get_图标_002400128085_0024m = null;
						}
						else if (_0024STATIC_0024get_图标_002400128085_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_图标_002400128085_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_图标_002400128085_0024m_0024Init);
						}
					}
					try
					{
						if (类型.为空(_0024STATIC_0024get_图标_002400128085_0024m))
						{
							_0024STATIC_0024get_图标_002400128085_0024m = Icon.ExtractAssociatedIcon(路径 + 文件名 + ".exe");
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						_0024STATIC_0024get_图标_002400128085_0024m = SystemIcons.Application;
						出错(ex2);
						ProjectData.ClearProjectError();
					}
					return _0024STATIC_0024get_图标_002400128085_0024m;
				}
			}

			protected 本程序()
			{
			}

			/// <summary>
			/// 重启本程序
			/// </summary>
			public static void 重启()
			{
				Directory.SetCurrentDirectory(路径);
				打开程序("C:\\Windows\\System32\\cmd.exe", "/c taskkill /f /pid " + Conversions.ToString(PID) + " & start \"\" " + 文本.引(文件名 + ".exe"), ProcessWindowStyle.Hidden);
			}

			/// <summary>
			/// 退出本程序
			/// </summary>
			public static void 退出()
			{
				进程.Kill();
			}

			/// <summary>
			/// 检查本程序是否真的在运行中
			/// </summary>
			public static bool 真的运行中()
			{
				return 类型.非空(进程) && Operators.CompareString(进程.ProcessName, "devenv", TextCompare: false) != 0;
			}
		}

		public delegate void WL出错EventHandler(string 内容);

		/// <summary>
		/// 一个用窗体线程的 Timer
		/// </summary>
		public class 计时器 : IDisposable
		{
			[CompilerGenerated]
			internal sealed class _Closure_0024__11_002D0
			{
				public EventHandler _0024VB_0024Local_事件;

				public 计时器 _0024VB_0024Me;

				[DebuggerHidden]
				internal void _Lambda_0024__R1(object a0, EventArgs a1)
				{
					_Lambda_0024__0();
				}

				internal void _Lambda_0024__0()
				{
					_0024VB_0024Local_事件(null, null);
					if ((long)_0024VB_0024Me.工作次数 > 0L)
					{
						ref uint ts = ref _0024VB_0024Me.ts;
						checked
						{
							ts = (uint)(unchecked((long)ts) + 1L);
							if (_0024VB_0024Me.ts >= _0024VB_0024Me.工作次数)
							{
								_0024VB_0024Me.启用 = false;
								_0024VB_0024Me.ts = 0u;
							}
						}
					}
				}
			}

			private System.Windows.Forms.Timer tm;

			private uint ts;

			private uint worktimes;

			/// <summary>
			/// 间隔，也就是 Interval，单位为毫秒，如果设置的比1还小就会直接停止工作，并且需要手动重启
			/// </summary>
			public int 间隔
			{
				get
				{
					return tm.Interval;
				}
				set
				{
					if (value < 1)
					{
						tm.Enabled = false;
					}
					tm.Interval = value;
				}
			}

			/// <summary>
			/// 也就是 Enabled
			/// </summary>
			public bool 启用
			{
				get
				{
					return tm.Enabled;
				}
				set
				{
					tm.Enabled = value;
				}
			}

			/// <summary>
			/// 工作几次后会自动停止
			/// </summary>
			public uint 工作次数
			{
				get
				{
					return worktimes;
				}
				set
				{
					if (worktimes != value)
					{
						worktimes = value;
						ts = 0u;
					}
				}
			}

			/// <summary>
			/// 新建一个用窗体线程的 Timer，不会自动启动
			/// </summary>
			public 计时器(int 间隔, EventHandler 事件 = null)
			{
				tm = new System.Windows.Forms.Timer();
				tm.Enabled = false;
				this.间隔 = 间隔;
				新增事件(事件);
				ts = 0u;
				worktimes = 0u;
			}

			/// <summary>
			/// 释放 Timer 的资源
			/// </summary>
			public void Dispose()
			{
				tm.Dispose();
				Finalize();
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}

			/// <summary>
			/// 新增一个计时器时间到的时候会进行的事件，无法删除老事件，只能删除本计时器后重新做一个
			/// </summary>
			public void 新增事件(EventHandler 事件)
			{
				if (类型.非空(事件))
				{
					_Closure_0024__11_002D0 CS_0024_003C_003E8__locals0;
					tm.Tick += delegate
					{
						CS_0024_003C_003E8__locals0._Lambda_0024__0();
					};
				}
			}
		}

		/// <summary>
		/// 使用系统的文件对话框选取文件文件夹
		/// </summary>
		public sealed class 系统文件对话框
		{
			private static void SetFolder(FileDialog x)
			{
				FileDialog fileDialog = x;
				fileDialog.AddExtension = true;
				fileDialog.CheckPathExists = true;
				fileDialog.SupportMultiDottedExtensions = true;
				fileDialog.DereferenceLinks = true;
				fileDialog.SupportMultiDottedExtensions = true;
				fileDialog.ValidateNames = true;
				fileDialog = null;
			}

			/// <summary>
			/// 打开窗口并选择一个文件，过滤写法是 Text files (*.txt)|*.txt|All files (*.*)|*.*
			/// </summary>
			public static string 打开文件(string 过滤 = "")
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				OpenFileDialog openFileDialog2 = openFileDialog;
				SetFolder(openFileDialog);
				openFileDialog2.CheckFileExists = true;
				openFileDialog2.Multiselect = false;
				if (类型.非空(过滤))
				{
					openFileDialog2.Filter = 过滤;
				}
				openFileDialog2.ShowDialog();
				return openFileDialog2.FileName;
			}

			/// <summary>
			/// 打开窗口并选择多个文件，过滤写法是 Text files (*.txt)|*.txt|All files (*.*)|*.*
			/// </summary>
			public static string[] 打开多个文件(string 过滤 = "")
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				OpenFileDialog openFileDialog2 = openFileDialog;
				SetFolder(openFileDialog);
				openFileDialog2.CheckFileExists = true;
				openFileDialog2.Multiselect = true;
				if (类型.非空(过滤))
				{
					openFileDialog2.Filter = 过滤;
				}
				openFileDialog2.ShowDialog();
				return openFileDialog2.FileNames;
			}

			/// <summary>
			/// 打开窗口并选择单个文件夹
			/// </summary>
			public static string 打开文件夹()
			{
				FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
				FolderBrowserDialog folderBrowserDialog2 = folderBrowserDialog;
				folderBrowserDialog2.ShowDialog();
				return 文件.路径标准化(folderBrowserDialog2.SelectedPath);
			}

			/// <summary>
			/// 打开窗口并保存单个文件，过滤写法是 Text files (*.txt)|*.txt|All files (*.*)|*.*
			/// </summary>
			public static string 保存文件(string 过滤 = "")
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				SaveFileDialog saveFileDialog2 = saveFileDialog;
				SetFolder(saveFileDialog);
				saveFileDialog2.CheckFileExists = false;
				if (类型.非空(过滤))
				{
					saveFileDialog2.Filter = 过滤;
				}
				saveFileDialog2.ShowDialog();
				return saveFileDialog2.FileName;
			}
		}

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static WL出错EventHandler WL出错Event;

		private static double _0024STATIC_0024计时_0024001_0024m;

		private static StaticLocalInitFlag _0024STATIC_0024计时_0024001_0024m_0024Init;

		/// <summary>
		/// 出错的时候会呼叫这个event
		/// </summary>
		public static event WL出错EventHandler WL出错
		{
			[CompilerGenerated]
			add
			{
				WL出错EventHandler wL出错EventHandler = WL出错Event;
				WL出错EventHandler wL出错EventHandler2;
				do
				{
					wL出错EventHandler2 = wL出错EventHandler;
					WL出错EventHandler value2 = (WL出错EventHandler)Delegate.Combine(wL出错EventHandler2, value);
					wL出错EventHandler = Interlocked.CompareExchange(ref WL出错Event, value2, wL出错EventHandler2);
				}
				while ((object)wL出错EventHandler != wL出错EventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				WL出错EventHandler wL出错EventHandler = WL出错Event;
				WL出错EventHandler wL出错EventHandler2;
				do
				{
					wL出错EventHandler2 = wL出错EventHandler;
					WL出错EventHandler value2 = (WL出错EventHandler)Delegate.Remove(wL出错EventHandler2, value);
					wL出错EventHandler = Interlocked.CompareExchange(ref WL出错Event, value2, wL出错EventHandler2);
				}
				while ((object)wL出错EventHandler != wL出错EventHandler2);
			}
		}

		/// <summary>
		/// Debug.Print 输出信息，也可以 Console.Writeline
		/// </summary>
		public static void 输出(params object[] 内容)
		{
			string text = "";
			string text2 = "<empty string>";
			for (int i = 0; i < 内容.Length; i = checked(i + 1))
			{
				object objectValue = RuntimeHelpers.GetObjectValue(内容[i]);
				string text3;
				if (Information.IsNothing(RuntimeHelpers.GetObjectValue(objectValue)))
				{
					text3 = "<nothing>";
				}
				else
				{
					text3 = objectValue.ToString();
					if (text3.Length < 1)
					{
						text3 = text2;
					}
				}
				text = text + text3 + "    ";
			}
			text = 文本.去右(text, 4u);
			if (text.Length < 1)
			{
				text = text2;
			}
			Debug.Print(text);
			if (本程序.控制台输出)
			{
				Console.WriteLine(text);
			}
		}

		/// <summary>
		/// 把线程强制中断
		/// </summary>
		public static void 中断线程(params Thread[] 线程)
		{
			foreach (Thread thread in 线程)
			{
				if (!Information.IsNothing(thread) && thread.IsAlive)
				{
					thread.Abort();
				}
			}
		}

		/// <summary>
		/// 打开一个程序，超时选项只有在等到运行结束为True的时候才会工作
		/// </summary>
		public static void 打开程序(string 文件, string 参数 = "", ProcessWindowStyle 窗口样式 = ProcessWindowStyle.Normal, bool 管理员权限运行 = false, bool 等到运行结束 = false, uint 超时 = 0u)
		{
			if (文件.Length < 4)
			{
				出错("打开程序，文件名不对", 文件);
			}
			else if (文件.文件夹存在(文件) || 文本.头(文件.ToLower(), "https://", "http://"))
			{
				try
				{
					Process.Start(文件);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					出错(ex2);
					ProjectData.ClearProjectError();
				}
			}
			else if (文件.文件存在(文件))
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				ProcessStartInfo processStartInfo2 = processStartInfo;
				processStartInfo2.FileName = 文件;
				processStartInfo2.Arguments = 参数;
				processStartInfo2.WindowStyle = 窗口样式;
				if (管理员权限运行)
				{
					processStartInfo2.Verb = "runas";
				}
				processStartInfo2 = null;
				try
				{
					Process process = Process.Start(processStartInfo);
					if (等到运行结束)
					{
						float num = 0f;
						while (!process.HasExited)
						{
							Thread.Sleep(100);
							num = (float)((double)num + 0.1);
							if ((long)超时 > 0L && num > (float)(double)超时)
							{
								process.Kill();
							}
						}
					}
					else
					{
						process.Dispose();
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					出错(ex4);
					ProjectData.ClearProjectError();
				}
			}
			else
			{
				出错("打开程序，文件不存在", 文件);
			}
		}

		/// <summary>
		/// 判断该名字的程序是否在运行中，无需.exe
		/// </summary>
		public static bool 程序运行中(string 程序名)
		{
			if (程序名.Length < 1)
			{
				return false;
			}
			return Process.GetProcessesByName(程序名).Length > 0;
		}

		/// <summary>
		/// 出错的时候会call这个sub
		/// </summary>
		public static void 出错(params object[] 内容)
		{
			string text = "WL出错：";
			for (int i = 0; i < 内容.Length; i = checked(i + 1))
			{
				object obj = RuntimeHelpers.GetObjectValue(内容[i]);
				string 文本;
				if (obj.GetType().ToString().ToLower()
					.Contains("exception"))
				{
					Exception ex = (Exception)obj;
					文本 = ex.TargetSite.Name + "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
				}
				else
				{
					if (Information.IsNothing(RuntimeHelpers.GetObjectValue(obj)))
					{
						obj = "<nothing>";
					}
					文本 = 文本.替换(obj.ToString(), "\r", "[CR]", "\n", "[LF]");
					string text2 = 文本.文本标准化(ref 文本);
					if (Operators.CompareString(text2, 文本, TextCompare: false) != 0)
					{
						文本 = "标准化后：" + text2;
					}
				}
				if (文本.Length < 1)
				{
					文本 = "<empty string>";
				}
				text = text + 文本 + "\r\n";
			}
			WL出错Event?.Invoke(text);
			输出(text);
		}

		/// <summary>
		/// 输出一条计时，包括距离上次计时的间距
		/// </summary>
		public static void 计时()
		{
			if (_0024STATIC_0024计时_0024001_0024m_0024Init == null)
			{
				Interlocked.CompareExchange(ref _0024STATIC_0024计时_0024001_0024m_0024Init, new StaticLocalInitFlag(), null);
			}
			bool lockTaken = false;
			try
			{
				Monitor.Enter(_0024STATIC_0024计时_0024001_0024m_0024Init, ref lockTaken);
				if (_0024STATIC_0024计时_0024001_0024m_0024Init.State == 0)
				{
					_0024STATIC_0024计时_0024001_0024m_0024Init.State = 2;
					_0024STATIC_0024计时_0024001_0024m = DateAndTime.Timer;
				}
				else if (_0024STATIC_0024计时_0024001_0024m_0024Init.State == 2)
				{
					throw new IncompleteInitialization();
				}
			}
			finally
			{
				_0024STATIC_0024计时_0024001_0024m_0024Init.State = 1;
				if (lockTaken)
				{
					Monitor.Exit(_0024STATIC_0024计时_0024001_0024m_0024Init);
				}
			}
			double timer = DateAndTime.Timer;
			输出(Math.Round(timer - _0024STATIC_0024计时_0024001_0024m, 3), 时间.时间格式化(DateAndTime.Now, "h:m:s"));
			_0024STATIC_0024计时_0024001_0024m = timer;
		}

		/// <summary>
		/// 运行Powershell脚本，并返回运行结果，只能是全自动脚本，不然会卡住
		/// </summary>
		public static string PowerShell运行脚本(string 脚本)
		{
			string 文本 = "";
			if (脚本.Length > 0)
			{
				try
				{
					PowerShell powerShell = PowerShell.Create();
					powerShell.AddScript(脚本);
					Collection<PSObject> collection = powerShell.Invoke();
					foreach (PSObject item in collection)
					{
						if (!Information.IsNothing(item))
						{
							文本 = 文本 + item.ToString() + "\r\n";
						}
					}
					powerShell.Dispose();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					出错(ex2);
					ProjectData.ClearProjectError();
				}
			}
			return 文本.文本标准化(ref 文本);
		}
	}
}
