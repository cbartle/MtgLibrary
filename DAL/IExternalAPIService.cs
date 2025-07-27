using Spiff.MtgLibrary.DAL.Models;

namespace Spiff.MtgLibrary.DAL;

public interface IExternalAPIService
{
    Task<Card> GetCardByNameAsync(string name);
}
