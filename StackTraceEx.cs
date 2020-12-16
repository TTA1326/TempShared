using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
	internal class StackTraceEx
	{
		#region Implementation

		public string GetCaller<SkipT>(string defaultCaller)
			where SkipT : class
		{
			StackTrace Trace = new StackTrace(1);

			foreach (StackFrame Frame in Trace.GetFrames())
			{
				if (false == Frame.GetMethod().DeclaringType.Name.Contains(typeof(SkipT).Name, StringComparison.OrdinalIgnoreCase))
					return string.Format("{0}.{1}()", Frame.GetMethod().DeclaringType, Frame.GetMethod().Name);
			}

			return defaultCaller;
		}

		public string GetCallerStackTrace<SkipT>(string defaultCaller)
			where SkipT : class
		{
			int FramesSkipped = 0;
			StackTrace Trace = new StackTrace(1);

			foreach (StackFrame Frame in Trace.GetFrames())
			{
				if (false == Frame.GetMethod().DeclaringType.Name.Contains(typeof(SkipT).Name, StringComparison.OrdinalIgnoreCase))
					return new StackTrace(1 + FramesSkipped).ToString();

				FramesSkipped++;
			}

			return defaultCaller;
		}

		#endregion
	}
}
