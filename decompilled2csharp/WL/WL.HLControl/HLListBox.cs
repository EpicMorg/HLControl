using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	[DefaultEvent("SelectedIndexChanged")]
	public class HLListBox : HLControlBase
	{
		public delegate void SelectedIndexChangedEventHandler(HLListBox sender, HLValueEventArgs e);

		private HLVScrollBar 滚动条;

		private List<string> 物品;

		private int 最高栏;

		private int 行高;

		private int 选中;

		private float 边缘;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SelectedIndexChangedEventHandler SelectedIndexChangedEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _NoAllowNoSelectedItem;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public List<string> Items
		{
			get
			{
				Invalidate();
				return 物品;
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
					HandleDontAllow();
					滚动条.Value = 选中;
					SelectedIndexChangedEvent?.Invoke(this, new HLValueEventArgs(num, 选中));
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		public string SelectedItem
		{
			get
			{
				if (选中 > -1)
				{
					return 物品[选中];
				}
				return "";
			}
			set
			{
				if (选中 > -1 && Operators.CompareString(物品[选中], value, TextCompare: false) != 0)
				{
					物品[选中] = value;
					Invalidate();
				}
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

		[DefaultValue(true)]
		public bool NoAllowNoSelectedItem
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

		public HLListBox()
		{
			base.MouseDown += _MouseDown;
			base.MouseWheel += PerformMouseWheel;
			DoubleBuffered = true;
			边缘 = 4f * HL辅助信息.DPI;
			物品 = new List<string>();
			滚动条 = new HLVScrollBar();
			最高栏 = 0;
			选中 = -1;
			NoAllowNoSelectedItem = true;
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
					_Lambda_0024__6_002D0();
				};
				行高 = (int)Math.Round(Font.GetHeight() + 3f * HL辅助信息.DPI);
			}
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
					选中 = Conversions.ToInteger(Interaction.IIf(num < 物品.Count, num, -1));
					HandleDontAllow();
					if (num2 != 选中)
					{
						SelectedIndexChangedEvent?.Invoke(this, new HLValueEventArgs(num2, 选中));
						Invalidate();
					}
				}
			}
		}

		public void PerformMouseWheel(object sender, MouseEventArgs e)
		{
			滚动条.PerformMouseWheel(RuntimeHelpers.GetObjectValue(sender), e);
		}

		public int FullHeight()
		{
			checked
			{
				行高 = (int)Math.Round(Font.GetHeight() + 3f * HL辅助信息.DPI);
				return (int)Math.Round((float)(行高 * 物品.Count) + 边缘);
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
				int num2 = 物品.Count - num;
				HLVScrollBar hLVScrollBar = 滚动条;
				hLVScrollBar.Left = base.Width - hLVScrollBar.Width;
				hLVScrollBar.Height = base.Height;
				hLVScrollBar.Enabled = (num2 > 0);
				hLVScrollBar = null;
				Graphics graphics = e.Graphics;
				Rectangle clientRectangle = base.ClientRectangle;
				Graphics graphics2 = graphics;
				HL辅助信息.绘制基础矩形(graphics, clientRectangle);
				if (num2 < 1)
				{
					num2 = 1;
				}
				滚动条.Maximum = num2;
				int num3 = (int)Math.Round(边缘);
				int num4 = 最高栏;
				int num5 = 最高栏 + num;
				for (int i = num4; i <= num5 && 物品.Count > i; i++)
				{
					if (选中 == i)
					{
						graphics2.FillRectangle(HL辅助信息.块黄笔刷, new Rectangle(0, num3, (int)Math.Round((float)base.Width - 边缘), 行高));
					}
					HL辅助信息.绘制文本(graphics, 物品[i], Font, 边缘, num3, HL辅助信息.获取文本状态(base.Enabled));
					num3 += 行高;
				}
				graphics2 = null;
			}
		}

		private void HandleDontAllow()
		{
			if (NoAllowNoSelectedItem && 选中 < 0 && Items.Count > 0)
			{
				选中 = 0;
			}
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R6_002D1(HLVScrollBar a0, HLValueEventArgs a1)
		{
			_Lambda_0024__6_002D0();
		}

		[CompilerGenerated]
		private void _Lambda_0024__6_002D0()
		{
			最高栏 = 滚动条.Value;
			Invalidate();
		}
	}
}
