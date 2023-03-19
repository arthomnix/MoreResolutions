using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using HarmonyLib;
using KSP.UI;
using UnityEngine;

namespace MoreResolutions;

[HarmonyPatch(typeof(GraphicsSettingsMenuManager))]
[HarmonyPatch("InitializeResolutionDropdown")]
public class GraphicsSettingsMenuManagerPatch
{
    // Regex to make sure the custom resolution is in the correct format
    private static readonly Regex Regex = new(@"^\d+x\d+$", RegexOptions.Compiled);
    
    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
    private static readonly MethodInfo MathfApproximately = SymbolExtensions.GetMethodInfo(() => Mathf.Approximately(0.0f, 0.0f));
    
    // ReSharper disable once UnusedMember.Local
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.Calls(MathfApproximately))
            {
                yield return new CodeInstruction(OpCodes.Pop);
                yield return new CodeInstruction(OpCodes.Pop);
                yield return new CodeInstruction(OpCodes.Ldc_I4_1);
            }
            else yield return instruction;
        }
    }
    
    // ReSharper disable once UnusedMember.Local
    // ReSharper disable once InconsistentNaming
    // Add the custom resolution to the dropdown
    private static void Postfix(GraphicsSettingsMenuManager __instance)
    {
        var customRes = MoreResolutionsPlugin.Instance.CustomResolution.Value;
        if (Regex.Matches(customRes).Count == 1)
            __instance._resolutionDropDown.AddOptions(new List<string> { customRes });
    }
}