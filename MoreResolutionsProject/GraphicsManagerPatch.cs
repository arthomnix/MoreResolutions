using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using KSP.Rendering;
using UnityEngine;

namespace MoreResolutions;

[HarmonyPatch]
public class GraphicsManagerPatch
{
    // ReSharper disable once UnusedMember.Local
    // we are patching an overloaded method so we can't specify the method in the HarmonyPatch attribute
    private static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(GraphicsManager), "SetResolution",
            new[] { typeof(int), typeof(int), typeof(FullScreenMode) });
    }

    // ReSharper disable once UnusedMember.Local
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.Is(OpCodes.Ldc_I4, 1280) || instruction.Is(OpCodes.Ldc_I4, 720))
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
            else yield return instruction;
        }
    }
}