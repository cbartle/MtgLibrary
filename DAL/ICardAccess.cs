using Spiff.MtgLibrary.DAL.Models;

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
        bool TryAddCard(Card card, out Card returnedCard);
        
        ///<summary>
        ///Gets the card from scryfall, converts to a Card, then adds by TryAddCard
        ///</summary>
        ///<param=name>The name of the card to add</param>
        ///<returns>Returns the card that was added..</returns>
        Task<Card> AddCard(string name);

        ///<summary>
        /// Checks for the existance of a card
        /// </summary>
        /// <returns>
        /// Returns true if the card exists already
        /// </returns>
        bool Exists(string name);

        ///<summary>
        ///Try to update the number of copies owned for a card
        ///</summary>
        ///<param=cardName>The name of the card to update </param>
        ///<param=copies>The number of copiesto set </param> 
        ///<param=card>The card that was updated </param>
        ///<returns>
        ///Returns whether or not the update was successful
        ///</returns>
        bool TryUpdateNumberOwned(string cardName, int copies, out Card card);

    }
}
