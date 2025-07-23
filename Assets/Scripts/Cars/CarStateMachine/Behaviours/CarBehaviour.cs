using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    [SerializeField]
    protected LayerMask street;

    [SerializeField]
    protected BoidSettings congestionBoidSettings, avoidingBoidSettings, swarmingBoidSettings, findStreetBoidSettings;

    protected List<CarStateMachine> stateMachines = new List<CarStateMachine>();

    [SerializeField]
    protected Vector3 avoidingStateBoxRadius = new Vector3(2f,1f,3f);

    [SerializeField]
    protected float checkingDistance = 2f;


    private void Update()
    {
        if (stateMachines.Count>0)
        {
            foreach (CarStateMachine stateMachine in stateMachines)
            {
                stateMachine.Tick();
            }
        }
    }

    public virtual void CreateStateMachine(CarEntity entity)
    {
        CongestionState congestionState = new CongestionState(entity, congestionBoidSettings);
        FindStreetState findStreetState = new FindStreetState(entity, findStreetBoidSettings);
        SwarmState swarmState = new SwarmState(entity, swarmingBoidSettings);
        AvoidingState avoidingState = new AvoidingState(entity, avoidingBoidSettings);

        Dictionary<CarState, List<CarTransition>> transitions = new Dictionary<CarState, List<CarTransition>>()
        {
            [congestionState] = new List<CarTransition>()
            {
                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity)),
                new CarTransition(avoidingState, () => !CheckStateInDirection(entity, EStates.Congestion, entity.transform.forward) /*CONDITION for last in row frees itself:*/|| !CheckInDirection(entity,-entity.transform.forward)),
            },

            [findStreetState] = new List<CarTransition>()
            {
                new CarTransition(swarmState, () => CheckIfIsOnStreet(entity))
            },

            [swarmState] = new List<CarTransition>()
            {
                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity)),
                new CarTransition(congestionState, () => CheckStateInDirection(entity, EStates.Congestion, entity.transform.forward) && CheckInDirection(entity,-entity.transform.forward)),
                new CarTransition(avoidingState, () => CheckForCongestionInRadius(entity)),
            },

            [avoidingState] = new List<CarTransition>()
            {
                new CarTransition(findStreetState, () => !CheckIfIsOnStreet(entity) ),
                new CarTransition(congestionState, () =>  CheckStateInDirection(entity, EStates.Congestion, entity.transform.forward) && CheckInDirection(entity,-entity.transform.forward)),
                new CarTransition(swarmState, () => !CheckForCongestionInRadius(entity) ),
            }
        };

        stateMachines.Add(new CarStateMachine(congestionState, transitions));
    }

    public void AddNew(CarEntity newCar)
    {
        CreateStateMachine(newCar);
    }

    protected bool CheckIfIsOnStreet(CarEntity entity)
    {
        if (Physics.Raycast(entity.transform.position,Vector3.down, 1f, street))
            return true;

        return false;
    }

    protected bool CheckForCongestionInRadius(CarEntity entity)
    {
        RaycastHit[] checkedCars = Physics.BoxCastAll(entity.transform.position, avoidingStateBoxRadius, Vector3.forward,Quaternion.identity,0f, LayerMask.GetMask("Car"));

        foreach (RaycastHit car in checkedCars)
        {
            if (car.transform.gameObject.GetComponent<CarEntity>() && car.transform.gameObject.GetComponent<CarEntity>().CurrentState == EStates.Congestion)
            {
                return true;
            }
        }

        return false;
    }

    bool CheckForCongestionBehind(CarEntity entity)
    {
        RaycastHit car;

        if(Physics.Raycast(entity.transform.position, entity.transform.forward*-1, out car, checkingDistance, LayerMask.GetMask("Car")))
        {
            if (!car.transform.gameObject.GetComponent<CarEntity>())
                return false;

            if (car.transform.gameObject.GetComponent<CarEntity>().CurrentState == EStates.Congestion)
            {
                return true;
            }
        }

        return false;
    }

    protected bool CheckStateInDirection(CarEntity entity, EStates state, Vector3 direction)
    {
        Ray ray = new Ray(entity.transform.position, direction);

        RaycastHit[] allHits = Physics.RaycastAll(ray, checkingDistance, LayerMask.GetMask("Car"));

        if (allHits.Length == 0) return false;
        
        foreach (var hit in allHits)
        {
            if (hit.collider.gameObject == entity.gameObject) continue;

            if (hit.collider.gameObject.GetComponent<CarEntity>() == null) continue;

            if (hit.collider.gameObject.GetComponent<CarEntity>().CurrentState == state)
            {
                return true;
            }
        }
        

        return false;
    }

    protected bool CheckInDirection(CarEntity entity,Vector3 direction)
    {
        Ray ray = new Ray(entity.transform.position, direction);

        RaycastHit[] allHits = Physics.RaycastAll(ray, checkingDistance, LayerMask.GetMask("Car"));

        if (allHits.Length != 0)
        {
            foreach (var hit in allHits)
            {
                if (hit.collider.gameObject != entity.gameObject)
                {
                    return true;
                }
            }
        }

        return false;
    }

}

