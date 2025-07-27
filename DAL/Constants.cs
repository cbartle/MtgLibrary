namespace Spiff.MtgLibrary.DAL
{
	public static class Constants
	{
		public static class SQL
		{
			public const string GETDECKS = @"SELECT Id, 
				Name, 
				Description, 
				Format 
			FROM Decks;";
			
			public const string GETCARDS = @"SELECT Id, 
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
			
			public const string ADDCARD = @"INSERT INTO Cards (Name, Cost, Type, Effect, Power, Toughness, Rarity, ConvertedManaCost, NumberOwned)
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

            public const string CARDEXISTS_BYNAME = @"SELECT IF(COUNT(Name) = 1, 1, 0) AS Found
                FROM Cards
                WHERE  Name = @Name;";

            public const string DECKEXISTS = @"SELECT IF(COUNT(Name) = 1, 1, 0) As Found
                FROM Decks
                WHERE Name = @Name;";

			public const string GETCARD_BYNAME = @"SELECT Id, 
				Name, 
				Cost, 
				Type, 
				Effect, 
				Power, 
				Toughness, 
				Rarity, 
				ConvertedManaCost, 
				NumberOwned 
			FROM Cards
			WHERE Name = @Name;";

            public const string UPDATENUMBEROWNED_BYNAME = @"UPDATE Card
                SET NumberOwned = @numberOwned
                WHERE Name = @Name";


		}

		public static string CONNECTION = "DB__CONNECTIONSTRING";
	}
}
