namespace Spiff.MtgLibrary.DAL
{
    public interface ICardAccess
    {
        /// <summary> 
        /// Get all the cards 
        /// </summary>
        /// <returns> Returns all cards in the database </returns>
        Task<IEnumerable<Card>> GetCards();

        ///<summary>
        /// Get a card by name
        /// </summary>
        /// <returns>
        /// Returns the card as defined in the database
        /// </returns>
        Card GetCard(string name);

        /// <summary>
        /// Add a card by contents       
        /// </summary>
        /// <returns> Returns the card that was added </returns>
        Task<bool> TryAddCard(Card card, out Card returnedCard);

        ///<summary>
        /// Checks for the existance of a card
        /// </summary>
        /// <returns>
        /// Returns true if the card exists already
        /// </returns>
        bool Exists(stirng name);

    }
}