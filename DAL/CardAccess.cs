using Dapper;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using Spiff.MtgLibrary.DAL.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spiff.MtgTracker.DAL.Helpers;
using System.Linq;

namespace Spiff.MtgLibrary.DAL;

public class CardAccess(ILogger<CardAccess> _logger, IExternalAPIService _externalService) : ICardAccess
{
    ///<inheritdoc />
    public async Task<IEnumerable<Card>> GetCards()
    {
        string connectionString = Environment.GetEnvironmentVariable("DB__CONNECTIONSTRING");
		
        using MySqlConnection connection = new MySqlConnection(connectionString);
		
        _logger.LogInformation("Getting all cards from the database");
        IEnumerable<Card> results = await connection.QueryAsync<Card>("SELECT * FROM Cards;");
        return results;
    }

    ///<inheritdoc />
    public Card GetCard(string name)
    {
        string connectionString = Environment.GetEnvironmentVariable("DB__CONNECTIONSTRING");
		
        using MySqlConnection connection = new MySqlConnection(connectionString);
		
        _logger.LogTrace($"Getting card {name} from the database");
        Card card = await connection.QuerySingleOrDefault<Card>(Constants.SQL.GETCARD_BYNAME, new {Name = name});
        return card;
    }

    ///<inheritdoc />
    public bool TryAddCard(Card cardToAdd, out Card returnedCard)
    {
        returnedCard = null;
        if (Exists(cardToAdd.Name))
        {
            _logger.LogWarning($"Card with name {cardToAdd.Name} already exists. A duplicate will not be added");
            return false;
        }

        string connectionString = Environment.GetEnvironmentVariable(Constants.CONNECTION);
		_logger.LogInformation($"Attempting to add a card with the name {cardToAdd.Name}");
		using MySqlConnection connection = new MySqlConnection(connectionString);
		
		connection.Open();

		using var transaction = connection.BeginTransaction();
		try
		{
			var parameters = new {
                Name = cardToAdd.Name,
                Cost = cardToAdd.Cost,
                Type = cardToAdd.Type,
                Effect = cardToAdd.Effect,
                Power = cardToAdd.Power,
                Toughness = cardToAdd.Toughness,
                Rarity = cardToAdd.Rarity,
                ConvertedManaCost = cardToAdd.ConvertedManaCost,
                NumberOwned = cardToAdd.NumberOwned
			};

			int id = connection.QuerySingle<int>(Constants.SQL.ADDCARD, parameters);
		
			List<string> colors = ColorParser.ParseColorsFromCost(cardToAdd.Cost);

			colors.AddRange(ColorParser.ParseColorsFromEffect(cardToAdd.Effect));
		
			var colorRows = colors.Select(c => new { CardId = id, Color = c});

			connection.Execute(Constants.SQL.ADDCARDCOLORS, colorRows);
			
			transaction.Commit();

            //Get the card from the database and set it as the out parameter
            returnedCard = GetCard(cardToAdd.Name);
            return true;
		}
		catch (Exception ex)
		{	
			_logger.LogError(ex.Message);
			transaction.Rollback();
            return false;
		}
    }

    ///<inheritdoc />
    public bool Exists(string cardName)
    {
        string connectionString = Environment.GetEnvironmentVariable("DB__CONNECTIONSTRING");
		
        using MySqlConnection connection = new MySqlConnection(connectionString);
		
        _logger.LogInformation($"Checking if card exists with name {cardName}");

        bool exists = connection.ExecuteScaler<bool>(Spiff.MtgLibrary.DAL.Constants.SQL.CARDEXISTS_ sBYNAME, new {Name = cardName});

        _logger.LogDebug($"Card does {(exists ? "" : "not")} exist");
        
        return exists;
    }
}
