using UnityEngine;

[CreateAssetMenu(menuName = "Player/Ship Data")]
public class ShipData : ScriptableObject {
    [Header("Movement")]
    public float moveSpeed;
    public float turnSpeed;
    public float acceleration;

    [Header("Attack")]
    public GameObject weaponPrefab;
    public int bombCapacity;
    public int initialBombs;

    [Header("Sprite")]
    public Sprite shipSprite;
    public ShipType shipType;
    public GameObject colliderObject;

    public enum ShipType {
        Ares, Hermes, Artemis, Zeus
    }
}