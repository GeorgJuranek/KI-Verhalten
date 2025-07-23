using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Assets.Scripts;

public class CongestionState : CarState
{
    public CongestionState(CarEntity entity, BoidSettings stateSettings) : base(entity, stateSettings) {}

    [SerializeField]
    PointsKeeper pointsKeeper;

    public override void OnStateEnter()
    {
        SetBoidSettings();

        ChangeColor(UnityEngine.Color.red);
        entity.CurrentState = EStates.Congestion;

        entity.Boid.Speed = entity.CongestionSpeed;

        CarEvents.OnCarCongested?.Invoke();
    }

    public override void OnStateExit()
    {
        entity.Boid.Speed = 3f;

        CarEvents.OnCarUncongested?.Invoke();
    }

}
