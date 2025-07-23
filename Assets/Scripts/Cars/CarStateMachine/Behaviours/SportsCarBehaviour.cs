using System.Collections.Generic;
using UnityEngine;

public class SportsCarBehaviour : CarBehaviour
{
    [SerializeField]
    BoidSettings speedingBoidSettings;

    [SerializeField]
    float highSpeedFrontCheckDistance;

    [SerializeField]
    float fromHighspeedSlowDownDistance;

    private void Update()
    {
        if (stateMachines.Count > 0)
        {
            foreach (CarStateMachine stateMachine in stateMachines)
            {
                stateMachine.Tick();
            }
        }
    }

    public override void CreateStateMachine(CarEntity entity)
    {
        CongestionState congestionState = new CongestionState(entity, congestionBoidSettings);
        FindStreetState findStreetState = new FindStreetState(entity, findStreetBoidSettings);
        SwarmState swarmState = new SwarmState(entity, swarmingBoidSettings);
        AvoidingState avoidingState = new AvoidingState(entity, avoidingBoidSettings);

        SpeedingState speedingState = new SpeedingState(entity, speedingBoidSettings);

        Dictionary<CarState, List<CarTransition>> transitions = new Dictionary<CarState, List<CarTransition>>()
        {
            [congestionState] = new List<CarTransition>()
            {
                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity)),
                new CarTransition(swarmState, () =>  !CheckStateInDirection(entity, EStates.Congestion, entity.transform.forward) /*CONDITION for last in row frees itself:*/|| !CheckInDirection(entity,-entity.transform.forward)),
            },

            [findStreetState] = new List<CarTransition>()
            {
                new CarTransition(swarmState, () => CheckIfIsOnStreet(entity))
            },

            [swarmState] = new List<CarTransition>()
            {
                new CarTransition(speedingState, () => !CheckInDirection(entity, entity.transform.forward, highSpeedFrontCheckDistance)),

                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity)),
                new CarTransition(congestionState, () => CheckStateInDirection(entity, EStates.Congestion, entity.transform.forward) && CheckInDirection(entity,-entity.transform.forward)),
                new CarTransition(avoidingState, () => CheckForCongestionInRadius(entity)),

            },

            [avoidingState] = new List<CarTransition>()
            {
                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity) ),
                new CarTransition(congestionState, () =>  CheckStateInDirection(entity, EStates.Congestion, entity.transform.forward) && CheckInDirection(entity,-entity.transform.forward)),
                new CarTransition(swarmState, () => !CheckForCongestionInRadius(entity) ),
            },

            [speedingState] = new List<CarTransition>()
            {
                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity) ),
                new CarTransition(avoidingState, () => CheckInDirection(entity, entity.transform.forward, fromHighspeedSlowDownDistance)),
            }
        };

        stateMachines.Add(new CarStateMachine(congestionState, transitions));
    }

    bool CheckInDirection(CarEntity entity, Vector3 direction, float distance) //Overload
    {
        Ray ray = new Ray(entity.transform.position, direction);

        RaycastHit[] allHits = Physics.RaycastAll(ray, distance, LayerMask.GetMask("Car"));

        if (allHits.Length != 0)
        {
            foreach (var hit in allHits)
            {
                if (hit.collider.gameObject != entity.gameObject)
                {
                    return true;
                }
            }
        }

        return false;
    }


}
