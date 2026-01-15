using UnityEngine;

public class Pickup : AsteroidBehaviour {
    protected override void OptionalOnEnable() {
        transform.rotation = Quaternion.identity;
        overrideRotation = true;
    }

    protected override void HandleCollision(Collider2D thisCol, Collider2D otherCol, int index) { }
}