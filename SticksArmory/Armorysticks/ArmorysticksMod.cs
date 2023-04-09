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

namespace Armorysticks
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class ArmorysticksMod : BaseSpaceWarpPlugin
    {
        
        public const string ModGuid = "com.github.sticks.sticksarmory";
        public const string ModName = "Sticks Armory";
        public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

        private bool uiLoaded = false;

        private GameObject MenuButton;
        private GameObject MenuSettings;

        public static ArmorysticksMod Instance;

        public AssetBundle effects;
        public AssetBundle audio;

        public static string Path { get; private set; }

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
            effects = AssetBundleLoader.LoadBundle("effects");
            audio = AssetBundleLoader.LoadBundle("audio");
            SticksArmory.Armorysticks.Logger.Log("Sticks Armory Loaded");
            SticksArmory.Armorysticks.Logger.Log("LOG LOCATION: " + BepInEx.Paths.PluginPath + @"/armorysticks/log.txt");

        }

        public void Update()
        {

            if(uiLoaded == false)
            {
                CreateMainMenuItem();
            }
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

                MenuSettings = new GameObject("SticksArmorylSettigns");
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
                htext.text = "Sticks Armory";
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