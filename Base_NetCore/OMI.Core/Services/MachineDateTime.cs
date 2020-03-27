using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OMI.Core.Services
{
  public class MachineDateTime : IDateTime
  {
    public DateTime Now => DateTime.Now;

    public int CurrentYear => DateTime.Now.Year;

    public override string ToString()
    {
      return Now.ToString("ggyy年M月d日", new CultureInfo("ja-JP", true));
    }
  }
}
