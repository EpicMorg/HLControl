using System.Collections.Generic;
using WL.基础;

namespace WL.HLControl
{
	internal class HLListViewItemSort : IComparer<HLListViewItem>
	{
		private uint L;

		private bool op;

		public HLListViewItemSort(uint L, bool 反向)
		{
			this.L = L;
			op = 反向;
		}

		public int Compare(HLListViewItem a, HLListViewItem b)
		{
			string a2 = "";
			string b2 = "";
			checked
			{
				if (unchecked((ulong)L) == 0)
				{
					a2 = a.Title;
					b2 = b.Title;
				}
				else
				{
					ref uint l = ref L;
					l = (uint)(unchecked((long)l) - 1L);
					if (a.Items.Count > L)
					{
						a2 = a.Items[(int)L];
					}
					if (b.Items.Count > L)
					{
						b2 = b.Items[(int)L];
					}
					ref uint l2 = ref L;
					l2 = (uint)(unchecked((long)l2) + 1L);
				}
				int num = 文本.比较文本(a2, b2);
				if (op)
				{
					num = -num;
				}
				return num;
			}
		}

		int IComparer<HLListViewItem>.Compare(HLListViewItem a, HLListViewItem b)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Compare
			return this.Compare(a, b);
		}
	}
}
