using Assets.Scripts;
using UnityEngine;

public class SUVCongestionState : CarState
{
    public SUVCongestionState(CarEntity entity, BoidSettings stateSettings) : base(entity, stateSettings) { }

    [SerializeField]
    PointsKeeper pointsKeeper;

    float currentSpeed;

    public override void OnStateEnter()
    {
        SetBoidSettings();

        ChangeColor(UnityEngine.Color.red);
        entity.CurrentState = EStates.Congestion;

        currentSpeed = entity.CongestionSpeed;

        entity.Boid.Speed = currentSpeed;

        CarEvents.OnCarCongested?.Invoke();

    }

    public override void OnStateExit()
    {
        entity.Boid.Speed = 3f;

        CarEvents.OnCarUncongested?.Invoke();
    }
}
