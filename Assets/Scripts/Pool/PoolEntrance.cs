using System.Collections.Generic;
using UnityEngine;

public class PoolEntrance : MonoBehaviour
{
    [SerializeField]
    CarPool pool;
    public CarPool Pool { get => pool; set { pool = value; } }

    [SerializeField]
    List<Transform> targets;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Car"))
        {
            PutInPool(collider.gameObject);
        }
    }

    public void PutInPool(GameObject aimlessBoid)
    {
            aimlessBoid.gameObject.GetComponent<Boid>().Target = targets[Random.Range(0, targets.Count)];

            pool.ReturnCarToPool(aimlessBoid.gameObject);

    }
}
