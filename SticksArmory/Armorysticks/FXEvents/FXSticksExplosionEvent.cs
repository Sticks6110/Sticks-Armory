﻿using Armorysticks;
using KSP.Networking.MP.Utils;
using KSP.Sim;
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
        public WeaponJSONSaveData data;

        public FXSticksExplosionEvent(ContextualFxSystem system, FXContextualEventParams eventParams, FXPartContextData partData, GameObject effect, WeaponJSONSaveData data) : base(eventParams, system, partData)
        {

            Armorysticks.Logger.Log("CREATION");
            this.origin = eventParams;
            this.effect = effect;
            this.data = data;
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

            //GameObject pr = ArmorysticksMod.Instance.effects.LoadAsset<GameObject>(prefab.name);
            //_spawnedPrefab = UnityEngine.Object.Instantiate(prefab, origin.SourcePosition, origin.SourceRotation);

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

            if(data.CustomEffectDir)
            {
                _spawnedPrefab.transform.rotation = Quaternion.Euler(data.ExplosionEffectRotationX, data.ExplosionEffectRotationY, data.ExplosionEffectRotationZ);
            }

            //_spawnedPrefab.transform.localScale = new Vector3(data.ExplosionEffectSize, data.ExplosionEffectSize, data.ExplosionEffectSize);
            //explo.transform.localRotation = Quaternion.Euler(0, 260, 90);

            //_particleSystem.Play();
            _vfxSpawned = true;
            Armorysticks.Logger.Log("EXPLOSION");
        }

        public override string SelectPrefab(FXContextData context)
        {
            return "fx_explosion_sticks/" + effect.name;
        }
    }
}