using UnityEngine;

[CreateAssetMenu(menuName = "Player/Ship Data")]
public class ShipData : ScriptableObject {
    [Header("Movement")]
    public float moveSpeed;
    public float turnSpeed;
    public float acceleration;

    [Header("Prefab")]
    public GameObject ship;
}