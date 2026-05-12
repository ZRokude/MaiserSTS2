using BaseLib.Extensions;
using MaiserSTS2.MaiserSTS2Code.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Utility;

public abstract class SpellboostCardModel : MaiserSTS2Card
{
    public SpellboostCardModel(int Cost, CardType Type, CardRarity Rarity, TargetType TargetType) :
        base(Cost, Type, Rarity, TargetType)
    {}
    
     protected virtual IEnumerable<DynamicVar> SpellboostVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new DynamicVar("SpellboostCount", 0),
        };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        SpellboostVars;
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!cardPlay.Card.Keywords.Contains(CustomCardKeyword.Spellboost)&& cardPlay.Card.Type != CardType.Skill &&
            !cardPlay.Card.Tags.Contains(CustomCardTags.Accel))return;
        if (cardPlay.Card == this)
            DynamicVars["SpellboostCount"].BaseValue = 0;
            
        if (Owner.PlayerCombatState.Hand.Cards.Contains(this))
        {
            DynamicVars["SpellboostCount"].BaseValue++;
            if (this.Tags.Contains(CustomCardTags.SpellboostSubtractCost))
                await IfSpellboostSubtractCost(
                    (int)DynamicVars["SpellboostCount"].BaseValue > 1
                        ?  (int)DynamicVars["SpellboostCount"].BaseValue
                        : 1
                );
        }
    }

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (!this.Owner.HasPower<LastWordSpellboostPowerModel>()) return;
        var lastWordPowers = this.Owner.Creature.Powers.OfType<LastWordSpellboostPowerModel>().ToList();
        foreach (var power in lastWordPowers)
        {
            DynamicVars["SpellboostCount"].BaseValue += power.DynamicVars["Spellboost"].BaseValue;
        }
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (!this.Tags.Contains(CustomCardTags.SpellboostDamageBoost)) return 0m;
        return DynamicVars["SpellboostCount"].BaseValue;
    }

    public override decimal ModifyBlockAdditive(Creature target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (!this.Tags.Contains(CustomCardTags.SpellboostBlockBoost)) return 0m;
        return DynamicVars["SpellboostCount"].BaseValue;
    }

    private async Task IfSpellboostSubtractCost(int time= 1)
    {
        int calcCost = this.CanonicalEnergyCost - DynamicVars["SpellboostCount"].IntValue;
        if (calcCost < 0)
            this.EnergyCost.SetUntilPlayed(0);
        else if (calcCost == this.EnergyCost.GetWithModifiers(CostModifiers.All)) return;
        EnergyCost.SetUntilPlayed(calcCost);
    }
    private async Task IfSpellboostAttackBoost(int time = 1)=>
        this.DynamicVars.Damage.BaseValue += time * DynamicVars["SpellboostDamageMultiplier"].BaseValue;
    private async Task IfSpellboostBlockBoost(int time = 1) =>
        this.DynamicVars.Block.BaseValue += time * DynamicVars["SpellboostBlockMultiplier"].BaseValue;
}