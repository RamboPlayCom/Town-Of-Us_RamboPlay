using System.Linq;

namespace TownOfUs.Roles
{
    public class Traitor : Role
    {

        public Traitor(PlayerControl player) : base(player)
        {
            Name = "背叛者";
            ImpostorText = () => "";
            TaskText = () => "你背叛了好人们！";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Traitor;
            AddToRoleHistory(RoleType);
            Faction = Faction.Impostors;
        }
    }
}