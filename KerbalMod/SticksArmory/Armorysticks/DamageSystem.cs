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

        public struct ExplosionDamageData
        {
            public bool explosion;
            public float explosion_Radius;
            public float explosion_Damage;
            public int explosion_ArmorPenetration;

            public Rotation rot;
            public Position pos;

            public bool shrapnell;
            public int shrapnell_Count;
            public float shrapnell_MaxTravelDistance;
            public float shrapnell_MinTravelDistance;
            public float shrapnell_MaxOffsetAngle;
            public float shrapnell_Damage;
            public int shrapnell_ArmorPenetration;
        }

        //string = partid
        public static Dictionary<string, PartDamageData> DefaultPartData = new Dictionary<string, PartDamageData>();

        public static Dictionary<PartComponent, PartDamageData> DamagedParts = new Dictionary<PartComponent, PartDamageData>();

        public static void ExplosionDamage(ExplosionDamageData data, UniverseModel universe)
        {

            if(data.explosion)
            {
                List<PartComponent> explosionDamageParts = new List<PartComponent>();
                universe.GetAllPartsInRange(data.pos, data.explosion_Radius, ref explosionDamageParts);

                foreach (PartComponent p in explosionDamageParts)
                {
                    
                    if(!DamagedParts.ContainsKey(p))
                    {
                        //DamagedParts.Add(p, )
                    }

                    if (DamagedParts[p].armor < data.explosion_ArmorPenetration) DamagedParts[p].health -= data.explosion_Damage;
                    else DamagedParts[p].armor -= data.explosion_ArmorPenetration;

                    if (DamagedParts[p].health <= 0)
                    {
                        p.DestroyPart(ExplosionType.Default);
                    }

                }

            }

            if(data.shrapnell)
            {

            }
        }

    }
}
