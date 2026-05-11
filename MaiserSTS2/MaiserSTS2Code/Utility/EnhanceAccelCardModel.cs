using MaiserSTS2.MaiserSTS2Code.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MaiserSTS2.MaiserSTS2Code.Utility;

public abstract class EnhanceAccelCardModel : MaiserSTS2Card
{
    

    public EnhanceAccelCardModel(int Cost, CardType Type, CardRarity Rarity, TargetType TargetType) :
        base(Cost, Type, Rarity, TargetType)
    {}

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!Owner.PlayerCombatState.Hand.Cards.Contains(this) && cardPlay.Card != this) return;
        if (this.Keywords.Contains(CustomCardKeyword.Enhance))
            await EnhanceEffect(DynamicVars["EnhanceCostValue"].IntValue);
        if (this.Keywords.Contains(CustomCardKeyword.Accel))
            await AccelEffect(DynamicVars["AccelCostValue"].IntValue);
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (!Owner.PlayerCombatState.Hand.Cards.Contains(this) && cardPlay.Card != this) return;
        if (this.Keywords.Contains(CustomCardKeyword.Enhance))
            await EnhanceEffect(DynamicVars["EnhanceCostValue"].IntValue);
        if (this.Keywords.Contains(CustomCardKeyword.Accel))
            await AccelEffect(DynamicVars["AccelCostValue"].IntValue);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (!Owner.PlayerCombatState.Hand.Cards.Contains(this)) return;
        if (this.Keywords.Contains(CustomCardKeyword.Enhance))
            await EnhanceEffect(DynamicVars["EnhanceCostValue"].IntValue);
        if (this.Keywords.Contains(CustomCardKeyword.Accel))
            await AccelEffect(DynamicVars["AccelCostValue"].IntValue);
    }
    
    private async Task EnhanceEffect(int costEnhance)
    {
        if (Owner.PlayerCombatState.Energy < costEnhance)
        {
            if (this.EnergyCost.GetWithModifiers(CostModifiers.All) <= this.CanonicalEnergyCost) return;
            this.EnergyCost.AddUntilPlayed(this.CanonicalEnergyCost - costEnhance);
        }
        this.EnergyCost.AddUntilPlayed(costEnhance - this.CanonicalEnergyCost);
    }

    private async Task AccelEffect(int costAccel)
    {
        if (Owner.PlayerCombatState.Energy > this.CanonicalEnergyCost)
        {
            if (this.EnergyCost.GetWithModifiers(CostModifiers.All) >= this.CanonicalEnergyCost) return;
            this.EnergyCost.AddUntilPlayed(this.CanonicalEnergyCost - costAccel);
        }
        this.EnergyCost.AddUntilPlayed(costAccel - this.CanonicalEnergyCost);
    }
}