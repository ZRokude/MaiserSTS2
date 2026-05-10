using BaseLib.Abstracts;
using MaiserSTS2.MaiserSTS2Code.Extensions;
using Godot;

namespace MaiserSTS2.MaiserSTS2Code.Character;

public class MaiserSTS2PotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => MaiserSTS2.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}