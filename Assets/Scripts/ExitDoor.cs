using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private Sprite[] doorSprites;    //Permite setear el array de sprites de la puerta desde el menu de Unity

    //              ----|Variables|----
    public bool doorOpen = false;

    //              ----|References|----
    [SerializeField] private GameObject Player;

    private SpriteRenderer _SpriteRenderer;
    private Collect _PlayerBag;

    //              ----|Functions|----

    private void OnEnable() {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _SpriteRenderer.sprite = doorSprites[0];
        _PlayerBag = Player.GetComponent<Collect>();
    }

    private void Update() {
        if(_PlayerBag.GetCollectablesCount() == 3) {
            OpenDoor();
        }
    }

    private void OpenDoor() {
        if(_SpriteRenderer.sprite == doorSprites[0]) {
            _SpriteRenderer.sprite = doorSprites[1];
            doorOpen = true;
            Debug.Log("Victoria!, La puerta de salida esta abierta");
        }
    }

    public bool GetDoorState() {
        return doorOpen;
    }

}
