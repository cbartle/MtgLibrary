using System.ComponentModel.DataAnnotations;
using Spiff.MtgLibrary.DAL.Models;

namespace Spiff.MtgLibrary.API.Models
{
	public class AddCardRequest
	{
		[Required]
		public string Name { get; set; }
		public string Cost { get; set; }
		[Required]
		public string Type { get; set; }
		public string OracleText { get; set;}
		public string Power { get; set; }
		public string Toughness { get; set; }
		[Required]
		public string Rarity { get; set; }
		[Required]
		public int CMC { get; set; }
		[Required]
		public int NumberOwned { get; set; }
	}
}
