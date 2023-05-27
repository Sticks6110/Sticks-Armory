using Armorysticks;
using KSP.Game;
using KSP.Iteration.UI.Binding;
using KSP.Sim;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using SpaceWarp.API.Assets;
using SticksArmory.Armorysticks.Patch;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks.Monobehaviors
{
    public class Radar : MonoBehaviour
    {

        public static Dictionary<VesselComponent, Blip> VesselLocks = new Dictionary<VesselComponent, Blip>();
        private Blip LocalLock;

        public bool Show = false;
        public bool WarningRadar = false; //RWRS
        public PartComponent parent;
        public PartBehavior parentBehaviour;
        public PartBehaviourModule parentBehaviourModule;
        public PartJSONSaveData data;

        public List<Blip> InRange = new List<Blip>();

        private Rect rect = new Rect(Screen.width / 2, Screen.height / 2, 0, 0);

        private static Texture2D RadarTexture;
        private static Texture2D EnemyTexture;
        private static Texture2D FriendlyTexture;
        private static Texture2D LockedEnemyTexture;
        private static Texture2D IncomingTexture;
        private static Texture2D TargetTexture;
        private static Texture2D ExclamationTexture;

        private float TimeLast;

        public static void Initialize()
        {

            RadarTexture = new Texture2D(230, 230);
            RadarTexture.LoadImage(File.ReadAllBytes($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar\Radar.png"));

            EnemyTexture = new Texture2D(100, 100);
            EnemyTexture.LoadImage(File.ReadAllBytes($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar\Enemy.png"));

            FriendlyTexture = new Texture2D(100, 100);
            FriendlyTexture.LoadImage(File.ReadAllBytes($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar\Friendly.png"));

            LockedEnemyTexture = new Texture2D(100, 100);
            LockedEnemyTexture.LoadImage(File.ReadAllBytes($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar\LockedEnemy.png"));

            IncomingTexture = new Texture2D(100, 100);
            IncomingTexture.LoadImage(File.ReadAllBytes($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar\MissileIncoming.png"));

            TargetTexture = new Texture2D(100, 100);
            TargetTexture.LoadImage(File.ReadAllBytes($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar\Target.png"));

            ExclamationTexture = new Texture2D(50, 50);
            ExclamationTexture.LoadImage(File.ReadAllBytes($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar\Exclamation.png"));

            Armorysticks.Logger.Log("SUCCESFULLY LOADED TEXTURES!");
            //RadarTexture =  AssetManager.GetAsset<Texture2D>($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar.png");
            //BlipTexture =   AssetManager.GetAsset<Texture2D>($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Blip.png");
        }

        public void OnGUI()
        {
            if (!Show || !ArmorysticksMod.ValidScene) return;

            GUI.skin = SpaceWarp.API.UI.Skins.ConsoleSkin;
            rect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), rect, PopulateWindow, "Radar", GUILayout.Height(270), GUILayout.Width(250));
        }

        public void Update()
        {

            VesselComponent activeVessel = ArmorysticksMod.Instance.GAME.ViewController.GetActiveSimVessel(true);

            if (!ArmorysticksMod.ValidScene || !Show) return;

            if(parentBehaviourModule.vessel.Model != activeVessel)
            {
                Show = false;
                return;
            }

            TimeLast += Time.deltaTime;

            if(TimeLast >= data.RadarUpdate)
            {
                TimeLast = 0;

                InRange = new List<Blip>();
                IEnumerable<VesselComponent> vessels = ArmorysticksMod.Instance.GAME.UniverseModel.GetAllVesselsInRange(activeVessel.transform.Position, data.RadarRadius);
                foreach (VesselComponent ve in vessels)
                {

                    if(ve == activeVessel) continue;

                    Vector3 local = (activeVessel.transform.Position - ve.transform.Position).vector;

                    Vector2 hudPos = new Vector3(map(local.x, -data.RadarRadius, data.RadarRadius, -115, 115) + 115, map(local.z, -data.RadarRadius, data.RadarRadius, -115, 115) + 115);

                    InRange.Add(new Blip()
                    {
                        vessel = ve,
                        pos = ve.transform.Position,
                        hudPos = hudPos,
                    });
                }

            }

        }

        private void PopulateWindow(int windowID)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            UnityEngine.GUI.DrawTexture(new Rect(10, 30, 230, 230), RadarTexture);

            foreach (Blip b in InRange)
            {
                Rect r = new Rect(b.hudPos.x + 10 - 12.5f, b.hudPos.y + 30 - 12.5f, 25, 25);

                Texture2D t;

                if(LocalLock != null)
                {
                    if (b.vessel.GlobalId == LocalLock.vessel.GlobalId) t = TargetTexture;
                    else t = EnemyTexture;
                }
                else
                {
                    t = EnemyTexture;
                }

                if(UnityEngine.GUI.Button(r, t, GUIStyle.none))
                {
                    LocalLock = b;

                    VesselComponent activeVessel = ArmorysticksMod.Instance.GAME.ViewController.GetActiveSimVessel(true);

                    VesselLocks[activeVessel] = b;
                    GameManager.Instance.Game.Notifications.ProcessNotification(new NotificationData
                    {
                        Tier = NotificationTier.Passive,
                        Primary = new NotificationLineItemData { LocKey = $"Locked Onto {LocalLock.vessel.Name}" }
                    });
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));

        }

        public float map(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

    }

    public class Blip
    {
        public VesselComponent vessel;
        public Position pos;
        public Vector2 hudPos;
    }

}
