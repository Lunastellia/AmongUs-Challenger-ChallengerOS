using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using System.Collections;
using System;
using System.Text;
using UnityEngine;
using System.Reflection;
using static ChallengerOS.Utils.Option.CustomOptionHolder;
using static ChallengerMod.Challenger;
using static ChallengerMod.Unity;
using static ChallengerMod.Roles;
using static ChallengerMod.ColorTable;
using static ChallengerMod.ResetData;
using ChallengerOS.Utils;
using ChallengerMod.RPC;

namespace ChallengerOS
{
    [HarmonyPatch]
    class MeetingHudPatch
    {
        private static GameData.PlayerInfo target = null;
        private const float scale = 0.65f;

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CheckForEndVoting))]
        class MeetingCalculateVotesPatch
        {
            private static Dictionary<byte, int> CalculateVotes(MeetingHud __instance)
            {
                Dictionary<byte, int> dictionary = new Dictionary<byte, int>();
                for (int i = 0; i < MeetingHud.Instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = MeetingHud.Instance.playerStates[i];
                    if (playerVoteArea.VotedFor != 252 && playerVoteArea.VotedFor != 255 && playerVoteArea.VotedFor != 254)
                    {
                        PlayerControl player = Helpers.playerById((byte)playerVoteArea.TargetPlayerId);
                        if (player == null || player.Data == null || player.Data.IsDead || player.Data.Disconnected) continue;

                        int currentVotes;
                        int additionalVotes = ((Mayor.Role != null && Mayor.Role.PlayerId == playerVoteArea.TargetPlayerId) ||
                            (Mayor.Role != null && CopyCat.Role != null && (CopyCat.copyRole == 8 && CopyCat.CopyStart == true) && Mayor.Role.Data.IsDead && CopyCat.Role.PlayerId == playerVoteArea.TargetPlayerId) ? 2 : 1); // Mayor vote
                        if (dictionary.TryGetValue(playerVoteArea.VotedFor, out currentVotes))
                            dictionary[playerVoteArea.VotedFor] = currentVotes + additionalVotes;
                        else
                            dictionary[playerVoteArea.VotedFor] = additionalVotes;



                    }
                }
                


                return dictionary;
            }



            static bool Prefix(MeetingHud __instance)
            {
                if (MeetingHud.Instance.playerStates.All((PlayerVoteArea ps) => ps.AmDead || ps.DidVote))
                {
                    if (Basilisk.Petrified != null && !Basilisk.Petrified.Data.IsDead)
                    {
                        Dictionary<byte, PlayerControl> playersById = ChallengerOS.Utils.Helpers.allPlayersById();

                        foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                        {
                            PlayerControl playerControl = playersById.ContainsKey((byte)player.TargetPlayerId) ? playersById[(byte)player.TargetPlayerId] : null;

                            if (playerControl != null && playerControl == Basilisk.Petrified)
                            {
                              player.VotedFor = 254;
                               
                            }
                        }
                    }

                    if ((Dictator.Role != null) || (CopyCat.Role != null && CopyCat.copyRole == 16 && CopyCat.CopyStart == true))
                    {
                        if (((!Dictator.Role.Data.IsDead) && Dictator.HMActive == true) || ((CopyCat.copyRole == 16 && CopyCat.CopyStart == true && !CopyCat.Role.Data.IsDead) && CopyCat.HMActive == true))


                        {
                            if (target == null)
                            {
                                foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
                                {

                                    

                                    if (playerVoteArea.VotedFor == 253)
                                    {
                                        playerVoteArea.VotedFor = playerVoteArea.TargetPlayerId;
                                    }
                                    if (playerVoteArea.VotedFor == 254)
                                    {
                                        playerVoteArea.VotedFor = playerVoteArea.TargetPlayerId;
                                    }
                            
                                }
                            }
                            else
                            { }
                        
                    
                
                        
                        

                        }
                    }
                    
                    
                    Dictionary<byte, int> self = CalculateVotes(__instance);
                    bool tie;
                    
                    KeyValuePair<byte, int> max = self.MaxPair(out tie);
                    GameData.PlayerInfo exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault(v => !tie && v.PlayerId == max.Key && !v.IsDead);

                    MeetingHud.VoterState[] array = new MeetingHud.VoterState[MeetingHud.Instance.playerStates.Length];
                    for (int i = 0; i < MeetingHud.Instance.playerStates.Length; i++)
                    {
                        PlayerVoteArea playerVoteArea = MeetingHud.Instance.playerStates[i];
                        array[i] = new MeetingHud.VoterState
                        {
                            VoterId = playerVoteArea.TargetPlayerId,
                            VotedForId = playerVoteArea.VotedFor
                        };
                    }

                    // RPCVotingComplete
                    MeetingHud.Instance.RpcVotingComplete(array, exiled, tie);
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.BloopAVoteIcon))]
        class MeetingHudBloopAVoteIconPatch
        {
            public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] GameData.PlayerInfo voterPlayer, [HarmonyArgument(1)] int index, [HarmonyArgument(2)] Transform parent)
            {
                SpriteRenderer spriteRenderer = UnityEngine.Object.Instantiate<SpriteRenderer>(__instance.PlayerVotePrefab);
                int cId = voterPlayer.DefaultOutfit.ColorId;
                if (!(!PlayerControl.GameOptions.AnonymousVotes 
                    || (PlayerControl.LocalPlayer.Data.IsDead)
                    || ((PlayerControl.LocalPlayer == Mentalist.Role) && ChallengerOS.Utils.Option.CustomOptionHolder.MentalistAbility.getSelection() != 1)
                    || ((PlayerControl.LocalPlayer == Assassin.Role) && Assassin.StealVoteColor == true)
                    || ((PlayerControl.LocalPlayer == CopyCat.Role) && (CopyCat.copyRole == 8 && CopyCat.CopyStart == true) && ChallengerOS.Utils.Option.CustomOptionHolder.MentalistAbility.getSelection() != 1))
                    )
                voterPlayer.Object.SetColor(6);
                voterPlayer.Object.SetPlayerMaterialColors(spriteRenderer);
                spriteRenderer.transform.SetParent(parent);
                spriteRenderer.transform.localScale = Vector3.zero;
                __instance.StartCoroutine(Effects.Bloop((float)index * 0.3f, spriteRenderer.transform, 1f, 0.5f));
                parent.GetComponent<VoteSpreader>().AddVote(spriteRenderer);
                voterPlayer.Object.SetColor(cId);
                return false;
            }
        }
        

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
        class MeetingHudPopulateVotesPatch
        {

            static bool Prefix(MeetingHud __instance, Il2CppStructArray<MeetingHud.VoterState> states)
            {

                PetrifyPosition = new Vector3(99f, 99f, 1f);

                MeetingHud.Instance.TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingVotingResults, new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                int num = 0;
                for (int i = 0; i < MeetingHud.Instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = MeetingHud.Instance.playerStates[i];
                    byte targetPlayerId = playerVoteArea.TargetPlayerId;
                    

                    playerVoteArea.ClearForResults();
                    int num2 = 0;
                    bool mayorFirstVoteDisplayed = false;
                    for (int j = 0; j < states.Length; j++)
                    {
                        MeetingHud.VoterState voterState = states[j];
                        GameData.PlayerInfo playerById = GameData.Instance.GetPlayerById(voterState.VoterId);
                        if (playerById == null)
                        {
                            Debug.LogError(string.Format("Couldn't find player info for voter: {0}", voterState.VoterId));
                        }
                        else if (i == 0 && voterState.SkippedVote && !playerById.IsDead)
                        {
                            MeetingHud.Instance.BloopAVoteIcon(playerById, num, MeetingHud.Instance.SkippedVoting.transform);
                            num++;
                        }
                        else if (voterState.VotedForId == targetPlayerId && !playerById.IsDead)
                        {
                            MeetingHud.Instance.BloopAVoteIcon(playerById, num2, playerVoteArea.transform);
                            num2++;
                        }

                        // Major vote, redo this iteration to place a second vote
                        if ((Mayor.Role != null && voterState.VoterId == (sbyte)Mayor.Role.PlayerId && !mayorFirstVoteDisplayed) 
                            || (Mayor.Role != null && Mayor.Role.Data.IsDead && CopyCat.Role != null && (CopyCat.copyRole == 8 && CopyCat.CopyStart == true) && voterState.VoterId == (sbyte)CopyCat.Role.PlayerId && !mayorFirstVoteDisplayed))
                        {
                            mayorFirstVoteDisplayed = true;
                            j--;
                        }
                        
                    }
                }
                return false;
            }
        }

        


        

        

        


        

        

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.StartMeeting))]
        class StartMeetingPatch
        {
            public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)] GameData.PlayerInfo meetingTarget)
            {
                if (meetingTarget == null) 
                // Save the meeting target
                target = meetingTarget;
                // Count meetings
                Camera.main.orthographicSize = 3f;
                CameraZoom = 3f;
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
        class MeetingHudUpdatePatch
        {

            static void Postfix(MeetingHud __instance)
            {
                ChallengerMod.Challenger.SafeTimer = 1f;
                


                if (GameObject.Find("PetrifySprite"))
                {
                    if (Basilisk.Role != null && Basilisk.Petrified != null && !Basilisk.Petrified.Data.IsDead && !Basilisk.Role.Data.IsDead)
                    {
                        var PetrifyNewposition = GameObject.Find("PetrifySprite");
                        PetrifyNewposition.transform.position = ChallengerMod.Challenger.PetrifyPosition;
                        PetrifyNewposition.layer = 5;
                    }
                    else
                    {
                        var PetrifyNewposition = GameObject.Find("PetrifySprite");
                        PetrifyNewposition.transform.position = ChallengerMod.Challenger.PetrifyPositionIni;
                        PetrifyNewposition.layer = 5;
                    }
                    
                }


                if (ChallengerMod.Challenger.PlayerOiled)
                {
                    HudManager.Instance.FullScreen.gameObject.SetActive(true);
                    HudManager.Instance.FullScreen.enabled = true;
                    HudManager.Instance.FullScreen.color = new Color(0.5f, 0.5f, 0f, 0.4f);
                }
                else
                {
                    HudManager.Instance.FullScreen.color = new Color(0f, 0f, 0f, 0f);
                }



               
                if (Basilisk.Role != null && Basilisk.Petrified != null && !Basilisk.Petrified.Data.IsDead && !Basilisk.Role.Data.IsDead)
                {
                    Dictionary<byte, PlayerControl> playersById = ChallengerOS.Utils.Helpers.allPlayersById();

                    foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                    {
                        PlayerControl playerControl = playersById.ContainsKey((byte)player.TargetPlayerId) ? playersById[(byte)player.TargetPlayerId] : null;

                        if (playerControl != null && playerControl == Basilisk.Petrified)
                        {
                            player.VotedFor = 254;
                            
                            player.Overlay.gameObject.SetActive(true);
                            player.SetVote(254);

                            if (Basilisk.PetrifyAndShield)
                            {
                                player.AmDead = true;
                                player.voteComplete = true;
                                player.Background.color.SetAlpha(100);
                                player.Background.color = Petrify2Color;
                            }
                            else
                            {
                                player.Background.color.SetAlpha(10);
                                player.Background.color = PetrifyColor;
                            }

                            Vector3 position = new Vector3(player.PlayerIcon.transform.position.x, player.PlayerIcon.transform.position.y + 0.115f, player.PlayerIcon.transform.position.z + -10f);
                            PetrifyPosition = position;

                            

                            
                            player.SetVote(254);
                        }
                    }

                }

                if (GameObject.Find("Main Camera").transform.FindChild("Hud").transform.FindChild("MeetingHub(Clone)").transform.FindChild("ChatUi").transform.FindChild("ChatScreen").transform.FindChild("TypingArea"))
                {
                    var ChatRead = GameObject.Find("Main Camera").transform.FindChild("Hud").transform.FindChild("MeetingHub(Clone)").transform.FindChild("ChatUi").transform.FindChild("ChatScreen").transform.FindChild("TypingArea");
                    
                    if (Basilisk.Role != null && Basilisk.Petrified != null && !Basilisk.Petrified.Data.IsDead && !Basilisk.Role.Data.IsDead)
                    {
                        if (PlayerControl.LocalPlayer == Basilisk.Petrified) { ChatRead.transform.localScale = new Vector3(0f, 0f, 0f); }
                        else { ChatRead.transform.localScale = new Vector3(1f, 1f, 1f); }
                    }
                    else { ChatRead.transform.localScale = new Vector3(1f, 1f, 1f); }
                }


                if ((Dictator.Role != null) || (CopyCat.Role != null && CopyCat.copyRole == 16 && CopyCat.CopyStart == true))
                {
                    if ((!Dictator.Role.Data.IsDead) || (CopyCat.copyRole == 16 && CopyCat.CopyStart == true && !CopyCat.Role.Data.IsDead))
                    {
                        if (target == null && (Dictator.HMActive == true || CopyCat.HMActive == true))
                        {
                            MeetingHud.Instance.SkipVoteButton.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}