using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SilkyUIFramework;

namespace ImproveGameStructureDLC.UI;

public class CreateWandPreviewPage:StructurePreviewPage
{
    public CreateWandPreviewPage() : base()
    {
        ImportingButton.LeftMouseClick += (sender, evt) =>
        {
            StructureDLCControllerViewModel.ImportCreateWand(InfoData);
        };
    }
    //protected override void DrawPreview(GameTime gameTime, SpriteBatch spriteBatch, Bounds bound)
    //{
    //    base.DrawPreview(gameTime, spriteBatch, bound);
    //}
}
