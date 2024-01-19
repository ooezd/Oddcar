using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Level _currentLevel;
    private int _activeCarIndex = 0;
    private CarManager _carManager;
    private LevelManager _levelManager;
    public Timer _timer { get; private set; }
    private bool _isLevelCompleted;

    private void Awake()
    {
        InitializeDependencies();
        SubscribeToCarEvents();
    }

    private void InitializeDependencies()
    {
        _carManager = Locator.Instance.Resolve<CarManager>();
        _levelManager = Locator.Instance.Resolve<LevelManager>();
        _timer = GetComponent<Timer>();
    }

    private void SubscribeToCarEvents()
    {
        Car.OnCollide += OnCarCollision;
        Car.OnReachTarget += OnCarReachTarget;
        Car.OnStartMoving += OnStartMoving;
    }

    public void Init()
    {
        _timer.Reset();
        _currentLevel = null;
        _activeCarIndex = 0;
        _isLevelCompleted = false;
        StartLevel(_levelManager.GetLoadedLevel());
    }

    public void StartLevel(Level level)
    {
        _currentLevel = level;
        _activeCarIndex = 0;
        _carManager.PrepareCars(_currentLevel.GetCarCount());
        SetActiveCar();
    }

    private void OnStartMoving()
    {
        _timer.Reset();
        _timer.StartTimer();
    }

    public void SetActiveCar()
    {
        var activeCar = _carManager.SetActiveCar(_activeCarIndex);

        activeCar.Init(
            _currentLevel.GetSpawnPoint(_activeCarIndex),
            _currentLevel.GetTarget(_activeCarIndex).position
        );

        _carManager.ResetAllCarsToStartState();

        _levelManager.OnActiveCarChanged(_activeCarIndex);
    }

    public void OnCarReachTarget()
    {
        if (_isLevelCompleted)
            return;

        if (_carManager.IsAllActiveTargetsReached())
        {
            if (_activeCarIndex == _currentLevel.GetCarCount() - 1)
            {
                CompleteLevel();
                return;
            }

            _activeCarIndex++;
            SetActiveCar();
            _timer.Pause();
        }
    }

    private void CompleteLevel()
    {
        _timer.Pause();
        _levelManager.LevelCompleted(_timer.currentTime);
        _isLevelCompleted = true;
        Debug.Log($"Level completed in: {_timer.currentTime}");
    }

    public void OnCarCollision()
    {
        _carManager.ResetAllCarsToStartState();
        _timer.Reset();
    }

    private void OnDestroy()
    {
        UnsubscribeFromCarEvents();
    }

    private void UnsubscribeFromCarEvents()
    {
        Car.OnReachTarget -= OnCarReachTarget;
        Car.OnCollide -= OnCarCollision;
        Car.OnStartMoving -= OnStartMoving;
    }
}
