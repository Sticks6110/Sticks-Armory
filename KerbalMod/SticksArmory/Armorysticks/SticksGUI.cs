using SpaceWarp;
using SpaceWarp.API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace SticksArmory.Armorysticks
{
    public class SticksGUI : MonoBehaviour
    {

        /*private Rect rect = new Rect((250 * 0.85f) - (Screen.width / 2), (Screen.height / 2) - (700 / 2), 0, 0);

        public void OnGUI()
        {

            GUI.skin = SpaceWarpManager.Skin;
            rect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), rect, PopulateWindow, "Weapons Manager", GUILayout.Height(700), GUILayout.Width(250));

        }

        private int selection;
        private List<string> weapons = new List<string>()
        {
            "Fatman",
            "Littleboy",
            "Hellfire",
            "Aim 120 AMRAAM",
            "Aim 9x",
            "Meteor AAM-5",
            "ASM1",
            "ASM2",
            "ASM3",
            "Exocet",
            "RBS 15",
            "Rb 05",
        };

        private void PopulateWindow(int windowID)
        {

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            if (GUI.Button(new Rect(10, 6, 230, 18), "<b><color=red>LAUNCH</color></b>", new GUIStyle(GUI.skin.button) { fontSize = 16 }))
            {
                SticksArmory.Armorysticks.Logger.Log("CLICKED");
            }

            GUI.Label(new Rect(10, 42, 230, 18), "<color=purple>SELECT WEAPON</color>", new GUIStyle(GUI.skin.button) { fontSize = 14 });
            selection = (int)GUI.HorizontalSlider(new Rect(10, 66, 230, 18), selection, 0, weapons.Count - 1);
            GUI.Label(new Rect(10, 90, 230, 18), "<color=green>" + weapons[selection] + "</color>", new GUIStyle(GUI.skin.button) { fontSize = 14 });


            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));

        }*/

    }
}
