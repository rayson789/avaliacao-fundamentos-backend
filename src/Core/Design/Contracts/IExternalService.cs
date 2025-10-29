namespace Core.Design.Contracts;
public interface IExternalService
{
    Task<string> GetDataAsync(CancellationToken cancellationToken);
}
