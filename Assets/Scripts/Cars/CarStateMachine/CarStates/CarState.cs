using UnityEngine;

public abstract class CarState
{
    protected CarEntity entity;

    protected BoidSettings stateSettings;

    protected CarState (CarEntity entity, BoidSettings stateSettings)
    {
        this.entity = entity;
        this.stateSettings = stateSettings;
    }

    public virtual void OnStateEnter() { }
    public virtual void OnStateUpdate() { }
    public virtual void OnStateExit() { }

    protected void SetBoidSettings()
    {
        entity.Boid.BoidSettings = stateSettings;
    }

    protected void ChangeColor(Color newColor)
    {
        entity.AppearenceRenderer.materials[0].color = newColor;
    }

}
