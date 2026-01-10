using UnityEngine;

public class PersistantObject : MonoBehaviour {
    static PersistantObject instance;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}