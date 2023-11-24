using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float velocity = 6.0f;
    [SerializeField] private float despawnTime = 0.5f;

    //              ----|Variables|----
    private Vector2 direction;
    private GameObject caster;

    //              ----|References|----
    private Rigidbody2D RB;

    //              ----|Functions|----
    private void OnEnable() {
        RB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        RB.MovePosition(RB.position + direction * (velocity * Time.fixedDeltaTime));
    }

    //Permite configurar el projectil luego de ser instanciado
    public void SetProjectile(Vector2 dir, GameObject _caster) {
        direction = dir;
        caster = _caster;
    }

    public void SetVelocity(float value) {
        velocity = value;
    }

    //Si el projectil impacta con cualquier objeto que no sea su propio caster es destruido e inflinge daño si es posible
    private void OnTriggerEnter2D(Collider2D collider) {
        if(caster == null) {
            Destroy(this.gameObject);
        } else if (collider.gameObject.layer != caster.layer && collider.gameObject.layer != gameObject.layer) {
            if (collider.tag == "Shield") {
                StartCoroutine(DestroyProjectile());
                caster.GetComponent<AgentProjectile>().PlayAudioImpact();
                return; }    //Si se impacta con un escudo
            //Si el objetivo impactado tiene vida le hace daño
            Health health;
            if (health = collider.gameObject.GetComponent<Health>()) {
                health.GetHit(1, caster);
                caster.GetComponent<AgentProjectile>().PlayAudioImpact(health);   //Reproduce el sonido
                StartCoroutine(DestroyProjectile());
            }
            caster.GetComponent<AgentProjectile>().PlayAudioImpact();
            StartCoroutine(DestroyProjectile());
        }
    }

    //Genera un pequeño delay para la destruccion del objeto para dar tiempo a las particulas a finalizar el runtime
    private IEnumerator DestroyProjectile() {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
        yield return null;
    }

}
