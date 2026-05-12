using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Powers;

public class CelestialShikigamiPower : ShikigamiPower
{
}
public class DemonicShikigamiPower : ShikigamiPower
{
    
}
public class ShikigamiPower : LastWordSpellboostPowerModel 
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomBigIconPath => "";
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        base.CanonicalVars.Concat((IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new DamageVar(0, ValueProp.Unpowered),
            new BlockVar(0, ValueProp.Unpowered),
            new CardsVar(0)
        });
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != CombatSide.Player) return;
        if(DynamicVars.Damage.BaseValue > 0) await TriggerDamage(choiceContext);
        if(DynamicVars.Block.BaseValue > 0) await TriggerBlockGain();
        await base.AfterTurnEnd(choiceContext, side);
    }
    private async Task TriggerDamage(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(choiceContext, base.Owner.CombatState.HittableEnemies, DynamicVars.Damage.BaseValue, ValueProp.Unpowered, Owner);
    }
    private async Task TriggerBlockGain()
    {
        await CreatureCmd.GainBlock(Owner, DynamicVars.Block.BaseValue, ValueProp.Unpowered, null);
    }
    public void IncrementNumber(int? damage = 0, int? block = 0, int? spellboost = 0)
    {
        AssertMutable();
        base.DynamicVars["PowerStackCount"].BaseValue++;
        DynamicVars.Damage.BaseValue += (decimal)(damage ?? 0);
        DynamicVars.Block.BaseValue += (decimal)(block ?? 0);
    }
}