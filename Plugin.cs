using BepInEx;
using BepInEx.Configuration;
using DadGamerMode.Patches;
using System;

namespace DadGamerMode
{
    [BepInPlugin("com.realderped.dadgamermode", "DadGamerMode", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        // Configuration Entries
        public static ConfigEntry<bool> GodMode { get; private set; }
        public static ConfigEntry<float> DamageReduction { get; private set; }
        public static ConfigEntry<string> Keep1HealthSelection { get; private set; }
        public static ConfigEntry<bool> NoFallDamage { get; private set; }

        private void Awake()
        {
            // 1. Setup Configuration Menu (F12)
            InitConfig();

            // 2. Initializing Patches
            try
            {
                new MergedDamagePatch().Enable();
                
                if (NoFallDamage.Value)
                {
                    // Assuming you keep your existing Fall Damage patch
                    new FallingDamagePatch().Enable();
                }

                Logger.LogInfo("DadGamerMode: Merged Logic Loaded for SPT 4.0.13");
            }
            catch (Exception ex)
            {
                Logger.LogError($"DadGamerMode: Failed to load patches. Error: {ex.Message}");
            }
        }

        private void InitConfig()
        {
            GodMode = Config.Bind(
                "1. Global Settings",
                "God Mode",
                false,
                "If enabled, you take zero damage from all sources."
            );

            DamageReduction = Config.Bind(
                "2. Damage Settings",
                "Damage Reduction %",
                100f,
                new ConfigDescription("Percentage of damage received (e.g., 50 means you take half damage).", new AcceptableValueRange<float>(0f, 100f))
            );

            Keep1HealthSelection = Config.Bind(
                "2. Damage Settings",
                "Keep 1 Health Selection",
                "None",
                new ConfigDescription("Select which body parts to protect from death.", new AcceptableValueList<string>("None", "Head", "Thorax", "Head & Thorax", "All"))
            );

            NoFallDamage = Config.Bind(
                "3. Movement Settings",
                "No Fall Damage",
                true,
                "Disable damage taken from falling high distances."
            );
        }
    }
}
