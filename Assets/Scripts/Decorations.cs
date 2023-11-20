using UnityEngine;

public class Decorations : MonoBehaviour {

    [Header("Decoration Config")]
    [SerializeField] private bool destroyOnContact = false;

    //              -|Functions|-
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && destroyOnContact) {
            Destroy(this.transform.gameObject);
        }
    }

}
