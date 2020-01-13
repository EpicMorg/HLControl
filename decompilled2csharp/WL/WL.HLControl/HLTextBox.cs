using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WL.基础;

namespace WL.HLControl
{
	[DefaultEvent("TextChanged")]
	public class HLTextBox : HLControlBase
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__5_002D0
		{
			public TextBox _0024W2;

			public HLTextBox _0024VB_0024Me;

			internal void _Lambda_0024__2(object sender, KeyEventArgs e)
			{
				_0024VB_0024Me.OnKeyDown(e);
				if (e.Control)
				{
					Keys keyCode = e.KeyCode;
					if (keyCode == Keys.A)
					{
						_0024W2.SelectAll();
					}
				}
				_0024VB_0024Me.FixScrollPos();
			}
		}

		private TextBox 文本框;

		private bool 滚动条;

		private HLVScrollBar 竖条;

		private float 边缘;

		private int 滚动条大小;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private HLLabel _HighLightLabel;

		public HLLabel HighLightLabel
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool ScrollBar
		{
			get
			{
				return 滚动条;
			}
			set
			{
				if (滚动条 != value)
				{
					滚动条 = value;
					Invalidate();
				}
			}
		}

		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		public override string Text
		{
			get
			{
				return 文本框.Text;
			}
			set
			{
				文本框.Text = value;
			}
		}

		[Browsable(false)]
		public int TextLength => 文本框.TextLength;

		[DefaultValue(false)]
		public bool AcceptsTab
		{
			get
			{
				return 文本框.AcceptsTab;
			}
			set
			{
				文本框.AcceptsTab = value;
			}
		}

		[DefaultValue(true)]
		public bool HideSelection
		{
			get
			{
				return 文本框.HideSelection;
			}
			set
			{
				文本框.HideSelection = value;
			}
		}

		public string[] Lines
		{
			get
			{
				return 文本框.Lines;
			}
			set
			{
				文本框.Lines = value;
			}
		}

		public new IntPtr Handle => 文本框.Handle;

		[DefaultValue(32767)]
		public virtual int MaxLength
		{
			get
			{
				return 文本框.MaxLength;
			}
			set
			{
				文本框.MaxLength = value;
			}
		}

		[Browsable(false)]
		public bool Modified
		{
			get
			{
				return 文本框.Modified;
			}
			set
			{
				文本框.Modified = value;
			}
		}

		[DefaultValue(false)]
		public virtual bool Multiline
		{
			get
			{
				return 文本框.Multiline;
			}
			set
			{
				文本框.Multiline = value;
				AcceptsReturn = value;
			}
		}

		[DefaultValue(false)]
		public bool ReadOnly
		{
			get
			{
				return 文本框.ReadOnly;
			}
			set
			{
				文本框.ReadOnly = value;
				FixSize();
			}
		}

		[Browsable(false)]
		public virtual string SelectedText
		{
			get
			{
				return 文本框.SelectedText;
			}
			set
			{
				文本框.SelectedText = value;
			}
		}

		[Browsable(false)]
		public int SelectionStart
		{
			get
			{
				return 文本框.SelectionStart;
			}
			set
			{
				文本框.SelectionStart = value;
			}
		}

		[Browsable(false)]
		public virtual int SelectionLength
		{
			get
			{
				return 文本框.SelectionLength;
			}
			set
			{
				文本框.SelectionLength = value;
			}
		}

		[DefaultValue(0)]
		public CharacterCasing CharacterCasing
		{
			get
			{
				return 文本框.CharacterCasing;
			}
			set
			{
				文本框.CharacterCasing = value;
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				return 文本框.AutoCompleteCustomSource;
			}
			set
			{
				文本框.AutoCompleteCustomSource = value;
			}
		}

		[Browsable(true)]
		[DefaultValue(128)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return 文本框.AutoCompleteSource;
			}
			set
			{
				文本框.AutoCompleteSource = value;
			}
		}

		[Browsable(true)]
		[DefaultValue(0)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return 文本框.AutoCompleteMode;
			}
			set
			{
				文本框.AutoCompleteMode = value;
			}
		}

		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public bool UseSystemPasswordChar
		{
			get
			{
				return 文本框.UseSystemPasswordChar;
			}
			set
			{
				文本框.UseSystemPasswordChar = value;
				文本框.PasswordChar = Conversions.ToChar(Interaction.IIf(value, "*", ""));
			}
		}

		[DefaultValue(false)]
		public bool AcceptsReturn
		{
			get
			{
				return 文本框.AcceptsReturn;
			}
			set
			{
				文本框.AcceptsReturn = value;
			}
		}

		public HLTextBox()
		{
			DoubleBuffered = true;
			HighLightLabel = null;
			边缘 = 3f * HL辅助信息.DPI;
			滚动条 = false;
			滚动条大小 = checked((int)Math.Round(25f * HL辅助信息.DPI));
			竖条 = new HLVScrollBar();
			文本框 = new TextBox();
			base.Controls.Add(竖条);
			base.Controls.Add(文本框);
			HLVScrollBar hLVScrollBar = 竖条;
			hLVScrollBar.Visible = false;
			hLVScrollBar.Top = 0;
			hLVScrollBar.Left = 0;
			hLVScrollBar.Width = 滚动条大小;
			hLVScrollBar.Height = base.Height;
			hLVScrollBar.ValueChanged += delegate(HLVScrollBar sender, HLValueEventArgs e)
			{
				引用.滚动(文本框, 垂直: true, Conversions.ToInteger(Operators.SubtractObject(e.NewValue, e.OldValue)));
			};
			hLVScrollBar = null;
			TextBox textBox = 文本框;
			textBox.BackColor = HL辅助信息.内容绿;
			textBox.TextAlign = HorizontalAlignment.Left;
			textBox.ImeMode = base.ImeMode;
			textBox.BorderStyle = BorderStyle.None;
			textBox.TextChanged += delegate
			{
				_Lambda_0024__5_002D1();
			};
			textBox.KeyDown += delegate(object sender, KeyEventArgs e)
			{
				base.OnKeyDown(e);
				if (e.Control)
				{
					Keys keyCode = e.KeyCode;
					if (keyCode == Keys.A)
					{
						textBox.SelectAll();
					}
				}
				FixScrollPos();
			};
			textBox.MouseDown += delegate(object sender, MouseEventArgs e)
			{
				base.OnMouseDown(e);
				if (类型.非空(HighLightLabel))
				{
					HighLightLabel.HighLight = true;
				}
				FixScrollPos();
			};
			textBox.MouseWheel += delegate(object sender, MouseEventArgs e)
			{
				base.OnMouseWheel(e);
				if (Multiline)
				{
					竖条.PerformMouseWheel(RuntimeHelpers.GetObjectValue(sender), e);
				}
			};
			textBox.DragDrop += delegate(object sender, EventArgs e)
			{
				base.OnDragDrop((DragEventArgs)e);
			};
			textBox.DragEnter += delegate(object sender, EventArgs e)
			{
				base.OnDragEnter((DragEventArgs)e);
			};
			textBox.DragOver += delegate(object sender, EventArgs e)
			{
				base.OnDragOver((DragEventArgs)e);
			};
			textBox.DragLeave += delegate(object sender, EventArgs e)
			{
				base.OnDragLeave(e);
			};
			textBox.GiveFeedback += delegate(object sender, EventArgs e)
			{
				base.OnGiveFeedback((GiveFeedbackEventArgs)e);
			};
			textBox.HandleCreated += delegate(object sender, EventArgs e)
			{
				base.OnHandleCreated(e);
			};
			textBox.HandleDestroyed += delegate(object sender, EventArgs e)
			{
				base.OnHandleDestroyed(e);
			};
			textBox.DoubleClick += delegate(object sender, EventArgs e)
			{
				base.OnDoubleClick(e);
			};
			textBox.Enter += delegate(object sender, EventArgs e)
			{
				base.OnEnter(e);
			};
			textBox.GotFocus += delegate(object sender, EventArgs e)
			{
				base.OnGotFocus(e);
			};
			textBox.KeyPress += delegate(object sender, EventArgs e)
			{
				base.OnKeyPress((KeyPressEventArgs)e);
			};
			textBox.KeyUp += delegate(object sender, EventArgs e)
			{
				base.OnKeyUp((KeyEventArgs)e);
			};
			textBox.Leave += delegate(object sender, EventArgs e)
			{
				base.OnLeave(e);
			};
			textBox.Click += delegate(object sender, EventArgs e)
			{
				base.OnClick(e);
			};
			textBox.LostFocus += delegate(object sender, EventArgs e)
			{
				base.OnLostFocus(e);
			};
			textBox.MouseClick += delegate(object sender, EventArgs e)
			{
				base.OnMouseClick((MouseEventArgs)e);
			};
			textBox.MouseDoubleClick += delegate(object sender, EventArgs e)
			{
				base.OnMouseDoubleClick((MouseEventArgs)e);
			};
			textBox.MouseCaptureChanged += delegate(object sender, EventArgs e)
			{
				base.OnMouseCaptureChanged(e);
			};
			textBox.MouseEnter += delegate(object sender, EventArgs e)
			{
				base.OnMouseEnter(e);
			};
			textBox.MouseLeave += delegate(object sender, EventArgs e)
			{
				base.OnMouseLeave(e);
			};
			textBox.MouseHover += delegate(object sender, EventArgs e)
			{
				base.OnMouseHover(e);
			};
			textBox.MouseMove += delegate(object sender, EventArgs e)
			{
				base.OnMouseMove((MouseEventArgs)e);
			};
			textBox.MouseUp += delegate(object sender, EventArgs e)
			{
				base.OnMouseUp((MouseEventArgs)e);
			};
			textBox.Move += delegate(object sender, EventArgs e)
			{
				base.OnMove(e);
			};
			textBox.PreviewKeyDown += delegate(object sender, EventArgs e)
			{
				base.OnPreviewKeyDown((PreviewKeyDownEventArgs)e);
			};
		}

		private void FixScrollPos()
		{
			int selectionStart = 文本框.SelectionStart;
			string 文本 = 文本.左(文本框.Text, checked((uint)selectionStart));
			竖条.ChangeValueWithoutRaiseEvent(文本.正则.检索(文本, "\r\n").Count);
		}

		private void FixSize()
		{
			checked
			{
				int num = (int)Math.Round(边缘 * 2f);
				int num2 = 0;
				int num3 = 0;
				HLVScrollBar hLVScrollBar = 竖条;
				hLVScrollBar.Top = (int)Math.Round(边缘);
				hLVScrollBar.Width = 滚动条大小;
				hLVScrollBar.Left = (int)Math.Round((float)base.Width - 边缘 - (float)hLVScrollBar.Width);
				hLVScrollBar.Height = base.Height - num;
				hLVScrollBar.Visible = (Multiline && 滚动条);
				if (hLVScrollBar.Visible)
				{
					num3 = 滚动条大小;
					hLVScrollBar.Maximum = 引用.获得文本框行数(文本框);
					hLVScrollBar.Enabled = (hLVScrollBar.Maximum > 1);
				}
				hLVScrollBar = null;
				TextBox textBox = 文本框;
				TextBox textBox2 = textBox;
				object obj = Interaction.IIf(ReadOnly, HL辅助信息.淡色, HL辅助信息.内容白);
				textBox2.ForeColor = ((obj != null) ? ((Color)obj) : default(Color));
				textBox.Font = Font;
				float num4 = 边缘;
				textBox.Left = (int)Math.Round(边缘);
				textBox.Top = (int)Math.Round(边缘);
				textBox.Width = base.Width - num - num3;
				textBox.Height = base.Height - num - num2;
				textBox.ScrollBars = ScrollBars.None;
				textBox.WordWrap = true;
				textBox = null;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			HL辅助信息.修正Dock(this, 允许横向: true, Multiline);
			if (!Multiline)
			{
				base.Height = checked((int)Math.Round((float)文本框.Height + 6f * HL辅助信息.DPI));
			}
			FixSize();
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			HL辅助信息.绘制基础矩形(e.Graphics, base.ClientRectangle, 按下: true, 黑框: false, HL辅助信息.内容绿);
			graphics = null;
		}

		public void Copy()
		{
			文本框.Copy();
		}

		public void Undo()
		{
			文本框.Undo();
		}

		public void AppendText(string text)
		{
			文本框.AppendText(text);
		}

		public void Clear()
		{
			文本框.Clear();
		}

		public void SelectAll()
		{
			文本框.SelectAll();
		}

		public void Select(int start, int length)
		{
			文本框.Select(start, length);
		}

		public void DeselectAll()
		{
			文本框.DeselectAll();
		}

		public void ScrollToCaret()
		{
			文本框.ScrollToCaret();
		}

		public void ClearUndo()
		{
			文本框.ClearUndo();
		}

		public void Cut()
		{
			文本框.Cut();
		}

		public void Paste()
		{
			文本框.Paste();
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D0(HLVScrollBar sender, HLValueEventArgs e)
		{
			引用.滚动(文本框, 垂直: true, Conversions.ToInteger(Operators.SubtractObject(e.NewValue, e.OldValue)));
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R5_002D1(object a0, EventArgs a1)
		{
			_Lambda_0024__5_002D1();
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D1()
		{
			base.OnTextChanged(null);
		}

		[CompilerGenerated]
		[CompilerGenerated]
		[DebuggerHidden]
		private void _0024VB_0024ClosureStub_OnKeyDown_MyBase(KeyEventArgs e)
		{
			base.OnKeyDown(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D3(object sender, MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (类型.非空(HighLightLabel))
			{
				HighLightLabel.HighLight = true;
			}
			FixScrollPos();
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D4(object sender, MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (Multiline)
			{
				竖条.PerformMouseWheel(RuntimeHelpers.GetObjectValue(sender), e);
			}
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D5(object sender, EventArgs e)
		{
			base.OnDragDrop((DragEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D6(object sender, EventArgs e)
		{
			base.OnDragEnter((DragEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D7(object sender, EventArgs e)
		{
			base.OnDragOver((DragEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D8(object sender, EventArgs e)
		{
			base.OnDragLeave(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D9(object sender, EventArgs e)
		{
			base.OnGiveFeedback((GiveFeedbackEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D10(object sender, EventArgs e)
		{
			base.OnHandleCreated(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D11(object sender, EventArgs e)
		{
			base.OnHandleDestroyed(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D12(object sender, EventArgs e)
		{
			base.OnDoubleClick(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D13(object sender, EventArgs e)
		{
			base.OnEnter(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D14(object sender, EventArgs e)
		{
			base.OnGotFocus(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D15(object sender, EventArgs e)
		{
			base.OnKeyPress((KeyPressEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D16(object sender, EventArgs e)
		{
			base.OnKeyUp((KeyEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D17(object sender, EventArgs e)
		{
			base.OnLeave(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D18(object sender, EventArgs e)
		{
			base.OnClick(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D19(object sender, EventArgs e)
		{
			base.OnLostFocus(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D20(object sender, EventArgs e)
		{
			base.OnMouseClick((MouseEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D21(object sender, EventArgs e)
		{
			base.OnMouseDoubleClick((MouseEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D22(object sender, EventArgs e)
		{
			base.OnMouseCaptureChanged(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D23(object sender, EventArgs e)
		{
			base.OnMouseEnter(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D24(object sender, EventArgs e)
		{
			base.OnMouseLeave(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D25(object sender, EventArgs e)
		{
			base.OnMouseHover(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D26(object sender, EventArgs e)
		{
			base.OnMouseMove((MouseEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D27(object sender, EventArgs e)
		{
			base.OnMouseUp((MouseEventArgs)e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D28(object sender, EventArgs e)
		{
			base.OnMove(e);
		}

		[CompilerGenerated]
		private void _Lambda_0024__5_002D29(object sender, EventArgs e)
		{
			base.OnPreviewKeyDown((PreviewKeyDownEventArgs)e);
		}
	}
}
