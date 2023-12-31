using UnityEngine;
using UnityEngine.Events;

public class ExitDoor : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private Sprite[] doorSprites;    //Permite setear el array de sprites de la puerta desde el menu de Unity

    //              ----|Variables|----
    public bool doorOpen = false;

    //              ----|References|----
    [SerializeField] private GameObject target;

    private SpriteRenderer _SpriteRenderer;

    //              ----|Events|----
    [SerializeField] private UnityEvent OnDoorPlayerCollision;

    //              ----|Functions|----
    private void OnEnable() {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _SpriteRenderer.sprite = doorSprites[0];
    }

    private void Update() {
        if(target == null) {
            OpenDoor();
        }
    }

    private void LevelCleared() => GameEvents.TriggerLevelCleared();

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (doorOpen == true) {
                OnDoorPlayerCollision.Invoke();
            }
        }
    }

    private void OpenDoor() {
        if(_SpriteRenderer.sprite == doorSprites[0]) {
            _SpriteRenderer.sprite = doorSprites[1];
            LevelCleared();
            doorOpen = true;
            Debug.Log("Victoria!, La puerta de salida esta abierta");
        }
    }

    public bool GetDoorState() {
        return doorOpen;
    }
}
