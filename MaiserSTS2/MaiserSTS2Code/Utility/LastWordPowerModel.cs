using BaseLib.Abstracts;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

public abstract class LastWordSpellboostPowerModel : LastWordPowerModel
{ }
public abstract class LastWordPowerModel : CustomPowerModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        base.CanonicalVars.Concat( (IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new DynamicVar("PowerStackCount", 0)
        });
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        DynamicVars["PowerStackCount"].BaseValue = 0;
        await PowerCmd.Remove(this);
    }
}