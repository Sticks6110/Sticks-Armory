using Armorysticks;
using AwesomeTechnologies.Utility;
using KSP.Sim;
using KSP.Sim.impl;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

    public class Missile : MonoBehaviour
    {

        public List<MissileStage> stages = new List<MissileStage>();
        public float maxSpeed; //           m/s
        public float operationalRange; //   km


        //base.Game.GraphicsManager.ContextualFxSystem.TriggerEvent(new FXExplosionContextualEvent(base.Game.GraphicsManager.ContextualFxSystem, eventParams, partContextData));

        private RigidbodyBehavior rb;
        private float secondsTillDry;
        private float secondsSinceLaunch;

        private bool launched;

        public void Update()
        {
            if (!launched) return;

            secondsSinceLaunch += Time.deltaTime;
        }

        public void Launch()
        {
            rb = GetComponent<RigidbodyBehavior>();
            secondsTillDry = operationalRange / (maxSpeed / 1000); //Will change later for rockets to accelerate linearly or exponentially but for now they stay at a constant speed



            launched = true;
        }

        public void FixedUpdate()
        {
            if (!launched && secondsSinceLaunch < secondsTillDry) return;

            rb.activeRigidBody.velocity = Vector3.ClampMagnitude(rb.activeRigidBody.velocity, maxSpeed);
            rb.activeRigidBody.AddForce(transform.forward * maxSpeed, ForceMode.Acceleration);

        }

    }
}
