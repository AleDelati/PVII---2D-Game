using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    public Sprite[] doorSprites;    //Permite setear el array de sprites de la puerta desde el menu de Unity

    //              ----|Variables|----

    //              ----|References|----
    [SerializeField] Agent _LevelBossAgent;
    private SpriteRenderer _SpriteRenderer;

    //              ----|Functions|----

    private void OnEnable() {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _SpriteRenderer.sprite = doorSprites[0];
    }

    private void Update() {

        if(_LevelBossAgent == null) {
            OpenDoor();
        }

    }

    private void OpenDoor() {
        if(_SpriteRenderer.sprite == doorSprites[0]) {
            _SpriteRenderer.sprite = doorSprites[1];
            Debug.Log("Victoria!, La puerta de salida esta abierta");
        }
    }

}
