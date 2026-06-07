using DvMod.Paperwork.Behaviours;
using HarmonyLib;
using UnityEngine;

namespace DvMod.Paperwork.Patches
{
    [HarmonyPatch(typeof(InventoryItemSpec))]
    public static class InventoryItemSpecPatch
    {
        [HarmonyPatch(typeof(InventoryItemSpec), nameof(InventoryItemSpec.ItemIconSprite), MethodType.Getter)]
        public static void Postfix(InventoryItemSpec __instance, ref Sprite __result)
        {
            if (__instance.GetComponent<JobBookletCheckmark>() == null)
                return;

            var texture = Graphics.Composite(__result.texture, EmbeddedResources.Assets.CheckPng.Value, 0.8f);

            __result = Sprite.Create(texture, __result.rect, __result.pivot);
        }
    }
}
