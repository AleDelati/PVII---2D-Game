using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private InputActionReference pointerPositionInput;
    [SerializeField] private InputActionReference movementInput;
    [SerializeField] private InputActionReference attackInput;

    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;

    //              ----|Variables|----

    //              ----|References|----
    private WeaponParent weaponParent;

    //              ----|Functions|----
    private void OnEnable() {
        attackInput.action.performed += PerformAttack;
    }

    private void OnDisable() {
        attackInput.action.performed -= PerformAttack;
    }

    private void Update() {
        weaponParent = GetComponentInChildren<WeaponParent>();
        OnMovementInput?.Invoke(movementInput.action.ReadValue<Vector2>()); //Guarda en movement los Inputs de movimiento recibidos
        OnPointerInput?.Invoke(GetPointerInput());  //Mantiene actualizada la variable que contiene la posicion del mouse
    }

    private Vector2 GetPointerInput() {
        Vector3 mousePos = pointerPositionInput.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void PerformAttack(InputAction.CallbackContext obj) {
        if(weaponParent != null){ OnAttack?.Invoke(); } 
    }

}
