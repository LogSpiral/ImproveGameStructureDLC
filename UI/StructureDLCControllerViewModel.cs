using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImproveGame.Content.Items;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ImproveGameStructureDLC.UI;

public partial class StructureDLCControllerViewModel : ObservableObject
{
    public List<StructureInfo> CreateInfoList { get; } = ImproveGameStructureDLC.CreateInfoList;
    public List<StructureInfo> ConstructInfoList { get; } = ImproveGameStructureDLC.ConstructInfoList;

    [RelayCommand]
    public static void OpenFolder()
    {
        Utils.OpenFolder(Path.Combine(Main.SavePath, "Mods", "ImproveGame"));
    }

    public static void ImportCreateWand(StructureInfo info)
    {
        SoundEngine.PlaySound(SoundID.ResearchComplete);
        var bytes = ImproveGameStructureDLC.Instance.GetFileBytes(Path.Combine(info.Path, info.Name + ".png"));
        using MemoryStream ms = new(bytes);
        var tex2D = Texture2D.FromStream(Main.graphics.GraphicsDevice, ms);
        File.WriteAllBytes(Path.Combine(Main.SavePath, "Mods", "ImproveGame", "CreateWand", info.Name + ".png"), bytes);
        CreateWand.AddNewPrisonStyle(tex2D, null);
    }

    public static void ImportConstructWand(StructureInfo info)
    {
        SoundEngine.PlaySound(SoundID.ResearchComplete);
        var bytes = ImproveGameStructureDLC.Instance.GetFileBytes(Path.Combine(info.Path, info.Name + ".qotstruct"));
        File.WriteAllBytes(Path.Combine(Main.SavePath, "Mods", "ImproveGame", info.Name + ".qotstruct"), bytes);
    }
}
