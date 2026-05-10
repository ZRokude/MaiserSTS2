using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace MaiserSTS2.MaiserSTS2Code.Cards;
[Pool(typeof(MaiserSTS2CardPool))]
public class SorceryInSolidarity : SpellboostCardModel
{
    private const int Cost = 0;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Uncommon;
    private const TargetType Target = TargetType.Self;
    
    public SorceryInSolidarity() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new DynamicVar("SpellboostCount", 0),
            new CardsVar(1),
        };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> cards = new List<CardModel>
        {
            CombatState.CreateCard<IsabellesConjuration>(Owner),
            CombatState.CreateCard<TetrasMettle>(Owner)
        };
        if (!base.IsUpgraded)
        {
            CardModel cardModel= await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
            if (cardModel != null)
            {
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner);
            }
            return;
        }
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand,  base.Owner);
    }
}