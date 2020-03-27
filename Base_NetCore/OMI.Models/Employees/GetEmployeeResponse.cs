using AutoMapper;
using OMI.Core.Mapper;
using OMI.Entity.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.DTO.Employees
{
  public class GetEmployeeResponse : IMapFrom<Employee>
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<GetEmployeeDetailsResponse> Details { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<Employee, GetEmployeeResponse>();
    }
  }
}
