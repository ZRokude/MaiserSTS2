using MaiserSTS2.MaiserSTS2Code.Powers;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MaiserSTS2.MaiserSTS2Code.Cards;

public class Demoncaller: MaiserSTS2Card
{
    private const int Cost = 2;
    private const CardType Type = CardType.Power;
    private const CardRarity Rarity = CardRarity.Common;
    private const TargetType Target = TargetType.Self;

    public Demoncaller() :
        base(Cost, Type, Rarity, Target)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => (IEnumerable<CardKeyword>)(object)new CardKeyword[1]
    {
        CustomCardKeyword.Spellboost
    };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new DynamicVar("DamageBonus", 1),
            new DynamicVar("BlockBonus", 3)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
       
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Block.UpgradeValueBy(2);
    }
}