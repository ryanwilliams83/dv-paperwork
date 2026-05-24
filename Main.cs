using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace DvMod.Paperwork
{
    [EnableReloading]
    public static class Main
    {
        public static UnityModManager.ModEntry? mod;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;

            mod.OnToggle = OnToggle;

            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Harmony harmony = new Harmony(modEntry.Info.Id);

            if (value)
            {
                harmony.PatchAll();
                PlayerManager.CarChanged += PlayerManager_CarChanged;
            }
            else
            {
                PlayerManager.CarChanged -= PlayerManager_CarChanged;
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        private static void PlayerManager_CarChanged(TrainCar car)
        {
            if (car == null)
                return;

            Debug.Log($"Player changed to car: '{car.name}' in trainset: '{car.trainset.id}'");

            Paperwork.GiveBookletsForTrainset(car.trainset);
        }
    }
}
