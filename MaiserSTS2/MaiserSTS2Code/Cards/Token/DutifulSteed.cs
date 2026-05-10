using BaseLib.Abstracts;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Powers;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
namespace MaiserSTS2.MaiserSTS2Code.Cards.Token;

[Pool(typeof(MaiserSTS2CardPool))]
public class DutifulSteed : MaiserSTS2Card
{
    private const int Cost = 1;
    private static CardType Type = CustomCardType.Amulet;
    private const CardRarity Rarity = CardRarity.Token;
    private const TargetType Target = TargetType.Self;
    public DutifulSteed() :
        base(Cost, Type, Rarity, Target)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => (IEnumerable<CardKeyword>)(object)new CardKeyword[1]
    {
        CardKeyword.Exhaust
    };
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new DynamicVar("DamageBonus", 1m),
            new DynamicVar("BlockBonus", 1m)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var applied = await PowerCmd.Apply<DutifulSteedPower>(
            choiceContext,
            Owner.Creature,
            amount: DynamicVars["DamageBonus"].BaseValue,
            applier: Owner.Creature,
            cardSource: this
        );
        (applied as DutifulSteedPower)?.IncrementNumber(
            (int)DynamicVars["DamageBonus"].BaseValue,
            (int)DynamicVars["BlockBonus"].BaseValue
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DamageBonus"].UpgradeValueBy(2);
        DynamicVars["BlockBonus"].UpgradeValueBy(2);
    }
}