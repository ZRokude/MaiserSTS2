using BaseLib.Abstracts;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards;

[Pool(typeof(MaiserSTS2CardPool))]
public class RivaylianBandit : EnhanceAccelCardModel
{
    private const int Cost = 1;
    private const CardType Type = CardType.Attack;
    private const CardRarity Rarity = CardRarity.Common;
    private const TargetType Target = TargetType.AnyEnemy;
    private bool _isEnhanced;
    public RivaylianBandit() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[7]
        {
            new DamageVar(1, ValueProp.Move),
            new BlockVar(4, ValueProp.Move),
            new DynamicVar("BlockBonus", 4),
            new DynamicVar("EnhanceCostValue", 3),
            new DynamicVar("EnhanceDamageValue",7),
            new DynamicVar("EnhanceBlockValue", 13),
            new DynamicVar("EnhanceBlockBonusValue", 13),
        };

    // public override async Task BeforeCardPlayed(CardPlay cardPlay)
    // {
    //     if (cardPlay.Card != this && this.Owner.PlayerCombatState.Energy < 3) return;
    //     this.EnergyCost.AddUntilPlayed(2);
    //     _isEnhanced = true;
    //    
    // }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var blockValue = DynamicVars.Block;
        var damageValue = DynamicVars.Damage;
        var blockBonusValue = DynamicVars["BlockBonus"];
        if (_isEnhanced)
        {
            damageValue.BaseValue += DynamicVars["EnhanceDamageValue"].BaseValue;
            blockValue.BaseValue += DynamicVars["EnhanceBlockValue"].BaseValue;
            blockBonusValue.BaseValue += DynamicVars["EnhanceBlockBonusValue"].BaseValue;
        }
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        if(Owner.Creature.HasPower<AmuletPowerBase>()) blockValue.BaseValue += blockBonusValue.BaseValue;
        await CreatureCmd.GainBlock(Owner.Creature, blockValue, cardPlay);
    }
    
    protected override async void OnUpgrade()
    {
        DynamicVars.Damage.BaseValue += 2;
        DynamicVars["BlockBonus"].BaseValue += 4;
    }
}   