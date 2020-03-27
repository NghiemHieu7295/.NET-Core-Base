using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.Core.ExceptionHandler
{
  public class BadRequestException : Exception
  {
    public BadRequestException(string message)
        : base(message)
    {
    }
  }
}
