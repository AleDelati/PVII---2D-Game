using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealSource : MonoBehaviour
{

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] float RestoredHP = 1f;
    public float coolDown = 1.0f;

    [Header("Audio Config")]
    [SerializeField] private AudioClip anvilRepair;

    //              ----|Variables|----
    bool healCoolDown;

    //              ----|References|----
    private AudioSource _AudioSource;

    //              ----|Functions|----

    private void OnEnable() {
        _AudioSource = GetComponent<AudioSource>();
    }

    //Si el Objeto colisiona con el jugador le restaura al mismo los puntos de vidas configurados
    private void OnCollisionEnter2D(Collision2D collision) {

        if(healCoolDown == true) {
            return;
        } else {
            if (collision.gameObject.CompareTag("Player")) {

                Agent player = collision.gameObject.GetComponent<Agent>();    //Referencia al player
                Health playerHP = player.GetComponent<Health>();                //Referencia al componente de HP del player

                if (playerHP.GetHP() < playerHP.GetMaxHP())    //No aumenta la salud del jugador si ya esta al maximo
                {
                    playerHP.SetHP(playerHP.GetHP() + RestoredHP);
                    //Debug.Log("Vida restaurada al jugador " + RestoredHP);

                    PlayAudio();
                    healCoolDown = true;
                    StartCoroutine(HealCoolDown());
                }
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
        if (_AudioSource.isPlaying) {
            return;
        } else {
            _AudioSource.PlayOneShot(anvilRepair);
        }
    }

}
