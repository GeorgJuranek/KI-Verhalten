using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedingState : CarState
{
    public SpeedingState(CarEntity entity, BoidSettings stateSettings) : base(entity, stateSettings) { }

    [SerializeField]
    GameObject CongestionBonus;

    public override void OnStateEnter()
    {
        SetBoidSettings();

        ChangeColor(UnityEngine.Color.blue);
        entity.CurrentState = EStates.Speeding;

        entity.Boid.Speed = 8f;
    }

    public override void OnStateExit()
    {
        entity.Boid.Speed = 3f;

        CarEvents.OnSpeederBrake?.Invoke(entity.transform);
    }

}
