using System.Net.Http;
using Spiff.MtgLibrary.DAL.Models;

namespace Spiff.MtgLibrary.DAL;

public class ExternalAPIService : IExternalAPIService
{
    private readonly HttpClient _client;
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
