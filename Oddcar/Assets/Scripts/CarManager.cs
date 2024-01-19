using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] private Car carPrefab;

    private List<Car> cars = new List<Car>();
    private int activeCarIndex;

    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = Locator.Instance.Resolve<LevelManager>();
        levelManager.OnLevelUnloaded += ResetAllCars;
    }

    public void PrepareCars(int carCount)
    {
        activeCarIndex = 0;

        int carsToSpawn = Mathf.Max(0, carCount - cars.Count);

        for (int i = 0; i < carsToSpawn; i++)
        {
            var car = Instantiate(carPrefab);
            cars.Add(car);
            car.gameObject.SetActive(false);
        }
    }

    public Car SetActiveCar(int index)
    {
        if (index < 0 || index >= cars.Count)
        {
            Debug.LogError($"Invalid car index: {index}");
            return null;
        }

        activeCarIndex = index;
        Car car = cars[index];
        car.gameObject.SetActive(true);
        car.SetAsActiveCar();

        return car;
    }

    public Car GetActiveCar()
    {
        return cars.Count > 0 ? cars[activeCarIndex] : null;
    }

    public bool IsAllActiveTargetsReached()
    {
        for (int i = 0; i <= activeCarIndex; i++)
        {
            if (!cars[i].hasReachedTarget)
                return false;
        }

        return true;
    }

    public void ResetAllCars()
    {
        foreach (Car car in cars)
        {
            car.ResetCar();
        }
    }

    public void ResetAllCarsToStartState()
    {
        for (int i = 0; i <= activeCarIndex; i++)
        {
            cars[i].ResetToStartState();
        }
    }
}
