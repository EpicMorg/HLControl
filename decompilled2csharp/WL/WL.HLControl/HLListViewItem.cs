using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WL.基础;

namespace WL.HLControl
{
	public class HLListViewItem
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<string> _Items;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string _Title;

		public List<string> Items
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public HLListViewItem(string Title, params string[] str)
		{
			this.Title = 文本.去除(Title, "\r", "\n");
			Items = new List<string>();
			Items.AddRange(str);
		}

		public static implicit operator HLListViewItem(string s)
		{
			return new HLListViewItem(s);
		}

		public static implicit operator string(HLListViewItem s)
		{
			return s.Title;
		}

		public override string ToString()
		{
			return Title;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() == GetType())
			{
				HLListViewItem hLListViewItem = (HLListViewItem)obj;
				if (Operators.CompareString(hLListViewItem.Title, Title, TextCompare: false) == 0 && hLListViewItem.Items.Count == Items.Count)
				{
					bool flag = true;
					foreach (string item in Items)
					{
						if (!hLListViewItem.Items.Contains(item))
						{
							flag = false;
						}
					}
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
