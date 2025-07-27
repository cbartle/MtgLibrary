namespace Spiff.Spiff.MtgLibrary.DAL;

public interface IExternalAPIService
{
    Task<> GetCardByNameAsync(string name);
}