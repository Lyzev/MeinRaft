using System;
using MeinRaft.module;
using MeinRaft.module.modules;
using MeinRaft.setting;
using MelonLoader;

namespace MeinRaft
{
    public class Loader : MelonMod
    {
        public static Loader Instance;

        public override void OnUpdate()
        {
            if (Instance == null)
            {
                Instance = this;
                var start = DateTime.Now.Ticks;
                MelonLogger.Msg("[Loader] Loading Mein Raft...");
                MelonLogger.Msg("[Loader] Initializing ModuleManager...");
                ModuleManager.Init();
                MelonLogger.Msg("[Loader] Initialized ModuleManager.");
                MelonLogger.Msg("[Loader] Patching All");
                global::Harmony.HarmonyInstance.Create("Lyzev.Mein_Raft").PatchAll();
                MelonLogger.Msg("[Loader] Patched All.");
                SettingManager.LoadSettings();
                AppDomain.CurrentDomain.ProcessExit += (_, _) => SettingManager.SaveSettings();
                ModuleGUI.Instance.InitSettings();
                MelonLogger.Msg("[Loader] Loaded Settings.");
                MelonLogger.Msg("[Loader] Finished Initializing Mein Raft in " + (DateTime.Now.Ticks - start) / TimeSpan.TicksPerMillisecond + "ms.");
            }

            ModuleManager.OnUpdate();
        }

        public override void OnGUI()
        {
            ModuleManager.OnGUI();
        }
    }
}