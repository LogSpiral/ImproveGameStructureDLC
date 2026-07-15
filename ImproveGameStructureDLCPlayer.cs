using ImproveGameStructureDLC.UI;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace ImproveGameStructureDLC;

public class ImproveGameStructureDLCPlayer : ModPlayer
{
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (ImproveGameStructureDLC.Instance.StructureDLCToggleKeyBind.JustPressed)
            StructureDLCController.Instance.ToggleOpen();
    }
}
