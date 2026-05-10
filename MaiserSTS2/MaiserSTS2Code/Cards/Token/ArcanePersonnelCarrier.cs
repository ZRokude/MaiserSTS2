using MaiserSTS2.MaiserSTS2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MaiserSTS2.MaiserSTS2Code.Cards.Token;

public class ArcanePersonnelCarrier: MaiserSTS2Card
{
    private const int Cost = 2;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Uncommon;
    private const TargetType Target = TargetType.Self;

    public ArcanePersonnelCarrier() :
        base(Cost, Type, Rarity, Target)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new DynamicVar("DamageBonus", 1),
            new DynamicVar("BlockBonus", 3)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var applied = await PowerCmd.Apply<ArcanaPersonnelCarrierPower>(
            choiceContext,
            Owner.Creature,
            amount: DynamicVars["DamageBonus"].BaseValue,
            applier: Owner.Creature,
            cardSource: this
        );
        (applied as ArcanaPersonnelCarrierPower)?.IncrementNumber(
            (int)DynamicVars["DamageBonus"].BaseValue,
            (int)DynamicVars["BlockBonus"].BaseValue
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Block.UpgradeValueBy(2);
    }
}