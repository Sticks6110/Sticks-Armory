using KSP.Sim;
using KSP.Sim.impl;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks
{

    public class PartDamageData
    {
        public float health;
        public int armor;
    }

    public static class DamageSystem
    {

        //string = partid
        public static Dictionary<string, PartDamageData> DefaultPartData = new Dictionary<string, PartDamageData>();

        public static Dictionary<PartComponent, PartDamageData> DamagedParts = new Dictionary<PartComponent, PartDamageData>();

        public static void ExplosionDamage(WeaponJSONSaveData data, Position pos, UniverseModel universe)
        {

            List<PartComponent> explosionDamageParts = new List<PartComponent>();
            universe.GetAllPartsInRange(pos, data.ExplosionRadius, ref explosionDamageParts);

            foreach (PartComponent p in explosionDamageParts)
            {

                if (!DamagedParts.ContainsKey(p))
                {
                    //DamagedParts.Add(p, )
                }

                if (DamagedParts[p].armor < data.ExplosionArmorPenetration) DamagedParts[p].health -= data.ExplosionDamage;
                else DamagedParts[p].armor -= data.ExplosionArmorPenetration;

                if (DamagedParts[p].health <= 0)
                {
                    p.DestroyPart(ExplosionType.Default);
                }
            }
        }

    }
}
