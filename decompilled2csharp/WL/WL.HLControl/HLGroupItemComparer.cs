using System.Collections.Generic;
using WL.基础;

namespace WL.HLControl
{
	internal class HLGroupItemComparer : IComparer<HLGroupItem>
	{
		private bool desc;

		public HLGroupItemComparer(bool desc)
		{
			this.desc = desc;
		}

		public int Compare(HLGroupItem x, HLGroupItem y)
		{
			int num = 文本.比较文本(x.Title, x.Title);
			if (desc)
			{
				num = checked(-num);
			}
			return num;
		}

		int IComparer<HLGroupItem>.Compare(HLGroupItem x, HLGroupItem y)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Compare
			return this.Compare(x, y);
		}
	}
}
