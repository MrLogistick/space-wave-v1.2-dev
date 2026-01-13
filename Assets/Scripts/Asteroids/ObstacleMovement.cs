using UnityEngine;

public class ObstacleMovement : MonoBehaviour {
    [SerializeField] float speedError;
    float currentSpeedError;
    float speed;
    [SerializeField] float maxRotSpeed;
    float rotSpeed = 0f;
    
    [SerializeField] int scoreIncrease;

    public float camOffset;
    [HideInInspector] public float camLeft;

    [SerializeField] ParticleSystem explosion;

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

    [SerializeField] AsteroidAbility ability;
    GameManager manager;

    void Start() {
        manager = GameManager.instance;

        currentSpeedError = Random.Range(-speedError, speedError);
        rotSpeed = Random.Range(-maxRotSpeed, maxRotSpeed);

        if (roidType == RoidType.Pickup || roidType == RoidType.Tunnelroid) {
            transform.rotation = Quaternion.identity;
        }
        else {
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        }

    }

    void Update() {
        if (transform.position.x <= camLeft - camOffset) {
            Disable(false);
        }
        else {
            speed = manager.rawSpeed + currentSpeedError;

            float speedOffset = roidType == RoidType.Pickup ? -10f : 10f;
            speed = Mathf.Min(speed, manager.publicMaxSpeed + speedOffset);

            transform.position -= Vector3.right * speed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0f, 0f, rotSpeed * Time.deltaTime);
        }

        if (manager.postGame) {
            rotSpeed *= manager.endMultiplier;
            currentSpeedError *= manager.endMultiplier;

            if (explosion) {
                var main = explosion.main;
                main.simulationSpeed *= manager.endMultiplier;

                if (roidType != RoidType.Speedring) return;
                for (var i = 0; i < explosion.transform.childCount; i++) {
                    main = explosion.transform.GetChild(i).GetComponent<ParticleSystem>().main;
                    main.simulationSpeed *= manager.endMultiplier;                
                }
            }
        }
    }

    public void FireAbility() {
        if (!ability) {
            Debug.LogError($"{gameObject.name}'s ability is inaccessible.");
            return;
        }

        ability.Fire();
    }

    public void Disable(bool explode) {
        var spawner = GetComponentInParent<AsteroidSpawner>();

        if (roidType == RoidType.Megaroid) {
            spawner.megaroidActive = false;
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
                    var e = explosion.transform.GetChild(i).GetComponent<ParticleSystem>();

                    var vol = e.velocityOverLifetime;
                    vol.x = -1f * (manager.rawSpeed + currentSpeedError);
                    e.Play();
                }
            }
            else {
                GetComponent<SpriteRenderer>().enabled = false;

                var vol = explosion.velocityOverLifetime;
                vol.x = -1f * (manager.rawSpeed + currentSpeedError);

                explosion.Play();

                if (roidType == RoidType.Megaroid) {
                    FireAbility();
                }
            }
        }
        else {
            Destroy(gameObject);
        }
    }

    void OnParticleSystemStopped() {
        Destroy(gameObject);
    }

    void HandleCollision(Transform other) {
        if (other.CompareTag("Weapon") && roidType != RoidType.Megaroid && roidType != RoidType.Tunnelroid) {
            manager.AlterScoreBy(scoreIncrease);
            Disable(true);

            return;
        }

        if (!other.GetComponent<ObstacleMovement>()) return;
        var obj = other.GetComponent<ObstacleMovement>();

        switch(roidType) {
            case RoidType.Megaroid:
            case RoidType.Tunnelroid:
                break;
            case RoidType.Speedring:
                if (obj.roidType == RoidType.Megaroid) {
                    Disable(true);
                }
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