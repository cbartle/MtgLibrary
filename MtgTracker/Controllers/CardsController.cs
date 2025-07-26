using Dapper;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using Spiff.MtgTracker.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spiff.MtgTracker;
using Spiff.MtgTracker.DAL.Helpers;
using System.Linq;

namespace Spiff.MtgTracker.Controllers
{
    [ApiController]
    [Route("Cards")]
    public class CardsController(ILogger<CardsController> _logger) : ControllerBase
    {
        [HttpGet]
	public async Task<IActionResult> GetCards()
	    {
		string connectionString = Environment.GetEnvironmentVariable("DB__CONNECTIONSTRING");
		_logger.LogInformation("We made it into the controller!!!!");
                using MySqlConnection connection = new MySqlConnection(connectionString);
		IEnumerable<Card> results = await connection.QueryAsync<Card>("SELECT * FROM Cards;");
		return Ok(results);
            }

	[HttpPost]
	public IActionResult AddCard(AddCardRequest request)
	{
		string connectionString = Environment.GetEnvironmentVariable(Constants.CONNECTION);
		_logger.LogInformation("we made it to the Add Card method");
		using MySqlConnection connection = new MySqlConnection(connectionString);
		
		connection.Open();

		using var transaction = connection.BeginTransaction();
		try
		{
			var parameters = new {
			Name = request.Name,
			Cost = request.Cost,
			Type = request.Type,
			Effect = request.Effect,
			Power = request.Power,
			Toughness = request.Toughness,
			Rarity = request.Rarity,
			ConvertedManaCost = request.ConvertedManaCost,
			NumberOwned = request.NumberOwned
			};

			int id = connection.QuerySingle<int>(Constants.SQL.ADDCARD, parameters);
		
			List<string> colors = ColorParser.ParseColorsFromCost(request.Cost);

			colors.AddRange(ColorParser.ParseColorsFromEffect(request.Effect));
		
			var colorRows = colors.Select(c => new { CardId = id, Color = c});

			connection.Execute(Constants.SQL.ADDCARDCOLORS, colorRows);
			
			transaction.Commit();
		}
		catch (Exception ex)
		{	
			_logger.LogError(ex.Message);
			transaction.Rollback();
		}

		return NoContent();
	}
    }
}
