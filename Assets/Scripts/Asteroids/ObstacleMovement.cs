using UnityEngine;

public class ObstacleMovement : MonoBehaviour {
    [SerializeField] float speed;
    [SerializeField] float camOffset;
    float camLeft = -20f;

    void Update()
    {
        if (transform.position.x <= camLeft - camOffset) {
            Disable();
        } else {
            transform.position -= Vector3.right * speed * Time.deltaTime;
        }
    }

    public void Disable()
    {
        print("Disabled.");
    }
}