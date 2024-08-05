using System.Reflection;
using HarmonyLib;
using MeinRaft.setting.settings;

namespace MeinRaft.module.modules;

public class ModuleSnowmobile : Module
{
    public static ModuleSnowmobile Instance;

    public SettingSlider MaxSpeed;
    public SettingSlider Acceleration;

    public ModuleSnowmobile() : base("Snowmobile", "Speeds up.")
    {
        MaxSpeed = new SettingSlider(Name, "Max Speed", 1, 10000, 10000);
        Acceleration = new SettingSlider(Name, "Acceleration", 1, 10000, 10000);
        Instance = this;
    }
}

[HarmonyPatch(typeof(Snowmobile), "HandleDrivingUpdate")]
public static class SnowmobilePatch
{
    [HarmonyPrefix]
    internal static void Prefix(Snowmobile __instance)
    {
        if (!ModuleSnowmobile.Instance.Enabled)
            return;
        var type = __instance.GetType();
        var maxSpeed = type.GetField("maxSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
        if (maxSpeed != null)
        {
            maxSpeed.SetValue(__instance, (float) ModuleSnowmobile.Instance.MaxSpeed.GetValue());
        }
        var acceleration = type.GetField("acceleration", BindingFlags.NonPublic | BindingFlags.Instance);
        if (acceleration != null)
        {
            acceleration.SetValue(__instance, (float) ModuleSnowmobile.Instance.Acceleration.GetValue());
        }
    }
}