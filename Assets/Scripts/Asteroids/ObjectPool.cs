using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;
    [SerializeField] int amount;
    Queue<GameObject> pool = new Queue<GameObject>();

    void Start() {
        for (int i = 0; i < amount; i++) {
            var instance = Instantiate(prefab, parent);
            instance.SetActive(false);
            pool.Enqueue(instance);
        }
    }

    public GameObject GetFromPool() {
        GameObject obj;

        if (pool.Count == 0) {
            obj = Instantiate(prefab, parent);
            obj.SetActive(true);
            return obj;
        }

        obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj) {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}