using ImproveGame.UserInterfaces.CreateWand;
using SilkyUIFramework.Elements;
using Terraria;

namespace ImproveGameStructureDLC.UI;

public class CreateWandCard : StructureEntryCard
{
    public CreateWandCard() : base()
    {
        ImportingButton.LeftMouseClick += (sender, evt) =>
        {
            StructureDLCControllerViewModel.ImportCreateWand(InfoData);
        };
    }
}

public class CreateWandCardTemplate : ISourcedUIViewTemplate 
{
    public static CreateWandCardTemplate Instance { get; } = new();
    UIView ISourcedUIViewTemplate.ConstructFromSource(object sourceData)
    {
        if (sourceData is not StructureInfo info)
            return null;
        CreateWandCard fileCard = new()
        {
            InfoData = info
        };
        fileCard.InnerText.Text = info.Name.ToString();
        fileCard.PreviewButton.LeftMouseClick += (sender, evt) =>
        {
            StructureDLCController.Instance.SwitchToCreatePreview((sender.Parent.Parent as CreateWandCard)?.InfoData);
        };
        return fileCard;
    }
}