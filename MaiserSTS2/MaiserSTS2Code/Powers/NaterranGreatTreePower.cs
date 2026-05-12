using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MaiserSTS2.MaiserSTS2Code.Powers;
 
public class NaterranGreatTreePower :AmuletEnchantPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomBigIconPath => "";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
    {
        new CardsVar(1),
        new DynamicVar("PowerStackCount", 0)
    };
    
    public async Task IncrementNumber(int addPowerStackCount, int energyGain, PlayerChoiceContext choiceContext)
    {
        AssertMutable();
        base.DynamicVars["PowerStackCount"].BaseValue += addPowerStackCount;
        int stackCountOver = (int)DynamicVars["PowerStackCount"].BaseValue - 1;
        if (base.DynamicVars["PowerStackCount"].BaseValue > 1)
        {
            await RemoveNaturanGreatTreeStack(stackCountOver);
            await DrawNaturanGreatTreeStack(choiceContext, stackCountOver);
            if(energyGain > 0)
                await PlayerCmd.GainEnergy(energyGain * stackCountOver, Owner.Player);
        }
    }

    private async Task RemoveNaturanGreatTreeStack(int times)
        => base.DynamicVars["PowerStackCount"].BaseValue -= times;

    private async Task DrawNaturanGreatTreeStack(PlayerChoiceContext choiceContext,int drawCount)
    {
        await CardPileCmd.Draw(choiceContext, drawCount, Owner.Player);
    }
}