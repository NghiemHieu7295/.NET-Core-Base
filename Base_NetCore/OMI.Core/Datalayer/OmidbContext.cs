using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OMI.Core.Helper;
using OMI.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMI.Core.Datalayer
{
  public class OmiDbContext : IOmiDbContext
  {
    private readonly string _connectionString;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMemoryCache _cache;

    public OmiDbContext()
    {
    }

    public OmiDbContext(IConfiguration configuration,
        ICurrentUserService currentUserService,
        IDateTime dateTime,
        IMemoryCache cache)
    {
      _connectionString = configuration.GetConnectionString("OMIDatabase");
      _currentUserService = currentUserService;
      _dateTime = dateTime;
      _cache = cache;
    }

    public IUnitOfWork Create(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, RetryOptions retryOptions = null)
    {
      var conn = new SqlConnection(_connectionString);
      conn.Open();

      return new UnitOfWork(conn, _currentUserService, _dateTime, _cache, transactional, isolationLevel, retryOptions);
    }

    public async Task<IUnitOfWork> CreateAsync(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, RetryOptions retryOptions = null, CancellationToken cancellationToken = default)
    {
      var conn = new SqlConnection(_connectionString);
      await conn.OpenAsync(cancellationToken);
      
      return new UnitOfWork(conn, _currentUserService, _dateTime, _cache, transactional, isolationLevel, retryOptions);
    }
  }
}
