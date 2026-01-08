using UnityEngine;

public class ObstacleMovement : MonoBehaviour {
    [SerializeField] float speedError;
    float currentSpeedError;
    [SerializeField] float maxSpeed;
    float speed;
    [SerializeField] float maxRotSpeed;
    float rotSpeed = 0f;
    [SerializeField] float camOffset;
    [HideInInspector] public float camLeft;

    [SerializeField] ParticleSystem explosion;
    [SerializeField] ParticleSystem secondaryEffect;

    public RoidType roidType;
    public enum RoidType {
        Asteroid,
        Megaroid,
        Tunnelroid,
        Bombroid,
        Asbroid,
        Shiproid,
        Pickup,
        Speedring
    }

    [SerializeField] float gameSpeedJump;
    GameManager manager;

    [SerializeField] int maxHits;
    int hits = 0;

    void Start() {
        manager = GameManager.instance;

        currentSpeedError = Random.Range(-speedError, speedError);
        rotSpeed = Random.Range(-maxRotSpeed, maxRotSpeed);

        if (roidType == RoidType.Pickup || roidType == RoidType.Tunnelroid) {
            transform.rotation = Quaternion.identity;
        } else {
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        }

    }

    void Update() {
        if (transform.position.x <= camLeft - camOffset) {
            Disable(false);
        }
        else {
            speed = manager.gameSpeed + currentSpeedError;
            speed = Mathf.Clamp(speed, 0f, maxSpeed);

            transform.position -= Vector3.right * speed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0f, 0f, rotSpeed * Time.deltaTime);
        }

        if (manager.postGame) {
            rotSpeed *= manager.endMultiplier;
            currentSpeedError *= manager.endMultiplier;

            if (explosion) {
                var main = explosion.main;
                main.simulationSpeed *= manager.endMultiplier;
            }

            if (secondaryEffect) {
                var main = secondaryEffect.main;
                main.simulationSpeed *= manager.endMultiplier;
            }
        }
    }

    public void Disable(bool explode) {
        if (roidType == RoidType.Megaroid || roidType == RoidType.Tunnelroid) {
            GetComponentInParent<AsteroidSpawner>().megaroidActive = false;
        }

        if (explode && explosion) {
            if (GetComponent<CircleCollider2D>()) {
                GetComponent<CircleCollider2D>().enabled = false;
            }
            else {
                GetComponent<PolygonCollider2D>().enabled = false;
            }

            if (roidType == RoidType.Speedring) {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);

                for (var i = 0; i < explosion.transform.childCount; i++) {
                    explosion.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
                }
            }
            else {
                GetComponent<SpriteRenderer>().enabled = false;

                var vol = explosion.velocityOverLifetime;
                vol.x = -1f * (manager.gameSpeed + currentSpeedError);

                explosion.Play();
            }
        }
        else {
            Destroy(gameObject);
        }
    }

    public void SpeedRingEffect() {
        if (!secondaryEffect) return;
        secondaryEffect.Play();
        manager.AlterGameSpeedBy(gameSpeedJump);
    }

    void OnParticleSystemStopped() {
        Destroy(gameObject);
    }

    void HandleCollision(Transform other) {
        if (other.CompareTag("Weapon")) {
            hits++;

            if (hits >= maxHits) {
                Disable(true);
            } else {
                secondaryEffect.Play();
            }

            return;
        }

        if (!other.GetComponent<ObstacleMovement>()) return;
        var obj = other.GetComponent<ObstacleMovement>();

        switch(roidType) {
            case RoidType.Speedring:
                if (obj.roidType == RoidType.Megaroid) {
                    Disable(true);
                }
                break;
            case RoidType.Megaroid:
                break;
            case RoidType.Tunnelroid:
                break;
            default:
                if (obj.roidType != RoidType.Speedring && obj.roidType != RoidType.Pickup) {
                    Disable(true);
                }
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