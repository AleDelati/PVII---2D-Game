using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnemyAIMageFinal : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float chaseDistanceThreshold = 10.0f, attackDistanceThreshold = 0.8f, mantainDistanceThreshold = 5.0f;
    [SerializeField] private float attackDelay = 3;

    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;

    [Header("Detection Circle Config")]
    [SerializeField] private Transform playerDetectionOrigin;
    [SerializeField] private float playerDetectionRadius;

    [Header("Projectile Config")]
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private Transform ProjectileSpawnPoint;

    [Header("TP Config")]
    [SerializeField] private float TPRadius = 1.0f;
    [SerializeField] private float TPStunBefore = 0.5f;
    [SerializeField] private float TPStunAfter = 1.5f;
    [SerializeField] private AudioClip TPAudioclip;
    [SerializeField] private ParticleSystem TPPS;

    [Header("Cast Spells Config")]
    [SerializeField] private float SpecialDelay = 10f;
    [SerializeField] Vector3 castSpellsPos;
    [SerializeField] float castSpellsArea = 10.0f;
    [SerializeField] private Vector3[] castPositions;

    [Header("Summon Config")]
    [SerializeField] private GameObject PreSummonPrefab;
    [SerializeField] private GameObject SummonPrefab;
    [SerializeField] private float summonDelay = 15f;
    [SerializeField] private float SummonStunBefore = 1.5f;
    [SerializeField] private float SummonStunBetwen = 1.0f;
    [SerializeField] private float SummonStunAfter = 1.5f;
    [SerializeField] private float summonMainDelay = -3f;
    [SerializeField] private ParticleSystem summonParticles;

    [Header("Special Attack A - Ametralladora")]
    [SerializeField] private int SpecialAQuantity = 8;
    [SerializeField] private float SpecialAMainDelay = -5f;
    [SerializeField] private float SpecialAStunBefore = 1.5f;
    [SerializeField] private float SpecialAStunBetwen = 0.1f;
    [SerializeField] private float SpecialAStunAfter = 1.5f;
    [SerializeField] private ParticleSystem SpecialAParticles;

    [Header("Special Attack B - TP Player")]
    [SerializeField] private float SpecialBMainDelay = -3f;
    [SerializeField] private float SpecialBStunBefore = 3.0f;
    [SerializeField] private float SpecialBStunAfter = 0.5f;
    [SerializeField] private AudioClip SpecialBAudioclip;
    public UnityEvent OnSpecialAttackBBegin;
    public UnityEvent OnSpecialAttackBEnd;

    [Header("Special Attack C - Lluvia de Projectiles")]
    [SerializeField] private float SpecialCMainDelay = -3f;
    [SerializeField] private float SpecialCStunBefore = 1.0f;
    [SerializeField] private float SpecialCStunAfter = 1.0f;
    [SerializeField] private GameObject SpecialCPrefab;


    //              ----|Variables|----
    private Vector3 startingPos;

    private float currentWaitTime;
    private int currentState;
    private bool teleporting = false;

    private float mainCooldown = 0;   //Main cooldown
    private float summonCooldown = 0; //Summon cooldown
    private float specialACooldown = 0;

    private float currentHealth;

    //              ----|References|----
    private Transform target;
    private GameObject ProjectileInstance;
    private GameObject SummonInstance;
    private GameObject PreSummonInstance;
    private Health health;
    private AudioSource AS;

    //              ----|Functions|----
    private void Start() {
        startingPos = transform.position;
        castSpellsPos = startingPos;
    }

    private void OnEnable() {
        StartCoroutine(MageBehavior());
        health = GetComponent<Health>();
        AS = GetComponent<AudioSource>();

        currentHealth = health.GetHP();
    }

    //Dibuja un area de busqueda de objetivos
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Vector3 position = playerDetectionOrigin == null ? Vector3.zero : playerDetectionOrigin.position;
        Gizmos.DrawWireSphere(position, playerDetectionRadius);

        //Cast Spells Area
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(castSpellsPos, castSpellsArea);

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
                    currentWaitTime = 0;
                    break;
                case 3:
                    StartCoroutine(SpecialAttackA());
                    currentWaitTime = 0;
                    break;
                case 4:
                    StartCoroutine(SpecialAttackB());
                    currentWaitTime = 0;
                    break;
                case 5:
                    StartCoroutine(SpecialAttackC());
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
        } else if (summonCooldown <= 0 && target != null) {
            currentState = 2;
        } else if (specialACooldown <= 0) {
            //Elige un ataque especial aleatorio
            int rand = Random.Range(0, 3);
            switch (rand) {
                case 0:
                    currentState = 3;   //Special Attack A
                    break;
                case 1:
                    currentState = 4;  //Special Attack B
                    break;
                case 2:
                    currentState = 5;
                    break;
            }

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
        if (distance < chaseDistanceThreshold) {    //Si el enemigo esta dentro del rango de seguimiento
            OnPointerInput?.Invoke(target.position);

            //Si recibe un golpe se teletransporta
            if(health.GetHP() != currentHealth) {
                currentHealth = health.GetHP();
                StartCoroutine(Teleport());
            }
            teleporting = false;

            if (distance <= mantainDistanceThreshold) {     //Si el enemigo se acerca demaciado
                //Mantener distancia con el jugador
                Vector2 direction = target.position - transform.position;
                OnMovementInput?.Invoke(-direction.normalized);

                foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, TPRadius)) {
                    //Si esta fuera del area del castSpell y el jugador esta cerca se teletransporta
                    if (collider.CompareTag("Player") == true && !CheckCastSpeelArea()) {
                        yield return new WaitForSeconds(TPStunBefore);
                        StartCoroutine(Teleport());
                        yield return new WaitForSeconds(TPStunAfter);
                        teleporting = false;
                    }
                }


                yield return null;

            } else if (distance <= attackDistanceThreshold) {   //Si el enemigo esta dentro del rango de ataque


                //Atacando al jugador
                OnMovementInput?.Invoke(Vector2.zero);
                yield return new WaitForSeconds(1.5f);

                if (mainCooldown >= attackDelay) {
                    mainCooldown = 0;
                    OnAttack?.Invoke();

                    //Disparar projectil
                    Vector2 direction = target.position - transform.position;

                    ProjectileInstance = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity);
                    ProjectileInstance.GetComponent<Projectile>().SetProjectile(direction.normalized, this.gameObject);

                    GetComponent<AgentProjectile>().PlayOnSpawnAudio();

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
        if (mainCooldown < attackDelay) {
            mainCooldown += Time.deltaTime;

            summonCooldown -= Time.deltaTime;
            specialACooldown -= Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator Summon() {
        OnMovementInput?.Invoke(Vector2.zero);
        OnPointerInput?.Invoke(transform.position + new Vector3(10, 1, 0));
        mainCooldown = summonMainDelay;

        if(PreSummonInstance == null) {
            PreSummonInstance = Instantiate(PreSummonPrefab, castSpellsPos + new Vector3(Random.Range(castSpellsArea - castSpellsArea * 2, castSpellsArea), Random.Range(castSpellsArea - castSpellsArea * 2, castSpellsArea), 0), Quaternion.identity);
        }

        yield return new WaitForSeconds(SummonStunBefore);

        if(SummonInstance == null && summonCooldown <= 0) {
            SummonInstance = Instantiate(SummonPrefab, PreSummonInstance.transform.position, Quaternion.identity);
            SummonInstance.GetComponent<Health>().SetDestroyOnDeath(true);
            yield return new WaitForSeconds(SummonStunBetwen);
            Destroy(PreSummonInstance.gameObject);
        }

        summonCooldown = summonDelay;

        yield return new WaitForSeconds(SummonStunAfter);
    }

    //              -|Special Attack A|-
    private IEnumerator SpecialAttackA() {

        //Atacando al jugador
        OnMovementInput?.Invoke(Vector2.zero);

        specialACooldown = SpecialDelay;
        mainCooldown = SpecialAMainDelay;

        SpecialAParticles.Play();
        yield return new WaitForSeconds(SpecialAStunBefore);

        for (int i = 0; i < SpecialAQuantity; i++) {
            yield return new WaitForSeconds(SpecialAStunBetwen);

            OnAttack?.Invoke();

            //Disparar projectil
            Vector2 direction = target.position - transform.position;

            ProjectileInstance = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity);
            ProjectileInstance.GetComponent<Projectile>().SetProjectile(direction.normalized, this.gameObject);

            GetComponent<AgentProjectile>().PlayOnSpawnAudio();
        }

        yield return new WaitForSeconds(SpecialAStunAfter);
    }

    //              -|Special Attack B|-
    private IEnumerator SpecialAttackB() {

        OnMovementInput?.Invoke(Vector2.zero);
        OnPointerInput?.Invoke(transform.position + new Vector3(10, 1, 0));

        specialACooldown = SpecialDelay;
        mainCooldown = SpecialBMainDelay;

        OnSpecialAttackBBegin?.Invoke();

        yield return new WaitForSeconds(SpecialBStunBefore);

        target.transform.position = castSpellsPos + new Vector3(Random.Range(castSpellsArea - castSpellsArea * 2, castSpellsArea), Random.Range(castSpellsArea - castSpellsArea * 2, castSpellsArea), 0);
        AS.PlayOneShot(SpecialBAudioclip);

        yield return new WaitForSeconds(SpecialBStunAfter);

        OnSpecialAttackBEnd?.Invoke();

    }

    //              -|Special Attack C|-
    private IEnumerator SpecialAttackC() {
        for(int i = 0; i < castPositions.Length; i++) {

            specialACooldown = SpecialDelay;
            mainCooldown = SpecialCMainDelay;

            int rand = Random.Range(0, 2);
            if(rand == 0) {
                yield return new WaitForSeconds(SpecialCStunBefore);
            }
            
            //Disparar projectil
            Vector2 direction = target.position - castPositions[i];

            ProjectileInstance = Instantiate(SpecialCPrefab, castPositions[i], Quaternion.identity);
            ProjectileInstance.GetComponent<Projectile>().SetProjectile(direction.normalized, this.gameObject);

            GetComponent<AgentProjectile>().PlayOnSpawnAudio();
        }
        
        yield return new WaitForSeconds(SpecialCStunAfter);
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

    private IEnumerator Teleport() {

        //Se teletransporta a una posicion asignada si esta dentro del area del castSpeel, de lo contrario vuelve a la posicion inicial
        if (!teleporting) {

            if (CheckCastSpeelArea() && !teleporting){
                int randPos = Random.Range(0, castPositions.Length);
                transform.position = castPositions[randPos];
            } else {
                transform.position = startingPos;
            }

            TPPS.Play();
            AS.PlayOneShot(TPAudioclip);
            teleporting = true;
            yield return null;
        }
    }

    //Retorna si el mago esta dentro del area del castSpell
    private bool CheckCastSpeelArea() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(castSpellsPos, castSpellsArea)) {
            if (collider.CompareTag(gameObject.tag) == true) {
                return true;
            }
        }
        return false;
    }

}
