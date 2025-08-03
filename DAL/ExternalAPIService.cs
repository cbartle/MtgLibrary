using System.Net.Http;
using Spiff.MtgLibrary.DAL.Models;
using System.IO;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;


namespace Spiff.MtgLibrary.DAL;

public class ExternalAPIService(ILogger<ExternalAPIService> _logger, HttpClient _httpClient) : IExternalAPIService
{
    public async Task<Card> GetCardByNameAsync(string name)
    {
       
        string url = $"cards/named?fuzzy={Uri.EscapeDataString(name)}";

        _logger.LogInformation($"URL being called: {_httpClient.BaseAddress}//{url}");
        
        //Get the card from Scryfall
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            _logger.LogError($"{response.StatusCode}: {response.ReasonPhrase} : {content}");
            throw new Exception($"{response.StatusCode}: {response.ReasonPhrase}");

        }
       
        string responseString = await response.Content.ReadAsStringAsync();

        _logger.LogInformation(responseString);

        using Stream stream = await response.Content.ReadAsStreamAsync();
        using JsonDocument doc = await JsonDocument.ParseAsync(stream);
        JsonElement root = doc.RootElement;
        
        string scryfallName = root.GetProperty("name").GetString();

        double cmcDouble = root.GetProperty("cmc").GetDouble();
        int cmc = (int)Math.Round(cmcDouble);

        //if(!int.TryParse(scryfallCMC, out int cmc))
        //{
        //    throw new Exception("Unable to cast converted mana cost as an integer");
        //}

        if(string.IsNullOrWhiteSpace(scryfallName))
        {
            throw new Exception("There is no name associated with the card that  came back from Scryfall");
        }
        string power = null;
        string toughness = null;
        if(root.TryGetProperty("power", out JsonElement powerElement))
        {
            power = powerElement.GetString();
        }

        if(root.TryGetProperty("toughness", out JsonElement toughnessElement))
        {
            toughness = toughnessElement.GetString();
        }

        return new Card()
        {
            Name = scryfallName,
            Cost = root.GetProperty("mana_cost").GetString() ?? "",
            Type = root.GetProperty("type_line").GetString() ?? "",
            Effect = root.GetProperty("oracle_text").GetString() ?? "",
            Power = power,
            Toughness = toughness,
            Rarity = root.GetProperty("rarity").GetString() ?? "Common",
            ConvertedManaCost = cmc,
            NumberOwned = 1
        };
    }
}
