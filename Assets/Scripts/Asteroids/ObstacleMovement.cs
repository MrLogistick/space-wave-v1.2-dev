using UnityEngine;

public class ObstacleMovement : MonoBehaviour {
    [SerializeField] float speedError;
    float currentSpeedError;
    [HideInInspector] public float speed;
    [SerializeField] float maxRotSpeed;
    float rotSpeed = 0f;

    bool disabled;
    float time;
    
    [SerializeField] int scoreIncrease;

    public float camOffset;
    [HideInInspector] public float camLeft;
    public float radius;

    [HideInInspector] public bool overrideSpeed;

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

        // Starting Rotation
        if (roidType == RoidType.Pickup || roidType == RoidType.Tunnelroid) {
            transform.rotation = Quaternion.identity;
        }
        else {
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        }

    }

    void Update() {
        if (transform.position.x <= camLeft - camOffset) {
            // Disable off-screen
            Disable(false);
        }
        else {
            // Movement and rotation
            if (!overrideSpeed) {
                speed = manager.rawSpeed + currentSpeedError;
            }

            float speedOffset = roidType == RoidType.Pickup ? -10f : 10f;
            speed = Mathf.Min(speed, manager.publicMaxSpeed + speedOffset);

            transform.position -= Vector3.right * speed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0f, 0f, rotSpeed * Time.deltaTime);

            time += Time.deltaTime;
            if (time >= 0.1f) {
                // 0.1 second updater
                time = 0f;
                if (roidType == RoidType.Asbroid) {
                    FireAbility();
                }
            }
        }

        if (manager.postGame) {
            // post-game slowing down
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
        // fire asteroid ability
        if (!ability) {
            Debug.LogError($"{gameObject.name}'s ability is inaccessible.");
            return;
        }

        ability.Fire(this);
    }

    public void Disable(bool explode) {
        var spawner = GetComponentInParent<AsteroidSpawner>();

        if (roidType == RoidType.Megaroid) {
            spawner.megaroidActive = false;
        }

        if (explode && explosion) {
            if (roidType == RoidType.Speedring) {
                // speedring explosion
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
                if (roidType == RoidType.Bombroid && GetComponent<BombroidEffect>().active) {
                    // Bombroid lethal explosion, then regular explosion
                    if (!disabled) FireAbility();
                }
                else {
                    // regular explosion
                    GetComponent<SpriteRenderer>().enabled = false;
                    var vol = explosion.velocityOverLifetime;
                    vol.x = -1f * (manager.rawSpeed + currentSpeedError);

                    if (!disabled) explosion.Play();
                }

                if (roidType == RoidType.Megaroid && !disabled) {
                    // Spawn new roids
                    FireAbility();
                }
            }

            if (disabled) return;

            // disable colliders if exploding
            if (GetComponent<CircleCollider2D>()) {
                GetComponent<CircleCollider2D>().enabled = false;
            }
            else {
                GetComponent<PolygonCollider2D>().enabled = false;
            }
        }
        else {
            // disable without explosion
            Destroy(gameObject);
        }
    }

    void OnParticleSystemStopped() {
        // OnExplosionSystemStopped
        Destroy(gameObject);
    }

    void HandleCollision(Transform other) {
        if (other.CompareTag("Weapon") && roidType != RoidType.Megaroid && roidType != RoidType.Tunnelroid) {
            // On Collision with a weapon
            manager.AlterScoreBy(scoreIncrease);
            Disable(true);

            return;
        }

        if (!other.GetComponent<ObstacleMovement>()) return;
        var obj = other.GetComponent<ObstacleMovement>();

        if (obj.roidType == RoidType.Pickup || obj.roidType == RoidType.Speedring) return;

        if (obj.radius >= radius) {
            // On collision with another ObstacleMovement inheritor
            Disable(true);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        HandleCollision(other.transform);
    }

    void OnTriggerEnter2D(Collider2D other) {
        HandleCollision(other.transform);
    }
}