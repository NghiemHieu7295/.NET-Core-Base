using AutoMapper;
using log4net;
using OMI.Core.Datalayer;
using OMI.Core.ExceptionHandler;
using OMI.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OMI.Core.BusinessLayer
{
  public abstract class OmiServiceBase
  {
    protected readonly ILog _logger;
    protected readonly IOmiDbContext _context;
    protected readonly IMapper _mapper;
    protected readonly ICurrentUserService _currentUser;
    protected readonly IDateTime _serverTime;

    public OmiServiceBase(IOmiDbContext context, IMapper mapper, ICurrentUserService currentUser, IDateTime serverTime)
    {
      _logger = LogManager.GetLogger(this.GetType());
      _context = context;
      _mapper = mapper;
      _currentUser = currentUser;
      _serverTime = serverTime;
    }
  }
}
