using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    private Vector3 startingPosition;

    private void Start() {
        startingPosition = transform.position;
    }

    private Vector3 getRoamingPosition() {
        return startingPosition + Auxiliares.GetRandomDir() * Random.Range(10f, 70f);
    }

}
