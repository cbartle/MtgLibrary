using Spiff.MtgLibrary.DAL.Models;

namespace Spiff.MtgTracker.Models
{
	public class CardResponse
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Cost { get; set; }
		public string Type { get; set; }
		public string OracleText { get; set; }
		public string Power { get; set; }
		public string Toughness { get; set; }
		public string Rarity { get; set; }
		public int CMC {get; set; }
		public int NumberOwned {get; set; }

		public CardResponse(Card card)
		{
			this.Id = card.Id;
			this.Name = card.Name;
			this.Cost = card.Cost;
			this.Type = card.Type;
			this.OracleText = card.Effect;
			this.Power = card.Power;
			this.Toughness = card.Toughness;
			this.Rarity = card.Rarity;
			this.CMC = card.ConvertedManaCost;
			this.NumberOwned = card.NumberOwned;
		}
	}
}
