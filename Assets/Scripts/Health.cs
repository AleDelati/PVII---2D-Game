using System.Collections;
using System.Collections.Generic;
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

    //              ----|References|----
    private Agent _Agent;

    //              ----|Functions|----
    private void Start() {
        currentHP = initialHP;
    }

    private void OnEnable() {
        _Agent = GetComponent<Agent>();
        InitializeHealth();
    }

    public void InitializeHealth() {
        currentHP = initialHP;
        isDead = false;
    }

    public void SetHP(float value) {
        currentHP = value;
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
            if(destroyOnDeath == true) {
                Destroy(gameObject);
            } else {
                gameObject.SetActive(false);
            }
        }

    }

}
