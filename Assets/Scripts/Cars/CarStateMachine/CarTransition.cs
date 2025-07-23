using System;

public class CarTransition
{
    Func<bool> condition;
    CarState targetState;

    public Func<bool> Condition => condition;
    public CarState TargetState => targetState;

    public CarTransition(CarState targetState, Func<bool> condition)
    {
        this.targetState = targetState;
        this.condition = condition;
    }

}
