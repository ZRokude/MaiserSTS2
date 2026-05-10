using BaseLib.Abstracts;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Powers;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards;
  
[Pool(typeof(MaiserSTS2CardPool))]
public class VincentThePeacekeeper : MaiserSTS2Card
{
    private const int Cost = 7;
    private const CardType Type = CardType.Power;
    private const CardRarity Rarity = CardRarity.Rare;
    private const TargetType Target = TargetType.Self;
    
    public VincentThePeacekeeper() :
        base(0, Type, Rarity, Target)
    { }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => (IEnumerable<IHoverTip>)(object)new IHoverTip[1]
    {
        HoverTipFactory.FromCard<WordsOfJudgement>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[3]
        {
            new DynamicVar("SubtractCost", 1),
            new DamageVar(5, ValueProp.Move),
            new DynamicVar("CreateCount",1),
        };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var applied = await PowerCmd.Apply<VincentThePeacekeeperPower>(
            choiceContext,
            this.Owner.Creature,
            DynamicVars["CreateCount"].BaseValue,
            this.Owner.Creature,
            this);
        (applied as VincentThePeacekeeperPower)?.IncrementNumber();
    }

    protected override async void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-2);
    }
}