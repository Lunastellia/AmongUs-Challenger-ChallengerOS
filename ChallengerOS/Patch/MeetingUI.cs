using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ChallengerMod.Challenger;
using static ChallengerMod.Unity;
using ChallengerMod.RoleInfos;
using ChallengerOS.Utils;
using ChallengerMod;
using ChallengerOS.RPC;
using static ChallengerMod.Roles;

namespace ChallengerOS.GuessData
{
    [HarmonyPatch]
    class MeetingHudPatch
    {

        public static GameObject guesserUI;
        public static PassiveButton guesserUIExitButton;
        static void guesserOnClick(int buttonTarget, MeetingHud __instance)
        {
            if (guesserUI != null || !(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted)) return;
            __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(false));

            Transform container = UnityEngine.Object.Instantiate(__instance.transform.FindChild("PhoneUI"), __instance.transform);
            container.transform.localPosition = new Vector3(0, 0, -5f);
            guesserUI = container.gameObject;

            int i = 0;
            var buttonTemplate = __instance.playerStates[0].transform.FindChild("votePlayerBase");
            var maskTemplate = __instance.playerStates[0].transform.FindChild("MaskArea");
            var smallButtonTemplate = __instance.playerStates[0].Buttons.transform.Find("CancelButton");
            var textTemplate = __instance.playerStates[0].NameText;

            Transform exitButtonParent = (new GameObject()).transform;
            exitButtonParent.SetParent(container);
            Transform exitButton = UnityEngine.Object.Instantiate(buttonTemplate.transform, exitButtonParent);
            Transform exitButtonMask = UnityEngine.Object.Instantiate(maskTemplate, exitButtonParent);
            exitButton.gameObject.GetComponent<SpriteRenderer>().sprite = smallButtonTemplate.GetComponent<SpriteRenderer>().sprite; 
            exitButtonParent.transform.localPosition = new Vector3(2.725f, 2.1f, -5);
            exitButtonParent.transform.localScale = new Vector3(0.25f, 0.9f, 1);





            guesserUIExitButton = exitButton.GetComponent<PassiveButton>();
            guesserUIExitButton.OnClick.RemoveAllListeners();
            guesserUIExitButton.OnClick.AddListener((System.Action)(() => {
                __instance.playerStates.ToList().ForEach(x => {
                    x.gameObject.SetActive(true);
                    if (PlayerControl.LocalPlayer.Data.IsDead && x.transform.FindChild("ShootButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("ShootButton").gameObject);
                });
                UnityEngine.Object.Destroy(container.gameObject);
            }));

            List<Transform> buttons = new List<Transform>();
            Transform selectedButton = null;

            foreach (RoleInfo roleInfo in RoleInfo.allRoleInfos)
            {
                if (roleInfo.color == ChallengerMod.ColorTable.RedColor || 
                    roleInfo.color == ChallengerMod.ColorTable.AssassinColor || 
                    roleInfo.color == ChallengerMod.ColorTable.BarghestColor ||
                    roleInfo.color == ChallengerMod.ColorTable.GhostColor ||
                    roleInfo.color == ChallengerMod.ColorTable.VectorColor ||
                    roleInfo.color == ChallengerMod.ColorTable.MorphColor ||
                    roleInfo.color == ChallengerMod.ColorTable.ScramblerColor ||
                    roleInfo.color == ChallengerMod.ColorTable.GuesserColor ||
                    roleInfo.color == ChallengerMod.ColorTable.MesmerColor ||
                    roleInfo.color == ChallengerMod.ColorTable.BasiliskColor ||
                    roleInfo.color == ChallengerMod.ColorTable.ReaperColor ||
                    roleInfo.color == ChallengerMod.ColorTable.SaboteurColor ||
                    roleInfo.color == ChallengerMod.ColorTable.CrewmateColor || // Suppr All role crew/teammate
                    roleInfo.color == ChallengerMod.ColorTable.SheriffColor || // Suppr All sheriffs
                    (roleInfo.color == ChallengerMod.ColorTable.SheriffsColor && ChallengerOS.Utils.Option.CustomOptionHolder.SherifAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.GuardianColor && ChallengerOS.Utils.Option.CustomOptionHolder.GuardianAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.EngineerColor && ChallengerOS.Utils.Option.CustomOptionHolder.engineerAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.TimeLordColor && ChallengerOS.Utils.Option.CustomOptionHolder.TimeLordAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.HunterColor && ChallengerOS.Utils.Option.CustomOptionHolder.HunterAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.MysticColor && (ChallengerOS.Utils.Option.CustomOptionHolder.GuessMystic.getBool() == true || ChallengerOS.Utils.Option.CustomOptionHolder.SpiritAdd.getBool() == false)) ||
                    (roleInfo.color == ChallengerMod.ColorTable.SpiritColor && (ChallengerOS.Utils.Option.CustomOptionHolder.GuessSpirit.getBool() == true || ChallengerOS.Utils.Option.CustomOptionHolder.SpiritAdd.getBool() == false)) ||
                    (roleInfo.color == ChallengerMod.ColorTable.MayorColor && ChallengerOS.Utils.Option.CustomOptionHolder.MayorAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.DetectiveColor && ChallengerOS.Utils.Option.CustomOptionHolder.DetectiveAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.NightwatchColor && ChallengerOS.Utils.Option.CustomOptionHolder.NightwatcherAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.SpyColor & ChallengerOS.Utils.Option.CustomOptionHolder.SpyAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.InformantColor && ChallengerOS.Utils.Option.CustomOptionHolder.InforAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.BaitColor && ChallengerOS.Utils.Option.CustomOptionHolder.BaitAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.MentalistColor && ChallengerOS.Utils.Option.CustomOptionHolder.MentalistAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.BuilderColor && ChallengerOS.Utils.Option.CustomOptionHolder.BuilderAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.DictatorColor && ChallengerOS.Utils.Option.CustomOptionHolder.DictatorAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.SentinelColor && ChallengerOS.Utils.Option.CustomOptionHolder.SentinelAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.LawkeeperColor && ChallengerOS.Utils.Option.CustomOptionHolder.LawkeeperAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.TeammateColor && ChallengerOS.Utils.Option.CustomOptionHolder.MateAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.FakeColor && (ChallengerOS.Utils.Option.CustomOptionHolder.GuessFake.getBool() == true || ChallengerOS.Utils.Option.CustomOptionHolder.FakeAdd.getBool() == false)) ||
                    (roleInfo.color == ChallengerMod.ColorTable.LeaderColor && ChallengerOS.Utils.Option.CustomOptionHolder.LeaderAdd.getBool() == false) ||
                    roleInfo.color == ChallengerMod.ColorTable.DoctorColor ||
                    roleInfo.color == ChallengerMod.ColorTable.TravelerColor ||
                    roleInfo.color == ChallengerMod.ColorTable.SlaveColor ||
                    (roleInfo.color == ChallengerMod.ColorTable.CupidColor && ChallengerOS.Utils.Option.CustomOptionHolder.CupidAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.CulteColor && ChallengerOS.Utils.Option.CustomOptionHolder.CultisteAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.OutlawColor && ChallengerOS.Utils.Option.CustomOptionHolder.OutlawAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.JesterColor && ChallengerOS.Utils.Option.CustomOptionHolder.JesterAdd.getBool() == false) ||
                     (roleInfo.color == ChallengerMod.ColorTable.CursedColor && ChallengerOS.Utils.Option.CustomOptionHolder.CursedAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.EaterColor && ChallengerOS.Utils.Option.CustomOptionHolder.EaterAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.ArsonistColor && ChallengerOS.Utils.Option.CustomOptionHolder.ArsonistAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.MercenaryColor && ChallengerOS.Utils.Option.CustomOptionHolder.MercenaryAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.CopyCatColor && ChallengerOS.Utils.Option.CustomOptionHolder.CopyCatAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.SurvivorColor && ChallengerOS.Utils.Option.CustomOptionHolder.SorcererAdd.getBool() == false) ||
                    (roleInfo.color == ChallengerMod.ColorTable.RevengerColor && ChallengerOS.Utils.Option.CustomOptionHolder.RevengerAdd.getBool() == false) ||
                    roleInfo.color == ChallengerMod.ColorTable.SorcererColor) continue; // Not guessable roles




                



                Transform buttonParent = (new GameObject()).transform;
                buttonParent.SetParent(container);
                Transform button = UnityEngine.Object.Instantiate(buttonTemplate, buttonParent);
                Transform buttonMask = UnityEngine.Object.Instantiate(maskTemplate, buttonParent);
                TMPro.TextMeshPro label = UnityEngine.Object.Instantiate(textTemplate, button);
                buttons.Add(button);
                int row = i / 4, col = i % 4;
                buttonParent.localPosition = new Vector3(-2.725f + 1.83f * col, 1.5f - 0.45f * row, -5);
                buttonParent.localScale = new Vector3(0.55f, 0.55f, 1f);
               // buttonParent.transform.gameObject.GetComponent<SpriteRenderer>().sprite = GuessUIIco;
                label.text = Helpers.cs(roleInfo.color, roleInfo.name);
                label.alignment = TMPro.TextAlignmentOptions.Center;
                label.transform.localPosition = new Vector3(0, 0, label.transform.localPosition.z);
                label.transform.localScale *= 1.7f;
                

                int copiedIndex = i;

                button.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
                if (!PlayerControl.LocalPlayer.Data.IsDead) button.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() => 
                {
                    if (selectedButton != button)
                    {
                        selectedButton = button;
                        buttons.ForEach(x => x.GetComponent<SpriteRenderer>().color = x == selectedButton ? ChallengerMod.ColorTable.VectorColor : ChallengerMod.ColorTable.WhiteColor);
                    }
                    else
                    {
                        PlayerControl target = Helpers.playerById((byte)__instance.playerStates[buttonTarget].TargetPlayerId);
                        if (!(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted) || target == null || Roles.Guesser.remainingShots <= 0) return;

                        var mainRoleInfo = RoleInfo.getRoleInfoForPlayer(target).FirstOrDefault();
                        if (mainRoleInfo == null) return;


                        target = (mainRoleInfo == roleInfo) ? target : PlayerControl.LocalPlayer;
                       
                        if (ChallengerOS.Utils.Option.CustomOptionHolder.GuessDie.getBool() == true || Roles.Guesser.remainingShots <= 1)
                        {
                            GuesserNotDie = false;
                        }
                        else
                        {
                            GuesserNotDie = true;
                        }

                        // Reset the GUI
                        __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                        UnityEngine.Object.Destroy(container.gameObject);
                        if ((Roles.Guesser.hasMultipleShotsPerMeeting == false) && Roles.Guesser.remainingShots > 1 &&
                        (
                        (ChallengerOS.Utils.Option.CustomOptionHolder.GuessDie.getBool() == false) 
                        || (ChallengerOS.Utils.Option.CustomOptionHolder.GuessDie.getBool() == true && target != PlayerControl.LocalPlayer))
                        )

                            


                            __instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == target.PlayerId && x.transform.FindChild("ShootButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("ShootButton").gameObject); });
                        else
                            __instance.playerStates.ToList().ForEach(x => { if (x.transform.FindChild("ShootButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("ShootButton").gameObject); });


                        if (ChallengerMod.Challenger.GuesserNotDie == false || target != Roles.Guesser.Role)
                        { // Shoot player
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.GuesserShoot, Hazel.SendOption.Reliable, -1);
                            writer.Write(target.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.guesserShoot(target.PlayerId);
                        }
                        else
                        {
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.GuesserFail, Hazel.SendOption.Reliable, -1);
                            writer.Write(target.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.guesserFail(target.PlayerId);
                            

                        }
                    }

                }));

                i++;
            }
            container.transform.localScale *= 1f;
        }
        static void SetLoverOnClick(int buttonTarget, MeetingHud __instance)
        {
            PlayerControl target = Helpers.playerById((byte)__instance.playerStates[buttonTarget].TargetPlayerId);
            if (Roles.Cupid.Lover1 == null)
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetLover1, Hazel.SendOption.Reliable, -1);
                writer.Write(target.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setLover1(target.PlayerId);
                Cupid.Love1Used = true;
                SoundManager.Instance.PlaySound(PoisonClip, false, 100f);
                __instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == target.PlayerId && x.transform.FindChild("LoveButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("LoveButton").gameObject); });

            }
            else if (Roles.Cupid.Lover1 != null && Roles.Cupid.Lover2 == null && target != Roles.Cupid.Lover1)
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetLover2, Hazel.SendOption.Reliable, -1);
                writer.Write(target.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setLover2(target.PlayerId);
                Cupid.Love2Used = true;
                SoundManager.Instance.PlaySound(PoisonClip, false, 100f);
                __instance.playerStates.ToList().ForEach(x => { if (x.transform.FindChild("LoveButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("LoveButton").gameObject); });
            }
            else { }
        }
        static void SetVotedOnClick(int buttonTarget, MeetingHud __instance)
        {
            PlayerControl target = Helpers.playerById((byte)__instance.playerStates[buttonTarget].TargetPlayerId);

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetAllVoteTarget, Hazel.SendOption.Reliable, -1);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.setAllVoteTarget(target.PlayerId);
            Dictator.SuperVote = true;
            SoundManager.Instance.PlaySound(Used, false, 100f);
            __instance.playerStates.ToList().ForEach(x => { if (x.transform.FindChild("VotedButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("VotedButton").gameObject); });

        }

        

       /* public static GameObject SuperCrewUI;
        public static PassiveButton SuperCrewUIExitButton;
        static void SuperCrewOnClick(int buttonTarget, MeetingHud __instance)
        {
            if (SuperCrewUI != null || !(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted)) return;
            __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(false));

            Transform container = UnityEngine.Object.Instantiate(__instance.transform.FindChild("PhoneUI"), __instance.transform);
            container.transform.localPosition = new Vector3(0, 0, -5f);
            SuperCrewUI = container.gameObject;

            int i = 0;
            var buttonTemplate = __instance.playerStates[0].transform.FindChild("votePlayerBase");
            var maskTemplate = __instance.playerStates[0].transform.FindChild("MaskArea");
            var smallButtonTemplate = __instance.playerStates[0].Buttons.transform.Find("CancelButton");
            var textTemplate = __instance.playerStates[0].NameText;

            Transform exitButtonParent = (new GameObject()).transform;
            exitButtonParent.SetParent(container);
            Transform exitButton = UnityEngine.Object.Instantiate(buttonTemplate.transform, exitButtonParent);
            Transform exitButtonMask = UnityEngine.Object.Instantiate(maskTemplate, exitButtonParent);
            exitButton.gameObject.GetComponent<SpriteRenderer>().sprite = smallButtonTemplate.GetComponent<SpriteRenderer>().sprite;
            exitButtonParent.transform.localPosition = new Vector3(2.725f, 2.1f, -5);
            exitButtonParent.transform.localScale = new Vector3(0.25f, 0.9f, 1);





            SuperCrewUIExitButton = exitButton.GetComponent<PassiveButton>();
            SuperCrewUIExitButton.OnClick.RemoveAllListeners();
            SuperCrewUIExitButton.OnClick.AddListener((System.Action)(() => {
                __instance.playerStates.ToList().ForEach(x => {
                    x.gameObject.SetActive(true);
                    if (PlayerControl.LocalPlayer.Data.IsDead && x.transform.FindChild("SuperCrewButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("SuperCrewButton").gameObject);
                });
                UnityEngine.Object.Destroy(container.gameObject);
            }));

            List<Transform> buttons = new List<Transform>();
            Transform selectedButton = null;

            foreach (RoleInfo roleInfo in RoleInfo.allRoleInfos)
            {
                if (roleInfo.color == ChallengerMod.ColorTable.RedColor ||
                    roleInfo.color == ChallengerMod.ColorTable.AssassinColor ||
                    roleInfo.color == ChallengerMod.ColorTable.BarghestColor ||
                    roleInfo.color == ChallengerMod.ColorTable.GhostColor ||
                    roleInfo.color == ChallengerMod.ColorTable.VectorColor ||
                    roleInfo.color == ChallengerMod.ColorTable.MorphColor ||
                    roleInfo.color == ChallengerMod.ColorTable.ScramblerColor ||
                    roleInfo.color == ChallengerMod.ColorTable.GuesserColor ||
                    roleInfo.color == ChallengerMod.ColorTable.MesmerColor ||
                    roleInfo.color == ChallengerMod.ColorTable.BasiliskColor ||
                    roleInfo.color == ChallengerMod.ColorTable.ReaperColor ||
                    roleInfo.color == ChallengerMod.ColorTable.SaboteurColor ||
                    roleInfo.color == ChallengerMod.ColorTable.SorcererColor ||
                    roleInfo.color == ChallengerMod.ColorTable.CrewmateColor ||
                    roleInfo.color == ChallengerMod.ColorTable.SheriffColor ||
                    roleInfo.color == ChallengerMod.ColorTable.FakeColor ||
                    roleInfo.color == ChallengerMod.ColorTable.TeammateColor ||
                    roleInfo.color == ChallengerMod.ColorTable.DoctorColor ||
                    roleInfo.color == ChallengerMod.ColorTable.TravelerColor ||
                    roleInfo.color == ChallengerMod.ColorTable.SlaveColor ||
                    roleInfo.color == ChallengerMod.ColorTable.CupidColor ||
                    roleInfo.color == ChallengerMod.ColorTable.CulteColor  ||
                    roleInfo.color == ChallengerMod.ColorTable.OutlawColor  ||
                    roleInfo.color == ChallengerMod.ColorTable.JesterColor  ||
                    roleInfo.color == ChallengerMod.ColorTable.CursedColor  ||
                    roleInfo.color == ChallengerMod.ColorTable.EaterColor  ||
                    roleInfo.color == ChallengerMod.ColorTable.ArsonistColor  ||
                    roleInfo.color == ChallengerMod.ColorTable.MercenaryColor  ||
                    roleInfo.color == ChallengerMod.ColorTable.CopyCatColor  ||
                    roleInfo.color == ChallengerMod.ColorTable.SurvivorColor  ||
                    roleInfo.color == ChallengerMod.ColorTable.RevengerColor  ||
                    

                    (roleInfo.color == ChallengerMod.ColorTable.SheriffsColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.SherifAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.SherifAdd.getBool() == true && (Sheriff1.Role != null || Sheriff2.Role != null || Sheriff3.Role != null)))) ||
                   
                    (roleInfo.color == ChallengerMod.ColorTable.GuardianColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.GuardianAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.GuardianAdd.getBool() == false && Guardian.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.EngineerColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.engineerAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.engineerAdd.getBool() == false && Engineer.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.TimeLordColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.TimeLordAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.TimeLordAdd.getBool() == false && Timelord.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.HunterColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.HunterAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.HunterAdd.getBool() == false && Hunter.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.MysticColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.MysticAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.MysticAdd.getBool() == false && Mystic.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.SpiritColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.SpiritAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.SpiritAdd.getBool() == false && Spirit.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.MayorColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.MayorAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.MayorAdd.getBool() == false && Mayor.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.DetectiveColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.DetectiveAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.DetectiveAdd.getBool() == false && Detective.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.NightwatchColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.NightwatcherAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.NightwatcherAdd.getBool() == false && Nightwatch.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.SpyColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.SpyAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.SpyAdd.getBool() == false && Spy.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.InformantColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.InforAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.InforAdd.getBool() == false && Informant.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.BaitColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.BaitAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.BaitAdd.getBool() == false && Bait.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.MentalistColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.MentalistAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.MentalistAdd.getBool() == false && Mentalist.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.BuilderColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.BuilderAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.BuilderAdd.getBool() == false && Builder.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.DictatorColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.DictatorAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.DictatorAdd.getBool() == false && Dictator.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.SentinelColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.SentinelAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.SentinelAdd.getBool() == false && Sentinel.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.LawkeeperColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.LawkeeperAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.LawkeeperAdd.getBool() == false && Lawkeeper.Role != null))) ||

                    (roleInfo.color == ChallengerMod.ColorTable.LeaderColor && ((ChallengerOS.Utils.Option.CustomOptionHolder.LeaderAdd.getBool() == false)
                    || (ChallengerOS.Utils.Option.CustomOptionHolder.LeaderAdd.getBool() == false && Leader.Role != null)))

                    ) continue; // Not guessable roles


                Transform buttonParent = (new GameObject()).transform;
                buttonParent.SetParent(container);
                Transform button = UnityEngine.Object.Instantiate(buttonTemplate, buttonParent);
                Transform buttonMask = UnityEngine.Object.Instantiate(maskTemplate, buttonParent);
                TMPro.TextMeshPro label = UnityEngine.Object.Instantiate(textTemplate, button);
                buttons.Add(button);
                int row = i / 4, col = i % 4;
                buttonParent.localPosition = new Vector3(-2.725f + 1.83f * col, 1.5f - 0.45f * row, -5);
                buttonParent.localScale = new Vector3(0.55f, 0.55f, 1f);
                // buttonParent.transform.gameObject.GetComponent<SpriteRenderer>().sprite = GuessUIIco;
                label.text = Helpers.cs(roleInfo.color, roleInfo.name);
                label.alignment = TMPro.TextAlignmentOptions.Center;
                label.transform.localPosition = new Vector3(0, 0, label.transform.localPosition.z);
                label.transform.localScale *= 1.7f;


                int copiedIndex = i;

                button.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
                if (!PlayerControl.LocalPlayer.Data.IsDead) button.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
                {
                    if (selectedButton != button)
                    {
                        selectedButton = button;
                        buttons.ForEach(x => x.GetComponent<SpriteRenderer>().color = x == selectedButton ? ChallengerMod.ColorTable.VectorColor : ChallengerMod.ColorTable.WhiteColor);
                    }
                    else
                    {
                        PlayerControl target = Helpers.playerById((byte)__instance.playerStates[buttonTarget].TargetPlayerId);
                        if (!(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted) || target == null || Roles.SuperCrew.remainingShots <= 0) return;

                        ChallengerMod.Roles.SuperCrew.NewRole = roleInfo.name + "";
                        PlayerControl.LocalPlayer = Dictator.Role;



                        // Reset the GUI
                        __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                        UnityEngine.Object.Destroy(container.gameObject);
                        __instance.playerStates.ToList().ForEach(x => { if (x.transform.FindChild("SuperCrewButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("SuperCrewButton").gameObject); });


                        
                    }

                }));

                i++;
            }
            container.transform.localScale *= 1f;
        }*/


        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
        class PlayerVoteAreaSelectPatch
        {
            static bool Prefix(MeetingHud __instance)
            {
                return !(PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer == Roles.Guesser.Role && guesserUI != null);
            }
        }


        static void populateButtonsPostfix(MeetingHud __instance)
        {
            

            // Add Guesser Buttons
            if (Roles.Guesser.Role != null && PlayerControl.LocalPlayer == Roles.Guesser.Role && !Roles.Guesser.Role.Data.IsDead && Roles.Guesser.remainingShots > 0 && ChallengerMod.Challenger.AbilityDisabled == false)
            {
                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    if (playerVoteArea.AmDead || playerVoteArea.TargetPlayerId == Roles.Guesser.Role.PlayerId) continue;

                    GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                    GameObject targetBox = UnityEngine.Object.Instantiate(template, playerVoteArea.transform);
                    targetBox.name = "ShootButton";
                    targetBox.transform.localPosition = new Vector3(-0.35f, 0f, -1f);
                    SpriteRenderer renderer = targetBox.GetComponent<SpriteRenderer>();
                    renderer.sprite = GuessIco;
                    PassiveButton button = targetBox.GetComponent<PassiveButton>();
                    button.OnClick.RemoveAllListeners();
                    int copiedIndex = i;
                    button.OnClick.AddListener((System.Action)(() => guesserOnClick(copiedIndex, __instance)));
                }
            }
            // Add Cupid Buttons
            if (Roles.Cupid.Role != null && PlayerControl.LocalPlayer == Roles.Cupid.Role && !Roles.Cupid.Role.Data.IsDead && (Roles.Cupid.Lover2 == null))
                
            {
                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    if (playerVoteArea.AmDead || playerVoteArea.TargetPlayerId == Roles.Cupid.Role.PlayerId 
                        || (Roles.Cupid.Lover1 != null && playerVoteArea.TargetPlayerId == Roles.Cupid.Lover1.PlayerId)
                        || (Roles.Cupid.Lover2 != null && playerVoteArea.TargetPlayerId == Roles.Cupid.Lover1.PlayerId)
                        ) continue;

                    GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                    GameObject targetBox = UnityEngine.Object.Instantiate(template, playerVoteArea.transform);
                    targetBox.name = "LoveButton";
                    targetBox.transform.localPosition = new Vector3(-0.35f, 0f, -1f);
                    SpriteRenderer renderer = targetBox.GetComponent<SpriteRenderer>();
                    renderer.sprite = miniloveIco;
                    PassiveButton button = targetBox.GetComponent<PassiveButton>();
                    button.OnClick.RemoveAllListeners();
                    int copiedIndex = i;
                    button.OnClick.AddListener((System.Action)(() => SetLoverOnClick(copiedIndex, __instance)));
                }
            }
            // Add Dictator Buttons
            if ((Roles.Dictator.Role != null && PlayerControl.LocalPlayer == Roles.Dictator.Role && !Roles.Dictator.Role.Data.IsDead && (!Dictator.SuperVote))
                || (Roles.CopyCat.Role != null && PlayerControl.LocalPlayer == Roles.CopyCat.Role && !Roles.CopyCat.Role.Data.IsDead && CopyCat.CopyStart == true && CopyCat.copyRole == 16 && (!CopyCat.SuperVote))
                )

            {
                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    if (playerVoteArea.AmDead || playerVoteArea.TargetPlayerId == Roles.Dictator.Role.PlayerId
                        || (Roles.CopyCat.Role != null && Dictator.Role.Data.IsDead && CopyCat.copyRole == 16 && CopyCat.CopyStart == true
                        && playerVoteArea.TargetPlayerId == Roles.CopyCat.Role.PlayerId)
                        ) continue;

                    GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                    GameObject targetBox = UnityEngine.Object.Instantiate(template, playerVoteArea.transform);
                    targetBox.name = "VotedButton";
                    targetBox.transform.localPosition = new Vector3(-0.35f, 0f, -1f);
                    SpriteRenderer renderer = targetBox.GetComponent<SpriteRenderer>();
                    renderer.sprite = MakeVoterIco;
                    PassiveButton button = targetBox.GetComponent<PassiveButton>();
                    button.OnClick.RemoveAllListeners();
                    int copiedIndex = i;
                    button.OnClick.AddListener((System.Action)(() => SetVotedOnClick(copiedIndex, __instance)));
                }
            }
            // Add SuperCrew Buttons
           /* if ((Roles.Timelord.Role != null && PlayerControl.LocalPlayer == Roles.Timelord.Role && !Roles.Timelord.Role.Data.IsDead)
                )

            {
                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    if (playerVoteArea.TargetPlayerId != Roles.Timelord.Role.PlayerId
                        ) continue;

                    GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                    GameObject targetBox = UnityEngine.Object.Instantiate(template, playerVoteArea.transform);
                    targetBox.name = "SuperCrewButton";
                    targetBox.transform.localPosition = new Vector3(-0.35f, 0f, -1f);
                    SpriteRenderer renderer = targetBox.GetComponent<SpriteRenderer>();
                    renderer.sprite = MakeVoterIco;
                    PassiveButton button = targetBox.GetComponent<PassiveButton>();
                    button.OnClick.RemoveAllListeners();
                    int copiedIndex = i;
                    button.OnClick.AddListener((System.Action)(() => SuperCrewOnClick(copiedIndex, __instance)));
                }
            }*/
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.ServerStart))]
        class MeetingServerStartPatch
        {
            static void Postfix(MeetingHud __instance)
            {
                populateButtonsPostfix(__instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Deserialize))]
        class MeetingDeserializePatch
        {
            static void Postfix(MeetingHud __instance, [HarmonyArgument(0)] MessageReader reader, [HarmonyArgument(1)] bool initialState)
            {
                // Add swapper buttons
                if (initialState)
                {
                    populateButtonsPostfix(__instance);
                }
            }
        }

        

        
    }
}