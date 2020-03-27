using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMI.Core.Datalayer
{
  public interface IQuery<out T>
  {
    bool RequiresCache { get; }

    T Execute(IDbConnection connection, IDbTransaction transaction);
  }

  public interface IAsyncQuery<T>
  {
    bool RequiresCache { get; }

    Task<T> ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default);
  }
}
