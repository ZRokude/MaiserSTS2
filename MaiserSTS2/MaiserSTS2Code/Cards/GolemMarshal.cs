using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code;
[Pool(typeof(MaiserSTS2CardPool))]
public class GolemMarshal : EnhanceAccelCardModel
{
    private const int Cost = 3;
    private const CardType Type = CardType.Attack;
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
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await CustomUtil.CreateCardInHand<ArcanePersonnelCarrier>(
            Owner, DynamicVars.Cards.IntValue, CombatState,
            isFreeUntilPlayed: true);
    }

    protected override async void OnUpgrade()
    {
        DynamicVars.Block.BaseValue += 2;
    }
}   