using BaseLib.Abstracts;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MaiserSTS2.MaiserSTS2Code.Powers;
public class TetrasMettlePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomBigIconPath => "";
    [SavedProperty] private List<int> PowerDurationlist = new List<int>();
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[3]
        {
            new DynamicVar("TurnDurationCount", 0),
            new DynamicVar("PowerStackCount", 0),
            new CardsVar(0)
        };
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        foreach (var duration in PowerDurationlist.Where(c => c == 1).ToList())
        {
            DynamicVars["PowerStackCount"].BaseValue--;
            PowerDurationlist.Remove(duration);
        }
        if (PowerDurationlist.Any())
            DynamicVars["TurnDurationCount"].BaseValue--;
        if (DynamicVars["TurnDurationCount"].BaseValue == 0)
            await PowerCmd.Remove(this);
        
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!cardPlay.Card.Keywords.Contains(CustomCardKeyword.Machina) || cardPlay.Card.Owner != this.Owner.Player) return;
        await CardPileCmd.Draw(choiceContext,DynamicVars.Cards.BaseValue, Owner.Player);
    }
    public void IncrementNumber(int drawCount, int turnDurationCount)
    {
        AssertMutable();
        PowerDurationlist.Add(turnDurationCount);
        base.DynamicVars["PowerStackCount"].BaseValue++;
        base.DynamicVars.Cards.BaseValue += drawCount;
        if(DynamicVars["TurnDurationCount"].BaseValue < turnDurationCount)
            DynamicVars["TurnDurationCount"].BaseValue = turnDurationCount;
    }
}