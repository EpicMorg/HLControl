using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace WL.HLControl
{
	public abstract class HLControlBase : Control
	{
		public HLControlBase()
		{
			base.SizeChanged += delegate
			{
				NeedRePaint();
			};
			base.Resize += delegate
			{
				NeedRePaint();
			};
			base.AutoSizeChanged += delegate
			{
				NeedRePaint();
			};
			base.FontChanged += delegate
			{
				NeedRePaint();
			};
			base.EnabledChanged += delegate
			{
				NeedRePaint();
			};
			base.TextChanged += delegate
			{
				NeedRePaint();
			};
			base.TextChanged += delegate
			{
				NeedRePaint();
			};
			DoubleBuffered = true;
		}

		private void NeedRePaint()
		{
			Invalidate();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R0_002D1(object a0, EventArgs a1)
		{
			NeedRePaint();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R0_002D2(object a0, EventArgs a1)
		{
			NeedRePaint();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R0_002D3(object a0, EventArgs a1)
		{
			NeedRePaint();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R0_002D4(object a0, EventArgs a1)
		{
			NeedRePaint();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R0_002D5(object a0, EventArgs a1)
		{
			NeedRePaint();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R0_002D6(object a0, EventArgs a1)
		{
			NeedRePaint();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _Lambda_0024__R0_002D7(object a0, EventArgs a1)
		{
			NeedRePaint();
		}
	}
}
