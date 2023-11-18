using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAIArcher : MonoBehaviour {

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
    [SerializeField] private float TPRadius = 1.0f;

    [Header("Projectile Config")]
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private Transform ProjectileSpawnPoint;

    //              ----|Variables|----
    private Vector3 startingPos;

    private float currentWaitTime;
    private int currentState;

    //              ----|References|----
    private Transform target;
    private GameObject ProjectileInstance;
    private Agent _Agent;

    //              ----|Functions|----
    private void Start() {
        startingPos = transform.position;
    }

    private void OnEnable() {
        StartCoroutine(ArcherBehavior());
        _Agent = GetComponent<Agent>();
    }

    //Dibuja un area de busqueda de objetivos
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Vector3 position = playerDetectionOrigin == null ? Vector3.zero : playerDetectionOrigin.position;
        Gizmos.DrawWireSphere(position, playerDetectionRadius);

        //TP Area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, TPRadius);
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

    private IEnumerator ArcherBehavior() {

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
        if (target == null) {
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
            _Agent.SetVelocity(5.0f);
            OnPointerInput?.Invoke(target.position);

            if (distance <= mantainDistanceThreshold) {
                //Mantener distancia con el jugador
                Vector2 direction = target.position - transform.position;
                //Reduce la velocidad mientras kitea
                _Agent.SetVelocity(2.5f);

                OnMovementInput?.Invoke(-direction.normalized);

                yield return null;

            } else if (distance <= attackDistanceThreshold) {

                //Atacando al jugador
                OnMovementInput?.Invoke(Vector2.zero);
                yield return new WaitForSeconds(1.5f);

                if (passedTime >= attackDelay && ProjectileInstance == null) {
                    passedTime = 0;
                    OnAttack?.Invoke();

                    //Disparar projectil
                    Vector2 direction = target.position - transform.position;

                    ProjectileInstance = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity);
                    ProjectileInstance.GetComponent<Projectile>().SetProjectile(direction.normalized, this.gameObject);
                    ProjectileInstance.transform.up = direction;

                    yield return new WaitForSeconds(0.5f);
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
