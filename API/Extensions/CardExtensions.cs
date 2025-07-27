using Spiff.MtgLibrary.DAL.Models;
using Spiff.MtgLibrary.API.Models;

namespace Spiff.MtgLibrary.API;

public static class Extensions
{
    public static Card ToDALCard(this AddCardRequest request)
    {
        return new Card()
        {
            Name = request.Name,
            Effect = request.OracleText,
            Cost = request.Cost,
            Power = request.Power,
            Toughness = request.Toughness,
            ConvertedManaCost = request.CMC,
            Type = request.Type,
            Rarity = request.Rarity,
            NumberOwned = request.NumberOwned
        };
    }
}
