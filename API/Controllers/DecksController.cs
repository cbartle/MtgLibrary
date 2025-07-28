using Dapper;
using MySql.Data.MySqlClient;
using Spiff.MtgTracker.Models;
using Spiff.MtgTracker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spiff.MtgTracker.Controllers
{
	[ApiController]
	[Route("Decks")]
	public class DecksController(ILogger<DecksController> _logger) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetDecks()
		{
			_logger.LogInformation("we make it to the decks API");
			string connectionString = Environment.GetEnvironmentVariable(Constants.CONNECTION);
			MySqlConnection connection = new MySqlConnection(connectionString);
			IEnumerable<DeckListResponse> results = await connection.QueryAsync<DeckListResponse>(Constants.SQL.GETDECKS);
			return Ok(results);

		}
	}
}
