using HarmonyLib;
using static ChallengerMod.Roles;
using UnityEngine;
using ChallengerMod;
using static Il2CppSystem.Net.TimerThread;
using static ChallengerMod.Set.Data;
using static ChallengerMod.Challenger;
using ChallengerOS.Utils;
using Hazel;
using System;
using System.Collections.Generic;
using static ChallengerMod.HarmonyMain;
using System.Linq;
using static ChallengerOS.Utils.Option.CustomOptionHolder;
using static ChallengerOS.Utils.Helpers;
using ChallengerOS.RPC;
using static ChallengerMod.Unity;
using DeadPlayer = ChallengerOS.Utils.Helpers.DeadPlayer;

namespace ChallengerOS
{

    
    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.CoPerformKill))]
    class KillAnimationCoPerformKillPatch
    {
        public static bool hideNextAnimation = true;
        public static void Prefix(KillAnimation __instance, [HarmonyArgument(0)] ref PlayerControl source, [HarmonyArgument(1)] ref PlayerControl target)
        {
            if (hideNextAnimation)
                source = target;
            hideNextAnimation = false;
        }
    }

    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.SetMovement))]
    class KillAnimationSetMovementPatch
    {
        private static int? colorId = null;
        public static void Prefix(PlayerControl source, bool canMove)
        {
            Color color = source.cosmetics.currentBodySprite.BodySprite.material.GetColor("_BodyColor");
            if (color != null && Morphling.Role != null && source.Data.PlayerId == Morphling.Role.PlayerId)
            {
                var index = Palette.PlayerColors.IndexOf(color);
                if (index != -1) colorId = index;
            }
        }

        public static void Postfix(PlayerControl source, bool canMove)
        {
            if (colorId.HasValue) source.RawSetColor(colorId.Value);
            colorId = null;
        }
    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class MurderPlayerPatch
    {
        public static bool resetToCrewmate = false;
        public static bool resetToDead = false;

        public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            // Allow everyone to murder players
            resetToCrewmate = !__instance.Data.Role.IsImpostor;
            resetToDead = __instance.Data.IsDead;
            __instance.Data.Role.TeamType = RoleTeamTypes.Impostor;
            __instance.Data.IsDead = false;

        }

        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {


            DeadPlayer deadPlayer = new DeadPlayer(target, DateTime.UtcNow, DeathReason.Kill, __instance);
            ChallengerOS.Utils.Helpers.GameHistory.deadPlayers.Add(deadPlayer);

            if (resetToCrewmate)
            {

                if (Mercenary.Role != null && __instance.PlayerId == Mercenary.Role.PlayerId)
                {
                    __instance.Data.Role.TeamType = RoleTeamTypes.Impostor;
                }
                else if (CopyCat.Role != null && __instance.PlayerId == CopyCat.Role.PlayerId && (CopyCat.copyRole == 25) && CopyCat.isImpostor)
                {
                    __instance.Data.Role.TeamType = RoleTeamTypes.Impostor;
                }
                else if (CopyCat.Role != null && __instance.PlayerId == CopyCat.Role.PlayerId && !CopyCat.isImpostor)
                {
                    __instance.Data.Role.TeamType = RoleTeamTypes.Crewmate;
                }
                else
                {
                    __instance.Data.Role.TeamType = RoleTeamTypes.Crewmate;

                }

            }

            if (resetToDead)
            {
                __instance.Data.IsDead = true;
            }


            // Remove fake tasks when player dies
            if (target.hasFakeTasks())
            {
                target.clearAllTasks();
            }
            // Bait
            if (Bait.bait.FindAll(x => x.PlayerId == target.PlayerId).Count > 0)
            {
                float reportDelay = (float)rnd.Next((int)Bait.reportDelayMin, (int)Bait.reportDelayMax);
                Bait.active.Add(deadPlayer, reportDelay);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
    public static class ExiledPlayerPatch
    {
        public static void Postfix(PlayerControl __instance)
        {

            if (__instance.hasFakeTasks())
                __instance.clearAllTasks();
        }
    }

   
    
    
    

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.LocalPlayer.CmdReportDeadBody))]
    class BodyReportPatch
    {

        static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] GameData.PlayerInfo target)
        {
            bool isReport = (Lawkeeper.Role != null && Lawkeeper.Role == PlayerControl.LocalPlayer && __instance.PlayerId == Lawkeeper.Role.PlayerId);
            bool isCopyReport = (CopyCat.Role != null && CopyCat.copyRole == 18 && CopyCat.CopyStart && CopyCat.Role == PlayerControl.LocalPlayer && __instance.PlayerId == CopyCat.Role.PlayerId);
            bool Ability = (Lawkeeper.Role != null && Lawkeeper.Role != PlayerControl.LocalPlayer && __instance.PlayerId != Lawkeeper.Role.PlayerId && Lawkeeper.AbilityEnable && !Lawkeeper.Role.Data.IsDead);
            bool AbilityCopy = (CopyCat.Role != null && CopyCat.copyRole == 18 && CopyCat.CopyStart && CopyCat.Role != PlayerControl.LocalPlayer && __instance.PlayerId != CopyCat.Role.PlayerId && CopyCat.AbilityEnable);


            if (isReport || isCopyReport || Ability)
            {
                DeadPlayer deadPlayer = ChallengerOS.Utils.Helpers.GameHistory.deadPlayers?.Where(x => x.player?.PlayerId == target?.PlayerId)?.FirstOrDefault();

                if (deadPlayer != null && deadPlayer.killerIfExisting != null)
                {
                    float timeSinceDeath = ((float)(DateTime.UtcNow - deadPlayer.timeOfDeath).TotalMilliseconds);
                    string msg = "";
                    string TypeofDeath = TXT_Deathreason0;

                    if (isReport || isCopyReport || Ability)
                    {

                        //Suicide Sheriff
                        if (deadPlayer.player == Sheriff1.Role && deadPlayer.killerIfExisting.Data.PlayerName == Sheriff1.Role.Data.PlayerName && Sheriff1.Suicide)
                        {
                            TypeofDeath = "" + TXT_Deathreason1;
                        }
                        if (deadPlayer.player == Sheriff2.Role && deadPlayer.killerIfExisting.Data.PlayerName == Sheriff2.Role.Data.PlayerName && Sheriff2.Suicide)
                        {
                            TypeofDeath = "" + TXT_Deathreason1;
                        }
                        if (deadPlayer.player == Sheriff3.Role && deadPlayer.killerIfExisting.Data.PlayerName == Sheriff3.Role.Data.PlayerName && Sheriff3.Suicide)
                        {
                            TypeofDeath = "" + TXT_Deathreason1;
                        }
                        if (deadPlayer.player == CopyCat.Role && deadPlayer.killerIfExisting.Data.PlayerName == CopyCat.Role.Data.PlayerName && CopyCat.copyRole == 1 && CopyCat.CopyStart && CopyCat.Suicide)
                        {
                            TypeofDeath = "" + TXT_Deathreason1;
                        }
                        //Cultist
                        if (deadPlayer.player == Cultist.Role && deadPlayer.killerIfExisting.Data.PlayerName == Cultist.Role.Data.PlayerName && Cultist.Suicide)
                        {
                            TypeofDeath = "" + TXT_Deathreason1;
                        }

                        //Lovers
                        if (deadPlayer.player == Cupid.Lover1 && deadPlayer.killerIfExisting.Data.PlayerName == Cupid.Lover1.Data.PlayerName && Cupid.lover1Suicide)
                        {
                            TypeofDeath = "" + TXT_Deathreason2;
                        }
                        if (deadPlayer.player == Cupid.Lover2 && deadPlayer.killerIfExisting.Data.PlayerName == Cupid.Lover2.Data.PlayerName && Cupid.lover2Suicide)
                        {
                            TypeofDeath = "" + TXT_Deathreason2;
                        }

                        //Hunters
                        if (deadPlayer.player == Hunter.Tracked && deadPlayer.killerIfExisting.Data.PlayerName == Hunter.Tracked.Data.PlayerName && Hunter.KilledByHunter)
                        {
                            TypeofDeath = "" + TXT_Deathreason3;
                        }
                        if (deadPlayer.player == Hunter.CopyTracked && deadPlayer.killerIfExisting.Data.PlayerName == Hunter.CopyTracked.Data.PlayerName && Hunter.KilledByCopyHunter)
                        {
                            TypeofDeath = "" + TXT_Deathreason3;
                        }

                        //Vector
                        if (ChallengerMod.Challenger.Vectorkill.Contains(deadPlayer.player))
                        {
                            TypeofDeath = "" + TXT_Deathreason4;
                        }





                        //Shield suicide
                        if (deadPlayer.player == Impostor1.Role && deadPlayer.killerIfExisting.Data.PlayerName == Impostor1.Role.Data.PlayerName && Impostor1.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Impostor2.Role && deadPlayer.killerIfExisting.Data.PlayerName == Impostor2.Role.Data.PlayerName && Impostor2.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Impostor3.Role && deadPlayer.killerIfExisting.Data.PlayerName == Impostor3.Role.Data.PlayerName && Impostor3.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Assassin.Role && deadPlayer.killerIfExisting.Data.PlayerName == Assassin.Role.Data.PlayerName && Assassin.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Vector.Role && deadPlayer.killerIfExisting.Data.PlayerName == Vector.Role.Data.PlayerName && Vector.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Morphling.Role && deadPlayer.killerIfExisting.Data.PlayerName == Morphling.Role.Data.PlayerName && Morphling.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Scrambler.Role && deadPlayer.killerIfExisting.Data.PlayerName == Scrambler.Role.Data.PlayerName && Scrambler.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Barghest.Role && deadPlayer.killerIfExisting.Data.PlayerName == Barghest.Role.Data.PlayerName && Barghest.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Ghost.Role && deadPlayer.killerIfExisting.Data.PlayerName == Ghost.Role.Data.PlayerName && Ghost.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Sorcerer.Role && deadPlayer.killerIfExisting.Data.PlayerName == Sorcerer.Role.Data.PlayerName && Sorcerer.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Guesser.Role && deadPlayer.killerIfExisting.Data.PlayerName == Guesser.Role.Data.PlayerName && Guesser.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Basilisk.Role && deadPlayer.killerIfExisting.Data.PlayerName == Basilisk.Role.Data.PlayerName && Basilisk.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Mesmer.Role && deadPlayer.killerIfExisting.Data.PlayerName == Mesmer.Role.Data.PlayerName && Mesmer.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Reaper.Role && deadPlayer.killerIfExisting.Data.PlayerName == Reaper.Role.Data.PlayerName && Reaper.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Saboteur.Role && deadPlayer.killerIfExisting.Data.PlayerName == Saboteur.Role.Data.PlayerName && Saboteur.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Outlaw.Role && deadPlayer.killerIfExisting.Data.PlayerName == Outlaw.Role.Data.PlayerName && Outlaw.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == Mercenary.Role && deadPlayer.killerIfExisting.Data.PlayerName == Mercenary.Role.Data.PlayerName && Mercenary.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }
                        if (deadPlayer.player == CopyCat.Role && deadPlayer.killerIfExisting.Data.PlayerName == CopyCat.Role.Data.PlayerName && CopyCat.SuicideShield)
                        {
                            TypeofDeath = "" + TXT_Deathreason5;
                        }










                        if (timeSinceDeath > TimeRList.getFloat() * 1000)
                        {
                            if (LKTimer.getBool() == true)
                            {
                                if (LKInfo.getBool() == true)
                                {
                                    msg = "" + $"({Math.Round(timeSinceDeath / 1000)}s) - (" + TypeofDeath + ")";
                                }
                                else
                                {
                                    msg = "" + $"({Math.Round(timeSinceDeath / 1000)}s)";
                                }

                            }
                            else
                            {
                                if (LKInfo.getBool() == true)
                                {
                                    msg = $"(" + TypeofDeath + ")";
                                }
                                else
                                {
                                    msg = $"...";
                                }
                            }

                        }
                        else if (timeSinceDeath < TimeRName.getFloat() * 1000)
                        {
                            if (LKTimer.getBool() == true)
                            {
                                if (LKInfo.getBool() == true)
                                {
                                    msg = "" + $"({Math.Round(timeSinceDeath / 1000)}s)" + " - (" + TypeofDeath + ") - " + TXT_LawKiller + $"{deadPlayer.killerIfExisting.Data.PlayerName}" + " !";
                                }
                                else
                                {
                                    msg = "" + $"({Math.Round(timeSinceDeath / 1000)}s)" + TXT_LawKiller + $"{deadPlayer.killerIfExisting.Data.PlayerName}" + " !";
                                }
                            }
                            else
                            {
                                if (LKInfo.getBool() == true)
                                {
                                    msg = "" + "(" + TypeofDeath + ") - " + TXT_LawKiller + $"{deadPlayer.killerIfExisting.Data.PlayerName}" + " !";
                                }
                                else
                                {
                                    msg = "" + TXT_LawKiller + $"{deadPlayer.killerIfExisting.Data.PlayerName}" + " !";
                                }
                            }
                        }
                        else
                        {
                            if (LKTimer.getBool() == true)
                            {
                                var IDS = new Dictionary<byte, string>()
                                {
                                   { 0, STR_List1},
                                   { 1, STR_List2},
                                   { 2, STR_List1},
                                   { 3, STR_List2},
                                   { 4, STR_List1},
                                   { 5, STR_List2},
                                   { 6, STR_List1},
                                   { 7, STR_List2},
                                   { 8, STR_List1},
                                   { 9, STR_List2},
                                   { 10, STR_List1},
                                   { 11, STR_List2},
                                   { 12, STR_List1},
                                   { 13, STR_List2},
                                   { 14, STR_List1},
                                   { 15, STR_List2},
                                };

                                var ListofPlayer = "" + IDS[(byte)deadPlayer.killerIfExisting.Data.PlayerId];

                                if (LKInfo.getBool() == true)
                                {
                                    msg = "" + "(" + TypeofDeath + ") - " + $"({Math.Round(timeSinceDeath / 1000)}s)" + TXT_LawSuspect + $"{ListofPlayer}" + " !";


                                }
                                else
                                {
                                    msg = "" + $"({Math.Round(timeSinceDeath / 1000)}s)" + TXT_LawSuspect + $"{ListofPlayer}" + " !";

                                }
                            }
                            else
                            {
                                var IDS = new Dictionary<byte, string>()
                                  {
                                     {0, STR_List1},
                                     {1, STR_List2},
                                     {2, STR_List1},
                                     {3, STR_List2},
                                     {4, STR_List1},
                                     {5, STR_List2},
                                     {6, STR_List1},
                                     {7, STR_List2},
                                     {8, STR_List1},
                                     {9, STR_List2},
                                     {10, STR_List1},
                                     {11, STR_List2},
                                     {12, STR_List1},
                                     {13, STR_List2},
                                     {14, STR_List1},
                                     {15, STR_List2},
                                  };
                                var ListofPlayer = "" + IDS[(byte)deadPlayer.killerIfExisting.Data.PlayerId];

                                if (LKInfo.getBool() == true)
                                {
                                    msg = "" + "(" + TypeofDeath + ") - " + TXT_LawSuspect + $"{ListofPlayer}" + " !";
                                }
                                else
                                {
                                    msg = "" + TXT_LawSuspect + $"{ListofPlayer}" + " !";
                                }

                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(msg))
                    {

                        if (AmongUsClient.Instance.AmClient && DestroyableSingleton<HudManager>.Instance)
                        {

                            DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, msg);

                        }
                        if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            DestroyableSingleton<Assets.CoreScripts.Telemetry>.Instance.SendWho();
                        }
                    }


                }
            }

        }
    }
}