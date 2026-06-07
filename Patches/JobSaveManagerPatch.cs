using DV.Logic.Job;
using DvMod.Paperwork.Cache;
using HarmonyLib;
using System.Linq;

namespace DvMod.Paperwork.Patches
{
    [HarmonyPatch(typeof(JobSaveManager))]
    public static class JobSaveManagerPatch
    {
        [HarmonyPatch(typeof(JobSaveManager), nameof(JobSaveManager.LoadJobSaveGameData))]
        [HarmonyPostfix]
        public static void LoadJobSaveGameData_Postfix()
        {
            Paperwork.LogTrace($"{nameof(JobSaveManager)}.{nameof(LoadJobSaveGameData_Postfix)}()");

            TrainsetCache.Enabled = true;
            TrainsetCache.Rebuild(JobsManager.Instance);

            foreach (var job in JobsManager.Instance.currentJobs.Where(Paperwork.IsReadyToSubmit))
                Paperwork.CheckJobBooklet(job, false);
        }
    }
}
