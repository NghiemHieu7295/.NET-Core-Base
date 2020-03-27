using OMI.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.Entity.Employees
{
  public class EmployeeDetail : BaseEntity
  {
    public int Id { get; set; }
    public int EmployeeId { get; set; }

    public string Description { get; set; }
  }
}
