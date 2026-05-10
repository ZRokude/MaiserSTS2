using BaseLib.Abstracts;
using BaseLib.Extensions;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Powers;

 
public class VincentThePeacekeeperPower: CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomBigIconPath => "";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[4]
        {
            new DynamicVar("CreateCount", 1),
            new DynamicVar("PowerStackCount", 0),
            new DynamicVar("EnhancedByAmuletCount", 0),
            new DamageVar(5, ValueProp.Move),
        };
    [SavedProperty] protected Dictionary<CardModel, int> _cardCounts = new();

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!cardPlay.Card.DynamicVars.ContainsKey("Damage") || !cardPlay.Card.DynamicVars.ContainsKey("Block") &&
            !cardPlay.Card.Owner.HasPower<AmuletPowerBase>())
            return;
        if(_cardCounts.ContainsKey(cardPlay.Card))
            _cardCounts[cardPlay.Card]++;
        else
            _cardCounts.Add(cardPlay.Card, 1);
        if (_cardCounts[cardPlay.Card] % 3 == 0)
            await EffectTriggerCount(choiceContext);
    }

    private async Task EffectTriggerCount(PlayerChoiceContext choiceContext)
    {
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            await CreatureCmd.Damage(choiceContext, hittableEnemy, DynamicVars.Damage.BaseValue, ValueProp.Unpowered, Owner);
        } 
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        _cardCounts.Clear();
        return Task.CompletedTask;
    }

    public void IncrementNumber()
    {
        AssertMutable();
        base.DynamicVars["PowerStackCount"].BaseValue++;
    }
}
