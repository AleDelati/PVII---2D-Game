using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    //              -|References|-
    [SerializeField] public Vector2 PointerPosition { get; set; }
   
    private GameObject shield;
    private GameObject player;

    //              -|Functions|-

    private void Start() {
        shield = GameObject.Find("Shield");
        player = GameObject.Find("Player");
    }

    private void Update() {
        UpdateShield();
    }

    //Mueve el escudo hacia la direccion en la que esta apuntando el jugador
    private void UpdateShield() {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        //Invierte la escala del escudo dependiendo en que direccion se la este apundando
        Vector2 scale = transform.localScale;
        if (direction.x < 0) {
            scale.y = -1;
        } else if (direction.x > 0) {
            scale.y = 1;
        }
        transform.localScale = scale;

        //Varia el SortingOrder del escudo dependiendo del angulo actual
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) {
            shield.GetComponent<SpriteRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 1;
        } else {
            shield.GetComponent<SpriteRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }

}
