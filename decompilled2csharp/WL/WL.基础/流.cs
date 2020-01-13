using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WL.基础
{
	/// <summary>
	/// 针对 stream 进行处理的模块
	/// </summary>
	[StandardModule]
	public sealed class 流
	{
		/// <summary>
		/// 读取该stream一直到末尾，输出字节数组，这并不会关闭流
		/// </summary>
		public static byte[] 读取完整流(Stream 流)
		{
			if (Information.IsNothing(流))
			{
				return null;
			}
			try
			{
				List<byte> list = new List<byte>();
				int num = 1;
				while (true)
				{
					num = 流.ReadByte();
					if (num < 0 || num > 255)
					{
						break;
					}
					list.Add(checked((byte)num));
				}
				return list.ToArray();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				程序.出错(ex2);
				ProjectData.ClearProjectError();
			}
			return null;
		}

		/// <summary>
		/// 读取流直到0字节或者流的末尾，然后输出该部分的字符串，这并不会关闭流
		/// </summary>
		public static string 读至零返回字符串(Stream 流, Encoding 编码 = null)
		{
			if (Information.IsNothing(流))
			{
				return "";
			}
			List<byte> list = new List<byte>();
			int num = 1;
			while (true)
			{
				num = 流.ReadByte();
				if (num < 1 || num > 255)
				{
					break;
				}
				list.Add(checked((byte)num));
			}
			return 文本.字节数组转文本(list.ToArray(), 编码);
		}
	}
}
