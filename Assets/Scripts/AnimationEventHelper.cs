using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeReference] private Sprite _WeaponSprite;   //Referencia al sprite actual del Arma para usar en la animaciones

    //              ----|Variables|----
    public UnityEvent OnAnimationEventTriggered, OnAttackPerformed;

    //              ----|References|----
    private SpriteRenderer _SpriteRenderer;

    //              ----|Functions|----
    private void OnEnable() {
        _SpriteRenderer = GetComponent<SpriteRenderer>();

        _WeaponSprite = _SpriteRenderer.sprite;     //Guarda el sprite actual del arma
    }

    public void TriggerEvent() {
        OnAnimationEventTriggered?.Invoke();
    }

    public void TriggerAttack() {
        OnAttackPerformed?.Invoke();
    }

}
