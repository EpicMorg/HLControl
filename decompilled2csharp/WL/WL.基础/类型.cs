using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace WL.基础
{
	/// <summary>
	/// 类型判断、转换、选择、比较的模块
	/// </summary>
	[StandardModule]
	public sealed class 类型
	{
		/// <summary>
		/// 比较两个东西之间是否相等的办法
		/// </summary>
		public enum 比较方法
		{
			/// <summary>
			/// 直接使用 = 来比较
			/// </summary>
			等于,
			/// <summary>
			/// 只比较两个对象的 HashCode 是否相等
			/// </summary>
			HashCode,
			/// <summary>
			/// 只比较对象的 Tostring 是否相等
			/// </summary>
			Tostring,
			/// <summary>
			/// 使用 Equals 来比较
			/// </summary>
			Equals,
			/// <summary>
			/// 只比较两个对象的 Type 是否相等
			/// </summary>
			Type,
			/// <summary>
			/// 比较两个对象是否有相同的基础类或接口
			/// </summary>
			BaseType,
			/// <summary>
			/// 使用 Is 关键字来比较，判断两个对象是否引用自同一对象
			/// </summary>
			Is
		}

		private static bool Oneof(Type t, params Type[] b)
		{
			foreach (Type right in b)
			{
				if (t == right)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 判断这个类型是否是整数类型 Byte Integer UInteger Long ULong Short UShort Decimal 
		/// 并不判断其内容是否真的是整数
		/// </summary>
		public static bool 是整数类型(Type t)
		{
			if (Information.IsNothing(t))
			{
				return false;
			}
			return Oneof(t, typeof(int), typeof(long), typeof(byte), typeof(uint), typeof(ulong), typeof(short), typeof(ushort), typeof(decimal));
		}

		/// <summary>
		/// 判断这个类型是否是小数类型 Single Double
		/// 并不判断其内容是否真的是小数
		/// </summary>
		public static bool 是小数(Type t)
		{
			if (Information.IsNothing(t))
			{
				return false;
			}
			return Oneof(t, typeof(float), typeof(double));
		}

		/// <summary>
		/// 判断这个类型是否是 Byte Integer UInteger Long ULong Short UShort Decimal Single Double
		/// </summary>
		public static bool 是数字(Type t)
		{
			if (Information.IsNothing(t))
			{
				return false;
			}
			return 是整数类型(t) || 是小数(t);
		}

		/// <summary>
		/// 判断这个类型是否是字符串 String Char
		/// </summary>
		public static bool 是字符串(Type t)
		{
			if (Information.IsNothing(t))
			{
				return false;
			}
			return Oneof(t, typeof(string), typeof(char));
		}

		/// <summary>
		/// 判断这个类型是否是 ICollection, IList, IDictionary, IEnumerable 的一个子类
		/// </summary>
		public static bool 是集合(Type t)
		{
			if (Information.IsNothing(t))
			{
				return false;
			}
			Type[] interfaces = t.GetInterfaces();
			foreach (Type t2 in interfaces)
			{
				if (Oneof(t2, typeof(ICollection), typeof(IEnumerable), typeof(IList), typeof(IDictionary)))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 判断t类型是否继承自parent类型
		/// </summary>
		public static bool 是同一继承类(Type t, Type parent)
		{
			if (Information.IsNothing(t) || Information.IsNothing(parent))
			{
				return false;
			}
			if (t == parent)
			{
				return true;
			}
			while (!Information.IsNothing(t.BaseType))
			{
				t = t.BaseType;
				if (t == parent)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 判断t类型是否是控件
		/// </summary>
		public static bool 是控件(Type t)
		{
			return 是同一继承类(t, typeof(Control));
		}

		/// <summary>
		/// 判断这个类型是否有Length属性
		/// </summary>
		public static bool 有Length(Type t)
		{
			if (Information.IsNothing(t))
			{
				return false;
			}
			return !Information.IsNothing(t.GetProperty("Length"));
		}

		/// <summary>
		/// 判断这个类型是否有Count属性
		/// </summary>
		public static bool 有Count(Type t)
		{
			if (Information.IsNothing(t))
			{
				return false;
			}
			return !Information.IsNothing(t.GetProperty("Count"));
		}

		/// <summary>
		/// 判断这个类型是否有Count属性
		/// </summary>
		public static bool 有LengthCount(Type t)
		{
			if (Information.IsNothing(t))
			{
				return false;
			}
			return 有Length(t) || 有Count(t);
		}

		/// <summary>
		/// 判断这个对象是否为空
		/// 数字判断是否为0
		/// 有Length或者Count就判断是否为0
		/// 是颜色判断是否为 Color.Empty
		/// 是控件就判断 IsHandleCreated = False
		/// </summary>
		public static bool 为空(object 对象)
		{
			if (Information.IsNothing(RuntimeHelpers.GetObjectValue(对象)))
			{
				return true;
			}
			Type type = 对象.GetType();
			try
			{
				if (是同一继承类(type, typeof(Stream)))
				{
					return false;
				}
				if (是数字(type))
				{
					return Operators.ConditionalCompareObjectEqual(对象, 0, TextCompare: false);
				}
				if (有Count(type))
				{
					return Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(对象, null, "Count", new object[0], null, null, null), 0, TextCompare: false);
				}
				if (有Length(type))
				{
					return Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(对象, null, "Length", new object[0], null, null, null), 0, TextCompare: false);
				}
				if (type == typeof(Color))
				{
					return ((对象 != null) ? ((Color)对象) : default(Color)) == Color.Empty;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				程序.出错(ex2, type);
				ProjectData.ClearProjectError();
			}
			return false;
		}

		/// <summary>
		/// 判断这些对象中是否有一个为空
		/// </summary>
		public static bool 为空(params object[] 对象)
		{
			for (int i = 0; i < 对象.Length; i = checked(i + 1))
			{
				object objectValue = RuntimeHelpers.GetObjectValue(对象[i]);
				if (为空(RuntimeHelpers.GetObjectValue(objectValue)))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 判断这些对象中是否全部为空
		/// </summary>
		public static bool 为空全部(params object[] 对象)
		{
			for (int i = 0; i < 对象.Length; i = checked(i + 1))
			{
				object objectValue = RuntimeHelpers.GetObjectValue(对象[i]);
				if (!为空(RuntimeHelpers.GetObjectValue(objectValue)))
				{
					return false;
				}
			}
			return false;
		}

		/// <summary>
		/// 判断这个对象是否非空
		/// </summary>
		public static bool 非空(object 对象)
		{
			return !为空(RuntimeHelpers.GetObjectValue(对象));
		}

		/// <summary>
		/// 判断这些对象中是否有一个非空
		/// </summary>
		public static bool 非空(params object[] 对象)
		{
			for (int i = 0; i < 对象.Length; i = checked(i + 1))
			{
				object objectValue = RuntimeHelpers.GetObjectValue(对象[i]);
				if (非空(RuntimeHelpers.GetObjectValue(objectValue)))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 判断这些对象中是否全部非空
		/// </summary>
		public static bool 非空全部(params object[] 对象)
		{
			for (int i = 0; i < 对象.Length; i = checked(i + 1))
			{
				object objectValue = RuntimeHelpers.GetObjectValue(对象[i]);
				if (!非空(RuntimeHelpers.GetObjectValue(objectValue)))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 用指定的比较方法来比较A和B两个对象是否相等，如果比较出错，只返回 A.Equals(B)
		/// </summary>
		public static bool 比较(object A, object B, 比较方法 方法)
		{
			try
			{
				switch (方法)
				{
				case 比较方法.等于:
					return Operators.ConditionalCompareObjectEqual(A, B, TextCompare: false);
				case 比较方法.Tostring:
					return Operators.CompareString(A.ToString(), B.ToString(), TextCompare: false) == 0;
				case 比较方法.Is:
					return A == B;
				case 比较方法.Type:
					return A.GetType() == B.GetType();
				case 比较方法.HashCode:
					return A.GetHashCode() == B.GetHashCode();
				case 比较方法.BaseType:
				{
					if (A.GetType().BaseType != typeof(object) && A.GetType().BaseType == B.GetType())
					{
						return true;
					}
					if (B.GetType().BaseType != typeof(object) && B.GetType().BaseType == A.GetType())
					{
						return true;
					}
					Type[] interfaces = A.GetType().GetInterfaces();
					foreach (Type left in interfaces)
					{
						Type[] interfaces2 = B.GetType().GetInterfaces();
						foreach (Type right in interfaces2)
						{
							if (left == right)
							{
								return true;
							}
						}
					}
					return false;
				}
				default:
					return false;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				程序.出错(ex2);
				ProjectData.ClearProjectError();
			}
			return A.Equals(RuntimeHelpers.GetObjectValue(B));
		}

		/// <summary>
		/// 用指定的办法来判断这个物品是否在列表内
		/// </summary>
		public static bool 在列表(IList 列表, object 物品, 比较方法 比较方法)
		{
			if (!Information.IsNothing(列表) && 列表.Count >= 1 && !Information.IsNothing(RuntimeHelpers.GetObjectValue(物品)))
			{
				IEnumerator enumerator = default(IEnumerator);
				try
				{
					enumerator = 列表.GetEnumerator();
					while (enumerator.MoveNext())
					{
						object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
						if (比较(RuntimeHelpers.GetObjectValue(objectValue), RuntimeHelpers.GetObjectValue(物品), 比较方法))
						{
							return true;
						}
					}
				}
				finally
				{
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
			}
			return false;
		}

		/// <summary>
		/// 针对list进行去重
		/// </summary>
		public static void 列表去重(IList 列表, 比较方法 比较方法)
		{
			if (非空(列表))
			{
				List<object> list = new List<object>();
				IEnumerator enumerator = default(IEnumerator);
				try
				{
					enumerator = 列表.GetEnumerator();
					while (enumerator.MoveNext())
					{
						object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
						if (!Information.IsNothing(RuntimeHelpers.GetObjectValue(objectValue)) && !在列表(list, RuntimeHelpers.GetObjectValue(objectValue), 比较方法))
						{
							list.Add(RuntimeHelpers.GetObjectValue(objectValue));
						}
					}
				}
				finally
				{
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
				列表.Clear();
				foreach (object item in list)
				{
					object objectValue = RuntimeHelpers.GetObjectValue(item);
					列表.Add(RuntimeHelpers.GetObjectValue(objectValue));
				}
			}
		}

		/// <summary>
		/// 判断寻找的对象是否=内容当中的一个
		/// </summary>
		public static bool 是当中一个(object 寻找, params object[] 内容)
		{
			if (Information.IsNothing(RuntimeHelpers.GetObjectValue(寻找)))
			{
				return false;
			}
			for (int i = 0; i < 内容.Length; i = checked(i + 1))
			{
				object objectValue = RuntimeHelpers.GetObjectValue(内容[i]);
				if (比较(RuntimeHelpers.GetObjectValue(objectValue), RuntimeHelpers.GetObjectValue(寻找), 比较方法.等于))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 判断寻找的对象是否是内容当中的一个，并使用对应的比较办法
		/// </summary>
		public static bool 是当中一个(比较方法 比较办法, object 寻找, params object[] 内容)
		{
			if (Information.IsNothing(RuntimeHelpers.GetObjectValue(寻找)))
			{
				return false;
			}
			for (int i = 0; i < 内容.Length; i = checked(i + 1))
			{
				object objectValue = RuntimeHelpers.GetObjectValue(内容[i]);
				if (比较(RuntimeHelpers.GetObjectValue(objectValue), RuntimeHelpers.GetObjectValue(寻找), 比较办法))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 互换AB两个对象
		/// </summary>
		public static void 互换(ref object A, ref object B)
		{
			object objectValue = RuntimeHelpers.GetObjectValue(B);
			B = RuntimeHelpers.GetObjectValue(A);
			A = RuntimeHelpers.GetObjectValue(objectValue);
		}

		/// <summary>
		/// 反转A
		/// </summary>
		public static bool 反转(ref bool A)
		{
			A = !A;
			return A;
		}

		/// <summary>
		/// 如果对象等于A，那么把对象换成B
		/// </summary>
		public static object 反转(ref object 对象, object A, object B)
		{
			if (非空(RuntimeHelpers.GetObjectValue(对象)))
			{
				if (Operators.ConditionalCompareObjectEqual(对象, A, TextCompare: false))
				{
					对象 = RuntimeHelpers.GetObjectValue(B);
				}
				else if (Operators.ConditionalCompareObjectEqual(对象, B, TextCompare: false))
				{
					对象 = RuntimeHelpers.GetObjectValue(A);
				}
			}
			return 对象;
		}

		/// <summary>
		///  If A 小于 最小值 那么 A = 最小值
		/// </summary>
		public static object 设最小值(ref object A, object 最小值)
		{
			if (Operators.ConditionalCompareObjectLess(A, 最小值, TextCompare: false))
			{
				A = RuntimeHelpers.GetObjectValue(最小值);
			}
			return A;
		}

		/// <summary>
		///  If A 大于 最小值 那么 A = 最小值
		/// </summary>
		public static object 设最大值(ref object A, object 最大值)
		{
			if (Operators.ConditionalCompareObjectGreater(A, 最大值, TextCompare: false))
			{
				A = RuntimeHelpers.GetObjectValue(最大值);
			}
			return A;
		}
	}
}
