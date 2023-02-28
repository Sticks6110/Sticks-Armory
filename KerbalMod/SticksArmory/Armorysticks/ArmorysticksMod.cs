using SpaceWarp.API.Mods;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Armorysticks
{
    [MainMod]
    public class ArmorysticksMod : Mod
    {

        public override void Initialize()
        {
            Logger.Info("Sticks Armory Loaded");
        }

        public void Update()
        {

        }
    }

}