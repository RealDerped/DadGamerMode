using System;
using System.Reflection;
using SPT.Reflection.Patching;
using EFT;
using EFT.HealthSystem;
using HarmonyLib;

namespace DadGamerMode.Patches
{
    public class FallingDamagePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Specifically targets the method that calculates leg/body damage upon impact
            return AccessTools.Method(typeof(ActiveHealthController), nameof(ActiveHealthController.ApplyFallingDamage));
        }

        [PatchPrefix]
        private static bool Prefix(ActiveHealthController __instance)
        {
            // Check if the player being processed is the user
            if (!__instance.Player.IsYourPlayer)
            {
                return true;
            }

            // Check the config toggle we set up in Plugin.cs
            if (Plugin.NoFallDamage.Value)
            {
                // Returning false stops the original method from running, 
                // effectively deleting the fall damage before it's calculated.
                return false; 
            }

            return true;
        }
    }
}
