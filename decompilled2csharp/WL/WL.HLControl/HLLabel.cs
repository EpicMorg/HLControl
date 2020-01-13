using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	public class HLLabel : Label
	{
		private bool 高亮;

		private bool 黯淡;

		private string 文本;

		[DefaultValue(false)]
		public bool HighLight
		{
			get
			{
				return 高亮;
			}
			set
			{
				高亮 = value;
				Invalidate();
				if (value)
				{
					黯淡 = false;
				}
				if (value && 类型.非空(base.Parent))
				{
					IEnumerator enumerator = default(IEnumerator);
					try
					{
						enumerator = base.Parent.Controls.GetEnumerator();
						while (enumerator.MoveNext())
						{
							Control control = (Control)enumerator.Current;
							if (control.GetType() == GetType() && Operators.CompareString(control.Name, base.Name, TextCompare: false) != 0)
							{
								HLLabel hLLabel = (HLLabel)control;
								hLLabel.HighLight = false;
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
			}
		}

		[DefaultValue(false)]
		public bool LowLight
		{
			get
			{
				return 黯淡;
			}
			set
			{
				if (value)
				{
					高亮 = false;
				}
				黯淡 = value;
				Invalidate();
			}
		}

		public HLLabel()
		{
			base.Paint += HLLabel_Paint;
			DoubleBuffered = true;
			高亮 = false;
			黯淡 = false;
			AutoSize = true;
		}

		private void HLLabel_Paint(object sender, PaintEventArgs e)
		{
			Color foreColor = HL辅助信息.内容白;
			if (HighLight)
			{
				foreColor = HL辅助信息.内容黄;
			}
			if (LowLight)
			{
				foreColor = HL辅助信息.淡色;
			}
			if (!base.Enabled)
			{
				foreColor = HL辅助信息.暗色;
			}
			ForeColor = foreColor;
		}
	}
}
