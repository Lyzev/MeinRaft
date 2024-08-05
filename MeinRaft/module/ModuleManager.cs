using System.Collections.Generic;
using System.Linq;
using System.Text;
using MelonLoader;
using MeinRaft.module.modules;
using UrGUI.UWindow;

namespace MeinRaft.module;

public static class ModuleManager
{
    public static List<Module> Modules = new();

    public static void Init()
    {
        Modules.Add(new ModuleSnowmobile());
        Modules.Add(new ModuleNoClip());
        Modules.Add(new ModuleHook());
        Modules.Add(new ModuleGUI());
        // Code to generate markdown for the modules
        // StringBuilder markdown = new StringBuilder();
        // foreach (var module in Modules)
        // {
        //     markdown.AppendLine("<details>");
        //     markdown.AppendLine($"<summary>{module.Name.Replace("Module", "")}</summary>");
        //     markdown.AppendLine();
        //     markdown.AppendLine($"{module.Description}");
        //     markdown.AppendLine("</details>");
        //     markdown.AppendLine();
        // }
        // MelonLogger.Msg(markdown.ToString());
    }

    public static void OnUpdate()
    {
        foreach (var module in Modules) module.OnUpdate();
    }

    public static void OnGUI()
    {
        foreach (var module in Modules) module.OnGUI();
    }

    public static void OnSettings(UWindow window)
    {
        foreach (var module in Modules)
        {
            var moduleType = module.GetType();
            while (moduleType != null)
            {
                moduleType = moduleType.BaseType;
            }

            if (moduleType != null)
                continue;
            module.OnSettings(window);
        }
    }

    public static List<T> GetModules<T>() where T : Module
    {
        var modules = new List<T>();
        foreach (var module in Modules)
            if (module is T)
                modules.Add((T)module);
        return modules;
    }

    public static T GetModule<T>() where T : Module
    {
        return Modules.First(m => m is T) as T;
    }
}