using HarmonyLib;
using Hazel;
using static ChallengerMod.Unity;
using ChallengerOS.RPC;
using InnerNet;
using UnityEngine;

namespace ChallengerOS
{

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class HudUpdateManager
    {
        static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started)
            {
               
                
                
                

                if (ChallengerMod.Challenger.IsrankedGame)
                {
                    if (!AmongUsClient.Instance.AmHost)
                    {
                        if (GameObject.Find("Main Camera/Hud/Buttons/BottomRight/ReadyButton"))
                        {
                            var Readybutton = GameObject.Find("Main Camera/Hud/Buttons/BottomRight/ReadyButton");
                            if (Readybutton != null)
                            {
                                if (ChallengerMod.Challenger.ReadyPlayers.Contains(PlayerControl.LocalPlayer.Data.PlayerName))
                                {
                                    Readybutton.transform.localScale = new Vector3(0f, 0f, 0f);
                                }
                                else
                                {
                                    Readybutton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (AmongUsClient.Instance.AmHost)
                    {
                        if (GameObject.Find("Main Camera/Hud/Buttons/BottomRight/ReadyButton"))
                        {
                            var Readybutton = GameObject.Find("Main Camera/Hud/Buttons/BottomRight/ReadyButton");
                            if (Readybutton != null)

                                Readybutton.transform.localScale = new Vector3(0f, 0f, 0f);

                        }
                    }


                }

                if (ChallengerMod.Challenger.IsrankedGame)
                {
                    if (AmongUsClient.Instance.AmHost)
                    {
                        if (!ChallengerMod.Challenger.ReadyPlayers.Contains(PlayerControl.LocalPlayer.name))
                        {
                            var PlayerName = PlayerControl.LocalPlayer.name;
                            SoundManager.Instance.PlaySound(Used, false, 100f);
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareReady, Hazel.SendOption.Reliable, -1);
                            writer.Write(PlayerName);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.shareReady(PlayerName);
                        }
                    }
                }
            }
        }
    }
}