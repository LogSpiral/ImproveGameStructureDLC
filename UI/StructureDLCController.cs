using ImproveGame.Content.Functions.Construction;
using ImproveGame.Content.Items;
using ImproveGame.UserInterfaces.CreateWand;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SilkyUIFramework;
using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Elements;
using SilkyUIFramework.Graphics2D;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ImproveGameStructureDLC.UI;

[RegisterUI]
public partial class StructureDLCController : BaseBody
{
#if DEBUG
    public static string LocalizationPrefix { get; } = "Mods.ImproveGameStructureDLC.StructureDLCController";
#else
    public const string LocalizationPrefix = "Mods.ImproveGameStructureDLC.StructureDLCController";
#endif

    public static string GetLocalizedTextValue(string suffix) => Language.GetTextValue($"{LocalizationPrefix}.{suffix}");
    public bool Active { get; set; }
    public AnimationTimer SwitchTimer { get; init; } = new(3);

    public override bool Enabled
    {
        get => Active || !SwitchTimer.IsReverseCompleted;
        set => Active = value;
    }

    public static StructureDLCController Instance { get; private set; }
    public override IEnumerable<UIView> BlurElements => [MainContainer];
    private SUIBuildMaterialItemSlot[] ItemSlots_Interanl { get; } = new SUIBuildMaterialItemSlot[24];
    public IReadOnlyList<SUIBuildMaterialItemSlot> ItemSlots => ItemSlots_Interanl;

    protected override void OnInitialize()
    {
        InitializeFull();
    }


    public void ToggleOpen()
    {
        Active = !Active;
        if (Active)
        {
            //_contentLoaded = false;
            //RemoveAllChildren();
            //InitializeFull();
            //Active = true;
            SoundEngine.PlaySound(SoundID.MenuOpen);
        }
        else
        {
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
    }
    private UIView CurrentActivePage { get; set; }
    public CreateWandPreviewPage CreateWandPreview { get; set; }
    public ConstructWandPreviewPage ConstructWandPreview { get; set; }
    public Vector2 PreviousSize { get; set; }
    public Vector2 CurrentSize { get; set; }
    public AnimationTimer SizeTimer = new();
    public Texture2D CurrentPreview { get; private set; }
    private string TempPath { get; set; }
    private bool IsWaitingPreview { get; set; }
    private void InitializeFull()
    {
        Instance = this;

        InitializeComponent();

        MainContainer.BorderColor = SUIColor.Border;
        MainContainer.BackgroundColor = SUIColor.Background * 0.75f;

        Header.ControlTarget = this;
        Title.UseDeathText();
        Title.Text = GetLocalizedTextValue("Title");
        CreateWandButton.Texture2D = TextureAssets.Item[ModContent.ItemType<CreateWand>()];
        CreateWandButton.MouseEnter += (sender, evt) => { SoundEngine.PlaySound(SoundID.MenuTick); };
        ConstructWandButton.Texture2D = TextureAssets.Item[ModContent.ItemType<ConstructWand>()];
        ConstructWandButton.MouseEnter += (sender, evt) => { SoundEngine.PlaySound(SoundID.MenuTick); };
        CreateWandList.ViewTemplate = CreateWandCardTemplate.Instance;
        ConstructWandList.ViewTemplate = ConstructWandCardTemplate.Instance;
        CurrentActivePage = MenuPage;
        CreateWandButton.LeftMouseClick += SwitchToCreateWandList;
        ConstructWandButton.LeftMouseClick += SwitchToConstructWandList;
        CreateWandPreview = new CreateWandPreviewPage()
        {
            Width = new(0, 1),
            Height = new(0, 1)
        };
        ConstructWandPreview = new ConstructWandPreviewPage()
        {
            Width = new(0, 1),
            Height = new(0, 1)
        };
        CreateWandPreviewContainer.AddChild(CreateWandPreview);
        ConstructWandPreviewContainer.AddChild(ConstructWandPreview);
        PreviousSize = new(400, 220);
        CurrentSize = new(400, 220);
        CloseButton.LeftMouseClick += CloseUI;
        CloseButton.CrossBorderColor = SUIColor.Border * .5f;
        CloseButton.CrossBackgroundColor = SUIColor.Warn * .5f;
        OpenFolderButton.Texture2D = ImproveGame.ModAsset.Folder;
        BackButton.Texture2D = ModAsset.Back;
        BackButton.LeftMouseClick += BackButtonSwitches;
    }

    private void CloseUI(UIView sender, UIMouseEvent evt)
    {
        ToggleOpen();
    }



    protected override void UpdateStatus(GameTime gameTime)
    {
        SizeTimer.Update(gameTime);
        if (SizeTimer.IsUpdating)
        {
            Vector2 size = SizeTimer.Lerp(PreviousSize, CurrentSize);
            PageContainer.SetSize(size.X, size.Y);
        }

        if (Active) SwitchTimer.StartUpdate();
        else SwitchTimer.StartReverseUpdate();

        SwitchTimer.Update(gameTime);

        UseRenderTarget = SwitchTimer.IsUpdating;
        Opacity = SwitchTimer.Lerp(0f, 1f);

        var center = Bounds.Center * Main.UIScale;
        RenderTargetMatrix =
            Matrix.CreateTranslation(-center.X, -center.Y, 0) *
            Matrix.CreateScale(SwitchTimer.Lerp(0.95f, 1f), SwitchTimer.Lerp(0.95f, 1f), 1) *
            Matrix.CreateTranslation(center.X, center.Y, 0);


        base.UpdateStatus(gameTime);
        if (CloseButton.HoverTimer.IsUpdating)
        {
            var timer = CloseButton.HoverTimer;
            CloseButton.CrossBackgroundColor = SUIColor.Warn * timer.Lerp(0.5f, 1f);
            CloseButton.CrossBorderColor = timer.Lerp(SUIColor.Border * .5f, SUIColor.Highlight);
            CloseButton.CrossBorderRadius = timer.Lerp(3f, 2f);
            CloseButton.BackgroundColor = Color.Black * timer.Lerp(0f, 0.25f);
        }
        if (OpenFolderButton.HoverTimer.IsUpdating)
        {
            var timer = OpenFolderButton.HoverTimer;
            OpenFolderButton.ImageColor = Color.White * timer.Lerp(0.5f, 1.0f);
            OpenFolderButton.BackgroundColor = Color.Black * timer.Lerp(0f, 0.25f);
        }
        if (BackButton.HoverTimer.IsUpdating)
        {
            var timer = BackButton.HoverTimer;
            BackButton.ImageColor = Color.White * timer.Lerp(0.5f, 1.0f);
            BackButton.BackgroundColor = Color.Black * timer.Lerp(0f, 0.25f);
        }
        if (CreateWandButton.HoverTimer.IsUpdating)
        {
            var button = CreateWandButton;
            var timer = button.HoverTimer;
            button.BackgroundColor = Color.Black * timer.Lerp(.25f, .1f);
            button.BorderColor = timer.Lerp(Color.Black, SUIColor.Highlight * .5f);
            button.Border = timer.Lerp(1, 2);
            button.ImageColor = Color.White * timer.Lerp(0.75f, 1.0f);
            if (timer.IsForward)
            {
                var t = timer.Schedule;
                button.ImageScale =
                    new Vector2(
                        1.2f - MathF.Exp(-0.5f * t) * 0.2f * MathF.Cos(2 * MathF.PI * t),
                        1.3f - MathF.Exp(-0.5f * t) * 0.3f * MathF.Cos(4 * MathF.PI * t)
                        );
            }
            else
                button.ImageScale = timer.Lerp(Vector2.One, button.ImageScale);
        }
        if (ConstructWandButton.HoverTimer.IsUpdating)
        {
            var button = ConstructWandButton;
            var timer = button.HoverTimer;
            button.BackgroundColor = Color.Black * timer.Lerp(.25f, .1f);
            button.BorderColor = timer.Lerp(Color.Black, SUIColor.Highlight * .5f);
            button.Border = timer.Lerp(1, 2);
            button.ImageColor = Color.White * timer.Lerp(0.75f, 1.0f);
            if (timer.IsForward)
            {
                var t = timer.Schedule;
                button.ImageScale =
                    new Vector2(
                        1.2f - MathF.Exp(-0.5f * t) * 0.2f * MathF.Cos(2 * MathF.PI * t),
                        1.3f - MathF.Exp(-0.5f * t) * 0.3f * MathF.Cos(4 * MathF.PI * t)
                        );
            }
            else
                button.ImageScale = timer.Lerp(Vector2.One, button.ImageScale);
        }


        if (IsWaitingPreview && PreviewRenderer.ResetPreviewTarget == PreviewRenderer.ResetState.Finished && File.Exists(TempPath))
        {
            FileOperator.CachedStructureDatas.Remove(TempPath);
            IsWaitingPreview = false;
            File.Delete(TempPath);
            var pv = PreviewRenderer.PreviewTarget;
            CurrentPreview?.Dispose();
            CurrentPreview = new Texture2D(Main.graphics.GraphicsDevice, pv.Width, pv.Height);
            Color[] colors = new Color[pv.Width * pv.Height];
            pv.GetData(colors);
            CurrentPreview.SetData(colors);
            pv?.Dispose();
        }
    }
    protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (BlurMakeSystem.BlurAvailable)
        {
            if (BlurMakeSystem.SingleBlur)
            {
                var batch = Main.spriteBatch;
                batch.End();
                BlurMakeSystem.KawaseBlur();
                batch.Begin();
            }

            SDFRectangle.SampleVersion(BlurMakeSystem.BlurRenderTarget,
                Bounds.Position * Main.UIScale, Bounds.Size * Main.UIScale, BorderRadius * Main.UIScale, Matrix.Identity);
        }

        base.Draw(gameTime, spriteBatch);
    }
    protected override void OnEnterTree()
    {
        LocalDataContext = new StructureDLCControllerViewModel();
    }
}
