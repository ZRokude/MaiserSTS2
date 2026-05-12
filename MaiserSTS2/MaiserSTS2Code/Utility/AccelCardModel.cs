using MaiserSTS2.MaiserSTS2Code.Cards;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

public abstract class AccelCardModel : MaiserSTS2Card
{
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag>
    {
        CustomCardTags.Accel
    };

    public AccelCardModel(int Cost, CardType Type, CardRarity Rarity, TargetType TargetType) :
        base(Cost, Type, Rarity, TargetType)
    {}

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!Owner.PlayerCombatState.Hand.Cards.Contains(this) && cardPlay.Card != this) return;
        if (this.Keywords.Contains(CustomCardKeyword.Accel))
            await AccelEffect(DynamicVars["AccelCostValue"].IntValue);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (!Owner.PlayerCombatState.Hand.Cards.Contains(this)) return;
        if (this.Tags.Contains(CustomCardTags.Accel))
            await AccelEffect(DynamicVars["AccelCostValue"].IntValue);
    }

    private async Task AccelEffect(int costAccel)
    {
        if (Owner.PlayerCombatState.Energy >= this.CanonicalEnergyCost)
        {
            if (this.EnergyCost.GetWithModifiers(CostModifiers.All) <= this.CanonicalEnergyCost)
            {
                this.EnergyCost.AddUntilPlayed(this.CanonicalEnergyCost - costAccel);
            }
            if(this.Keywords.Contains(CustomCardKeyword.Accel))
                this.RemoveKeyword(CustomCardKeyword.Accel);
            return;
        }
        this.AddKeyword(CustomCardKeyword.Accel);
        this.EnergyCost.AddUntilPlayed(costAccel - this.CanonicalEnergyCost);
    }
}