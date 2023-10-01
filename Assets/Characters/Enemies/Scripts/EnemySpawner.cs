using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private GameObject[] enemyPrefab;

    [SerializeField]
    [Range(1.0f, 60.0f)]
    private float coolDown;

    [SerializeField]
    [Range(1.0f, 30.0f)]
    private float coolDownI;

    //              ----|Variables|----

    //              ----|References|----
    [SerializeField] private GameObject _ExitDoor;
    //              ----|Functions|----
    void SpawnRandomEnemy() {
        //Si la puerta de salida esta abierta detiene la generacion de nuevos enemigos en los puntos de spawn
        if (_ExitDoor.GetComponent<ExitDoor>().GetDoorState() == false) {
            int randomIndex = Random.Range(0, enemyPrefab.Length);
            GameObject randomEnemy = enemyPrefab[randomIndex];
            Instantiate(randomEnemy, transform.position, Quaternion.identity);
        } else {
            Debug.Log("Puerta de salida abierta, Spawn de enemigo cancelado");
        }
    }

    private void OnBecameVisible() {
        //Debug.Log("Spawner dentro del rango de la camara, Activado");
        InvokeRepeating(nameof(SpawnRandomEnemy), coolDown, coolDownI);
    }

    private void OnBecameInvisible() {
        //Debug.Log("Spawner fuera del rango de la camara, Desactivado");
        CancelInvoke(nameof(SpawnRandomEnemy));
    }

}
