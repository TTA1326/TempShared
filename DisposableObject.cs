using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Interfaces;
using Common.Logging;

namespace Common.Helpers
{
	/// <remarks>TODO:  The SuppressMessage attribute isn't working correctly.</remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Using the finalizer to track memory, connection, or other leaks.")]
	public abstract class DisposableObject : LogBase, IDisposable
	{
		#region Construction
		
		public DisposableObject(ILogger logger)
			: base(logger)
		{
			_AllocationStackTrace = new StackTraceEx().GetCallerStackTrace<DisposableObject>("DisposableObject.c'tor()");
		}

		#endregion

		#region DllImport Beep

#if DEBUG

		protected class NativeMethods
		{
			[System.Runtime.InteropServices.DllImport("kernel32.dll")]
			private static extern bool Beep(int freq, int duration);

			public static void AlertBeeps()
			{
				Beep(1000, 500); // Low frequency, longer duration
				Beep(2000, 150); // High frequency, short duration
			}
		}

#endif // #if DEBUG

		#endregion

		#region Finalizer

		~DisposableObject()
		{
			if (false == this.Disposed)
			{
				string Message =
					string.Format("{1} ERROR:  Memory leak detected {1}{0}{0}The object was allocated at...{0}{2}",
								  Environment.NewLine,
								  string.Concat(Enumerable.Repeat("*", 7)),
								  _AllocationStackTrace);

				_Log?.Write(Message);

#if DEBUG
				NativeMethods.AlertBeeps();
#endif // #if DEBUG
			}

			Dispose(false);
		}

		#endregion

		#region Properties

		public bool Disposed { get; private set; }

		#endregion

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion

		#region Virtuals

		protected virtual void DisposeManagedResources()
		{
		}

		protected virtual void DisposeUnmanagedResources()
		{
		}

		#endregion

		#region Implementation

		private void Dispose(bool disposing)
		{
			if (false == this.Disposed)
			{
				if (true == disposing)
					DisposeManagedResources();

				DisposeUnmanagedResources();

				this.Disposed = true;
			}
		}

		#endregion

		#region Protected Member Variables

		protected string _AllocationStackTrace;

		#endregion
	}
}
