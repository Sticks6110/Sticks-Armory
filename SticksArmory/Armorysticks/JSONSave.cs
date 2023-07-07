using SticksArmory.Armorysticks.Missile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks
{

    [System.Serializable]
    public class WeaponJSONSaveData
    {

        public string PartType; //                  missile, bomb, gunraycast, gunphysical
        public string PartId; //                    The Id of the used part

        public string TextType; //                      Statistics Side Panel String
        public string TextGuidance; //                  Statistics Side Panel String
        public string TextWarhead; //                   Statistics Side Panel String
        public string TextTntEquivilant; //             Statistics Side Panel String
        public string TextDetonation; //                Statistics Side Panel String
        public string TextOrigin; //                    Statistics Side Panel String
        public string TextCalliber; //                  Statistics Side Panel String
        public string TextFR; //                        Statistics Side Panel String

        public float MaxSpeed; //                   m/s
        public float OperationalRange; //           km
        public float TurnSpeed; //                  torque multiplier
        public float DropToFireTime; //             s (Time between deploying and it firing)

        public string CustomAssetBundle;//          put na if you dont have one and are just gonna use the default sticks armory one, put custom asset bundles into the asset folder then bundles of the sticks armory mod
        public string ExplosionEffect; //           ex: Explosion3D_1
        public float ExplosionRadius; //            m
        public float ExplosionEffectSize; //        m
        public float ExplosionDamage; //            float
        public int ExplosionArmorPenetration; //    (Look at ShrapnellArmorPenetration description i do not feel like rewriting it or copying it)
        public bool CustomEffectDir; //             True if you do not want the explosion to face the hit normal
        public float ExplosionEffectRotationX; //   If CustomEffectDir is true then it defaults to this (X)
        public float ExplosionEffectRotationY; //   If CustomEffectDir is true then it defaults to this (Y)
        public float ExplosionEffectRotationZ; //   If CustomEffectDir is true then it defaults to this (Z)

        public string AudioBase; //                 The next ones are just the WWISE Events
        public string AudioClose;
        public string AudioFar;
        public string AudioFireClose;
        public string AudioFireFar;
        public string AudioFireInternal;
        public string AudioExplosion;

        public bool Shrapnell; //                   True if shrapnell
        public int ShrapnellCount; //               Amount of shrapnell shards
        public float ShrapnellMaxAngle; //          Degrees (This determinse what the max angle a shrapnell can offset from the bombs original angle)
        public float ShrapnellDamage; //            float
        public int ShrapnellArmorPenetration; //    All armor has a int that gives it a armor level, this is what the max armor level it can go through is

    }

    [System.Serializable]
    public class PartJSONSaveData
    {
        public string PartId; //                    The Id of the used part

        public bool Radar; //                       True if part is capable of being a radar
        public float RadarRadius; //                Distance radar can go
        public float RadarSensitivity; //           Radar sensitivty (Used for stealth)
        public float RadarUpdate; //                How many seconds for each pulse


    }

    public class JSONSave
    {

        public static Dictionary<string, WeaponJSONSaveData> Weapons = new Dictionary<string, WeaponJSONSaveData>();
        public static Dictionary<string, PartJSONSaveData> Parts = new Dictionary<string, PartJSONSaveData>();

        public static void LoadAllWeapons()
        {

            DirectoryInfo folder = new DirectoryInfo(BepInEx.Paths.PluginPath + @"/armorysticks/weapons/");

            foreach (string file in Directory.GetFiles(folder.FullName, "*.json"))
            {
                Armorysticks.Logger.Log(file);
                string text = File.ReadAllText(file);
                WeaponJSONSaveData data = JsonUtility.FromJson<WeaponJSONSaveData>(text);
                Weapons.Add(data.PartId, data);
                Armorysticks.Logger.Log("Weapon Added: " + data.PartId);
            }

        }

        public static void LoadAllParts()
        {

            DirectoryInfo folder = new DirectoryInfo(BepInEx.Paths.PluginPath + @"/armorysticks/parts/");

            foreach (string file in Directory.GetFiles(folder.FullName, "*.json"))
            {
                Armorysticks.Logger.Log(file);
                string text = File.ReadAllText(file);
                PartJSONSaveData data = JsonUtility.FromJson<PartJSONSaveData>(text);
                Parts.Add(data.PartId, data);
                Armorysticks.Logger.Log("Part Added: " + data.PartId);
            }

        }

    }
}
