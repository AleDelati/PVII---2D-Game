using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    public Sprite[] playerSprites;  //Permite setear el array de sprites del jugador desde el menu de Unity
    [SerializeReference] private Sprite _PlayerSprite;   //Referencia al sprite actual del jugador para usar en la animaciones

    //              ----|Variables|----

    //              ----|References|----
    private SpriteRenderer _SpriteRenderer;
    private Health _Health;
    private PlayerInput _PlayerInput;

    //              ----|Functions|----
    private void Update() {
        PlayerSpriteUpdate();
    }

    private void OnEnable() {           //Se ejecuta cuando el objeto se activa en el nivel

        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _Health = GetComponent<Health>();
        _PlayerInput = GetComponent<PlayerInput>();

    }

    private void PlayerSpriteUpdate() { //Actualiza el Sprite del jugador dependiendo de la cantidad de hp restante
        _SpriteRenderer.sprite = playerSprites[(int)_Health.GetHP() - 1];
        _PlayerSprite = _SpriteRenderer.sprite;     //Mantiene actualizado cual es el sprite actual del jugador
    }

}
