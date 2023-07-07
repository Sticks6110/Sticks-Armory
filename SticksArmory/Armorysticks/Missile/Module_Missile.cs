﻿using Armorysticks;
using KSP.Game;
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

            ModuleAction _actionCamera = new ModuleAction(new Action(CameraToggle));
            dataMissile.AddAction("STArmory/Modules/Missile/Data/Camera", _actionCamera, 2);
            dataMissile.SetVisible(_actionCamera, base.PartBackingMode == PartBackingModes.Flight);

        }

        public override void AddDataModules()
        {
            base.AddDataModules();
            DataModules.TryAddUnique(dataMissile, out dataMissile);
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
                Armorysticks.Logger.Log("LAUNCHING");
            }

            if (!launched) return;

            timeSinceLaunched += Time.deltaTime;

        }

        private void FixedUpdate()
        {
            if(!launched) return;
            //if (!launched || Position.Distance(dropPos, transform.Position) >= oprangemeters) return;

            float acceleration = (data.MaxSpeed - rb.activeRigidBody.velocity.magnitude) / timeSinceLaunched;
            rb.activeRigidBody.AddForce(acceleration * part.transform.forward * Time.deltaTime, ForceMode.Acceleration);

            //UPDATE IN FUTURE TO BE MORE ACUATE SUCH AS TAKING INTO ACCOUNT PLANET CURVATURE.

            Vector3d p1 = GameManager.Instance.Game.UniverseView.PhysicsSpace.PositionToPhysics(Radar.VesselLocks[pastParent].pos); //Radar Blip
            Vector3 p2 = part.transform.position;     //Parent
            Game.CameraManager.CreateFlightCamera(new CameraTweakables());
            Vector3 dir = (p1 - p2).normalized;

            rb.activeRigidBody.angularVelocity = -Vector3.Cross(dir, part.transform.forward) * data.TurnSpeed;

            //rb.activeRigidBody.MoveRotation(Quaternion.RotateTowards(r1, rot, data.TurnSpeed));

            //Armorysticks.Logger.Log(torque);

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
            Launch(true);
        }



        //Public

        public void Launch(bool state)
        {
            if (launched || deployed) return;

            pastParent = GameManager.Instance.Game.ViewController.GetActiveSimVessel(true);

            Module_Decouple de = GetComponent<Module_Decouple>();
            de.OnDecouple();

            dropPos = transform.Position;

            rb = part.GetComponent<RigidbodyBehavior>();

            data = JSONSave.Weapons[part.SimObjectComponent.Name];
            oprangemeters = data.OperationalRange * 1000;

            deployed = true;

        }

        public void CameraToggle()
        {

        }

    }
}
