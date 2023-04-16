using Armorysticks;
using KSP.Iteration.UI.Binding;
using KSP.Sim;
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

        public bool Show = false;
        public bool WarningRadar = false; //RWRS
        public PartComponent parent;
        public PartJSONSaveData data;

        public List<Position> InRange = new List<Position>();

        private List<Vector2> VesselHudPos = new List<Vector2>();

        private Rect rect = new Rect(Screen.width / 2, Screen.height / 2, 0, 0);

        private static Texture2D RadarTexture;
        private static Texture2D BlipTexture;

        private float TimeLast;

        public static void Initialize()
        {
            Armorysticks.Logger.Log($@"LOADING TEXTURE: {BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar.png");
            RadarTexture = new Texture2D(230, 230);
            RadarTexture.LoadImage(File.ReadAllBytes($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar.png"));

            Armorysticks.Logger.Log($@"LOADING TEXTURE: {BepInEx.Paths.PluginPath}\armorysticks\assets\images\Blip.png");
            BlipTexture = new Texture2D(25, 25);
            BlipTexture.LoadImage(File.ReadAllBytes($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Blip.png"));

            Armorysticks.Logger.Log("SUCCESFULLY LOADED TEXTURES!");
            //RadarTexture =  AssetManager.GetAsset<Texture2D>($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Radar.png");
            //BlipTexture =   AssetManager.GetAsset<Texture2D>($@"{BepInEx.Paths.PluginPath}\armorysticks\assets\images\Blip.png");
        }

        public void OnGUI()
        {
            if (!Show) return;

            GUI.skin = SpaceWarp.API.UI.Skins.ConsoleSkin;
            rect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), rect, PopulateWindow, "Radar", GUILayout.Height(270), GUILayout.Width(250));
        }

        public void Update()
        {
            TimeLast += Time.deltaTime;

            if(TimeLast >= data.RadarUpdate)
            {
                TimeLast = 0;

                VesselComponent activeVessel = ArmorysticksMod.Instance.GAME.ViewController.GetActiveSimVessel(true);

                InRange = new List<Position>();
                VesselHudPos = new List<Vector2>();
                IEnumerable<VesselComponent> vessels = ArmorysticksMod.Instance.GAME.UniverseModel.GetAllVesselsInRange(activeVessel.transform.Position, data.RadarRadius);
                foreach (VesselComponent ve in vessels)
                {
                    InRange.Add(ve.transform.Position);
                }

                foreach (VesselComponent ve in vessels)
                {

                    Vector3 local = (activeVessel.transform.Position - ve.transform.Position).vector;

                    Vector2 hudPos = new Vector3(map(local.x, -data.RadarRadius, data.RadarRadius, -115, 115) + 115, map(local.z, -data.RadarRadius, data.RadarRadius, -115, 115) + 115);

                    VesselHudPos.Add(hudPos);
                }

            }

        }

        private void PopulateWindow(int windowID)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            UnityEngine.GUI.DrawTexture(new Rect(10, 30, 230, 230), RadarTexture);

            foreach (Vector2 vec in VesselHudPos)
            {
                Armorysticks.Logger.Log($"ADDING BLIP AT {vec}");
                UnityEngine.GUI.DrawTexture(new Rect(vec.x + 10, vec.y + 30, 25, 25), BlipTexture);
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
}
