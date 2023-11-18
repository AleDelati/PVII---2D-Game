using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {

    [SerializeField] private Toggle musicToggle;

    private void Start() {
        musicToggle.isOn = PersistenceManager.Instance.GetBool("Music");
    }
}
