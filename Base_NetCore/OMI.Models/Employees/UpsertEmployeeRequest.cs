using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.DTO.Employees
{
  public class UpsertEmployeeRequest
  {
    public int? EmployeeId { get; set; }
    public string EmployeeName { get; set; }

    public IList<UpsertEmployeeDetailRequest> EmployeeDetails { get; set; }
  }
}
