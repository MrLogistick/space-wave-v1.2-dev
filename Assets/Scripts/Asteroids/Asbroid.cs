using UnityEngine;

public class Asbroid : AsteroidBehaviour {
    // [SerializeField] float shotRate;
    // [SerializeField] GameObject shot;
    // [SerializeField] Transform shootPoint;
    // float time;

    // protected override void OptionalUpdate() {
    //     time += Time.deltaTime;

    //     if (time >= shotRate && !manager.postGame) {
    //         var instance = Instantiate(shot, shootPoint.position, shootPoint.rotation);
    //         instance.GetComponent<PlasmaShotBehaviour>().xSpeed = speed;
    //         time = 0f;
    //     }
    // }

    // protected override void HandleCollision(Collider2D thisCol, Collider2D otherCol, int index) {
    //     if (otherCol.GetComponent<PlasmaShotBehaviour>()) return;
    //     base.HandleCollision(thisCol, otherCol, index);
    // }
}