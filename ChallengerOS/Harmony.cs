using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using System;
using System.Linq;
using System.Net;
using Reactor;
using static ChallengerMod.Challenger;
using UnityEngine;
using System.IO;
using Reactor.Extensions;
using Hazel.Udp;
using BepInEx.Logging;
using ChallengerOS.Utils.Option;
using static ChallengerMod.Roles;
using ChallengerOS.RainbowPlugin;
using UnhollowerRuntimeLib;


namespace ChallengerOS
{
    [BepInPlugin(Id, "ChallengerOS", VersionString)]
    [BepInDependency(ReactorPlugin.Id)]
    [BepInProcess("Among Us.exe")]

    public class HarmonyMain : BasePlugin
    {
        
        public const string VersionString = "5.1.3";
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
