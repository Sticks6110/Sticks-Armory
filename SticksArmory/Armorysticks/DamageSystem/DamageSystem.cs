using KSP.Sim;
using KSP.Sim.impl;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks.DamageSystem
{
    public static class DamageSystem
    {

        public static void ExplosionDamage(WeaponJSONSaveData data, Position pos, UniverseModel universe)
        {

            List<PartComponent> explosionDamageParts = new List<PartComponent>();
            universe.GetAllPartsInRange(pos, data.ExplosionRadius, ref explosionDamageParts);

            foreach (PartComponent p in explosionDamageParts)
            {
                if(p.TryGetModuleData<PartComponentModule_DamageSystem, Data_DamageSystem>(out Data_DamageSystem out_damageSystem))
                {

                    float dist = (float)Position.DistanceSqr(pos, p.transform.Position);

                    Vector3 _pos = GameManager.Instance.Game.UniverseView.PhysicsSpace.PositionToPhysics(pos);
                    Vector3 _partpos = p.transform.Position(_partpos);

                    if (Physics.Linecast(_pos,_partpos, out RaycastHit hit, data.ExplosionRadius))
                    {
                        PartBehavior componentInParent = hit.collider.transform.gameObject.GetComponentInParent<PartBehavior>();
                        if (componentInParent == null/*||data.ExplosionArmorPenetration<=p.WeaponJSONSaveData.Armor*/) return;
                    }
                    else
                    {
                        out_damageSystem.Damage(1 / (Mathf.Pow(dist, 2)), false);
                    }

                }
            }

        }
    }
}
