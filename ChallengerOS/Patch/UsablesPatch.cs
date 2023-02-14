using HarmonyLib;
using UnityEngine;
using System.Linq;
using static ChallengerMod.Challenger;
using static ChallengerMod.Roles;
using static ChallengerMod.Set.Data;
using PowerTools;
using ChallengerOS.Utils;

namespace ChallengerOS.VentPatch
{





    [HarmonyPatch(typeof(VentButton), nameof(VentButton.DoClick))]
    class VentButtonDoClickPatch
    {
        static bool Prefix(VentButton __instance) {
            if (__instance.currentTarget != null) __instance.currentTarget.Use();
            return false;
        }
    }

    

    

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    class VentButtonVisibilityPatch
    {
        static void Postfix(PlayerControl __instance) {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            if (__instance.AmOwner && __instance.roleCanUseVents() && !MeetingHud.Instance) {
                HudManager.Instance.ImpostorVentButton.Show();
            }
            else if (__instance.AmOwner && __instance.roleCanUseVents() && !MeetingHud.Instance && HudManager.Instance.ReportButton.isActiveAndEnabled || __instance.AmOwner && __instance.roleCanUseVents() && HudManager.Instance.ReportButton.isActiveAndEnabled) {
                HudManager.Instance.ImpostorVentButton.Show();
            }
            else if (__instance.AmOwner && MeetingHud.Instance)
            {
                HudManager.Instance.ImpostorVentButton.Hide();
            }
        }
    }

    

    internal class VisibleVentPatches
    {
        public static int ShipAndObjectsMask = LayerMask.GetMask(new string[]
        {
            "Ship",
            "Objects"
        });

        [HarmonyPatch(typeof(Vent), nameof(Vent.EnterVent))] 
        public static class EnterVentPatch
        {
            public static bool Prefix(Vent __instance, PlayerControl pc) {

                if (!__instance.EnterVentAnim) {
                    return false;
                }

                var truePosition = PlayerControl.LocalPlayer.GetTruePosition();

                Vector2 vector = pc.GetTruePosition() - truePosition;
                var magnitude = vector.magnitude;
                if (pc.AmOwner && magnitude < PlayerControl.LocalPlayer.myLight.LightRadius &&
                    !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude,
                        ShipAndObjectsMask)) {
                    __instance.GetComponent<SpriteAnim>().Play(__instance.EnterVentAnim, 1f);
                }

                if (pc.AmOwner && Constants.ShouldPlaySfx()) 
                {
                    SoundManager.Instance.StopSound(ShipStatus.Instance.VentEnterSound);
                    SoundManager.Instance.PlaySound(ShipStatus.Instance.VentEnterSound, false, 1f).pitch =
                        UnityEngine.Random.Range(0.8f, 1.2f);
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(Vent), nameof(Vent.ExitVent))] 
        public static class ExitVentPatch
        {
            public static bool Prefix(Vent __instance, PlayerControl pc) {

                if (!__instance.ExitVentAnim) {
                    return false;
                }

                var truePosition = PlayerControl.LocalPlayer.GetTruePosition();

                Vector2 vector = pc.GetTruePosition() - truePosition;
                var magnitude = vector.magnitude;
                if (pc.AmOwner && magnitude < PlayerControl.LocalPlayer.myLight.LightRadius &&
                    !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude,
                        ShipAndObjectsMask)) {
                    __instance.GetComponent<SpriteAnim>().Play(__instance.ExitVentAnim, 1f);
                }

                if (pc.AmOwner && Constants.ShouldPlaySfx()) 
                {
                    SoundManager.Instance.StopSound(ShipStatus.Instance.VentEnterSound);
                    SoundManager.Instance.PlaySound(ShipStatus.Instance.VentEnterSound, false, 1f).pitch =
                        UnityEngine.Random.Range(0.8f, 1.2f);
                }

                return false;
            }
        }
    }



  

    [HarmonyPatch(typeof(EmergencyMinigame), nameof(EmergencyMinigame.Update))]
    class EmergencyMinigameUpdatePatch
    {
        static void Postfix(EmergencyMinigame __instance)
        {
            var CanCallEmergency = true;
            var statusText = "";

            if (EmergencyDestroy)
            {
                CanCallEmergency = false;
                statusText = "" + TXT_Buzz;
            }
           

            if (!CanCallEmergency)
            {
                __instance.StatusText.text = statusText;
                __instance.NumberText.text = string.Empty;
                __instance.ClosedLid.gameObject.SetActive(true);
                __instance.OpenLid.gameObject.SetActive(false);
                __instance.ButtonActive = false;
                return;
            }
        }
    }

    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetName))]
    public static class SetBubbleName
    {
        public static void Postfix(ChatBubble __instance, [HarmonyArgument(0)] string playerName)
        {
            PlayerControl player = PlayerControl.AllPlayerControls.ToArray().ToList().FirstOrDefault(x => x.Data != null && x.Data.PlayerName.Equals(playerName));
            if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor && (Fake.Role != null && player.PlayerId == Fake.Role.PlayerId && __instance != null))
            {
                __instance.NameText.color = Palette.ImpostorRed;
            }
           
        }
    }

   



}