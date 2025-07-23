using System.Collections.Generic;
using UnityEngine;

public class CarPool : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();
    virtual public List<GameObject> Pool { get; private set; }


    virtual public void ReturnCarToPool(GameObject car)
    {
        car.SetActive(false);
        pool.Add(car);
    }

    virtual public GameObject ReleaseCarFromPoolAt()
    {
        GameObject next = pool[pool.Count - 1];

        if (next != null)
        next.SetActive(true);

        pool.Remove(next);
        return next;
    }

}
