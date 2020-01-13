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
	public class HLComboBox : HLControlBase
	{
		public delegate void SelectedIndexChangedEventHandler(HLComboBox sender, HLValueEventArgs e);

		private HLListBox 列表;

		private int 原高度;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private HLLabel _HighLightLabel;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _NoAllowNoSelectedItem;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SelectedIndexChangedEventHandler SelectedIndexChangedEvent;

		public HLLabel HighLightLabel
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool NoAllowNoSelectedItem
		{
			get;
			set;
		}

		[DefaultValue(1)]
		public int SmallChange
		{
			get
			{
				return 列表.SmallChange;
			}
			set
			{
				列表.SmallChange = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public List<string> Items => 列表.Items;

		[Browsable(false)]
		public int SelectedIndex
		{
			get
			{
				return 列表.SelectedIndex;
			}
			set
			{
				if (列表.SelectedIndex != value)
				{
					列表.SelectedIndex = value;
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		public string SelectedItem
		{
			get
			{
				return 列表.SelectedItem;
			}
			set
			{
				if (Operators.CompareString(列表.SelectedItem, value, TextCompare: false) != 0)
				{
					列表.SelectedItem = value;
					Invalidate();
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

		public HLComboBox()
		{
			base.MouseWheel += _MouseWheel;
			base.MouseDown += _MouseDown;
			DoubleBuffered = true;
			列表 = new HLListBox();
			base.Controls.Add(列表);
			NoAllowNoSelectedItem = true;
			HLListBox hLListBox = 列表;
			hLListBox.Visible = false;
			hLListBox.Top = 0;
			hLListBox.Width = 1;
			hLListBox.Left = 0;
			hLListBox.ShowScrollBar = true;
			hLListBox.Click += delegate
			{
				HideListBox();
			};
			hLListBox.SelectedIndexChanged += delegate(HLListBox sender, HLValueEventArgs e)
			{
				SelectedIndexChangedEvent?.Invoke(this, e);
			};
			hLListBox = null;
			原高度 = base.Height;
		}

		private void _MouseWheel(object sender, MouseEventArgs e)
		{
			if (base.Enabled && e.Y < 原高度)
			{
				SelectedIndex = Conversions.ToInteger(Operators.AddObject(SelectedIndex, Interaction.IIf(e.Delta < 0, 1, -1)));
			}
		}

		private void ShowListbox()
		{
			if (类型.为空(base.Parent) || !base.Visible || 列表.Visible || 列表.Items.Count < 1)
			{
				return;
			}
			HLListBox hLListBox = 列表;
			int count = hLListBox.Items.Count;
			checked
			{
				if (count >= 1)
				{
					hLListBox.Width = base.Width;
					hLListBox.Top = base.Height;
					hLListBox.Left = 0;
					count = hLListBox.FullHeight();
					object A = count;
					类型.设最大值(ref A, 350f * HL辅助信息.DPI);
					count = (hLListBox.Height = Conversions.ToInteger(A));
					hLListBox.Visible = true;
					base.Height += hLListBox.Height;
					if (类型.非空(HighLightLabel))
					{
						HighLightLabel.HighLight = true;
					}
					hLListBox.BringToFront();
					BringToFront();
					hLListBox = null;
					Invalidate();
				}
			}
		}

		private void HideListBox()
		{
			HLListBox hLListBox = 列表;
			checked
			{
				if (hLListBox.Visible)
				{
					hLListBox.Visible = false;
					base.Height -= hLListBox.Height;
					hLListBox = null;
					Invalidate();
				}
			}
		}

		private void _MouseDown(object sender, MouseEventArgs e)
		{
			if (!列表.Visible)
			{
				ShowListbox();
			}
			else
			{
				HideListBox();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			HL辅助信息.修正Dock(this);
			base.OnPaint(e);
			checked
			{
				if (!列表.Visible)
				{
					base.Height = (int)Math.Round(Font.GetHeight() + 6f * HL辅助信息.DPI);
					原高度 = base.Height;
				}
				if (!base.Enabled)
				{
					列表.Visible = false;
				}
				Graphics graphics = e.Graphics;
				Rectangle clientRectangle = base.ClientRectangle;
				Graphics graphics2 = graphics;
				HL辅助信息.绘制基础矩形(graphics, clientRectangle, 按下: true);
				float num = (float)((double)原高度 * 0.15);
				object A = 0.4 * (double)原高度;
				Font font = new Font("Segoe UI", Conversions.ToSingle(类型.设最大值(ref A, 20)));
				SizeF sizeF = graphics2.MeasureString("▼", font);
				int num2 = (int)Math.Round((double)((float)原高度 - sizeF.Width) * 0.5);
				int num3 = (int)Math.Round((double)((float)原高度 - sizeF.Height) * 0.5);
				HL辅助信息.绘制文本(graphics, "▼", font, base.Width - 原高度 + num2, num3, HL辅助信息.获取文本状态(base.Enabled));
				num = 3f * HL辅助信息.DPI;
				HL辅助信息.绘制文本(graphics, 列表.SelectedItem, Font, num, num, HL辅助信息.获取文本状态(base.Enabled));
				graphics2 = null;
			}
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R2_002D1(object a0, EventArgs a1)
		{
			HideListBox();
		}

		[CompilerGenerated]
		private void _Lambda_0024__2_002D0()
		{
			HideListBox();
		}

		[CompilerGenerated]
		private void _Lambda_0024__2_002D1(HLListBox sender, HLValueEventArgs e)
		{
			SelectedIndexChangedEvent?.Invoke(this, e);
		}
	}
}
