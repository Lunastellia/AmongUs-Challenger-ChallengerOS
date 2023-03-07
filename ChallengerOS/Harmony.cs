using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using ChallengerOS.Utils.Option;
using ChallengerOS.RainbowPlugin;
using UnhollowerRuntimeLib;


namespace ChallengerOS
{
    [BepInPlugin(Id, "ChallengerOS", VersionString)]
    [BepInDependency(ReactorPlugin.Id)]
    [BepInProcess("Among Us.exe")]

    public class HarmonyMain : BasePlugin
    {
        
        public const string VersionString = "5.2.1";
        public static ConfigFile OPSettings { get; private set; }

        public static System.Version Version = System.Version.Parse(VersionString);
        
        public const string Id = "Config.ChallengerMod";
        public static HarmonyMain Instance { get { return PluginSingleton<HarmonyMain>.Instance; } }
        
        public Harmony Harmony { get; } = new Harmony(Id);

        public static bool Logging
        {
            get
            {
                if (OPSettings == null)
                    return true;
                return OPSettings.Bind("Settings", "Logging", true).Value;
            }
        }

        public override void Load()
        {


            CustomOptionHolder.Load();
            PalettePatch.Load();
            ClassInjector.RegisterTypeInIl2Cpp<RainbowBehaviour>();
            Harmony.PatchAll();
        }




        
    }
    
}
