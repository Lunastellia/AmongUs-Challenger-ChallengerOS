﻿using HarmonyLib;
using Hazel;
using System.Linq;
using UnityEngine;
using System;
using ChallengerOS.Versioncheck;
using static ChallengerMod.Challenger;
using static ChallengerMod.Unity;
using static ChallengerMod.Roles;
using ChallengerOS.Utils.Option;
using static ChallengerOS.Utils.Option.CustomOptionHolder;
using ChallengerOS.Utils;
using static UnityEngine.GraphicsBuffer;

namespace ChallengerOS.RPC
{




    enum CustomRPC
    {
        // Main Controls
        VersionHandshake = 60,
        ShareReady,
        ShareOptions,
        UncheckedMurderPlayer,
        ShieldedMurderAttempt,
        UseUncheckedVent,
        GuesserShoot,
        GuesserFail,
        UncheckedCmdReportDeadBody,
        SetLover1,
        SetLover2,
        CultistDieMeeting,
        SetAllVoteTarget,


    }
    public static class RPCProcedure
    {





        public static void versionHandshake(int major, int minor, int build, int revision, Guid guid, int clientId)
        {
            System.Version ver;
            if (revision < 0)
                ver = new System.Version(major, minor, build);
            else
                ver = new System.Version(major, minor, build, revision);
            GameStartManagerPatch.playerVersions[clientId] = new GameStartManagerPatch.PlayerVersion(ver, guid);
        }
        public static void shareReady(string PlayerName)
        {
            try
            {
                if (ChallengerMod.Challenger.ReadyPlayers.Contains(PlayerName))
                {

                }
                else
                {
                    ChallengerMod.Challenger.ReadyPlayers.Add(PlayerName);
                }
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] shareReady " + e.Message);
                return;
            }

        }


        public static void ShareOptions(int numberOfOptions, MessageReader reader)
        {
            try
            {
                for (int i = 0; i < numberOfOptions; i++)
                {
                    uint optionId = reader.ReadPackedUInt32();
                    uint selection = reader.ReadPackedUInt32();
                    CustomOption option = CustomOption.options.FirstOrDefault(option => option.id == (int)optionId);
                    option.updateSelection((int)selection);
                }
            }
            catch (Exception e) 
            {
            }
        }
        public static void uncheckedMurderPlayer(byte sourceId, byte targetId, byte showAnimation)
        {
            try
            {
                PlayerControl source = Helpers.playerById(sourceId);
                PlayerControl target = Helpers.playerById(targetId);
                if (source != null && target != null)
                {
                    if (showAnimation == 0) KillAnimationCoPerformKillPatch.hideNextAnimation = true;
                    source.MurderPlayer(target);
                }
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] uncheckedMurderPlayer " + e.Message);
                return;
            }

        }
        public static void useUncheckedVent(int ventId, byte playerId, byte isEnter)
        {
            try
            {
                PlayerControl player = Helpers.playerById(playerId);
                if (player == null) return;
                // Fill dummy MessageReader and call MyPhysics.HandleRpc as the corountines cannot be accessed
                MessageReader reader = new MessageReader();
                byte[] bytes = BitConverter.GetBytes(ventId);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                reader.Buffer = bytes;
                reader.Length = bytes.Length;

                //Hats.startAnimation(ventId);
                player.MyPhysics.HandleRpc(isEnter != 0 ? (byte)19 : (byte)20, reader);

            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] useUncheckedVent " + e.Message);
                return;
            }

        }
        //GUARDIAN
        public static void shieldedMurderAttempt()
        {
            try
            {
                if (Guardian.Protected != null && Guardian.Protected == PlayerControl.LocalPlayer && GuardianShieldSound.getBool() == true)
                {
                    SoundManager.Instance.PlaySound(shieldClip, false, 100f);
                    Guardian.TryKill = true;
                }
                else if (Guardian.ProtectedMystic != null && Guardian.ProtectedMystic == PlayerControl.LocalPlayer && GuardianShieldSound.getBool() == true)
                {
                    SoundManager.Instance.PlaySound(shieldClip, false, 100f);
                    Guardian.TryKill = true;
                }
                else
                {
                    Guardian.TryKill = true;
                }
                if (Guardian.Protected != null)
                {
                    GLMod.GLMod.currentGame.addAction(Guardian.Protected.Data.PlayerName, "", "kill_resist_by_shield");
                }
                if (Guardian.ProtectedMystic != null)
                {
                    GLMod.GLMod.currentGame.addAction(Guardian.ProtectedMystic.Data.PlayerName, "", "kill_resist_by_supershield");
                }
                Guardian.TryKill = true;
                return;
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] shieldedMurderAttempt " + e.Message);
                return;
            }

        }
        //GUESSER

        public static void guesserShoot(byte playerId)
        {
            try
            {
                PlayerControl target = Helpers.playerById(playerId);
                if (target == null)
                {
                    return;
                }
                if (target == Guesser.Role)
                {
                    GLMod.GLMod.currentGame.addAction(Guesser.Role.Data.PlayerName, "", "guess_suicide");
                }
                else
                {
                    GLMod.GLMod.currentGame.addAction(Guesser.Role.Data.PlayerName, target.Data.PlayerName, "guess_kill");
                }
                target.Exiled();
                PlayerControl partner = target.getPartner(); // Lover check
                byte partnerId = partner != null ? partner.PlayerId : playerId;
                Guesser.remainingShots = Mathf.Max(0, Guesser.remainingShots - 1);
                //Guesser.remainingShots -= 1;
                //if (Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(target.KillSfx, false, 0.8f);
                //SoundManager.Instance.PlaySound(target.KillSfx, false, 0.8f);
                if (MeetingHud.Instance)
                {
                    foreach (PlayerVoteArea pva in MeetingHud.Instance.playerStates)
                    {
                        if (pva.TargetPlayerId == playerId || pva.TargetPlayerId == partnerId)
                        {
                            pva.SetDead(pva.DidReport, true);
                            pva.Overlay.gameObject.SetActive(true);
                        }
                    }
                    if (AmongUsClient.Instance.AmHost)
                        MeetingHud.Instance.CheckForEndVoting();
                }
                if (HudManager.Instance != null && Guesser.Role != null)
                    if (PlayerControl.LocalPlayer == target)
                        HudManager.Instance.KillOverlay.ShowKillAnimation(Guesser.Role.Data, target.Data);
                    else if (partner != null && PlayerControl.LocalPlayer == partner && Loverdie.getBool() == true)
                        HudManager.Instance.KillOverlay.ShowKillAnimation(partner.Data, partner.Data);
                    else
                        SoundManager.Instance.PlaySound(SoulTake, false, 100f);
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] guesserShoot " + e.Message);
                return;
            }

        }

        public static void guesserFail(byte playerId)
        {
            try
            {
                PlayerControl target = Helpers.playerById(playerId);
                if (target == null)
                {
                    return;
                }
                GLMod.GLMod.currentGame.addAction(Guesser.Role.Data.PlayerName, target.Data.PlayerName, "guess_try");
                //target.Exiled();
                PlayerControl partner = target.getPartner(); // Lover check
                byte partnerId = partner != null ? partner.PlayerId : playerId;
                Guesser.remainingShots = Mathf.Max(0, Guesser.remainingShots - 1);
                // ChallengerMod.Set.HudManagerPatch.GuessValue -= 1;
                //if (Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(target.KillSfx, false, 0.8f);
                //SoundManager.Instance.PlaySound(target.KillSfx, false, 0.8f);
                if (MeetingHud.Instance)
                {
                    foreach (PlayerVoteArea pva in MeetingHud.Instance.playerStates)
                    {
                        if (pva.TargetPlayerId == playerId || pva.TargetPlayerId == partnerId)
                        {
                            //pva.SetDead(pva.DidReport, true);
                            //pva.Overlay.gameObject.SetActive(true);
                        }
                    }
                    //if (AmongUsClient.Instance.AmHost)
                    // MeetingHud.Instance.CheckForEndVoting();
                }
                if (HudManager.Instance != null && Guesser.Role != null)

                    SoundManager.Instance.PlaySound(SoulTake, false, 100f);
                GuesserNotDie = false;
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] guesserFail " + e.Message);
                return;
            }

        }
        public static void uncheckedCmdReportDeadBody(byte sourceId, byte targetId)
        {
            try
            {
                PlayerControl source = Helpers.playerById(sourceId);
                var t = targetId == Byte.MaxValue ? null : Helpers.playerById(targetId).Data;
                if (source != null) source.ReportDeadBody(t);
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] uncheckedCmdReportDeadBody " + e.Message);
                return;
            }

        }
        public static void setLover1(byte loved1Id)
        {
            try
            {
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    if (player.PlayerId == loved1Id)
                    {
                        Cupid.Lover1 = player;
                        GLMod.GLMod.currentGame.addAction(Cupid.Role.Data.PlayerName, player.Data.PlayerName, "make_love");
                    }
                }

                Cupid.Love1Used = true;
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] setLover1 " + e.Message);
                return;
            }

        }
        public static void setLover2(byte loved2Id)
        {
            try
            {
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    if (player.PlayerId == loved2Id)
                    {
                        Cupid.Lover2 = player;
                        GLMod.GLMod.currentGame.addAction(Cupid.Role.Data.PlayerName, player.Data.PlayerName, "make_love");

                    }

                }
                Cupid.Love2Used = true;
                Cupid.Love1Used = true;
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] setLover2 " + e.Message);
                return;
            }

        }
        public static void cultistDieMeeting()
        {
            try
            {
                if (MeetingHud.Instance)
                {
                    foreach (PlayerVoteArea pva in MeetingHud.Instance.playerStates)
                    {
                        if (pva.TargetPlayerId == Cultist.Role.PlayerId)
                        {
                            pva.SetDead(pva.DidReport, true);
                            pva.Overlay.gameObject.SetActive(true);
                        }
                    }
                    if (AmongUsClient.Instance.AmHost)
                        MeetingHud.Instance.CheckForEndVoting();
                    if (PlayerControl.LocalPlayer == Cultist.Role)
                        HudManager.Instance.KillOverlay.ShowKillAnimation(Cultist.Role.Data, Cultist.Role.Data);

                    Cultist.Role.Data.IsDead = true;
                    Cultist.Suicide = true;
                    GLMod.GLMod.currentGame.addAction(Cultist.Role.Data.PlayerName, "", "cultist_die");
                }
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] cultistDieMeeting " + e.Message);
                return;
            }

        }
        public static void setAllVoteTarget(byte votedId)
        {
            try
            {
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    if (Dictator.Role != null && !Dictator.Role.Data.IsDead)
                    {
                        if (player.PlayerId == votedId)
                        {
                            if (DictatorForcedVote.getBool() == true && (CultePlayers.Count() <= 0 || CultePlayers.Count() > 0 && !CultePlayers.Contains(Dictator.Role)))
                            {
                                if ((player.Data.Role.IsImpostor)
                                    || (Jester.Role != null && player == Jester.Role)
                                    || (Eater.Role != null && player == Eater.Role)
                                    || (Arsonist.Role != null && player == Arsonist.Role)
                                    || (Outlaw.Role != null && player == Outlaw.Role)
                                    || (Cursed.Role != null && player == Cursed.Role)
                                    || (Cultist.Role != null && player == Cultist.Role)
                                    || (Cultist.Culte1 != null && player == Cultist.Culte1)
                                    || (Cultist.Culte2 != null && player == Cultist.Culte2)
                                    || (Cultist.Culte3 != null && player == Cultist.Culte3))
                                {
                                    Dictator.VotedFor = player;
                                    GLMod.GLMod.currentGame.addAction(Dictator.Role.Data.PlayerName, player.Data.PlayerName, "allvotefor");
                                    Dictator.SuperVote = true;
                                }
                                else
                                {
                                    Dictator.VotedFor = Dictator.Role;
                                    GLMod.GLMod.currentGame.addAction(Dictator.Role.Data.PlayerName, player.Data.PlayerName, "allvoteforFail");
                                    Dictator.SuperVote = true;
                                }
                            }
                            else
                            {
                                Dictator.VotedFor = player;
                                GLMod.GLMod.currentGame.addAction(Dictator.Role.Data.PlayerName, player.Data.PlayerName, "allvotefor");
                                Dictator.SuperVote = true;
                            }
                        }
                    }
                    if (Dictator.Role != null && CopyCat.Role != null && Dictator.Role.Data.IsDead)
                    {
                        if (player.PlayerId == votedId)
                        {
                            if (DictatorForcedVote.getBool() == true && (CultePlayers.Count() <= 0 || CultePlayers.Count() > 0 && !CultePlayers.Contains(CopyCat.Role)))
                            {
                                if ((player.Data.Role.IsImpostor)
                                    || (Jester.Role != null && player == Jester.Role)
                                    || (Eater.Role != null && player == Eater.Role)
                                    || (Arsonist.Role != null && player == Arsonist.Role)
                                    || (Outlaw.Role != null && player == Outlaw.Role)
                                    || (Cursed.Role != null && player == Cursed.Role)
                                    || (Cultist.Role != null && player == Cultist.Role)
                                    || (Cultist.Culte1 != null && player == Cultist.Culte1)
                                    || (Cultist.Culte2 != null && player == Cultist.Culte2)
                                    || (Cultist.Culte3 != null && player == Cultist.Culte3))
                                {
                                    Dictator.VotedFor = player;
                                    GLMod.GLMod.currentGame.addAction(CopyCat.Role.Data.PlayerName, player.Data.PlayerName, "allvotefor");
                                    CopyCat.SuperVote = true;
                                }
                                else
                                {
                                    Dictator.VotedFor = CopyCat.Role;
                                    GLMod.GLMod.currentGame.addAction(CopyCat.Role.Data.PlayerName, player.Data.PlayerName, "allvoteforFail");
                                    CopyCat.SuperVote = true;
                                }
                            }
                            else
                            {
                                Dictator.VotedFor = player;
                                GLMod.GLMod.currentGame.addAction(CopyCat.Role.Data.PlayerName, player.Data.PlayerName, "allvotefor");
                                CopyCat.SuperVote = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GLMod.GLMod.logError("[RPC] setAllVoteTarget " + e.Message);
                return;
            }

        }
    }


    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class HandleRpcPatch
    {
        static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            byte packetId = callId;
            switch (packetId)
            {

                case (byte)CustomRPC.VersionHandshake:
                    byte major = reader.ReadByte();
                    byte minor = reader.ReadByte();
                    byte patch = reader.ReadByte();
                    int versionOwnerId = reader.ReadPackedInt32();
                    byte revision = 0xFF;
                    Guid guid;
                    if (reader.Length - reader.Position >= 17)
                    { // enough bytes left to read
                        revision = reader.ReadByte();
                        // GUID
                        byte[] gbytes = reader.ReadBytes(16);
                        guid = new Guid(gbytes);
                    }
                    else
                    {
                        guid = new Guid(new byte[16]);
                    }
                    RPCProcedure.versionHandshake(major, minor, patch, revision == 0xFF ? -1 : revision, guid, versionOwnerId);
                    break;
                case (byte)CustomRPC.ShareOptions:
                    RPCProcedure.ShareOptions((int)reader.ReadPackedUInt32(), reader);
                    break;
                case (byte)CustomRPC.ShareReady:
                    {
                        RPCProcedure.shareReady(reader.ReadString());
                        break;
                    }
                
                case (byte)CustomRPC.UncheckedMurderPlayer:
                    byte source = reader.ReadByte();
                    byte target = reader.ReadByte();
                    byte showAnimation = reader.ReadByte();
                    RPCProcedure.uncheckedMurderPlayer(source, target, showAnimation);
                    break;
                case (byte)CustomRPC.UseUncheckedVent:
                    int ventId = reader.ReadPackedInt32();
                    byte ventingPlayer = reader.ReadByte();
                    byte isEnter = reader.ReadByte();
                    RPCProcedure.useUncheckedVent(ventId, ventingPlayer, isEnter);
                    break;
                case (byte)CustomRPC.ShieldedMurderAttempt:
                    RPCProcedure.shieldedMurderAttempt();
                    break;
                case (byte)CustomRPC.GuesserShoot:
                    {
                        RPCProcedure.guesserShoot(reader.ReadByte());
                        break;
                    }
                case (byte)CustomRPC.GuesserFail:
                    {
                        RPCProcedure.guesserFail(reader.ReadByte());
                        break;
                    }
                case (byte)CustomRPC.UncheckedCmdReportDeadBody:
                    byte reportSource = reader.ReadByte();
                    byte reportTarget = reader.ReadByte();
                    RPCProcedure.uncheckedCmdReportDeadBody(reportSource, reportTarget);
                    break;
                //CUPID
                case (byte)CustomRPC.SetLover1:
                    {
                        RPCProcedure.setLover1(reader.ReadByte());
                        break;
                    }
                case (byte)CustomRPC.SetLover2:
                    {
                        RPCProcedure.setLover2(reader.ReadByte());
                        break;
                    }
                case (byte)CustomRPC.CultistDieMeeting:
                    {
                        RPCProcedure.cultistDieMeeting();
                        break;
                    }
                case (byte)CustomRPC.SetAllVoteTarget:
                    {
                        RPCProcedure.setAllVoteTarget(reader.ReadByte());
                        break;
                    }
            }
        }
    }
}

