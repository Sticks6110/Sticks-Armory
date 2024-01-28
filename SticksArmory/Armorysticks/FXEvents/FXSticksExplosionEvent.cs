using Armorysticks;
using KSP.Api;
using KSP.Game;
using KSP.Networking.MP.Utils;
using KSP.Sim;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
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
        public FXPartContextData partData;

        public Position pos;
        public Rotation rot;
        
        public FXSticksExplosionEvent(ContextualFxSystem system, FXContextualEventParams eventParams, FXPartContextData partData, GameObject effect, WeaponJSONSaveData data) : base(eventParams, system, partData)
        {

            Armorysticks.Logger.Log("CREATION");
            this.origin = eventParams;
            this.effect = effect;
            this.data = data;
            this.partData = partData;

        }

        public override void Tick(FXContextData context)
        {
            if (_vfxSpawned == false) return;
            _spawnedPrefab.transform.position = GameManager.Instance.Game.UniverseView.PhysicsSpace.PositionToPhysics(pos);
            _spawnedPrefab.transform.rotation = GameManager.Instance.Game.UniverseView.PhysicsSpace.RotationToPhysics(rot);

            base.Tick(context);

            //_spawnedPrefab.transform.position = context.
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
            _particleSystems = _spawnedPrefab.GetComponentsInChildren<ParticleSystem>();

            this.pos = GameManager.Instance.Game.UniverseView.PhysicsSpace.PhysicsToPosition(_spawnedPrefab.transform.position);
            this.rot = GameManager.Instance.Game.UniverseView.PhysicsSpace.PhysicsToRotation(_spawnedPrefab.transform.rotation);

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

            _particleSystems.ToList().ForEach((ParticleSystem i) => {
                i.playbackSpeed = 1 / data.ExplosionEffectSize;
                i.scalingMode = ParticleSystemScalingMode.Local;
                i.transform.localScale = new Vector3(data.ExplosionEffectSize, data.ExplosionEffectSize, data.ExplosionEffectSize);
            });

            //SpawnedPrefab.transform.localScale = new Vector3(data.ExplosionEffectSize, data.ExplosionEffectSize, data.ExplosionEffectSize);

            _vfxSpawned = true;
            Armorysticks.Logger.Log("EXPLOSION");
        }

        public override string GetVFXPrefabName(FXContextData context)
        {
            return "fx_explosion_sticks/" + effect.name;
        }
    }
}
