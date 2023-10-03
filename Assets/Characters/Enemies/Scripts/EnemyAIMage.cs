using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAIMage : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float chaseDistanceThreshold = 10.0f, attackDistanceThreshold = 0.8f, mantainDistanceThreshold = 5.0f;
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float passedTime = 1;

    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;

    [Header("Detection Circle Config")]
    [SerializeField] private Transform playerDetectionOrigin;
    [SerializeField] private float playerDetectionRadius;

    [Header("MovePositions")]
    [SerializeField] private Vector2[] movePositions;

    [Header("Projectile Config")]
    [SerializeField] private GameObject Projectile;
    [SerializeField] private Transform ProjectileSpawnPoint;

    //              ----|Variables|----
    private Vector3 startingPos;

    private float currentWaitTime;
    private int currentState;
    private Transform target;
    //              ----|References|----

    //              ----|Functions|----
    private void Start() {
        startingPos = transform.position;
        StartCoroutine(MageBehavior());
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
                target = collider.transform;
            }
        }
    }

    private IEnumerator MageBehavior() {

        while (true) {
            switch (currentState) {
                case 0:     //idle
                    StartCoroutine(Idle());
                    currentWaitTime = 0;
                    break;
                case 1:
                    StartCoroutine(Combat());
                    currentWaitTime = 0;
                    break;
            }

            //Debug.Log(currentState);
            yield return new WaitForSeconds(currentWaitTime);
            UpdateState();

        }

    }

    private void UpdateState() {
        if(target == null) {
            currentState = 0;
        } else {
            currentState = 1;
        }
    }

    private IEnumerator Idle() {
        //Vuelve a la posicion inicial al perder de vista al jugador
        MoveTo(startingPos);
        DetectPlayerColliders();
        yield return null;
    }

    private IEnumerator Combat() {
        float distance = Vector2.Distance(target.position, transform.position);
        if (distance < chaseDistanceThreshold) {
            OnPointerInput?.Invoke(target.position);
            if (distance <= mantainDistanceThreshold) {
                //Mantener distancia con el jugador
                Vector2 direction = target.position + transform.position;
                OnMovementInput?.Invoke(direction.normalized);
                yield return null;

            } else if (distance <= attackDistanceThreshold) {
                //Atacando al jugador
                OnMovementInput?.Invoke(Vector2.zero);
                if (passedTime >= attackDelay) {
                    passedTime = 0;
                    OnAttack?.Invoke();
                    yield return null;
                }

            } else {
                //Siguiendo al jugador
                Vector2 direction = target.position - transform.position;
                OnMovementInput?.Invoke(direction.normalized);
                yield return null;
            }
        }
        //Idle
        if (passedTime < attackDelay) {
            passedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void MoveTo(Vector3 targetPos) {
        float distance = Vector2.Distance(targetPos, transform.position);
        if (distance > 0.05f) {
            Vector3 direction = targetPos - transform.position;
            OnMovementInput?.Invoke(direction.normalized);
        } else {
            OnMovementInput?.Invoke(Vector2.zero);
            DetectPlayerColliders();
        }
    }

}
