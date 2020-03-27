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
  public class UpsertEmployeeDetailCommand : IAsyncCommand<int>
  {
    private const string Sql = "sp_UpsertEmployeeDetail";

    private readonly EmployeeDetail _employeeDetail;

    public bool RequiresTransaction => true;
    public bool RequiresCache => false;

    public UpsertEmployeeDetailCommand(EmployeeDetail employeeDetail)
    {
      _employeeDetail = employeeDetail;
    }

    public async Task<int> ExecuteAsync(IDbConnection connect, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
      return await connect.ExecuteAsync(Sql, new
      {
        Id = _employeeDetail.Id,
        _employeeDetail.EmployeeId,
        Description = _employeeDetail.Description,
        CreatedBy = _employeeDetail.CreatedBy,
        CreatedDate = _employeeDetail.CreatedDate
      }, transaction: transaction, commandType: CommandType.StoredProcedure);
    }
  }
}
