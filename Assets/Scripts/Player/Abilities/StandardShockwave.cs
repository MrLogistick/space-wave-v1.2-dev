using UnityEngine;

public class StandardShockwave : MonoBehaviour
{
    [SerializeField] float topSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float initialSpeed;
    float currentSpeed;

    [SerializeField] float initialSize;
    [SerializeField] float maxScale;
    [SerializeField] float scaleMultiplier;
    float currentScale;

    [SerializeField] float lifetime;
    float lifetimeElapsed = 0f;

    float time;
    float slowDown = 1f;

    GameManager manager;

    void Start() {
        currentSpeed = initialSpeed;
        currentScale = initialSize;
        manager = GameManager.instance;
    }

    void Update() {
        if (lifetimeElapsed >= lifetime) {
            Destroy(gameObject);
        }
        else {
            if (manager.postGame) {
                slowDown *= manager.endMultiplier;
                time = Time.deltaTime * slowDown;
                GetComponent<EdgeCollider2D>().enabled = false;
            }
            else {
                time = Time.deltaTime;
            }

            if (!GetComponent<SpriteRenderer>().enabled) {
                Destroy(gameObject);
            }

            lifetimeElapsed += time;

            currentSpeed += acceleration * time;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, topSpeed);
            transform.position += Vector3.right * currentSpeed * time;

            currentScale += scaleMultiplier * time;
            currentScale = Mathf.Clamp(currentScale, 0f, maxScale);
            transform.localScale = Vector3.one * currentScale;
        }
    }
}