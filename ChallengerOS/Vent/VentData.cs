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

namespace ChallengerOS
{

    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        public static bool Prefix(bool __runOriginal, Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse)
        {
            if (!__runOriginal)
            {
                return false;
            }

            float num = float.MaxValue;
            PlayerControl @object = pc.Object;

            bool roleCouldUse = @object.roleCanUseVents();

            var usableDistance = __instance.UsableDistance;
            if (__instance.name.Contains("Barghest_"))
            {
                if ((Barghest.Role != PlayerControl.LocalPlayer && ChallengerOS.Utils.Option.CustomOptionHolder.CanUseBarghestVent.getSelection() == 0)
                    || (!PlayerControl.LocalPlayer.Data.Role.IsImpostor && ChallengerOS.Utils.Option.CustomOptionHolder.CanUseBarghestVent.getSelection() == 1)
                    || (!PlayerControl.LocalPlayer.Data.Role.IsImpostor && !CanUseVent && ChallengerOS.Utils.Option.CustomOptionHolder.CanUseBarghestVent.getSelection() == 2)
                    || (!CanUseVent && ChallengerOS.Utils.Option.CustomOptionHolder.CanUseBarghestVent.getSelection() == 4)
                    )

                {
                    canUse = false;
                    couldUse = false;
                    __result = num;
                    return false;
                }
                else
                {
                    usableDistance = 0.4f;
                }
            }
            else if (__instance.name.Contains("SealedVent_"))
            {
                canUse = couldUse = false;
                __result = num;
                return false;
            }

            else if (PlayerControl.GameOptions.MapId == 5)
            {
                if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor && (__instance.name.StartsWith("LowerCentralVent") || __instance.name.StartsWith("UpperCentralVent")))
                {
                    canUse = couldUse = false;
                    __result = num;
                    return false;
                }
            }

            couldUse = (@object.inVent || roleCouldUse) && !pc.IsDead && (@object.CanMove || @object.inVent);
            canUse = couldUse;
            if (canUse)
            {
                Vector3 center = @object.Collider.bounds.center;
                Vector3 position = __instance.transform.position;
                num = Vector2.Distance(center, position);

                canUse &= (num <= usableDistance && !PhysicsHelpers.AnythingBetween(@object.Collider, center, position, Constants.ShipOnlyMask, false));
            }
            __result = num;
            return false;
        }
    }
    [HarmonyPatch(typeof(Vent), nameof(Vent.Use))]
    public static class VentUsePatch
    {
        public static bool Prefix(Vent __instance)
        {
            bool canUse;
            bool couldUse;
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out canUse, out couldUse);
            bool canMoveInVents = true;
            if (!canUse) return false;

            bool isEnter = !PlayerControl.LocalPlayer.inVent;

            if (__instance.name.Contains("Barghest_"))
            {
                __instance.SetButtons(isEnter && canMoveInVents);
                MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UseUncheckedVent, Hazel.SendOption.Reliable);
                writer.WritePacked(__instance.Id);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write(isEnter ? byte.MaxValue : (byte)0);
                writer.EndMessage();
                RPCProcedure.useUncheckedVent(__instance.Id, PlayerControl.LocalPlayer.PlayerId, isEnter ? byte.MaxValue : (byte)0);
                return false;
            }

            if (isEnter)
            {
                PlayerControl.LocalPlayer.MyPhysics.RpcEnterVent(__instance.Id);
            }
            else
            {
                PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(__instance.Id);
            }
            __instance.SetButtons(isEnter && canMoveInVents);
            return false;
        }
    }

    [HarmonyPatch(typeof(VentButton), nameof(VentButton.SetTarget))]
    class VentButtonSetTargetPatch
    {
        static Sprite defaultVentSprite = null;
        static void Postfix(VentButton __instance)
        {
            if (Barghest.Role != null && Barghest.Role == PlayerControl.LocalPlayer)
            {
                if (defaultVentSprite == null) defaultVentSprite = __instance.graphic.sprite;
                bool isSpecialVent = __instance.currentTarget != null && __instance.currentTarget.gameObject != null && __instance.currentTarget.gameObject.name.Contains("Barghest_");
                __instance.graphic.sprite = isSpecialVent ? ventmapIco : defaultVentSprite;
                __instance.buttonLabelText.enabled = !isSpecialVent;
            }
        }
    }

    

        
    
}