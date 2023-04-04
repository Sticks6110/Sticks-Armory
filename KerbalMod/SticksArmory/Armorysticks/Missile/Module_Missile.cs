using Armorysticks;
using KSP.Modules;
using KSP.Rendering.Planets;
using KSP.Sim;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using KSP.VFX;
using SticksArmory.Armorysticks;
using SticksArmory.Armorysticks.FXEvents;
using SticksArmory.Armorysticks.Missile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
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

            base.AddActionGroupAction(new Action<bool>(this.Launch), KSP.Sim.KSPActionGroup.None, "Launch Missile", this.dataMissile.IsDeployed);
            base.AddActionGroupAction(new Action<bool>(this.CameraOn), KSP.Sim.KSPActionGroup.None, "Enable Camera", this.dataMissile.CameraEnabled);
            base.AddActionGroupAction(new Action<bool>(this.CameraOff), KSP.Sim.KSPActionGroup.None, "Disable Camera", this.dataMissile.CameraEnabled);

            _actionDecouple = new ModuleAction(new Action(StageLaunch));
            dataMissile.SetStageActivationAction(_actionDecouple, this);
        }

        private float timeSinceDeployed = 0;
        private float timeSinceLaunched = 0;
        private bool deployed;
        private bool launched;
        private float secondsTillDry;

        private RigidbodyBehavior rb;

        public WeaponJSONSaveData data;



        //Interfaces

        private void Update()
        {
            if (!deployed) return;

            timeSinceDeployed += Time.deltaTime;

            if(timeSinceDeployed >= this.dataMissile.DropToFire.GetValue() && !launched)
            {
                launched = true;
                Armorysticks.Logger.Log("LAUNCHING");
            }

            if (!launched) return;

            timeSinceLaunched += Time.deltaTime;

        }

        private void FixedUpdate()
        {
            if(!launched || timeSinceLaunched >= secondsTillDry) return;

            float acceleration = (data.MaxSpeed - rb.activeRigidBody.velocity.magnitude) / timeSinceLaunched;

            rb.activeRigidBody.AddForce(acceleration * part.transform.forward * Time.deltaTime, ForceMode.Acceleration);

        }



        //Overrides

        public override void OnShutdown()
        {
            base.OnShutdown();
        }



        //Private

        private void StageLaunch()
        {
            if(launched || deployed) return;
            Launch(true);
        }



        //Public

        public void Launch(bool state)
        {
            if (launched || deployed) return;
            Module_Decouple de = GetComponent<Module_Decouple>();
            de.OnDecouple();

            rb = part.GetComponent<RigidbodyBehavior>();

            data = JSONSave.Launchables[part.SimObjectComponent.Name];

            secondsTillDry = data.OperationalRange / (data.MaxSpeed / 1000);

            deployed = true;

        }

        public void CameraOff(bool state)
        {

        }

        public void CameraOn(bool state)
        {
            
        }

    }
}
