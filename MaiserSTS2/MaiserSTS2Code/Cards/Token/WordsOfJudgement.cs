using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Powers;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards.Token;

[Pool(typeof(MaiserSTS2CardPool))]
public class WordsOfJudgement : MaiserSTS2Card
{
    private const int Cost = 1;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Token;
    private const TargetType Target = TargetType.AnyEnemy;
    
    public WordsOfJudgement() :
        base(Cost, Type, Rarity, Target)
    { }

    public override IEnumerable<CardKeyword> CanonicalKeywords => (IEnumerable<CardKeyword>)(object)new CardKeyword[1]
        {
            CardKeyword.Exhaust
        };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        (IEnumerable<DynamicVar>)(object)new DynamicVar[2]
        {
            new DynamicVar("CardPlayedCount", 0),
            new DynamicVar("EnergyGain", 1),
        };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target.Powers.Any())
        {
            foreach (var power in cardPlay.Target.Powers)
            {
                await PowerCmd.Remove(power);
            }
        }
        if (base.IsUpgraded) await StrengthLossEffect(choiceContext, cardPlay);
        await IfThereAmuletPower(Owner);
    }
    private async Task IfThereAmuletPower(Player owner) 
    {
        if (Owner.HasPower<AmuletEnchantPowerModel>())
        {
            await PlayerCmd.GainEnergy(DynamicVars["EnergyGain"].BaseValue, owner);
        }
    }

    private async Task StrengthLossEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal predictedDamage = 0;

        foreach (var intent in cardPlay.Target.Monster.NextMove.Intents)
        {
            if (intent is AttackIntent attackIntent)
            {
                predictedDamage += attackIntent.GetSingleDamage(
                    cardPlay.Target.CombatState.Enemies,
                    cardPlay.Target);
            }
        }

        decimal neededStrengthChange = 1 - predictedDamage;

        await PowerCmd.Apply<TemporaryStrengthPower>(
            choiceContext,
            cardPlay.Target,
            (int)neededStrengthChange,
            Owner.Creature,
            this);
    }
    
}