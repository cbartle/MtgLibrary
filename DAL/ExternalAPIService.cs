using System.Net.Http;

namespace Spiff.MtgLibrary.DAL;

public class ExternalAPIService : IExternalAPIService
{
    private readonly HttpCilent _client;
    public ExternalAPIService(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<Card> GetCardByNameAsync(string name)
    {
        //Get the card from Scryfall
        return null;

    }
}
