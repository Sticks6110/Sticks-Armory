using KSP.Sim.impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace SticksArmory.Armorysticks.Missile
{

    public class VesselInfo
    {

        public struct VInfo
        {
            public float crossSection;
            public int team;
        }

        public static Dictionary<VesselComponent, VInfo> Vessels = new Dictionary<VesselComponent, VInfo>();


    }
}
