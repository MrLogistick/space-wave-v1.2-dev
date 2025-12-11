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

    [SerializeField] Sprite[] stages;

    [SerializeField] float lifetime;
    float lifetimeElapsed = 0f;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        currentSpeed = initialSpeed;
        transform.localScale = Vector3.one * initialSize;
    }

    void Update()
    {
        if (lifetimeElapsed >= lifetime)
        {
            Destroy(gameObject);
        } else
        {
            lifetimeElapsed += Time.deltaTime;
            float currentStage = Mathf.Clamp(lifetimeElapsed / (lifetime / stages.Length), 0, stages.Length - 1);
            sr.sprite = stages[Mathf.FloorToInt(currentStage)];

            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, topSpeed);
            transform.position += Vector3.right * currentSpeed * Time.deltaTime;

            currentScale += scaleMultiplier * Time.deltaTime;
            currentScale = Mathf.Clamp(currentScale, 0f, maxScale);
            transform.localScale = Vector3.one * currentScale;
        }
    }
}