using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponParent : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    public SpriteRenderer characterRenderer;
    public SpriteRenderer weaponRenderer;
    public Vector2 PointerPosition { get; set; }
    public Animator _weaponAnimator;
    public float coolDown = 0.3f;

    public Transform circleOrigin;
    public float radius;

    [Header("Audio Config")]
    [SerializeField] private AudioClip swordSwing;
    [SerializeField] private AudioClip swordImpact;
    [SerializeField] private AudioClip swordLethal;

    //              ----|Variables|----
    private bool attackCoolDown;
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
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius)) {
            //Debug.Log(collider.name);
            Health health;
            if(health = collider.GetComponent<Health>()) {
                health.GetHit(1, transform.parent.gameObject);
            }

            //Reproduce un sonido dependiendo de si se impacto a un enemigo o no
            if(health.transform.gameObject.layer != gameObject.layer) {
                if(health.GetHP() > 0) {
                    PlayAudio(1);   //Impacto
                } else {
                    PlayAudio(2);   //Letal
                }
            } else {
                PlayAudio(0);   //Sin Impacto
            }

        }
    }

    //Ejecuta los sonidos del arma
    private void PlayAudio(int typeofimpact) {
        if (_AudioSource.isPlaying) {
            return;
        } else {
            switch (typeofimpact) {
                case 0:     //Sin impacto
                    _AudioSource.PlayOneShot(swordSwing);
                    break;
                case 1:     //Impacto
                    _AudioSource.PlayOneShot(swordImpact);
                    break;
                case 2:     //Letal
                    _AudioSource.PlayOneShot(swordLethal);
                    break;
            }
        }
    }
}
