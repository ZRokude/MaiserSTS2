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
public class GolemMarshal : AccelCardModel
{
    private const int Cost = 3;
    private const CardType Type = CardType.Power;
    private const CardRarity Rarity = CardRarity.Common;
    private const TargetType Target = TargetType.RandomEnemy;
    
    public GolemMarshal() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[3]
        {
            new BlockVar(5, ValueProp.Move),
            new DamageVar(3, ValueProp.Move),
            new CardsVar(1)
        };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (this.CanonicalEnergyCost > this.EnergyCost.GetWithModifiers(CostModifiers.All))
        {
            await CustomUtil.CreateCardInHand<DutifulSteed>(Owner,
                DynamicVars.Cards.IntValue, CombatState, isFreeUntilPlayed: true);
            return;
        }
        await CustomUtil.CreateCardInHand<ArcanePersonnelCarrier>(
            Owner, DynamicVars.Cards.IntValue, CombatState,
            isFreeUntilPlayed: true);
        var applied = await PowerCmd.Apply<GolemMarshalPower>(
            choiceContext,
            Owner.Creature,
            amount: 0,
            applier: Owner.Creature,
            cardSource: this
        );
        (applied as GolemMarshalPower)?.IncrementNumber(DynamicVars.Cards.IntValue);
    }
    protected override async void OnUpgrade()
    {
        DynamicVars.Block.BaseValue += 2;
    }
}   