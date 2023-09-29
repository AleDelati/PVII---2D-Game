using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private GameObject[] enemyPrefab;

    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float coolDown;

    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float coolDownI;

    //              ----|Variables|----

    //              ----|References|----

    //              ----|Functions|----

    void SpawnRandomEnemy() {
        int randomIndex = Random.Range(0, enemyPrefab.Length);
        GameObject randomEnemy = enemyPrefab[randomIndex];
        Instantiate(randomEnemy, transform.position, Quaternion.identity);
    }

    private void OnBecameVisible() {
        Debug.Log("Spawner dentro del rango de la camara, Activado");
        InvokeRepeating(nameof(SpawnRandomEnemy), coolDown, coolDownI);
    }

    private void OnBecameInvisible() {
        Debug.Log("Spawner fuera del rango de la camara, Desactivado");
        CancelInvoke(nameof(SpawnRandomEnemy));
    }

}
