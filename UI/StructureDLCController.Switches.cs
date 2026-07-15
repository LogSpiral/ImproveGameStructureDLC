using ImproveGame.Common.ModSystems;
using ImproveGame.Content.Functions.Construction;
using ImproveGame.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SilkyUIFramework;
using SilkyUIFramework.Elements;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static log4net.Appender.ColoredConsoleAppender;

namespace ImproveGameStructureDLC.UI;

public partial class StructureDLCController
{
    private void SwitchActivePage(UIView nextPage)
    {
        CurrentActivePage.Invalid = true;
        CurrentActivePage = nextPage;
        CurrentActivePage.Invalid = false;
    }

    private void SwitchSize(Vector2 nextSize)
    {
        Vector2 preciseSize = SizeTimer.Lerp(PreviousSize, CurrentSize);
        PreviousSize = preciseSize;
        CurrentSize = nextSize;
        SizeTimer.StartUpdate(true);
    }

    private void SwitchToMenuPage()
    {
        BackButton.Invalid = true;
        SoundEngine.PlaySound(SoundID.MenuClose);
        SwitchActivePage(MenuPage);
        SwitchSize(new Vector2(400, 220));
    }

    private void SwitchToCreateWandList(UIView sender, UIMouseEvent evt)
    {
        BackButton.Invalid = false;
        SoundEngine.PlaySound(SoundID.MenuOpen);
        SwitchActivePage(CreateWandList);
        SwitchSize(new Vector2(400, 220));
    }

    private void SwitchToConstructWandList(UIView sender, UIMouseEvent evt)
    {
        BackButton.Invalid = false;
        SoundEngine.PlaySound(SoundID.MenuOpen);
        SwitchActivePage(ConstructWandList);
        SwitchSize(new Vector2(400, 220));
    }

    public void SwitchToCreatePreview(StructureInfo info)
    {
        BackButton.Invalid = false;
        SoundEngine.PlaySound(SoundID.MenuOpen);
        SwitchActivePage(CreateWandPreviewContainer);
        CreateWandPreview.SetInfoData(info);
        SwitchSize(new Vector2(720, 540));

        var bytes = ImproveGameStructureDLC.Instance.GetFileBytes(Path.Combine(info.Path, info.Name + ".png"));
        using MemoryStream ms = new(bytes);
        var tex2D = Texture2D.FromStream(Main.graphics.GraphicsDevice, ms);
        int width = tex2D.Width;
        int height = tex2D.Height;
        Color[] colors = new Color[tex2D.Width * tex2D.Height];
        tex2D.GetData(colors);
        var tag = CreateWand.CreateStructureTagFromColors(colors, width, height);
        var tagPath = Path.Combine(ModLoader.ModPath, nameof(ImproveGame), "tempStructure.qotstruct");
        TempPath = tagPath;
        IsWaitingPreview = true;
        TagIO.ToFile(tag, tagPath);
        WandSystem.ConstructFilePath = tagPath;
        PreviewRenderer.ResetPreviewTarget = PreviewRenderer.ResetState.WaitReset;

        PreviewRenderer.PreviewTarget =
            new RenderTarget2D(
                Main.graphics.GraphicsDevice,
                width * 16 + 4,
                height * 16 + 4,
                false,
                default,
                default,
                default,
                RenderTargetUsage.PreserveContents);
    }

    public void SwitchToConstructPreview(StructureInfo info)
    {
        BackButton.Invalid = false;
        SoundEngine.PlaySound(SoundID.MenuOpen);
        SwitchActivePage(ConstructWandPreviewContainer);
        ConstructWandPreview.SetInfoData(info);
        SwitchSize(new Vector2(720, 540));

        var tagPath = Path.Combine(ModLoader.ModPath, nameof(ImproveGame), "tempStructure.qotstruct");
        var bytes = ImproveGameStructureDLC.Instance.GetFileBytes(Path.Combine(info.Path, info.Name + ".qotstruct"));
        File.WriteAllBytes(tagPath, bytes);
        var tag = FileOperator.GetTagFromFile(tagPath);
        int width = tag.GetShort("Width") + 1;
        int height = tag.GetShort("Height") + 1;
        TempPath = tagPath;
        IsWaitingPreview = true;
        WandSystem.ConstructFilePath = tagPath;
        PreviewRenderer.ResetPreviewTarget = PreviewRenderer.ResetState.WaitReset;
        PreviewRenderer.PreviewTarget =
            new RenderTarget2D(
                Main.graphics.GraphicsDevice,
                width * 16 + 4,
                height * 16 + 4,
                false,
                default,
                default,
                default,
                RenderTargetUsage.PreserveContents);
    }

    private void BackButtonSwitches(UIView sender, UIMouseEvent evt)
    {
        var active = CurrentActivePage;
        if (active == ConstructWandList || active == CreateWandList)
            SwitchToMenuPage();
        else if (active == ConstructWandPreviewContainer)
            SwitchToConstructWandList(sender, evt);
        else if (active == CreateWandPreviewContainer)
            SwitchToCreateWandList(sender, evt);
    }
}
