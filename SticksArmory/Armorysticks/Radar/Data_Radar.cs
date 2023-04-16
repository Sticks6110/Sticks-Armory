using KSP.Sim.Definitions;
using KSP.Sim;
using System;
using System.Collections.Generic;
using System.Text;

namespace SticksArmory.Armorysticks.Radar
{
    [Serializable]
    public class Data_Radar : ModuleData
    {

        public override Type ModuleType
        {
            get
            {
                return typeof(Data_Radar);
            }
        }

    }
}
