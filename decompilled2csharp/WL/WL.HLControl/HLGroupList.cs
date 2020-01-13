using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	[DefaultEvent("SelectedIndexChanged")]
	public class HLGroupList : HLControlBase
	{
		public delegate void SelectedIndexChangedEventHandler(HLGroupList sender, HLValueEventArgs e);

		private HLVScrollBar 滚动条;

		private List<HLGroup> 物品;

		private int 最高栏;

		private int 行高;

		private int 选中;

		private float 边缘;

		private bool 展示图标;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SelectedIndexChangedEventHandler SelectedIndexChangedEvent;

		[DefaultValue(1)]
		public int SmallChange
		{
			get
			{
				return 滚动条.SmallChange;
			}
			set
			{
				滚动条.SmallChange = value;
			}
		}

		[Browsable(false)]
		public List<HLGroup> Groups
		{
			get
			{
				Invalidate();
				return 物品;
			}
		}

		[DefaultValue(true)]
		public bool ShowIcon
		{
			get
			{
				return 展示图标;
			}
			set
			{
				if (展示图标 != value)
				{
					展示图标 = value;
					Invalidate();
				}
			}
		}

		[DefaultValue(true)]
		public bool ShowScrollBar
		{
			get
			{
				return 滚动条.Visible;
			}
			set
			{
				if (滚动条.Visible != value)
				{
					滚动条.Visible = value;
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		public int SelectedIndex
		{
			get
			{
				return 转外部序号(选中);
			}
			set
			{
				int num = 转内部序号(value);
				if (选中 != num)
				{
					int c = 选中;
					if (num == -1)
					{
						value = -1;
					}
					滚动条.Value = num;
					选中 = num;
					SelectedIndexChangedEvent?.Invoke(this, new HLValueEventArgs(转外部序号(c), 选中));
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		public HLGroupItem SelectedItem
		{
			get
			{
				object objectValue = RuntimeHelpers.GetObjectValue(序号物品(选中));
				if (类型.非空(RuntimeHelpers.GetObjectValue(objectValue)) && objectValue.GetType() == typeof(HLGroupItem))
				{
					return (HLGroupItem)objectValue;
				}
				return null;
			}
			set
			{
				checked
				{
					if (类型.非空(value) && 选中 > 0)
					{
						int num = -1;
						HLGroup hLGroup = null;
						int num2 = 0;
						bool flag = false;
						foreach (HLGroup item in 物品)
						{
							hLGroup = item;
							num++;
							num2 = -1;
							foreach (HLGroupItem item2 in hLGroup.Items)
							{
								num++;
								num2++;
								if (num == 选中)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
						if (flag)
						{
							程序.输出(num2);
							hLGroup.Items[num2] = value;
							Invalidate();
						}
					}
				}
			}
		}

		public event SelectedIndexChangedEventHandler SelectedIndexChanged
		{
			[CompilerGenerated]
			add
			{
				SelectedIndexChangedEventHandler selectedIndexChangedEventHandler = SelectedIndexChangedEvent;
				SelectedIndexChangedEventHandler selectedIndexChangedEventHandler2;
				do
				{
					selectedIndexChangedEventHandler2 = selectedIndexChangedEventHandler;
					SelectedIndexChangedEventHandler value2 = (SelectedIndexChangedEventHandler)Delegate.Combine(selectedIndexChangedEventHandler2, value);
					selectedIndexChangedEventHandler = Interlocked.CompareExchange(ref SelectedIndexChangedEvent, value2, selectedIndexChangedEventHandler2);
				}
				while ((object)selectedIndexChangedEventHandler != selectedIndexChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				SelectedIndexChangedEventHandler selectedIndexChangedEventHandler = SelectedIndexChangedEvent;
				SelectedIndexChangedEventHandler selectedIndexChangedEventHandler2;
				do
				{
					selectedIndexChangedEventHandler2 = selectedIndexChangedEventHandler;
					SelectedIndexChangedEventHandler value2 = (SelectedIndexChangedEventHandler)Delegate.Remove(selectedIndexChangedEventHandler2, value);
					selectedIndexChangedEventHandler = Interlocked.CompareExchange(ref SelectedIndexChangedEvent, value2, selectedIndexChangedEventHandler2);
				}
				while ((object)selectedIndexChangedEventHandler != selectedIndexChangedEventHandler2);
			}
		}

		public HLGroupList()
		{
			base.MouseWheel += PerformMouseWheel;
			base.MouseDown += _MouseDown;
			DoubleBuffered = true;
			物品 = new List<HLGroup>();
			滚动条 = new HLVScrollBar();
			最高栏 = 0;
			选中 = -1;
			HLVScrollBar hLVScrollBar = 滚动条;
			checked
			{
				hLVScrollBar.Width = (int)Math.Round(25f * HL辅助信息.DPI);
				hLVScrollBar.Height = base.Height;
				hLVScrollBar.Top = 0;
				hLVScrollBar.Left = 0;
				hLVScrollBar = null;
				base.Controls.Add(滚动条);
				滚动条.ValueChanged += delegate
				{
					_Lambda_0024__7_002D0();
				};
				行高 = (int)Math.Round(Font.GetHeight() + 3f * HL辅助信息.DPI);
				边缘 = 5f * HL辅助信息.DPI;
				展示图标 = true;
			}
		}

		public void PerformMouseWheel(object sender, MouseEventArgs e)
		{
			滚动条.PerformMouseWheel(RuntimeHelpers.GetObjectValue(sender), e);
		}

		private void _MouseDown(object sender, MouseEventArgs e)
		{
			checked
			{
				if (!ShowScrollBar || e.X < 滚动条.Left)
				{
					int num = (int)Math.Round((float)e.Y - 边缘);
					num = (int)Math.Round(Conversion.Int((double)num / (double)行高) + (double)最高栏);
					int num2 = 选中;
					选中 = Conversions.ToInteger(Interaction.IIf(num < 总物品数(), num, -1));
					if (选中 != num2)
					{
						SelectedIndexChangedEvent?.Invoke(this, new HLValueEventArgs(转外部序号(num2), 转外部序号(选中)));
						Invalidate();
					}
				}
			}
		}

		public HLGroup GetGroup(string title)
		{
			foreach (HLGroup item in 物品)
			{
				if (Operators.CompareString(item.Title.ToLower(), title.ToLower(), TextCompare: false) == 0)
				{
					Invalidate();
					return item;
				}
			}
			return null;
		}

		private int 转外部序号(int c)
		{
			if (c < 0)
			{
				return -1;
			}
			int num = -1;
			int num2 = -1;
			checked
			{
				foreach (HLGroup item in 物品)
				{
					num++;
					if (num == c)
					{
						return -1;
					}
					foreach (HLGroupItem item2 in item.Items)
					{
						num++;
						num2++;
						if (num == c)
						{
							return num2;
						}
					}
				}
				return -1;
			}
		}

		private int 转内部序号(int c)
		{
			if (c < 0)
			{
				return -1;
			}
			int num = -1;
			int num2 = -1;
			checked
			{
				foreach (HLGroup item in 物品)
				{
					num++;
					foreach (HLGroupItem item2 in item.Items)
					{
						num++;
						num2++;
						if (num2 == c)
						{
							return num;
						}
					}
				}
				return -1;
			}
		}

		private object 序号物品(int v)
		{
			if (v < 0)
			{
				return null;
			}
			int num = 0;
			checked
			{
				foreach (HLGroup item in 物品)
				{
					if (num == v)
					{
						return item;
					}
					foreach (HLGroupItem item2 in item.Items)
					{
						num++;
						if (num == v)
						{
							return item2;
						}
					}
					num++;
				}
				return null;
			}
		}

		private int 总物品数()
		{
			int num = 0;
			foreach (HLGroup item in 物品)
			{
				num = checked(num + (1 + item.Items.Count));
			}
			滚动条.Maximum = Conversions.ToInteger(Interaction.IIf(num > 0, num, 1));
			return num;
		}

		public void SortAll(bool desc = false)
		{
			Groups.Sort(new HLGroupComparer(desc));
			foreach (HLGroup group in Groups)
			{
				group.Sort(desc);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			checked
			{
				行高 = (int)Math.Round(Font.GetHeight() + 3f * HL辅助信息.DPI);
				object A = base.Width;
				类型.设最小值(ref A, 30f * HL辅助信息.DPI);
				base.Width = Conversions.ToInteger(A);
				A = base.Height;
				类型.设最小值(ref A, 50f * HL辅助信息.DPI);
				base.Height = Conversions.ToInteger(A);
				int num = (int)Conversion.Int((double)(((float)base.Height - 边缘) / (float)行高) - 0.5);
				int num2 = 总物品数() - num;
				HLVScrollBar hLVScrollBar = 滚动条;
				hLVScrollBar.Left = base.Width - hLVScrollBar.Width;
				hLVScrollBar.Height = base.Height;
				hLVScrollBar.Enabled = (num2 > 0);
				hLVScrollBar = null;
				Graphics graphics = e.Graphics;
				Rectangle clientRectangle = base.ClientRectangle;
				int num3 = (int)Math.Round(Font.GetHeight());
				Font truePart = new Font(Font.Name, (float)((double)Font.Size * 0.8));
				Graphics graphics2 = graphics;
				HL辅助信息.绘制基础矩形(graphics, clientRectangle, 按下: true, 黑框: false, HL辅助信息.内容绿);
				if (num2 < 1)
				{
					num2 = 1;
				}
				滚动条.Maximum = num2;
				int num4 = (int)Math.Round(边缘);
				int num5 = (int)Math.Round(边缘);
				if (ShowIcon)
				{
					num5 += num3;
				}
				int num6 = 最高栏;
				int num7 = 最高栏 + num;
				for (int i = num6; i <= num7; i++)
				{
					object objectValue = RuntimeHelpers.GetObjectValue(序号物品(i));
					if (类型.为空(RuntimeHelpers.GetObjectValue(objectValue)))
					{
						break;
					}
					bool flag = objectValue.GetType() == typeof(HLGroup);
					if (选中 == i && !flag)
					{
						graphics2.FillRectangle(HL辅助信息.块黄笔刷, new Rectangle(0, num4, (int)Math.Round((float)base.Width - 边缘), 行高));
					}
					HL辅助信息.绘制文本(graphics, Conversions.ToString(Interaction.IIf(flag, objectValue.ToString().ToUpper(), objectValue.ToString())), (Font)Interaction.IIf(flag, truePart, Font), num5, Conversions.ToSingle(Interaction.IIf(flag, (double)num4 + (double)num3 * 0.2, num4)), HL辅助信息.获取文本状态(base.Enabled, flag));
					if (ShowIcon && !flag)
					{
						HLGroupItem hLGroupItem = (HLGroupItem)objectValue;
						if (hLGroupItem.HasIco)
						{
							graphics2.DrawIcon(hLGroupItem.Ico, new Rectangle((int)Math.Round(边缘), (int)Math.Round((double)num4 + (double)num3 * 0.1), (int)Math.Round((double)num3 * 0.8), (int)Math.Round((double)num3 * 0.8)));
						}
					}
					else if (flag)
					{
						graphics2.DrawLine(HL辅助信息.黑边框, HL辅助信息.点((int)Math.Round(边缘), num4 + 行高), HL辅助信息.点(Conversions.ToInteger(Operators.SubtractObject(Interaction.IIf(滚动条.Visible, 滚动条.Left, base.Width), 边缘)), num4 + 行高));
					}
					num4 += 行高;
				}
				graphics2 = null;
			}
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R7_002D1(HLVScrollBar a0, HLValueEventArgs a1)
		{
			_Lambda_0024__7_002D0();
		}

		[CompilerGenerated]
		private void _Lambda_0024__7_002D0()
		{
			最高栏 = 滚动条.Value;
			Invalidate();
		}
	}
}
