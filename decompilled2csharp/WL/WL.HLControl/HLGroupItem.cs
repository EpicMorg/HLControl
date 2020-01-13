using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using WL.基础;

namespace WL.HLControl
{
	public class HLGroupItem
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string _Title;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Icon _Ico;

		public string Title
		{
			get;
			set;
		}

		public Icon Ico
		{
			get;
			set;
		}

		public bool HasIco => 类型.非空(Ico);

		public HLGroupItem(string title, Icon ico = null)
		{
			Title = title;
			Ico = ico;
		}

		public static implicit operator HLGroupItem(string m)
		{
			return new HLGroupItem(m);
		}

		public static implicit operator string(HLGroupItem m)
		{
			return m.Title;
		}

		public override string ToString()
		{
			return Title;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() == GetType())
			{
				return Operators.ConditionalCompareObjectEqual(Title, NewLateBinding.LateGet(obj, null, "title", new object[0], null, null, null), TextCompare: false);
			}
			return false;
		}
	}
}
