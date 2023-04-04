using Armorysticks;
using KSP.Networking.MP.Utils;
using KSP.Sim.Definitions;
using KSP.VFX;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static RTG.CameraFocus;

namespace SticksArmory.Armorysticks.FXEvents
{
    public class FXSticksExplosionEvent : FXContextualEvent
    {
        public override string DefaultVFXAssetName => "fx_explosion_sticks";

        public override VFXEventType EventType => VFXEventType.Explosion;

        public GameObject effect;
        public FXContextualEventParams origin;
        public float scale;

        public FXSticksExplosionEvent(ContextualFxSystem system, FXContextualEventParams eventParams, FXPartContextData partData, GameObject effect, FXContextualEventParams original, float scale) : base(eventParams, system, partData)
        {

            Armorysticks.Logger.Log("CREATION");
            this.origin = original;
            this.effect = effect;
            this.scale = scale;

        }

        public override void Tick(FXContextData context)
        {
            base.Tick(context);
        }

        public override void CleanUp()
        {
            Armorysticks.Logger.Log("CLEAN");
            base.CleanUp();
        }

        public override void Instantiate(GameObject prefab, FXContextData contextData)
        {

            Armorysticks.Logger.Log("INSTANTIATION");

            _spawnedPrefab = UnityEngine.Object.Instantiate(effect, origin.SourcePosition, origin.SourceRotation);
            _particleSystem = _spawnedPrefab.GetComponentInChildren<ParticleSystem>();
            parameterizers = _spawnedPrefab.GetComponentsInChildren<VFXParameterizer>();

            if (parameterizers.Length != 0)
            {
                UpdateParameterizers();
            }
            else
            {
                Armorysticks.Logger.Log("MISSING VFXParamaterizer");
            }

            _spawnedPrefab.transform.localScale = new Vector3(scale, scale, scale);
            //explo.transform.localRotation = Quaternion.Euler(0, 260, 90);

            _particleSystem.Play();
            _vfxSpawned = true;
            Armorysticks.Logger.Log("EXPLOSION");
        }

        public override string SelectPrefab(FXContextData context)
        {
            return "fx_explosion_sticks/" + effect.name;
        }
    }
}
