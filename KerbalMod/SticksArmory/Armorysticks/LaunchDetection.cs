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

            if(ArmorysticksMod.simulation == null)
            {
                Armorysticks.Logger.Log("LOADING SIMULATION");
                ArmorysticksMod.Instance.LoadSimulation();
            }

            if(IGGuid.TryParse(GUID, out IGGuid guid))
            {
                Armorysticks.Logger.Log("SUCCESS");
                if (guid != default(IGGuid))
                {
                    Armorysticks.Logger.Log("EXIST");
                    Armorysticks.Logger.Log(GUID);
                    SimulationObjectModel simObj = ArmorysticksMod.simulation.FindSimObject(guid);

                    SimulationObjectView simulationObjectView = GameManager.Instance.Game.SpaceSimulation.ModelViewMap.FromModel(simObj);
                    if(simulationObjectView == null)
                    {

                        while (simulationObjectView == null)
                        {
                            simulationObjectView = GameManager.Instance.Game.SpaceSimulation.ModelViewMap.FromModel(simObj);
                            Armorysticks.Logger.Log("WAITING TO EXIST");
                            await Task.Delay(25);
                        }

                    }

                    Armorysticks.Logger.Log("LAUNCHING");

                    if (simulationObjectView.Rigidbody != null && simulationObjectView.transform != null)
                    {

                        Armorysticks.Logger.Log("ADDING MISSILE COMPONENT");
                        Missile.Missile m = new Missile.Missile();
                        m.Init(simulationObjectView.Rigidbody, simulationObjectView.transform, new List<Missile.MissileStage>(), 250, new Vector3(0, 0, -2), 1);
                        ArmorysticksMod.Instance.LaunchedMissiles.Add(m);
                        Armorysticks.Logger.Log("MISSILE COMPONENT ADDED");

                    }
                    else
                    {

                        Armorysticks.Logger.Log("ERROR FINDING RIGIDBODY OR TRANSFORM NOT LAUNCHING MISSILE");

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
