using Assets.Scripts;
using System;
using UnityEngine;

public class CongestedCarsObserver : MonoBehaviour
{
    public static event Action<int> OnCountChanged;

    int congestedCarCount;

    float currentFloatingPoints;
    public float CurrentFloatingPoints {
        get => currentFloatingPoints;

        set
        {
            int previousPoints = Mathf.FloorToInt(currentFloatingPoints);

            currentFloatingPoints = value;

            int currentFullPoints = Mathf.FloorToInt(currentFloatingPoints);

            if (previousPoints < currentFullPoints)
            {
                pointsKeeper.Add(currentFullPoints-previousPoints);
            }
        }
            
    }

    [SerializeField]
    PointsKeeper pointsKeeper;

    private void OnEnable()
    {
        CarEvents.OnCarUncongested += RemoveCar;
        CarEvents.OnCarCongested += AddCar;
    }

    private void AddCar()
    {
        congestedCarCount++;
        OnCountChanged?.Invoke(congestedCarCount);
    }

    private void RemoveCar()
    {
        congestedCarCount = Mathf.Max(0, congestedCarCount-1);
        OnCountChanged?.Invoke(congestedCarCount);
    }

    private void OnDisable()
    {
        CarEvents.OnCarUncongested -= RemoveCar;
        CarEvents.OnCarCongested -= AddCar;
    }


   private void Update()
   {
        if (congestedCarCount > 0)
        {
            float basePoints = congestedCarCount;
            float bonus = Mathf.Floor(congestedCarCount / 3f); // für je 3 Autos +1 Bonus
            float total = basePoints + bonus;
            CurrentFloatingPoints += total * Time.deltaTime;
        }
   }
}
