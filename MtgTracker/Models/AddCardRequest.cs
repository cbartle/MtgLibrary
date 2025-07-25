using System.ComponentModel.DataAnnotations;

namespace Spiff.MtgTracker.Models
{
	public class AddCardRequest
	{
		[Required]
		public string Name { get; set; }
		public string Cost { get; set; }
		[Required]
		public string Type { get; set; }
		public string Effect { get; set;}
		public string Power { get; set; }
		public string Toughness { get; set; }
		[Required]
		public string Rarity { get; set; }
		[Required]
		public int ConvertedManaCost { get; set; }
		[Required]
		public int NumberOwned { get; set; }
	}
}
