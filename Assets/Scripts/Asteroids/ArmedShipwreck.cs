using UnityEngine;

public class ArmedShipwreck : AsteroidBehaviour {
    [SerializeField] int shots;
    [SerializeField] float chargeTime;
    float offset;
    [SerializeField] GameObject shot;
    [SerializeField] Transform shootPoint;
    float time;

    protected override void OptionalOnEnable() {
        offset = Random.Range(-0.1f, 0.1f);
    }

    protected override void OptionalUpdate() {
        if (manager.aiIsSane && !shot) return;

        time += Time.deltaTime;

        if (time >= chargeTime && shots > 0 && !manager.postGame) {
            var instance = Instantiate(shot, shootPoint.position, shootPoint.rotation);
            instance.GetComponent<PlasmaShotBehaviour>().xSpeed = speed;

            time = 0f;
            shots--;
            offset = Random.Range(-0.1f, 0.1f);
        }
    }

    protected override void HandleCollision(Collider2D thisCol, Collider2D otherCol, int index) {
        if (otherCol.GetComponent<PlasmaShotBehaviour>()) return;
        base.HandleCollision(thisCol, otherCol, index);
    }
}