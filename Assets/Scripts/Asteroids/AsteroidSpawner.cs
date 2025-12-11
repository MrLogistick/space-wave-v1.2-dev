using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {
    // asteroid spawner, field spawner and pickup spawner

    // x=asteroids, y=fields
    [SerializeField] Vector2 initialRate;
    [SerializeField] Vector2 maxRate;
    [SerializeField] Vector2 rateError;
    [SerializeField] Vector2 rateJump;
    [SerializeField] Vector2 density;
    float currentRoidRate = 0f;
    float currentFieldRate = 0f;
    float currentDensity = 0;

    [SerializeField] Animator fieldWarningVisual;
    [SerializeField] float fieldWarningTime;

    [SerializeField] Vector2 fieldLife;
    bool fieldActive = false;

    [SerializeField] GameObject[] targetObject;

    [SerializeField] float specialRoidDelay;
    bool specialRoidsActive = false;

    GameManager manager;

    IEnumerator Start() {
        currentRoidRate = initialRate.x;
        currentFieldRate = initialRate.y;
        currentDensity = density.x;

        manager = GameManager.instance;

        FieldSpawn();

        yield return new WaitForSeconds(specialRoidDelay);
        specialRoidsActive = true;
    }

    void Update() {
        if (fieldActive) {
            currentDensity = density.y;
        } else {
            currentDensity = density.x;
        }
    }

    IEnumerator FieldSpawn() {
        var rate = currentFieldRate + Random.Range(-rateError.y, rateError.y);
        yield return new WaitForSeconds(rate - fieldWarningTime);

        fieldWarningVisual.SetTrigger("Switch");
        yield return new WaitForSeconds(fieldWarningTime);

        fieldWarningVisual.SetTrigger("Switch");
        fieldActive = true;

        currentFieldRate = Mathf.Min(currentFieldRate - rateJump.y, maxRate.y);

        yield return new WaitForSeconds(Random.Range(fieldLife.x, fieldLife.y));

        fieldActive = false;

        FieldSpawn();
    }
}