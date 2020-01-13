using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WL.基础
{
	/// <summary>
	/// 文件信息处理模块
	/// </summary>
	[StandardModule]
	public sealed class 文件
	{
		/// <summary>
		/// 把路径进行标准化处理，最后带一个\
		/// </summary>
		public static string 路径标准化(string 路径)
		{
			if (路径.Length > 2)
			{
				路径 = 文本.文本标准化(ref 路径).Trim();
				string text = "\\";
				路径 = 文本.替换(路径, "/", text);
				if (!文本.尾(路径, text))
				{
					路径 += text;
				}
			}
			return 路径;
		}

		/// <summary>
		/// 判断这个文件名或者文件夹名当中是否包含非法字符
		/// </summary>
		public static bool 是合法文件名(string 名字)
		{
			名字 = 名字.Trim();
			int length = 名字.Length;
			if (length < 1 || length > 250)
			{
				return false;
			}
			string text = "/\\:*?<>|\"";
			foreach (char value in text)
			{
				if (名字.Contains(Conversions.ToString(value)))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 返回文本当中的小写盘符
		/// </summary>
		public static string 盘符(string 文件)
		{
			if (文件.Length > 2)
			{
				return 文本.去除(Regex.Match(文件.ToLower(), "[a-z]*?:").ToString(), ":");
			}
			return "";
		}

		/// <summary>
		/// 如果是文件夹就返回上层路径，如果是文件就返回路径
		/// </summary>
		public static string 文件路径(string 文件)
		{
			if (文件.Length > 2)
			{
				if (文件.EndsWith("\\"))
				{
					文件 = 文本.去右(文件, 1u);
				}
				return 路径标准化(Regex.Match(文件, "(\\S|\\s)+\\\\").ToString());
			}
			return "";
		}

		/// <summary>
		/// 如果是文件夹就返回文件夹名，如果是文件就返回文件名，默认包括后缀
		/// </summary>
		public static string 文件名(string 文件, bool 包含后缀 = true)
		{
			if (文件.Length > 2)
			{
				if (文件.EndsWith("\\"))
				{
					文件 = 文本.去右(文件, 1u);
				}
				文件 = 文本.去除(文件, 文件路径(文件)).Trim();
				if (!包含后缀 && 文件.Length > 0)
				{
					文件 = Regex.Match(文件, ".*?\\.").ToString();
					if (文件.EndsWith("."))
					{
						文件 = 文本.去右(文件, 1u);
					}
				}
				return 文件;
			}
			return "";
		}

		/// <summary>
		/// 返回文件的小写后缀，包括第一个点
		/// </summary>
		public static string 文件后缀(string 文件)
		{
			if (文件.Length > 2)
			{
				if (文件.EndsWith("\\"))
				{
					文件 = 文本.去右(文件, 1u);
				}
				文件 = 文本.去除(文件, 文件路径(文件)).Trim();
				if (文件.Length > 2)
				{
					return Regex.Match(文本.去除(文件, 文件路径(文件)).Trim(), "\\.[\\S]+").ToString().ToLower();
				}
			}
			return "";
		}

		/// <summary>
		/// 检查这个文件是否存在 
		/// </summary>
		public static bool 文件存在(string 文件)
		{
			if (文件.Length < 3)
			{
				return false;
			}
			return File.Exists(文件);
		}

		/// <summary>
		/// 检查这个文件夹是否存在 
		/// </summary>
		public static bool 文件夹存在(string 文件夹)
		{
			if (文件夹.Length < 3)
			{
				return false;
			}
			return Directory.Exists(文件夹);
		}

		/// <summary>
		/// 返回这个文件的大小，单位为Byte，如果文件不存在则返回0
		/// </summary>
		public static uint 文件大小Byte(string 文件)
		{
			if (文件存在(文件))
			{
				return checked((uint)FileSystem.FileLen(文件));
			}
			return 0u;
		}

		/// <summary>
		/// 返回这个文件的大小，单位为KB，如果文件不存在则返回0
		/// </summary>
		public static float 文件大小KB(string 文件)
		{
			return (float)((double)文件大小Byte(文件) / 1024.0);
		}

		/// <summary>
		/// 返回这个文件的大小，单位为MB，如果文件不存在则返回0
		/// </summary>
		public static float 文件大小MB(string 文件)
		{
			return (float)((double)文件大小Byte(文件) / 1024.0 / 1024.0);
		}

		/// <summary>
		/// 读取文件大小，并自动选择合适的单位返回一个字符串，只保留整数
		/// </summary>
		public static string 文件大小文字(string 文件)
		{
			return 文件大小文字(文件大小Byte(文件));
		}

		/// <summary>
		/// 根据long值，单位应该为B，自动选择合适的单位返回一个字符串，只保留整数
		/// </summary>
		public static string 文件大小文字(long 大小)
		{
			if (大小 < 1024)
			{
				return Conversions.ToString(大小) + "B";
			}
			if (大小 < 1048576)
			{
				return Conversions.ToString(Conversion.Int((double)大小 / 1024.0)) + "KB";
			}
			if (大小 < 1073741824)
			{
				return Conversions.ToString(Conversion.Int((double)大小 / 1024.0 / 1024.0)) + "MB";
			}
			return Conversions.ToString(Conversion.Int((double)大小 / 1024.0 / 1024.0 / 1024.0)) + "GB";
		}

		/// <summary>
		/// 读取文件的内容为字节数组，如果无法读取，则返回nothing
		/// </summary>
		public static byte[] 读文件为字节数组(string 文件)
		{
			checked
			{
				if (文件存在(文件))
				{
					byte[] array = null;
					try
					{
						Stream stream = File.OpenRead(文件);
						int num = (int)(stream.Length - 1);
						array = new byte[num + 1];
						int num2 = num;
						for (int i = 0; i <= num2; i++)
						{
							array[i] = (byte)stream.ReadByte();
						}
						stream.Close();
						return array;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						程序.出错(ex2);
						ProjectData.ClearProjectError();
					}
				}
				return null;
			}
		}

		/// <summary>
		/// 读取文件的内容为字符串，并且会进行标准化处理
		/// </summary>
		public static string 读文件为文本(string 文件, Encoding 编码 = null)
		{
			byte[] array = 读文件为字节数组(文件);
			if (类型.为空(array))
			{
				return "";
			}
			return 文本.字节数组转文本(array, 编码);
		}

		/// <summary>
		/// 删除文件或文件夹，返回删除是否成功，如果不存在也算删除成功
		/// </summary>
		public static bool 删除文件(string 文件)
		{
			try
			{
				if (文件存在(文件))
				{
					File.Delete(文件);
				}
				if (文件夹存在(文件))
				{
					Directory.Delete(文件, recursive: true);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				程序.出错(ex2);
				ProjectData.ClearProjectError();
			}
			return !文件存在(文件) && !文件夹存在(文件);
		}

		/// <summary>
		/// 尝试创建文件夹，返回文件夹是否已经成功存在
		/// </summary>
		public static bool 创建文件夹(string 路径)
		{
			if (文件夹存在(路径))
			{
				return true;
			}
			bool flag = false;
			try
			{
				Directory.CreateDirectory(路径);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				程序.出错(ex2);
				ProjectData.ClearProjectError();
			}
			return 文件夹存在(路径);
		}

		/// <summary>
		/// 删除文件，然后重新写入对应的字节数组，返回是否写入成功
		/// </summary>
		public static bool 写字节数组到文件(string 文件, byte[] 字节数组)
		{
			if (删除文件(文件) && 创建文件夹(文件路径(文件)))
			{
				bool flag = false;
				try
				{
					Stream stream = File.OpenWrite(文件);
					int num = 字节数组.Length;
					stream.Write(字节数组, 0, num);
					stream.Close();
					return 文件大小Byte(文件) == num;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					程序.出错(ex2);
					ProjectData.ClearProjectError();
				}
			}
			return false;
		}

		/// <summary>
		/// 删除文件，然后重新写入对应的文本，返回是否写入成功
		/// </summary>
		public static bool 写文本到文件(string 文件, string 文本, Encoding 编码 = null)
		{
			return 写字节数组到文件(文件, 文本.文本转字节数组(文本, 编码));
		}

		/// <summary>
		/// 把路径拆分开来
		/// </summary>
		public static List<string> 路径拆分(string 路径)
		{
			return 文本.分割(路径标准化(路径), "\\");
		}

		/// <summary>
		/// 复制文件到新文件夹并重命名，并返回是否复制成功
		/// </summary>
		public static bool 文件复制重命名(string 文件, string 新文件)
		{
			string 路径 = 文件路径(新文件);
			if (!文件存在(文件) || !创建文件夹(路径))
			{
				return false;
			}
			if (删除文件(新文件))
			{
				try
				{
					File.Copy(文件, 新文件, overwrite: true);
					return 文件大小Byte(新文件) == 文件大小Byte(文件);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					程序.出错(ex2);
					ProjectData.ClearProjectError();
				}
			}
			return false;
		}

		/// <summary>
		/// 复制文件到新文件夹，并返回是否复制成功
		/// </summary>
		public static bool 文件复制(string 文件, string 路径)
		{
			return 文件复制重命名(文件, 路径 + 文件名(文件));
		}
	}
}
