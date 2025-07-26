using System.Text.RegularExpressions;

namespace Spiff.MtgLibrary.DAL.Helpers
{
	public static class ColorParser
	{
		private static Dictionary<string, string> _colorMap = new Dictionary<string, string>()
		{
			{"B", "Black"},
			{"G", "Green"},
			{"R", "Red"},
			{"U", "Blue"},
			{"W", "White"},
		};

		public static List<string> ParseColorsFromEffect(string effect)
		{
			HashSet<string> colors = new HashSet<string>();
			if (string.IsNullOrWhiteSpace(effect))
			{
				return colors.ToList();
			}		
			string input = effect.ToUpperInvariant();

			IEnumerable<Match> matches = Regex.Matches(input, @"\{([^}]+)\}");
			
			foreach(Match match in matches)
			{
				var symbol = match.Groups[1].Value;
				if(_colorMap.TryGetValue(symbol, out string singleColor))
				{
					colors.Add(singleColor);
				}
				else if (symbol.Contains("/"))
				{
					string[] parts = symbol.Split("/");
					foreach (string part in parts)
					{
						if(_colorMap.TryGetValue(part, out string hybridColorPart))
						{
							colors.Add(hybridColorPart);
						}
					}
				}
			}


			return  colors.ToList();
		}

		public static List<string> ParseColorsFromCost(string cost)
		{
			HashSet<string> colors = new HashSet<string>();
			if(string.IsNullOrWhiteSpace(cost))
			{
				return colors.ToList();
			}

			foreach(char c in cost.ToUpperInvariant())
			{
				if(_colorMap.TryGetValue(c.ToString(), out string color))
				{
					colors.Add(color);
				}
			}

			return colors.ToList();
		}
	}
}
