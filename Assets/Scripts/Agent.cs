using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]

    [Header("Movement Config")]
    [SerializeField] float velocity = 5f;

    [SerializeReference] private Sprite _AgentSprite;   //Referencia al sprite actual del agente para usar en la animaciones



    //              ----|Variables|----
    private Vector2 movementInput, pointerInput;

    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }
    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }




    //              ----|References|----
    private SpriteRenderer _SpriteRenderer;
    private Rigidbody2D _Rigidbody2D;
    private Animator _Animator;
    private WeaponParent _WeaponParent;
    private Health _Health;



    //              ----|Functions|----
    private void OnEnable() {           //Se ejecuta cuando el objeto se activa en el nivel

        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _Health = GetComponent<Health>();

        _WeaponParent = GetComponentInChildren<WeaponParent>();

        _AgentSprite = _SpriteRenderer.sprite;     //Guarda el sprite actual del agente
    }

    private void Update() {              //Se ejecuta en cada frame del juego (Intervalo variable) 

        _WeaponParent.PointerPosition = PointerInput;        //Envia la posicion del mouse al WeaponParent vinculado

        MovementAnimTrigger();

    }

    private void FixedUpdate() {

        MoveAgent();

    }

    private void MoveAgent() {         //Ejecuta las acciones relacionadas al movimiento del Agente
       
        _Rigidbody2D.MovePosition(_Rigidbody2D.position + MovementInput * (velocity * Time.fixedDeltaTime));
    }

    private void MovementAnimTrigger() {         //Si se recibe input de movimiento acciona los triggers de las animaciones del agente
      
        if(PointerInput.x >= transform.position.x) {
            _SpriteRenderer.flipX = false;
        } else {
            _SpriteRenderer.flipX = true;
        }

        //Retorna el estado de movimiento al componente animator
        if (MovementInput.x != 0 || MovementInput.y != 0) {
            _Animator.SetBool("EnMovimiento", true);
        } else {
            _Animator.SetBool("EnMovimiento", false);
        }
    }

    public void PerformAttack() {
        _WeaponParent.Attack();
    }

}
