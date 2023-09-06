using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSource : MonoBehaviour
{

    //Variables para configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float VidaRestaurada = 1f;

    //Si el Objeto colisiona con el jugador le restaura al mismo los puntos de vidas configurados
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            if(jugador.VidaActual() < 5)    //No aumenta la salud del jugador si ya esta al maximo
            {
                jugador.ModificarVida(VidaRestaurada);
                Debug.Log("Vida restaurada al jugador " + VidaRestaurada);
            }
        }
    }
}
