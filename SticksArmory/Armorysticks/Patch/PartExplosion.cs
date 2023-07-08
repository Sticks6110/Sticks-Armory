using Armorysticks;
using HarmonyLib;
using KSP.OAB;
using KSP.Rendering.Planets;
using KSP.Sim;
using KSP.Sim.impl;
using KSP.VFX;
using SticksArmory.Armorysticks;
using SticksArmory.Armorysticks.FXEvents;
using System;
using System.Collections.Generic;
using System.Text;
using static RTG.CameraFocus;
using UnityEngine;
using SticksArmory.Modules;
using KSP.Game;
using KSP.Iteration.UI.Binding;
using System.Reflection;
using KSP.Audio;
using RTG;

namespace SticksArmory.Armorysticks.Patch
{
    [HarmonyPatch(typeof(PartBehavior), nameof(PartBehavior.TriggerSurfaceImpactEffect))]
    class PartExplosion
    {

        public static bool Prefix(PartBehavior __instance, Vector3 contactPoint, Quaternion effectRotation, float deviationFromVertical, SimulationObjectModel celestialBodyModel, Collider hitCollider)
        {

            Logger.Log("Explosion Prefix");

            if (!JSONSave.Weapons.ContainsKey(__instance.Name) || __instance == null) return true;

            WeaponJSONSaveData d = JSONSave.Weapons[__instance.Name];

            PQS pqs = null;
            if (__instance.Game.SpaceSimulation.TryGetViewObjectComponent<CelestialBodyBehavior>(celestialBodyModel, out var viewObjectComponent))
            {
                pqs = viewObjectComponent.PqsController;
            }
            SurfaceColliderData data = new SurfaceColliderData();
            if (hitCollider != null)
            {
                data = hitCollider.gameObject.GetComponent<SurfaceColliderData>();
            }
            FXContextualEventParams fXContextualEventParams = new FXContextualEventParams(__instance.transform, __instance.transform.position, effectRotation, __instance, __instance.explosionPotential, pqs, data);
            fXContextualEventParams.SurfaceType = __instance.Game.GraphicsManager.ContextualFxSystem.GetSurfaceType(hitCollider, __instance, contactPoint);
            FXPartContextData partContextData = __instance.Game.GraphicsManager.ContextualFxSystem.GetPartContextData(__instance, pqs);
            partContextData.DeviationFromVertical = deviationFromVertical;

            string[] effects = d.ExplosionEffect.Split(char.Parse(","));
            string efct = effects[UnityEngine.Random.Range(0, effects.Length - 1)];
            GameObject prefab = ArmorysticksMod.Instance.effects.LoadAsset<GameObject>(efct);

            VesselComponent vessel = GameManager.Instance.Game.ViewController.GetActiveSimVessel(true);

            __instance.Game.GraphicsManager.ContextualFxSystem.TriggerEvent(new FXSticksExplosionEvent(__instance.Game.GraphicsManager.ContextualFxSystem, fXContextualEventParams, partContextData, prefab, d));

            string[] sounds = d.AudioExplosion.Split(char.Parse(","));
            string snd = sounds[UnityEngine.Random.Range(0, sounds.Length - 1)];

            KSPBaseAudio.PostEvent(d.AudioBaseStop, __instance.gameObject);
            KSPBaseAudio.PostEvent(snd, __instance.gameObject);

            return false;

        }

    }
}
