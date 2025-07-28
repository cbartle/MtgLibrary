namespace Spiff.MtgTracker
{
	public static class Constants
	{
		public static class SQL
		{
			public static string GETDECKS = @"SELECT Id, Name, Description, Format 
				FROM Decks;";
			public static string GETCARDS = @"SELECT Id, 
				Name, 
				Cost, 
				Type, 
				Effect, 
				Power, 
				Toughness, 
				Rarity, 
				ConvertedManaCost, 
				NumberOwned 
					FROM Cards;";			
			public static string ADDCARD = @"INSERT INTO Cards (Name, Cost, Type, Effect, Power, Toughness, Rarity, ConvertedManaCost, NumberOwned)
			Values( @Name,
				@Cost,
				@Type,
				@Effect,
				@Power,
				@Toughness,
				@Rarity,
				@ConvertedManaCost,
				@NumberOwned);
			SELECT LAST_INSERT_ID();";

			public const string ADDCARDCOLORS = @"INSERT INTO CardColors (CardId, Color)
				VALUES (@CardId, @Color)";
		}

		public static string CONNECTION = "DB__CONNECTIONSTRING";
	}
}
