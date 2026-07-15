using ImproveGame.UserInterfaces.CreateWand;
using Microsoft.Xna.Framework;
using SilkyUIFramework.Elements;
namespace ImproveGameStructureDLC.UI;

public partial class StructureEntryCard : UIElementGroup
{
    public StructureInfo InfoData { get; set; }
    public StructureEntryCard()
    {
        InitializeComponent();
        ImportingButton.Texture2D = ModAsset.Import;
        PreviewButton.Texture2D = ModAsset.Preview;
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);
        if (HoverTimer is { IsUpdating: true } timer)
        {
            BackgroundColor = Color.Black * timer.Lerp(0.25f, 0.1f);
            InnerText.TextBorderColor = timer.Lerp(Color.Black, Color.Yellow * .25f);
        }
        if (ImportingButton.HoverTimer.IsUpdating)
        {
            ImportingButton.BackgroundColor = ImportingButton.HoverTimer.Lerp(Color.Gray, Color.White) * 0.25f;
        }
        if (PreviewButton.HoverTimer.IsUpdating)
        {
            PreviewButton.BackgroundColor = PreviewButton.HoverTimer.Lerp(Color.Gray, Color.White) * 0.25f;
        }
    }
}
public class StructureEntryCardTemplate : ISourcedUIViewTemplate
{
    public static StructureEntryCardTemplate Instance { get; } = new();
    UIView ISourcedUIViewTemplate.ConstructFromSource(object sourceData)
    {
        if (sourceData is not StructureInfo info)
            return null;
        StructureEntryCard fileCard = new()
        {
            InfoData = info
        };
        fileCard.InnerText.Text = info.Name.ToString();
        return fileCard;
    }
}
