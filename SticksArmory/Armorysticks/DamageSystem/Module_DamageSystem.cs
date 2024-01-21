using Armorysticks;
using KSP.Modules;
using KSP.Sim;
using KSP.Sim.Definitions;
using KSP.Sim.DeltaV;
using KSP.Sim.impl;
using SticksArmory.Armorysticks.Missile;
using SticksArmory.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SticksArmory.Armorysticks.DamageSystem
{
    [DisallowMultipleComponent]
    public class Module_DamageSystem : PartBehaviourModule
    {
        public override Type PartComponentModuleType
        {
            get
            {
                return typeof(PartComponentModule_DamageSystem);
            }
        }

        [SerializeField]
        protected Data_DamageSystem dataDamage = new Data_DamageSystem();

        public override void OnUpdate(float deltaTime)
        {
            if (base.PartBackingMode != PartBackingModes.Flight) return;
            if(dataDamage.OnFire)
            {
                if(dataDamage.Armor != 0)
                {
                    dataDamage.Health--;
                    CheckArmorAndHealth();
                }
            }

            if(dataDamage.CheckDamageChanges)
            {
                CheckArmorAndHealth();
            }
        }

        public override void OnInitialize()
        {
            SticksArmory.Armorysticks.Logger.Log(part.Name);
        }

        private void DamageComponents()
        {
            if (part.Model.IsPartEngine(out Data_Engine en_dat))
            {
                en_dat.thrustCurveRatio = en_dat.thrustCurveRatio / 2;
            }

            if (part.Model.IsPartAirIntake(out Data_ResourceIntake ri_dat))
            {
                ri_dat.intakeSpeed = ri_dat.intakeSpeed / 2;
            }

            if (part.Model.TryGetModuleData<PartComponentModule_LiftingSurface, Data_LiftingSurface>(out Data_LiftingSurface ls_dat))
            {
                float liftScale = ls_dat.LiftScalar.GetValue();
                ls_dat.LiftScalar.SetValue(liftScale / 2);
            }
        }

        private void CheckArmorAndHealth()
        {
            if (dataDamage.Health < 0)
            {
                part.Model.DestroyPart(KSP.Sim.ExplosionType.Default);
            }

            if (dataDamage.Armor < 0)
            {
                DamageComponents();
                dataDamage.Health -= MathF.Abs(dataDamage.Armor);
                if (dataDamage.Health < 0)
                {
                    part.Model.DestroyPart(KSP.Sim.ExplosionType.Default);
                }
            }
        }

    }
}
