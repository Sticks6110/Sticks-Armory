using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using I2.Loc;
using KSP;
using KSP.OAB;
using RTG;
using SticksArmory.Armorysticks;

namespace SticksArmory.Patch
{
    [HarmonyPatch(typeof(ObjectAssemblyFlexibleModal), nameof(ObjectAssemblyFlexibleModal.GetPriorityInfoFromPart))]
    class PrioriteInfo
    {

        public static void Postfix(ref List<KeyValuePair<string, string>> __result, IObjectAssemblyAvailablePart IOBAPart)
        {

            if (JSONSave.Launchables.ContainsKey(IOBAPart.PartData.partName))
            {
                WeaponJSONSaveData data = JSONSave.Launchables[IOBAPart.PartData.partName];
                __result.Add(new KeyValuePair<string, string>("Type", data.TextType));
                __result.Add(new KeyValuePair<string, string>("Guidance", data.TextGuidance));
                __result.Add(new KeyValuePair<string, string>("Warhead", data.TextWarhead));
                __result.Add(new KeyValuePair<string, string>("Detonation", data.TextDetonation));
                __result.Add(new KeyValuePair<string, string>("Tnt Equivelent", data.TextTntEquivilant));
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
