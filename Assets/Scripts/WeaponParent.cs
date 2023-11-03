using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponParent : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] public Vector2 PointerPosition { get; set; }
    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private float coolDown = 0.3f;

    [SerializeField] private Transform circleOrigin;
    [SerializeField] private float radius;
    [SerializeField] private bool areaAttack;

    [Header("Audio Config")]
    [SerializeField] private AudioClip swordSwing;
    [SerializeField] private AudioClip swordImpact;
    [SerializeField] private AudioClip swordLethal;

    //              ----|Variables|----
    private bool attackCoolDown;
    private bool hasTarget;
    public bool isAttacking { get; private set; }

    //              ----|References|----
    private AudioSource _AudioSource;

    //              ----|Functions|----
    private void OnEnable() {
        _AudioSource = GetComponentInParent<AudioSource>();
    }

    private void Update() {

        if (isAttacking) return;

        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        //Invierte la escala del arma dependiendo en que direccion se la este apundando
        Vector2 scale = transform.localScale;
        if (direction.x < 0) {
            scale.y = -1;
        }else if (direction.x > 0) {
            scale.y = 1;
        }
        transform.localScale = scale;

        //Varia el SortingOrder del arma dependiendo del angulo actual
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        } else {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

    public void Attack() {      //Triggerea la accion de ataque si el cooldown lo permite
        if( attackCoolDown == true ) {
            return;
        } else {
            _weaponAnimator.SetTrigger("Attack");
            isAttacking = true;
            attackCoolDown = true;
            StartCoroutine(AttackCoolDown());
        }
    }

    //Gestiona el tiempo de enfriamiento para el ataque
    private IEnumerator AttackCoolDown() {
        yield return new WaitForSeconds(coolDown);
        attackCoolDown = false;
        hasTarget = false;
    }

    public void ResetIsAttacking() {
        isAttacking = false;
    }

    //Dibuja el area de ataque
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders() {
        bool impact = false;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius)) {
            //Debug.Log(collider.name);
            Health health;
            if (health = collider.GetComponent<Health>()) {
                impact = true;
                //Ataque con daño en area
                if(areaAttack == true) {    
                    health.GetHit(1, transform.parent.gameObject);
                    PlayAudioImpact(health);

                //Ataque con daño individual
                }else if(areaAttack == false && hasTarget == false) {   //Evita que se impacten a varios enemigos a la vez si no tiene daño en area
                    health.GetHit(1, transform.parent.gameObject);
                    hasTarget = true;
                    PlayAudioImpact(health);
                }
            }
        }
        //Si al terminar de verificar las colisiones no se impacto a ningun objetivo reproduce el sonido correspondiente
        if (impact == false) {
            _AudioSource.PlayOneShot(swordSwing);    //Sin Impacto
        }
    }

    //Ejecuta los sonidos del arma
    private void PlayAudioImpact(Health _health) {

        Health health = _health;
        //Reproduce un sonido dependiendo de si se impacto a un enemigo o no
        if (health.transform.gameObject.layer != gameObject.layer) {
            if (health.GetHP() > 0) {
                _AudioSource.PlayOneShot(swordImpact);   //Impacto
            } else {
                _AudioSource.PlayOneShot(swordLethal);   //Letal
            }
        }
    }
}
