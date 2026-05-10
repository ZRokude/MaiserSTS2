using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MaiserSTS2.MaiserSTS2Code.Character;
using MaiserSTS2.MaiserSTS2Code.Extensions;
using Godot;

namespace MaiserSTS2.MaiserSTS2Code.Relics;

[Pool(typeof(MaiserSTS2RelicPool))]
public abstract class MaiserSTS2Relic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();

    protected override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();

    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}