using BaseLib.Abstracts;
using HarmonyLib;
using MaiserSTS2.MaiserSTS2Code.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace MaiserSTS2.MaiserSTS2Code.Patches;

[HarmonyPatch]
public static class CustomCardTypePatches
{
    private static CardType GetFrameType(CardModel cardModel)
    {
        if (cardModel.Type == CustomCardType.Amulet)
            return CardType.Power;
        if (cardModel.Type == CustomCardType.Sigil)
            return CardType.Power;
        return CardType.None;
    }
    private unsafe static string FramePathFor(CardType frameType)
    {
        return ImageHelper.GetImagePath("atlases/compressed.sprites/card/card_frame_" + ((object)(*(CardType*)(&frameType))).ToString().ToLowerInvariant() + "_s.tres");
    }
    private unsafe static string PortraitBorderPathFor(CardType frameType)
    {
        return ImageHelper.GetImagePath("atlases/compressed.sprites/card/card_portrait_border_" + ((object)(*(CardType*)(&frameType))).ToString().ToLowerInvariant() + "_s.tres");
    }
    private unsafe static string AncientTextBgPathFor(CardType frameType)
    {
        return ImageHelper.GetImagePath("atlases/compressed.sprites/card_template/ancient_card_text_bg_" + ((object)(*(CardType*)(&frameType))).ToString().ToLowerInvariant() + ".tres");
    }
    [HarmonyPatch(typeof(CardModel), "FramePath", MethodType.Getter)]
    [HarmonyFinalizer]
    public static Exception FramePathFinalizer(CardModel __instance, Exception __exception, ref string __result)
    {
        if (__exception is ArgumentOutOfRangeException)
        {
            __result = FramePathFor(GetFrameType(__instance));
            return null;
        }
        return __exception;
    }
    [HarmonyPatch(typeof(CardModel), "PortraitBorderPath", MethodType.Getter)]
    [HarmonyFinalizer]
    public static Exception PortraitBorderPathFinalizer(CardModel __instance, Exception __exception, ref string __result)
    {
        if (__exception is ArgumentOutOfRangeException)
        {
            __result = PortraitBorderPathFor(GetFrameType(__instance));
            return null;
        }
        return __exception;
    }

    [HarmonyPatch(typeof(CardModel),"AncientTextBgPath", MethodType.Getter)]
    [HarmonyFinalizer]
    public static Exception AncientTextBgPathFinalizer(CardModel __instance, Exception __exception, ref string __result)
    {
       
        if (__exception is ArgumentOutOfRangeException)
        {
            __result = AncientTextBgPathFor(GetFrameType(__instance));
            return null;
        }
        return __exception;
    }
    private static string GetLocKey(CardType cardType)
    {
        if (cardType == CustomCardType.Amulet)
        {
            return "CARD_TYPE.AMULET";
        }
        if (cardType == CustomCardType.Sigil)
        {
            return "CARD_TYPE.SIGIL";
        }
        return "CARD_TYPE.ARTIFACT";
    }

    [HarmonyPatch(typeof(CardTypeExtensions), "ToLocString")]
    [HarmonyFinalizer]
    public static Exception ToLocStringFinalizer(CardType cardType, Exception __exception, ref LocString __result)
    {
        if (__exception != null)
        {
            __result = new LocString("gameplay_ui", GetLocKey(cardType));
            return null;
        }
        return __exception;
    }

}