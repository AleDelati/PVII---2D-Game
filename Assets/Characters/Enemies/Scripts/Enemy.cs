using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //              -|Se encarga de gestionar caracteristicas generales de los enemigos|-

    //[SerializeField] private float xpDrop;
    [SerializeField] private int scoreDrop;

    public int GetScoreDrop() {
        return scoreDrop;
    }
}
