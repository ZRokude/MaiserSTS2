using BaseLib.Abstracts;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
namespace MaiserSTS2.MaiserSTS2Code.Cards.Token;

[Pool(typeof(MaiserSTS2CardPool))]
public class IsabellesConjuration : MaiserSTS2Card
    {
    private const int Cost = 2;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Uncommon;
    private const TargetType Target = TargetType.Self;
    
    public IsabellesConjuration() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[1]
        {
            new CardsVar(1),
        };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await RandomDrawAttackFromPile();
        if (base.IsUpgraded)
            await RandomDrawAttackFromPile();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.BaseValue++;
    }

    private async Task RandomDrawAttackFromPile()
    {
        IEnumerable<CardModel> cards = PileType.Draw.GetPile(Owner).Cards.Where(c => c.Type == CardType.Attack && c.Pool == Pool)
            .ToList();
        if (!cards.Any()) return;
        await CardPileCmd.Add(Owner.RunState.Rng.Shuffle.NextItem(cards)!, PileType.Hand);
    }
}