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

    //              ----|Variables|----

    //              ----|References|----

    //              ----|Functions|----

    private void Update() {
        if (player == null) {   //Verifica si el jugador sigue con vida
            OnMovementInput?.Invoke(Vector2.zero);
            return;
        } else {
            float distance = Vector2.Distance(player.position, transform.position);
            if (distance < chaseDistanceThreshold) {
                OnPointerInput?.Invoke(player.position);
                if(distance <= attackDistanceThreshold) {
                    //Atacando al jugador
                    OnMovementInput?.Invoke(Vector2.zero); 
                    if(passedTime >= attackDelay) {
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
            if(passedTime < attackDelay) {
                passedTime += Time.deltaTime;
            }
        }
    }
}
