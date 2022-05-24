using UnityEngine;

namespace TownOfUs.Roles
{
    public class Engineer : Role
    {
        public Engineer(PlayerControl player) : base(player)
        {
            Name = "工程师";
            ImpostorText = () => "啊哈！修Maker~";
            TaskText = () => "Vent and fix a sabotage from anywhere!";
            Color = Patches.Colors.Engineer;
            RoleType = RoleEnum.Engineer;
            AddToRoleHistory(RoleType);
        }

        public bool UsedThisRound { get; set; } = false;
    }
}