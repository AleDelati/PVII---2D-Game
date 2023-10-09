using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    
    public Dictionary<Item, int> items = new Dictionary<Item, int>();

    public void AddItem(Item item, int quantity) {
        if (items.ContainsKey(item)) {
            items[item] += quantity;
        } else {
            items.Add(item, quantity);
        }
    }

}
