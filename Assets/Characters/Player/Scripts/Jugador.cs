using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float vida = 3f;

    [SerializeReference] private Sprite PlayerSprite;   //Referencia al sprite actual del jugador para usar en la animaciones
    private SpriteRenderer miSpriteRenderer;    // Variable para referenciar otro componente del objeto

    public Sprite[] playerSprites;

    //                 ----Funciones----
    public void ModificarVida(float puntos)     //Modifica los puntos de vida del jugador segun el valor ingresado
    {
        vida += puntos;
        Debug.Log(EstasVivo());
    }

    private bool EstasVivo() { return vida > 0; }   //Retorna si el jugador sigue con vida

    public float VidaActual() { return vida; }   //Retorna los puntos de vida actuales del jugador

    //Actualiza el Sprite del jugador dependiendo de la cantidad de hp restante
    private void PlayerSpriteUpdate()
    {
        miSpriteRenderer.sprite = playerSprites[(int)vida-1];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Salida")) { return; }

        Debug.Log("Victoria!");
    }

    // Codigo ejecutado cuando el objeto se activa en el nivel
    private void OnEnable()
    {
        miSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Codigo ejecutado en cada frame del juego (Intervalo variable)
    private void Update()
    {
        PlayerSpriteUpdate();
        PlayerSprite = miSpriteRenderer.sprite;     //Mantiene actualizado cual es el sprite actual del jugador
    }

}
