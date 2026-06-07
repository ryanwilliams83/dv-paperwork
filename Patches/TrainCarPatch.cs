using DV.Booklets;
using DV.Logic.Job;
using DvMod.Paperwork.Behaviours;
using DvMod.Paperwork.Cache;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VLB;

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
            if (__instance.IsLoco)
                return;

            if (!__instance.brakeSystem.hasHandbrake)
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
            if (handbrakeChangedHandlers.TryGetValue(__instance, out var handler))
            {
                __instance.brakeSystem.HandbrakePositionChanged -= handler;
                handbrakeChangedHandlers.Remove(__instance);
            }
        }

        private static void OnHandbrakePositionChanged(TrainCar car, float value, bool forced)
        {
            if (value < 1f)
                return;

            // Debug.Log($"Handbrake applied on '{car.logicCar.ID}', value={value}, forced={forced}");
            var job = JobsManager.Instance.GetJobOfCar(car.logicCar, true);
            if (job == null)
                return;

            var allTasksCompleted = job.tasks.All(x => x.IsTaskCompleted());
            Debug.Log($"Job: '{job.ID}' TasksCompleted: '{allTasksCompleted}'");

            if (!allTasksCompleted)
                return;

            var booklet = JobBooklet.allExistingJobBooklets.FirstOrDefault(x => x.job.ID == job.ID)
                ?? BookletCreator.CreateJobBooklet(job, PlayerManager.PlayerTransform.position, PlayerManager.PlayerTransform.rotation);

            booklet.gameObject.GetOrAddComponent<JobBookletCheckmark>();

            SfxHost.Instance.StartCoroutine(PlaySoundDelayed());
        }

        private static IEnumerator PlaySoundDelayed()
        {
            yield return new WaitForSeconds(2f);

            var src = SfxHost.Instance.GetComponent<AudioSource>();
            var sfx = EmbeddedResources.Assets.CheckWav.Value;

            const float volume = 1f;
            src.PlayOneShot(sfx, volume);
        }
    }
}
