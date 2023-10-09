using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour {

    //              ----|Unity Config|----
    [SerializeField] private List<GameObject> collectables;
    [SerializeField] private GameObject bag;

    //              ----|Variables|----

    //              ----|References|----
    private Player _Player;
    private PlayerProgression _PlayerProgression;

    //              ----|Functions|----
    private void Awake() {
        collectables = new List<GameObject>();
        _Player = GetComponent<Player>();
        _PlayerProgression = GetComponent<PlayerProgression>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Collectable")) { return; }

        GameObject newCollectable = collision.gameObject;
        newCollectable.SetActive(false);

        collectables.Add(newCollectable);
        newCollectable.transform.SetParent(bag.transform);

        _PlayerProgression.GiveXP(_Player.PlayerProfile.XPKeyFragment); //Da XP al jugador al recolectar

        Debug.Log("Fragmentos de llaves obtenidos: " + collectables.Count);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            Debug.Log("Fragmentos de llaves obtenidos: " + collectables.Count);
            if(collectables.Count == 0) {
                Debug.Log("Necesitas 3 fragmentos mas para poder abrir la puerta de salida");
            } else if (collectables.Count == 1) {
                Debug.Log("Necesitas 2 fragmentos mas para poder abrir la puerta de salida");
            } else if (collectables.Count == 2) {
                Debug.Log("Necesitas 1 fragmentos mas para poder abrir la puerta de salida");
            } else {
                Debug.Log("Ya tienes todos los fragmentos necesarios, Puedes abrir la puerta de salida!");
            }
        }
    }

    public int GetCollectablesCount() {
        return collectables.Count;
    }

}
