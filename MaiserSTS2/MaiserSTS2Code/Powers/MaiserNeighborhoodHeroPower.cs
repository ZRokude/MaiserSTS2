using BaseLib.Abstracts;
using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MaiserSTS2.MaiserSTS2Code.Powers;

public class MaiserNeighborhoodHeroPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomBigIconPath => "";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new DynamicVar("CreateCount", 0),
            new DynamicVar("PowerStackCount", 0)
        };
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        DynamicVars["PowerStackCount"].BaseValue = 0;
        DynamicVars["CreateCount"].BaseValue = 0;
        await PowerCmd.Remove(this);
    }
    public void IncrementNumber(int createCount)
    {
        AssertMutable();
        base.DynamicVars["PowerStackCount"].BaseValue++;
        base.DynamicVars["CreateCount"].BaseValue += createCount;
    }
}
