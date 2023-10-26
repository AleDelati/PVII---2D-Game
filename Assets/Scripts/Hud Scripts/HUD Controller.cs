using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

    [SerializeField] GameObject healthIcon;
    [SerializeField] GameObject playerHealthContainter;

    public void UpdateHPHud(float HP) {
        Debug.Log("Hud HP Update");
        if (HealthContainerQuantity() == 0) {
            LoadHealthContainer(HP);
            return;
        }

        if(HealthContainerQuantity() > HP) {
            RemoveHealthIcon();
        } else if (HealthContainerQuantity() < HP) {
            AddHealthIcon();
        } else {
            return;
        }

    }

    private int HealthContainerQuantity() {
        return playerHealthContainter.transform.childCount;
    }



    private void LoadHealthContainer(float Quantity) {
        for(int i = 0; i < Quantity; i++) {
            AddHealthIcon();
        }
    }

    private void AddHealthIcon() {
        Instantiate(healthIcon, playerHealthContainter.transform);
    }

    private void RemoveHealthIcon() {
        Transform container = playerHealthContainter.transform;
        GameObject.Destroy(container.GetChild(container.childCount - 1).gameObject);
    }

}
