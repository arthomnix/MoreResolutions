using HarmonyLib;
using KSP.UI;

namespace MoreResolutions;

[HarmonyPatch(typeof(SettingsMenuManager))]
[HarmonyPatch("ShowGraphicsSettings")]
public class SettingsMenuManagerPatch
{
    // ReSharper disable once UnusedMember.Local
    // ReSharper disable once InconsistentNaming
    // Refresh the resolution dropdown every time graphics settings is opened,
    // in case the user changed their custom resolution
    private static void Prefix(SettingsMenuManager __instance)
    {
        __instance._graphicsSettings.InitializeResolutionDropdown();
    }
}