using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float currentSpeed;
    float[] clampDir = { 0f, -90f };
    float currentRot;

    bool frozen;
    [SerializeField] float borderPosition;
    int gravity = -1;

    Transform ship;
    ShipAbility ab;
    ParticleSystem[] trails;
    PolygonCollider2D coll;

    [SerializeField] ShipData data;

    void Start() {
        if (!data) { 
            Debug.LogError("ShipData Not Found"); 
            enabled = false;
        } else {
            Instantiate(data.ship, transform);
            ship = GetComponentInChildren<PrefabSetup>().ship;
            ab = GetComponentInChildren<PrefabSetup>().ability;
            trails = GetComponentInChildren<PrefabSetup>().trails;

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

    public void FlipGravity(InputAction.CallbackContext context) { if (context.performed && !frozen) gravity *= -1; }
    public void ActivateAbility(InputAction.CallbackContext context) { if (context.performed && !ab.ability && !frozen) ab.ability = true; }
}
