using UnityEngine;

public class SwarmState : CarState
{
    public SwarmState(CarEntity entity, BoidSettings stateSettings) : base(entity, stateSettings){}

    public override void OnStateEnter()
    {
        SetBoidSettings();
        ChangeColor(Color.green);
        entity.CurrentState = EStates.Swarm;

    }

}
