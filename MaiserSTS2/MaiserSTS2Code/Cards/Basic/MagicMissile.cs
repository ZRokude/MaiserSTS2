using BaseLib.Abstracts;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards.Basic;

[Pool(typeof(MaiserSTS2CardPool))]
public class MagicMissile : MaiserSTS2Card
{
    private const int Cost = 1;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Basic;
    private const TargetType Target = TargetType.AnyEnemy;
    
    public MagicMissile() :
        base(Cost, Type, Rarity, Target)
    { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new DamageVar(4, ValueProp.Move),
            new CardsVar(1),
        };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override async void OnUpgrade()
    {
        DynamicVars.Damage.BaseValue += 2;
    }
}   
