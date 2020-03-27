using OMI.Core.BusinessLayer;
using OMI.DTO.Employees;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OMI.BUS.Employees
{
  public interface IEmployeeService : IBusinessService
  {
    Task<GetEmployeeResponse> Get(int id);
    Task<IEnumerable<GetEmployeeResponse>> Search(string pattern);
    Task DeleteEmployee(int id);
    Task UpsertEmployee(UpsertEmployeeRequest employee);
  }
}
