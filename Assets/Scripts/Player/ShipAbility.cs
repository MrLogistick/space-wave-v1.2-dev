using UnityEngine;

public class ShipAbility : MonoBehaviour
{
    public bool ability;

    [SerializeField] int initialBullets;
    [SerializeField] int maxBullets;
    int bulletCount;

    [SerializeField] GameObject bullet;

    [SerializeField] Transform shootPoint;
    [SerializeField] Transform secondShootPoint;
    Transform currentPoint;

    [SerializeField] ShipType currentShip;
    enum ShipType {
        Athena,
        Hermes,
        Zeus,
        Hephaetsus
    }

    void Start() {
        bulletCount = initialBullets;
    }

    void Update() {
        switch (currentShip) {
            case ShipType.Athena: // script in weapon
                currentPoint = shootPoint;
                break;
            case ShipType.Hermes: // script in ship
                currentPoint = (transform.rotation.z >= -45f)
                    ? secondShootPoint : shootPoint;
                break;
        }

        if (bulletCount > 0 && ability) {
            ability = false;
            bulletCount--;
            Instantiate(bullet, currentPoint.position, bullet.transform.rotation);
        }
    }
}