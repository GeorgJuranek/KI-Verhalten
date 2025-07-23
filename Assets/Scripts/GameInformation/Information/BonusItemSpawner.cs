using Assets.Scripts;
using UnityEngine;

public class BonusItemSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject objectToSpawn;

    private void OnEnable()
    {
        CarEvents.OnSpeederBrake += SpawnBonusPoints; 
    }

    private void OnDisable()
    {
        CarEvents.OnSpeederBrake -= SpawnBonusPoints;
    }

    void SpawnBonusPoints(Transform car)
    {
        Instantiate(objectToSpawn, car.position, Quaternion.identity);
    }
}
