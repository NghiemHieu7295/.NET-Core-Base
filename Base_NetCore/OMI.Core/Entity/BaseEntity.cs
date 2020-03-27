using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.Core.Entity
{
  /// <summary>
  /// Define Base Entity.
  /// </summary>
  public abstract class BaseEntity
  {
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
  }
}
