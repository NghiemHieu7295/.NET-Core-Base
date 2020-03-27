using Dapper;
using Dapper.Mapper;
using OMI.Core.Datalayer;
using OMI.Entity.Employees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMI.DAL.Employees.Queries
{
  public class GetEmployeeByIdQuery : IAsyncQuery<Employee>
  {
    private const string Sql = @"
      SELECT *
      FROM [dbo].[Employees] E     
			WHERE
				E.Id = @employeeId;
		";

    private readonly int _employeeId;

    public bool RequiresCache => false;

    public GetEmployeeByIdQuery(int employeeId)
        => _employeeId = employeeId;

    public async Task<Employee> ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
      var results = await connection.QueryFirstOrDefaultAsync<Employee>(Sql, new
      {
        employeeId = _employeeId
      }, transaction: transaction, commandType: CommandType.Text);

      return results;
    }
  }
}
