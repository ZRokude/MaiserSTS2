using System.ComponentModel.DataAnnotations;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MaiserSTS2.MaiserSTS2Code.Utility;

  
public static class CustomCardKeyword
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] public static CardKeyword Spellboost;
}