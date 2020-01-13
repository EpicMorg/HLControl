using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using WL.My;

namespace WL.基础
{
	/// <summary>
	/// 和Windows系统相关操作的模块
	/// </summary>
	[StandardModule]
	public sealed class 系统
	{
		/// <summary>
		/// 从剪贴板内获得字符串或图片
		/// </summary>
		public sealed class 剪贴板
		{
			/// <summary>
			/// 检查剪贴板内是否是文本
			/// </summary>
			public static bool 有文本
			{
				get
				{
					try
					{
						return MyProject.Computer.Clipboard.ContainsText();
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						程序.出错(ex2);
						bool result = false;
						ProjectData.ClearProjectError();
						return result;
					}
				}
			}

			/// <summary>
			/// 检查剪贴板内是否是图片
			/// </summary>
			public static bool 有图片
			{
				get
				{
					try
					{
						return MyProject.Computer.Clipboard.ContainsImage();
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						程序.出错(ex2);
						bool result = false;
						ProjectData.ClearProjectError();
						return result;
					}
				}
			}

			/// <summary>
			/// 检查剪贴板内是否是文件列表
			/// </summary>
			public static bool 有文件列表
			{
				get
				{
					try
					{
						return MyProject.Computer.Clipboard.ContainsFileDropList();
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						程序.出错(ex2);
						bool result = false;
						ProjectData.ClearProjectError();
						return result;
					}
				}
			}

			/// <summary>
			/// 获取或设置剪贴板的字符串
			/// </summary>
			public static string 文本
			{
				get
				{
					string result = "";
					try
					{
						if (MyProject.Computer.Clipboard.ContainsText())
						{
							result = MyProject.Computer.Clipboard.GetText();
						}
						return result;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						string result2 = "";
						ProjectData.ClearProjectError();
						return result2;
					}
				}
				set
				{
					if (类型.为空(value))
					{
						清空();
					}
					else
					{
						try
						{
							MyProject.Computer.Clipboard.SetText(value);
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
			}

			/// <summary>
			/// 获取或设置剪贴板的图片
			/// </summary>
			public static Bitmap 图片
			{
				get
				{
					Image image = null;
					try
					{
						if (MyProject.Computer.Clipboard.ContainsImage())
						{
							image = MyProject.Computer.Clipboard.GetImage();
						}
						return (Bitmap)image;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						程序.出错(ex2);
						Bitmap result = null;
						ProjectData.ClearProjectError();
						return result;
					}
				}
				set
				{
					if (类型.为空(value))
					{
						清空();
					}
					else
					{
						try
						{
							MyProject.Computer.Clipboard.SetImage(value);
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
			}

			/// <summary>
			/// 获取剪贴板里存储的 Windows Explorer 的复制文件列表
			/// </summary>
			public static List<string> 文件列表
			{
				get
				{
					List<string> list = new List<string>();
					try
					{
						if (MyProject.Computer.Clipboard.ContainsFileDropList())
						{
							StringEnumerator enumerator = MyProject.Computer.Clipboard.GetFileDropList().GetEnumerator();
							while (enumerator.MoveNext())
							{
								string current = enumerator.Current;
								if (current.Length > 2 && !list.Contains(current))
								{
									list.Add(current);
								}
							}
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						程序.出错(ex2);
						ProjectData.ClearProjectError();
					}
					return list;
				}
			}

			protected 剪贴板()
			{
			}

			/// <summary>
			/// 清空剪贴板的内容
			/// </summary>
			public static void 清空()
			{
				try
				{
					MyProject.Computer.Clipboard.Clear();
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
		/// 获取Windows系统的一些软硬件信息
		/// </summary>
		public sealed class 系统信息
		{
			private static ulong gpuRAM = 0uL;

			private static string _0024STATIC_0024get_CPU型号_002400E_0024s;

			private static StaticLocalInitFlag _0024STATIC_0024get_CPU型号_002400E_0024s_0024Init;

			private static uint _0024STATIC_0024get_CPU频率_0024009_0024s;

			private static StaticLocalInitFlag _0024STATIC_0024get_CPU频率_0024009_0024s_0024Init;

			private static uint _0024STATIC_0024get_CPU核心数量_0024009_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_CPU核心数量_0024009_0024m_0024Init;

			private static string _0024STATIC_0024get_CPU类型_002400E_0024s;

			private static StaticLocalInitFlag _0024STATIC_0024get_CPU类型_002400E_0024s_0024Init;

			private static Size _0024STATIC_0024get_屏幕分辨率_00240011815_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_屏幕分辨率_00240011815_0024m_0024Init;

			private static float _0024STATIC_0024get_DPI_002400C_0024d;

			private static StaticLocalInitFlag _0024STATIC_0024get_DPI_002400C_0024d_0024Init;

			private static string _0024STATIC_0024get_电脑名_002400E_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_电脑名_002400E_0024m_0024Init;

			private static string _0024STATIC_0024get_用户名_002400E_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_用户名_002400E_0024m_0024Init;

			private static ulong _0024STATIC_0024get_内存大小_002400B_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_内存大小_002400B_0024m_0024Init;

			private static string _0024STATIC_0024get_显卡型号_002400E_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_显卡型号_002400E_0024m_0024Init;

			private static string _0024STATIC_0024get_主板型号_002400E_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_主板型号_002400E_0024m_0024Init;

			private static string _0024STATIC_0024get_主板品牌_002400E_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_主板品牌_002400E_0024m_0024Init;

			private static string _0024STATIC_0024get_BIOS类型_002400E_0024m;

			private static StaticLocalInitFlag _0024STATIC_0024get_BIOS类型_002400E_0024m_0024Init;

			/// <summary>
			/// 获取电脑CPU的名字字符串
			/// </summary>
			public static string CPU型号
			{
				get
				{
					if (_0024STATIC_0024get_CPU型号_002400E_0024s_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_CPU型号_002400E_0024s_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_CPU型号_002400E_0024s_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_CPU型号_002400E_0024s_0024Init.State == 0)
						{
							_0024STATIC_0024get_CPU型号_002400E_0024s_0024Init.State = 2;
							_0024STATIC_0024get_CPU型号_002400E_0024s = 系统信息.get_获取注册表("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", "ProcessorNameString", "Unknown").Trim();
						}
						else if (_0024STATIC_0024get_CPU型号_002400E_0024s_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_CPU型号_002400E_0024s_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_CPU型号_002400E_0024s_0024Init);
						}
					}
					return _0024STATIC_0024get_CPU型号_002400E_0024s;
				}
			}

			/// <summary>
			/// 返回电脑CPU的频率，单位为MHZ
			/// </summary>
			public static uint CPU频率
			{
				get
				{
					if (_0024STATIC_0024get_CPU频率_0024009_0024s_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_CPU频率_0024009_0024s_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_CPU频率_0024009_0024s_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_CPU频率_0024009_0024s_0024Init.State == 0)
						{
							_0024STATIC_0024get_CPU频率_0024009_0024s_0024Init.State = 2;
							_0024STATIC_0024get_CPU频率_0024009_0024s = checked((uint)Math.Round(Conversion.Val(系统信息.get_获取注册表("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", "~MHz", "0"))));
						}
						else if (_0024STATIC_0024get_CPU频率_0024009_0024s_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_CPU频率_0024009_0024s_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_CPU频率_0024009_0024s_0024Init);
						}
					}
					return _0024STATIC_0024get_CPU频率_0024009_0024s;
				}
			}

			/// <summary>
			/// 返回CPU的核心数量
			/// </summary>
			public static uint CPU核心数量
			{
				get
				{
					if (_0024STATIC_0024get_CPU核心数量_0024009_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_CPU核心数量_0024009_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_CPU核心数量_0024009_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_CPU核心数量_0024009_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_CPU核心数量_0024009_0024m_0024Init.State = 2;
							_0024STATIC_0024get_CPU核心数量_0024009_0024m = checked((uint)Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor", writable: false).SubKeyCount);
						}
						else if (_0024STATIC_0024get_CPU核心数量_0024009_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_CPU核心数量_0024009_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_CPU核心数量_0024009_0024m_0024Init);
						}
					}
					return _0024STATIC_0024get_CPU核心数量_0024009_0024m;
				}
			}

			/// <summary>
			/// 返回CPU的类型
			/// </summary>
			public static string CPU类型
			{
				get
				{
					if (_0024STATIC_0024get_CPU类型_002400E_0024s_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_CPU类型_002400E_0024s_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_CPU类型_002400E_0024s_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_CPU类型_002400E_0024s_0024Init.State == 0)
						{
							_0024STATIC_0024get_CPU类型_002400E_0024s_0024Init.State = 2;
							_0024STATIC_0024get_CPU类型_002400E_0024s = 系统信息.get_获取注册表("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", "Identifier", "Unknown").Trim();
						}
						else if (_0024STATIC_0024get_CPU类型_002400E_0024s_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_CPU类型_002400E_0024s_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_CPU类型_002400E_0024s_0024Init);
						}
					}
					return _0024STATIC_0024get_CPU类型_002400E_0024s;
				}
			}

			/// <summary>
			/// 获取电脑的显示器的分辨率，单位是像素
			/// </summary>
			public static Size 屏幕分辨率
			{
				get
				{
					if (_0024STATIC_0024get_屏幕分辨率_00240011815_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_屏幕分辨率_00240011815_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_屏幕分辨率_00240011815_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_屏幕分辨率_00240011815_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_屏幕分辨率_00240011815_0024m_0024Init.State = 2;
							_0024STATIC_0024get_屏幕分辨率_00240011815_0024m = MyProject.Computer.Screen.Bounds.Size;
						}
						else if (_0024STATIC_0024get_屏幕分辨率_00240011815_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_屏幕分辨率_00240011815_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_屏幕分辨率_00240011815_0024m_0024Init);
						}
					}
					return _0024STATIC_0024get_屏幕分辨率_00240011815_0024m;
				}
			}

			/// <summary>
			/// 获取电脑的显示DPI，并强制转换为0.25的倍数
			/// </summary>
			public static float DPI
			{
				get
				{
					if (_0024STATIC_0024get_DPI_002400C_0024d_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_DPI_002400C_0024d_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_DPI_002400C_0024d_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_DPI_002400C_0024d_0024Init.State == 0)
						{
							_0024STATIC_0024get_DPI_002400C_0024d_0024Init.State = 2;
							_0024STATIC_0024get_DPI_002400C_0024d = 0f;
						}
						else if (_0024STATIC_0024get_DPI_002400C_0024d_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_DPI_002400C_0024d_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_DPI_002400C_0024d_0024Init);
						}
					}
					checked
					{
						if (_0024STATIC_0024get_DPI_002400C_0024d == 0f)
						{
							int num = (int)Math.Round(Conversion.Val(系统信息.get_获取注册表("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "AppliedDPI", "100")));
							if (num < 100)
							{
								num = 100;
							}
							int num2 = 50;
							do
							{
								if ((double)Math.Abs(num2 - num) < 12.5)
								{
									num = num2;
									break;
								}
								num2 += 25;
							}
							while (num2 <= 2000);
							_0024STATIC_0024get_DPI_002400C_0024d = (float)((double)num / 100.0);
						}
						return _0024STATIC_0024get_DPI_002400C_0024d;
					}
				}
			}

			/// <summary>
			/// 获取电脑的名字
			/// </summary>
			public static string 电脑名
			{
				get
				{
					if (_0024STATIC_0024get_电脑名_002400E_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_电脑名_002400E_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_电脑名_002400E_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_电脑名_002400E_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_电脑名_002400E_0024m_0024Init.State = 2;
							_0024STATIC_0024get_电脑名_002400E_0024m = MyProject.Computer.Name;
						}
						else if (_0024STATIC_0024get_电脑名_002400E_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_电脑名_002400E_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_电脑名_002400E_0024m_0024Init);
						}
					}
					return _0024STATIC_0024get_电脑名_002400E_0024m;
				}
			}

			/// <summary>
			/// 返回当前的系统用户的用户名
			/// </summary>
			public static string 用户名
			{
				get
				{
					if (_0024STATIC_0024get_用户名_002400E_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_用户名_002400E_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_用户名_002400E_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_用户名_002400E_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_用户名_002400E_0024m_0024Init.State = 2;
							_0024STATIC_0024get_用户名_002400E_0024m = 文本.提取之后(MyProject.User.Name, "\\");
						}
						else if (_0024STATIC_0024get_用户名_002400E_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_用户名_002400E_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_用户名_002400E_0024m_0024Init);
						}
					}
					return _0024STATIC_0024get_用户名_002400E_0024m;
				}
			}

			/// <summary>
			/// 内存的大小，只包括物理内存，单位为MB
			/// </summary>
			public static ulong 内存大小
			{
				get
				{
					if (_0024STATIC_0024get_内存大小_002400B_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_内存大小_002400B_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_内存大小_002400B_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_内存大小_002400B_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_内存大小_002400B_0024m_0024Init.State = 2;
							_0024STATIC_0024get_内存大小_002400B_0024m = checked((ulong)Math.Round((double)MyProject.Computer.Info.TotalPhysicalMemory / 1024.0 / 1024.0));
						}
						else if (_0024STATIC_0024get_内存大小_002400B_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_内存大小_002400B_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_内存大小_002400B_0024m_0024Init);
						}
					}
					return _0024STATIC_0024get_内存大小_002400B_0024m;
				}
			}

			/// <summary>
			/// 返回电脑的显卡的型号
			/// </summary>
			public static string 显卡型号
			{
				get
				{
					if (_0024STATIC_0024get_显卡型号_002400E_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_显卡型号_002400E_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_显卡型号_002400E_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_显卡型号_002400E_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_显卡型号_002400E_0024m_0024Init.State = 2;
							_0024STATIC_0024get_显卡型号_002400E_0024m = "";
						}
						else if (_0024STATIC_0024get_显卡型号_002400E_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_显卡型号_002400E_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_显卡型号_002400E_0024m_0024Init);
						}
					}
					if (_0024STATIC_0024get_显卡型号_002400E_0024m.Length < 1)
					{
						RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Video\\", writable: false);
						RegistryKey registryKey2 = null;
						if (!Information.IsNothing(registryKey))
						{
							string[] subKeyNames = registryKey.GetSubKeyNames();
							foreach (string name in subKeyNames)
							{
								registryKey2 = registryKey.OpenSubKey(name);
								if (Information.IsNothing(registryKey2))
								{
									continue;
								}
								registryKey2 = registryKey2.OpenSubKey("0000");
								if (!Information.IsNothing(registryKey2))
								{
									object objectValue = RuntimeHelpers.GetObjectValue(registryKey2.GetValue("DriverDesc", null));
									if (!Information.IsNothing(RuntimeHelpers.GetObjectValue(objectValue)) && objectValue.GetType() == typeof(string))
									{
										string 文本 = objectValue.ToString();
										_0024STATIC_0024get_显卡型号_002400E_0024m = 文本.文本标准化(ref 文本).Trim();
									}
									if (_0024STATIC_0024get_显卡型号_002400E_0024m.Length > 0)
									{
										gpuRAM = checked((ulong)Math.Round(Math.Abs(Conversion.Val(registryKey2.GetValue("HardwareInformation.MemorySize", 0).ToString())) / 1024.0 / 1024.0));
										break;
									}
								}
							}
						}
					}
					if (_0024STATIC_0024get_显卡型号_002400E_0024m.Length < 1)
					{
						_0024STATIC_0024get_显卡型号_002400E_0024m = "未知";
					}
					return _0024STATIC_0024get_显卡型号_002400E_0024m;
				}
			}

			/// <summary>
			/// 返回电脑的显卡的内存，单位为MB
			/// </summary>
			public static ulong 显卡内存大小
			{
				get
				{
					if (decimal.Compare(new decimal(gpuRAM), 1m) < 0)
					{
						string 显卡型号 = 系统信息.显卡型号;
					}
					return gpuRAM;
				}
			}

			/// <summary>
			/// 返回电脑的主板的型号
			/// </summary>
			public static string 主板型号
			{
				get
				{
					if (_0024STATIC_0024get_主板型号_002400E_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_主板型号_002400E_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_主板型号_002400E_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_主板型号_002400E_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_主板型号_002400E_0024m_0024Init.State = 2;
							_0024STATIC_0024get_主板型号_002400E_0024m = "";
						}
						else if (_0024STATIC_0024get_主板型号_002400E_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_主板型号_002400E_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_主板型号_002400E_0024m_0024Init);
						}
					}
					if (_0024STATIC_0024get_主板型号_002400E_0024m.Length < 1)
					{
						_0024STATIC_0024get_主板型号_002400E_0024m = 系统信息.get_获取注册表("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\BIOS", "BaseBoardProduct", "Unknown").Trim();
					}
					return _0024STATIC_0024get_主板型号_002400E_0024m;
				}
			}

			/// <summary>
			/// 返回电脑的主板生产厂家的名字
			/// </summary>
			public static string 主板品牌
			{
				get
				{
					if (_0024STATIC_0024get_主板品牌_002400E_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_主板品牌_002400E_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_主板品牌_002400E_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_主板品牌_002400E_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_主板品牌_002400E_0024m_0024Init.State = 2;
							_0024STATIC_0024get_主板品牌_002400E_0024m = "";
						}
						else if (_0024STATIC_0024get_主板品牌_002400E_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_主板品牌_002400E_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_主板品牌_002400E_0024m_0024Init);
						}
					}
					if (_0024STATIC_0024get_主板品牌_002400E_0024m.Length < 1)
					{
						_0024STATIC_0024get_主板品牌_002400E_0024m = 系统信息.get_获取注册表("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\BIOS", "BaseBoardManufacturer", "Unknown").Trim();
					}
					return _0024STATIC_0024get_主板品牌_002400E_0024m;
				}
			}

			/// <summary>
			/// 返回电脑的主板生产厂家的名字
			/// </summary>
			public static string BIOS类型
			{
				get
				{
					if (_0024STATIC_0024get_BIOS类型_002400E_0024m_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024get_BIOS类型_002400E_0024m_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024get_BIOS类型_002400E_0024m_0024Init, ref lockTaken);
						if (_0024STATIC_0024get_BIOS类型_002400E_0024m_0024Init.State == 0)
						{
							_0024STATIC_0024get_BIOS类型_002400E_0024m_0024Init.State = 2;
							_0024STATIC_0024get_BIOS类型_002400E_0024m = "";
						}
						else if (_0024STATIC_0024get_BIOS类型_002400E_0024m_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024get_BIOS类型_002400E_0024m_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024get_BIOS类型_002400E_0024m_0024Init);
						}
					}
					if (_0024STATIC_0024get_BIOS类型_002400E_0024m.Length < 1)
					{
						_0024STATIC_0024get_BIOS类型_002400E_0024m = 系统信息.get_获取注册表("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\BIOS", "BIOSVendor", "Unknown").Trim();
					}
					return _0024STATIC_0024get_BIOS类型_002400E_0024m;
				}
			}

			/// <summary>
			/// 是否存在任意可用本地或互联网的网络连接
			/// </summary>
			public static bool 存在网络 => MyProject.Computer.Network.IsAvailable;

			/// <summary>
			/// 获取本电脑的首选ipv4地址
			/// </summary>
			public static string IPv4地址
			{
				get
				{
					if (存在网络)
					{
						string text = 程序.PowerShell运行脚本("ipconfig");
						if (text.Length > 100)
						{
							text = text.ToLower();
							text = 文本.提取最之前(文本.提取之后(text, "windows", "ipv4", ":"), "\r\n").Trim();
						}
						return text;
					}
					return "";
				}
			}

			/// <summary>
			/// 获取本地的首选ipv4地址
			/// </summary>
			public static string IPv6地址
			{
				get
				{
					if (存在网络)
					{
						string text = 程序.PowerShell运行脚本("ipconfig");
						if (text.Length > 100)
						{
							text = text.ToLower();
							text = 文本.提取最之前(文本.提取之后(text, "windows", "ipv6", ":"), "\r\n").Trim();
						}
						return text;
					}
					return "";
				}
			}

			/// <summary>
			/// 应用程序的主题是否是暗色模式，只针对windows10，一般为false
			/// </summary>
			/// <returns></returns>
			public static bool 暗色应用模式 => Conversion.Val(系统信息.get_获取注册表("HKEY_CURRENT_USER\\Software\\Microsoft9\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", "1")) == 0.0;

			/// <summary>
			/// 系统的主题是否是暗色模式，只针对windows10，一般为false
			/// </summary>
			/// <returns></returns>
			public static bool 暗色系统模式 => Conversion.Val(系统信息.get_获取注册表("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "SystemUsesLightTheme", "1")) == 0.0;

			/// <summary>
			/// 从注册表里读取值
			/// </summary>
			public static string 获取注册表
			{
				get
				{
					try
					{
						return Registry.GetValue(键, 值名, 默认).ToString();
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						程序.出错(ex2);
						ProjectData.ClearProjectError();
						return 默认;
					}
				}
			}

			protected 系统信息()
			{
			}
		}
	}
}
