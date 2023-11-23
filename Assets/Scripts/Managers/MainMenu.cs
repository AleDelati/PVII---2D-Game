using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour {

    public UnityEvent<string> OnLoadBestScore, OnLoadBestScoreDeathCount, OnLoadScore, OnLoadDeaths;
    public UnityEvent<bool> OnLoadEasyModeState;

    private void Start() {
        LoadScore();
        LoadDeaths();
        LoadHighScore();
        LoadHighScoreDeaths();
        LoadEasyModeState();
        ResetCheckPoints();
    }

    public void LoadScore() {
        int score = PersistenceManager.Instance.GetInt("Score");
        OnLoadScore?.Invoke(score.ToString());
    }

    public void LoadDeaths() {
        int deaths = GameManager.instance.GetDeathCount();
        OnLoadDeaths?.Invoke(deaths.ToString());
    }

    public void LoadHighScore() {
        int highScore = PersistenceManager.Instance.GetInt("HighScore");
        OnLoadBestScore?.Invoke(highScore.ToString());
    }

    public void LoadHighScoreDeaths() {
        int highScoreDeaths = PersistenceManager.Instance.GetInt("DeathCount");
        OnLoadBestScoreDeathCount?.Invoke(highScoreDeaths.ToString());
    }

    public void LoadEasyModeState() {
        bool state = PersistenceManager.Instance.GetBool("EasyMode");
        OnLoadEasyModeState?.Invoke(state);
    }

    public void ResetCheckPoints() {
        PersistenceManager.Instance.SetBool("Level 1 CheckPoint", false);
        PersistenceManager.Instance.SetBool("Level 2 CheckPoint", false);
        PersistenceManager.Instance.SetBool("Level 3 CheckPoint", false);
    }

    public void ResetMusic() {
        PersistenceManager.Instance.SaveMusicConfig(true);
    }

}
