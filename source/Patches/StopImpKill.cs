using HarmonyLib;
using TownOfUs.CrewmateRoles.MedicMod;
using Hazel;
using TownOfUs.Extensions;
using TownOfUs.Roles;

namespace TownOfUs
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    public class StopImpKill
    {
        [HarmonyPriority(Priority.First)]
        public static bool Prefix(KillButton __instance)
        {
            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton) return true;
            if (!PlayerControl.LocalPlayer.Data.IsImpostor()) return true;
            var target = __instance.currentTarget;
            if (target == null) return true;
            if (!__instance.isActiveAndEnabled || __instance.isCoolingDown) return true;
            if (target.Is(RoleEnum.Pestilence))
            {
                Utils.RpcMurderPlayer(target, PlayerControl.LocalPlayer);
                return false;
            }
            if (target.IsInfected() || PlayerControl.LocalPlayer.IsInfected())
            {
                foreach (var pb in Role.GetRoles(RoleEnum.Plaguebearer)) ((Plaguebearer)pb).RpcSpreadInfection(target, PlayerControl.LocalPlayer);
            }
            if (target.IsOnAlert())
            {
                if (target.IsShielded())
                {
                    var medic = target.GetMedic().Player.PlayerId;
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.AttemptSound, SendOption.Reliable, -1);
                    writer.Write(medic);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    if (CustomGameOptions.ShieldBreaks) PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                    else PlayerControl.LocalPlayer.SetKillTimer(0.01f);

                    StopKill.BreakShield(medic, target.PlayerId,
                        CustomGameOptions.ShieldBreaks);
                    if (!PlayerControl.LocalPlayer.IsProtected())
                    {
                        Utils.RpcMurderPlayer(target, PlayerControl.LocalPlayer);
                    }
                }
                else if (PlayerControl.LocalPlayer.IsShielded())
                {
                    var medic = PlayerControl.LocalPlayer.GetMedic().Player.PlayerId;
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.AttemptSound, SendOption.Reliable, -1);
                    writer.Write(medic);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    if (CustomGameOptions.ShieldBreaks) PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                    else PlayerControl.LocalPlayer.SetKillTimer(0.01f);

                    StopKill.BreakShield(medic, PlayerControl.LocalPlayer.PlayerId, CustomGameOptions.ShieldBreaks);
                    if (CustomGameOptions.KilledOnAlert && !target.IsProtected())
                    {
                        Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, target);
                        PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                    }
                }
                else
                {
                    if (!PlayerControl.LocalPlayer.IsProtected())
                    {
                        Utils.RpcMurderPlayer(target, PlayerControl.LocalPlayer);
                    }
                    if (CustomGameOptions.KilledOnAlert && !target.IsProtected())
                    {
                        Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, target);
                        PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                    }
                    else
                    {
                        PlayerControl.LocalPlayer.SetKillTimer(CustomGameOptions.ProtectKCReset + 0.01f);
                    }
                }
                return false;
            }
            else if (target.IsShielded())
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.AttemptSound, SendOption.Reliable, -1);
                writer.Write(target.GetMedic().Player.PlayerId);
                writer.Write(target.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

                System.Console.WriteLine(CustomGameOptions.ShieldBreaks + "- 护盾破碎");
                if (CustomGameOptions.ShieldBreaks) PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                else PlayerControl.LocalPlayer.SetKillTimer(0.01f);
                StopKill.BreakShield(target.GetMedic().Player.PlayerId, target.PlayerId, CustomGameOptions.ShieldBreaks);
                return false;
            }
            else if (target.IsVesting())
            {
                PlayerControl.LocalPlayer.SetKillTimer(CustomGameOptions.VestKCReset + 0.01f);
                return false;
            }
            else if (target.IsProtected())
            {
                PlayerControl.LocalPlayer.SetKillTimer(CustomGameOptions.ProtectKCReset + 0.01f);
                return false;
            }
            Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, target);
            PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
            return false;
        }
    }
}