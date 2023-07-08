using Armorysticks;
using KSP.Game;
using KSP.Sim.impl;
using SticksArmory.Armorysticks.Monobehaviors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks
{
    public class SettingsWindow
    {

        public static bool settingsOpen = false;
        private static Rect rect = new Rect(Screen.width / 2, Screen.height / 2, 0, 0);

        private static void PopulateWindow(int windowID)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));

        }

        public static void OnGUI()
        {
            if (!settingsOpen || !ArmorysticksMod.ValidScene) return;
            GUI.skin = SpaceWarp.API.UI.Skins.ConsoleSkin;
            rect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), rect, PopulateWindow, "Settings", GUILayout.Height(2 * (Screen.height / 3)), GUILayout.Width(2 * (Screen.width / 3)));
        }

        public static void SettingsMenuOpened()
        {
            settingsOpen = !settingsOpen;
        }

    }
}
