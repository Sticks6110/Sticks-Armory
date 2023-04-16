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

namespace SticksArmory.Armorysticks.Patch
{
    //[HarmonyPatch(typeof(PartBehavior), nameof(PartBehavior.OnPartComponentExplosion))]
    [HarmonyPatch(typeof(PartBehavior), nameof(PartBehavior.TriggerSurfaceImpactEffect))]
    class PartExplosion
    {

        public static bool Prefix(PartBehavior __instance, Vector3 contactPoint, Quaternion effectRotation, float deviationFromVertical, SimulationObjectModel celestialBodyModel, Collider hitCollider)
        {

            Armorysticks.Logger.Log("Explosion Prefix");

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

            __instance.Game.GraphicsManager.ContextualFxSystem.TriggerEvent(new FXSticksExplosionEvent(__instance.Game.GraphicsManager.ContextualFxSystem, fXContextualEventParams, partContextData, prefab, d));

            return false;

        }

        /*
        public static bool Prefix(PartComponent __instance, ExplosionType explosionType)
        {

            Armorysticks.Logger.Log("Explosion Prefix");

            PartBehavior part = __instance.Game.SpaceSimulation.ModelViewMap.FromModel(__instance.SimulationObject).Part;

            GameManager.Instance.Game.Notifications.ProcessNotification(new NotificationData
            {
                Tier = NotificationTier.Passive,
                Primary = new NotificationLineItemData { LocKey = "PartExplosion" }
            });

            if (!JSONSave.Launchables.ContainsKey(part.Name) || part == null) return true;



            return false;
        }

        public static void Prefix()
        {
            GameManager.Instance.Game.Notifications.ProcessNotification(new NotificationData
            {
                Tier = NotificationTier.Passive,
                Primary = new NotificationLineItemData { LocKey = "PartExplosion" }
            });
            Armorysticks.Logger.Log("Explosion Prefix");
        }

        public static bool Prefix(PartBehavior __instance, FXContextualEventParams eventParams)
        {
            Armorysticks.Logger.Log("Explosion Prefix");
            ITransformFrame simSOIBodyParentTransformFrame = __instance._simObjectComponent.transform.GetSimSOIBodyParentTransformFrame();
            PQS pqs = null;
            if (__instance.Game.SpaceSimulation.TryGetViewObjectComponent<CelestialBodyBehavior>(simSOIBodyParentTransformFrame.transform.objectModel, out var viewObjectComponent))
            {
                pqs = viewObjectComponent.PqsController;
            }
            FXPartContextData partContextData = __instance.Game.GraphicsManager.ContextualFxSystem.GetPartContextData(__instance, pqs);
            PartOwnerBehavior partOwnerBehavior = __instance.partOwner;

            if (JSONSave.Launchables.ContainsKey(__instance.Name))
            {
                Module_Missile missile = __instance.GetComponent<Module_Missile>();
                string[] effects = missile.data.ExplosionEffect.Split(char.Parse(","));
                string efct = effects[UnityEngine.Random.Range(0, effects.Length - 1)];
                GameObject prefab = ArmorysticksMod.Instance.effects.LoadAsset<GameObject>(efct);

                __instance.Game.GraphicsManager.ContextualFxSystem.TriggerEvent(new FXSticksExplosionEvent(__instance.Game.GraphicsManager.ContextualFxSystem, eventParams, partContextData, prefab, eventParams, missile.data.ExplosionEffectSize));
            }
            else
            {
                if ((object)partOwnerBehavior != null && partOwnerBehavior.SimObjectComponent?.SimulationObject?.Vessel?.IsKerbalEVA == true)
                {
                    __instance.Game.GraphicsManager.ContextualFxSystem.TriggerEvent(new FXKerbalPoofContextualEvent(__instance.Game.GraphicsManager.ContextualFxSystem, eventParams, partContextData));
                }
                else
                {
                    __instance.Game.GraphicsManager.ContextualFxSystem.TriggerEvent(new FXExplosionContextualEvent(__instance.Game.GraphicsManager.ContextualFxSystem, eventParams, partContextData));
                }
            }

            return false;

        }*/

    }
}
