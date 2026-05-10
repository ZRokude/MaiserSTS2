using BaseLib.Abstracts;
using BaseLib.Extensions;
using MaiserSTS2.MaiserSTS2Code.Extensions;
using Godot;

namespace MaiserSTS2.MaiserSTS2Code.Powers;

public abstract class MaiserSTS2Power : CustomPowerModel
{
    //Loads from MaiserSTS2/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}