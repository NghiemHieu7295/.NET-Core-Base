using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.Core.ExceptionHandler
{
	public interface IExceptionDetector
	{
		bool ShouldRetryOn(Exception ex);
	}
}
