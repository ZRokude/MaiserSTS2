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
public class NaterranGreatTree : CustomCardModel
{
    private const int Cost = 0;
    private static CardType Type = CustomCardType.Amulet;
    private const CardRarity Rarity = CardRarity.Token;
    private const TargetType Target = TargetType.Self;

    public NaterranGreatTree() :
        base(Cost, Type, Rarity, Target)
    {
    }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new CardsVar(1),
            new DynamicVar("EnergyGain", 0),
        };
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var applied = await PowerCmd.Apply<Powers.NaterranGreatTreePower>(
            choiceContext,
            Owner.Creature,
            0m,
            applier: Owner.Creature,
            cardSource: this
        );
        (applied as Powers.NaterranGreatTreePower)?.IncrementNumber(1,(int)DynamicVars["EnergyGain"].BaseValue, choiceContext );
    }

    protected override void OnUpgrade()
    {
        DynamicVars["EnergyGain"].BaseValue += 1;
    }

    
}