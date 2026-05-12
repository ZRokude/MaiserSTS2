using BaseLib.Abstracts;
using BaseLib.Extensions;
using MaiserSTS2.MaiserSTS2Code.Cards;
using MaiserSTS2.MaiserSTS2Code.Cards.Token;
using MaiserSTS2.MaiserSTS2Code.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MaiserSTS2.MaiserSTS2Code.Utility;



public abstract class AmuletEnchantPowerModel : CustomPowerModel
{

    public virtual async Task OnAmuletEnhancedRemoved(CardPlay cardPlay,Creature creature, int powerCount, ICombatState combatState)
    {
        await GetMaiserNeighborhoodHeroCreateCardEffect(creature, powerCount, combatState);
        await GetVincentCreateCardEffect(creature,powerCount, combatState);
        await GetVincentSubtractCostEffect(creature.Player, powerCount);
    }

    // private async Task GetSuperchargedGuitaristEffect(CardPlay cardPlay, Creature creature, int times, ICombatState combatState)
    // {
    //     if (cardPlay.Card is not SuperchargedGuitarist) return;
    //     cardPlay.Card.Owner.PlayerCombatState.Energy += cardPlay.Card.EnergyCost.GetWithModifiers(CostModifiers.Local);
    // }
    private async Task GetMaiserNeighborhoodHeroCreateCardEffect(Creature creature, int times, ICombatState combatState)
    {
        if (!creature.Player.HasPower<MaiserNeighborhoodHeroPower>()) return;
        int maiserPowerCount = (int)creature.Powers.OfType<MaiserNeighborhoodHeroPower>().FirstOrDefault()!
            .DynamicVars["PowerStackCount"].BaseValue;
        await CustomUtil.CreateCardInHand<RapidFire>(creature.Player,maiserPowerCount*times, combatState);
    }
    private async Task GetVincentCreateCardEffect(Creature creature, int times, ICombatState combatState)
    {
        if (!creature.Player.HasPower<VincentThePeacekeeperPower>()) return;
        var vincentPower = creature.Powers.OfType<VincentThePeacekeeperPower>().FirstOrDefault()!;
        vincentPower.DynamicVars["EnhancedByAmuletCount"].BaseValue += times;
                
        await CustomUtil.CreateCardInHand<WordsOfJudgement>(creature.Player
            , (int)vincentPower.DynamicVars["PowerStackCount"].BaseValue * times,  combatState);  
    }
    private async Task GetVincentSubtractCostEffect(Player owner, int times)
    {
        var cards = owner.PlayerCombatState.Hand.Cards;
        if (!cards.Any(c => c is VincentThePeacekeeper)) return;
        for (int i = 0; i < times; i++)
        {
            foreach (var card in cards.Where(c => c is VincentThePeacekeeper))
            {
                card.EnergyCost.AddUntilPlayed(-(int)card.DynamicVars["SubtractCost"].BaseValue);
        
            }
        }    
    }
}