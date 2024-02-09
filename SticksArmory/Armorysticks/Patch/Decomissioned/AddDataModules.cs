using Armorysticks;
using HarmonyLib;
using KSP.Game;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using SticksArmory.Armorysticks.Monobehaviors;
using SticksArmory.Armorysticks.Radar;
using SticksArmory.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using static KSP.Sim.Definitions.PartBehaviourModule;

namespace SticksArmory.Armorysticks.Patch
{

    /*
     * 
     *  DO NOT USE THIS, THIS IS JUST HERE FOR REFERENCE AS THIS HAS BEEN MOVED TO THE PATCH MANAGER
     * 
    */


    //AddDataModules PartBehaviourModule
    /*[HarmonyPatch(typeof(PartBehaviourModule), nameof(PartBehaviourModule.OnInitialize))]
    public class AddDataModules
    {

        private static List<PartBehavior> UniqueParts = new List<PartBehavior>();
        private static Dictionary<PartBehavior, Monobehaviors.Radar> Radars = new Dictionary<PartBehavior, Monobehaviors.Radar>();

        public static void Postfix(PartBehaviourModule __instance)
        {
            if (__instance.PartBackingMode != PartBackingModes.Flight || !ArmorysticksMod.ValidScene) return;
            if (UniqueParts.Contains(__instance.part)) return;
            if (!JSONSave.Parts.ContainsKey(__instance.part.Name)) return;

            PartJSONSaveData js = JSONSave.Parts[__instance.part.Name];

            if(js.Radar)
            {

                Monobehaviors.Radar radgui = __instance.gameObject.AddComponent<Monobehaviors.Radar>();
                radgui.parent = __instance.part.Model;
                radgui.parentBehaviour = __instance.part;
                radgui.parentBehaviourModule = __instance;
                radgui.data = js;

                Radars.Add(__instance.part, radgui);

                Data_Radar dr = new Data_Radar();

                ModuleAction _toggleRadar = new ModuleAction(new Action(() => { ToggleRadar(__instance.part); }));
                dr.AddAction("STArmory/Modules/Radar/Data/Toggle/Radar", _toggleRadar, 1);
                //STArmory/Modules/Radar/Data/Toggle/RWR
                dr.SetVisible(_toggleRadar, __instance.PartBackingMode == PartBackingModes.Flight);

                __instance.DataModules.TryAddUnique(dr, out dr);
            }

            UniqueParts.Add(__instance.part);

        }

        public static void ToggleRadar(PartBehavior part)
        {
            Radars[part].Show = !Radars[part].Show;
        }

    }*/
}
