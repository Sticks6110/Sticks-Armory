using Armorysticks;
using KSP.Sim.impl;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace SticksArmory.Armorysticks.Missile
{
    public class ActiveRadar : MonoBehaviour
    {

        public Missile missile;

        public float radius;
        public bool jammed;

        public VesselComponent VesselToTrack;
        

        public void Update()
        {
            //VesselsInRange = ArmorysticksMod.Instance.GetUniverse().GetAllVesselsInRange(missile.parent.Model.PartOwner.transform.Position, 19).ToArray();
        }

    }
}
