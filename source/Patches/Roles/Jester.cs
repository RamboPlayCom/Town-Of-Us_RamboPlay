using Il2CppSystem.Collections.Generic;
using TownOfUs.Extensions;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Jester : Role
    {
        public bool VotedOut;


        public Jester(PlayerControl player) : base(player)
        {
            Name = "小丑";
            ImpostorText = () => "开香槟の小曲";
            TaskText = () => "被投出去！\n伪任务:";
            Color = Patches.Colors.Jester;
            RoleType = RoleEnum.Jester;
            AddToRoleHistory(RoleType);
            Faction = Faction.Neutral;
        }

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__21 __instance)
        {
            var jestTeam = new List<PlayerControl>();
            jestTeam.Add(PlayerControl.LocalPlayer);
            __instance.teamToShow = jestTeam;
        }

        internal override bool EABBNOODFGL(ShipStatus __instance)
        {
            if (!VotedOut || !Player.Data.IsDead && !Player.Data.Disconnected) return true;
            Utils.EndGame();
            return false;
        }

        public void Wins()
        {
            //System.Console.WriteLine("Reached Here - Jester edition");
            VotedOut = true;
        }

        public void Loses()
        {
            LostByRPC = true;
        }
    }
}