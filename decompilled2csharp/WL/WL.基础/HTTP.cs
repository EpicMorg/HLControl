using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace WL.基础
{
	/// <summary>
	/// HTTP相关操作模块
	/// </summary>
	[StandardModule]
	public sealed class HTTP
	{
		/// <summary>
		/// HTTP请求用的常用的浏览器UA
		/// </summary>
		public sealed class 常用UserAgent
		{
			public const string Chrome64 = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.167 Safari/537.36";

			public const string Firefox63 = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0";

			public const string Steam = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; Valve Steam Client/default/1522709999; ) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";

			public const string IE11 = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; rv:11.0) like Gecko ";

			public const string IE8 = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";

			public const string iPhone = "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_3 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.0 Mobile/14G60 Safari/602.1";

			public const string Edge = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/18.17763";

			protected 常用UserAgent()
			{
			}
		}

		/// <summary>
		/// 简单的发送HTTP请求并获得回应
		/// </summary>
		public class 发送HTTP
		{
			private WebHeaderCollection headers;

			private List<byte> write;

			private string err;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string _链接;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string _Accept;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string _方法;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string _Content_Type;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string _UA;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string _Referer;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private uint _超时;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private uint _重试次数;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private uint _重试间隔时间;

			/// <summary>
			/// 请求用的链接
			/// </summary>
			public string 链接
			{
				get;
				set;
			}

			/// <summary>
			/// Accept 请求头用来告知客户端可以处理的内容类型
			/// 默认为空
			/// </summary>
			public string Accept
			{
				get;
				set;
			}

			/// <summary>
			/// 请求用的方法 GET POST DELETE PUT 等
			/// 默认为 GET
			/// </summary>
			public string 方法
			{
				get;
				set;
			}

			/// <summary>
			/// Content-Type 实体头部用于指示资源的MIME类型
			/// 默认为 application/x-www-form-urlencoded; charset=UTF-8
			/// </summary>
			public string Content_Type
			{
				get;
				set;
			}

			/// <summary>
			/// 请求的 User-Agent
			/// 默认为 Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_3 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.0 Mobile/14G60 Safari/602.1
			/// </summary>
			public string UA
			{
				get;
				set;
			}

			/// <summary>
			/// 请求用的 cookie
			/// 默认为空
			/// </summary>
			public string Cookie
			{
				get
				{
					return this.get_自定义头("Cookie");
				}
				set
				{
					this.set_自定义头("Cookie", value);
				}
			}

			/// <summary>
			/// Origin 首部字段表明预检请求或实际请求的源站
			/// 默认为空
			/// </summary>
			public string Origin
			{
				get
				{
					return this.get_自定义头("Origin");
				}
				set
				{
					this.set_自定义头("Origin", value);
				}
			}

			/// <summary>
			/// Referer 首部包含了当前请求页面的来源页面的地址，即表示当前页面是通过此来源页面里的链接进入的。
			/// 默认为空
			/// </summary>
			public string Referer
			{
				get;
				set;
			}

			/// <summary>
			/// 设置几秒后超时
			/// 默认为8秒
			/// </summary>
			public uint 超时
			{
				get;
				set;
			}

			/// <summary>
			/// 自定义请求头的集合
			/// </summary>
			public string 自定义头
			{
				get
				{
					if (名字.Length < 1)
					{
						return "";
					}
					string 文本 = headers.Get(名字);
					return 文本.文本标准化(ref 文本);
				}
				set
				{
					if (名字.Length > 0)
					{
						headers.Remove(名字);
						if (value.Length > 0)
						{
							headers.Add(名字, value);
						}
					}
				}
			}

			/// <summary>
			/// 如果请求失败，重试的次数，默认不重试
			/// </summary>
			public uint 重试次数
			{
				get;
				set;
			}

			/// <summary>
			/// 重试请求的时候的时间间隔，单位为秒，默认是3秒
			/// </summary>
			public uint 重试间隔时间
			{
				get;
				set;
			}

			/// <summary>
			/// 对HTTP请求内容进行预览，默认不输出请求流的内容
			/// </summary>
			public string 预览内容
			{
				get
				{
					文本.数据登记表 数据登记表 = new 文本.数据登记表();
					文本.数据登记表 数据登记表2 = 数据登记表;
					数据登记表2.增加("Url", 链接);
					数据登记表2.增加("Method", 方法);
					数据登记表2.增加("Timeout", 超时.ToString());
					if ((long)重试次数 > 0L)
					{
						数据登记表2.增加("重试次数", 重试次数.ToString());
						数据登记表2.增加("重试间隔时间", 重试间隔时间.ToString());
					}
					数据登记表2.增加("Accept", Accept);
					数据登记表2.增加("Content-Type", Content_Type);
					byte[] array = write.ToArray();
					数据登记表2.增加("Content-Length", Conversions.ToString(array.Length));
					数据登记表2.增加("User-Agent", UA);
					数据登记表2.增加("Referer", Referer);
					string[] allKeys = headers.AllKeys;
					foreach (string text in allKeys)
					{
						数据登记表2.增加(text, headers.Get(text));
					}
					if (输出请求流的内容)
					{
						数据登记表2.增加(文本.字节数组转文本(array));
					}
					数据登记表2 = null;
					return 数据登记表.ToString();
				}
			}

			/// <summary>
			/// 新建一个HTTP请求
			/// </summary>
			public 发送HTTP(string 链接, string 方法 = "GET")
			{
				headers = new WebHeaderCollection();
				this.链接 = 链接;
				this.方法 = 方法;
				UA = "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_3 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.0 Mobile/14G60 Safari/602.1";
				Accept = "";
				Content_Type = "application/x-www-form-urlencoded; charset=UTF-8";
				Cookie = "";
				Origin = "";
				Referer = "";
				write = new List<byte>();
				超时 = 8u;
				重试次数 = 0u;
				重试间隔时间 = 3u;
				err = "";
			}

			/// <summary>
			/// 清空已经向请求内写入的各种内容
			/// </summary>
			public void 清空已写入内容()
			{
				write.Clear();
			}

			/// <summary>
			/// 向请求内写入字节数组
			/// </summary>
			public void 写入字节数组(byte[] 字节数组)
			{
				if (类型.非空(字节数组))
				{
					write.AddRange(字节数组);
				}
			}

			/// <summary>
			/// 向请求内写入文本
			/// </summary>
			public void 写入文本(string 文本, Encoding 编码 = null)
			{
				if (文本.Length > 0)
				{
					写入字节数组(文本.文本转字节数组(文本, 编码));
				}
			}

			/// <summary>
			/// 写入FormData
			/// </summary>
			public void 写入FormData(生成FormData 生成器)
			{
				清空已写入内容();
				方法 = "POST";
				写入文本(生成器.ToString());
			}

			/// <summary>
			/// 写入 multipart/form-data
			/// </summary>
			public void 写入multipartformdata(生成multipartformdata 生成器)
			{
				清空已写入内容();
				方法 = "POST";
				Content_Type = "multipart/form-data; boundary=" + 生成器.分隔符;
				写入字节数组(生成器.实际生成内容);
			}

			/// <summary>
			/// 发送请求，获取回应并读取为字节数组，如果出错会返回nothing
			/// </summary>
			public byte[] 获取回应为字节数组()
			{
				checked
				{
					int num = (int)重试次数;
					int num2 = num;
					for (int i = 0; i <= num2; i++)
					{
						try
						{
							byte[] array = null;
							HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(链接);
							HttpWebRequest httpWebRequest2 = httpWebRequest;
							httpWebRequest2.Timeout = (int)(unchecked((long)超时) * 1000L);
							httpWebRequest2.ReadWriteTimeout = httpWebRequest2.Timeout;
							httpWebRequest2.ContinueTimeout = httpWebRequest2.Timeout;
							httpWebRequest2.Method = 方法;
							if (Accept.Length > 0)
							{
								httpWebRequest2.Accept = Accept;
							}
							if (Content_Type.Length > 0)
							{
								httpWebRequest2.ContentType = Content_Type;
							}
							string[] allKeys = headers.AllKeys;
							foreach (string name in allKeys)
							{
								httpWebRequest2.Headers.Remove(name);
								httpWebRequest2.Headers.Add(name, headers.Get(name));
							}
							if (Referer.Length > 0)
							{
								httpWebRequest2.Referer = Referer;
							}
							if (UA.Length > 0)
							{
								httpWebRequest2.UserAgent = UA;
							}
							if (write.Count > 0)
							{
								byte[] array2 = write.ToArray();
								httpWebRequest2.ContentLength = array2.Length;
								httpWebRequest2.GetRequestStream().Write(array2, 0, array2.Length);
							}
							else
							{
								httpWebRequest2.ContentLength = 0L;
							}
							Stream responseStream = httpWebRequest2.GetResponse().GetResponseStream();
							array = 流.读取完整流(responseStream);
							responseStream.Close();
							return array;
						}
						catch (Exception ex)
						{
							ProjectData.SetProjectError(ex);
							Exception ex2 = ex;
							程序.出错(ex2, Interaction.IIf(i > 0, "第" + Conversions.ToString(i) + "次重试。", "首次失败。"));
							err = ex2.Message;
							ProjectData.ClearProjectError();
						}
					}
					return null;
				}
			}

			/// <summary>
			/// 发送请求，获取回应并读取为字符串，如果出错会返回错误信息的字符串
			/// 默认不去除引号，但默认反转义
			/// </summary>
			public string 获取回应为字符串(Encoding 编码 = null, bool 反转义 = true, bool 去除引号 = false)
			{
				byte[] array = 获取回应为字节数组();
				if (类型.为空(array))
				{
					return err;
				}
				string text = 文本.字节数组转文本(array, 编码);
				if (去除引号)
				{
					text = 文本.去除(text, "\"");
				}
				if (反转义)
				{
					text = 文本.反转义(text);
				}
				return text;
			}
		}

		/// <summary>
		/// HTTP请求用的 FormData 生成器
		/// </summary>
		public class 生成FormData
		{
			private string m;

			/// <summary>
			/// 新建一个生成器并写入内容
			/// </summary>
			public 生成FormData(params string[] 内容)
			{
				m = "";
				写入(内容);
			}

			/// <summary>
			/// 写入内容
			/// </summary>
			public void 写入(params string[] 内容)
			{
				checked
				{
					int num = 内容.Length - 1;
					if (num <= 0)
					{
						return;
					}
					if (数学.是偶数(num))
					{
						num--;
					}
					if (m.Length > 0)
					{
						m += "&";
					}
					int num2 = num;
					for (int i = 0; i <= num2; i += 2)
					{
						ref string reference = ref m;
						reference = reference + 内容[i] + "=" + 内容[i + 1];
						if (i + 1 != num)
						{
							m += "&";
						}
					}
				}
			}

			/// <summary>
			/// 输出 FormData 原文
			/// </summary>
			public override string ToString()
			{
				return m;
			}
		}

		/// <summary>
		/// HTTP请求用的 multipart/form-data 生成器
		/// </summary>
		public class 生成multipartformdata
		{
			private List<byte> m;

			private string pv;

			private Encoding ec;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string _分隔符;

			/// <summary>
			/// 返回随机生成的 boundary
			/// </summary>
			public string 分隔符
			{
				get;
			}

			/// <summary>
			/// 返回实际生成的内容，HTTP请求用
			/// </summary>
			public byte[] 实际生成内容
			{
				get
				{
					List<byte> list = new List<byte>();
					list.AddRange(m.ToArray());
					list.AddRange(文本.文本转字节数组("\r\n--" + 分隔符 + "--", ec));
					return list.ToArray();
				}
			}

			/// <summary>
			/// 新建一个 multipart/form-data 生成器，支持自定义分隔符和自定义编码
			/// </summary>
			public 生成multipartformdata(string 自定义分隔符 = "", Encoding 自定义编码 = null)
			{
				pv = "";
				m = new List<byte>();
				if (自定义分隔符.Length > 0)
				{
					_分隔符 = 自定义分隔符;
				}
				else
				{
					_分隔符 = "--------WebKitFormBoundary" + 数学.随机.当中字符("abcdefghijklmnopqrstuvwxyz", 10u);
				}
				if (类型.为空(自定义编码))
				{
					ec = 文本.无BOM的UTF8编码();
				}
				else
				{
					ec = 自定义编码;
				}
			}

			private void AddHead()
			{
				string text = "--" + 分隔符;
				if (m.Count > 5)
				{
					text = "\r\n" + text;
				}
				text += "\r\n";
				m.AddRange(文本.文本转字节数组(text, ec));
				pv += text;
			}

			/// <summary>
			/// 写入一个参数，默认不写类型，无需对写入内容进行URL编码
			/// </summary>
			public void 写入参数(string 名字, string 内容, string 类型 = "")
			{
				if (名字.Length >= 1)
				{
					AddHead();
					string str = "Content-Disposition: form-data; name=" + 文本.引(文本.URL编码(名字)) + "\r\n";
					if (类型.Length > 0)
					{
						str = str + "Content-Type: " + 类型 + "\r\n";
					}
					str = str + "\r\n" + 文本.URL编码(内容);
					m.AddRange(文本.文本转字节数组(str, ec));
					pv += str;
				}
			}

			/// <summary>
			/// 写入字节数组
			/// </summary>
			public void 写入字节数组(string 名字, string 文件名, string 类型, byte[] 字节数组)
			{
				if (字节数组.Length > 0 && 文件名.Length > 0 && 类型.Length > 0)
				{
					AddHead();
					string str = "Content-Disposition: form-data; name=" + 文本.引(文本.URL编码(名字)) + "; filename=" + 文本.引(文本.URL编码(文件名)) + "\r\n";
					str = str + "Content-Type: " + 类型 + "\r\n\r\n";
					m.AddRange(文本.文本转字节数组(str, ec));
					m.AddRange(字节数组);
					ref string reference = ref pv;
					reference = reference + str + "[二进制内容 长度：" + Conversions.ToString(字节数组.Length) + "]";
				}
			}

			/// <summary>
			/// 读取文件的二进制内容并写入
			/// </summary>
			public void 写入文件(string 名字, string 类型, string 文件)
			{
				byte[] array = 文件.读文件为字节数组(文件);
				if (类型.非空(array))
				{
					写入字节数组(名字, 文件.文件名(文件), 类型, array);
				}
			}

			/// <summary>
			/// 返回生成内容的预览文本
			/// </summary>
			public override string ToString()
			{
				return pv + "\r\n--" + 分隔符 + "--";
			}
		}
	}
}
