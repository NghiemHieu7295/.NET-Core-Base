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
  public class DeleteEmployeeCommand : IAsyncCommand<int>
  {
    private const string Sql = @"
      DELETE
				[dbo].[Employees]
			WHERE
				Id = @employeeId;
		";

    private readonly int _employeeId;

    public bool RequiresTransaction => false;
    public bool RequiresCache => false;

    public DeleteEmployeeCommand(int employeeId)
        => _employeeId = employeeId;

    public Task<int> ExecuteAsync(IDbConnection connect, IDbTransaction transaction, CancellationToken cancellationToken = default)
      => connect.ExecuteAsync(Sql, new
      {
        employeeId = _employeeId
      }, transaction: transaction, commandType: CommandType.Text);
  }
}
