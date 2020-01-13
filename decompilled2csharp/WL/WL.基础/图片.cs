using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WL.基础
{
	/// <summary>
	/// 图片处理模块
	/// </summary>
	[StandardModule]
	public sealed class 图片
	{
		/// <summary>
		/// 把图片的指定范围裁剪出来成新的图片
		/// </summary>
		public static Bitmap 裁剪(Bitmap 图片, Rectangle 范围)
		{
			if (类型.为空(图片))
			{
				return null;
			}
			return 图片.Clone(范围, 图片.PixelFormat);
		}

		/// <summary>
		/// 获取Bitmap的帧数，普通图片帧数为1
		/// </summary>
		public static uint 获取帧数(Bitmap 图片)
		{
			if (类型.为空(图片))
			{
				return 0u;
			}
			FrameDimension dimension = new FrameDimension(图片.FrameDimensionsList[0]);
			return checked((uint)图片.GetFrameCount(dimension));
		}

		/// <summary>
		/// 根据图片格式，获取 ImageCodecInfo 的 Encoder
		/// </summary>
		public static ImageCodecInfo 获取图片编码器(ImageFormat 格式)
		{
			if (类型.为空(格式))
			{
				return null;
			}
			try
			{
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				foreach (ImageCodecInfo imageCodecInfo in imageEncoders)
				{
					if (imageCodecInfo.FormatID == 格式.Guid)
					{
						return imageCodecInfo;
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
			return null;
		}

		/// <summary>
		/// 把图片变成字节数组，只保存PNG
		/// </summary>
		public static byte[] 图片转字节数组(Bitmap 图片)
		{
			if (类型.为空(图片))
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			ImageFormat png = ImageFormat.Png;
			图片.Save(memoryStream, png);
			return memoryStream.ToArray();
		}

		/// <summary>
		/// 把字节数组变成图片，只支持单帧
		/// </summary>
		public static Bitmap 字节数组转图片(byte[] 字节数组)
		{
			if (类型.非空(字节数组))
			{
				try
				{
					MemoryStream memoryStream = new MemoryStream();
					memoryStream.Write(字节数组, 0, 字节数组.Length);
					Bitmap result = (Bitmap)Image.FromStream(memoryStream);
					memoryStream.Dispose();
					return result;
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

		/// <summary>
		/// 把图片文件进行读取，并且不占用实际的本地文件 
		/// </summary>
		public static Bitmap 读图片文件(string 文件)
		{
			byte[] array = 文件.读文件为字节数组(文件);
			if (类型.为空(array))
			{
				return null;
			}
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.Write(array, 0, array.Length);
				return (Bitmap)Image.FromStream(memoryStream);
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
	}
}
