using System.Collections.Generic;

public class SUVBehaviour : CarBehaviour
{
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
        SUVCongestionState congestionState = new SUVCongestionState(entity, congestionBoidSettings);
        FindStreetState findStreetState = new FindStreetState(entity, findStreetBoidSettings);
        SwarmState swarmState = new SwarmState(entity, swarmingBoidSettings);
        AvoidingState avoidingState = new AvoidingState(entity, avoidingBoidSettings);

        Dictionary<CarState, List<CarTransition>> transitions = new Dictionary<CarState, List<CarTransition>>()
        {
            [congestionState] = new List<CarTransition>()
            {
                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity)),
                new CarTransition(swarmState, () =>  !CheckStateInDirection(entity, EStates.Congestion, entity.transform.forward) || !CheckInDirection(entity,-entity.transform.forward)),
            },

            [findStreetState] = new List<CarTransition>()
            {
                new CarTransition(swarmState, () => CheckIfIsOnStreet(entity))
            },

            [swarmState] = new List<CarTransition>()
            {
                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity)),
                new CarTransition(avoidingState, () => CheckForCongestionInRadius(entity)),
            },

            [avoidingState] = new List<CarTransition>()
            {
                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity) ),
                new CarTransition(congestionState, () =>  CheckStateInDirection(entity, EStates.Congestion, entity.transform.forward) && CheckStateInDirection(entity,EStates.Congestion,-entity.transform.forward)),
                new CarTransition(swarmState, () => !CheckForCongestionInRadius(entity) ),
            },
        };

        stateMachines.Add(new CarStateMachine(congestionState, transitions));
    }

}
