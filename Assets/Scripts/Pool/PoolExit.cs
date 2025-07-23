using System.Collections;
using UnityEngine;

public class PoolExit : MonoBehaviour
{
    [SerializeField]
    CarPool pool;

    [SerializeField]
    float minTime = 0;

    [SerializeField]
    float maxTime = 1;

    [SerializeField]
    int minUnitsPerTime = 1;

    [SerializeField]
    int maxUnitsPerTime = 3;

    bool isInCoroutine;

    float timeTillNext;


    void FixedUpdate()
    {
        if (!isInCoroutine && pool != null && pool.Pool.Count>0)
        {
            timeTillNext = Random.Range(minTime, maxTime);
            StartCoroutine("ReleaseFromPool");
        }
    }

    IEnumerator ReleaseFromPool()
    {
        int unitsPerTime = Mathf.Clamp( Random.Range(minUnitsPerTime, maxUnitsPerTime) ,0,pool.Pool.Count);

        if (unitsPerTime == 0)
            yield break;

        isInCoroutine = true;

        while (timeTillNext > 0)
        {
            timeTillNext-= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < unitsPerTime; i++)
        {
            GameObject toReintigrate = pool.ReleaseCarFromPoolAt();

            float colliderWidth = GetComponent<Collider>().bounds.size.x;

            Vector3 positionInsideMeasurements = new Vector3( transform.position.x+(Random.Range(-colliderWidth/2, colliderWidth/2)), transform.position.y, transform.position.z);

            toReintigrate.transform.position = positionInsideMeasurements;

            toReintigrate.transform.rotation = Quaternion.identity;
        }

        isInCoroutine = false;
    }
}
