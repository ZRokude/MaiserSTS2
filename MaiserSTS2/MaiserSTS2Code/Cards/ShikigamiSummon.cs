using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Cards;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Powers;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code;
[Pool(typeof(MaiserSTS2CardPool))]
public class ShikigamiSummon : MaiserSTS2Card
{
    private const int Cost = 1;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Common;
    private const TargetType Target = TargetType.Self;

    public ShikigamiSummon() :
        base(Cost, Type, Rarity, Target)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => (IEnumerable<CardKeyword>)(object)new CardKeyword[1]
    {
        CustomCardKeyword.Spellboost
    };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[3]
        {
            new BlockVar(6, ValueProp.Move),
            new DamageVar(6, ValueProp.Move),
            new DynamicVar("RequirementDeckCount", 5),
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        // if (Owner.Piles.Count() <= DynamicVars["RequirementDeckCount"].IntValue)
        // {
        //     List<Creature> creatures = ((CardModel)this).CombatState!.HittableEnemies.ToList();
        //     if (creatures.Count > 0)
        //     {
        //         Creature rngCreature =
        //             ((CardModel)this).Owner.RunState.Rng.Shuffle.NextItem<Creature>((IEnumerable<Creature>)creatures)!;K
        //         await CreatureCmd.Damage(
        //             choiceContext,
        //             rngCreature,
        //             DynamicVars.Damage.BaseValue,
        //             ValueProp.Unblockable,
        //             ((CardModel)this).Owner.Creature,
        //             (CardModel)(object)this);
        //     }
        // }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.BaseValue += 2;
        DynamicVars.Damage.BaseValue += 2;
    }
}