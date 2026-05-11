using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MaiserSTS2.MaiserSTS2Code.Utility;

public abstract class LastPowerModel : CustomPowerModel
{
    public override Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        foreach (var card in Owner.Player.PlayerCombatState.Hand.Cards.ToList())
        {
            if (card.Keywords.Contains(CustomCardKeyword.Spellboost))
            {
                
            }
        }
    }

    private async Task TriggerSpellboost() => DynamicVars["IsTriggeredLastWord"].BaseValue = 1;
}