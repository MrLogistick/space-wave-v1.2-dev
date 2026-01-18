using UnityEngine;

public class Hermes : ShipAbility {
    [SerializeField] Transform[] shootPoints;
    int currentPoint;

    protected override void OptionalUpdate() {
        currentPoint = transform.rotation.z < -45f ? 1 : 0;
    }

    protected override void Fire() {
        Instantiate(ability, shootPoints[currentPoint].position, ability.transform.rotation);
    }
}