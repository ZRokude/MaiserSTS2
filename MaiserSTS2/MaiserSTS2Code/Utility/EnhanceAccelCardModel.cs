using MaiserSTS2.MaiserSTS2Code.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MaiserSTS2.MaiserSTS2Code.Utility;

public abstract class EnhanceAccelCardModel : MaiserSTS2Card
{
    public EnhanceAccelCardModel(int Cost, CardType Type, CardRarity Rarity, TargetType TargetType) :
        base(Cost, Type, Rarity, TargetType)
    {}
}