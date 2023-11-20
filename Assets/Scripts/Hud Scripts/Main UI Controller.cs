using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour {

    public void LoadNextScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadPreviousScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }

    public void LoadMainMenuScene() {
        SceneManager.LoadScene(0);
    }

}
