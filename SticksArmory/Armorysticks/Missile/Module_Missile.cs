using Armorysticks;
using KSP.Game;
using KSP.Iteration.UI.Binding;
using KSP.Modules;
using KSP.Rendering.Planets;
using KSP.Sim;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using KSP.Sim.State;
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
using static KSP.Api.UIDataPropertyStrings.View.Vessel.Stages;
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

        private float timeSinceDeployed = 0;
        private float timeSinceLaunched = 0;
        private bool deployed = false;
        private bool launched = false;
        private bool throttleMaxed = false;

        public WeaponJSONSaveData data;

        public override void OnInitialize()
        {
            base.OnInitialize();

            ModuleAction _actionLaunch = new ModuleAction(new Action(Launch));
            dataMissile.AddAction("STArmory/Modules/Missile/Data/Launch", _actionLaunch, 1);
            dataMissile.SetVisible(_actionLaunch, base.PartBackingMode == PartBackingModes.Flight);
        }

        public override void AddDataModules()
        {
            base.AddDataModules();
            DataModules.TryAddUnique(dataMissile, out dataMissile);
        }

        private void Update()
        {
            if (!deployed) return;

            timeSinceDeployed += Time.deltaTime;

            if (timeSinceDeployed >= data.DropToFireTime && !launched)
            {
                launched = true;
                Armorysticks.Logger.Log("LAUNCHING");
            }

            if (!launched) return;

            timeSinceLaunched += Time.deltaTime;

            if(!throttleMaxed)
            {
                FlightCtrlState flightCtrlState = part.vessel.flightCtrlState;
                flightCtrlState.mainThrottle = 1.0f;
                part.vessel.SimObjectComponent.SetFlightControlState(flightCtrlState);
                throttleMaxed = true;
            }
        }

        private void FixedUpdate()
        {
            if (!launched) return;



        }

        public void Launch()
        {
            if (launched || deployed) return;

            Module_Decouple de = GetComponent<Module_Decouple>();
            de.OnDecouple();

            Module_Engine ee = GetComponent<Module_Engine>();
            ee.StageEngine();

            FlightCtrlState flightCtrlState = part.vessel.flightCtrlState;
            flightCtrlState.mainThrottle = 0f;
            part.vessel.SimObjectComponent.SetFlightControlState(flightCtrlState);

            data = JSONSave.Weapons[part.SimObjectComponent.Name];

            deployed = true;

            //KSPBaseAudio.PostEvent(data.AudioFire, gameObject);
            //KSPBaseAudio.PostEvent(data.AudioBaseStart, gameObject);

        }
    }
}
