using BaseLib.Abstracts;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Powers;

public class AuthoringTomorrowPower: CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomBigIconPath => "";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[4]
        {
            new DamageVar(3, ValueProp.Move),
            new DynamicVar("DamageBoostValue", 4),
            new DynamicVar("PowerStackCount", 0),
            new CardsVar(0)
        };
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (CombatState.RoundNumber % 2 == 0)
        {
            for (int i = 0; i < DynamicVars["PowerStackCount"].BaseValue; i++)
            {
                foreach (Creature hittableEnemy in CombatState.HittableEnemies)
                {
                    await CreatureCmd.Damage(
                        choiceContext,
                        hittableEnemy,
                        CombatState.RoundNumber > 10 ? DynamicVars.Damage.BaseValue + DynamicVars["DamageBoostValue"].BaseValue 
                            : DynamicVars.Damage.BaseValue,
                        ValueProp.Unpowered,
                        Owner
                    );
                }
            }
        }
        else
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner.Player);

    }

    public void IncrementNumber(int damageVar = 0, int damageBoostVar = 0)
    {
        AssertMutable();
        DynamicVars.Cards.BaseValue++;
        DynamicVars.Damage.BaseValue += damageVar;
        DynamicVars["DamageBoostValue"].BaseValue += damageBoostVar;
    }
}