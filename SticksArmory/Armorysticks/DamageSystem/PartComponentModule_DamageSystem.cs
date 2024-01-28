using KSP.Sim.impl;
using SticksArmory.Armorysticks.Missile;
using System;
using System.Collections.Generic;
using System.Text;

namespace SticksArmory.Armorysticks.DamageSystem
{
    public class PartComponentModule_DamageSystem : PartComponentModule
    {
        public override Type PartBehaviourModuleType
        {
            get
            {
                return typeof(Module_DamageSystem);
            }
        }
    }
}
