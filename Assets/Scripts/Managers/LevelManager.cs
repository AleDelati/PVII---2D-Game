using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    //              ----|Variables|----
    [Header("Level Configuration")]
    [SerializeField] private bool introSkip = false;
    [SerializeField] private bool easyMode = true;
    
    //              ----|References|----
    private AudioSource BackgroundMusic;
    private bool musicEnabled = true;

    //Referencias para Skipear la Intro
    [Header("Skip Intro Configuration")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject auxCamera;
    [SerializeField] private GameObject introAnimation;
    private GameObject player;
    private GameObject auxPlayer;

    private Inventory playerInventory;


    //              ----|Functions|----
    private void OnEnable() {
        BackgroundMusic = GetComponent<AudioSource>();

        GameEvents.onPause += Pause;
        GameEvents.onResume += Resume;

        player = GameObject.Find("Player");
        auxPlayer = GameObject.Find("PlayerAux");

        playerInventory = player.GetComponent<Inventory>();

        CheckIntroSkip();
        CheckEasyMode();
    }

    private void OnDisable() {
        GameEvents.onPause -= Pause;
        GameEvents.onResume -= Resume;
    }

    private void Pause() => BackgroundMusic.Pause();
    private void Resume() => BackgroundMusic.UnPause();

    private void Update() {
        CheckGameOver(player);
        CheckMusicState();
        CheckIntroSkipState();
    }

    private void CheckIntroSkip() {
        introSkip = PersistenceManager.Instance.GetBool("SkipIntroCinematics");
        if (introSkip) {
            mainCamera.SetActive(true);
            auxCamera.SetActive(false);
            player.SetActive(true);
            auxPlayer.SetActive(false);
            introAnimation.SetActive(false);
        } else {
            introAnimation.SetActive(true);
        }
    }

    private void CheckIntroSkipState() {
        introSkip = !PersistenceManager.Instance.GetBool("SkipIntroCinematics");
    }

    //Si el jugador fue eliminado reinicia el nivel
    private void CheckGameOver(GameObject _player) {
        if (_player == null) {
            ResetGame();
        }
    }

    //Reinicia el nivel
    public void ResetGame() {
        PersistenceManager.Instance.SaveHighScore(GameManager.instance.GetScore());
        PersistenceManager.Instance.SaveScore(GameManager.instance.GetScore());
        playerInventory.ResetItems();
        GameManager.instance.ResetScore();

        if (easyMode) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else {
            SceneManager.LoadScene(0);
        }
    }

    private void CheckMusicState() {
        musicEnabled = PersistenceManager.Instance.GetBool("Music");
        if (musicEnabled) {
            if(BackgroundMusic.isPlaying == false) {
                BackgroundMusic.Play();
            }
        } else {
            BackgroundMusic.Stop();
        }
    }

    private void CheckEasyMode() {
        easyMode = PersistenceManager.Instance.GetBool("EasyMode");
    }

}
