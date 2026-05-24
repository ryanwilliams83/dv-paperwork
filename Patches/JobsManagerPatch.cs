using DV.Logic.Job;
using DvMod.Paperwork.Cache;
using HarmonyLib;
namespace DvMod.Paperwork.Patches
{
    [HarmonyPatch(typeof(JobsManager))]
    public static class JobsManagerPatch
    {
        [HarmonyPatch(typeof(JobsManager), nameof(JobsManager.AbandonJob))]
        [HarmonyPostfix]
        public static void AbandonJob_Postfix(JobsManager __instance)
        {
            TrainsetCache.Rebuild(__instance);
        }

        [HarmonyPatch(typeof(JobsManager), nameof(JobsManager.CompleteTheJob))]
        [HarmonyPostfix]
        public static void CompleteTheJob_Postfix(JobsManager __instance)
        {
            TrainsetCache.Rebuild(__instance);
        }

        [HarmonyPatch(typeof(JobsManager), nameof(JobsManager.RegisterGeneratedJob))]
        [HarmonyPostfix]
        public static void RegisterGeneratedJob_Postfix(JobsManager __instance)
        {
            TrainsetCache.Rebuild(__instance);
        }

        [HarmonyPatch(typeof(JobsManager), nameof(JobsManager.UnregisterJob))]
        [HarmonyPostfix]
        public static void UnregisterJob_Postfix(JobsManager __instance)
        {
            TrainsetCache.Rebuild(__instance);
        }
    }
}
