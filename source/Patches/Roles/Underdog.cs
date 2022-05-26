using TownOfUs.ImpostorRoles.UnderdogMod;

namespace TownOfUs.Roles
{
    public class Underdog : Role
    {
        public Underdog(PlayerControl player) : base(player)
        {
            Name = "潜伏者";
            ImpostorText = () => "内鬼的王牌";
            TaskText = () => "存活的内鬼越少，冷却时间越短。";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Underdog;
            AddToRoleHistory(RoleType);
            Faction = Faction.Impostors;
        }

        public float MaxTimer() => PerformKill.LastImp() ? PlayerControl.GameOptions.KillCooldown - (CustomGameOptions.UnderdogKillBonus) : (PerformKill.IncreasedKC() ? PlayerControl.GameOptions.KillCooldown : PlayerControl.GameOptions.KillCooldown + (CustomGameOptions.UnderdogKillBonus));

        public void SetKillTimer()
        {
            Player.SetKillTimer(MaxTimer());
        }
    }
}
