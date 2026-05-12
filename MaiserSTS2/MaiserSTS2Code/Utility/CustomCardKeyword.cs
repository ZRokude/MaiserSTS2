using System.ComponentModel.DataAnnotations;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MaiserSTS2.MaiserSTS2Code.Utility;

  
  
public static class CustomCardKeyword
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] public static CardKeyword Spellboost;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] public static CardKeyword Shikigami;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] public static CardKeyword Enhance;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] public static CardKeyword Accel;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] public static CardKeyword Machina;
}