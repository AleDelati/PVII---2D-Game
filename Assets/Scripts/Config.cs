using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configs : MonoBehaviour {

    public int targetFPS = 60;

    private void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }
}
