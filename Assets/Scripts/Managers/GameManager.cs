using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance {get; private set;}
    private int score;
    private int deathCount;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    //              -|Pausa|-

    private void OnEnable() {
        GameEvents.onPause += Pause;
        GameEvents.onResume += Resume;
    }

    private void OnDisable() {
        GameEvents.onPause -= Pause;
        GameEvents.onResume -= Resume;
    }

    private void Pause() => Time.timeScale = 0;
    private void Resume() => Time.timeScale = 1;

    private void Update() {
        //Pausa el Juego al presionar ESC
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Time.timeScale != 0) {
                GameEvents.TriggerPause();
            } else {
                GameEvents.TriggerResume();
            }
        }
    }

    //              -|Score|-
    public void AddScore(int points) {
        score += points;
        //Debug.Log("Score Game Manager: " + score);
    }

    public void SubtractScore(int points) {
        score -= points;
    }

    public void ResetScore() {
        score = 0;
    }

    public int GetScore() {
        return score;
    }

    //              -|deathCount|-
    public void AddDeathCount() {
        deathCount++;
        //Debug.Log("Death Count GameManager: " + deathCount);
    }

    public void ResetDeathCount() {
        deathCount = 0;
    }

    public int GetDeathCount() {
        return deathCount;
    }

}