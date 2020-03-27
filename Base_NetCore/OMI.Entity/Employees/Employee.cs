using OMI.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.Entity.Employees
{
  public class Employee : BaseEntity
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public IList<EmployeeDetail> Details { get; set; }
  }
}
