using UnityEngine;

public class Configs : MonoBehaviour {

    [Header("Intro Config")]
    [SerializeField] private bool skipIntro = true;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera auxCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject auxPlayer;
    [SerializeField] private GameObject introAnimation;
    
    public int targetFPS = 60;

    private void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;

        if(skipIntro) {
            mainCamera.enabled = true;
            auxCamera.enabled = false;
            player.SetActive(true);
            auxPlayer.SetActive(false);
            introAnimation.SetActive(false);
        }

    }
}
