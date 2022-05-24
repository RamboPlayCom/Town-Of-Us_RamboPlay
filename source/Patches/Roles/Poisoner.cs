using System;
using UnityEngine;
using HarmonyLib;

namespace TownOfUs.Roles
{
    public class Poisoner : Role

    {
        public KillButton _poisonButton;
        public PlayerControl ClosestPlayer;
        public DateTime LastPoisoned;
        public PlayerControl PoisonedPlayer;
        public float TimeRemaining;
        public bool Enabled = false;

        public Poisoner(PlayerControl player) : base(player)
        {
            Name = "绝命毒师";
            ImpostorText = () => "寄汤来啦~哈哈哈~";
            TaskText = () => "给其它船员下毒";
            Color = Palette.ImpostorRed;
            LastPoisoned = DateTime.UtcNow;
            RoleType = RoleEnum.Poisoner;
            AddToRoleHistory(RoleType);
            Faction = Faction.Impostors;
            PoisonedPlayer = null;
        }
        public KillButton PoisonButton
        {
            get => _poisonButton;
            set
            {
                _poisonButton = value;
                ExtraButtons.Clear();
                ExtraButtons.Add(value);
            }
        }
        public bool Poisoned => TimeRemaining > 0f;
        public void Poison()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;
            if (MeetingHud.Instance)
            {
                TimeRemaining = 0;
            }
            if (TimeRemaining <= 0)
            {
                PoisonKill();
            }
        }
        public void PoisonKill()
        {
            Utils.RpcMurderPlayer(Player, PoisonedPlayer);
            PoisonedPlayer = null;
            Enabled = false;
            LastPoisoned = DateTime.UtcNow;
            SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.KillSfx, false, 0.5f);
        }
        public float PoisonTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastPoisoned;
            var num = CustomGameOptions.PoisonCd * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}