using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks
{
    public class AssetBundleLoader
    {

        public static AssetBundle LoadBundle(string name)
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(BepInEx.Paths.PluginPath + @"/armorysticks/assets/bundles/" + name);
            if (bundle == null)
            {
                SticksArmory.Armorysticks.Logger.Log("Failed to load AssetBundle!");
                return null;
            }
            return bundle;
        }

    }
}
