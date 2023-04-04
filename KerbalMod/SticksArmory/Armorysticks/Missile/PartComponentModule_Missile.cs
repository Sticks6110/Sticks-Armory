using System;
using System.Collections.Generic;
using System.Text;
using KSP.Sim.impl;

namespace SticksArmory.Armorysticks.Missile
{
    public class PartComponentModule_Missile : PartComponentModule_ControlSurface
    {

        public override Type PartBehaviourModuleType
        {
            get
            {
                return typeof(PartComponentModule_Missile);
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
