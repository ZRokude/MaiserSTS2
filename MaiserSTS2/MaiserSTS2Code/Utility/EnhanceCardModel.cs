using MaiserSTS2.MaiserSTS2Code.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MaiserSTS2.MaiserSTS2Code.Utility;

public abstract class EnhanceCardModel : MaiserSTS2Card
{
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag>
    {
        CustomCardTags.Enhance
    };

    public EnhanceCardModel(int Cost, CardType Type, CardRarity Rarity, TargetType TargetType) :
        base(Cost, Type, Rarity, TargetType)
    {}

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!Owner.PlayerCombatState.Hand.Cards.Contains(this) && cardPlay.Card != this) return;
        if (this.Keywords.Contains(CustomCardKeyword.Enhance))
            await EnhanceEffect(DynamicVars["EnhanceCostValue"].IntValue);
    }
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (!Owner.PlayerCombatState.Hand.Cards.Contains(this)) return;
        if (this.Tags.Contains(CustomCardTags.Enhance))
            await EnhanceEffect(DynamicVars["EnhanceCostValue"].IntValue);
    }
    private async Task EnhanceEffect(int costEnhance)
    {
        if (Owner.PlayerCombatState.Energy < costEnhance)
        {
            if (this.EnergyCost.GetWithModifiers(CostModifiers.All) > this.CanonicalEnergyCost)
            {
                this.EnergyCost.AddUntilPlayed(this.CanonicalEnergyCost - costEnhance);
            }
            if(this.Keywords.Contains(CustomCardKeyword.Enhance))
               this.RemoveKeyword(CustomCardKeyword.Enhance);
        }
        this.AddKeyword(CustomCardKeyword.Enhance);
        this.EnergyCost.AddUntilPlayed(costEnhance - this.CanonicalEnergyCost);
    }
}