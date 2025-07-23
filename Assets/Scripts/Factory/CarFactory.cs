using System;
using System.Collections.Generic;
using UnityEngine;

public class CarFactory : MonoBehaviour
{
    [SerializeField]
    List<FactoryOrderInformations> factoryOrders = new List<FactoryOrderInformations>();

    [SerializeField]
    PoolEntrance poolEntrance;

    private void Awake()
    {
        foreach(FactoryOrderInformations order in factoryOrders)
        {
            for (int i = 0; i < order.productAmount; i++)
            {
                CreateCar(order.product, order.carBehaviour);
            }
        }
    }

    void CreateCar(GameObject product, CarBehaviour fittingCarBehaviour)
    {
        GameObject newCar = Instantiate(product);
        fittingCarBehaviour.CreateStateMachine(newCar.GetComponent<CarEntity>());
        poolEntrance.PutInPool(newCar);
    }

    [Serializable]
    public class FactoryOrderInformations
    {
        public GameObject product;
        public int productAmount;
        public CarBehaviour carBehaviour;
    }
}
