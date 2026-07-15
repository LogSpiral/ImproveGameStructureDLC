using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SilkyUIFramework;

namespace ImproveGameStructureDLC.UI;

public class ConstructWandPreviewPage : StructurePreviewPage
{
    public ConstructWandPreviewPage() : base()
    {
        ImportingButton.LeftMouseClick += (sender, evt) =>
        {
            StructureDLCControllerViewModel.ImportConstructWand(InfoData);
        };
    }
    //protected override void DrawPreview(GameTime gameTime, SpriteBatch spriteBatch, Bounds bound)
    //{
    //    base.DrawPreview(gameTime, spriteBatch, bound);
    //}
}
