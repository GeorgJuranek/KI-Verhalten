using System;
using UnityEngine;

[RequireComponent(typeof(TimeSelfDestructor))]

public class StreetObstacle : MonoBehaviour, IClickable
{
    [SerializeField]
    int cost = 500;
    public int Cost { get => cost; }

    TimeSelfDestructor timeBomb;

    public static Action<int> OnPointsSpend;

    private void Start()
    {
        timeBomb = GetComponent<TimeSelfDestructor>();
    }

    public void LeftClick()
    {
        timeBomb.AddTime(30);
        OnPointsSpend?.Invoke(cost);
    }

    public void RightClick()
    {
        timeBomb.DestroyNow();
    }

    public void Hover(Color newColor)
    {
        timeBomb.ChangeAppearence(newColor);
    }

    public void Unhover()
    {
        timeBomb.DechangeAppearence();
    }

}
