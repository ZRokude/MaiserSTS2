using BaseLib.Abstracts;
using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Powers;

public class GolemMarshalPower: CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomBigIconPath => "";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new BlockVar(5, ValueProp.Move),
            new DynamicVar("PowerStackCount", 0),
            new CardsVar(1)
        };
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        await CustomUtil.CreateCardInHand<ArcanePersonnelCarrier>(
            Owner.Player,
            DynamicVars.Cards.IntValue,
            CombatState,
            cardKeywords: new List<CardKeyword> { CardKeyword.Retain },
            isFreeUntilPlayed: true);
        await CreatureCmd.GainBlock(Owner, DynamicVars.Block, null);
    }
    public void IncrementNumber(int createCount)
    {
        AssertMutable();
        base.DynamicVars["PowerStackCount"].BaseValue++;
        if (DynamicVars.Cards.BaseValue == createCount) return;
            DynamicVars.Cards.BaseValue++;
    }
}