using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float currentSpeed;
    Vector2 clampDir = new Vector2(0f, -90f);
    float currentRot;

    bool frozen;
    [SerializeField] float borderPosition;
    int gravity = -1;

    Transform ship;
    ShipAbility ability;
    ParticleSystem[] trails;
    PolygonCollider2D coll;

    [SerializeField] ShipData data;

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
        }
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.y) >= borderPosition && !frozen) { Die("Gravity", false); }

        if (frozen) {
            coll.enabled = false;
        
            foreach (var trail in trails) {
                var emission = trail.emission;
                emission.enabled = false;
            }
        }

        if (!manager) return;

        foreach (var trail in trails) {
            var speed = trail.velocityOverLifetime;
            speed.x = new ParticleSystem.MinMaxCurve(-manager.gameSpeed);
        }
    }

    void FixedUpdate()
    {
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

    void Die(string deathBy, bool explode) {
        frozen = true;
        manager.TriggerPostGame(deathBy);

        if (explode) {
            ship.gameObject.SetActive(false);
            ability.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.GetComponent<ObstacleMovement>()) return;
        var obj = other.GetComponent<ObstacleMovement>();
        
        switch (obj.roidType) {
            case ObstacleMovement.RoidType.Asteroid:
                obj.Disable(true);
                Die("Asteroid", true);
                break;
            case ObstacleMovement.RoidType.Shiproid:
                obj.Disable(true);
                Die("Shipwreck", true);
                break;
            case ObstacleMovement.RoidType.Bombroid:
                obj.Disable(true);
                Die("Bombroid", true);
                break;
            case ObstacleMovement.RoidType.Asbroid:
                obj.Disable(true);
                Die("ASBroid", true);
                break;
            case ObstacleMovement.RoidType.Megaroid:
                Die("Megaroid", true);
                break;
            case ObstacleMovement.RoidType.Tunnelroid:
                Die("Tunnelroid", true);
                break;
            case ObstacleMovement.RoidType.Pickup:
                obj.Disable(true);
                ability.ChangeCountBy(1);
                break;
            case ObstacleMovement.RoidType.Speedring:
                manager.AlterGameSpeedBy(obj.gameSpeedJump);
                obj.PlaySecondaryEffect();
                break;
        }
    }

    public void FlipGravity(InputAction.CallbackContext context) { if (context.performed && !frozen) gravity *= -1; }
    public void ActivateAbility(InputAction.CallbackContext context) { if (context.performed && !ability.activated && !frozen) ability.activated = true; }
}
