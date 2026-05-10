using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;



namespace MaiserSTS2.MaiserSTS2Code.Utility;

public static class CustomCardTags
{
    [CustomEnum] public static CardTag SpellboostSubtractCost;
    [CustomEnum] public static CardTag SpellboostDamageBoost;
    [CustomEnum] public static CardTag SpellboostBlockBoost;
    [CustomEnum] public static CardTag SpellboostDrawBoost;
    [CustomEnum] public static CardTag SpellboostAdditionalEffect;
    [CustomEnum] public static CardTag Machina;
    [CustomEnum] public static CardTag Spellboost;
}