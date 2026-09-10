// Based on code made by MarC0 / ManlyMarco
// Copyright 2018 GNU General Public License v3.0

using HarmonyLib;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConfigurationManager
{
    public static class Patches
    {
        const string OpenLogString = "Show Player.log";
        public static GameObject OpenMenuButton { get; set; }

        public static void ApplyPatches()
        {
            Harmony harmony = new Harmony(BepInExPlugin.GUID);

            harmony.Patch(
                original: AccessTools.Method(typeof(FejdStartup), "Start"),
                postfix: new HarmonyMethod(AccessTools.Method(typeof(Patches), nameof(Start)))
                );

            harmony.Patch(
                original: AccessTools.Method(typeof(Terminal), "InputText"),
                prefix: new HarmonyMethod(AccessTools.Method(typeof(Patches), nameof(InputText)))
                );

            BepInExPlugin._openMenuText.SettingChanged += (s, e) => SetupMenuButton();
            BepInExPlugin._showMenuButton.SettingChanged += (s, e) => SetupMenuButton();
        }

        public static void SetupMenuButton()
        {
            if (OpenMenuButton == null && FejdStartup.instance?.m_mainMenu != null)
            {
                var openLogLabel = FejdStartup.instance.m_mainMenu.GetComponentsInChildren<TMP_Text>(true)
                    .FirstOrDefault(t => t.text == OpenLogString || (Localization.instance != null && t.text == Localization.instance.Localize(OpenLogString)));
                if (openLogLabel != null)
                {
                    OpenMenuButton = Object.Instantiate(openLogLabel.transform.parent.gameObject, openLogLabel.transform.parent.parent);
                    OpenMenuButton.name = "OpenConfigMenu";
                    OpenMenuButton.transform.localPosition += new Vector3(0, 25, 0);
                    var button = OpenMenuButton.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick = new Button.ButtonClickedEvent();
                        button.onClick.AddListener(() => BepInExPlugin.context.DisplayingWindow = true);
                    }
                }
            }

            if (OpenMenuButton == null)
                return;

            var label = OpenMenuButton.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = BepInExPlugin._openMenuText.Value;
            OpenMenuButton.SetActive(BepInExPlugin._showMenuButton.Value);
        }

        public static bool InputText(Terminal __instance)
        {
            if (__instance?.m_input == null)
                return true;

            string text = __instance.m_input.text;
            if (text != null && text.ToLower().Equals("configurationmanager reset"))
            {
                BepInExPlugin.context.Config.Reload();
                BepInExPlugin.context.Config.Save();
                __instance.AddString(text);
                __instance.AddString($"{BepInExPlugin.context.Info.Metadata.Name} config reloaded");
                return false;
            }
            return true;
        }

        public static void Start()
        {
            SetupMenuButton();
        }
    }
}