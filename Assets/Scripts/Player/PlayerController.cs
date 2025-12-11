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

    void Start() {
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
        if (Mathf.Abs(transform.position.y) >= borderPosition && !frozen) { Die("Gravity"); }

        if (frozen) {
            coll.enabled = false;
        
            foreach (var trail in trails) {
                var emission = trail.emission;
                emission.enabled = false;
            }
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

    void Die(string deathBy) {
        frozen = true;
        GameManager.instance.TriggerPostGame(deathBy);

        if (deathBy.Contains("roid")) {
            ship.gameObject.SetActive(false);
            ability.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        var obj = other.gameObject;
        
        if (obj.CompareTag("Asteroid")) {
            Die("Asteroid");
        }

        if (obj.CompareTag("Pickup")) {
            obj.GetComponent<ObstacleMovement>().Disable();
            ability.ChangeCountBy(1);
        }
    }

    public void FlipGravity(InputAction.CallbackContext context) { if (context.performed && !frozen) gravity *= -1; }
    public void ActivateAbility(InputAction.CallbackContext context) { if (context.performed && !ability.activated && !frozen) ability.activated = true; }
}
