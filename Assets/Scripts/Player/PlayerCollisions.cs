using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour {
    PlayerController controller;

    void Start() {
        controller = transform.parent.parent.GetComponent<PlayerController>();
    }

    void HandleCollision(Transform other) {
        // If player touches a weapon without SafeForPlayer
        if (other.GetComponent<SafeForPlayer>()) return;

        if (!other.GetComponent<AsteroidBehaviour>()) {
            if (!other.CompareTag("Weapon")) return;
            
            if (other.GetComponent<PlasmaShotBehaviour>()) {
                controller.Die("ASBroid", true);
            }
            if (other.GetComponentInParent<Bombroid>()) {
                controller.Die("Bombroid", true);
            }

            return;
        }

        // Asteroid Collisions
        var obj = other.GetComponent<AsteroidBehaviour>();
        switch (obj.roidType) {
            case AsteroidBehaviour.RoidType.Asteroid:
            case AsteroidBehaviour.RoidType.Asbroid:
            case AsteroidBehaviour.RoidType.Bombroid:
                obj.Disable(true);
                controller.Die("Asteroid", true);
                break;
            case AsteroidBehaviour.RoidType.Shiproid:
                obj.Disable(true);
                controller.Die("Shipwreck", true);
                break;
            case AsteroidBehaviour.RoidType.Megaroid:
                controller.Die("Megaroid", true);
                break;
            case AsteroidBehaviour.RoidType.Tunnelroid:
                controller.Die("Tunnelroid", true);
                break;
            case AsteroidBehaviour.RoidType.Pickup:
                controller.AlterAbility();
                obj.Disable(true);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        HandleCollision(other.transform);
    }

    void OnTriggerEnter2D(Collider2D other) {
        HandleCollision(other.transform);
    }

    void OnTriggerStay2D(Collider2D other) {
        if (!other.CompareTag("Weapon")) return;
        HandleCollision(other.transform);
    }
}