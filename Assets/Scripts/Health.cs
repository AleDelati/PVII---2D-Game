using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float initialHP;
    [SerializeField] private float maxHP;
    
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool destroyOnDeath = false;

    //              ----|Variables|----
    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;
    private float currentHP;
    private float healEffectCooldown = 0.25f;

    //              ----|References|----
    private Agent _Agent;
    private SpriteRenderer _SpriteRenderer;
    private OnDeath _OnDeath;

    //              ----|Functions|----
    private void Start() {
        currentHP = initialHP;
    }

    private void OnEnable() {
        _Agent = GetComponent<Agent>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _OnDeath = GetComponent<OnDeath>();
        InitializeHealth();
    }

    public void InitializeHealth() {
        currentHP = initialHP;
        isDead = false;
    }

    public void SetHP(float value) {
        currentHP = value;

        _SpriteRenderer.color = Color.green;
        StartCoroutine(ColorReset());
    }

    public float GetHP() {      //Retorna los puntos de vida actuales
        return currentHP;
    }

    public float GetMaxHP() {   //Retorna los puntos de vida maximos
        return maxHP;
    }

    public void GetHit(int amount, GameObject sender) {

        if(isDead == true) {
            return;
        }else if(sender.layer == gameObject.layer) {
            return;
        } else {
            currentHP -= amount;
        }

        if(currentHP > 0) {
            OnHitWithReference?.Invoke(sender);
        } else {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;

            //Asigna los puntos al contador en caso de que el jugador elimine a un enemigo
            if (sender.CompareTag("Player") == true) {
                if(gameObject.GetComponent<Enemy>() != null) {
                    GameManager.instance.AddScore(gameObject.GetComponent<Enemy>().GetScoreDrop());
                    //Debug.Log("Puntuacion Actual: " + GameManager.instance.GetScore());
                }
            }
            //Añade la muerte del jugador al contador de muertes
            if(gameObject.CompareTag("Player") == true) {
                GameManager.instance.AddDeathCount();
            }

            //Instancia el cadaver del agente
            _OnDeath.LeaveCorpse();

            if(destroyOnDeath == true) {
                Destroy(gameObject);
            } else {
                gameObject.SetActive(false);
            }
        }

    }

    private IEnumerator ColorReset() {
        yield return new WaitForSeconds(healEffectCooldown);
        _SpriteRenderer.color = Color.white;
    }

}
