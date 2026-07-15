using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SilkyUIFramework;
using SilkyUIFramework.Elements;
using System;
using System.Diagnostics;
using Terraria;

namespace ImproveGameStructureDLC.UI;

public partial class StructurePreviewPage : UIElementGroup
{
    public StructurePreviewPage()
    {
        InitializeComponent();
        ImportingButton.Text = StructureDLCController.GetLocalizedTextValue("Import");
    }


    public StructureInfo InfoData { get; private set; }

    public void SetInfoData(StructureInfo infoData)
    {
        Debug.Assert(infoData != null);
        if (infoData == null) return;
        InfoData = infoData;
        NameText.Text = $"{StructureDLCController.GetLocalizedTextValue("Name")}: {InfoData.Name}";
        AuthorText.Text = $"{StructureDLCController.GetLocalizedTextValue("AuthorOrUploader")}: {InfoData.Author}";
        DateText.Text = $"{StructureDLCController.GetLocalizedTextValue("Date")}: {InfoData.Date}";
        TagText.Text = $"{StructureDLCController.GetLocalizedTextValue("Tag")}: {string.Join(", ", InfoData.Tags)}";
        DescriptionText.Text = $"{StructureDLCController.GetLocalizedTextValue("Description")}: {InfoData.Description}";
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);
        if (ImportingButton.HoverTimer.IsUpdating)
        {
            var button = ImportingButton;
            var timer = button.HoverTimer;
            button.BackgroundColor = timer.Lerp(Color.LimeGreen * .5f, Color.LightGreen * .5f);
        }
    }

    public override void HandleDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.HandleDraw(gameTime, spriteBatch);
        if (StructureDLCController.Instance?.CurrentPreview is { } preview)
        {
            var bound = PreviewPanel.Bounds;
            float scale = Math.Min((bound.Width - 64f) / preview.Width, (bound.Height - 64f) / preview.Height);
            spriteBatch.Draw(preview, bound.Center, null, Color.White, 0, preview.Size() * .5f, scale, 0, 0);
        }
    }
}
