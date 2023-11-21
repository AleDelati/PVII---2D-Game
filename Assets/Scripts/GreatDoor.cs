using UnityEngine;
using UnityEngine.Events;

public class GreatDoor : MonoBehaviour {

    [Header("General Config")]
    [SerializeField] bool doorTriggered = false;

    [SerializeField] bool TargetTriggered = false;
    [SerializeField] GameObject target;

    [SerializeField] bool invertTrigger = false;

    [SerializeField] bool keyTriggered = true;

    [SerializeField] private AudioClip doorSound;
    
    private bool lastDoorState = false;
    private bool disableDoor = false;
    private bool keyDelivered = false;
    private bool startTriggered = false;

    private BoxCollider2D doorRB;
    private GameObject playerKeys;
    private AudioSource AS;

    [SerializeField]
    private UnityEvent OnDoorTriggered;

    private void Start() {
        doorRB = GetComponent<BoxCollider2D>();
        playerKeys = GameObject.Find("Inventory/Keys");
        AS = GetComponent<AudioSource>();

        startTriggered = doorTriggered;
    }

    private void Update() {
        DetectPlayerTrigger();
        DetectPlayerKeyDeliver();
        if(disableDoor == false) {
            CheckTrigger();
        }
    }
    
    private void OnDrawGizmos() {

        //Dibuja el Trigger de la puerta
        Gizmos.color = Color.yellow;
        if (invertTrigger) {
            Gizmos.DrawCube(transform.position + new Vector3(0, -2, 0), new Vector3(3.3f, 1f, 1f));
        } else {
            Gizmos.DrawCube(transform.position + new Vector3(0, 1, 0), new Vector3(3.3f, 1f, 1f));
        }

        //Dibuja el area de colision para la entrega de la llave
        Gizmos.color = Color.cyan;
        if (doorRB != null) {
            Gizmos.DrawCube(transform.position + new Vector3(doorRB.offset.x, doorRB.offset.y, 0), doorRB.size + new Vector2(0, 0.2f));
        }
    }

    private void DetectPlayerTrigger() {
        if (invertTrigger) {
            foreach (Collider2D collider in Physics2D.OverlapBoxAll(transform.position + new Vector3(0, -2, 0), new Vector3(3.3f, 1, 1), 0)) {
                if (collider.CompareTag("Player")) {
                    doorTriggered = true;
                }
            }
        } else {
            foreach (Collider2D collider in Physics2D.OverlapBoxAll(transform.position + new Vector3(0, 1, 0), new Vector3(3.3f, 1, 1), 0)) {
                if (collider.CompareTag("Player")) {
                    doorTriggered = true;
                }
            }
        }
    }

    private void DetectPlayerKeyDeliver() {
        foreach (Collider2D collider in Physics2D.OverlapBoxAll(transform.position + new Vector3(doorRB.offset.x, doorRB.offset.y, 0), doorRB.size + new Vector2(0, 0.2f), 0)) {
            if (collider.CompareTag("Player")) {

                //Busca la llave en el inventario del jugador
                
                if (playerKeys != null && playerKeys.transform.childCount != 0 && keyTriggered == true) {
                    playerKeys.transform.GetChild(0).GetComponent<Item>().DestroyItem();
                    keyDelivered = true;
                }
            }
        }
    }

    private void CheckTrigger() {
        if(doorTriggered != lastDoorState) {
            lastDoorState = doorTriggered;
            if (!startTriggered) { AS.PlayOneShot(doorSound); }
            UpdateDoorStatus();
        }

        //Desactiva la puerta si el target murio
        if(TargetTriggered && target == null) {
            doorTriggered = false;
            disableDoor = true;
            AS.PlayOneShot(doorSound);
            UpdateDoorStatus();
        }

        //Desactiva la puerta si se entrego la llave
        if(keyTriggered && keyDelivered) {
            doorTriggered = false;
            disableDoor = true;
            AS.PlayOneShot(doorSound);
            UpdateDoorStatus();
        }

    }

    private void UpdateDoorStatus() {
        if (doorTriggered) {
            doorRB.enabled = true;
            OnDoorTriggered?.Invoke();
        } else {
            doorRB.enabled = false;
            OnDoorTriggered?.Invoke();
        }
    }



}
