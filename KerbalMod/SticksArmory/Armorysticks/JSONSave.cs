using SticksArmory.Armorysticks.Missile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks
{
    [System.Serializable]
    public class JSONSaveData
    {

        public string PartType; //          missile, bomb, gunraycast, gunphysical
        public string PartId; //            The Id of the used part

        public float MaxSpeed; //           m/s
        public float OperationalRange; //   km

    }

    public class JSONSave
    {

        public static Dictionary<string, JSONSaveData> Launchables = new Dictionary<string, JSONSaveData>();
        
        public static void LoadAllParts()
        {

            DirectoryInfo folder = new DirectoryInfo(SpaceWarp.API.SpaceWarpManager.MODS_FULL_PATH + @"/armorysticks/weapons/");

            foreach (string file in Directory.GetFiles(folder.FullName, "*.json"))
            {
                Armorysticks.Logger.Log(file);
                string text = File.ReadAllText(file);
                JSONSaveData data = JsonUtility.FromJson<JSONSaveData>(text);
                Launchables.Add(data.PartId, data);
                Armorysticks.Logger.Log("Weapon Added: " + data.PartId);
            }

        }
    }
}
