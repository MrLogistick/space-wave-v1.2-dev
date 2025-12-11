using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] float threshold;

    [SerializeField] Camera cam;
    ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Size
        var shape = ps.shape;
        var height = cam.orthographicSize * 2f;

        shape.scale = new Vector2(height * cam.aspect + offset * 2f, height + 10f);
        shape.position = new Vector2(offset, 0f);

        // Trails
        var trails = ps.trails;
        trails.enabled = (ps.main.simulationSpeed >= threshold) ? enabled : !enabled;

        var curve = trails.widthOverTrail.curve;
        var key = curve.keys[0];

        var change = (ps.main.simulationSpeed - threshold) / 100f;
        
        trails.ratio = change;
        key.value = change;
        curve.MoveKey(0, key);

        // Speed
        // var main = ps.main;
        // main.simulationSpeed += Time.deltaTime * 0.1f;
    }
}