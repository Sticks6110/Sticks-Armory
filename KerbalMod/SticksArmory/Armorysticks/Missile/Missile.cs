using Armorysticks;
using AwesomeTechnologies.Utility;
using KSP.Sim;
using KSP.Sim.impl;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks.Missile
{

    public class MissileStage
    {
        public Action<MissileStage> stage;

        //If not used set to -1 (Set range float to -1 to disable range)

        public char stageType; //"s, a, t, r

        //Speed
        public float maxSpeed;
        public float minSpeed;

        //Altitude
        public float maxAltitude;
        public float minAltitude;

        //Timed
        public float waitTime;

        //Range
        public Vector3 targetRange;
        public float range;

    }

    public class Missile : IFixedUpdate, IUpdate
    {

        public List<MissileStage> stages = new List<MissileStage>();
        public float FireAfterLaunchTime;
        public float MotorSpeed;
        public Vector3 MotorPos;
        public float LaunchTime { get; private set; }
        public int currentStage { get; private set; }
        public bool launched { get; private set; }
        public bool initited { get; private set; }


        public RigidbodyBehavior rb { get; private set; }
        public Transform transform { get; private set; }

        private bool removed;

        //base.Game.GraphicsManager.ContextualFxSystem.TriggerEvent(new FXExplosionContextualEvent(base.Game.GraphicsManager.ContextualFxSystem, eventParams, partContextData));

        public void OnFixedUpdate(float deltaTime)
        {
            if (!initited || !launched) return;
            Fixed();
        }

        public void OnUpdate(float deltaTime)
        {
            if (rb == null || transform == null) return;
            if (!initited) return;
            if(launched) LaunchTime += deltaTime;

            Update();
        }

        public virtual void CheckStaging()
        {

            if (rb == null || transform == null) return;
            if (!initited) return;
            if(!launched) return;

            foreach (MissileStage s in stages)
            {
                switch (s.stageType)
                {
                    case 's':

                        if(s.maxSpeed != -1 && rb.Velocity.relativeVelocity.vector.magnitude >= s.maxSpeed)
                        {
                            s.stage.Invoke(s);
                        }
                        else if(s.minSpeed != -1 && rb.Velocity.relativeVelocity.vector.magnitude <= s.minSpeed)
                        {
                            s.stage.Invoke(s);
                        }

                        break;

                    case 'a':

                        if (s.maxAltitude != -1 && rb.Model.SimulationObject.AltitudeFromSeaLevel >= s.maxAltitude)
                        {
                            s.stage.Invoke(s);
                        }
                        else if (s.minAltitude != -1 && rb.Model.SimulationObject.AltitudeFromSeaLevel <= s.minAltitude)
                        {
                            s.stage.Invoke(s);
                        }

                        break;

                    case 't':

                        if (s.waitTime != -1 && s.waitTime >= LaunchTime)
                        {
                            s.stage.Invoke(s);
                        }

                        break;

                    case 'r':
                        if (s.range != -1 && Vector3.Distance(transform.position, s.targetRange) <= s.range)
                        {
                            s.stage.Invoke(s);
                        }

                        break;
                }

            }
        }

        public virtual void Init(RigidbodyBehavior rb, Transform transform, List<MissileStage> stages, float EngineForce, Vector3 EnginePos, float LaunchDelay)
        {
            this.rb = rb;
            this.transform = transform;
            this.stages = stages;
            this.MotorPos = EnginePos;
            this.MotorSpeed = EngineForce;
            this.FireAfterLaunchTime = LaunchDelay;
            this.currentStage = 0;

            this.initited = true;
            this.launched = true;
            
            rb.Model.SimulationObject.onComponentRemoved += (t, ObjectComponent) =>
            {
                if (removed) return;
                ArmorysticksMod.Instance.Game.UnregisterFixedUpdate(this);
                ArmorysticksMod.Instance.Game.UnregisterUpdate(this);
                Armorysticks.Logger.Log("UNREGISTERED MISSILE");
                ArmorysticksMod.Instance.LaunchedMissiles.Remove(this);
                removed = true;
                Removed();
            };

            ArmorysticksMod.Instance.Game.RegisterFixedUpdate(this);
            ArmorysticksMod.Instance.Game.RegisterUpdate(this);
            Armorysticks.Logger.Log("REGISTERED MISSILE");

        }

        public virtual void Fixed()
        {
            if (rb == null || transform == null) return;
            Debug.DrawLine(rb.transform.position * 10, MotorPos, Color.red);
            IForce f = new HandOfGodForces.GodForce(rb.Model, ForceType.Acceleration, transform.forward * MotorSpeed, MotorPos, Vector3.zero);

            if (rb != null && transform != null)
            {
                rb.Model.AddForce(f);
            }

        }

        public virtual void Removed()
        {
            
        }

        public virtual void Update()
        {

        }
    }
}
