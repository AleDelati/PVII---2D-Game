using UnityEngine;

public class PersistenceManager : MonoBehaviour {
    
    public static PersistenceManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void SetInt(string key, int value) {
        PlayerPrefs.SetInt(key, value);
    }

    public int GetInt(string key, int defaultValue = 0) {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public void SetFloat(string key, float value) {
        PlayerPrefs.SetFloat(key, value);
    }

    public float GetFloat(string key, float defaultValue = 0.0f) {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public void SetString(string key, string value) {
        PlayerPrefs.SetString(key, value);
    }

    public string GetString(string key, string defaultValue = "") {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public void SetBool(string key, bool state) {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }

    public bool GetBool(string key, bool defaultValue = false) {
        int value = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
        return value == 1;
    }

    public void Save() {
        PlayerPrefs.Save();
    }

    public void DeleteKey(string key) {
        PlayerPrefs.DeleteKey(key);
    }

    public void DeleteAll() {
        PlayerPrefs.DeleteAll();
    }

    //              -||-
    public void SaveMusicConfig(bool status) {
        SetBool("Music", status);
        Save();
    }

    public void SaveScore(int score) {
        SetInt("Score", score);
        Save();
    }

    //Guarda la puntuacion del jugador si es la mas alta registrada
    public void SaveHighScore(int score) {
        if( score > GetInt("HighScore")) {
            SetInt("HighScore", score);
            SetInt("HighScoreDeathCount", GetInt("DeathCount"));
            Save();
            Debug.Log("Se guardo una nueva puntuacion mas alta: " + score);
        } else {
            Debug.Log("No se alcanzo una nueva puntuacion mas alta, la mas alta registrada es: " + GetInt("HighScore"));
        }
    }

    //Guarda la cantidad de muertes del jugador durante la Partida
    public void SaveDeathCount(int count) {
        SetInt("DeathCount", count);
        Save();
    }

    //              -||-
    public void SaveSkipIntroCinematicsConfig(bool status) {
        SetBool("SkipIntroCinematics", status);
        Save();
    }

    public void SaveEasyMode(bool status) {
        SetBool("EasyMode", status);
        Save();
    }

    //Guarda el checkpoint actual de los niveles
    public void SaveCheckPoint(int currentLevel) {
        switch(currentLevel) {
            case 2:
                SetBool("Level 2 CheckPoint", true);
                break;
            case 3:
                SetBool("Level 3 CheckPoint", true);
                break;
        }
    }

}
