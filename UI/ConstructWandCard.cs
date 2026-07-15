using ImproveGame.UserInterfaces.CreateWand;
using SilkyUIFramework.Elements;
using Terraria;

namespace ImproveGameStructureDLC.UI;

public class ConstructWandCard : StructureEntryCard
{
    public ConstructWandCard() : base()
    {
        ImportingButton.LeftMouseClick += (sender, evt) =>
        {
            StructureDLCControllerViewModel.ImportConstructWand(InfoData);
        };
    }
}

public class ConstructWandCardTemplate : ISourcedUIViewTemplate 
{
    public static ConstructWandCardTemplate Instance { get; } = new();
    UIView ISourcedUIViewTemplate.ConstructFromSource(object sourceData)
    {
        if (sourceData is not StructureInfo info)
            return null;
        ConstructWandCard fileCard = new()
        {
            InfoData = info
        };
        fileCard.InnerText.Text = info.Name.ToString();
        fileCard.PreviewButton.LeftMouseClick += (sender, evt) =>
        {
            StructureDLCController.Instance.SwitchToConstructPreview((sender.Parent.Parent as ConstructWandCard)?.InfoData);
        };
        return fileCard;
    }
}