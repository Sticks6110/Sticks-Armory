using System;
using System.Collections.Generic;
using System.Text;
using System;
using KSP.Sim;
using KSP.Sim.Definitions;
using UnityEngine;
using UnityEngine.Serialization;
using I2.Loc;
using KSP.Modules;
using HarmonyLib;
using System.Runtime.CompilerServices;
using SticksArmory.Armorysticks;

namespace SticksArmory.Modules
{

    [Serializable]
    public class Data_Missile : ModuleData
    {

        public static Dictionary<ModuleData, Data_Missile> PatchInstances = new Dictionary<ModuleData, Data_Missile>();

        public override Type ModuleType
        {
            get
            {
                return typeof(Data_Missile);
            }
        }

        [LocalizedField("STArmory/Modules/Missile/Data/Launch")]
        [KSPState(CopyToSymmetrySet = true)]
        public ModuleProperty<bool> IsDeployed = new ModuleProperty<bool>(false, true);

        [LocalizedField("STArmory/Modules/Missile/Data/Camera")]
        [KSPState(CopyToSymmetrySet = true)]
        public ModuleProperty<bool> CameraEnabled = new ModuleProperty<bool>(false, true);

        [LocalizedField("STArmory/Modules/Missile/Data/DropToFireTime")]
        [PAMDisplayControl(SortIndex = 2)]
        [SteppedRange(1f, 5f, 1f)]
        [KSPState(CopyToSymmetrySet = true)]
        public ModuleProperty<float> DropToFire = new ModuleProperty<float>(1);

        [KSPDefinition]
        public string JSONId = "name.json";

        private WeaponJSONSaveData UncipheredJson = null;

        public void LoadJson()
        {
            UncipheredJson = JSONSave.Launchables[JSONId];

        }

        public override void Copy(ModuleData sourceModuleData)
        {

            Data_Missile data_Missile = (Data_Missile)sourceModuleData;
            if (data_Missile != null)
            {
                DropToFire.SetValue(data_Missile.DropToFire.GetValue());
            }

        }

        /*public override List<OABPartData.PartInfoModuleEntry> GetPartInfoEntries(Type partBehaviourModuleType, List<OABPartData.PartInfoModuleEntry> delegateList)
        {
            SticksArmory.Armorysticks.Logger.Log("M");
            if (partBehaviourModuleType == ModuleType)
            {
                SticksArmory.Armorysticks.Logger.Log("MM");
                delegateList.Add(new OABPartData.PartInfoModuleEntry(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Type"), (OABPartData.PartInfoModuleEntryValueDelegate)_GetType));
                delegateList.Add(new OABPartData.PartInfoModuleEntry(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Guidance"), (OABPartData.PartInfoModuleEntryValueDelegate)_GetGuidance));
                delegateList.Add(new OABPartData.PartInfoModuleEntry(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Warhead"), (OABPartData.PartInfoModuleEntryValueDelegate)_GetWarhead));
                delegateList.Add(new OABPartData.PartInfoModuleEntry(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Detonation"), (OABPartData.PartInfoModuleEntryValueDelegate)_GetDetonation));
                delegateList.Add(new OABPartData.PartInfoModuleEntry(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Tnt"), (OABPartData.PartInfoModuleEntryValueDelegate)_GetTnt));
                delegateList.Add(new OABPartData.PartInfoModuleEntry(LocalizationManager.GetTranslation("STArmory/Modules/Missile/Tooltip/Origin"), (OABPartData.PartInfoModuleEntryValueDelegate)_GetOrigin));
            }
            SticksArmory.Armorysticks.Logger.Log("MMM");
            return delegateList;
        }*/

    }
}
