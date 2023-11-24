using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("Spawner Config")]
    [SerializeField]
    [Range(1.0f, 60.0f)]
    private float Interval;

    [SerializeField]
    [Range(1.0f, 30.0f)]
    private float repeatRate;
    private bool active = true;

    [SerializeField] private bool spawnLimit = true;
    [SerializeField] private int spawnCountLimit = 3;


    //              ----|Variables|----
    private int spawnCount = 0;
    //              ----|References|----
    private ObjectPool objectPool;
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

    //Si se limpio el nivel detiene la generacion de enemigos
    private void Disable() => active = false;

    void SpawnEnemy() {
        if (active) {
            GameObject pooledObject = objectPool.GetPooledObject();
            if (pooledObject != null) {
                pooledObject.transform.position = transform.position;
                pooledObject.transform.rotation = Quaternion.identity;
                pooledObject.SetActive(true);

                if(spawnCount >= spawnCountLimit) { active = false; }
            }
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
