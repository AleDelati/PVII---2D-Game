using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour {

    //              ----|Unity Config|----
    [SerializeField] private List<GameObject> items;
    [SerializeField] private GameObject inventory;

    //              ----|Variables|----

    //              ----|References|----

    //              ----|Events|----
    public UnityEvent OnItemPickUp;

    //              ----|Functions|----
    private void Awake() {
        items = new List<GameObject>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Item")) { return; }

        GameObject newItem = collision.gameObject;
        newItem.SetActive(false);

        items.Add(newItem);
        newItem.transform.SetParent(inventory.transform);

        OnItemPickUp?.Invoke();  //Triggerea un evento al recolectar un Collectable Object

    }

    public int GetItemsCount() {
        return items.Count;
    }

    //Reinicia la lista ( Usado en GameOver )
    public void ResetItems() {
        items.Clear();
    }

}
