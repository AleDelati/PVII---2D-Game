using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour {

    public UnityEvent<string> OnLoadBestScore, OnLoadBestScoreDeathCount, OnLoadScore, OnLoadDeaths;

    private void Start() {
        LoadScore();
        LoadDeaths();
        LoadHighScore();
        LoadHighScoreDeaths();
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

}
