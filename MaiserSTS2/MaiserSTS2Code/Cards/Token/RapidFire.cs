using BaseLib.Abstracts;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards.Token;

[Pool(typeof(MaiserSTS2CardPool))]
public class RapidFire : MaiserSTS2Card
{
    private const int Cost = 0;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Token;
    private const TargetType Target = TargetType.AnyEnemy;

    public RapidFire() :
        base(Cost, Type, Rarity, Target)
    {
    }public override IEnumerable<CardKeyword> CanonicalKeywords => (IEnumerable<CardKeyword>)(object)new CardKeyword[1]
    {
        CardKeyword.Exhaust
    };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[3]
        {
            new DamageVar(3, ValueProp.Move),
            new DynamicVar("DamageEffect", 3),
            new IntVar("CardPlayedCount", 0)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(((DynamicVar)((CardModel)this).DynamicVars.Damage).BaseValue)
            .FromCard((CardModel)(object)this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        if (DynamicVars["CardPlayedCount"].BaseValue > 3)
        {
            List<Creature> creatures = ((CardModel)this).CombatState!.HittableEnemies.ToList();
            if (creatures.Count > 0)
            {
                Creature rngCreature =
                    ((CardModel)this).Owner.RunState.Rng.Shuffle.NextItem<Creature>((IEnumerable<Creature>)creatures)!;
                await CreatureCmd.Damage(
                    choiceContext,
                    rngCreature,
                    ((DynamicVar)((CardModel)this).DynamicVars["DamageEffect"]).BaseValue,
                    ValueProp.Unblockable,
                    ((CardModel)this).Owner.Creature,
                    (CardModel)(object)this);
            }
        }
        await CustomUtil.UpdateCardPlayed(this, DynamicVars);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["DamageEffect"].UpgradeValueBy(2);
    }

    
}