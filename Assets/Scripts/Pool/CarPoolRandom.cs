using System.Collections.Generic;
using UnityEngine;

public class CarPoolRandom : CarPool
{
    private List<GameObject> randomPool = new List<GameObject>();
    override public List<GameObject> Pool { get => randomPool; }

    override public void ReturnCarToPool(GameObject car)
    {
        car.SetActive(false);
        randomPool.Add(car);

        ShuffleList(randomPool);
    }

    override public GameObject ReleaseCarFromPoolAt()
    {
        int random = (int)Random.Range(0, randomPool.Count - 1);

        GameObject next = randomPool[random];

        if (next != null)
            next.SetActive(true);

        randomPool.Remove(next);

        return next;
    }


    // NOT in BaseClass
    void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}
