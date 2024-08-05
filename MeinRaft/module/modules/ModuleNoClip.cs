using UnityEngine;

namespace MeinRaft.module.modules;

public class ModuleNoClip : Module
{
    public ModuleNoClip() : base("NoClip", "Allows you to fly through walls.")
    {
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        var players = Object.FindObjectsOfType<Network_Player>();
        foreach (var player in players)
        {
            if (player.IsLocalPlayer && !player.currentModel.thirdPersonSettings.ThirdPersonState)
            {
                player.flightCamera.Toggle(true);
                break;
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        var players = Object.FindObjectsOfType<Network_Player>();
        foreach (var player in players)
        {
            if (player.IsLocalPlayer && !player.currentModel.thirdPersonSettings.ThirdPersonState)
            {
                player.flightCamera.Toggle(true);
                break;
            }
        }
    }
}