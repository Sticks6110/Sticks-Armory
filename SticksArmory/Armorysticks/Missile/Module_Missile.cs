using Armorysticks;
using KSP.Game;
using KSP.Iteration.UI.Binding;
using KSP.Modules;
using KSP.Rendering.Planets;
using KSP.Sim;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using KSP.VFX;
using SticksArmory.Armorysticks;
using SticksArmory.Armorysticks.FXEvents;
using SticksArmory.Armorysticks.Missile;
using SticksArmory.Armorysticks.Monobehaviors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using static KSP.Api.UIDataPropertyStrings;
using static KSP.Modules.Data_Engine;
using static RTG.CameraFocus;

namespace SticksArmory.Modules
{
    [DisallowMultipleComponent]
    public class Module_Missile : PartBehaviourModule, IUpdate, IFixedUpdate
    {

        [SerializeField]
        protected Data_Missile dataMissile = new Data_Missile();

        private ModuleAction _actionDecouple;

        public override Type PartComponentModuleType
        {
            get
            {
                return typeof(PartComponentModule_Missile);
            }
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            ModuleAction _actionLaunch = new ModuleAction(new Action(StageLaunch));
            dataMissile.AddAction("STArmory/Modules/Missile/Data/Launch", _actionLaunch, 1);
            dataMissile.SetVisible(_actionLaunch, base.PartBackingMode == PartBackingModes.Flight);
        }

        public override void AddDataModules()
        {
            base.AddDataModules();
            DataModules.TryAddUnique(dataMissile, out dataMissile);
        }

        public void StageLaunch()
        {
            if (dataMissile.Deployed.GetValue() == true) return;
            Module_Decouple de = part.GetComponent<Module_Decouple>();
            de.OnDecouple();
            WeaponJSONSaveData data = JSONSave.Weapons[part.SimObjectComponent.Name];
            KSPBaseAudio.PostEvent(data.AudioFire, gameObject);
            KSPBaseAudio.PostEvent(data.AudioBaseStart, gameObject);

            dataMissile.Deployed.SetValue(true);
        }
    }
}
