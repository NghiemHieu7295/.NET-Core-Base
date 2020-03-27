using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.Core.TransferObject
{
  public interface IResquest
  {
    public int UserId { get; set; }
    public bool IsAu { get; set; }
  }
}
