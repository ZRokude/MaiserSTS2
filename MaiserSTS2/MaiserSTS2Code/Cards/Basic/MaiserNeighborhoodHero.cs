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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards.Basic;

[Pool(typeof(MaiserSTS2CardPool))]
public class MaiserNeighborhoodHero : MaiserSTS2Card
{
    private const int Cost = 1;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Basic;
    private const TargetType Target = TargetType.Self;
    
    public MaiserNeighborhoodHero() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => (IEnumerable<IHoverTip>)(object)new IHoverTip[1]
    {
        HoverTipFactory.FromCard<RapidFire>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[1]
        {
            new DynamicVar("CreateCount", 1)
        };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if(base.IsUpgraded)
            await CustomUtil.CreateCardInHand<RapidFire>(this.Owner, (int)DynamicVars["CreateCount"].BaseValue, this.CombatState);
        await CustomUtil.CreateCardInHand<DutifulSteed>(this.Owner, (int)DynamicVars["CreateCount"].BaseValue, this.CombatState);
        var applied = await PowerCmd.Apply<MaiserNeighborhoodHeroPower>(
            choiceContext,
            this.Owner.Creature,
            DynamicVars["CreateCount"].BaseValue,
            this.Owner.Creature,
            this);
        (applied as MaiserNeighborhoodHeroPower)?.IncrementNumber((int)DynamicVars["CreateCount"].BaseValue);
    }
    
}