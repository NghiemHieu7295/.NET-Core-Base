using AutoMapper;
using OMI.Core.Mapper;
using OMI.Entity.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.DTO.Employees
{
  public class GetEmployeeDetailsResponse : IMapFrom<EmployeeDetail>
  {
    public int Id { get; set; }

    public string Description { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<EmployeeDetail, GetEmployeeDetailsResponse>();
    }
  }
}
