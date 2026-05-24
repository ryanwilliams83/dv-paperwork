using DvMod.Paperwork.Cache;
using HarmonyLib;

namespace DvMod.Paperwork.Patches
{
    [HarmonyPatch(typeof(JobOverview))]
    public static class JobOverviewPatch
    {
        [HarmonyPatch(typeof(JobOverview), nameof(JobOverview.DestroyJobOverview))]
        [HarmonyPrefix]
        public static void DestroyJobOverview_Prefix(JobOverview __instance)
        {
            JobOverviewCache.JobsToJobOverviews.Remove(__instance.job);
        }

        [HarmonyPatch(typeof(JobOverview), nameof(JobOverview.Start))]
        [HarmonyPostfix]
        public static void Start_Postfix(JobOverview __instance)
        {
            JobOverviewCache.JobsToJobOverviews.Add(__instance.job, __instance);
        }
    }
}
