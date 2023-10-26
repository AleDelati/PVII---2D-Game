using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField]
    [Range(1.0f, 60.0f)]
    private float coolDown;

    [SerializeField]
    [Range(1.0f, 30.0f)]
    private float coolDownI;

    //              ----|Variables|----
    private ObjectPool objectPool;
    //              ----|References|----
    [SerializeField] private GameObject _ExitDoor;
    //              ----|Functions|----
    private void Start() {
        objectPool = GetComponent<ObjectPool>();
    }

    void SpawnEnemy() {
        //Si la puerta de salida esta abierta detiene la generacion de nuevos enemigos en los puntos de spawn
        if (_ExitDoor.GetComponent<ExitDoor>().GetDoorState() == false) {
            GameObject pooledObject = objectPool.GetPooledObject();
            if (pooledObject != null) {
                pooledObject.transform.position = transform.position;
                pooledObject.transform.rotation = Quaternion.identity;
                pooledObject.SetActive(true);
            }
        } else {
            Debug.Log("Puerta de salida abierta, Spawn de enemigo cancelado");
        }
    }

    private void OnBecameVisible() {
        //Debug.Log("Spawner dentro del rango de la camara, Activado");
        InvokeRepeating(nameof(SpawnEnemy), coolDown, coolDownI);
    }

    private void OnBecameInvisible() {
        //Debug.Log("Spawner fuera del rango de la camara, Desactivado");
        CancelInvoke(nameof(SpawnEnemy));
    }

}
