using UnityEngine;

public class PlayerCollisions : MonoBehaviour {
    PlayerController controller;

    void Start() {
        controller = transform.parent.parent.GetComponent<PlayerController>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.GetComponent<ObstacleMovement>()) return;
        var obj = other.gameObject.GetComponent<ObstacleMovement>();
        
        switch (obj.roidType) {
            case ObstacleMovement.RoidType.Asteroid:
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
            case ObstacleMovement.RoidType.Asbroid:
                obj.Disable(true);
                controller.Die("ASBroid", true);
                break;
            case ObstacleMovement.RoidType.Megaroid:
                controller.Die("Megaroid", true);
                break;
            case ObstacleMovement.RoidType.Tunnelroid:
                controller.Die("Tunnelroid", true);
                break;
        }
    }
}