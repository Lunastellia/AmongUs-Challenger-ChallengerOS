
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System;
using ChallengerOS.Utils;
using ChallengerMod.RPC;
using static ChallengerMod.Set.Data;
using System.Linq;
using InnerNet;

namespace ChallengerOS.Versioncheck
{
    public class GameStartManagerPatch
    {
        public static Dictionary<int, PlayerVersion> playerVersions = new Dictionary<int, PlayerVersion>();
        private static bool versionSent = false;

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
        public class AmongUsClientOnPlayerJoinedPatch
        {
            public static void Postfix()
            {
                if (PlayerControl.LocalPlayer != null)
                {
                    Helpers.shareGameVersion();

                    if (ChallengerMod.Challenger.IsrankedGame)
                    {
                        ChallengerMod.Challenger.ReadyPlayers = new List<string>();
                    }
         
                }
            }
        }

        

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerLeft))]
        public class AmongUsClientOnPlayerLeftPatch
        {
            public static void Postfix()
            {
                if (PlayerControl.LocalPlayer != null && ChallengerMod.Challenger.IsrankedGame)
                {
                    ChallengerMod.Challenger.ReadyPlayers = new List<string>();
                }
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        public class GameStartManagerStartPatch
        {
            public static void Postfix(GameStartManager __instance)
            {
                // Trigger version refresh
                versionSent = false;

                
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
        public class GameStartManagerUpdatePatch
        {
            private static bool update = false;

            public static void Prefix(GameStartManager __instance)
            {
                if (!GameData.Instance) return; // No instance
                update = GameData.Instance.PlayerCount != __instance.LastPlayerCount;
            }

            public static void Postfix(GameStartManager __instance)
            {
                // Send version as soon as CachedPlayer.LocalPlayer.PlayerControl exists
                if (PlayerControl.LocalPlayer != null && !versionSent)
                {
                    versionSent = true;
                    Helpers.shareGameVersion();
                }
                if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started)
                {
                    // Check version handshake infos

                    

                    bool versionMismatch = false;
                    bool notplayable = false;
                    string message = "";

                    if (ChallengerMod.HarmonyMain.CanStartTheGame == false)
                    {
                        notplayable = true;
                    }

                    foreach (InnerNet.ClientData client in AmongUsClient.Instance.allClients.ToArray())
                    {
                        if (__instance.StartButton.isVisible && client.Character.Data.PlayerId == 0)
                        {
                            if (__instance.LastPlayerCount > 3 && ChallengerMod.Challenger.ReadyPlayers.Count() != __instance.LastPlayerCount - 0 && ChallengerMod.Challenger.IsrankedGame)
                            {
                                notplayable = true;
                                message += $"<size=2.5><color=#FF0000> • </color><color=#F25944>All players has not Ready !</color></size>\n";
                            }

                            if ((__instance.LastPlayerCount <= 3) && !ChallengerMod.Challenger.IsrankedGame)
                            {
                                message += $"<size=2.5><color=#FF0000> • </color><color=#F25944>No enough players to start the game !</color></size>\n";
                                notplayable = true;
                            }
                            if ((__instance.LastPlayerCount <= 9) && ChallengerMod.Challenger.IsrankedGame)
                            {
                                message += $"<size=2.5><color=#FF0000> • </color><color=#F25944>No enough players to start the game !</color></size>\n";
                                notplayable = true;
                            }
                            if ((0 + ChallengerOS.Utils.Option.CustomOptionHolder.QTImp.getFloat() + ChallengerOS.Utils.Option.CustomOptionHolder.QTSpe.getFloat() + ChallengerOS.Utils.Option.CustomOptionHolder.QTDuo.getFloat() + ChallengerOS.Utils.Option.CustomOptionHolder.QTCrew.getFloat()) > __instance.LastPlayerCount && (__instance.LastPlayerCount > 3))
                            {
                                message += $"<size=2.5><color=#FF0000> • </color><color=#F25944>The number of roles exceeds the number of players !</color></size>\n";
                                notplayable = true;
                            }
                            if (ChallengerOS.Utils.Option.CustomOptionHolder.QTImp.getFloat() > ChallengerMod.Set.Data.RealImpostor && (__instance.LastPlayerCount > 3))
                            {
                                message += $"<size=2.5><color=#FF0000> • </color><color=#F25944>The amount of Impostor roles exceeds the number of slots available for that category !</color></size>\n";
                                notplayable = true;
                            }
                            if ((0 + ChallengerOS.Utils.Option.CustomOptionHolder.QTCrew.getFloat() + ChallengerOS.Utils.Option.CustomOptionHolder.QTDuo.getFloat() + ChallengerOS.Utils.Option.CustomOptionHolder.QTSpe.getFloat()) > (__instance.LastPlayerCount - ChallengerMod.Set.Data.RealImpostor) && (__instance.LastPlayerCount > 3))
                            {
                                message += $"<size=2.5><color=#FF0000> • </color><color=#F25944>The amount of Crewmate / Special / Hybrid roles exceeds the number of slots available for that category !</color></size>\n";
                                notplayable = true;
                            }
                        }
                        if (client.Character == null) continue;
                        var dummyComponent = client.Character.GetComponent<DummyBehaviour>();
                        if (dummyComponent != null && dummyComponent.enabled)
                            continue;


                        else if (!playerVersions.ContainsKey(client.Id))
                        {
                            versionMismatch = true;
                            if (__instance.StartButton.isVisible)
                            {
                                message += $"<size=3><color=#96E7DF>{client.Character.Data.PlayerName}</color><color=#96E7DF> - </color><color=#FF9E43>Challenger</color><color=#FFA69E> " + Client_VerMiss + "</color></size>\n";
                            }
                        }
                        else
                        {
                            PlayerVersion PV = playerVersions[client.Id];
                            int diff = ChallengerMod.HarmonyMain.Version.CompareTo(PV.version);
                            if (diff > 0)
                            {
                                if (__instance.StartButton.isVisible)
                                {

                                    message += $"<size=3><color=#96E7DF>{client.Character.Data.PlayerName}</color><color=#96E7DF> - </color><color=#FFA69E> " + Client_VerDiff + " : <color=#FF9E43>Challenger</color> <color=#688AFE>(" + playerVersions[client.Id].version.ToString() + ")\n</color></size>";
                                    versionMismatch = true;
                                }
                            }
                            else if (diff < 0)
                            {
                                if (__instance.StartButton.isVisible)
                                {

                                    message += $"<size=3><color=#96E7DF>{client.Character.Data.PlayerName}</color><color=#96E7DF> - </color><color=#FFA69E> " + Client_VerDiff + " : <color=#FF9E43>Challenger</color> <color=#688AFE>(" + playerVersions[client.Id].version.ToString() + ")\n</color></size>";
                                    versionMismatch = true;
                                }
                            }
                            else if (!PV.GuidMatches())
                            { // version presumably matches, check if Guid matches
                                if (__instance.StartButton.isVisible)
                                {

                                    message += $"<size=3><color=#96E7DF>{client.Character.Data.PlayerName}</color><color=#96E7DF> - </color><color=#FFA69E> " + Client_VerDiff + " : <color=#FF9E43>Challenger</color> <color=#688AFE>(" + playerVersions[client.Id].version.ToString() + ")\n</color></size>";
                                    versionMismatch = true;
                                }
                            }
                        }
                    }

                    // Display message to the host
                    if (AmongUsClient.Instance.AmHost)
                    {
                        if (versionMismatch || notplayable)
                        {
                            __instance.StartButton.color = __instance.startLabelText.color = Palette.DisabledClear;
                            __instance.GameStartText.text = message;
                            __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + Vector3.up * 2;
                        }
                        else
                        {
                            __instance.StartButton.color = __instance.startLabelText.color = ((__instance.LastPlayerCount >= __instance.MinPlayers) ? Palette.EnabledColor : Palette.DisabledClear);
                            __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition;
                        }

                    }

                    else
                    {
                        if (!playerVersions.ContainsKey(AmongUsClient.Instance.HostId) || ChallengerMod.HarmonyMain.Version.CompareTo(playerVersions[AmongUsClient.Instance.HostId].version) != 0)
                        {
                            __instance.GameStartText.text = $"";
                            __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + Vector3.up * 2;
                        }
                        else if (versionMismatch)
                        {
                            __instance.GameStartText.text = $"" + message;
                            __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + Vector3.up * 2;
                        }
                        else
                        {
                            __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition;
                            if (__instance.startState != GameStartManager.StartingStates.Countdown)
                            {
                                __instance.GameStartText.text = String.Empty;
                            }
                        }
                    }
                }

            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
        public class GameStartManagerBeginGame
        {
            public static bool Prefix(GameStartManager __instance)
            {
                // Block game start if not everyone has the same mod version
                bool continueStart = true;

                if (AmongUsClient.Instance.AmHost)
                {
                    foreach (InnerNet.ClientData client in AmongUsClient.Instance.allClients)
                    {
                        if (client.Character == null) continue;
                        var dummyComponent = client.Character.GetComponent<DummyBehaviour>();
                        if (dummyComponent != null && dummyComponent.enabled)
                            continue;

                        if (!playerVersions.ContainsKey(client.Id))
                        {
                            continueStart = false;
                            break;
                        }

                        PlayerVersion PV = playerVersions[client.Id];
                        int diff = ChallengerMod.HarmonyMain.Version.CompareTo(PV.version);
                        if (diff != 0 || !PV.GuidMatches())
                        {
                            continueStart = false;
                            break;
                        }
                    }


                }
                return continueStart;
            }
        }
    

        public class PlayerVersion
        {
            public readonly Version version;
            public readonly Guid guid;

            public PlayerVersion(Version version, Guid guid)
            {
                this.version = version;
                this.guid = guid;
            }

            public bool GuidMatches()
            {
                return Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId.Equals(this.guid);
            }
        }
    }
}