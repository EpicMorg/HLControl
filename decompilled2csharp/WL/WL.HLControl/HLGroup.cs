using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WL.HLControl
{
	public class HLGroup
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string _Title;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<HLGroupItem> _Items;

		public string Title
		{
			get;
			set;
		}

		public List<HLGroupItem> Items
		{
			get;
			set;
		}

		public HLGroup(string title, params string[] str)
		{
			Title = title;
			Items = new List<HLGroupItem>();
			foreach (string title2 in str)
			{
				Items.AddRange((IEnumerable<HLGroupItem>)new HLGroupItem(title2));
			}
		}

		public HLGroupItem GetItem(string title)
		{
			foreach (HLGroupItem item in Items)
			{
				if (Operators.CompareString(item.Title.ToLower(), title.ToLower(), TextCompare: false) == 0)
				{
					return item;
				}
			}
			return null;
		}

		public static implicit operator HLGroup(string m)
		{
			return new HLGroup(m);
		}

		public static implicit operator string(HLGroup m)
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

		public void Sort(bool desc = false)
		{
			Items.Sort(new HLGroupItemComparer(desc));
		}
	}
}
