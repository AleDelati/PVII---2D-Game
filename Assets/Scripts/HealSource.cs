using System.Collections;
using UnityEngine;

public class HealSource : MonoBehaviour
{

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] float RestoredHP = 1f;
    [SerializeField] int scorePenalty = 50;
    [SerializeField] float coolDown = 1.0f;
    [SerializeField] float interactionArea = 1.0f;

    [Header("Audio Config")]
    [SerializeField] private AudioClip anvilRepair;

    //              ----|Variables|----
    bool healCoolDown;

    //              ----|References|----
    private GameObject player;
    private AudioSource AS;
    private ParticleSystem PS;

    //              ----|Functions|----

    private void OnEnable() {
        player = GameObject.Find("Player");
        AS = GetComponent<AudioSource>();
        PS = GetComponent<ParticleSystem>();
    }

    private void Update() {
        CheckPlayerInteraction();
    }

    //Dibuja el Area de interaccion del Anvil
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionArea);
    }

    //Si el jugador esta dentro del area de interaccion e interactua con el Anvil restaura los puntos de vida configurados
    private void CheckPlayerInteraction() {
        if(CheckInteractionArea() && !healCoolDown && Input.GetKeyDown(KeyCode.E)) {
            Health playerHP = player.GetComponent<Health>();    //Referencia al componente de Health del player

            if (playerHP.GetHP() < playerHP.GetMaxHP())    //No aumenta la salud del jugador si ya esta al maximo
            {
                playerHP.SetHP(playerHP.GetHP() + RestoredHP);
                //Debug.Log("Vida restaurada al jugador " + RestoredHP);

                PlayAudio();
                healCoolDown = true;
                GameManager.instance.SubtractScore(scorePenalty);     //Disminuye la puntuacion al curarse
                StartCoroutine(HealCoolDown());
                PS.Play();
            }
        }
    }

    //Gestiona el tiempo de enfriamiento para la curacion
    private IEnumerator HealCoolDown() {
        yield return new WaitForSeconds(coolDown);
        healCoolDown = false;
    }

    //Ejecuta los sonidos de curacion
    private void PlayAudio() {
        if (AS.isPlaying) {
            return;
        } else {
            AS.PlayOneShot(anvilRepair);
        }
    }

    //Retorna si el jugador esta dentro del area de interaccion
    private bool CheckInteractionArea() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, interactionArea)) {
            if (collider.CompareTag("Player") == true) {
                return true;
            }
        }
        return false;
    }

}
