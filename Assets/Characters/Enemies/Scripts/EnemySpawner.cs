using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField]
    [Range(1.0f, 60.0f)]
    private float Interval;

    [SerializeField]
    [Range(1.0f, 30.0f)]
    private float repeatRate;
    private bool active = true;

    //              ----|Variables|----
    private ObjectPool objectPool;
    //              ----|References|----

    //              ----|Functions|----
    private void Start() {
        objectPool = GetComponent<ObjectPool>();
    }

    private void OnEnable() {
        GameEvents.onLevelCleared += Disable;
    }

    private void OnDisable() {
        GameEvents.onLevelCleared -= Disable;
    }

    private void Disable() => active = false;

    void SpawnEnemy() {
        //Si se limpio el nivel detiene la generacion de enemigos
        if (active) {
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
        InvokeRepeating(nameof(SpawnEnemy), Interval, repeatRate);
    }

    private void OnBecameInvisible() {
        //Debug.Log("Spawner fuera del rango de la camara, Desactivado");
        CancelInvoke(nameof(SpawnEnemy));
    }

}
