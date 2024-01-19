using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour, ITransformRecorder
{
    [SerializeField] private CarConfig config;
    [SerializeField] private SpriteRenderer carSprite;

    public static event Action OnCollide;
    public static event Action OnReachTarget;
    public static event Action OnStartMoving;

    private bool isActiveCar = false;
    private bool hasCompletedPath = false;
    private bool hasStartedMoving = false;
    public bool hasReachedTarget { get; private set; } = false;

    private Transform startTransform;
    private Vector2 targetPoint;

    public List<Vector2> positionRecords { get; set; } = new();
    public List<Quaternion> rotationRecords { get; set; } = new();
    public bool isRecording { get; set; } = false;
    public int currentReplayingFrame { get; set; } = 0;
    public bool isReplaying { get; set; }

    private void Awake()
    {
        if(config == null)
            config = ScriptableObject.CreateInstance<CarConfig>();

        OnStartMoving += OnStartMovingHandler;
    }

    public void Init(Transform startTransform, Vector2 targetPoint)
    {
        this.startTransform = startTransform;
        this.targetPoint = targetPoint;
    }

    private void OnStartMovingHandler()
    {
        if (!isActiveCar && hasCompletedPath)
        {
            isReplaying = true;
            hasStartedMoving = true;
        }
    }

    public void SetAsActiveCar()
    {
        isActiveCar = true;
    }

    private void Update()
    {
        if (hasCompletedPath && isReplaying)
        {
            ReplayPath();
            return;
        }

        if (isActiveCar)
        {
            if (!hasStartedMoving && InputProvider.isScreenTouched)
            {
                hasStartedMoving = true;
                hasReachedTarget = false;
                OnStartMoving?.Invoke();
            }

            if (!hasStartedMoving)
                return;

            RecordTransform();
            MoveForward();

            if (InputProvider.isTurnLeftButtonPressed)
                TurnLeft();
            else if (InputProvider.isTurnRightButtonPressed)
                TurnRight();
        }
    }

    public void ResetToStartState()
    {
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
        hasStartedMoving = false;
        hasReachedTarget = false;
        isReplaying=false;
        currentReplayingFrame=0;
    }

    private void MoveForward()
    {
        transform.Translate(config.Speed * Time.deltaTime * Vector3.up);
    }
    private void TurnRight()
    {
        transform.Rotate(Vector3.forward, -config.TurningSpeed * Time.deltaTime);
    }

    private void TurnLeft()
    {
        transform.Rotate(Vector3.forward, config.TurningSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActiveCar)
            return;

        OnCollide?.Invoke();
        StartRecording();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isActiveCar) 
            return;

        if (collision.CompareTag("Target"))
        {
            float distance = Vector2.Distance(targetPoint, transform.position);

            if (distance > 1f) //Not this car's target
                return;

            hasReachedTarget = true;
            OnReachTarget?.Invoke();
            
            if(isActiveCar)
                hasCompletedPath = true;

            isActiveCar = false;
            carSprite.color = new Color(.6f, .6f, .6f);
        }
    }

    public void StartRecording()
    {
        isRecording = true;
        positionRecords.Clear();
        rotationRecords.Clear();
    }

    public void RecordTransform()
    {
        positionRecords.Add(transform.position);
        rotationRecords.Add(transform.rotation);
    }

    public void ReplayPath()
    {
        if(++currentReplayingFrame < positionRecords.Count)
        {
            transform.SetPositionAndRotation(positionRecords[currentReplayingFrame], 
                rotationRecords[currentReplayingFrame]);
        }
        else
        {
            hasReachedTarget = true;
            OnReachTarget?.Invoke();
        }
    }

    public void ResetCar()
    {
        positionRecords.Clear();
        rotationRecords.Clear();
        carSprite.color = Color.white;
        isActiveCar = false;
        hasCompletedPath = false;
        hasStartedMoving = false;
        hasReachedTarget = false;
        startTransform = null;
        targetPoint = Vector2.zero;
        isRecording = false;
        isReplaying = false;
        currentReplayingFrame = 0;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        OnStartMoving -= OnStartMovingHandler;
    }
}