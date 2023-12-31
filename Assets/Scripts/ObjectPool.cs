using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [SerializeField] private GameObject[] objectPrefab;
    [SerializeField] private int poolSize = 5;

    private List<GameObject> pooledObjects;

    private void Start() {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++) {
            //Aleatoriza los enemigos en cada Pool
            int rand = Random.Range(0, objectPrefab.Length);
            GameObject obj = Instantiate(objectPrefab[rand]);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject() {
        foreach (GameObject obj in pooledObjects) {
            if (!obj.activeInHierarchy) {
                return obj;
            }
        }
        return null;
    }

}
