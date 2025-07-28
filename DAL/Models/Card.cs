namespace Spiff.MtgLibrary.DAL.Models
{
	public class Card
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Cost { get; set; }
		public string Type { get; set; }
		public string Effect { get; set; }
		public string Power { get; set; }
		public string Toughness { get; set; }
		public string Rarity { get; set; }
		public int ConvertedManaCost {get; set; }
		public int NumberOwned {get; set; }
	}
}
