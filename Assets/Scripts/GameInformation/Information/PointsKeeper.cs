using System;
using UnityEngine;

public class PointsKeeper : MonoBehaviour
{
    public event Action<int> OnPointsChanged;

    [SerializeField]
    int points;

    public int Points {get => points;}


    public void Add(int toAdd)
    {
        points += toAdd;

        OnPointsChanged?.Invoke(points);
    }

    public void Decrease(int toDecrease)
    {
        if (points <= 0) return;

        if((points - toDecrease)<0)
        {
            points = 0;
        }

        points -= toDecrease;

        OnPointsChanged?.Invoke(points);
    }

    public bool DecreaseCheck(int toDecrease)
    {
        if (points < 0) return false;

        if ((points - toDecrease) < 0)
        {
            return false;
        }

        return true;
    }

    public void Reset()
    {
        points = 0;

        OnPointsChanged?.Invoke(points);
    }


    private void OnEnable()
    {
        BonusItem.OnBonusReleased += ReceiveBonusPoints;
    }

    private void OnDisable()
    {
        BonusItem.OnBonusReleased -= ReceiveBonusPoints;
    }

    void ReceiveBonusPoints(int bonusPoints)
    {
        Add(bonusPoints);
        OnPointsChanged?.Invoke(points);
    }

}
