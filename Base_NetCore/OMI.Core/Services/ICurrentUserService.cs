namespace OMI.Core.Services
{
  public interface ICurrentUserService
  {
    int? UserId { get; }

    bool IsAuthenticated { get; }
  }
}