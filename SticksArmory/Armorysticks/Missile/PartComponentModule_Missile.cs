using System;
using System.Collections.Generic;
using System.Text;
using KSP.Sim.impl;
using static RTG.CameraFocus;
using UnityEngine;
using KSP.Game;
using KSP.Iteration.UI.Binding;
using KSP.Modules;
using RTG;
using static KSP.Api.UIDataPropertyStrings.View.Vessel.Stages;
using SticksArmory.Armorysticks.Monobehaviors;
using KSP.Sim;
using SticksArmory.Armorysticks.Radar;
using SticksArmory.Modules;
using Armorysticks;
using static KSP.Modules.Data_Engine;
using UnityEngine.UIElements;

namespace SticksArmory.Armorysticks.Missile
{
    public class PartComponentModule_Missile : PartComponentModule
    {

        public override Type PartBehaviourModuleType
        {
            get
            {
                return typeof(PartComponentModule_Missile);
            }
        }

    }
}
