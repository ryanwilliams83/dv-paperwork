using HarmonyLib;
using UnityModManagerNet;

namespace DvMod.Paperwork
{
    [EnableReloading]
    public static class Main
    {
        public static UnityModManager.ModEntry? mod;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Paperwork.LogTrace($"{nameof(Main)}.{nameof(Load)}()");

            mod = modEntry;

            mod.OnToggle = OnToggle;

            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Paperwork.LogTrace($"{nameof(Main)}.{nameof(OnToggle)}()");

            Harmony harmony = new Harmony(modEntry.Info.Id);

            if (value)
            {
                harmony.PatchAll();
            }
            else
            {
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }
    }
}
