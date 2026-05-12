using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MaiserSTS2.MaiserSTS2Code.Powers;

public class VagabondLizardPower : LastWordPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string? CustomBigIconPath => "";
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        base.CanonicalVars.Concat( (IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new CardsVar(0)
        });

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != CombatSide.Player) return;
        await CustomUtil.CreateCardInHand<BulletBike>(
            Owner.Player, DynamicVars.Cards.IntValue, CombatState, isFreeUntilPlayed: true);
        await base.AfterTurnEnd(choiceContext, side);
    }

    public void IncrementNumber(int cardNumber)
    {
        DynamicVars.Cards.BaseValue += cardNumber;
        DynamicVars["PowerStackCount"].BaseValue++;
    }
}