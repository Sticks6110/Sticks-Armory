using KSP.Sim;
using KSP.Sim.impl;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Armorysticks;
using System.Linq;
using UnityEngine;
using KSP.Game;

namespace SticksArmory.Armorysticks
{
    public class LaunchDetection : MonoBehaviour
    {

        public static async void Launched(string GUID)
        {

            if(IGGuid.TryParse(GUID, out IGGuid guid))
            {
                Armorysticks.Logger.Log("SUCCESS");
                if (guid != default(IGGuid))
                {
                    Armorysticks.Logger.Log("EXIST");
                    Armorysticks.Logger.Log(GUID);
                    SimulationObjectModel simObj = ArmorysticksMod.Instance.GetSim().FindSimObject(guid);

                    if (!JSONSave.Launchables.ContainsKey(simObj.Name)) return;

                    SimulationObjectView simulationObjectView = GameManager.Instance.Game.SpaceSimulation.ModelViewMap.FromModel(simObj);

                    Armorysticks.Logger.Log("LAUNCHING");

                    WeaponJSONSaveData d = JSONSave.Launchables[simObj.Name];

                    if(simulationObjectView != null)
                    {
                        AudioSource a = simulationObjectView.gameObject.AddComponent<AudioSource>();
                        Missile.Missile m = simulationObjectView.gameObject.AddComponent<Missile.Missile>();
                        m.maxSpeed = d.MaxSpeed;
                        m.operationalRange = d.OperationalRange;
                        m.parent = simulationObjectView.Part;
                        m.audio = a;
                        m.data = d;
                        simulationObjectView.Model.onComponentRemoved += m.Explode;

                        m.Launch();
                    }
                    
                    
                }
            }

            /*if(GUID != default(IGGuid))
            {
                Armorysticks.Logger.Log("EXIST");
                Armorysticks.Logger.Log(GUID);
                SimulationObjectModel simObj = ArmorysticksMod.simulation.FindSimObject(GUID);
                

            }*/
        }

        private static PartComponent GetPartComponentFromGuid(IGGuid instanceGuid, ISimulationModelMap simulationModelMap)
        {
            SimulationObjectModel simulationObjectModel = simulationModelMap.FromGlobalId(instanceGuid);
            if (simulationObjectModel == null)
            {
                Armorysticks.Logger.Log("Requested Attachment Object `{0}` does not exist " + instanceGuid);
                return null;
            }
            PartComponent partComponent = simulationObjectModel.FindComponent<PartComponent>();
            if (partComponent == null)
            {
                Armorysticks.Logger.Log("Requested Attachment Object `{0}` Does not contain a PartComponent " + instanceGuid);
                return null;
            }
            return partComponent;
        }

    }
}
