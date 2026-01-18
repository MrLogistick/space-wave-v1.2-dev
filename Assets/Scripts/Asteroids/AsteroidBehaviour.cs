using Unity.VisualScripting;
using UnityEngine;

public abstract class AsteroidBehaviour : MonoBehaviour {
    [SerializeField] float maxSpeedOffset;
    [SerializeField] float speedError = 2f;
    float chosenSpeedError;
    [HideInInspector] public float speed;
    [HideInInspector] public bool overrideSpeed;

    [SerializeField] float maxRotSpeed = 180f;
    float chosenRotSpeed;
    [HideInInspector] public bool overrideRotation;

    public float roidStrength;
    public int scoreOnDestroy;
    public ParticleSystem[] particles;
    public Collider2D[] colliders;
    public SpriteRenderer[] renderers;
    public ObjectPool pool;
    [HideInInspector] public bool disabled;

    Collider2D[] results = new Collider2D[8];
    ContactFilter2D filter = new ContactFilter2D();

    Camera cam;
    [HideInInspector] public float camLeft;
    public float camOffset;

    [HideInInspector] public GameManager manager;

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

    protected void Awake() {
        filter.useTriggers = true;
        filter = ContactFilter2D.noFilter;
    }

    protected void Start() {
        manager = GameManager.instance;
    }

    protected void OnEnable() {
        chosenSpeedError = Random.Range(-speedError, speedError);
        chosenRotSpeed = Random.Range(-maxRotSpeed, maxRotSpeed);
        disabled = false;

        cam = Camera.main;

        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        foreach (var renderer in renderers) {
            renderer.enabled = true;
        }

        foreach(var collider in colliders) {
            collider.enabled = true;
        }

        OptionalOnEnable();
    }
    protected virtual void OptionalOnEnable() { }

    protected void  Update() {
        Move();
        PostGame();

        OptionalUpdate();
    }
    protected virtual void OptionalUpdate() { }

    protected virtual void Move() {
        if (!overrideSpeed) speed = manager.rawSpeed + chosenSpeedError;
        speed = Mathf.Min(speed, manager.publicMaxSpeed + maxSpeedOffset);

        camLeft = cam.transform.position.x - cam.orthographicSize * cam.aspect;

        transform.position += Vector3.left * speed * Time.deltaTime;
        if (!overrideRotation) transform.rotation *= Quaternion.Euler(0f, 0f, chosenRotSpeed * Time.deltaTime);

        if (transform.position.x < camLeft - camOffset) {
            Disable(false);
        }
    }

    protected virtual void PostGame() {
        if (!manager.postGame) return;

        chosenSpeedError *= manager.endMultiplier;
        chosenRotSpeed *= manager.endMultiplier;

        foreach (var system in particles) {
            var main = system.main;
            main.simulationSpeed *= manager.endMultiplier;
        }
    }

    public virtual void Disable(bool explode) {
        if (disabled) return;
        disabled = true;

        OptionalDisable(explode);

        if (explode) {
            foreach (var renderer in renderers) renderer.enabled = false;
            foreach(var collider in colliders) collider.enabled = false;
            foreach (var system in particles) {
                var vol = system.velocityOverLifetime;
                vol.x = -speed;

                system.Play();
            }
        }
        else {
            if (!manager.postGame) pool.ReturnToPool(gameObject);        
        }
    }
    protected virtual void OptionalDisable(bool explode) { }

    protected void OnParticleSystemStopped() {
        if (!manager.postGame) pool.ReturnToPool(gameObject);        
    }

    protected virtual void HandleCollision(Collider2D thisCol, Collider2D otherCol, int index) {
        if (otherCol.gameObject.CompareTag("Weapon")) {
            if (thisCol.isTrigger) return;
            if (otherCol.GetComponent<SafeForPlayer>()) manager.AlterScoreBy(scoreOnDestroy);
            Disable(true);
        }

        var otherRoid = otherCol.GetComponent<AsteroidBehaviour>();
        if (otherRoid != null) {
            if (thisCol.isTrigger) return;

            if (otherRoid.roidStrength >= roidStrength) {
                Disable(true);
            }
        }

        OptionalCollisions(thisCol, otherCol, index);
    }
    protected virtual void OptionalCollisions(Collider2D thisCol, Collider2D otherCol, int index) { }

    protected void OnCollisionEnter2D(Collision2D other) {
        foreach (ContactPoint2D contact in other.contacts) {
            Collider2D thisCol = contact.collider;
            Collider2D otherCol = contact.otherCollider;

            if (System.Array.IndexOf(colliders, thisCol) == -1) {
                Collider2D temp = thisCol;
                thisCol = otherCol;
                otherCol = temp;
            }

            int index = System.Array.IndexOf(colliders, thisCol);
            HandleCollision(thisCol, otherCol, index);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        foreach (var col in colliders) {
            int count = col.Overlap(filter, results);

            for (int i = 0; i < count; i++) {
                if (results[i] ==  other) {
                    Collider2D thisCol = col;

                    int index = System.Array.IndexOf(colliders, thisCol);
                    HandleCollision(thisCol, other, index);
                }
            }
        }
    }
}