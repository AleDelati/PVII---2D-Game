using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {



    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float HP = 3f;
    public Sprite[] playerSprites;  //Permite setear el array de sprites del jugador desde el menu de Unity
    [SerializeReference] private Sprite _PlayerSprite;   //Referencia al sprite actual del jugador para usar en la animaciones
    [SerializeField] private InputActionReference pointerPositionInput;

    [Header("Movement Config")]
    [SerializeField] float velocity = 5f;
    [SerializeField] private InputActionReference movementInput;

    [Header("Combat Config")]
    [SerializeField] private InputActionReference attackInput;



    //              ----|Variables|----
    Vector2 movement;
    Vector2 pointerPosition;


    //              ----|References|----
    private SpriteRenderer _SpriteRenderer;
    private Rigidbody2D _Rigidbody2D;
    private Animator _Animator;
    private WeaponParent _WeaponParent;



    //              ----|Functions|----
    private void OnEnable() {           //Se ejecuta cuando el objeto se activa en el nivel
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();

        _WeaponParent = GetComponentInChildren<WeaponParent>();

        attackInput.action.performed += PerformAttack;
    }

    private void OnDisable() {
        attackInput.action.performed -= PerformAttack;
    }

    private void Update() {              //Se ejecuta en cada frame del juego (Intervalo variable)

        movement = movementInput.action.ReadValue<Vector2>();   //Guarda en movement los Inputs de movimiento recibidos
        pointerPosition = GetPointerInput();                    //Mantiene actualizada la variable que contiene la posicion del mouse
        _WeaponParent.PointerPosition = pointerPosition;        //Envia la posicion del mouse al WeaponParent vinculado

        MovementAnimTrigger();
        PlayerSpriteUpdate();

    }

    private void FixedUpdate() {

        MovePlayer();

    }

    public void SetHP(float value) {    //Permite modificar los puntos de vida del jugador
        HP += value;
    }

    public float GetHP() {              //Retorna los puntos de vida actual del jugador
        return HP;
    }

    private Vector2 GetPointerInput() {
        Vector3 mousePos = pointerPositionInput.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void MovePlayer() {         //Ejecuta las acciones relacionadas al movimiento del jugador
       
        _Rigidbody2D.MovePosition(_Rigidbody2D.position + movement * (velocity * Time.fixedDeltaTime));
    }

    private void PlayerSpriteUpdate() { //Actualiza el Sprite del jugador dependiendo de la cantidad de hp restante
        _SpriteRenderer.sprite = playerSprites[(int)HP - 1];
        _PlayerSprite = _SpriteRenderer.sprite;     //Mantiene actualizado cual es el sprite actual del jugador
    }

    private void MovementAnimTrigger() {         //Si se recibe input de movimiento acciona los triggers de las animaciones del jugador
      
        if(pointerPosition.x >= transform.position.x) {
            _SpriteRenderer.flipX = false;
        } else {
            _SpriteRenderer.flipX = true;
        }

        //Retorna el estado de movimiento al componente animator
        if (movement.x != 0 || movement.y != 0) {
            _Animator.SetBool("EnMovimiento", true);
        } else {
            _Animator.SetBool("EnMovimiento", false);
        }
    }

    private void PerformAttack(InputAction.CallbackContext obj) {
        _WeaponParent.Attack();
    }

}
