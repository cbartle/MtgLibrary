using Dapper;
using System.Collection.Generic;
using MySql.Data.MySqlConnection;
using Microsoft.AspNetCore.Mvc;

namespace Spiffs.MtgTracker.Controllers
{
    [ApiController]
    [Route("Cards")]
    public class CardsController(string connectionString) : BaseController
    {
        [HttpGet]
	public async Task<IActionResult> GetCards()
	    {
                MySqlConnection connection = new MySqlConnection(connectionString);
		IEnumerable<object> results = await connection.QueryAsync<IEnumerable<object>>("SELECT * FROM Cards c Join CardColors cc ON cc.CardId = c.Id");
		return Ok(results);
            }

    }
}
