using ImproveGameStructureDLC.UI;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ImproveGameStructureDLC;

public class ImproveGameStructureDLC : Mod
{
    public static ImproveGameStructureDLC Instance { get; private set; }
    public ModKeybind StructureDLCToggleKeyBind { get; private set; }
    public static List<StructureInfo> CreateInfoList { get; } = [];
    public static List<StructureInfo> ConstructInfoList { get; } = [];
    public override void Load()
    {
        Instance = this;
        StructureDLCToggleKeyBind = KeybindLoader.RegisterKeybind(this, nameof(StructureDLCToggleKeyBind), Keys.OemQuotes);
        foreach (var name in GetFileNames())
        {
            if (name.StartsWith("Structures/ConstructWand") && name.EndsWith(".json"))
            {
                using MemoryStream ms = new(GetFileBytes(name));
                using StreamReader reader = new(ms);
                var bytes = GetFileBytes(name);
                var info = new StructureInfo();
                JsonConvert.PopulateObject(reader.ReadToEnd(), info, ConfigManager.serializerSettings);
                info.Name = Path.GetFileNameWithoutExtension(name);
                info.Path = Path.GetDirectoryName(name);
                ConstructInfoList.Add(info);
            }
            if (name.StartsWith("Structures/CreateWand") && name.EndsWith(".json"))
            {
                using MemoryStream ms = new(GetFileBytes(name));
                using StreamReader reader = new(ms);
                var bytes = GetFileBytes(name);
                var meta = new StructureInfo();
                JsonConvert.PopulateObject(reader.ReadToEnd(), meta, ConfigManager.serializerSettings);
                meta.Name = Path.GetFileNameWithoutExtension(name);
                meta.Path = Path.GetDirectoryName(name);
                CreateInfoList.Add(meta);
            }
        }
    }
}