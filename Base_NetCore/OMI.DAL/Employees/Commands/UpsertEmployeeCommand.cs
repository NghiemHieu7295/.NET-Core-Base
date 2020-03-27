using Dapper;
using OMI.Core.Datalayer;
using OMI.Entity.Employees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMI.DAL.Employees.Commands
{
  public class UpsertEmployeeCommand : IAsyncCommand<int>
  {
    private const string Sql = "sp_UpsertEmployee";

    private readonly Employee _employee;

    public bool RequiresTransaction => true;
    public bool RequiresCache => false;

    public UpsertEmployeeCommand(Employee employee)
    {
      _employee = employee;
    }

    public async Task<int> ExecuteAsync(IDbConnection connect, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
      return await connect.ExecuteAsync(Sql, new
      {
        Id = _employee.Id,
        Name = _employee.Name,
        CreatedBy = _employee.CreatedBy,
        CreatedDate = _employee.CreatedDate
      }, transaction: transaction, commandType: CommandType.StoredProcedure);
    }
  }
}
