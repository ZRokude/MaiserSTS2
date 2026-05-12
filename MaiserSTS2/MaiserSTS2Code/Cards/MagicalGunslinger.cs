using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards;

public class MagicalGunslinger : MaiserSTS2Card
{
    private const int Cost = 1;
    private const CardType Type = CardType.Attack;
    private const CardRarity Rarity = CardRarity.Uncommon;
    private const TargetType Target = TargetType.RandomEnemy;
    
    public MagicalGunslinger() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new DamageVar(3, ValueProp.Move),
            new CardsVar(1)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await RandomDrawPowerFromPile();
        await CustomUtil.CreateCardInHand<DutifulSteed>(
            Owner, DynamicVars.Cards.IntValue, CombatState);
    }
    private async Task RandomDrawPowerFromPile()
    {
        IEnumerable<CardModel> cards = PileType.Draw.GetPile(Owner).Cards.Where(c => c.Type == CardType.Power && c.Pool == Pool)
            .ToList();
        if (!cards.Any()) return;
        await CardPileCmd.Add(Owner.RunState.Rng.Shuffle.NextItem(cards)!, PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}