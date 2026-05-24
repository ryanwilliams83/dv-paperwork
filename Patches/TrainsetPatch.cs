using DV.Logic.Job;
using DvMod.Paperwork.Cache;
using HarmonyLib;

namespace DvMod.Paperwork.Patches
{
    [HarmonyPatch(typeof(Trainset))]
    public static class TrainsetPatchPatch
    {
        [HarmonyPatch(typeof(Trainset), nameof(Trainset.Merge))]
        [HarmonyPostfix]
        public static void Merge_Postfix()
        {
            TrainsetCache.Rebuild(JobsManager.Instance);

            var car = PlayerManager.Car;
            if (car != null)
                Paperwork.GiveBookletsForTrainset(car.trainset);
        }

        [HarmonyPatch(typeof(Trainset), nameof(Trainset.Split))]
        [HarmonyPostfix]
        public static void Split_Postfix()
        {
            TrainsetCache.Rebuild(JobsManager.Instance);

            var car = PlayerManager.Car;
            if (car != null)
                Paperwork.GiveBookletsForTrainset(car.trainset);
        }
    }
}
