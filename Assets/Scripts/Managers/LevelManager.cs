using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private AudioSource BackgroundMusic;

    private void OnEnable() {
        BackgroundMusic = GetComponent<AudioSource>();

        GameEvents.onPause += Pause;
        GameEvents.onResume += Resume;
    }

    private void OnDisable() {
        GameEvents.onPause -= Pause;
        GameEvents.onResume -= Resume;
    }

    private void Pause() => BackgroundMusic.Pause();
    private void Resume() => BackgroundMusic.UnPause();

}
