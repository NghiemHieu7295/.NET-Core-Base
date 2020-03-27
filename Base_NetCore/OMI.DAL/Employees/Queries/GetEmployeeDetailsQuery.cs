using Dapper;
using OMI.Core.Datalayer;
using OMI.Entity.Employees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMI.DAL.Employees.Queries
{
  public class GetEmployeeDetailsQuery : IAsyncQuery<IList<EmployeeDetail>>
  {
    private const string Sql = @"
      SELECT *
      FROM [dbo].[EmployeeDetails] D
			WHERE
				D.EmployeeId = @employeeId;
		";

    private readonly int _employeeId;
    public GetEmployeeDetailsQuery(int employeeId)
        => _employeeId = employeeId;

    public bool RequiresCache => false;

    public async Task<IList<EmployeeDetail>> ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
      var results = await connection.QueryAsync<EmployeeDetail>(Sql, new
      {
        employeeId = _employeeId
      }, transaction: transaction, commandType: CommandType.Text);

      return results.AsList();
    }
  }
}
