using UnityEngine;

public class AgentProjectile : MonoBehaviour {

    //              -|References|-
    [Header("Projectile Audio Config")]
    [SerializeField] private AudioClip projectileSwing;
    [SerializeField] private AudioClip projectileImpact;
    [SerializeField] private AudioClip projectileLethal;
    [SerializeField] private AudioClip projectileSpawn;
    private AudioSource AS;

    //              -|Functions|-

    private void Awake() {
        AS = GetComponent<AudioSource>();
    }

    //Ejecuta los sonidos de impacto del Projectil
    public void PlayAudioImpact(Health _health) {

        Health health = _health;
        //Reproduce un sonido dependiendo de si se impacto a un enemigo o no
        if (health.transform.gameObject.layer != gameObject.layer) {
            if (health.GetHP() > 0) {
                AS.PlayOneShot(projectileImpact);   //Impacto
            } else {
                AS.PlayOneShot(projectileLethal);   //Letal
            }
        }
    }
    public void PlayAudioImpact() {
        AS.PlayOneShot(projectileSwing);
    }

    //Ejecuta un sonido al instanciar el Projectil
    public void PlayOnSpawnAudio() {
        AS.PlayOneShot(projectileSpawn);
    }

}
