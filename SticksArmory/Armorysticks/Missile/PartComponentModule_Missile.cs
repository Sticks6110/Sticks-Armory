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
    public class PartComponentModule_Missile : PartComponentModule, IFixedUpdate
    {

        public override Type PartBehaviourModuleType
        {
            get
            {
                return typeof(PartComponentModule_Missile);
            }
        }

        private float timeSinceDeployed = 0;
        private float timeSinceLaunched = 0;
        private bool deployed;
        private bool launched;

        private RigidbodyComponent rb;

        public WeaponJSONSaveData data;

        private VesselComponent pastParent;

        public Data_Missile dataMissile;

        private EngineForce Force;

        public override void OnStart(double universalTime)
        {
            if (!DataModules.TryGetByType<Data_Missile>(out dataMissile))
            {
                Armorysticks.Logger.Log("CANNOT FIND DATA MODULE MISSILE");
                return;
            }
        }

        public override void OnUpdate(double universalTime, double deltaUniversalTime)
        {

            if (dataMissile.Deployed.GetValue() == true && deployed == false) Launch();

            if (!deployed) return;

            timeSinceDeployed += (float)deltaUniversalTime;

            if (timeSinceDeployed >= data.DropToFireTime && !launched)
            {
                launched = true;
                Armorysticks.Logger.Log("LAUNCHING");
            }

            if (!launched) return;

            timeSinceLaunched += (float)deltaUniversalTime;
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (!launched) return;

            //if(rb.activeRigidBody.velocity.magnitude < data.MaxSpeed)
            //{
            //float acceleration = (data.MaxSpeed - rb.activeRigidBody.velocity.magnitude) / timeSinceLaunched;
            //rb.activeRigidBody.AddForce(acceleration * part.transform.forward * Time.deltaTime, ForceMode.Acceleration);
            //}

            Vector3d ThrustDir = Part.transform.forward.vector;
            Vector3d ThrustPos = Vector3d.zero;

            Force.ForceMode = ForceType.Force;
            Force.RelativeForce = data.MaxSpeed * ThrustDir;
            Force.RelativePosition = ThrustPos;

            Blip _lock = null;

            if (Monobehaviors.Radar.VesselLocks.TryGetValue(pastParent, out _lock))
            {

                //Position p1 = _lock.pos;
                //Position p2 = Part.transform.Position;

                //rb.Model.AngularVelocity = new AngularVelocity(rb.Model.relativeToMotion, Vector.scale(Vector.cross(p2 - p1, transform.forward), data.TurnSpeed));

            }
        }

        public void Launch()
        {
            if (dataMissile.Deployed.GetValue() == true && deployed == true) return;

            pastParent = GameManager.Instance.Game.ViewController.GetActiveSimVessel(true);

            rb = Part.SimulationObject.Rigidbody;

            data = JSONSave.Weapons[Part.SimulationObject.Name];

            deployed = true;

            Force = new EngineForce();
            rb.AddForce(Force);

        }
    }
}
