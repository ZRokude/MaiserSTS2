using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace MaiserSTS2.MaiserSTS2Code.Utility;

public class CustomUtil
{
    public static async Task<IEnumerable<CardModel>> CreateCardInHand<T>(Player owner, int count,
        ICombatState combatState, List<CardKeyword>? cardKeywords = null
        , bool isUpgraded = false, bool isFreeUntilPlayed = false, bool isFree = false)
    where T: CardModel
    {
        if (count == 0 || CombatManager.Instance.IsOverOrEnding)
        {
            return Array.Empty<CardModel>();
        }
        List<CardModel> cards = Enumerable
            .Range(0, count)
            .Select(_ =>
            {
                var card = (CardModel)combatState.CreateCard<T>(owner);
                if(isUpgraded) card.UpgradeInternal();
                if(isFreeUntilPlayed)card.SetToFreeThisTurn();
                if(isFree) card.SetToFreeThisCombat();
                if(cardKeywords != null && cardKeywords.Count > 0)
                    foreach (var keyword in cardKeywords)
                    {
                        card.AddKeyword(keyword);
                        
                    }
                
                return card;
            })
            .ToList();
         await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, owner);
        return cards;
    }
    public static async Task<IEnumerable<CardModel>> CreateCardInHand<T>(T cardModel, Player owner, int count, ICombatState combatState, List<CardKeyword>? cardKeywords = null, bool isUpgraded = false)
        where T: CardModel
    => await CreateCardInHand<T>(owner,  count, combatState, cardKeywords, isUpgraded);

    public static async Task UpdateCardPlayed<T>(T cardModel, DynamicVarSet DynamicVars)
    where T: CardModel  
    {
        DynamicVars["CardPlayedCount"].BaseValue = CombatManager.Instance.History.CardPlaysFinished.Count((entry =>
            entry.CardPlay.Card is T));
    }
}