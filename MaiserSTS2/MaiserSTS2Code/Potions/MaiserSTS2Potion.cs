using BaseLib.Abstracts;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;

namespace MaiserSTS2.MaiserSTS2Code.Potions;

[Pool(typeof(MaiserSTS2PotionPool))]
public abstract class MaiserSTS2Potion : CustomPotionModel;