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
	public class HLListView : HLControlBase
	{
		public delegate void SelectedIndexChangedEventHandler(HLListView sender, HLValueEventArgs e);

		private List<HLListViewColumn> 列;

		private List<HLListViewItem> 物品;

		private int 最高栏;

		private int 行高;

		private int 选中;

		private HLVScrollBar 滚动条;

		private int 边缘;

		private int 选中列;

		private bool 拖动;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _ShowCount;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SelectedIndexChangedEventHandler SelectedIndexChangedEvent;

		private Dictionary<int, bool> _0024STATIC_0024_MouseDown_002420211C128095_0024d;

		private StaticLocalInitFlag _0024STATIC_0024_MouseDown_002420211C128095_0024d_0024Init;

		[Browsable(false)]
		public List<HLListViewColumn> Columns
		{
			get
			{
				Invalidate();
				return 列;
			}
		}

		[Browsable(false)]
		public List<HLListViewItem> Items
		{
			get
			{
				Invalidate();
				return 物品;
			}
		}

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
		public HLListViewItem SelectedItem
		{
			get
			{
				if (选中 >= 物品.Count)
				{
					选中 = -1;
				}
				if (选中 < 0)
				{
					return null;
				}
				return 物品[选中];
			}
			set
			{
				if (选中 >= 物品.Count)
				{
					选中 = -1;
				}
				if (选中 >= 0)
				{
					物品[选中] = value;
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		public int SelectedIndex
		{
			get
			{
				return 选中;
			}
			set
			{
				if (value < 0)
				{
					value = -1;
				}
				if (value >= 物品.Count)
				{
					value = -1;
				}
				if (选中 != value)
				{
					int num = 选中;
					选中 = value;
					滚动条.Value = value;
					SelectedIndexChangedEvent?.Invoke(this, new HLValueEventArgs(num, 选中));
					Invalidate();
				}
			}
		}

		[DefaultValue(true)]
		public bool ShowCount
		{
			get;
			set;
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

		public HLListView()
		{
			base.MouseDown += _MouseDown;
			base.MouseUp += _MouseUp;
			base.MouseWheel += _MouseWheel;
			base.MouseMove += _MouseMove;
			DoubleBuffered = true;
			列 = new List<HLListViewColumn>();
			物品 = new List<HLListViewItem>();
			滚动条 = new HLVScrollBar();
			HLVScrollBar hLVScrollBar = 滚动条;
			hLVScrollBar.Left = 0;
			hLVScrollBar.Top = 0;
			hLVScrollBar.Width = 10;
			hLVScrollBar.Height = 10;
			base.Controls.Add(滚动条);
			hLVScrollBar.Dock = DockStyle.Right;
			hLVScrollBar = null;
			最高栏 = 0;
			选中 = -1;
			拖动 = false;
			ShowCount = true;
			选中列 = -1;
			滚动条.ValueChanged += delegate
			{
				_Lambda_0024__9_002D0();
			};
		}

		private void _MouseDown(object sender, MouseEventArgs e)
		{
			if (类型.为空(物品.Count, 列.Count))
			{
				return;
			}
			int num = 选中;
			选中列 = -1;
			选中 = -1;
			bool flag = false;
			checked
			{
				if (e.X < 滚动条.Left)
				{
					int y = e.Y;
					int num2 = 0;
					int num3 = 0;
					if (y <= 边缘)
					{
						foreach (HLListViewColumn item in 列)
						{
							num2 = (int)(unchecked((long)num2) + unchecked((long)item.Rwidth));
							if ((num3 == 列.Count - 1 && e.X >= unchecked((long)num2) - unchecked((long)item.Rwidth)) || (e.X <= num2 - 边缘 && e.X >= unchecked((long)num2) - unchecked((long)item.Rwidth) + 边缘))
							{
								选中列 = num3;
								flag = true;
								break;
							}
							num3++;
						}
					}
					else
					{
						int num4 = y - 边缘;
						num4 = (int)Math.Round(Conversion.Int((double)num4 / (double)行高) + (double)最高栏);
						选中 = Conversions.ToInteger(Interaction.IIf(num4 < 物品.Count, num4, -1));
						flag = true;
					}
				}
				if (!flag)
				{
					return;
				}
				if (选中列 >= 0)
				{
					if (_0024STATIC_0024_MouseDown_002420211C128095_0024d_0024Init == null)
					{
						Interlocked.CompareExchange(ref _0024STATIC_0024_MouseDown_002420211C128095_0024d_0024Init, new StaticLocalInitFlag(), null);
					}
					bool lockTaken = false;
					try
					{
						Monitor.Enter(_0024STATIC_0024_MouseDown_002420211C128095_0024d_0024Init, ref lockTaken);
						if (_0024STATIC_0024_MouseDown_002420211C128095_0024d_0024Init.State == 0)
						{
							_0024STATIC_0024_MouseDown_002420211C128095_0024d_0024Init.State = 2;
							_0024STATIC_0024_MouseDown_002420211C128095_0024d = new Dictionary<int, bool>();
						}
						else if (_0024STATIC_0024_MouseDown_002420211C128095_0024d_0024Init.State == 2)
						{
							throw new IncompleteInitialization();
						}
					}
					finally
					{
						_0024STATIC_0024_MouseDown_002420211C128095_0024d_0024Init.State = 1;
						if (lockTaken)
						{
							Monitor.Exit(_0024STATIC_0024_MouseDown_002420211C128095_0024d_0024Init);
						}
					}
					if (!_0024STATIC_0024_MouseDown_002420211C128095_0024d.ContainsKey(选中列))
					{
						_0024STATIC_0024_MouseDown_002420211C128095_0024d.Add(选中列, value: true);
					}
					List<HLListViewItem> list = 物品;
					uint l = (uint)选中列;
					Dictionary<int, bool> dictionary;
					int key;
					bool A = (dictionary = _0024STATIC_0024_MouseDown_002420211C128095_0024d)[key = 选中列];
					bool 反向 = 类型.反转(ref A);
					dictionary[key] = A;
					list.Sort(new HLListViewItemSort(l, 反向));
				}
				SelectedIndexChangedEvent?.Invoke(this, new HLValueEventArgs(num, 选中));
				Invalidate();
			}
		}

		private void _MouseUp(object sender, MouseEventArgs e)
		{
			选中列 = -1;
			拖动 = false;
			Cursor = Cursors.Default;
			Invalidate();
		}

		private void _MouseWheel(object sender, MouseEventArgs e)
		{
			滚动条.PerformMouseWheel(RuntimeHelpers.GetObjectValue(sender), e);
		}

		private void _MouseMove(object sender, MouseEventArgs e)
		{
			checked
			{
				if (e.Y <= 边缘)
				{
					int num = 0;
					bool expression = false;
					foreach (HLListViewColumn item in 列)
					{
						if ((float)Math.Abs(e.X - (unchecked((long)num) + unchecked((long)item.Rwidth))) <= 10f * HL辅助信息.DPI)
						{
							expression = true;
							if (e.Button != 0)
							{
								拖动 = true;
								object A = (float)(e.X - num) / HL辅助信息.DPI;
								item.Width = Conversions.ToUInteger(类型.设最大值(ref A, (float)(base.Width - 边缘) - 30f * HL辅助信息.DPI - (float)num));
								Invalidate();
							}
							break;
						}
						num = (int)(unchecked((long)num) + unchecked((long)item.Rwidth));
					}
					Cursor = (Cursor)Interaction.IIf(expression, Cursors.SizeWE, Cursors.Default);
				}
			}
		}

		public void AddColumn(string name)
		{
			Columns.Add(new HLListViewColumn(name));
		}

		public void AddColumn(string name, uint width)
		{
			Columns.Add(new HLListViewColumn(name, width));
		}

		public void AddItem(string title, params string[] str)
		{
			Items.Add(new HLListViewItem(title, str));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			checked
			{
				行高 = (int)Math.Round(Font.GetHeight() + 3f * HL辅助信息.DPI);
				边缘 = (int)Math.Round((float)行高 + 3f * HL辅助信息.DPI);
				object A = base.Width;
				类型.设最小值(ref A, 30f * HL辅助信息.DPI);
				base.Width = Conversions.ToInteger(A);
				A = base.Height;
				类型.设最小值(ref A, 50f * HL辅助信息.DPI);
				base.Height = Conversions.ToInteger(A);
				if (选中 >= 物品.Count)
				{
					选中 = -1;
				}
				Graphics graphics = e.Graphics;
				Rectangle clientRectangle = base.ClientRectangle;
				int num = 0;
				int num2 = 0;
				int num3 = (int)Conversion.Int((double)(base.Height - 边缘 * 2) / (double)行高 - 0.5);
				int num4 = (int)Math.Round(2f * HL辅助信息.DPI);
				滚动条.Width = 边缘;
				HLVScrollBar hLVScrollBar = 滚动条;
				A = 物品.Count - num3;
				hLVScrollBar.Maximum = Conversions.ToInteger(类型.设最小值(ref A, 3));
				Graphics graphics2 = graphics;
				HL辅助信息.绘制基础矩形(graphics, clientRectangle, 按下: false, 黑框: false, HL辅助信息.内容绿);
				foreach (HLListViewColumn item in 列)
				{
					bool expression = num2 == 列.Count - 1;
					Rectangle c = new Rectangle(num, 0, Conversions.ToInteger(Interaction.IIf(expression, base.Width - num, item.Rwidth)), 边缘 - num4);
					HL辅助信息.绘制基础矩形(graphics, c, 选中列 == num2);
					string text = item.Name.Trim();
					if (num2 == 0 && ShowCount)
					{
						text = text + " (" + 物品.Count.ToString() + ")";
					}
					HL辅助信息.绘制文本(graphics, text, Font, num + num4, num4, HL辅助信息.获取文本状态(base.Enabled));
					num2++;
					num = (int)(unchecked((long)num) + unchecked((long)item.Rwidth));
					if (num >= 滚动条.Left)
					{
						break;
					}
				}
				num2 = 边缘;
				int num5 = 最高栏;
				int num6 = 最高栏 + num3 + 1;
				for (int i = num5; i <= num6 && 物品.Count > i; i++)
				{
					bool flag = true;
					HLListViewItem hLListViewItem = 物品[i];
					int num7 = 0;
					num = 0;
					if (选中 == i)
					{
						graphics2.FillRectangle(HL辅助信息.块黄笔刷, new Rectangle(0, num2, base.Width, 行高));
					}
					foreach (HLListViewColumn item2 in 列)
					{
						if (!flag)
						{
							NewLateBinding.LateCall(graphics2, null, "FillRectangle", new object[2]
							{
								Interaction.IIf(选中 == i, HL辅助信息.块黄笔刷, HL辅助信息.内容绿笔刷),
								new Rectangle(num - 2 * num4, num2, base.Width, 行高)
							}, null, null, null, IgnoreReturn: true);
						}
						string text2 = "";
						text2 = ((!flag) ? hLListViewItem.Items[num7] : hLListViewItem.Title);
						HL辅助信息.绘制文本(graphics, text2, Font, num + num4, num2, HL辅助信息.获取文本状态(base.Enabled));
						if (!flag)
						{
							num7++;
						}
						if (num7 >= hLListViewItem.Items.Count)
						{
							break;
						}
						flag = false;
						num = (int)Math.Round((float)num + (float)(double)item2.Width * HL辅助信息.DPI);
					}
					num2 += 行高;
				}
				graphics2 = null;
			}
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R9_002D1(HLVScrollBar a0, HLValueEventArgs a1)
		{
			_Lambda_0024__9_002D0();
		}

		[CompilerGenerated]
		private void _Lambda_0024__9_002D0()
		{
			最高栏 = 滚动条.Value;
			Invalidate();
		}
	}
}
