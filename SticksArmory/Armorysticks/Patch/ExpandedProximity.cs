using HarmonyLib;
using KSP.Sim;
using KSP.Sim.impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace SticksArmory.Armorysticks.Patch
{
    //[HarmonyPatch(typeof(UniverseView), nameof(UniverseView.LoadUnloadProximityViewObjects))]
    public class ExpandedProximity
    {

        /*public static bool Prefix(UniverseView __instance, Position position, Action<bool> loadFinishedCallback = null)
        {
            __instance.LoadUnloadProximityViewObjects(position, 10000000000, loadFinishedCallback);
            return false;
        }*/

    }
}
