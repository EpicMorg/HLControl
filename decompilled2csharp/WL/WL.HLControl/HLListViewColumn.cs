using Microsoft.VisualBasic.CompilerServices;
using System;
using WL.基础;

namespace WL.HLControl
{
	public class HLListViewColumn
	{
		private string n;

		private int w;

		public string Name
		{
			get
			{
				return n;
			}
			set
			{
				n = 文本.去除(value, "\r", "\n");
			}
		}

		public uint Width
		{
			get
			{
				return checked((uint)w);
			}
			set
			{
				object A = value;
				object value2 = 类型.设最小值(ref A, 20);
				value = Conversions.ToUInteger(A);
				w = Conversions.ToInteger(value2);
			}
		}

		public uint Rwidth => checked((uint)Math.Round((float)w * HL辅助信息.DPI));

		public HLListViewColumn(string name, uint width)
		{
			Name = name;
			Width = width;
		}

		public HLListViewColumn(string name)
		{
			Name = name;
			Width = checked((uint)(name.Length * 25));
		}

		public override string ToString()
		{
			return Name;
		}

		public static implicit operator HLListViewColumn(string s)
		{
			return new HLListViewColumn(s);
		}

		public static implicit operator string(HLListViewColumn s)
		{
			return s.Name;
		}

		public static bool operator ==(HLListViewColumn a, HLListViewColumn b)
		{
			return Operators.CompareString(a.Name, b.Name, TextCompare: false) == 0;
		}

		public static bool operator !=(HLListViewColumn a, HLListViewColumn b)
		{
			return Operators.CompareString(a.Name, b.Name, TextCompare: false) != 0;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() == GetType())
			{
				return Operators.ConditionalCompareObjectEqual(Name, NewLateBinding.LateGet(obj, null, "Name", new object[0], null, null, null), TextCompare: false);
			}
			return false;
		}
	}
}
