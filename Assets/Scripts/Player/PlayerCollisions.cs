using UnityEngine;

public class PlayerCollisions : MonoBehaviour {
    PlayerController controller;

    void Start() {
        controller = transform.parent.parent.GetComponent<PlayerController>();
    }

    void HandleCollision(Transform other) {

        // If player touches a weapon without SafeForPlayer
        if (other.GetComponent<SafeForPlayer>()) return;
        if (!other.GetComponent<ObstacleMovement>()) {
            if (other.CompareTag("Weapon")) {
                controller.Die("ASBroid", true);
            }

            return;
        }

        // Asteroid Collisions
        var obj = other.GetComponent<ObstacleMovement>();
        switch (obj.roidType) {
            case ObstacleMovement.RoidType.Asteroid:
            case ObstacleMovement.RoidType.Asbroid:
                obj.Disable(true);
                controller.Die("Asteroid", true);
                break;
            case ObstacleMovement.RoidType.Shiproid:
                obj.Disable(true);
                controller.Die("Shipwreck", true);
                break;
            case ObstacleMovement.RoidType.Bombroid:
                obj.Disable(true);
                controller.Die("Bombroid", true);
                break;
            case ObstacleMovement.RoidType.Megaroid:
                controller.Die("Megaroid", true);
                break;
            case ObstacleMovement.RoidType.Tunnelroid:
                controller.Die("Tunnelroid", true);
                break;
            case ObstacleMovement.RoidType.Pickup:
                obj.Disable(true);
                controller.AlterAbility();
                break;
            case ObstacleMovement.RoidType.Speedring:
                obj.FireAbility();
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        HandleCollision(other.transform);
    }

    void OnTriggerEnter2D(Collider2D other) {
        HandleCollision(other.transform);
    }
}