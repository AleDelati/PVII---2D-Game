using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour {

    public UnityEvent<string> OnLoadBestScore, OnLoadBestScoreDeathCount;

    public void LoadHighScore() {
        int highScore = PersistenceManager.Instance.GetInt("HighScore");
        OnLoadBestScore?.Invoke(highScore.ToString());
    }

    public void LoadHighScoreDeaths() {
        int highScoreDeaths = PersistenceManager.Instance.GetInt("DeathCount");
        OnLoadBestScoreDeathCount?.Invoke(highScoreDeaths.ToString());
    }

}
