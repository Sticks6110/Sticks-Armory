using System;
using System.Collections.Generic;
using System.Text;
using System;
using KSP.Sim;
using KSP.Sim.Definitions;
using UnityEngine;
using UnityEngine.Serialization;
using I2.Loc;
using KSP.Modules;
using HarmonyLib;
using System.Runtime.CompilerServices;
using SticksArmory.Armorysticks;

namespace SticksArmory.Modules
{

    [Serializable]
    public class Data_Bomb : ModuleData
    {

        public override Type ModuleType
        {
            get
            {
                return typeof(Data_Bomb);
            }
        }

        [KSPDefinition]
        public string JSONId = "name.json";

        private WeaponJSONSaveData UncipheredJson = null;

        public void LoadJson()
        {
            UncipheredJson = JSONSave.Weapons[JSONId];

        }

    }
}
