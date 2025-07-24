namespace Spiff.MtgTracker
{
	public static class Constants
	{
		public static class SQL
		{
			public static string GETDECKS = "SELECT Id, Name, Description, Format FROM Decks;";
			public static string GETCARDS = "SELECT Id, Name, Cost, Type, Effect, Power, Toughness, Rarity, ConvertedManaCost, NumberOwned FROM Cards;";			
		}

		public static string CONNECTION = "DB__CONNECTIONSTRING";
	}
}
