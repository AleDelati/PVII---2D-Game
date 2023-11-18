using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private Transform player;
    
    [SerializeField] private float chaseDistanceThreshold = 3, attackDistanceThreshold = 0.8f;
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float passedTime = 1;

    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;

    [Header("Detection Circle Config")]
    [SerializeField] private Transform playerDetectionOrigin;
    [SerializeField] private float playerDetectionRadius;

    //              ----|Variables|----
    private Vector3 startingPos;
    //              ----|References|----

    //              ----|Functions|----
    private void Start() {
        startingPos = transform.position;
    }

    private void Update() {
        BasicBehavior();
    }

    //Dibuja un area de busqueda de objetivos
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Vector3 position = playerDetectionOrigin == null ? Vector3.zero : playerDetectionOrigin.position;
        Gizmos.DrawWireSphere(position, playerDetectionRadius);
    }

    public void DetectPlayerColliders() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(playerDetectionOrigin.position, playerDetectionRadius)) {

            //Si se detecta un jugador en el area de busqueda
            if (collider.CompareTag("Player") == true) {
                Transform transform = collider.transform;
                player = collider.transform;
            }
        }
    }

    private void BasicBehavior() {
        if (player == null) {   //Verifica si el jugador sigue con vida
           
            DetectPlayerColliders();
            if(player == null) {
                //Vuelve a la posicion inicial al perder de vista al jugador
                float distance = Vector2.Distance(startingPos, transform.position);
                if (distance > 0.05f) {
                    Vector3 direction = startingPos - transform.position;
                    OnMovementInput?.Invoke(direction.normalized);
                } else {
                    OnMovementInput?.Invoke(Vector2.zero);
                    DetectPlayerColliders();
                }
            }
        } else {
            float distance = Vector2.Distance(player.position, transform.position);
            if (distance < chaseDistanceThreshold) {
                OnPointerInput?.Invoke(player.position);
                if (distance <= attackDistanceThreshold) {
                    //Atacando al jugador
                    OnMovementInput?.Invoke(Vector2.zero);
                    if (passedTime >= attackDelay) {
                        passedTime = 0;
                        OnAttack?.Invoke();
                    }

                } else {
                    //Siguiendo al jugador
                    Vector2 direction = player.position - transform.position;
                    OnMovementInput?.Invoke(direction.normalized);
                }
            }
            //Idle
            if (passedTime < attackDelay) {
                passedTime += Time.deltaTime;
            }
        }
    }

}
