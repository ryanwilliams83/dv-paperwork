using DV.InventorySystem;
using DV.RenderTextureSystem.ItemIconRendering;
using DV.UI.Inventory;
using DvMod.Paperwork.Cache;
using HarmonyLib;

namespace DvMod.Paperwork.Patches
{
    [HarmonyPatch(typeof(InventoryUIController))]
    public static class InventoryManagerPatch
    {
        //[HarmonyPatch(typeof(InventoryUIController), nameof(InventoryUIController.))]
        //[HarmonyPrefix]
        //public static void DestroyJobOverview_Prefix(JobOverview __instance)
        //{
        //    JobOverviewCache.JobsToJobOverviews.Remove(__instance.job);
        //}

        //[HarmonyPatch(typeof(JobOverview), nameof(JobOverview.Start))]
        //[HarmonyPostfix]
        //public static void Start_Postfix(JobOverview __instance)
        //{
        //    JobOverviewCache.JobsToJobOverviews.Add(__instance.job, __instance);
        //}
    }
}
