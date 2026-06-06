using HarmonyLib;
using UnityEngine;
namespace DvMod.Paperwork.Patches
{
    [HarmonyPatch(typeof(PlayerManager))]
    public static class PlayerManagerPatch
    {
        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.SetCar))]
        [HarmonyPostfix]
        public static void SetCar_Postfix(PlayerManager __instance)
        {
            var car = PlayerManager.Car;
            if (car == null)
                return;

            Debug.Log($"Player changed to car: '{car.name}' in trainset: '{car.trainset.id}'");

            Paperwork.GiveBookletsForTrainset(car.trainset);
        }
    }
}
