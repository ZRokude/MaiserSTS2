using BaseLib.Abstracts;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards;
  
[Pool(typeof(MaiserSTS2CardPool))]
public class TravelersRespite: MaiserSTS2Card
{
    private const int Cost = 1;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Common;
    private const TargetType Target = TargetType.Self;
    
    public TravelersRespite() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => (IEnumerable<IHoverTip>)(object)new IHoverTip[1]
    {
        HoverTipFactory.FromCard<NaterranGreatTree>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new DynamicVar("HealingValue", 2m),
            new DynamicVar("CreateCount", 1),
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars["HealingValue"].BaseValue);
        await CustomUtil.CreateCardInHand<NaterranGreatTree>(Owner,(int)DynamicVars["CreateCount"].BaseValue, CombatState);
    }

    protected override async void OnUpgrade()
    {
        DynamicVars["HealingValue"].UpgradeValueBy(2);
    }
}   