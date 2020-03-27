using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OMI.Core.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OMI.Core.Controllers
{
  [Route("[controller]/[action]")]
  public abstract class OmiControllerBase : ControllerBase
  {
    protected readonly ILog _logger;

    public OmiControllerBase()
    {
      _logger = LogManager.GetLogger(this.GetType());
    }
  }
}
