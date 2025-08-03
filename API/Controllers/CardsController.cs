using Dapper;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using Spiff.MtgLibrary.API.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spiff.MtgLibrary.DAL.Helpers;
using Spiff.MtgLibrary.DAL.Models;
using System.Linq;
using Spiff.MtgLibrary.DAL;
using Spiff.MtgLibrary.API;

namespace Spiff.MtgLibrary.API.Controllers
{
    [ApiController]
    [Route("Cards")]
    public class CardsController(ILogger<CardsController> _logger, ICardAccess _cardAccess) : ControllerBase
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
            //If the card exists, just update the number owned by 1
            if(_cardAccess.Exists(request.Name))
            {
                Card ownedCard = _cardAccess.GetCard(request.Name);
                if(!_cardAccess.TryUpdateNumberOwned(request.Name, ownedCard.NumberOwned + 1, out ownedCard))
                {
                    return BadRequest($"Unable to update the number of copies of {request.Name} owned");
                }

                return Ok(ownedCard);
            }
            
            if(!_cardAccess.TryAddCard(request.ToDALCard(), out Card cardReturn))
            {
                return BadRequest("check logs for more details");
            }
		       
            return Ok(cardReturn);
        }

        [HttpPost]
        [Route("name")]
        public async Task<IActionResult> AddCardByName([FromQuery]string name)
        {
            if(_cardAccess.Exists(name))
            {
                Card ownedCard = _cardAccess.GetCard(name);

                if(!_cardAccess.TryUpdateNumberOwned(name, ownedCard.NumberOwned + 1, out ownedCard))
                {
                    return BadRequest($"Unable to update the number of copies of {name} owned");
                }

                return Ok(ownedCard);
            }

            Card card = await _cardAccess.AddCard(name);

            
            if(card == null)
            {
                return BadRequest("Card cannot be added");
            }

            return Ok(card);
        }
    }
}
