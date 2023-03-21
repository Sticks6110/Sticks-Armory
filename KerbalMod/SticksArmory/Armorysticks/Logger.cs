using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks
{
    internal class Logger : MonoBehaviour
    {

        public static List<string> stack = new List<string>();

        public static void Log(object o)
        {
            string t = "[STICKS ARMORY] >> " + o.ToString();
            Debug.Log(t);
            stack.Add(t);
        }

        public static void Closing()
        {
            Log("Saving Log");
            System.IO.File.WriteAllText(BepInEx.Paths.PluginPath + @"\armorysticks\log.txt", String.Join("\n", stack));
        }

    }
}
