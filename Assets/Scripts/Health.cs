using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float currentHP;
    [SerializeField] private float maxHP;
    
    [SerializeField] public bool isDead = false;
    //              ----|Variables|----
    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    //              ----|References|----
    private Agent _Agent;

    //              ----|Functions|----
    private void OnEnable() {
        _Agent = GetComponent<Agent>();
    }

    public void InitializeHealth(int initialHP) {
        currentHP = initialHP;
        maxHP = initialHP;
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
            Destroy(gameObject);
        }

    }

}
