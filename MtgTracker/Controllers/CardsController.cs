using Dapper;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using Spiffs.MtgTracker.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Spiffs.MtgTracker.Controllers
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
                MySqlConnection connection = new MySqlConnection(connectionString);
		IEnumerable<Card> results = await connection.QueryAsync<Card>("SELECT * FROM Cards;");
		return Ok(results);
            }
    }
}
