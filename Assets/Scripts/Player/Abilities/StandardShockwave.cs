using UnityEngine;

public class StandardShockwave : MonoBehaviour
{
    [SerializeField] float topSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float initialSpeed;
    float currentSpeed;

    [Range(1.001f, 1.01f)]
    [SerializeField] float scaleMultiplier;
    [SerializeField] Sprite[] stages;

    [SerializeField] float lifetime;
    float lifetimeElapsed = 0f;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        currentSpeed = initialSpeed;
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

            transform.localScale *= scaleMultiplier;
        }
    }
}