using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace OMI.Core.Services
{
  /// <summary>
  /// Inject into controller to detect user in http request.
  /// </summary>
  public class CurrentUserService : ICurrentUserService
  {
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
      UserId = Int32.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var tempVal) ? tempVal : (int?)null; ;
      IsAuthenticated = UserId != null;
    }

    public int? UserId { get; }

    public bool IsAuthenticated { get; }
  }
}
