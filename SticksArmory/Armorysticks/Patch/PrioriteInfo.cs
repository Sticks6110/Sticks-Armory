using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using I2.Loc;
using KSP;
using KSP.OAB;
using KSP.Sim.impl;
using KSP.UI;
using RTG;
using SticksArmory.Armorysticks;

namespace SticksArmory.Patch
{
    [HarmonyPatch(typeof(PartInfoOverlay), nameof(PartInfoOverlay.PopulateCoreInfoFromPart))]
    class PrioriteInfo
    {

        public static void Postfix(ref List<KeyValuePair<string, string>> __result, IObjectAssemblyAvailablePart IOBAPart)
        {
            if (JSONSave.Parts.ContainsKey(IOBAPart.PartData.partName))
            {
                PartJSONSaveData data = JSONSave.Parts[IOBAPart.PartData.partName];

                if(data.Radar)
                {
                    __result.Add(new KeyValuePair<string, string>("Radar", (data.RadarUpdate == 0) ? "Continuous" : "Pulse"));
                    __result.Add(new KeyValuePair<string, string>("Radar Distance", (data.RadarRadius * 2).ToString()));
                    __result.Add(new KeyValuePair<string, string>("Radar Sensitivity", data.RadarSensitivity.ToString()));
                }

            }

            if (JSONSave.Weapons.ContainsKey(IOBAPart.PartData.partName))
            {
                WeaponJSONSaveData data = JSONSave.Weapons[IOBAPart.PartData.partName];
                switch (data.PartType)
                {

                    case "missile":
                        __result.Add(new KeyValuePair<string, string>("Type", data.TextType));
                        __result.Add(new KeyValuePair<string, string>("Guidance", data.TextGuidance));
                        __result.Add(new KeyValuePair<string, string>("Warhead", data.TextWarhead));
                        __result.Add(new KeyValuePair<string, string>("Detonation", data.TextDetonation));
                        __result.Add(new KeyValuePair<string, string>("Tnt Equivelent", data.TextTntEquivilant));
                        break;

                    case "bomb":
                        __result.Add(new KeyValuePair<string, string>("Type", data.TextType));
                        __result.Add(new KeyValuePair<string, string>("Warhead", data.TextWarhead));
                        __result.Add(new KeyValuePair<string, string>("Tnt Equivelent", data.TextTntEquivilant));
                        break;

                    case "gunraycast":
                        __result.Add(new KeyValuePair<string, string>("Calliber", data.TextCalliber));
                        __result.Add(new KeyValuePair<string, string>("Fire Rate", data.TextFR));
                        break;

                    case "gunphysical":
                        __result.Add(new KeyValuePair<string, string>("Calliber", data.TextCalliber));
                        __result.Add(new KeyValuePair<string, string>("Fire Rate", data.TextFR));
                        break;

                    default:
                        break;
                }
                __result.Add(new KeyValuePair<string, string>("Origin", data.TextOrigin));
            }

            /*__result.Add(new KeyValuePair<string, string>(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Type"), "Test"));
            __result.Add(new KeyValuePair<string, string>(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Guidance"), "Test"));
            __result.Add(new KeyValuePair<string, string>(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Warhead"), "Test"));
            __result.Add(new KeyValuePair<string, string>(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Detonation"), "Test"));
            __result.Add(new KeyValuePair<string, string>(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Tnt"), "Test"));
            __result.Add(new KeyValuePair<string, string>(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Origin"), "Test"));*/
        }

    }
}
