using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OMI.BUS.Employees;
using OMI.Core.Controllers;
using OMI.DTO.Employees;

namespace OMI.API.Controllers
{
  [ApiController]  
  public class EmployeesController : OmiControllerBase
  {
    protected IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
      _service = service;
    }

    /// <summary>
    /// Get Employee by Id .
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>The instance of Employee</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetEmployeeResponse>> Get(int id)
    {
      return Ok(await _service.Get(id));
    }

    /// <summary>
    /// Search Employees by pattern.
    /// </summary>
    /// <param name="pattern">Contain string in Name</param>
    /// <returns>The instance of Employees</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetEmployeeResponse>>> Search(string pattern)
    {
      return Ok(await _service.Search(pattern));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Upsert([FromBody]UpsertEmployeeRequest employee)
    {
      await _service.UpsertEmployee(employee);

      return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
      await _service.DeleteEmployee(id);

      return NoContent();
    }
  }
}