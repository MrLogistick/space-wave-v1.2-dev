using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;
    float currentSpeed;
    float[] clampDir = { 0f, -90f };
    [SerializeField] float turnSpeed;
    float currentRot;

    int gravity = -1;

    [SerializeField] Transform ship;
    [SerializeField] ShipAbility ab;

    void Start()
    {
        currentRot = ship.rotation.z;
    }

    void FixedUpdate()
    {
        currentSpeed += acceleration * gravity * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -moveSpeed, moveSpeed);

        float targetRot = Mathf.Lerp(clampDir[0], clampDir[1], gravity * -1);
        currentRot = Mathf.MoveTowards(currentRot, targetRot, turnSpeed * Time.deltaTime);

        var newPos = transform.position;
        newPos.y += currentSpeed;
        transform.position = newPos;

        ship.rotation = Quaternion.Euler(0f, 0f, currentRot);
    }

    public void FlipGravity(InputAction.CallbackContext context) { if (context.performed) gravity *= -1; }
    public void ActivateAbility(InputAction.CallbackContext context) { if (context.performed && !ab.ability) ab.ability = true; }
}
