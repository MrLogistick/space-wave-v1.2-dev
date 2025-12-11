using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float currentSpeed;
    float[] clampDir = { 0f, -90f };
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
        if (Mathf.Abs(transform.position.y) >= borderPosition) { Die("Gravity"); }

        // foreach (var trail in trails) {
        //    var main = trail.main;
        //  main.simulationSpeed += Time.deltaTime;
        //}
    }

    void FixedUpdate()
    {
        if (frozen) return;

        currentSpeed += data.acceleration * gravity * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -data.moveSpeed, data.moveSpeed);

        float targetRot = Mathf.Lerp(clampDir[0], clampDir[1], gravity * -1);
        currentRot = Mathf.MoveTowards(currentRot, targetRot, data.turnSpeed * Time.deltaTime);

        var newPos = transform.position;
        newPos.y += currentSpeed;
        transform.position = newPos;

        ship.rotation = Quaternion.Euler(0f, 0f, currentRot);
    }

    void Die(string deathBy)
    {
        if (deathBy == "Gravity") {
            frozen = true;
            foreach (var trail in trails) {
                var emission = trail.emission;
                emission.enabled = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        var obj = other.gameObject;
        print("Touchdown!");
        
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
