using System.Collections.Generic;

public class CarStateMachine
{
    private CarState currentState;
    private Dictionary<CarState, List<CarTransition>> transitions;

    public CarStateMachine(CarState startState, Dictionary<CarState, List<CarTransition>> transitions)
    {
        this.currentState = startState;
        this.transitions = transitions;
    }

    private CarState GetNextState()
    {
        List<CarTransition> currentTransitions = transitions[currentState];

        foreach(CarTransition transition in currentTransitions)
        {
            if (transition.Condition()) return transition.TargetState;
        }

        return null;
    }

    private void SwitchState(CarState targetState)
    {
        if (currentState == targetState) return;

        currentState.OnStateExit();
        targetState.OnStateEnter();

        currentState = targetState;
    }

    public void Tick()
    {
        CarState nextState = GetNextState();

        if (nextState != null) SwitchState(nextState);

        currentState.OnStateUpdate();
    }
}
