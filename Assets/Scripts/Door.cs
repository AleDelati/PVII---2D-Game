using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField] Sprite[] doorSprites;
    [SerializeField] bool doorOpen = true;

    private SpriteRenderer SR;

    private void OnEnable() {
        SR = GetComponent<SpriteRenderer>();
    }

    public void CycleDoorSprites() {
        doorOpen = !doorOpen;
        if (doorOpen) {
            SR.sprite = doorSprites[0];
            SR.sortingOrder = -1;
        } else {
            SR.sprite = doorSprites[1];
            SR.sortingOrder = 2;
        }
    }

}
