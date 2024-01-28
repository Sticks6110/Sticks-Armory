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
                if (p.TryGetModuleData<PartComponentModule_DamageSystem, Data_DamageSystem>(out Data_DamageSystem out_damageSystem))
                {

                    float dist = (float)Position.DistanceSqr(pos, p.transform.Position);

                    Vector3 _pos = GameManager.Instance.Game.UniverseView.PhysicsSpace.PositionToPhysics(pos);
                    Vector3 _partpos = GameManager.Instance.Game.UniverseView.PhysicsSpace.PositionToPhysics(p.transform.Position);

                    if (Physics.Linecast(_pos, _partpos, out RaycastHit hit, 1000))
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
        public static void Shrapnell(WeaponJSONSaveData data, Position pos, Rotation rot, UniverseModel universe)
        {
            Vector3 _dir = GameManager.Instance.Game.UniverseView.PhysicsSpace.RotationToPhysics(rot).eulerAngles;
            Vector3 _pos = GameManager.Instance.Game.UniverseView.PhysicsSpace.PositionToPhysics(pos);
            //Get unity space pos and dir

            for (int i = 0; i < data.ShrapnellCount; i++)
            {
                float lat = UnityEngine.Random.Range(-data.ShrapnellArmorPenetration, data.ShrapnellMaxAngle); //Get its lateral
                float lon = UnityEngine.Random.Range(-data.ShrapnellArmorPenetration, data.ShrapnellMaxAngle); //Get its longitude

                Vector3 _divergant = (lat * Vector3.up) + (lon * Vector3.right); //Create its diverged direction

                if (Physics.Raycast(_pos, _dir + _divergant, out RaycastHit hit, 300))
                {

                    PartBehavior componentInParent = hit.collider.transform.gameObject.GetComponentInParent<PartBehavior>();
                    if (componentInParent == null) return;

                    PartComponent _part = universe.FindPartComponent(componentInParent.Guid);

                    if (_part.TryGetModuleData<PartComponentModule_DamageSystem, Data_DamageSystem>(out Data_DamageSystem out_damageSystem)) //There is 100% probably a better way to do this without 2 FindComponent calls
                    {
                        out_damageSystem.Damage(data.ShrapnellDamage, false); //Add damage with this method

                    }
                }
            }
        
    }
}
