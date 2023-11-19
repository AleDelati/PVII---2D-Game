using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAIMage : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float chaseDistanceThreshold = 10.0f, attackDistanceThreshold = 0.8f, mantainDistanceThreshold = 5.0f;
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float passedTime = 1;

    [SerializeField] private GameObject DropPrefab;

    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;

    [Header("Detection Circle Config")]
    [SerializeField] private Transform playerDetectionOrigin;
    [SerializeField] private float playerDetectionRadius;

    [Header("Projectile Config")]
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private Transform ProjectileSpawnPoint;

    [Header("Summon Config")]
    [SerializeField] private GameObject SummonPrefab;
    [SerializeField] private float SummonTime = 1.5f;
    [SerializeField] private float summonRadius = 3.0f;

    [Header("TP Config")]
    [SerializeField] private Vector3[] TPpositions;
    [SerializeField] private float TPRadius = 1.0f;
    [SerializeField] private float TPStunBefore = 1.0f;
    [SerializeField] private float TPStunAfter = 1.0f;

    //              ----|Variables|----
    private Vector3 startingPos;

    private float currentWaitTime;
    private int currentState;
    private bool teleporting = false;

    //              ----|References|----
    private Transform target;
    private GameObject ProjectileInstance;
    private GameObject SummonInstance;
    private GameObject DropInstance;
    private ParticleSystem PS;

    //              ----|Functions|----
    private void Start() {
        startingPos = transform.position;
    }

    private void OnEnable() {
        StartCoroutine(MageBehavior());
        PS = GetComponent<ParticleSystem>();
    }

    //Dibuja un area de busqueda de objetivos
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Vector3 position = playerDetectionOrigin == null ? Vector3.zero : playerDetectionOrigin.position;
        Gizmos.DrawWireSphere(position, playerDetectionRadius);

        //Summon Area
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, summonRadius);

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
                case 2:
                    StartCoroutine(Summon());
                    currentWaitTime = SummonTime;
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
        } else if (SummonInstance == null && target != null) {
            currentState = 2;
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
                Vector2 direction = target.position - transform.position;
                OnMovementInput?.Invoke(-direction.normalized);

                foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, TPRadius)) {
                    //Si esta atascado al borde del mapa realiza un tp
                    if (collider.CompareTag("General Map") == true) {
                        PS.Play();
                        yield return new WaitForSeconds(TPStunBefore);
                        Teleport();
                        yield return new WaitForSeconds(TPStunAfter);
                        teleporting = false;
                    }
                }

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

    private IEnumerator Summon() {
        OnMovementInput?.Invoke(Vector2.zero);
        OnPointerInput?.Invoke(transform.position + new Vector3(10, 1, 0));
        yield return new WaitForSeconds(SummonTime);

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, summonRadius)) {
            //Si se esta lo suficientemente alejado del borde del mapa se summonea en el area especificada
            if (collider.CompareTag("General Map") != true) {
                if (SummonInstance == null) {
                    SummonInstance = Instantiate(SummonPrefab, transform.position + new Vector3(Random.Range(summonRadius - summonRadius*2, summonRadius), Random.Range(summonRadius - summonRadius*2, summonRadius), 0), Quaternion.identity);
                }
            } else {
                if (SummonInstance == null) {
                    SummonInstance = Instantiate(SummonPrefab, transform.position, Quaternion.identity);
                }
            }
        }

       
        yield return null;
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
    
    private void OnDestroy() {
        if (target != null) {   //Evita que se instancie el objeto si el jugador murio y se reinicia el nivel
            DropInstance = Instantiate(DropPrefab, transform.position, Quaternion.identity);
            DropInstance.name = "Key";
        }
    }

    private void Teleport() {
        if (!teleporting) {
            int randPos = Random.Range(0, TPpositions.Length);
            transform.position = TPpositions[randPos];
            teleporting = true;
        }
    }

}
