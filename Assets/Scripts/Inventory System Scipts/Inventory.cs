using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour {

    //              ----|Unity Config|----
    [SerializeField] private List<GameObject> items;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject keyInventory;


    //              ----|Variables|----

    //              ----|References|----
    private AudioSource AS;

    //              ----|Events|----
    public UnityEvent OnItemPickUp;

    //              ----|Functions|----
    private void Awake() {
        items = new List<GameObject>();
        AS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (!collider.gameObject.CompareTag("Item")) { return; }

        GameObject newItem = collider.gameObject;
        items.Add(newItem);

        if(collider.transform.name == "Key") {
            AS.PlayOneShot(collider.GetComponent<Item>().GetItemSound());
            newItem.transform.SetParent(keyInventory.transform);
        } else {
            newItem.transform.SetParent(inventory.transform);
        }

        newItem.SetActive(false);
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
