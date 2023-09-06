using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour {
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = Auxiliares.GetMouseWorldPosition();
            Vector3 mouseDir = (mousePosition - transform.position).normalized;

            float attackOffset = 1.5f;
            
            Vector3 attackPosition = transform.position + mouseDir * attackOffset;
            //Debug.Log("PlayerAtack " +attackPosition);
        }
    }
}
