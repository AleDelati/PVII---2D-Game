using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float velocidad = 5f;

    // Variables de uso interno en el script
    private float moverHorizontal;
    private float moverVertical;
    private Vector2 direccion;

    // Variable para referenciar otro componente del objeto
    private Rigidbody2D miRigidbody2D;
    private Animator miAnimator;

    // Codigo ejecutado cuando el objeto se activa en el nivel
    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAnimator = GetComponent<Animator>();
    }

    // Codigo ejecutado en cada frame del juego (Intervalo variable)
    private void Update()
    {
        moverHorizontal = Input.GetAxis("Horizontal");
        moverVertical = Input.GetAxis("Vertical");
        direccion = new Vector2(moverHorizontal, moverVertical);

        //Retorna el estado de movimiento al componente animator
        if (moverHorizontal != 0 || moverVertical != 0)
        {
            miAnimator.SetBool("EnMovimiento", true);
        }
        else
        {
            miAnimator.SetBool("EnMovimiento", false);
        }

    }
    private void FixedUpdate()
    {
        miRigidbody2D.MovePosition(miRigidbody2D.position + direccion * (velocidad * Time.fixedDeltaTime));
    }
}