using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using KSP.UI;
using UnityEngine;

namespace MoreResolutions;

[HarmonyPatch(typeof(GraphicsSettingsMenuManager))]
[HarmonyPatch("InitializeResolutionDropdown")]
public class GraphicsSettingsMenuManagerPatch
{
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
}