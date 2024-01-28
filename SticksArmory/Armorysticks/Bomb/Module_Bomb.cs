using Armorysticks;
using KSP.Game;
using KSP.Modules;
using KSP.Rendering.Planets;
using KSP.Sim;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using KSP.VFX;
using SticksArmory.Armorysticks;
using SticksArmory.Armorysticks.FXEvents;
using SticksArmory.Armorysticks.Bomb;
using SticksArmory.Armorysticks.Monobehaviors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using static KSP.Api.UIDataPropertyStrings;
using static RTG.CameraFocus;
using System.Runtime.CompilerServices;

namespace SticksArmory.Modules
{
    [DisallowMultipleComponent]
    public class Module_Bomb : PartBehaviourModule, IUpdate, IFixedUpdate
    {

        [SerializeField]
        protected Data_Bomb dataBomb = new Data_Bomb();

        private ModuleAction _actionDecouple;

        public override Type PartComponentModuleType
        {
            get
            {
                return typeof(PartComponentModule_Bomb);
            }
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            ModuleAction _actionLaunch = new ModuleAction(new Action(StageLaunch));
            dataBomb.AddAction("STArmory/Modules/Bomb/Data/Launch", _actionLaunch, 1);
            dataBomb.SetVisible(_actionLaunch, base.PartBackingMode == PartBackingModes.Flight);

            ModuleAction _actionCamera = new ModuleAction(new Action(CameraToggle));
            dataBomb.AddAction("STArmory/Modules/Bomb/Data/Camera", _actionCamera, 2);
            dataBomb.SetVisible(_actionCamera, base.PartBackingMode == PartBackingModes.Flight);

        }

        public override void AddDataModules()
        {
            base.AddDataModules();
            DataModules.TryAddUnique(dataBomb, out dataBomb);
        }

        private float timeSinceDeployed = 0;
        private float timeSinceLaunched = 0;
        private float oprangemeters;
        private bool deployed;
        private bool launched;

        private Position dropPos;

        private RigidbodyBehavior rb;

        public WeaponJSONSaveData data;

        private VesselComponent pastParent;



        //Interfaces

        private void Update()
        {
            if (!deployed) return;

            timeSinceDeployed += Time.deltaTime;

            if(timeSinceDeployed >= data.DropToFireTime && !launched)
            {
                launched = true;
                Armorysticks.Logger.Log("DROPPING BOMB");
            }

            if (!launched) return;

            timeSinceLaunched += Time.deltaTime;

        }

        private void FixedUpdate()
        {
            if(!launched) return;
            //if (!launched || Position.Distance(dropPos, transform.Position) >= oprangemeters) return;

            float acceleration = (data.MaxSpeed - rb.activeRigidBody.velocity.magnitude) / timeSinceLaunched;
            // Code was taken from Missile Code, leaving this line in case it causes issues-> rb.activeRigidBody.AddForce(acceleration * part.transform.forward * Time.deltaTime, ForceMode.Acceleration);

            //UPDATE IN FUTURE TO BE MORE ACURATE SUCH AS TAKING INTO ACCOUNT PLANET CURVATURE.

            //Line below was for locating Radar Target, might use this for GPS later
            //Vector3d p1 = GameManager.Instance.Game.UniverseView.PhysicsSpace.PositionToPhysics(Radar.VesselLocks[pastParent].pos);
            Vector3 p2 = part.transform.position;     //Parent

            if (rb.activeRigidBody.velocity.magnitude > data.MaxSpeed)
            {
                rb.activeRigidBody.velocity = rb.activeRigidBody.velocity.normalized * data.MaxSpeed;
            }

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
            Launch(true, true);  // TODO : change the second true to a condition that detects whether the bomb is gps guided
        }



        //Public

        public void Launch(bool state, bool GPSuse)
        {
            if (launched || deployed) return; //this line needs to go if gps guidance works from below
            bool Guidance = false; //for now true = gps and false = dumb
            
            if (Guidance)
            {
                //do guidance things
            }
            else if (launched || deployed)
            {
                return;
            }

            //the rest from here on is 1:1 the missile script

            pastParent = GameManager.Instance.Game.ViewController.GetActiveSimVessel(true);

            Module_Decouple de = GetComponent<Module_Decouple>();
            de.OnDecouple();

            dropPos = transform.Position;

            rb = part.GetComponent<RigidbodyBehavior>();

            data = JSONSave.Weapons[part.SimObjectComponent.Name];
            oprangemeters = data.OperationalRange * 1000;

            deployed = true;

            KSPBaseAudio.PostEvent(data.AudioFire, gameObject);
            KSPBaseAudio.PostEvent(data.AudioBaseStart, gameObject);

        }

        public void CameraToggle()
        {

        }

    }
}
