using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WL.HLControl
{
	public class HLValueEventArgs : EventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object _OldValue;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object _NewValue;

		public object OldValue
		{
			[CompilerGenerated]
			get
			{
				return _OldValue;
			}
			[CompilerGenerated]
			set
			{
				_OldValue = RuntimeHelpers.GetObjectValue(value);
			}
		}

		public object NewValue
		{
			[CompilerGenerated]
			get
			{
				return _NewValue;
			}
			[CompilerGenerated]
			set
			{
				_NewValue = RuntimeHelpers.GetObjectValue(value);
			}
		}

		public HLValueEventArgs(object old, object @new)
		{
			OldValue = RuntimeHelpers.GetObjectValue(old);
			NewValue = RuntimeHelpers.GetObjectValue(@new);
		}

		public override string ToString()
		{
			return "{old:" + OldValue.ToString() + ", new:" + NewValue.ToString() + "}";
		}
	}
}
