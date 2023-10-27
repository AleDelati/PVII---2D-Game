using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float velocity = 6.0f;

    //              ----|Variables|----
    private Vector2 direction;
    private GameObject caster;

    //              ----|References|----
    private Rigidbody2D _RigidBody2D;
    private BoxCollider2D _BoxCollider2D;

    //              ----|Functions|----
    private void OnEnable() {
        _RigidBody2D = GetComponent<Rigidbody2D>();
        _BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        _RigidBody2D.MovePosition(_RigidBody2D.position + direction * (velocity * Time.fixedDeltaTime));
    }

    //Permite configurar el projectil luego de ser instanciado
    public void SetProjectile(Vector2 dir, GameObject _caster) {
        direction = dir;
        caster = _caster;
    }

    //Si el projectil impacta con cualquier objeto que no sea su propio caster es destruido e inflinge daño si es posible
    private void OnCollisionEnter2D(Collision2D collision) {
        if(caster == null) {
            Destroy(this.gameObject);
        } else if (collision.gameObject.layer != caster.layer) {
            //Si el objetivo impactado tiene vida le hace daño
            Health health;
            if (health = collision.gameObject.GetComponent<Health>()) {
                health.GetHit(1, caster);
                //PlayAudioImpact(health)
                Destroy(this.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

}
