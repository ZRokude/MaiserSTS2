using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Powers;

public class ArcanaPersonnelCarrierPower: AmuletEnchantPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomBigIconPath => "";
    private bool _isTriggered = false;
    private bool isDealingDMG;
    private bool isGainingBlock;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[3]
        {
            new DynamicVar("DamageBonus", 0),
            new DynamicVar("BlockBonus", 0),
            new IntVar("PowerStackCount", 0)
        };
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        isDealingDMG = card.DynamicVars.Values.Any(v => v.Name == "Damage");
        isGainingBlock = card.GainsBlock;
    }
    public override decimal ModifyDamageAdditive(
        Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (!isDealingDMG) return 0m;
        _isTriggered = true;
        return base.DynamicVars["DamageBonus"].BaseValue;
    }

    public override decimal ModifyBlockAdditive(Creature target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (!isGainingBlock) return 0m;
        _isTriggered = true;
        return base.DynamicVars["BlockBonus"].BaseValue;;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!_isTriggered) return;
        _isTriggered = false;
        
        await PowerCmd.Remove(this);
        await OnAmuletEnhancedRemoved(cardPlay,Owner,(int)DynamicVars["PowerStackCount"].BaseValue, this.CombatState);
        base.DynamicVars["PowerStackCount"].BaseValue = 0;
    }

    public void IncrementNumber(int dmgAmount, int blockAmount)
    {
        AssertMutable();
        base.DynamicVars["DamageBonus"].BaseValue += dmgAmount;
        base.DynamicVars["BlockBonus"].BaseValue += blockAmount;
        base.DynamicVars["PowerStackCount"].BaseValue++;
    }
}