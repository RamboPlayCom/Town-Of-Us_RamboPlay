using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    //[HarmonyPriority(Priority.VeryHigh)] // to show this message first, or be overrided if any plugins do
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class PingTracker_Update
    {

        [HarmonyPostfix]
        public static void Postfix(PingTracker __instance)
        {
            var position = __instance.GetComponent<AspectPosition>();
            position.DistanceFromEdge = new Vector3(3.6f, 0.1f, 0);
            position.AdjustPosition();

            __instance.text.text =
                "<color=#00FF00FF>我们的小镇 v" + TownOfUs.VersionString + "</color>\n" +
                $"延迟: {AmongUsClient.Instance.Ping}毫秒\n" +
                (!MeetingHud.Instance
                    ? "<color=#1a75ff>本地帽子</color>" : "") +
                (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started
                    ? "<color=#1a75ff>兰博玩对战（内测）</color>" : "");
        }
    }
}
