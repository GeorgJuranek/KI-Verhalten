
public class AvoidingState : CarState
{
    public AvoidingState(CarEntity entity, BoidSettings stateSettings) : base(entity, stateSettings)
    {
    }

    public override void OnStateEnter()
    {
        SetBoidSettings();
        ChangeColor(UnityEngine.Color.yellow);
        entity.CurrentState = EStates.Avoiding;
        entity.Boid.Speed = 1f;
    }

    public override void OnStateExit()
    {
        entity.Boid.Speed = 3f;
    }
}
