using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MaiserSTS2.MaiserSTS2Code.Cards;
[Pool(typeof(MaiserSTS2CardPool))]
public class TetrasMettle : MaiserSTS2Card
{
    private const int Cost = 2;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Uncommon;
    private const TargetType Target = TargetType.Self;

    public TetrasMettle() :
        base(Cost, Type, Rarity, Target)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new DynamicVar("TurnDurationCount", 1),
            new CardsVar(1),
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var applied = await PowerCmd.Apply<TetrasMettlePower>(
            choiceContext,
            this.Owner.Creature,
            DynamicVars.Cards.IntValue,
            this.Owner.Creature,
            this);
        (applied as TetrasMettlePower)?.IncrementNumber(DynamicVars.Cards.IntValue, DynamicVars["TurnDurationCount"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.BaseValue++;
    }
}