using Armorysticks;
using HarmonyLib;
using KSP.Game;
using KSP.Sim.impl;
using KSP.VFX;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static RTG.CameraFocus;
using static UnityEngine.UI.Image;

namespace SticksArmory.Armorysticks.Patch
{
    /*[HarmonyPatch(typeof(ContextualFxSystem), nameof(ContextualFxSystem.LoadFXPrefabFromAssetDatabase))] //OLD CODE
    class ExplosionSpawner
    {

        public static bool Prefix(ContextualFxSystem __instance, string prefabAssetName, FXContextualEvent contextualEvent, bool isDefault = false)
        {

            if (!prefabAssetName.Contains("fx_explosion_sticks")) return true;

            Logger.Log("ExplosionSpawner Prefix");

            GameObject result = ArmorysticksMod.Instance.effects.LoadAsset<GameObject>(prefabAssetName.Split('/')[1]);

            __instance._loadedVFXCache.AddOrUpdate(prefabAssetName, result);
            __instance.OnPrefabLoaded(result, contextualEvent);

            return false;

        }

    }*/

    [HarmonyPatch(typeof(ContextualFxSystem), nameof(ContextualFxSystem.TriggerEvent))]
    class ExplosionSpawner
    {
        public static bool Prefix(ContextualFxSystem __instance, FXContextualEvent contextualEvent)
        {

            string name = contextualEvent.EventParams.SourcePartBehavior.Name;
            if (!JSONSave.Weapons.ContainsKey(name) || __instance == null) return true;

            WeaponJSONSaveData d = JSONSave.Weapons[name];

            string[] effects = d.ExplosionEffect.Split(char.Parse(","));
            string efct = effects[UnityEngine.Random.Range(0, effects.Length - 1)];
            GameObject prefab = ArmorysticksMod.Instance.effects.LoadAsset<GameObject>(efct);

            GameObject gobj = UnityEngine.Object.Instantiate(prefab, contextualEvent.EventParams.SourcePosition, contextualEvent.EventParams.SourceRotation);
            ParticleSystem[] psys = gobj.transform.GetComponentsInChildren<ParticleSystem>();

            if (d.CustomEffectDir)
            {
                gobj.transform.rotation = Quaternion.Euler(d.ExplosionEffectRotationX, d.ExplosionEffectRotationY, d.ExplosionEffectRotationZ);
            }

            psys.ToList().ForEach((ParticleSystem i) => {
                i.playbackSpeed = 1 / d.ExplosionEffectSize;
                i.scalingMode = ParticleSystemScalingMode.Local;
                i.transform.localScale = new Vector3(d.ExplosionEffectSize, d.ExplosionEffectSize, d.ExplosionEffectSize);
            });

            //contextualEvent.EventParams.SourcePartBehavior.vessel.Model.Latitude

            //this.pos = GameManager.Instance.Game.UniverseView.PhysicsSpace.PhysicsToPosition(_spawnedPrefab.transform.position);
            //this.rot = GameManager.Instance.Game.UniverseView.PhysicsSpace.PhysicsToRotation(_spawnedPrefab.transform.rotation);

            return false;
        }
    }

}
