using HarmonyLib;
using System;
using static ChallengerMod.Roles;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ChallengerMod.RoleInfos;
using static ChallengerMod.Set.Data;


namespace ChallengerOS.Intropatch
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    [HarmonyPatch]
    class IntroCutsceneOnDestroy {
        public static void SetPlayersTeam(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> Team) {
            // Intro solo teams
            if (PlayerControl.LocalPlayer == Cupid.Role 
                || PlayerControl.LocalPlayer == Cultist.Role 
                || PlayerControl.LocalPlayer == Outlaw.Role
                || PlayerControl.LocalPlayer == Jester.Role
                || PlayerControl.LocalPlayer == Eater.Role 
                || PlayerControl.LocalPlayer == Arsonist.Role
                || PlayerControl.LocalPlayer == Cursed.Role
                || PlayerControl.LocalPlayer == Survivor.Role
                || (PlayerControl.LocalPlayer == Impostor1.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Impostor2.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Impostor3.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Assassin.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Vector.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Morphling.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Scrambler.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Barghest.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Ghost.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Guesser.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Sorcerer.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Basilisk.Role && ChallengerMod.Challenger.UnknowImpostors)
                || (PlayerControl.LocalPlayer == Reaper.Role && ChallengerMod.Challenger.UnknowImpostors)
                ) 
            {
                var AloneTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                AloneTeam.Add(PlayerControl.LocalPlayer);
                Team = AloneTeam;
            }

            // Add the Fake (for the Impostors)
            if (Fake.Role != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor) {
                List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
                var ImpoTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>(); 
                ImpoTeam.Add(PlayerControl.LocalPlayer);
                foreach (PlayerControl p in players) {
                    if (PlayerControl.LocalPlayer != p && (p == Fake.Role || p.Data.Role.IsImpostor))
                        ImpoTeam.Add(p);
                }
                Team = ImpoTeam;
            }
           

        }

        public static void SetIntroScreen(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> Team) {
            List<RoleInfo> _RoleInfo = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer);
            RoleInfo roleInfo = _RoleInfo.FirstOrDefault();
            if (roleInfo == null) return;
            if (roleInfo.isSpecialRole) 
            {
                if (PlayerControl.LocalPlayer == Arsonist.Role)
                {
                    var SpecialColor = RoleInfo._Arsonist.color;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = RoleInfo._Arsonist.name;
                    __instance.TeamTitle.color = SpecialColor;
                }
                if (PlayerControl.LocalPlayer == Jester.Role)
                {
                    var SpecialColor = RoleInfo._Jester.color;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = RoleInfo._Jester.name;
                    __instance.TeamTitle.color = SpecialColor;
                }
                if (PlayerControl.LocalPlayer == Eater.Role)
                {
                    var SpecialColor = RoleInfo._Eater.color;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = RoleInfo._Eater.name;
                    __instance.TeamTitle.color = SpecialColor;
                }
                if (PlayerControl.LocalPlayer == Cupid.Role)
                {
                    var SpecialColor = RoleInfo._Cupid.color;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = RoleInfo._Cupid.name;
                    __instance.TeamTitle.color = SpecialColor;
                }
                if (PlayerControl.LocalPlayer == Cultist.Role)
                {
                    var SpecialColor = RoleInfo._Cultist.color;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = RoleInfo._Cultist.name;
                    __instance.TeamTitle.color = SpecialColor;
                }
                if (PlayerControl.LocalPlayer == Outlaw.Role)
                {
                    var SpecialColor = RoleInfo._Outlaw.color;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = RoleInfo._Outlaw.name;
                    __instance.TeamTitle.color = SpecialColor;
                }
                if (PlayerControl.LocalPlayer == Cursed.Role)
                {
                    var SpecialColor = RoleInfo._Cursed.color;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = RoleInfo._Cursed.name;
                    __instance.TeamTitle.color = SpecialColor;
                }

                if (PlayerControl.LocalPlayer == Survivor.Role)
                {
                    var SpecialColor = ChallengerMod.ColorTable.DuoColor;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = SizeTT + R_CrewmateColor + Role_Crewmate + CC + C_WhiteColor + " / " + CC + R_ImpostorColor + Role_Impostor + CC + CZ ;
                }
                if (PlayerControl.LocalPlayer == Mercenary.Role)
                {
                    var SpecialColor = ChallengerMod.ColorTable.DuoColor;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = SizeTT + R_CrewmateColor + Role_Crewmate + CC + C_WhiteColor + " / " + CC + R_ImpostorColor + Role_Impostor + CC + CZ;
                }
                if (PlayerControl.LocalPlayer == CopyCat.Role)
                {
                    var SpecialColor = ChallengerMod.ColorTable.DuoColor;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = SizeTT + R_CrewmateColor + Role_Crewmate + CC + C_WhiteColor + " / " + CC + R_ImpostorColor + Role_Impostor + CC + CZ;
                }
                if (PlayerControl.LocalPlayer == Revenger.Role)
                {
                    var SpecialColor = ChallengerMod.ColorTable.DuoColor;
                    __instance.BackgroundBar.material.color = SpecialColor;
                    __instance.TeamTitle.text = SizeTT + R_CrewmateColor + Role_Crewmate + CC + C_WhiteColor + " / " + CC + R_ImpostorColor + Role_Impostor + CC + CZ;
                }




            }
            else
            {
                if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                {
                    var ImpoColor = ChallengerMod.ColorTable.ImpostorColor;
                    __instance.BackgroundBar.material.color = ImpoColor;
                    __instance.TeamTitle.text = R_ImpostorColor + Role_Impostor + CC;
                    
                }
                else
                {
                    var CrewColor = ChallengerMod.ColorTable.CrewmateColor;
                    __instance.BackgroundBar.material.color = CrewColor;
                    __instance.TeamTitle.text = R_CrewmateColor + Role_Crewmate + CC;
                }

            }
        }

        public static IEnumerator<WaitForSeconds> EndShowRole(IntroCutscene __instance) {
            yield return new WaitForSeconds(5f);
            __instance.YouAreText.gameObject.SetActive(false);
            __instance.RoleText.gameObject.SetActive(false);
            __instance.RoleBlurbText.gameObject.SetActive(false);
            __instance.ourCrewmate.gameObject.SetActive(false);
        }
        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CreatePlayer))]
        class CreatePlayerPatch
        {
            public static void Postfix(IntroCutscene __instance, bool impostorPositioning, ref PoolablePlayer __result)
            {
                if (impostorPositioning) __result.SetNameColor(Palette.ImpostorRed);
            }
        }
        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
        class SetUpRoleTextPatch {
            static public void SetRoleTexts(IntroCutscene __instance) {
                // Don't override the intro of the vanilla roles
                List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer);
                RoleInfo roleInfo = infos.FirstOrDefault();
                __instance.RoleBlurbText.text = "";
                if (roleInfo != null) {

                    if (Teammate1.Role != null && PlayerControl.LocalPlayer == Teammate1.Role)
                    {
                        __instance.RoleText.text = Role_Teammate;
                        __instance.RoleText.color = roleInfo.color;
                        __instance.RoleBlurbText.text = "<size=2>" + roleInfo.introDescription + "</size>";
                        __instance.RoleBlurbText.color = roleInfo.color;
                        ChallengerMod.Challenger.IntroScreen = true;
                    }
                    else if (Teammate2.Role != null && PlayerControl.LocalPlayer == Teammate2.Role)
                    {
                        __instance.RoleText.text = Role_Teammate;
                        __instance.RoleText.color = roleInfo.color;
                        __instance.RoleBlurbText.text = "<size=2>" + roleInfo.introDescription + "</size>";
                        __instance.RoleBlurbText.color = roleInfo.color;
                        ChallengerMod.Challenger.IntroScreen = true;
                    }
                    else
                    {
                        __instance.RoleText.text = roleInfo.name;
                        __instance.RoleText.color = roleInfo.color;
                        __instance.RoleBlurbText.text = "<size=2>" + roleInfo.introDescription + "</size>";
                        __instance.RoleBlurbText.color = roleInfo.color;
                        ChallengerMod.Challenger.IntroScreen = true;
                    }
                }
            }
            public static bool Prefix(IntroCutscene __instance)
            {
                
                HudManager.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) => {
                    SetRoleTexts(__instance);
                })));
                return true;
            }

        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch {




            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                SetPlayersTeam(__instance, ref teamToDisplay);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                SetIntroScreen(__instance, ref teamToDisplay);
                
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch {
            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                SetPlayersTeam(__instance, ref yourTeam);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                SetIntroScreen(__instance, ref yourTeam);

                
            }
        }
    }
}

