using UnityEngine;

public class FindStreetState : CarState
{
    Transform rememberedTarget;

    public FindStreetState(CarEntity entity, BoidSettings stateSettings) : base(entity, stateSettings) {}

    public override void OnStateEnter()
    {
        SetBoidSettings();
        ChangeColor(Color.gray);
        entity.CurrentState = EStates.FindStreet;

        entity.Boid.Speed = 1.5f;
        rememberedTarget = entity.GetComponent<Boid>().Target;
    }
    public override void OnStateUpdate()
    {
        RaycastHit[] orientations = Physics.SphereCastAll(entity.transform.position, 150f, entity.transform.forward, 150f, LayerMask.GetMask("Orientation"));
        Transform shortTarget = GetNearest(orientations);
        entity.GetComponent<Boid>().Target = shortTarget;
    }

    public override void OnStateExit()
    {
        entity.Boid.Speed = 3f;
        entity.GetComponent<Boid>().Target = rememberedTarget;
    }

    Transform GetNearest(RaycastHit[] orientations)
    {
        Transform nearest = null;
        Transform current;

        foreach (RaycastHit orientationMark in orientations)
        {
            current = orientationMark.transform;

            if (nearest == null || Vector3.Distance(entity.transform.position,current.position) < Vector3.Distance(entity.transform.position, nearest.position))
            {
                nearest = current;
            }
        }

        return nearest;
    }
}
