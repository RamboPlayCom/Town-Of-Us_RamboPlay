using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace TownOfUs.Roles
{
    public class Amnesiac : Role
    {
        public Dictionary<byte, ArrowBehaviour> BodyArrows = new Dictionary<byte, ArrowBehaviour>();

        public Amnesiac(PlayerControl player) : base(player)
        {
            Name = "失忆者";
            ImpostorText = () => "窃取死者的职业";
            TaskText = () => "窃取尸体的职业\n假任务：";
            Color = Patches.Colors.Amnesiac;
            RoleType = RoleEnum.Amnesiac;
            AddToRoleHistory(RoleType);
            Faction = Faction.Neutral;
        }

        public DeadBody CurrentTarget;

        public void Loses()
        {
            LostByRPC = true;
        }

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__21 __instance)
        {
            var amnesiacTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            amnesiacTeam.Add(PlayerControl.LocalPlayer);
            __instance.teamToShow = amnesiacTeam;
        }

        public void DestroyArrow(byte targetPlayerId)
        {
            var arrow = BodyArrows.FirstOrDefault(x => x.Key == targetPlayerId);
            if (arrow.Value != null)
                Object.Destroy(arrow.Value);
            if (arrow.Value.gameObject != null)
                Object.Destroy(arrow.Value.gameObject);
            BodyArrows.Remove(arrow.Key);
        }
    }
}