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
            if (!TrainsetCache.Enabled)
                return;

            Paperwork.LogTrace($"{nameof(JobOverviewPatch)}.{nameof(AbandonJob_Postfix)}()");

            TrainsetCache.Rebuild(__instance);
        }

        [HarmonyPatch(typeof(JobsManager), nameof(JobsManager.CompleteTheJob))]
        [HarmonyPostfix]
        public static void CompleteTheJob_Postfix(JobsManager __instance)
        {
            if (!TrainsetCache.Enabled)
                return;

            Paperwork.LogTrace($"{nameof(JobOverviewPatch)}.{nameof(CompleteTheJob_Postfix)}()");

            TrainsetCache.Rebuild(__instance);
        }

        [HarmonyPatch(typeof(JobsManager), nameof(JobsManager.RegisterGeneratedJob))]
        [HarmonyPostfix]
        public static void RegisterGeneratedJob_Postfix(JobsManager __instance)
        {
            if (!TrainsetCache.Enabled)
                return;

            Paperwork.LogTrace($"{nameof(JobOverviewPatch)}.{nameof(RegisterGeneratedJob_Postfix)}()");

            TrainsetCache.Rebuild(__instance);
        }

        [HarmonyPatch(typeof(JobsManager), nameof(JobsManager.UnregisterJob))]
        [HarmonyPostfix]
        public static void UnregisterJob_Postfix(JobsManager __instance)
        {
            if (!TrainsetCache.Enabled)
                return;

            Paperwork.LogTrace($"{nameof(JobOverviewPatch)}.{nameof(UnregisterJob_Postfix)}()");

            TrainsetCache.Rebuild(__instance);
        }
    }
}
