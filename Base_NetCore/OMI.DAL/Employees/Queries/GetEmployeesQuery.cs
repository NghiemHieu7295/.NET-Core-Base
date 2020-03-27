using Dapper;
using Dapper.Mapper;
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
  public class GetEmployeesQuery : IAsyncQuery<IEnumerable<Employee>>
  {
    private readonly string Sql = SqlResource.GetSql("Employees.GetEmployees");

    private readonly string _pattern;

    public bool RequiresCache => false;

    public GetEmployeesQuery(string pattern)
        => _pattern = pattern;

    public async Task<IEnumerable<Employee>> ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
      var parameters = new DynamicParameters(new Dictionary<string, object>
                {
                    { "@pattern", _pattern }
                });
      var employees = new Dictionary<string, Employee>();

      await connection.QueryAsync<Employee, EmployeeDetail, Employee>(Sql, (e, d) =>
      {
        if (!employees.TryGetValue(e.Id.ToString(), out Employee employee))
        {
          employees.Add(e.Id.ToString(), employee = e);
          employee.Details = new List<EmployeeDetail>();
        }

        employee.Details.Add(d);
        return employee;
      },
      param: parameters,
      commandType: CommandType.Text);

      return employees.Values.AsList();
    }
  }
}
