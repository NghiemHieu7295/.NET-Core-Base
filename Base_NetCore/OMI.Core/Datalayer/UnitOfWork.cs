using Dapper;
using Microsoft.Extensions.Caching.Memory;
using OMI.Core.Helper;
using OMI.Core.Security.Crypt;
using OMI.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMI.Core.Datalayer
{
  public class UnitOfWork : IUnitOfWork
  {
    private bool _disposed;
    private IDbConnection _connection;
    private readonly RetryOptions _retryOptions;
    private IDbTransaction _transaction;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    private readonly IMemoryCache _cache;
    private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromSeconds(30);

    internal UnitOfWork(
      IDbConnection connection,
      ICurrentUserService currentUserService,
      IDateTime dateTime,
      IMemoryCache cache,
      bool transactional = false,
      IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
      RetryOptions retryOptions = null)
    {
      _connection = connection;
      _currentUserService = currentUserService;
      _dateTime = dateTime;
      _cache = cache;
      _retryOptions = retryOptions;

      if (transactional)
        _transaction = connection.BeginTransaction(isolationLevel);
    }

    public T Query<T>(IQuery<T> query)
    {
      if (query.RequiresCache && _cache != null)
      {
        var key = CacheHelper.GenerateSuffixKeyByParams(query);
        var hashKey = Md5Util.Md5EnCrypt(key);
        return _cache.GetOrCreate(hashKey, entry =>
        {
          entry.SlidingExpiration = _defaultCacheDuration;
          return Retry.Do(() => query.Execute(_connection, _transaction), _retryOptions);
        });
      }
      else
        return Retry.Do(() => query.Execute(_connection, _transaction), _retryOptions);
    }

    public Task<T> QueryAsync<T>(IAsyncQuery<T> query, CancellationToken cancellationToken = default)
    {
      if (query.RequiresCache && _cache != null)
      {
        var key = CacheHelper.GenerateSuffixKeyByParams(query);
        var hashKey = Md5Util.Md5EnCrypt(key);
        return _cache.GetOrCreate(hashKey, entry =>
        {
          entry.SlidingExpiration = _defaultCacheDuration;
          return Retry.DoAsync(() => query.ExecuteAsync(_connection, _transaction, cancellationToken), _retryOptions);
        });
      }
      else
        return Retry.DoAsync(() => query.ExecuteAsync(_connection, _transaction, cancellationToken), _retryOptions);
    }

    public void Execute(ICommand command)
    {
      if (command.RequiresTransaction && _transaction == null)
        throw new Exception($"The command {command.GetType()} requires a transaction");

      Retry.Do(() => command.Execute(_connection, _transaction), _retryOptions);
    }

    public T Execute<T>(ICommand<T> command)
    {
      if (command.RequiresTransaction && _transaction == null)
        throw new Exception($"The command {command.GetType()} requires a transaction");

      if (command.RequiresCache && _cache != null)
      {
        var key = CacheHelper.GenerateSuffixKeyByParams(command);
        var hashKey = Md5Util.Md5EnCrypt(key);
        return _cache.GetOrCreate(hashKey, entry =>
        {
          entry.SlidingExpiration = _defaultCacheDuration;
          return Retry.Do(() => command.Execute(_connection, _transaction), _retryOptions);
        });
      }
      else
        return Retry.Do(() => command.Execute(_connection, _transaction), _retryOptions);
    }

    public Task ExecuteAsync(IAsyncCommand command, CancellationToken cancellationToken = default)
    {
      if (command.RequiresTransaction && _transaction == null)
        throw new Exception($"The command {command.GetType()} requires a transaction");

      return Retry.DoAsync(() => command.ExecuteAsync(_connection, _transaction, cancellationToken), _retryOptions);
    }

    public Task<T> ExecuteAsync<T>(IAsyncCommand<T> command, CancellationToken cancellationToken = default)
    {
      if (command.RequiresTransaction && _transaction == null)
        throw new Exception($"The command {command.GetType()} requires a transaction");

      if (command.RequiresCache && _cache != null)
      {
        var key = CacheHelper.GenerateSuffixKeyByParams(command);
        var hashKey = Md5Util.Md5EnCrypt(key);
        return _cache.GetOrCreateAsync(hashKey, entry =>
        {
          entry.SlidingExpiration = _defaultCacheDuration;
          return Retry.DoAsync(() => command.ExecuteAsync(_connection, _transaction, cancellationToken), _retryOptions);
        });
      }
      else
        return Retry.DoAsync(() => command.ExecuteAsync(_connection, _transaction, cancellationToken), _retryOptions);
    }

    public void Commit()
      => _transaction?.Commit();

    public void Rollback()
      => _transaction?.Rollback();

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
      => Dispose(false);

    protected virtual void Dispose(bool disposing)
    {
      if (_disposed)
        return;

      if (disposing)
      {
        _transaction?.Dispose();
        _connection?.Dispose();
      }

      _transaction = null;
      _connection = null;

      _disposed = true;
    }
  }
}
