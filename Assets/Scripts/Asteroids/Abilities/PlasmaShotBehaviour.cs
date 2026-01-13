using System.Collections;
using UnityEngine;

public class PlasmaShotBehaviour : MonoBehaviour {
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    float time;

    IEnumerator Start() {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }

    void Update() {
        float slowDown = 1f;
        slowDown *= GameManager.instance.postGame ? GameManager.instance.endMultiplier : 1f;
        time = Time.deltaTime * slowDown;

        transform.position += transform.up * speed * time;
    }
}