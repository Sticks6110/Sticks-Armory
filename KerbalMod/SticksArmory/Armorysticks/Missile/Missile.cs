using Armorysticks;
using AwesomeTechnologies.Utility;
using KSP.Game;
using KSP.Rendering.Planets;
using KSP.Sim;
using KSP.Sim.impl;
using KSP.VFX;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static KSP.Rendering.Planets.PQJob;

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

        public PartBehavior parent;
        public AudioSource audio;

        public WeaponJSONSaveData data;

        //base.Game.GraphicsManager.ContextualFxSystem.TriggerEvent(new FXExplosionContextualEvent(base.Game.GraphicsManager.ContextualFxSystem, eventParams, partContextData));

        private RigidbodyBehavior rb;
        private float secondsTillDry;
        private float secondsSinceLaunch;

        private Camera cam;

        private RenderTexture renderTexture;

        private Rect rect = new Rect((Screen.width / 2), (Screen.height / 2), 0, 0);

        private bool launched;

        public void OnGUI()
        {
            if(!launched) return;
            GUI.skin = SpaceWarp.API.UI.Skins.ConsoleSkin;
            rect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), rect, PopulateWindow, parent.GetDisplayName(), GUILayout.Height(270), GUILayout.Width(250));

        }

        private void PopulateWindow(int windowID)
        {

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            GUI.DrawTexture(new Rect(10, 30, 230, 230), renderTexture);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));

        }

        public void Update()
        {
            if (!launched) return;

            secondsSinceLaunch += Time.deltaTime;
        }

        public void Explode(Type t, ObjectComponent c)
        {
            string[] effects = data.ExplosionEffect.Split(char.Parse(","));
            string efct = effects[UnityEngine.Random.Range(0, effects.Length - 1)];
            GameObject prefab = ArmorysticksMod.Instance.effects.LoadAsset<GameObject>(efct);
            Instantiate(prefab, transform.position, transform.rotation);

            Armorysticks.Logger.Log("EXPLOSION");
            Destroy(this);
            
        }

        public void Launch()
        {
            rb = GetComponent<RigidbodyBehavior>();
            secondsTillDry = operationalRange / (maxSpeed / 1000); //Will change later for rockets to accelerate linearly or exponentially but for now they stay at a constant speed

            GameObject g = gameObject.GetChild("Camera");
            g.SetActive(true);

            Camera cam = g.GetComponent<Camera>();
            cam.forceIntoRenderTexture = true;

            renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
            renderTexture.Create();
            
            cam.targetTexture = renderTexture;

            cam.gameObject.SetActive(true);

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
