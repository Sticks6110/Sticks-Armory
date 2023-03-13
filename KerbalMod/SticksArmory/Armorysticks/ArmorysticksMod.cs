using SpaceWarp.API.Mods;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using KSP.Api;
using System.Collections;
using KSP.Messages;
using KSP.Sim.impl;
using KSP.Sim.DeltaV;
using KSP.Modules;
using KSP.Sim.Definitions;
using SticksArmory.Armorysticks;
using I2.Loc;
using KSP.Game;
using KSP.Sim;
using System.Drawing;
using SpaceWarp.API;
using System.Runtime.CompilerServices;
using System;
using SticksArmory.Armorysticks.Missile;
using KSP.VFX;

namespace Armorysticks
{
    [MainMod]
    public class ArmorysticksMod : Mod
    {

        public List<Missile> LaunchedMissiles = new List<Missile>();

        private bool uiLoaded = false;
        private SticksGUI gui;

        private GameObject MenuButton;
        private GameObject MenuSettings;

        public static ArmorysticksMod Instance;

        public static SpaceSimulation simulation;

        public override void Initialize()
        {
            Instance = this;
            SticksArmory.Armorysticks.Logger.Log("Sticks Armory Loaded");
            SticksArmory.Armorysticks.Logger.Log("LOG LOCATION: " + SpaceWarp.API.SpaceWarpManager.MODS_FULL_PATH + @"/armorysticks/log.txt");
        }

        public void OnApplicationQuit()
        {

            SticksArmory.Armorysticks.Logger.Closing();
        }

        public void Awake()
        {
            /*GameObject g = new GameObject("WeaponGUI");
            gui = g.AddComponent<SticksGUI>();
            DontDestroyOnLoad(g);*/

            base.Game.Messages.Subscribe<DecoupleMessage>((m) => { LaunchMissile(m); });
            //GameManager/Default Game Instance(Clone)/UI Manager(Clone)/Main Canvas/MainMenu(Clone)/MenuItemsGroup/Singleplayer/
        }

        public void Update()
        {

            if(uiLoaded == false)
            {
                CreateMainMenuItem();
            }
        }

        public void LaunchMissile(MessageCenterMessage m)
        {
            
            DecoupleMessage decoupled = (DecoupleMessage)m;
            LaunchDetection.Launched(decoupled.PartGuid);
        }

        public void LoadSimulation()
        {
            simulation = base.Game.SpaceSimulation;
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
                btn.GetComponentInChildren<UIAction_Void_Button>().button.onClick.AddListener(SettingsMenuOpened);

                MenuSettings = new GameObject("SticksArsenalSettigns");
                MenuSettings.transform.parent = GameObject.Find("Main Canvas").transform;
                MenuSettings.AddComponent<CanvasRenderer>();
                MenuSettings.AddComponent<Image>().color = new Color32(42, 42, 42, 255);
                MenuSettings.transform.localScale = new Vector3(12, 8, 1);

                GameObject headerHolder = new GameObject("Header");
                headerHolder.transform.parent = MenuSettings.transform;
                headerHolder.AddComponent<CanvasRenderer>();
                headerHolder.AddComponent<Image>().color = new Color32(67, 67, 67, 255);
                headerHolder.transform.localScale = new Vector3(1, .075f, 1);
                headerHolder.transform.position = new Vector3(0, 350, 0);

                GameObject headerText = new GameObject("HeaderText");
                headerText.transform.parent = headerHolder.transform;
                headerText.AddComponent<CanvasRenderer>();
                TMP_Text htext = (TMP_Text) CopyComponent(btnText, headerText);
                htext.text = "Sticks Arsenal";
                htext.fontSize = 48f;
                htext.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
                headerText.transform.localScale = new Vector3(0.12f, 2, 0);
                headerText.transform.localPosition = new Vector3(0, 0, 0);
                

                MenuSettings.SetActive(false);

                uiLoaded = true;

            }
        }

        //https://answers.unity.com/questions/458207/copy-a-component-at-runtime.html
        Component CopyComponent(Component original, GameObject destination)
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }

        public void SettingsMenuOpened()
        {
            MenuSettings.SetActive(!MenuSettings.activeSelf);
        }

    }

}