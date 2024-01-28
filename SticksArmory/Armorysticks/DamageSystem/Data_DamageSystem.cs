using KSP.Iteration.UI.Binding;
using KSP.Modules;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using SticksArmory.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace SticksArmory.Armorysticks.DamageSystem
{
    [Serializable]
    public class Data_DamageSystem : ModuleData
    {
        public override Type ModuleType
        {
            get
            {
                return typeof(Data_DamageSystem);
            }
        }

        public float Health = 100;
        public float Armor = 100;

        //DO NOT MESS WITH THESE
        public bool OnFire = false;
        public bool CheckDamageChanges = false;

        public void SetAblaze()
        {
            OnFire = true;
        }

        public void Damage(float amount, bool armor_penetrating)
        {
            if (armor_penetrating || Armor == 0)
            {
                Health -= amount;
            }
            else
            {
                Armor -= amount;
            }

            CheckDamageChanges = true;
        }
    }
}
