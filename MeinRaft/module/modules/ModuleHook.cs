using System.Reflection;
using HarmonyLib;
using MeinRaft.setting.settings;
using UnityEngine;

namespace MeinRaft.module.modules;

public class ModuleHook : Module
{
    public SettingSlider Range;

    public ModuleHook() : base("Hook", "Automatically picks up items in the water.")
    {
        Range = new SettingSlider(Name, "Range", 5, 200, 75);
        Instance = this;
    }

    public static ModuleHook Instance { get; private set; }
}

[HarmonyPatch(typeof(Hook), "Update")]
public static class HookPatch
{
    [HarmonyPrefix]
    internal static void Prefix(Hook __instance)
    {
        if (!ModuleHook.Instance.Enabled)
            return;
        if (!__instance.throwable.IsLocked && __instance.throwable.InWater && MyInput.GetButton("LMB"))
        {
            var hookType = __instance.GetType();
            var playerNetworkFieldInfo =
                hookType.GetField("playerNetwork", BindingFlags.NonPublic | BindingFlags.Instance);
            if (playerNetworkFieldInfo != null)
            {
                var playerNetwork = (Network_Player)playerNetworkFieldInfo.GetValue(__instance);
                if (playerNetwork.IsLocalPlayer)
                {
                    var items = Object.FindObjectsOfType<PickupItem>();
                    foreach (var item in items)
                    {
                        if (Vector3.Distance(__instance.hookItemCollector.transform.position, item.transform.position) >
                            (float)ModuleHook.Instance.Range.GetValue())
                            continue;
                        if (item.pickupItemType == PickupItemType.Default || item.pickupItemType == PickupItemType.QuestItem)
                            playerNetwork.PickupScript.PickupItem(item, true, false);
                    }

                }
            }
        }
    }
}