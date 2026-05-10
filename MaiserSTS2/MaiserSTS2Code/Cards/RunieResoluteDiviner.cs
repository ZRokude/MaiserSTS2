using BaseLib.Abstracts;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards;
[Pool(typeof(MaiserSTS2CardPool))]
  
public class RunieResoluteDiviner: SpellboostCardModel
{
    private const int Cost = 1;
    private const CardType Type = CardType.Attack;
    private const CardRarity Rarity = CardRarity.Common;
    private const TargetType Target = TargetType.RandomEnemy;
    
    public RunieResoluteDiviner() :
        base(Cost, Type, Rarity, Target)
    { }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => (IEnumerable<IHoverTip>)(object)new IHoverTip[1]
    {
        HoverTipFactory.FromCard<RunieResoluteDiviner>()
    };
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag>
    {
        CustomCardTags.SpellboostAdditionalEffect
    };
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
        (IEnumerable<CardKeyword>)(object)new CardKeyword[1]
        {
            CustomCardKeyword.Spellboost
        };
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[6]
        {
            new DamageVar(5, ValueProp.Move),
            new BlockVar(4, ValueProp.Move),
            new DynamicVar("HealingValue", 3),
            new CardsVar(1),
            new DynamicVar("CreateCount", 3),
            new DynamicVar("SpellboostCount", 0)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<Creature> enemies =CombatState.HittableEnemies.ToList();
        Creature RngEnemy = Owner.RunState.Rng.Shuffle.NextItem<Creature>(enemies);
        if (DynamicVars["SpellboostCount"].BaseValue >= 1)
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        if (DynamicVars["SpellboostCount"].BaseValue >= 3)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(RngEnemy)
                .Execute(choiceContext);
        if (DynamicVars["SpellboostCount"].BaseValue >= 7)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(RngEnemy)
                .Execute(choiceContext);
            await CreatureCmd.Heal(Owner.Creature, DynamicVars["HealingValue"].BaseValue);
        }
        if (DynamicVars["SpellboostCount"].BaseValue >= 10)
            await CustomUtil.CreateCardInHand<RunieResoluteDiviner>(Owner, (int)DynamicVars["CreateCount"].BaseValue
                , CombatState, new List<CardKeyword> { CardKeyword.Exhaust, CardKeyword.Retain }, isUpgraded: base.IsUpgraded);
    }

    protected override async void OnUpgrade()
    {
        DynamicVars.Damage.BaseValue += 2;
        DynamicVars["CreateCount"].BaseValue += 1;
        DynamicVars["HealingValue"].BaseValue += 2;
    }
}   