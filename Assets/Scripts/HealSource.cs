using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSource : MonoBehaviour
{

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] float RestoredHP = 1f;


    //              ----|Functions|----

    //Si el Objeto colisiona con el jugador le restaura al mismo los puntos de vidas configurados
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Agent player = collision.gameObject.GetComponent<Agent>();    //Referencia al player
            Health playerHP = player.GetComponent<Health>();                //Referencia al componente de HP del player

            if (playerHP.GetHP() < playerHP.GetMaxHP())    //No aumenta la salud del jugador si ya esta al maximo
            {
                playerHP.SetHP(playerHP.GetHP() + RestoredHP);
                Debug.Log("Vida restaurada al jugador " + RestoredHP);
            }
        }
    }
}
