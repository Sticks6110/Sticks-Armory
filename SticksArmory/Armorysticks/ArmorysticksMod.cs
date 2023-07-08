using UnityEngine;
using TMPro;
using UnityEngine.UI;
using KSP.Messages;
using KSP.Sim.impl;
using SticksArmory.Armorysticks;
using SticksArmory.Armorysticks.Missile;
using KSP.VFX;
using KSP.Rendering.Planets;
using BepInEx;
using SpaceWarp;
using SpaceWarp.API.Mods;
using SticksArmory.Armorysticks.FXEvents;
using HarmonyLib;
using KSP.OAB;
using SticksArmory.Patch;
using SticksArmory.Modules;
using KSP.Game;
using SticksArmory.Armorysticks.Patch;
using SticksArmory.Armorysticks.Monobehaviors;
using SpaceWarp.API.Parts;
using static KSP.Api.UIDataPropertyStrings.View.Vessel.Stages;
using KSP.Networking.OnlineServices.Telemetry;
using System.Runtime.InteropServices;
using KSP.Audio;

namespace Armorysticks
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class ArmorysticksMod : BaseSpaceWarpPlugin
    {
        
        public const string ModGuid = "com.github.sticks.sticksarmory";
        public const string ModName = "Sticks Armory";
        public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

        private bool uiLoaded = false;
        private bool settingsOpen = false;

        private GameObject MenuButton;

        public static ArmorysticksMod Instance;

        public AssetBundle effects;

        //https://github.com/Halbann/LazyOrbit/blob/master/LazyOrbit/LazyOrbit.cs
        public static bool ValidScene => validScenes.Contains(GameManager.Instance.Game.GlobalGameState.GetState());
        private static GameState[] validScenes = new[] { GameState.FlightView };

        public KSP.Game.GameInstance GAME { get { return Game; } }

        public static string Path { get; private set; }
        public uint audioID;

        public void OnApplicationQuit()
        {
            SticksArmory.Armorysticks.Logger.Closing();
        }

        public override void OnPreInitialized()
        {
            Path = PluginFolderPath;

            SticksArmory.Armorysticks.Logger.Log("Patching");

            Harmony.CreateAndPatchAll(typeof(ArmorysticksMod).Assembly);

        }

        public override void OnInitialized()
        {

            Instance = this;
            JSONSave.LoadAllParts();
            JSONSave.LoadAllWeapons();
            Radar.Initialize();

            SticksArmory.Armorysticks.Logger.Log("Getting Bundles");

            effects = AssetBundleLoader.LoadBundle("effects");

            SticksArmory.Armorysticks.Logger.Log("Loading WWISE");

            byte[] bytes = File.ReadAllBytes(BepInEx.Paths.PluginPath + @"/armorysticks/assets/audio/STArmory.bnk");
            SticksArmory.Armorysticks.Logger.Log(AkSoundEngine.LoadBankMemoryView(GCHandle.Alloc(bytes, GCHandleType.Pinned).AddrOfPinnedObject(), (uint)bytes.Length, out uint bankId).ToString());
            audioID = bankId;
            SticksArmory.Armorysticks.Logger.Log(audioID);
            AkSoundEngine.SetRTPCValue("Volume_Explosions", 1000);
            AkSoundEngine.SetRTPCValue("Volume_Engines", 100);

            SticksArmory.Armorysticks.Logger.Log("Sticks Armory Loaded");
            SticksArmory.Armorysticks.Logger.Log("LOG LOCATION: " + BepInEx.Paths.PluginPath + @"/armorysticks/log.txt");

        }

        public void Update()
        {
            if(uiLoaded == false)
            {
                CreateMainMenuItem();
            }

            //Game.UniverseModel.GetAllVessels().ForEach(v => { v.OnVesselDestroyed});

        }

        public void OnGUI()
        {
            SettingsWindow.OnGUI();
        }

        public void CreateMainMenuItem()
        {
            if(GameObject.Find("MenuItemsGroup") is GameObject gobj)
            {

                GameObject g = gobj.GetChild("Singleplayer");

                GameObject btn = Instantiate(g, gobj.transform);

                TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();
                btnText.text = "Sticks Armory";
                Destroy(btn.GetComponentInChildren<UIAction_Void_Button>());
                btn.GetComponentInChildren<UIAction_Void_Button>().button.onClick.AddListener(SettingsWindow.SettingsMenuOpened);
                
                uiLoaded = true;

            }
        }

    }

}