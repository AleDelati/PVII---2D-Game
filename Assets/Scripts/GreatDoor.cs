using UnityEngine;
using UnityEngine.Events;

public class GreatDoor : MonoBehaviour {

    [Header("General Config")]
    [SerializeField] GameObject target;
    [SerializeField] bool doorTriggered = false;
    
    private bool lastDoorState = false;
    private bool disableDoor = false;
    private BoxCollider2D doorRB;

    [SerializeField]
    private UnityEvent OnDoorTriggered;

    private void Start() {
        doorRB = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        DetectPlayer();
        if(disableDoor == false) {
            CheckTrigger();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + new Vector3(0, 1, 0), new Vector3(3.3f, 1f, 1f));
    }

    private void DetectPlayer() {
        foreach (Collider2D collider in Physics2D.OverlapBoxAll(transform.position + new Vector3(0, 1, 0), new Vector3(3.3f, 1, 1), 0)) {
            if (collider.CompareTag("Player")) {
                doorTriggered = true;
                Debug.Log("Door Triggered");
            }
        }
    }

    private void CheckTrigger() {
        if(doorTriggered != lastDoorState) {
            lastDoorState = doorTriggered;
            UpdateDoorStatus();
        }

        //Desactiva la puerta si el target murio
        if(target == null) {
            doorTriggered = false;
            disableDoor = true;
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
