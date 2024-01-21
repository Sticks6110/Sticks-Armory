using System;
using System.Collections.Generic;
using System.Text;
using KSP.Sim.impl;

namespace SticksArmory.Armorysticks.Bomb
{
    public class PartComponentModule_Bomb : PartComponentModule_ControlSurface
    {

        public override Type PartBehaviourModuleType
        {
            get
            {
                return typeof(PartComponentModule_Bomb);
            }
        }

        public override void OnStart(double universalTime)
        {
        }

        public override void OnUpdate(double universalTime, double deltaUniversalTime)
        {
        }

    }
}
