using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace WL.基础
{
	/// <summary>
	/// WSave配置文件的模块
	/// </summary>
	[StandardModule]
	public sealed class WSave配置文件
	{
		/// <summary>
		/// WSave配置文件
		/// </summary>
		public class WSave
		{
			[CompilerGenerated]
			internal sealed class _Closure_0024__25_002D0
			{
				public Control _0024VB_0024Local_控件;

				public 控件值类型 _0024VB_0024Local_值;

				public string _0024VB_0024Local_s;

				public WSave _0024VB_0024Me;
			}

			[CompilerGenerated]
			internal sealed class _Closure_0024__25_002D1
			{
				public int _0024VB_0024Local_tp;

				public _Closure_0024__25_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

				[DebuggerHidden]
				internal void _Lambda_0024__R1(object a0, FormClosingEventArgs a1)
				{
					_Lambda_0024__0();
				}

				internal void _Lambda_0024__0()
				{
					string text = Versioned.CallByName(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_控件, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_值.ToString(), CallType.Get).ToString();
					switch (_0024VB_0024Local_tp)
					{
					case 0:
						_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Me.set_字符串(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_s, "", text);
						break;
					case 1:
						_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Me.set_真假(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_s, 默认: false, text.Length == 4);
						break;
					case 2:
						_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Me.set_数字(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_s, 0.0, Conversion.Val(text));
						break;
					}
				}
			}

			private 文本.走過去加密 En;

			private string key;

			private Dictionary<string, string> Di;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string _本地文件;

			/// <summary>
			/// 本地文件的位置，通常为 .wsave 后缀
			/// </summary>
			public string 本地文件
			{
				get;
				set;
			}

			/// <summary>
			/// 从配置文件里读取或写入字符串
			/// </summary>
			public string 字符串
			{
				get
				{
					文本.文本标准化(ref 默认);
					名字 = 名字.ToLower().Trim();
					if (名字.Length > 0 && Di.ContainsKey(名字))
					{
						return Di[名字];
					}
					return 默认;
				}
				set
				{
					if (名字.Length >= 1)
					{
						名字 = 名字.ToLower().Trim();
						if (Di.ContainsKey(名字))
						{
							Di.Remove(名字);
						}
						if (value.Length > 0)
						{
							Di.Add(名字, value);
						}
					}
				}
			}

			/// <summary>
			/// 从配置文件里检查是否有这个名字的值
			/// </summary>
			public bool 非空 => this.get_字符串(名字, "").Length > 0;

			/// <summary>
			/// 从配置文件里读取或写入数字
			/// </summary>
			public double 数字
			{
				get
				{
					string text = this.get_字符串(名字, "");
					if (text.Length > 0)
					{
						return Conversion.Val(text);
					}
					return 默认;
				}
				set
				{
					if (value != 0.0)
					{
						this.set_字符串(名字, "", value.ToString());
					}
					else
					{
						this.set_字符串(名字, "", "");
					}
				}
			}

			/// <summary>
			/// 从配置文件里读取或写入真假
			/// </summary>
			public bool 真假
			{
				get
				{
					string text = this.get_字符串(名字, "");
					if (text.Length == 4)
					{
						return true;
					}
					return 默认;
				}
				set
				{
					this.set_字符串(名字, "", Conversions.ToString(Interaction.IIf(value, "True", "")));
				}
			}

			/// <summary>
			/// 从配置文件里读取或写入日期
			/// </summary>
			public DateTime 日期
			{
				get
				{
					string text = 文本.仅数字(this.get_字符串(名字, ""));
					if (text.Length > 1)
					{
						return 时间.时间戳转出(checked((ulong)Math.Round(Conversion.Val(text))));
					}
					return 默认;
				}
				set
				{
					this.set_字符串(名字, "", Conversions.ToString(时间.转时间戳(value)));
				}
			}

			/// <summary>
			/// 新建一个WSave配置文件类
			/// </summary>
			public WSave(string 本地文件 = "")
			{
				key = "戈登走過去只是个平凡的人";
				En = new 文本.走過去加密(key);
				Di = new Dictionary<string, string>();
				if (本地文件.Length > 4)
				{
					this.本地文件 = 本地文件;
					从文件读取();
				}
			}

			/// <summary>
			/// 测试该外部密钥是否和内部密钥一样
			/// </summary>
			public bool 密钥冲突(string 测试)
			{
				文本.走過去加密 走過去加密 = new 文本.走過去加密(测试);
				return 走過去加密.密钥.Contains(En.密钥) || En.密钥.StartsWith(走過去加密.密钥) || En.密钥.EndsWith(走過去加密.密钥);
			}

			/// <summary>
			/// 清空，并从本地读取文件
			/// </summary>
			public void 从文件读取()
			{
				Di.Clear();
				if ((long)文件.文件大小Byte(本地文件) > 5L)
				{
					StreamReader streamReader = null;
					try
					{
						streamReader = new StreamReader(File.OpenRead(本地文件));
						streamReader.ReadLine();
						do
						{
							string text = En.解密为字符串(streamReader.ReadLine()).Trim().ToLower();
							if (streamReader.EndOfStream)
							{
								break;
							}
							string text2 = En.解密为字符串(streamReader.ReadLine());
							if (类型.非空全部(text, text2) && !Di.ContainsKey(text))
							{
								Di.Add(text, text2);
							}
						}
						while (!streamReader.EndOfStream);
						streamReader.Dispose();
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						if (类型.非空(streamReader))
						{
							streamReader.Dispose();
						}
						程序.出错(ex2);
						ProjectData.ClearProjectError();
					}
				}
			}

			/// <summary>
			/// 保存内容到本地文件
			/// </summary>
			public void 保存到本地()
			{
				if (文件.删除文件(本地文件))
				{
					StreamWriter streamWriter = null;
					try
					{
						streamWriter = new StreamWriter(File.OpenWrite(本地文件));
						streamWriter.WriteLine("戈登走過去的配置文件，请勿乱修改。 " + 程序.本程序.文件名 + ".exe");
						foreach (string key2 in Di.Keys)
						{
							string text = Di[key2];
							if (类型.非空全部(key2, text))
							{
								streamWriter.WriteLine(En.加密(key2));
								streamWriter.WriteLine(En.加密(text));
							}
						}
						streamWriter.Dispose();
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						if (类型.非空(streamWriter))
						{
							streamWriter.Dispose();
						}
						程序.出错(ex2);
						ProjectData.ClearProjectError();
					}
				}
			}

			/// <summary>
			/// 把值与控件绑定，一个控件只能有一个值，绑定的时候读取值，窗口关闭的时候保存值
			/// </summary>
			public void 绑定控件(Control 控件, 控件值类型 值, object 默认 = null)
			{
				string text = 生成控件读取(控件);
				if (text.Length >= 1)
				{
					try
					{
						object obj = null;
						int num = 0;
						switch (值)
						{
						case 控件值类型.Text:
							obj = this.get_字符串(text, Conversions.ToString(默认));
							break;
						case 控件值类型.Checked:
							obj = this.get_真假(text, Conversions.ToBoolean(默认));
							num = 1;
							break;
						default:
							obj = this.get_数字(text, Conversions.ToDouble(默认));
							num = 2;
							break;
						}
						Versioned.CallByName(控件, 值.ToString(), CallType.Set, obj);
						_Closure_0024__25_002D1 CS_0024_003C_003E8__locals0;
						控件.FindForm().FormClosing += delegate
						{
							CS_0024_003C_003E8__locals0._Lambda_0024__0();
						};
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						程序.出错(ex2, 控件, 值.ToString());
						ProjectData.ClearProjectError();
					}
				}
			}

			/// <summary>
			/// 绑定控件的时候的内部ID
			/// </summary>
			public string 生成控件读取(Control 控件)
			{
				if (类型.为空(控件, 控件.FindForm()))
				{
					return "";
				}
				return 控件.FindForm().Name + 控件.GetType().ToString() + 控件.Name;
			}
		}

		/// <summary>
		/// Wsave控件绑定的时候用的控件值类型，注意Value视作为数字类型
		/// </summary>
		public enum 控件值类型
		{
			Text,
			Checked,
			Value,
			Maximum,
			Minimum,
			Left,
			Top,
			Width,
			Height,
			SelectedIndex,
			Interval
		}
	}
}
