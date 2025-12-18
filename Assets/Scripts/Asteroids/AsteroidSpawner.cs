using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {
    // asteroid spawner, field spawner and pickup spawner

    // x=asteroids, y=fields
    [Header("Asteroids, Fields")]
    [SerializeField] Vector2 rateGap;
    [SerializeField] Vector2 initialGapProgress;
    [SerializeField] Vector2 rateError;
    [SerializeField] Vector2 density;
    float currentDensity = 0;

    [Header("Asteroids Extended")]
    [SerializeField] GameObject[] generalRoids;
    [SerializeField] GameObject[] specialRoids;
    [SerializeField] GameObject[] megaRoids;
    [SerializeField] GameObject[] pickup;
    [SerializeField] GameObject[] speedrings;
    [SerializeField] float generalWeight;
    [SerializeField] float specialWeight;
    [SerializeField] float megaWeight;
    [SerializeField] float pickupWeight;
    [SerializeField] float speedringWeight;
    [Space]
    [SerializeField] float spawnError;
    [SerializeField] float initialDensity;
    [Space]
    [SerializeField] float specialRoidDelay;
    bool specialRoidsActive = false;
    [HideInInspector] public bool megaroidActive;

    [Header("Fields Extended")]
    [SerializeField] Animator fieldWarningVisual;
    [SerializeField] float fieldWarningTime;
    [Space]
    [SerializeField] Vector2 fieldLife;
    int fieldsEndured;
    [Space]
    [SerializeField] float gameSpeedJump;

    [Header("Borders")]
    [SerializeField] float generalBorder;
    [SerializeField] float megaroidBorder;
    [SerializeField] float posOffset;

    GameManager manager;
    [SerializeField] Camera cam;

    IEnumerator Start() {
        var x = cam.transform.position.x + posOffset + cam.orthographicSize * cam.aspect;
        transform.position = new Vector2(x, 0f);

        currentDensity = initialDensity;

        manager = GameManager.instance;

        StartCoroutine(AsteroidSpawn(true));
        StartCoroutine(FieldSpawn(true));

        yield return new WaitForSeconds(specialRoidDelay);
        specialRoidsActive = true;
    }

    void Update() {
        if (!manager.postGame) return;
        fieldWarningVisual.speed *= manager.endMultiplier;
    }

    IEnumerator AsteroidSpawn(bool initial) {
        while (manager.gameSpeed > 0.5f) {
            float rate;

            if (initial) {
                initial = false;
                rate = initialGapProgress.x / manager.gameSpeed;
            }
            else {
                rate = rateGap.x / manager.gameSpeed;
            }
            
            rate += Random.Range(-rateError.x, rateError.x);
            yield return new WaitForSeconds(rate);

            for (int i = 0; i < currentDensity; i++) {
                float total = generalWeight + specialWeight + megaWeight + pickupWeight + speedringWeight;
                float roll;

                if (!specialRoidsActive) {
                    roll = 0f;
                } else {
                    
                    roll = Random.value * total;
                }

                if (roll < generalWeight) {
                    // General Asteroids
                    InstantiateAsteroid(generalRoids);
                }
                else if (roll < generalWeight + specialWeight) {
                    // Special Asteroids (ASB, Bomb, Shipwreck)
                    InstantiateAsteroid(specialRoids);
                }
                else if (roll < generalWeight + specialWeight + megaWeight) {
                    // Megaroids (Megaroid, Tunnelroid)
                    if (megaroidActive) {
                        InstantiateAsteroid(generalRoids);
                    }
                    else {
                        megaroidActive = true;
                        InstantiateAsteroid(megaRoids);                        
                    }

                }
                else if (roll < generalWeight + specialWeight + megaWeight + pickupWeight) {
                    // Pickup
                    InstantiateAsteroid(pickup);
                }
                else {
                    // Speedrings
                    InstantiateAsteroid(speedrings);
                }

                var error = Random.value * spawnError;
                yield return new WaitForSeconds(error);
            }
        }
    }

    void InstantiateAsteroid(GameObject[] list) {
        if (list.Length < 1) return;
        var i = Random.Range(0, list.Length);
        var roid = Instantiate(list[i], transform);
        var roidScript = roid.GetComponent<ObstacleMovement>();

        roidScript.camLeft = cam.transform.position.x - cam.orthographicSize * cam.aspect;

        float roidPos;
        switch (roidScript.roidType) {
            default:
                roidPos = Random.Range(-generalBorder, generalBorder);
                break;
            case ObstacleMovement.RoidType.Megaroid:
                roidPos = megaroidBorder * (Random.value < 0.5f ? -1 : 1);
                break;
            case ObstacleMovement.RoidType.Tunnelroid:
                roidPos = 0f;
                break;
        }

        roid.transform.position = new Vector2(transform.position.x, roidPos);
    }

    IEnumerator FieldSpawn(bool initial) {
        while (!manager.postGame) {
            float rate;

            if (initial) {
                initial = false;
                rate = initialGapProgress.y / manager.gameSpeed;
            }
            else {
                rate = rateGap.y / manager.gameSpeed;
            }

            rate += Random.Range(-rateError.y, rateError.y);
            yield return new WaitForSeconds(rate - fieldWarningTime);
            if (manager.postGame) yield break;

            fieldWarningVisual.SetTrigger("Switch");
            yield return new WaitForSeconds(fieldWarningTime);
            if (manager.postGame) yield break;

            currentDensity = density.y;

            yield return new WaitForSeconds(Random.Range(fieldLife.x, fieldLife.y));
            if (manager.postGame) yield break;

            fieldsEndured++;
            currentDensity = density.x;
            manager.AlterGameSpeedBy(gameSpeedJump);

            InstantiateAsteroid(speedrings);
            fieldWarningVisual.SetTrigger("Switch");
        }
    }
}