using MaiserSTS2.MaiserSTS2Code.Powers;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MaiserSTS2.MaiserSTS2Code.Cards.Token;

public class BulletBike : MaiserSTS2Card
{
    private const int Cost = 2;
    private static CardType Type = CustomCardType.Amulet;
    private const CardRarity Rarity = CardRarity.Uncommon;
    private const TargetType Target = TargetType.Self;

    public BulletBike() :
        base(Cost, Type, Rarity, Target)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new DynamicVar("DamageBonus", 2),
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var applied = await PowerCmd.Apply<BulletBikePower>(
            choiceContext,
            Owner.Creature,
            amount: 0,
            applier: Owner.Creature,
            cardSource: this
        );
        (applied as BulletBikePower)?.IncrementNumber(
            (int)DynamicVars["DamageBonus"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Block.UpgradeValueBy(2);
    }
}