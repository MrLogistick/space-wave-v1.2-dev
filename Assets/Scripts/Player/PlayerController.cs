using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float currentSpeed;
    Vector2 clampDir = new Vector2(0f, -90f);
    float currentRot = -45f;

    bool frozen = true;
    [SerializeField] float borderPosition;
    int gravity = -1;

    Transform ship;
    ShipAbility ability;
    ParticleSystem[] trails;
    PolygonCollider2D coll;

    [SerializeField] ShipData data;
    [SerializeField] bool collisionsEnabled;

    [SerializeField] ParticleSystem explosion;

    GameManager manager;

    void Start() {
        manager = GameManager.instance;

        if (!data) { 
            Debug.LogError("ShipData Not Found"); 
            enabled = false;
        } else {
            Instantiate(data.ship, transform);

            var prefab = GetComponentInChildren<PrefabSetup>();
            ship = prefab.ship;
            ability = prefab.ability;
            trails = prefab.trails;
            coll = prefab.coll;

            currentRot = ship.rotation.z;

            coll.enabled = collisionsEnabled ? true : false;
        }
    }

    void Update() {
        if (Mathf.Abs(transform.position.y) >= borderPosition && !frozen) { Die("Gravity", false); }
        coll.enabled = !frozen;

        if (!manager) return;

        if(manager.postGame) {
            var main = explosion.main;
            main.simulationSpeed *= manager.endMultiplier;

            foreach (var trail in trails) {
                var emission = trail.emission;
                emission.enabled = false;

                main = trail.main;
                main.simulationSpeed *= manager.endMultiplier;
            }
        } else {
            foreach (var trail in trails) {
                var speed = trail.velocityOverLifetime;
                speed.x = -manager.gameSpeed;
            }
        }
    }

    void FixedUpdate() {
        if (frozen) return;

        currentSpeed += data.acceleration * gravity * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -data.moveSpeed, data.moveSpeed);

        float targetRot = Mathf.Lerp(clampDir.x, clampDir.y, gravity * -1);
        currentRot = Mathf.MoveTowards(currentRot, targetRot, data.turnSpeed * Time.deltaTime);

        var newPos = transform.position;
        newPos.y += currentSpeed;
        transform.position = newPos;

        ship.rotation = Quaternion.Euler(0f, 0f, currentRot);
    }

    public void AlterAbility() {
        ability.ChangeCountBy(1);
    }

    public void ApplyRootMotion() {
        GetComponent<Animator>().applyRootMotion = true;
        frozen = false;
    }

    public void Die(string deathBy, bool explode) {
        frozen = true;
        manager.TriggerPostGame(deathBy);

        if (explode) {
            ship.GetComponent<SpriteRenderer>().enabled = false;
            coll.enabled = false;
            ability.gameObject.SetActive(false);

            explosion.Play();
        }
    }

    public void FlipGravity(InputAction.CallbackContext context) { if (context.performed && !frozen) gravity *= -1; }
    public void ActivateAbility(InputAction.CallbackContext context) { if (context.performed && !ability.activated && !frozen) ability.activated = true; }
}
