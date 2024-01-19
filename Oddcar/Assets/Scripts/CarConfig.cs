using UnityEngine;

[CreateAssetMenu(fileName = "CarProperties", menuName = "Game/Car Config", order = 1)]
public class CarConfig : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f; // Default speed
    [SerializeField] private float turningSpeed = 180f; // Default turning speed

    public float Speed => speed;
    public float TurningSpeed => turningSpeed;
}