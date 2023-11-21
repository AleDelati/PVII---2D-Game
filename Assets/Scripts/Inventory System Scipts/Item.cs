using UnityEngine;

public class Item : MonoBehaviour {

    //              -|Variables|-
    [SerializeField] AudioClip itemSound;

    //              -|References|-

    //              -|Functions-

    public AudioClip GetItemSound() {
        return itemSound;
    }

    public void DestroyItem () {
        Destroy(gameObject);
    }

}
