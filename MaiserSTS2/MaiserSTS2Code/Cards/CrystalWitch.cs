using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace MaiserSTS2.MaiserSTS2Code.Cards;
[Pool(typeof(MaiserSTS2CardPool))]
public class CrystalWitch: SpellboostCardModel
{
    private const int Cost = 5;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Common;
    private const TargetType Target = TargetType.Self;
    
    public CrystalWitch() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag>
    {
        CustomCardTags.SpellboostSubtractCost
    };
    public override IEnumerable<CardKeyword> CanonicalKeywords => (IEnumerable<CardKeyword>)(object)new CardKeyword[1]
    {
        CustomCardKeyword.Spellboost
    };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[1]
        {
            new CardsVar(1),
        };
   
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}