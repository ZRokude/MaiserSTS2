using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Powers;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards;
[Pool(typeof(MaiserSTS2CardPool))]
public class VagabondLizard : MaiserSTS2Card
{
    private const int Cost = 2;
    private const CardType Type = CardType.Attack;
    private const CardRarity Rarity = CardRarity.Uncommon;
    private const TargetType Target = TargetType.AnyEnemy;

    public VagabondLizard() :
        base(Cost, Type, Rarity, Target)
    {
    }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new DamageVar(8, ValueProp.Move),
            new CardsVar(1),
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).Targeting(cardPlay.Target)
            .FromCard(this).Execute(choiceContext);
        await CustomUtil.CreateCardInHand<DutifulSteed>(
            Owner,DynamicVars.Cards.IntValue,CombatState, isFreeUntilPlayed:true);
        var applied = await PowerCmd.Apply<VagabondLizardPower>(choiceContext,
            Owner.Creature, 0, Owner.Creature, this);
        (applied as VagabondLizardPower)?.IncrementNumber(DynamicVars.Cards.IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.BaseValue += 2;
    }
}