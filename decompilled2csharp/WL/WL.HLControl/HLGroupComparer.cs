using System.Collections.Generic;
using WL.基础;

namespace WL.HLControl
{
	internal class HLGroupComparer : IComparer<HLGroup>
	{
		private bool desc;

		public HLGroupComparer(bool desc)
		{
			this.desc = desc;
		}

		public int Compare(HLGroup x, HLGroup y)
		{
			int num = 文本.比较文本(x.Title, x.Title);
			if (desc)
			{
				num = checked(-num);
			}
			return num;
		}

		int IComparer<HLGroup>.Compare(HLGroup x, HLGroup y)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Compare
			return this.Compare(x, y);
		}
	}
}
