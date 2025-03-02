using System;
using UnityEngine;
using TMPro;
using TownOfUs.Extensions;

namespace TownOfUs.Roles
{
    public class GuardianAngel : Role
    {
        public bool Enabled;
        public DateTime LastProtected;
        public float TimeRemaining;

        public int UsesLeft;
        public TextMeshPro UsesText;

        public bool ButtonUsable => UsesLeft != 0;

        public PlayerControl target;

        public GuardianAngel(PlayerControl player) : base(player)
        {
            Name = "守护天使";
            ImpostorText = () =>
                target == null ? "不知怎的，你没有需要保护的目标……" : $"用你的生命守护 {target.name}！";
            TaskText = () =>
                target == null
                    ? "不知怎的，你没有需要保护的目标……"
                    : $"守护 {target.name}!";
            Color = Patches.Colors.GuardianAngel;
            LastProtected = DateTime.UtcNow;
            RoleType = RoleEnum.GuardianAngel;
            AddToRoleHistory(RoleType);
            Faction = Faction.Neutral;
            Scale = 1.4f;

            UsesLeft = CustomGameOptions.MaxProtects;
        }

        public bool Protecting => TimeRemaining > 0f;

        public float ProtectTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastProtected;
            var num = CustomGameOptions.ProtectCd * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }

        public void Protect()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;
        }


        public void UnProtect()
        {
            Enabled = false;
            LastProtected = DateTime.UtcNow;
        }

        public void ImpTargetWin()
        {
            Player.Data.Role.TeamType = RoleTeamTypes.Impostor;
            RoleManager.Instance.SetRole(Player, RoleTypes.Impostor);
        }

        public void ImpTargetLose()
        {
            LostByRPC = true;
        }

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__21 __instance)
        {
            var gaTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            gaTeam.Add(PlayerControl.LocalPlayer);
            gaTeam.Add(target);
            __instance.teamToShow = gaTeam;
        }
    }
}