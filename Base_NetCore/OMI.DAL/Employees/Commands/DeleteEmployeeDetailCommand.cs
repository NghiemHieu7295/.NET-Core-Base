using Dapper;
using OMI.Core.Datalayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMI.DAL.Employees.Commands
{
  public class DeleteEmployeeDetailCommand : IAsyncCommand<int>
  {
    private readonly string Sql = SqlResource.GetSql("Employees.DeleteEmployeeDetail");

    private readonly int _id;

    public bool RequiresTransaction => false;
    public bool RequiresCache => false;

    public DeleteEmployeeDetailCommand(int id)
        => _id = id;

    public Task<int> ExecuteAsync(IDbConnection connect, IDbTransaction transaction, CancellationToken cancellationToken = default)
      => connect.ExecuteAsync(Sql, new
      {
        id = _id
      }, transaction: transaction, commandType: CommandType.Text);
  }
}
