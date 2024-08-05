using MelonLoader;
using MeinRaft.module.modules;
using MeinRaft.setting;
using MeinRaft.setting.settings;
using UnityEngine;
using UrGUI.UWindow;

namespace MeinRaft.module;

public abstract class Module
{
    public readonly string Description;
    public readonly string Name;
    protected readonly GUIStyle Style;
    public SettingSwitch EnabledSetting;

    protected Module(string name, string description)
    {
        Name = name;
        Description = description;
        Style = new GUIStyle();
        Style.fontSize = 30;
        Style.normal.textColor = Color.white;
        Style.fontStyle = FontStyle.Bold;
        EnabledSetting = new SettingSwitch(Name, "Enabled", false, isEnabled =>
        {
            if (isEnabled)
                OnEnable();
            else
                OnDisable();
        });
        MelonLogger.Msg($"[Module Manager] {Name} loaded.");
    }

    public bool Enabled => (bool)EnabledSetting.GetValue();

    public void Toggle()
    {
        EnabledSetting.SetValue(!Enabled);
        if (EnabledSetting.toggle != null)
            EnabledSetting.toggle.Value = Enabled;
        if (Enabled)
            OnEnable();
        else
            OnDisable();
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnGUI()
    {
    }

    public virtual void OnSettings(UWindow window)
    {
        window.Separator();
        window.Label($"<b>{Name}</b>");
        window.Label($"<i>{Description}</i>");
        SettingManager.Get(Name).ForEach(setting => setting.OnSettings(window));
    }
}