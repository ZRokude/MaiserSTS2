using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards;

public class SuperchargedGuitarist: MaiserSTS2Card
{
    private const int Cost = 2;
    private const CardType Type = CardType.Attack;
    private const CardRarity Rarity = CardRarity.Uncommon;
    private const TargetType Target = TargetType.AnyEnemy;
    
    public SuperchargedGuitarist() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new DamageVar(2, ValueProp.Move),
            new CardsVar(1)
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner) return;
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .Targeting(cardPlay.Target).FromCard(this).Execute(choiceContext);
    }

    public override Task AfterModifyingDamageAmount(CardModel? cardSource)
    {
        this.Owner.PlayerCombatState.Energy += this.CanonicalEnergyCost;
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}