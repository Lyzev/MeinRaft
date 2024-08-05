using System.Linq;
using System.Reflection;
using MeinRaft.module;
using UnityEngine;
using UrGUI.UWindow;

namespace MeinRaft.module.modules;

public class ModuleGUI : Module
{
    private readonly UWindow _window;

    public ModuleGUI() : base("Graphical User Interface", "Toggle the GUI with Insert or RightShift.")
    {
        Instance = this;
        _window = UWindow.Begin("Mein Raft by Lyzev", 0, 0, 350, startHeight: 400, dynamicHeight: true);
        _window.SameLine();
        _window.Button("Discord", () => Application.OpenURL("https://lyzev.github.io/discord/"));
        _window.Button("GitHub", () => Application.OpenURL("https://github.com/Lyzev/MeinRaft"));
        _window.Button("TP to Raft", () =>
        {
            var players = Object.FindObjectsOfType<Network_Player>();
            var me = players.FirstOrDefault(p => p.IsLocalPlayer);
            if (me == null)
                return;
            me.SetToWalkableBlockPosition();
        });
        _window.Button("Heal", () =>
        {
            var players = Object.FindObjectsOfType<Network_Player>();
            var me = players.FirstOrDefault(p => p.IsLocalPlayer);
            if (me == null)
                return;
            Stat_WellBeing.Factor = WellBeing.Good;
            me.Stats.stat_health.Heal();
            me.Stats.stat_thirst.Consume(100f, 0f, false);
            me.Stats.stat_hunger.Consume(100f, 0f, false);
            var statsType = me.Stats.GetType();
            var updateUISlidersMethod = statsType.GetMethod("UpdateUISliders", BindingFlags.NonPublic | BindingFlags.Instance);
            if (updateUISlidersMethod != null)
            {
                updateUISlidersMethod.Invoke(me.Stats, null);
            }
            var handleUIFeedbackMethod = statsType.GetMethod("HandleUIFeedback", BindingFlags.NonPublic | BindingFlags.Instance);
            if (handleUIFeedbackMethod != null)
            {
                handleUIFeedbackMethod.Invoke(me.Stats, null);
            }
        });
        _window.IsDrawing = false;
    }

    public static ModuleGUI Instance { get; private set; }

    public void InitSettings()
    {
        ModuleManager.OnSettings(_window);
    }

    protected override void OnEnable()
    {
        _window.IsDrawing = true;
    }

    protected override void OnDisable()
    {
        _window.IsDrawing = false;
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Insert) || Input.GetKeyDown(KeyCode.RightShift)) Toggle();
    }
}