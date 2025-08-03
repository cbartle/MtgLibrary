using Dapper;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Spiff.MtgLibrary.DAL.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spiff.MtgLibrary.DAL.Helpers;
using System.Linq;
using System.Data;

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
        Card card = connection.QuerySingleOrDefault<Card>(Constants.SQL.GETCARD_BYNAME, new {Name = name});
        return card;
    }

    ///<inheritdoc />
    public bool TryAddCard(Card cardToAdd, out Card returnedCard)
    {
        returnedCard = null;
        if(cardToAdd == null)
        {
            _logger.LogError("The card to add was null. Unable to do any further processing");
            return false;
        }
        
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

        bool exists = connection.ExecuteScalar<bool>(Constants.SQL.CARDEXISTS_BYNAME, new {Name = cardName});

        _logger.LogDebug($"Card does {(exists ? "" : "not")} exist");
        
        return exists;
    }

    ///<inheritdoc />
    public bool TryUpdateNumberOwned(string cardName, int numberOwned, out Card card)
    {
        card = null;
        string connectionString = Environment.GetEnvironmentVariable(Constants.CONNECTION);
        using MySqlConnection connection = new MySqlConnection(connectionString);

        _logger.LogInformation($"Trying to update the number of {cardName} owned");
        connection.Open();

		using IDbTransaction transaction = connection.BeginTransaction();

        try
        {
            connection.Execute(Constants.SQL.UPDATENUMBEROWNED_BYNAME, new { Name = cardName, NumberOwned = numberOwned}, transaction: transaction);
            
            _logger.LogTrace($"Getting card {cardName} from the database");
            card = connection.QuerySingleOrDefault<Card>(Constants.SQL.GETCARD_BYNAME, new {Name = cardName});

            transaction.Commit();

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
    public async Task<Card> AddCard(string name)
    {
        _logger.LogInformation("Getting card from scryfall");
        
        Card scryfallCard = null;
        try
        {
            scryfallCard = await _externalService.GetCardByNameAsync(name);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }

        if(!TryAddCard(scryfallCard, out Card card))
        {
            _logger.LogError("Unable to add card to database after getting it from scryfall");
            throw new Exception("Unable to add card.");
        }
        return card;
    }
}
