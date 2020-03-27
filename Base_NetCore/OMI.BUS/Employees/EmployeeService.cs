using AutoMapper;
using AutoMapper.QueryableExtensions;
using OMI.Core.BusinessLayer;
using OMI.Core.Datalayer;
using OMI.Core.ExceptionHandler;
using OMI.Core.Services;
using OMI.DAL.Employees.Commands;
using OMI.DAL.Employees.Queries;
using OMI.DTO.Employees;
using OMI.Entity.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMI.BUS.Employees
{
  public class EmployeeService : OmiServiceBase, IEmployeeService
  {
    public EmployeeService(IOmiDbContext context, IMapper mapper, ICurrentUserService currentUser, IDateTime serverTime) : base(context, mapper, currentUser, serverTime)
    {
    }

    public async Task DeleteEmployee(int id)
    {
      using (var uow = await _context.CreateAsync())
      {
        var entity = await uow.QueryAsync(new GetEmployeeByIdQuery(id));

        if (entity == null)
        {
          throw new NotFoundException(nameof(Entity.Employees.Employee), id);
        }
        
        await uow.ExecuteAsync(new DeleteEmployeeCommand(id));
      }
    }

    public async Task<GetEmployeeResponse> Get(int id)
    {
      using (var uow = await _context.CreateAsync())
      {
        var result = await uow.QueryAsync(new GetEmployeeByIdQuery(id));
        result.Details = await uow.QueryAsync(new GetEmployeeDetailsQuery(id));

        return _mapper.Map<GetEmployeeResponse>(result);
      }
    }

    public async Task<IEnumerable<GetEmployeeResponse>> Search(string pattern)
    {
      using (var uow = await _context.CreateAsync())
      {
        var results = await uow.QueryAsync(new GetEmployeesQuery(pattern));

        var vm = results.AsQueryable().ProjectTo<GetEmployeeResponse>(_mapper.ConfigurationProvider).ToList();

        return vm;
      }
    }

    public async Task UpsertEmployee(UpsertEmployeeRequest request)
    {
      using (var uow = await _context.CreateAsync(transactional: true))
      {
        Employee entity;
        if (request.EmployeeId.HasValue)
        {
          entity = await uow.QueryAsync(new GetEmployeeByIdQuery(request.EmployeeId.Value));

          if (entity == null) throw new NotFoundException(nameof(Employee), request);

          entity.Details = await uow.QueryAsync(new GetEmployeeDetailsQuery(entity.Id));
        }
        else
        {
          entity = new Employee()
          {
            Name = request.EmployeeName,
            CreatedBy = _currentUser.UserId ?? 1,  // TODO: must throw excetion if curentuser is null 
            CreatedDate = _serverTime.Now
          };

          entity.Id = await uow.ExecuteAsync(new UpsertEmployeeCommand(entity));
        }

        foreach (var detail in request.EmployeeDetails)
        {
          var entityDetail = new EmployeeDetail()
          {
            Id = detail.Id,
            Description = detail.Description,
            EmployeeId = entity.Id,
            CreatedBy = _currentUser.UserId ?? 1,  // TODO: must throw excetion if curentuser is null 
            CreatedDate = _serverTime.Now
          };

          detail.Id = await uow.ExecuteAsync(new UpsertEmployeeDetailCommand(entityDetail));
        }

        // delete detail info
        var deleteDetails = entity.Details?.Where(d => request.EmployeeDetails?.Any(r => r.Id != d.Id) ?? false);
        if (deleteDetails?.Count() > 0)
        {
          foreach (var detail in deleteDetails)
          {
            await uow.ExecuteAsync(new DeleteEmployeeDetailCommand(detail.Id));
          }
        }

        uow.Commit();
      }
    }
  }
}
