using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Powers;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards;
[Pool(typeof(MaiserSTS2CardPool))]
public class ImpalementArts : SpellboostCardModel
{
    private const int Cost = 1;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Uncommon;
    private const TargetType Target = TargetType.AnyEnemy;

    public ImpalementArts() :
        base(Cost, Type, Rarity, Target)
    {
    }
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag>
    {
        CustomCardTags.SpellboostAdditionalEffect
    };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        base.CanonicalVars.Concat((IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new DamageVar(8, ValueProp.Move),
            new CardsVar(1),
            new DynamicVar("SpellboostRequirement", 5),
        });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).Targeting(cardPlay.Target).FromCard(this)
            .Execute(choiceContext);
        if (DynamicVars["SpellboostCount"].BaseValue >= DynamicVars["SpellboostRequirement"].BaseValue)
        {
            await CustomUtil.CreateCardInHand<ImpalementArts>(
                Owner, DynamicVars.Cards.IntValue, CombatState,
                new List<CardKeyword> { CardKeyword.Ethereal });
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.BaseValue++;
        DynamicVars["SpellboostRequirement"].BaseValue -= 2;
    }
}