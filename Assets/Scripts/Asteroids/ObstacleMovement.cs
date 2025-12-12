using UnityEngine;

public class ObstacleMovement : MonoBehaviour {
    float speed;
    [SerializeField] float maxRotSpeed;
    float rotSpeed;
    [SerializeField] float camOffset;
    [HideInInspector] public float camLeft;

    public RoidType roidType;
    public enum RoidType {
        Asteroid,
        Megaroid,
        Tunnelroid,
        Bombroid,
        Asbroid,
        Shiproid,
        Pickup
    }

    void Start() {
        rotSpeed = Random.Range(-maxRotSpeed, maxRotSpeed);
    }

    void Update() {
        if (transform.position.x <= camLeft - camOffset) {
            Disable(false);
        }
        else {
            speed = GameManager.instance.gameSpeed;
            transform.position -= Vector3.right * speed * Time.deltaTime;

            if (roidType != RoidType.Pickup || roidType != RoidType.Tunnelroid) {
                transform.rotation *= Quaternion.Euler(0f, 0f, rotSpeed * Time.deltaTime);
            }
        }

        if (GameManager.instance.postGame) {
            rotSpeed *= GameManager.instance.endMultiplier;
        }
    }

    public void Disable(bool explode) {
        if (explode) {
            // particle system
        }

        if (roidType == RoidType.Megaroid || roidType == RoidType.Tunnelroid) {
            GetComponentInParent<AsteroidSpawner>().megaroidActive = false;
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (roidType == RoidType.Megaroid || roidType == RoidType.Tunnelroid) return;

        if (other.GetComponent<ObstacleMovement>()) {
            Disable(true);
        }
    }
}