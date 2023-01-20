using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using ChallengerMod.Patches;
using ChallengerOS.Versioncheck;
using static ChallengerMod.Challenger;
using static ChallengerMod.Unity;
using static ChallengerMod.Roles;
using static ChallengerMod.ColorTable;
using static ChallengerMod.ResetData;
using static ChallengerMod.WinData;
using static ChallengerMod.Set.Data;
using ChallengerMod.Item;
using Reactor.Extensions;
using ChallengerOS.Utils.Option;
using static ChallengerOS.Utils.Option.CustomOptionHolder;
using ChallengerOS.Utils;
using ChallengerOS.Objects;

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
            if (ChallengerMod.Challenger.ReadyPlayers.Contains(PlayerName))
            {

            }
            else
            {
                ChallengerMod.Challenger.ReadyPlayers.Add(PlayerName);
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
            catch (Exception e) { }
        }
        public static void uncheckedMurderPlayer(byte sourceId, byte targetId, byte showAnimation)
        {
            PlayerControl source = Helpers.playerById(sourceId);
            PlayerControl target = Helpers.playerById(targetId);
            if (source != null && target != null)
            {
                if (showAnimation == 0) KillAnimationCoPerformKillPatch.hideNextAnimation = true;
                source.MurderPlayer(target);
            }
        }
        public static void useUncheckedVent(int ventId, byte playerId, byte isEnter)
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
        //GUARDIAN
        public static void shieldedMurderAttempt()
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
        //GUESSER

        public static void guesserShoot(byte playerId)
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

        public static void guesserFail(byte playerId)
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
        public static void uncheckedCmdReportDeadBody(byte sourceId, byte targetId)
        {
            PlayerControl source = Helpers.playerById(sourceId);
            var t = targetId == Byte.MaxValue ? null : Helpers.playerById(targetId).Data;
            if (source != null) source.ReportDeadBody(t);
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
                
            }
        }
    }
}

