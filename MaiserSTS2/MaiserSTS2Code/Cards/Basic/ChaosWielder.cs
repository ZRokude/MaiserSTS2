using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MaiserSTS2.MaiserSTS2Code.Cards.Basic;

[Pool(typeof(MaiserSTS2CardPool))]
public class ChaosWielder : SpellboostCardModel
{
    
    private const int Cost = 5;
    private const CardType Type = CardType.Skill;
    private const CardRarity Rarity = CardRarity.Basic;
    private const TargetType Target = TargetType.None;
    
    public ChaosWielder() :
        base(Cost, Type, Rarity, Target)
    { }

    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag>
    {
        CustomCardTags.SpellboostSubtractCost
    };
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        base.CanonicalVars.Concat((IEnumerable<DynamicVar>)(object)new DynamicVar[]
        {
            new BlockVar(4, ValueProp.Move),
            new CardsVar(2),
            new DynamicVar("Spellboost", 1),
        });
    

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override async void OnUpgrade()
    {
        DynamicVars.Block.BaseValue += 4;
        CanonicalKeywords.AddItem(CustomCardKeyword.Spellboost);
    }
}   