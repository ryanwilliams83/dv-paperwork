using DvMod.Paperwork.Cache;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DvMod.Paperwork.Patches
{
    [HarmonyPatch(typeof(TrainCar))]
    public static class TrainCarPatch
    {
        private static readonly Dictionary<TrainCar, Action<(float value, bool forced)>> handbrakeChangedHandlers =
            new Dictionary<TrainCar, Action<(float value, bool forced)>>();

        [HarmonyPatch(typeof(TrainCar), nameof(TrainCar.Awake))]
        [HarmonyPostfix]
        public static void Awake_Postfix(TrainCar __instance)
        {
            // Paperwork.LogTrace($"{nameof(TrainCarPatch)}.{nameof(Awake_Postfix)}()");

            if (__instance.brakeSystem == null || !__instance.brakeSystem.hasHandbrake)
                return;

            if (__instance.IsLoco)
                return;

            if (handbrakeChangedHandlers.ContainsKey(__instance))
                return;

            Action<(float value, bool forced)> handler = args =>
            {
                OnHandbrakePositionChanged(__instance, args.value, args.forced);
            };
            handbrakeChangedHandlers[__instance] = handler;

            __instance.brakeSystem.HandbrakePositionChanged += handler;
        }

        [HarmonyPatch(typeof(TrainCar), nameof(TrainCar.OnDestroy))]
        [HarmonyPrefix]
        public static void OnDestroy_Prefix(TrainCar __instance)
        {
            Paperwork.LogTrace($"{nameof(TrainCarPatch)}.{nameof(OnDestroy_Prefix)}()");

            if (handbrakeChangedHandlers.TryGetValue(__instance, out var handler))
            {
                __instance.brakeSystem.HandbrakePositionChanged -= handler;
                handbrakeChangedHandlers.Remove(__instance);
            }
        }

        private static void OnHandbrakePositionChanged(TrainCar car, float value, bool forced)
        {
            if (!TrainsetCache.Enabled)
                return;

            Paperwork.LogTrace($"{nameof(TrainCarPatch)}.{nameof(OnHandbrakePositionChanged)}()");

            // Debug.Log($"Handbrake changed on '{car.logicCar.ID}', value={value}, forced={forced}");
            if (value < 1f)
                return;

            if (!TrainsetCache.TrainsetToJobs.TryGetValue(car.trainset, out var trainsetJobs))
                return;

            var completedJobs = trainsetJobs.Where(Paperwork.IsReadyToSubmit);
            foreach (var completedJob in completedJobs)
            {
                Paperwork.CheckJobBooklet(completedJob, !forced);
            }
        }
    }
}
