using Armorysticks;
using HarmonyLib;
using KSP.Sim.impl;
using KSP.VFX;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks.Patch
{
    [HarmonyPatch(typeof(ContextualFxSystem), nameof(ContextualFxSystem.LoadFXPrefabFromAssetDatabase))]
    class ExplosionSpawner
    {

        public static bool Prefix(ContextualFxSystem __instance, string prefabAssetName, FXContextualEvent contextualEvent, bool isDefault = false)
        {

            if (!prefabAssetName.Contains("fx_explosion_sticks")) return true;

            GameObject result = ArmorysticksMod.Instance.effects.LoadAsset<GameObject>(prefabAssetName.Split('/')[1]);

            __instance._loadedVFXCache.AddOrUpdate(prefabAssetName, result);
            __instance.OnPrefabLoaded(result, contextualEvent);

            return false;

        }

    }
}
