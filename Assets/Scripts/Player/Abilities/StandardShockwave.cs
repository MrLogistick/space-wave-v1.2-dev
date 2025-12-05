using UnityEngine;

public class StandardShockwave : MonoBehaviour
{
    [SerializeField] float topSpeed;
    [SerializeField] float acceleration;
    [SerializeField] Sprite[] stages;

    [SerializeField] float lifetime;
    float lifetimeElapsed = 0f;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (lifetimeElapsed >= lifetime)
        {
            Destroy(gameObject);
        } else
        {
            lifetimeElapsed += Time.deltaTime;
            
            float currentStage = lifetimeElapsed / (lifetime / stages.Length);
            sr.sprite = stages[Mathf.FloorToInt(currentStage)];
        }
    }
}